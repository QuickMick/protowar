using Newtonsoft.Json;
using Protobase;
using Protobase.entity;
using Protobase.manager;
using Protobase.resources;
using Protobase.util;
using Protowar.scenes.gameplay.entity;
using Protowar.scenes.gameplay.entity.attribute;
using Protowar.scenes.gameplay.entity.behaviour;
using Protowar.scenes.gameplay.hud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.scenecomponents
{
    public class GameplayComponent : ISceneComponent
    {
        private WorldManager worldManager;

        private ProtoCamera camera;
        public void Initialize(SceneContext context)
        {
            Console.WriteLine("init");

            this.worldManager = context.GetComponent<WorldManager>();
            this.camera = context.GetComponent<ProtoCamera>();

            Proto.Initialize(context.SceneResourceManager, this.worldManager);
            ProtowarLibrary.Initialize(context.SceneResourceManager, this.worldManager);
        }

        public void LoadContent(SceneContext context)
        {
            Console.WriteLine("load");
            context.SceneResourceManager.LoadContentFromFile();
   
        }

        public void OnStart(SceneContext ec, Scene previous = null)
        {
            Console.WriteLine("start");
            Random r = new Random();
            Entity test = null;
            //for (int i = 0; i < 5; i++)
            //{
                double x = r.Next(100);
                double y = r.Next(100);

                string concat = "{'x':" + x + ",'y':" + y + ", 'type':'test', 'rotation':5,'material':{'type':'SimpleTexturedMaterial','textureName':'table'},'mesh':{'type':'QuadMesh','width':12.0,'height':8.0},attributes:[{'type':'ActorData','speed':1,'rotationspeed':20,'speedmultiplicator':1.2,'maxspeed':0.005}],behaviours:['MovementInputHandlerBehaviour']}";
                EntityDefinition asd = JsonConvert.DeserializeObject<EntityDefinition>(concat);
                test = EntityFactory.Instance.Generate(asd);

                EntityFactory.Instance.GenerateByName("TEST_WEAPON", (float)x, (float)y+5);

                

            //}
            InputConfig ic = InputConfig.Instance;
                Crosshair ch = new Crosshair(
                    new Axis[]{ic.MOUSE,ic.RIGHT_AXIS},ic.LEFT_MOUSE,ic.RIGHT_MOUSE,
                    test, this.worldManager.World) { CursorSensitivity = 10, CursorRange = 50, CursorSize = 5, CursorImage = ec.SceneResourceManager.GetTexture("crosshair") };
                this.camera.TrackingBody = ch.Sensor;
            
            ec.Hud.Add(ch);
           // test = EntityFactory.Instance.Generate(asd);
        }

        public void Update(SceneContext e, Microsoft.Xna.Framework.GameTime t)
        {
          /*  ProtoCamera cam = e.GetComponent<ProtoCamera>();
            cam.Zoom = 2f;
            cam.EnableTracking = true;
            cam.EnableRotationTracking = true;
            cam.MinRotation = -0.05f;
            cam.MaxRotation = 0.05f;
            cam.SmoothSpeedMultiplicator = 50f;*/
        }

        public void Render(SceneContext e)
        {
        }

        public void RenderDebug(SceneContext e)
        {
            
        }

        public void OnPause(SceneContext e, Scene next = null)
        {
            Console.WriteLine("pause");
        }

        public void UnloadContent(Protobase.resources.IResourceManager scene)
        {
            Console.WriteLine("unload");
        }
    }


    public static class ProtowarLibrary
    {
        public const string BULLET_BODY = "BULLET_BODY";
        public static void Initialize(IResourceManager rm, WorldManager wm)
        {
            #region attributes
            EntityFactory.Instance.AddAttributeFactory("ActorData", (d) =>
            {
                return new ActorDataAttribute(d.GetFloat("speed"),d.GetFloat("rotationspeed"),d.GetFloat("maxspeed"),d.GetFloat("speedmultiplicator"));
            });

            EntityFactory.Instance.AddAttributeFactory("BulletDataAttribute", (d) =>
            {
                return new BulletDataAttribute()
                {
                    Damage = d.GetFloat("damage"),
                    BulletSpeed = d.GetFloat("bulletspeed"),
                    Restitution = d.GetFloat("restitution"),
                    SlugCount = d.GetInt("slugcount"),
                    BounceCount = d.GetInt("bouncecount")
                };
            });


            EntityFactory.Instance.AddAttributeFactory("WeaponDataAttribute", (d) =>
            {
                return new WeaponDataAttribute()
                {
                    Recoil = d.GetFloat("recoil"),
                    SlugCount = d.GetInt("slugcount"),
                    BounceCount = d.GetInt("bouncecount"),
                    MagazineSize = d.GetInt("magazinesize"),
                    Precission = d.GetFloat("precission"),
                    Condition = d.GetFloat("condition"),
                    BulletType = d.Get<string>("bullettype"),
                    MagazinLoad = d.GetInt("magazinload"),
                    Damage = d.GetInt("damage"),
                    FireRate = d.GetFloat("firerate"),
                    AttachPointX = d.GetFloat("attachpointx"),
                    AttachPointY = d.GetFloat("attachpointy")
                };
            });
  
            #endregion attributes

            #region behaviours
            EntityFactory.Instance.AddBehaviour<MovementInputHandlerBehaviour>("MovementInputHandlerBehaviour");
            EntityFactory.Instance.AddBehaviour<WeaponInputHandlingBehaviour>("WeaponInputHandlingBehaviour");
            #endregion behaviours


            AddPrefabList();
        }

        public static void AddPrefabList()
        {

          //  string bullet = "{'x':0,'y':0, 'type':'Bullet','rotation':0,'mesh':{'type':'CircleMesh','radius':3.0}}";

          //  EntityFactory.Instance.AddPrefab(BULLET_BODY, JsonConvert.DeserializeObject<EntityDefinition>(bullet));

            string test_weapon = "{"+ //'x':0,'y':0,"+
            "'Classes':['weapon'],'rotation':0,"+
            "'mesh':{'type':'QuadMesh','height':12,'width':2},"+
            "'attributes':[{"+
            "   'type':'WeaponDataAttribute',"+
            "   'recoil':'1',"+
            "   'slugcount':'2',"+
            "   'bouncecount':'2',"+
            "   'magazinesize':'32',"+
            "   'precission':'0.5',"+
            "   'condition':'0.5',"+
            "   'bullettype':'9mm',"+
            "   'magazinload':'32',"+
            "   'damage':'100',"+
            "   'firerate':'1',"+
            "   'attachpointx':'1'," +
            "   'attachpointy':'2'" +
            "}]," +
     //       "'behaviours':['WeaponHandlingBehaviour']," +
            "'material':{'type':'SimpleTexturedMaterial','textureName':'weapon'}" +
            "}";

            EntityFactory.Instance.AddPrefab("TEST_WEAPON", JsonConvert.DeserializeObject<EntityDefinition>(test_weapon));
        }
    }

    public static class EntityFlags{
        public const string IS_RUNNING = "IS_RUNNING";
        public const string IS_MOVING = "IS_MOVING";
        public const string STARTS_RUNNING = "STARTS_RUNNING";
        public const string STOPS_RUNNING = "STOPS_RUNNING";
        public const string STARTS_MOVING = "STARTS_MOVING";
        public const string STOPS_MOVING = "STOPS_MOVING";
    }


    //[Flags]
    public static class GameFlags
    {
        public const string INVENTORY_OPEN = "INVENTORY_OPEN";
        //  public const string PLAYER_INSIDE_CAR = "PLAYER_INSIDE_CAR";
    }

    public static class GameFlagMask
    {
        public static readonly string[] MENU_OPEN = new string[] { GameFlags.INVENTORY_OPEN };

        public static readonly string[] CROSSHAIR_HIDE_MASK = new string[] { GameFlags.INVENTORY_OPEN };

    }
}
