using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Protobase.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.entity
{
    public static class Categories
    {
        public const Category ACTOR = Category.Cat1;
        public const Category WALL = Category.Cat2;
        public const Category ITEM = Category.Cat3;
        public const Category WEAPON = Category.Cat4;
        public const Category BULLET = Category.Cat5;
        public const Category BLOOD = Category.Cat6;
        public const Category CAR = Category.Cat7;

        //TODO: vllt wieder rein
        public const Category ALL = Category.All;// &~ Categories.SENSOR; 


        public const Category SENSOR = Category.Cat10;
    }

    public abstract class EntityMesh
    {
        public Body Body { get; protected internal set; }

        public Entity Parent { get; private set; }

        protected WorldManager world;

       // public bool IsVisible { get; protected internal set; }

        public EntityMesh(WorldManager world)
        {
            this.world = world;
        }

        public void DebugRender(SceneContext g)
        {

        }

        public void Update(GameTime gt)
        {

        }

        public virtual void Construct(Entity e)
        {
            this.Parent = e;
        }

        public virtual void Destruct()
        {
            this.Parent = null;
        }

     /*   private void createAABB()
        {
            AABB aabb = new AABB(); 
            aabb.LowerBound=  new Vector2(float.MaxValue,float.MaxValue);
            aabb.UpperBound = new Vector2(float.MinValue,float.MinValue);

            //TODO: iwie die whidth un hight machn
            foreach(Fixture f in this.Parent.Mesh.Body.FixtureList){

           ///     aabb.Combine(ref x);

            //    aabb.Combine(ref f.GetAABB(());
           
            }
        }
        */
        public abstract float Width
        {
            get;
            protected set;
        }

        public abstract float Height
        {
            get;
            protected set;
        }
    }


    public class QuadMesh : EntityMesh
    {
        public const string JSON_WIDTH = "width";
        public const string JSON_HEIGHT = "height";

        public QuadMesh(WorldManager c, float width, float height,float x,float y,float r) 
            : base(c)
        {
            
            this.Width = width;
            this.Height = height;

            this.initX = x;
            this.initY = y;
            this.initRotation = r;
        }

        private float initX;
        private float initY;
        private float initRotation;

        public override void Construct(Entity e)
        {
            base.Construct(e);
            this.Body = BodyFactory.CreateRectangle(this.world.World, ConvertUnits.ToSimUnits(this.Width), ConvertUnits.ToSimUnits(this.Height), 1,new Vector2(ConvertUnits.ToSimUnits(this.initX), ConvertUnits.ToSimUnits(this.initY)), this.Parent);
          //  this.Body.SetTransform(new Vector2(ConvertUnits.ToSimUnits(this.initX), ConvertUnits.ToSimUnits(this.initY)), this.initRotation);
            this.Body.Rotation = this.initRotation;
            this.Body.UserData = this.Parent;
            this.Body.BodyType = BodyType.Dynamic;

            this.Body.CollisionCategories = Categories.ACTOR;
            this.Body.CollidesWith = Categories.ALL & ~Categories.ITEM;

            this.Body.Friction = 0.5f;
            this.Body.LinearDamping = 3;
            this.Body.AngularDamping = 3;
        }

        public override void Destruct()
        {
            this.Body.UserData = null;
            this.world.RemoveBody(this.Body);
            base.Destruct();
        }

        public override float Width
        {
            get;
            protected set;
        }

        public override float Height
        {
            get;
            protected set;
        }
    }
    
    public class CircleMesh : EntityMesh
    {
        public const string JSON_RADIUS = "radius";

        public CircleMesh(WorldManager c, float radius, float x, float y, float rotation = 0)
            : base(c)
        {
            this.Width = this.Height = this.Radius = radius * 2;

            this.initX = x;
            this.initY = y;
            this.initRotation = rotation;
        }

        private float initX;
        private float initY;
        private float initRotation;

        public override void Construct(Entity e)
        {
            base.Construct(e);
            float density = 2;
            this.Body = BodyFactory.CreateCircle(this.world.World, this.Radius, density, new Vector2(ConvertUnits.ToSimUnits(this.initX), ConvertUnits.ToSimUnits(this.initY)), this.Parent);

            //this.Body.SetTransform(new Vector2(ConvertUnits.ToSimUnits(this.initX), ConvertUnits.ToSimUnits(this.initY)), this.initRotation);
            this.Body.Rotation = this.initRotation;
            this.Body.UserData = this.Parent;
            this.Body.BodyType = BodyType.Dynamic;

            this.Body.CollisionCategories = Categories.ACTOR;
            this.Body.CollidesWith = Categories.ALL & ~Categories.ITEM;

            this.Body.Friction = 0.5f;
            this.Body.LinearDamping = 3;
            this.Body.AngularDamping = 3;

        }

        public override void Destruct()
        {
            this.Body.UserData = null;
            this.world.RemoveBody(this.Body);
            base.Destruct();
        }

        public float Radius
        {
            get;
            set;
        }

        public override float Width
        {
            get;
            protected set;
        }

        public override float Height
        {
            get;
            protected set;
        }
    }
}
