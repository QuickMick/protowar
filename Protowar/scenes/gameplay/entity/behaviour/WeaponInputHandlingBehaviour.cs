using FarseerPhysics;
using Microsoft.Xna.Framework;
using Protobase.entity;
using Protobase.manager;
using Protobase.util;
using Protowar.scenes.gameplay.entity.attribute;
using Protowar.scenes.gameplay.scenecomponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity.behaviour
{
    public class WeaponInputHandlingBehaviour : EntityBehaviour
    {
        public const string EQUIPED_IN_LEFT_SLOT = "EQUIPED_IN_LEFT_SLOT";
        public const string EQUIPED_IN_RIGHT_SLOT = "EQUIPED_IN_RIGHT_SLOT";

        //sicherheitsabstand, dass die eigene waffe nicht getroffen wird
        public const float WEAPON_SHOT_OFFSET = 1f;

        private InputAction shootAction;

        private WeaponDataAttribute weaponData;

        private bool isBlocked=false;

        public override void Construct(Entity e)
        {
            this.weaponData = e.GetAttribute<WeaponDataAttribute>();   
        //    key holen
        }

        public override void Update(SceneContext c, Entity e, GameTime gt)
        {
            if (this.shootAction.IsDown)
            {
                this.Shoot(c,e);
            }
        }

        public override void Destruct(Entity e)
        {
        }



        //public override void AddToWorld(Context c)
        //{
        //    if (this.IsAddedToWorld)
        //    {
        //        this.RemoveFromWorld(c);
        //    }

        //    this.Body = BodyFactory.CreateRectangle(c.EntityManager.World, ConvertUnits.ToSimUnits(this.Width), ConvertUnits.ToSimUnits(this.Height), 0.001f, this);
        //    this.Body.UserData = this;
        //    this.Body.BodyType = BodyType.Dynamic;
        //    this.Body.CollidesWith = Categories.BULLET;
        //    this.Body.CollisionCategories = Categories.WEAPON;

        //    this.IsAddedToWorld = true;
        //}


        public bool Shoot(SceneContext c, Entity e)
        {
            if (this.isBlocked)
            {
                return false;
            }

            BulletType bulletType = StaticsLibrary.Instance.GetBullet(this.weaponData.BulletType);

            //TODO: schaun dass da alles passt mit sim units un displayunits
            float offset = e.Mesh.Height / 2 - this.weaponData.AttachPointY + WeaponInputHandlingBehaviour.WEAPON_SHOT_OFFSET + bulletType.Radius;
            Vector2 dir = VectorMath.RotateAroundOrigin(new Vector2(0, 1), e.Mesh.Body.Rotation, false);
            Vector2 position = e.Transform.Position + dir * new Vector2(offset);
            


            EntityDefinition bulletDefinition= new EntityDefinition();
            bulletDefinition.X = position.X;
            bulletDefinition.Y = position.Y;
            bulletDefinition.Mesh = new Dictionary<string,object>();
            bulletDefinition.Mesh.Add("type","CircleMesh");
            bulletDefinition.Mesh.Add("radius", bulletType.Radius);

            //TODO: die werte müssn aus den standartwerden des kugeltyps und aus der waffe generiert werden
            float bulletSpeed = 0.1f;
            Dictionary<string,object> bulletData = new Dictionary<string,object>();
            bulletData.Add("damage",1);
            bulletData.Add("bulletspeed", bulletSpeed);
            bulletData.Add("restitution",1);
            bulletData.Add("slugcount",1);
            bulletData.Add("bouncecount",1);

            bulletDefinition.Attributes = new Dictionary<string, object>[] { bulletData };
       

            Entity bullet = EntityFactory.Instance.Generate(bulletDefinition);


            // Kugel schiessen
            bullet.Mesh.Body.ApplyLinearImpulse(dir * ConvertUnits.ToSimUnits(bulletSpeed));

            // recoil auf das crosshair
            //  Game1.crosshair.Sensor.ApplyForce(-dir * ConvertUnits.ToSimUnits(this.WeaponDef.Recoil));

            // recoil der waffe auf player
            e.Mesh.Body.ApplyForce(-dir * ConvertUnits.ToSimUnits(this.weaponData.Recoil));

            this.isBlocked = true;

            Timer t = new Timer() {
                IsLooping = false,
                TimeInverval = 1000 * this.weaponData.FireRate
            };

            t.OnTimerFinish += (x) =>
            {
                this.isBlocked = false; // waffe nach cooldown wieder freischalten
            };

            t.Start();

         ///   e.AddFlag(FLAGS.SHOT_RIGHT);
         ///   TODO: animation abspieln --> vllt als animations entity?
            return true;
        }
    }
}
