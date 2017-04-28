//using FarseerPhysics;
//using FarseerPhysics.DebugView;
//using FarseerPhysics.Dynamics;
//using FarseerPhysics.Factories;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using Protobase.entity;
//using Protobase.entity.materials;
//using Protobase.manager;
//using Protobase.resources;
//using Protobase.util;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Dynamic;
//using System.IO;
//using System.Resources;

//namespace Protobase
//{
//    /// <summary>
//    /// This is the main type for your game.
//    /// </summary>
//    public class Game1 : Game
//    {
//        public const float METER = 4f;
//        float scale = 4f;

//        GraphicsDeviceManager graphics;
//        SpriteBatch spriteBatch;

//        DebugViewXNA debug;

//        World World;

//        public Game1()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//        }

//        /// <summary>
//        /// Allows the game to perform any initialization it needs to before starting to run.
//        /// This is where it can query for any required services and load any non-graphic
//        /// related content.  Calling base.Initialize will enumerate through any components
//        /// and initialize them as well.
//        /// </summary>
//        protected override void Initialize()
//        {
//            base.Initialize();
//        }



//        /// <summary>
//        /// LoadContent will be called once per game and is the place to load
//        /// all of your content.
//        /// </summary>
//        protected override void LoadContent()
//        {
//            // Create a new SpriteBatch, which can be used to draw textures.
//            spriteBatch = new SpriteBatch(GraphicsDevice);




//                  this.World = new World(Vector2.Zero);

//            debug = new DebugViewXNA(this.World);
//            debug.LoadContent(this.graphics.GraphicsDevice, this.Content);



//            debug.DefaultShapeColor = Color.White;
//            debug.SleepingShapeColor = Color.LightGray;
            

//            debug.AppendFlags(DebugViewFlags.PerformanceGraph);

//            debug.AppendFlags(DebugViewFlags.DebugPanel);

//            debug.AppendFlags(DebugViewFlags.ContactPoints);
//            debug.AppendFlags(DebugViewFlags.ContactNormals);


//            debug.AppendFlags(DebugViewFlags.PolygonPoints);


//            debug.AppendFlags(DebugViewFlags.Controllers);
//            debug.AppendFlags(DebugViewFlags.CenterOfMass);
//            debug.AppendFlags(DebugViewFlags.AABB);

//            debug.RemoveFlags(DebugViewFlags.Joint);


//            /*

//            string concat = "{'x':10,'y':10, 'type':'test', 'rotation':5,'material':{'type':'SimpleTexturedMaterial','textureName':'table'},'mesh':{'type':'QuadMesh','width':10.0,'height':10.0}, 'children':[{'x':0,'y':-20, 'type':'test', 'rotation':50,'material':{'type':'SimpleTexturedMaterial','textureName':'table'},'mesh':{'type':'QuadMesh','width':10.0,'height':10.0}}]}";
//            EntityDefinition asd = JsonConvert.DeserializeObject<EntityDefinition>(concat);
//            Entity test = EntityFactory.Instance.Generate(asd);
//            test.Mesh.Body.ApplyLinearImpulse(new Vector2(0.03f, 0.03f));*/





//            Body body;
//            float density = 2;
//           body = BodyFactory.CreateRectangle(this.World, ConvertUnits.ToSimUnits(10), ConvertUnits.ToSimUnits(100), 1,new Vector2(0,0));
         
//            //this.Body.SetTransform(new Vector2(ConvertUnits.ToSimUnits(this.initX), ConvertUnits.ToSimUnits(this.initY)), this.initRotation);
//            body.Rotation = 5;
//            body.BodyType = BodyType.Dynamic;

//            body.CollisionCategories = Categories.ACTOR;
//           body.CollidesWith = Categories.ALL & ~Categories.ITEM;

//           body.Friction = 0.5f;
//            body.LinearDamping = 3;
//           body.AngularDamping = 3;


//           body = BodyFactory.CreateRectangle(this.World, ConvertUnits.ToSimUnits(10), ConvertUnits.ToSimUnits(100), 1, new Vector2(0, 0));

//           //this.Body.SetTransform(new Vector2(ConvertUnits.ToSimUnits(this.initX), ConvertUnits.ToSimUnits(this.initY)), this.initRotation);
//           body.Rotation = 5;
//           body.BodyType = BodyType.Dynamic;

//           body.CollisionCategories = Categories.ACTOR;
//           body.CollidesWith = Categories.ALL & ~Categories.ITEM;

//           body.Friction = 0.5f;
//           body.LinearDamping = 3;
//           body.AngularDamping = 3;


//        }

//        /// <summary>
//        /// UnloadContent will be called once per game and is the place to unload
//        /// game-specific content.
//        /// </summary>
//        protected override void UnloadContent()
//        {
//            // TODO: Unload any non ContentManager content here
//        }

//        /// <summary>
//        /// Allows the game to run logic such as updating the world,
//        /// checking for collisions, gathering input, and playing audio.
//        /// </summary>
//        /// <param name="gameTime">Provides a snapshot of timing values.</param>
//        protected override void Update(GameTime gameTime)
//        {
//         //   this.camera.Position = new Vector2(this.camera.Position.X-4f, this.camera.Position.Y);


//            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
//                Exit();


//            base.Update(gameTime);
//        }

//        /// <summary>
//        /// This is called when the game should draw itself.
//        /// </summary>
//        /// <param name="gameTime">Provides a snapshot of timing values.</param>
//        protected override void Draw(GameTime gameTime)
//        {
//            GraphicsDevice.Clear(Color.CornflowerBlue);
//            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

//            /*

//            GraphicsDevice.Clear(Color.CornflowerBlue);
//            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

//             spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.View);



//            spriteBatch.End();
//            */
//            //this.debug.RenderDebugData(Matrix.Create.Identity, Matrix.Identity);

//            base.Draw(gameTime);
//        }
//    }
//}











////using Microsoft.Xna.Framework;
////using Microsoft.Xna.Framework.Graphics;
////using Microsoft.Xna.Framework.Input;
////using Newtonsoft.Json;
////using Newtonsoft.Json.Converters;
////using Protobase.entity;
////using Protobase.entity.materials;
////using Protobase.manager;
////using Protobase.resources;
////using Protobase.util;
////using System;
////using System.Collections.Generic;
////using System.Diagnostics;
////using System.Dynamic;
////using System.IO;
////using System.Resources;

////namespace Protobase
////{
////    /// <summary>
////    /// This is the main type for your game.
////    /// </summary>
////    public class Game1 : Game
////    {
////        public const float METER = 4f;
////        float scale = 4f;

////        GraphicsDeviceManager graphics;
////        SpriteBatch spriteBatch;

////        Camera2D camera;

////        private WorldManager wm;

////        SceneContext context;

////        public Game1()
////        {
////            graphics = new GraphicsDeviceManager(this);
////            Content.RootDirectory = "Content";
////        }

////        /// <summary>
////        /// Allows the game to perform any initialization it needs to before starting to run.
////        /// This is where it can query for any required services and load any non-graphic
////        /// related content.  Calling base.Initialize will enumerate through any components
////        /// and initialize them as well.
////        /// </summary>
////        protected override void Initialize()
////        {
////            base.Initialize();
////        }



////        /// <summary>
////        /// LoadContent will be called once per game and is the place to load
////        /// all of your content.
////        /// </summary>
////        protected override void LoadContent()
////        {
////            // Create a new SpriteBatch, which can be used to draw textures.
////            spriteBatch = new SpriteBatch(GraphicsDevice);

////            Dictionary<Type, ISceneComponent> componentsDictionary = new Dictionary<Type, ISceneComponent>();
////            this.camera = new Camera2D() { Zoom = scale, EnableTracking = true, EnableRotationTracking = true, MinRotation = -0.05f, MaxRotation = 0.05f, SmoothSpeedMultiplicator = 50f };

////            componentsDictionary.Add(this.camera.GetType(), this.camera);

////            this.context = new SceneContext(componentsDictionary, this.spriteBatch, this.graphics, null, this.Services, new InputManager(PlayerIndex.One), null, new HudManager());

////            ((PWResourceManager)this.context.SceneResourceManager).Create();





////            this.wm = new WorldManager();
////            this.wm.Initialize(this.context);
////            context.SceneResourceManager.LoadContentFromFile();
////            Proto.Initialize(context.SceneResourceManager, this.wm);


////            this.wm.LoadContent(this.context);
////            //this.sceneManager.LoadContent(this.spriteBatch, this.graphics, this.Content);




////            string concat = "{'x':10,'y':10, 'type':'test', 'rotation':5,'material':{'type':'SimpleTexturedMaterial','textureName':'table'},'mesh':{'type':'QuadMesh','width':10.0,'height':10.0}, 'children':[{'x':0,'y':-20, 'type':'test', 'rotation':50,'material':{'type':'SimpleTexturedMaterial','textureName':'table'},'mesh':{'type':'QuadMesh','width':10.0,'height':10.0}}]}";
////            EntityDefinition asd = JsonConvert.DeserializeObject<EntityDefinition>(concat);
////            Entity test = EntityFactory.Instance.Generate(asd);
////            test.Mesh.Body.ApplyLinearImpulse(new Vector2(0.03f, 0.03f));

////        }

////        /// <summary>
////        /// UnloadContent will be called once per game and is the place to unload
////        /// game-specific content.
////        /// </summary>
////        protected override void UnloadContent()
////        {
////            // TODO: Unload any non ContentManager content here
////        }

////        /// <summary>
////        /// Allows the game to run logic such as updating the world,
////        /// checking for collisions, gathering input, and playing audio.
////        /// </summary>
////        /// <param name="gameTime">Provides a snapshot of timing values.</param>
////        protected override void Update(GameTime gameTime)
////        {
////         //   this.camera.Position = new Vector2(this.camera.Position.X-4f, this.camera.Position.Y);

////            this.camera.Update(this.context, gameTime);
////            this.wm.Update(this.context, gameTime);
////            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
////                Exit();


////            base.Update(gameTime);
////        }

////        /// <summary>
////        /// This is called when the game should draw itself.
////        /// </summary>
////        /// <param name="gameTime">Provides a snapshot of timing values.</param>
////        protected override void Draw(GameTime gameTime)
////        {
////            GraphicsDevice.Clear(Color.CornflowerBlue);
////            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

////            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.View);
////            this.wm.Render(this.context);
////            spriteBatch.End();


////            this.wm.RenderDebug(this.context);

////            base.Draw(gameTime);
////        }
////    }
////}















////using Microsoft.Xna.Framework;
////using Microsoft.Xna.Framework.Graphics;
////using Microsoft.Xna.Framework.Input;
////using Newtonsoft.Json;
////using Newtonsoft.Json.Converters;
////using Protobase.entity;
////using Protobase.entity.materials;
////using Protobase.manager;
////using Protobase.resources;
////using Protobase.util;
////using System;
////using System.Diagnostics;
////using System.Dynamic;
////using System.IO;

////namespace Protobase
////{
////    /// <summary>
////    /// This is the main type for your game.
////    /// </summary>
////    public class Game1 : Game
////    {
////        public const float METER = 4f;
////        float scale = 5f;

////        GraphicsDeviceManager graphics;
////        SpriteBatch spriteBatch;


////        private InputManager im;
////        private WorldManager entityManager;

////        private Camera2D cam;

////     //   PWResourceManager rm = new PWResourceManager();


////        public Game1()
////        {
////            graphics = new GraphicsDeviceManager(this);
////            Content.RootDirectory = "Content";
////        }

////        /// <summary>
////        /// Allows the game to perform any initialization it needs to before starting to run.
////        /// This is where it can query for any required services and load any non-graphic
////        /// related content.  Calling base.Initialize will enumerate through any components
////        /// and initialize them as well.
////        /// </summary>
////        protected override void Initialize()
////        {
////            im = new InputManager(PlayerIndex.One);
////            InputConfig.Instance.Init(im);


////            this.cam = new Camera2D(this.graphics.GraphicsDevice);
////            this.cam.Zoom = scale;
////            this.cam.EnableTracking = true;
////            this.cam.EnableRotationTracking = true;
////            this.cam.MinRotation = -0.05f;
////            this.cam.MaxRotation = 0.05f;
////            this.cam.SmoothSpeedMultiplicator = 50f;

////            base.Initialize();
////        }

////        /// <summary>
////        /// LoadContent will be called once per game and is the place to load
////        /// all of your content.
////        /// </summary>
////        protected override void LoadContent()
////        {
////            // Create a new SpriteBatch, which can be used to draw textures.
////            spriteBatch = new SpriteBatch(GraphicsDevice);
////            PWResourceManager.Instance.LoadContentFromFile(this.Content);

////            this.entityManager = new WorldManager();


////            Proto.Initialize(this.entityManager);

////            Stopwatch stopwatch = new Stopwatch();


////            /*   
////           */
////            /*

////            string json = "{'X':0,'Y':0,'Rotation':0,'Material':{'Type':'SimpleTexturedMaterial','textureName':'table'},'Mesh':{'Type':'QuadMesh','Width':10.0,'Height':10.0}}";
////            string json2 = "{'X':0,'Y':20,'Rotation':45,'Material':{'Type':'SimpleTexturedMaterial','textureName':'table'},'Mesh':{'Type':'QuadMesh','Width':10.0,'Height':10.0}}";

////            */
////            string concat = "{'x':0,'y':0, 'type':'test', 'rotation':5,'material':{'type':'SimpleTexturedMaterial','textureName':'table'},'mesh':{'type':'QuadMesh','width':10.0,'height':10.0}, 'children':[{'x':0,'y':-20, 'type':'test', 'rotation':50,'material':{'type':'SimpleTexturedMaterial','textureName':'table'},'mesh':{'type':'QuadMesh','width':10.0,'height':10.0}}]}";





////          //  var j = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
////            EntityDefinition asd = JsonConvert.DeserializeObject<EntityDefinition>(concat);

////            Entity test;

////            /*  stopwatch.Start();
////              test = EntityFactory.Instance.GenerateDynamical(j);
////              stopwatch.Stop();
////              Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);


////                  stopwatch.Start();
////                  test = new Entity();
////                  test.Mesh = new QuadMesh(this.context.EntityManager, 10, 10,40,40,45);
////                  test.Material = new SimpleTexturedMaterial("table");
////                  stopwatch.Stop();
////                  Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
////                 */

////            stopwatch.Start();

////                     test = EntityFactory.Instance.Generate(asd);
////                     stopwatch.Stop();
////                     Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);





////            //JObject.Parse(

////           // var earth = Content.Load<Texture2D>("earth");
////            // TODO: use this.Content to load your game content here
////        }

////        /// <summary>
////        /// UnloadContent will be called once per game and is the place to unload
////        /// game-specific content.
////        /// </summary>
////        protected override void UnloadContent()
////        {
////            // TODO: Unload any non ContentManager content here
////        }

////        /// <summary>
////        /// Allows the game to run logic such as updating the world,
////        /// checking for collisions, gathering input, and playing audio.
////        /// </summary>
////        /// <param name="gameTime">Provides a snapshot of timing values.</param>
////        protected override void Update(GameTime gameTime)
////        {
////            Timer.UpdateAll(gameTime);
////            im.Update(gameTime, this.context);
////            this.entityManager.Update(this.context, gameTime);
////            this.cam.Update(gameTime);

////            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
////                Exit();


////            base.Update(gameTime);
////        }

////        /// <summary>
////        /// This is called when the game should draw itself.
////        /// </summary>
////        /// <param name="gameTime">Provides a snapshot of timing values.</param>
////        protected override void Draw(GameTime gameTime)
////        {
////            GraphicsDevice.Clear(Color.CornflowerBlue);

////            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
////            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cam.View);

////            this.entityManager.Render(this.context);

////            this.spriteBatch.End();

////            /*
////            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
////            this.hudManager.RenderAbsolute(this.context, gameTime);
////            this.spriteBatch.End();*/

////            this.entityManager.RenderDebug(this.context);
////            base.Draw(gameTime);
////        }
////    }
////}
