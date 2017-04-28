using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Protobase.resources;
using Protobase.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.manager
{
    public interface ISceneController
    {
        void StartScene(string s);
        Scene GetScene(string name);
        void Stop(string exitCode);
    }

    public class SceneManager : Game, ISceneController
    {
        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

        private Scene currentScene = null;

        private string nextSceneName = "";
        private string currentSceneName = "";

        private IResourceManager sharedResources;

        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;

    

        public SceneManager(int width,int height)
        {
            this.graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            this.graphicsDeviceManager.PreferredBackBufferHeight = height;
            this.graphicsDeviceManager.PreferredBackBufferWidth = width;
        }

        public Scene GetScene(string name)
        {
            return this.scenes[name];
        }

        public void AddScene(Scene s)
        {
            if (this.initialized)
            {
                throw new Exception("Already initialized!");
            }
            this.scenes.Add(s.SceneName, s);
        }

        private bool initialized = false;

        public void Run(string startScreenName)
        {

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
           
            this.sharedResources = new PWResourceManager(this.Content);

            foreach (Scene s in this.scenes.Values)
            {
                s.LoadSharedContent(this.sharedResources);
            }

            foreach (Scene s in this.scenes.Values)
            {
                s.Initialize(this.spriteBatch, this.graphicsDeviceManager, this.sharedResources, this.Services, this);
            }

            this.initialized = true;
        }

        //protected override void Initialize()
        //{
 //TODO: hier vllt teile von load content rein
        //    base.Initialize();
        //}




        public void StartScene(string s)
        {
            this.nextSceneName = s;
        }

        private void changeScene(string s)
        {
            if (!this.initialized)
                throw new Exception("Scenemanager not initialized!");

            Scene next = null;

            if (this.scenes.ContainsKey(s))
            {
                next = this.scenes[s];
            }
            if (this.currentScene != null)
            {
                this.currentScene.Pause(next);
            }

            if (next != null)
            {
                next.Start();
            }

            this.currentScene = next;
        }

        protected override void Update(GameTime gameTime)
        {
            Timer.UpdateAll(gameTime);

            if (this.currentSceneName != this.nextSceneName)
            {
                this.changeScene(this.nextSceneName);
                this.currentSceneName = this.nextSceneName;
            }

            if (this.currentScene == null)
                return;


            this.currentScene.Update(gameTime);

#if DEBUG
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (this.currentScene == null)
                return;

            this.currentScene.Render();

            base.Draw(gameTime);
        }

        public void Stop(string exitCode)
        {
            Console.WriteLine(exitCode);
            foreach (Scene s in this.scenes.Values)
            {
                if (this.currentScene != null)
                {
                    this.currentScene.Pause();
                }

                s.UnloadContent();
            }

            
        }
    }
}
























//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Protobase.resources;
//using Protobase.util;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Protobase.manager
//{
//    public delegate void LoadSharedContentEventHandler(IResourceManager rm);

//    public interface ISceneController
//    {
//        void StartScene(string s);
//        Scene GetScene(string name);
//        void Stop(string exitCode);
//    }

//    public class SceneManager : ISceneController
//    {
//        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
//        private Scene currentScene = null;

//        private IResourceManager sharedResources;

//        public event LoadSharedContentEventHandler LoadSharedContent;

//        public SceneManager()
//        {

//        }

//        public Scene GetScene(string name)
//        {
//            return this.scenes[name];
//        }

//        public void AddScene(Scene s)
//        {
//            if (this.initialized)
//            {
//                throw new Exception("Already initialized!");
//            }
//            this.scenes.Add(s.SceneName, s);
//        }

//        private bool initialized = false;


//        public void Initialize(SpriteBatch sb, GraphicsDeviceManager gdm, ContentManager cm,IServiceProvider sp)
//        {
//            if (!this.initialized)
//            {
//                throw new Exception("not initialized!");
//            }
            
//            this.sharedResources = new PWResourceManager(cm);

//            if (LoadSharedContent != null)
//            {
//                this.LoadSharedContent(this.sharedResources);
//            }

//            foreach (Scene s in this.scenes.Values)
//            {
//                s.Initialize(sb, gdm, this.sharedResources, sp,this);
//            }

//            this.initialized = true;
//        }

//        public void StartScene(string s)
//        {
//            if (!this.initialized)
//                throw new Exception("Scenemanager not initialized!");

//            Scene next = null;
            
//            if(this.scenes.ContainsKey(s)){
//                next = this.scenes[s];
//            }
//            if (this.currentScene != null)
//            {
//                this.currentScene.Pause(next);
//            }

//            if (next != null)
//            {
//                next.Start();
//            }

//            this.currentScene = next;
//        }

//        public void Render()
//        {
//            if (this.currentScene == null)
//                return;
//            this.currentScene.Render();
//        }

//        public void Update(GameTime gameTime)
//        {
//            Timer.UpdateAll(gameTime);
//            if (this.currentScene == null)
//                return;
//            this.currentScene.Update(gameTime);
//        }

//        public void Stop(string exitCode)
//        {
//            Console.WriteLine(exitCode);
//            foreach (Scene s in this.scenes.Values)
//            {
//                if (this.currentScene != null)
//                {
//                    this.currentScene.Pause();
//                }

//                s.UnloadContent();
//            }

            
//        }
//    }
//}
