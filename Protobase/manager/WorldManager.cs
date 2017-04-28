using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Protobase.entity;
using Protobase.resources;
using Protobase.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.manager
{
    public class WorldManager : ISceneComponent
    {
#if DEBUG
        private DebugViewXNA debug;
#endif
        public const float ENTITY_RANGE_X = 200;
        public const float ENTITY_RANGE_Y = 200;

        public const float DROPS_SPEED = 2f;
        public const float DROP_ANGLE = 45f;

        //   public Camera2 Camera { get; set; }

        private List<Body> toRemove = new List<Body>();

        public bool IsEnabled { get; set; }

        public float WorldStep { get; private set; }

        public List<Entity> EntitiesInRange { get; private set; }

        public World World { get; private set; }

        public IGameMap Map { get; set; }

        private ProtoCamera camera;

        //TODO: debugview in eiegene componente auslagern

        public WorldManager()
        {            
            this.WorldStep = 30f;

            this.IsEnabled = true;
            this.EntitiesInRange = new List<Entity>();
            this.World = new World(Vector2.Zero);


#if DEBUG
            debug = new DebugViewXNA(this.World);
            debug.DefaultShapeColor = Color.White;
            debug.SleepingShapeColor = Color.LightGray;
            

        //    debug.AppendFlags(DebugViewFlags.PerformanceGraph);

        //    debug.AppendFlags(DebugViewFlags.DebugPanel);

            debug.AppendFlags(DebugViewFlags.ContactPoints);
       //     debug.AppendFlags(DebugViewFlags.ContactNormals);


      //      debug.AppendFlags(DebugViewFlags.PolygonPoints);


            debug.AppendFlags(DebugViewFlags.Controllers);
            debug.AppendFlags(DebugViewFlags.CenterOfMass);
            debug.AppendFlags(DebugViewFlags.AABB);

            debug.RemoveFlags(DebugViewFlags.Joint);
#endif
        }

        public void Initialize(SceneContext context)
        {
            this.camera = context.GetComponent<ProtoCamera>();
        }

        public void OnStart(SceneContext ec, Scene previous = null)
        {
         //TODO:
        }

        public void OnPause(SceneContext e, Scene next = null)
        {
            //TODO:
        }

       /* public void DropItem(EntityContext c, Vector2 pos, float angel, Item def)
        {
            def.AddToWorld(c);       
            def.Body.Position = pos;

            Random r = new Random();

            angel -= MathHelper.ToRadians(DROP_ANGLE/2);
            angel += (float)r.NextDouble() * MathHelper.ToRadians(DROP_ANGLE);
                      
            Vector2 impulse = VectorMath.RotateAroundOrigin(new Vector2(0, 1), angel, false);
            impulse *= new Vector2(DROPS_SPEED * (float)r.NextDouble());
            def.Body.ApplyForce(ConvertUnits.ToSimUnits(impulse));
        }*/

        public void Update(SceneContext c, GameTime gameTime)
        {
            this.EntitiesInRange.Clear();
            if (!this.IsEnabled)
            {
                return;
            }

            AABB aabb = new AABB(ConvertUnits.ToSimUnits(this.camera.Position),
                                ConvertUnits.ToSimUnits(ENTITY_RANGE_X),
                                ConvertUnits.ToSimUnits(ENTITY_RANGE_Y));

            this.World.QueryAABB(
                (f)=>{
                    Entity e = f.Body.UserData as Entity;
                    if(e != null){
                        this.EntitiesInRange.Add(e);
                    }
                    return true;
                },
                ref aabb);
            
            foreach (Entity e in this.EntitiesInRange)
            {
                // this.updateChildren(e, gc, gameTime);
                e.Update(c,gameTime);
               // e.FinishUpdating(c);
            }

            foreach (Entity e in this.EntitiesInRange)
            {
                // this.updateChildren(e, gc, gameTime);
                e.ClearFlags();
                // e.FinishUpdating(c);
            }

            this.World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / this.WorldStep)));

            if(this.Map != null)
                this.Map.Update(c, gameTime);

            if (this.toRemove.Count > 0)
            {
                foreach (Body b in this.toRemove)
                {
                    this.World.RemoveBody(b);
                }

                this.toRemove.Clear();
            }

        }



        public void Render(SceneContext c)
        {
            if (this.Map != null)
                this.Map.RenderGround(c);
            foreach (Entity e in this.EntitiesInRange)
            {
                e.Render(c);
            }
            if (this.Map != null)
                this.Map.RenderTop(c);
        }


        public void RenderDebug(SceneContext c)
        {
#if DEBUG
            this.debug.RenderDebugData(this.camera.SimProjection, this.camera.SimView);
#endif
        }

        public void RemoveBody(Body body)
        {
            this.toRemove.Add(body);
        }


        public void LoadContent(SceneContext c)
        {
            debug.LoadContent(c.GraphicsDeviceManager.GraphicsDevice, c.SceneResourceManager.Content);
        }

        public void UnloadContent(resources.IResourceManager scene)
        {
          
        }

    
    }

    
}
