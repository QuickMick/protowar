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
using System.Threading.Tasks;

namespace Protobase.manager
{

   // public delegate void InitSceneEventHandler(Scene s);

    /*

    public interface IScene{
        SceneContext Context { get; }
        string SceneName { get; }

        void AddComponent(ISceneComponent sc);

        event InitContextEventHandler InitContext;
    }
    */

    public abstract class Scene
    {
        public SceneContext Context { get; private set; }

        private List<ISceneComponent> compontentsOrder = new List<ISceneComponent>();
        private Dictionary<Type,ISceneComponent> componentsDictionary = new Dictionary<Type,ISceneComponent>();

        public string SceneName { get; private set; }

        private SpriteBatch hudSpritebatch;

        // hier drin sollen alle componenten hinzugefügt werden
      //  public event InitSceneEventHandler InitScene;

        private bool reloadContent = true;
        public bool ReloadContent { get { return reloadContent; } set { reloadContent = value; } }

        public Scene(string name)
        {
            this.SceneName = name;
        }

        public void AddComponent(ISceneComponent gc)
        {
            this.compontentsOrder.Add(gc);
            this.componentsDictionary.Add(gc.GetType(), gc);
        }

        public void Initialize(SpriteBatch sb, GraphicsDeviceManager gdm, IResourceManager shared, IServiceProvider sp, ISceneController sc)
        {
            this.hudSpritebatch = new SpriteBatch(gdm.GraphicsDevice);
            this.Context = new SceneContext(this.componentsDictionary, sb, gdm, shared, sp, new InputManager(PlayerIndex.One),sc, new HudManager());

            this.BuildScene();

            //initialize components
            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.Initialize(this.Context);
            }
        }
        /*
        public void LoadContent()
        {
            //initialize content

            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.LoadContent(this.Context);
            }
        }*/

        public void Start(Scene previous = null)
        {
            //initialize content
            if (!this.Context.SceneResourceManager.ContentLoaded || this.reloadContent)
            {
                ((PWResourceManager)this.Context.SceneResourceManager).Create();

                foreach (ISceneComponent gc in this.compontentsOrder)
                {
                    gc.LoadContent(this.Context);
                }
            }

            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.OnStart(this.Context, previous);
            }
        }

        public void Render()
        {
            this.Context.GraphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.OpenSpritebatch(this.Context);

            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.Render(this.Context);
            }

            this.Context.Hud.RenderInGameLoop(this.Context);

            this.Context.SpriteBatch.End();

#if DEBUG
            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.RenderDebug(this.Context);
            }
#endif

            this.OpenHudSpritebatch(this.Context);
            this.Context.Hud.Render(this.Context);
            this.Context.SpriteBatch.End();
        }

        public void Update(GameTime gt)
        {
            this.Context.InputManager.Update(gt, this.Context);
            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.Update(this.Context,gt);
            }

            this.Context.Hud.Update(this.Context, gt);
        }

        public void Pause(Scene next = null)
        {
            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.OnPause(this.Context, next);
            }

            if (this.reloadContent && this.Context.SceneResourceManager.ContentLoaded)
            {
                this.UnloadContent();
            }
        }
               
        public void UnloadContent()
        {
            foreach (ISceneComponent gc in this.compontentsOrder)
            {
                gc.UnloadContent(this.Context.SceneResourceManager);
            }

            ((PWResourceManager)this.Context.SceneResourceManager).UnloadContent();
        }


        public abstract void LoadSharedContent(IResourceManager shared);
        public abstract void BuildScene();

        public abstract void OpenSpritebatch(SceneContext context);
        public abstract void OpenHudSpritebatch(SceneContext context);
    }

    public class SceneContext
    {
        public static Texture2D COLOR;

        #region gameflags
        private HashSet<string> GameFlags = new HashSet<string>();

        public bool IsFlagSetOR(params string[] flags)
        {
            foreach (string f in flags)
            {
                if (this.GameFlags.Contains(f))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsFlagSet(string flag)
        {
            return this.GameFlags.Contains(flag);
        }

        public bool IsFlagSetAND(params string[] flags)
        {
            int count = 0;
            foreach (string f in flags)
            {
                if (this.GameFlags.Contains(f))
                {
                    count++;
                }
            }
            return count == flags.Length;
        }

        public void AddFlag(string s)
        {
            this.GameFlags.Add(s);
        }

        public void AddFlag(params string[] flags)
        {
            foreach (string s in flags)
            {
                this.GameFlags.Add(s);
            }
        }

        public void RemoveFlag(string s)
        {
            this.GameFlags.Remove(s);
        }

        #endregion gameflags
  
        public SpriteBatch SpriteBatch { get; internal set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public InputManager InputManager { get; private set; }

        public HudManager Hud { get; private set; }

        public ISceneController SceneController { get; private set; }

        public IResourceManager SceneResourceManager { get; private set; }

        public IResourceManager SharedResourceManager { get; private set; }

        private Dictionary<Type, ISceneComponent> components;
        
        public T GetComponent<T>()
        {
            return (T)this.components[typeof(T)];
        }

        public SceneContext(Dictionary<Type, ISceneComponent> c, SpriteBatch sb, GraphicsDeviceManager gdm, IResourceManager cm, IServiceProvider sp, InputManager im,ISceneController sc, HudManager hudmanager)
        {
            this.components = c;
            this.SpriteBatch = sb;
            this.GraphicsDeviceManager = gdm;
            this.SharedResourceManager = cm;
            this.InputManager = im;
            this.SceneController = sc;
            this.Hud = hudmanager;

            this.SceneResourceManager = new PWResourceManager(sp);

            COLOR = new Texture2D(this.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            COLOR.SetData<Color>(new Color[] { Color.White });// fill the texture with white

        }
        
        /*
      
       internal void Start()
       {

       }*/

       
       #region util methods
       public void DrawLine(Vector2 start, Vector2 end, Color color, float lineThickness = 0.1f)
       {
           Vector2 edge = end - start;
           // calculate angle to rotate line
           float angle = (float)Math.Atan2(edge.Y, edge.X);

           this.SpriteBatch.Draw(
               texture: COLOR, // Texture
               position: new Vector2(start.X, start.Y),
               color: color, // If you don't want to add tinting use white
               rotation: angle,                      // Rotation
               origin: new Vector2(0, lineThickness / 4),                   // Origin
               scale: new Vector2(edge.Length(), lineThickness),
               //  effect: SpriteEffects.None,     // Mirroring effects
               layerDepth: 0,
               sourceRectangle: null,
               destinationRectangle: null

           );                 // Layer depth
       }

       public void DrawImage(Texture2D tex, Vector2 position, float width, float height, Color color, float rotation = 0)
       {
           float w = tex.Width;
           float h = tex.Height;

           this.SpriteBatch.Draw(
             texture: tex, // Texture
             position: position,             // Position
             color: color, // If you don't want to add tinting use white
             rotation: rotation,
             sourceRectangle: new Rectangle(0, 0, (int)w, (int)h),
             origin: new Vector2(0, 0), //Vector2.One / 2,                     
             scale: new Vector2(width / w, height / h) // scale        
           );


       }

        /*
     /// <summary>
     /// Draw a line into a SpriteBatch
     /// </summary>
     /// <param name="batch">SpriteBatch to draw line</param>
     /// <param name="color">The line color</param>
     /// <param name="point1">Start Point</param>
     /// <param name="point2">End Point</param>
     /// <param name="Layer">Layer or Z position</param>
     public void DrawLine2(Vector2 point1, Vector2 point2, Color color, float thinkness = 0.1f)
     {
         float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
         float length = (point2 - point1).Length();

         Vector2 dir = point2 - point1;
         dir.Normalize();

         point1 += dir * thinkness / 4;

         dir = VectorMath.RotateAroundOrigin(dir, 90, true);

         point1 += dir * (thinkness / 2);


         this.SpriteBatch.Draw(COLOR, point1, null, color,
                    angle, Vector2.Zero, new Vector2(length, thinkness),
                    SpriteEffects.None, 0);
     }

     public void DrawPolyLine(Vector2[] points, Color color, float width = 1, bool closed = false)
     {
         for (int i = 0; i < points.Length - 1; i++)
             this.DrawLine(points[i], points[i + 1], color, width);
         if (closed)
             this.DrawLine(points[points.Length - 1], points[0], color, width);
     }


     public void DrawCircle(float steps, float radius, Vector2 position, Color color, float thinkness = 0.1f)
     {
         double angleStep = steps / radius;

         List<Vector2> points = new List<Vector2>();

         for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
         {
             // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
             int x = (int)Math.Round(radius + radius * Math.Cos(angle));
             int y = (int)Math.Round(radius + radius * Math.Sin(angle));

             points.Add(position + new Vector2(x, y));
         }

         this.DrawPolyLine(points.ToArray(), color, thinkness, true);
     }

     public void DrawSprite(Texture2D tex, Vector2 position, float width, float height, Color color, float rotation = 0)
     {
         float w = tex.Width;
         float h = tex.Height;

         this.SpriteBatch.Draw(
           texture: tex, // Texture
           position: position,             // Position
           color: color, // If you don't want to add tinting use white
           rotation: rotation,
           sourceRectangle: new Rectangle(0, 0, (int)w, (int)h),
           origin: new Vector2(w / 2, h / 2), //Vector2.One / 2,                     
           scale: new Vector2(width / w, height / h) // scale        
         );


     }

     

     public void DrawRectangle(Vector2 position, Vector2 size, float thicknessOfBorder, Color borderColor)
     {

         // Draw top line
         //     spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

         this.SpriteBatch.Draw(
             texture: COLOR, // Texture
             position: new Vector2(position.X, position.Y),
             scale: new Vector2(size.X, thicknessOfBorder),
             color: borderColor // If you don't want to add tinting use white
             );


         // Draw left line
         // spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

         this.SpriteBatch.Draw(
             texture: COLOR, // Texture
             position: new Vector2(position.X, position.Y),
             scale: new Vector2(thicknessOfBorder, size.Y),
             color: borderColor // If you don't want to add tinting use white
             );

         // Draw right line
      

         this.SpriteBatch.Draw(
             texture: COLOR, // Texture
             position: new Vector2((position.X + size.X - thicknessOfBorder), position.Y),
             scale: new Vector2(thicknessOfBorder, size.Y),
             color: borderColor // If you don't want to add tinting use white
             );


         // Draw bottom line
      

         this.SpriteBatch.Draw(
             texture: COLOR, // Texture
             position: new Vector2(position.X, position.Y + size.Y - thicknessOfBorder),
             scale: new Vector2(size.X, thicknessOfBorder),
             color: borderColor // If you don't want to add tinting use white
             );
     }

     public Texture2D CreateCircle(int radius)
     {
         int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
         Texture2D texture = new Texture2D(this.Graphics.GraphicsDevice, outerRadius, outerRadius);

         Color[] data = new Color[outerRadius * outerRadius];

         // Colour the entire texture transparent first.
         for (int i = 0; i < data.Length; i++)
             data[i] = Color.Transparent;

         // Work out the minimum step necessary using trigonometry + sine approximation.
         double angleStep = 1f / radius;

         for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
         {
             // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
             int x = (int)Math.Round(radius + radius * Math.Cos(angle));
             int y = (int)Math.Round(radius + radius * Math.Sin(angle));

             data[y * outerRadius + x + 1] = Color.White;
         }

         texture.SetData(data);
         return texture;
     }
          */
       #endregion util methods


    }

}



//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Protobase.entity;
//using Protobase.resources;
//using Protobase.util;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Protobase.manager
//{

//    public delegate void InitContextEventHandler(Scene s);
//    public delegate void LoadContentEventHandler(Scene s); 
//    public interface IScene{
//        SceneContext Context { get; }
//        string SceneName { get; }

//        void AddComponent(ISceneComponent sc);

//        void Initialize(SceneContext context);

//        void LoadContent(IResourceManager shared, IResourceManager scene);

//        void UnloadContent(IResourceManager scene);

        

//        void Update(GameTime gt);
//        void Render();

//        void Start(Scene previous = null);
//        void Pause(Scene previous = null);
//    }

//    public delegate void InitContextEventHandler(Scene s); 
//    public sealed class Scene
//    {
//        public SceneContext Context { get; private set; }

//        private List<ISceneComponent> compontentsOrder = new List<ISceneComponent>();
//        private Dictionary<Type,ISceneComponent> componentsDictionary = new Dictionary<Type,ISceneComponent>();

//        public string SceneName { get; private set; }

//        public event InitContextEventHandler InitContext;

//        public Scene(string name)
//        {
//            this.SceneName = name;
//        }

//        public void AddComponent(ISceneComponent gc)
//        {
//            this.compontentsOrder.Add(gc);
//            this.componentsDictionary.Add(gc.GetType(), gc);
//        }


//        public void LoadContent(SpriteBatch sb, GraphicsDeviceManager gdm, ContentManager cm)
//        {
//            this.Context = new SceneContext(this.componentsDictionary,sb,gdm,cm,new InputManager(PlayerIndex.One));
            
//            if (this.InitContext != null)
//            {
//                this.InitContext(this);
//            }

//            foreach (ISceneComponent gc in this.compontentsOrder)
//            {
//                gc.OnCreation(this.Context);
//            }
//        }


//        public void Render()
//        {
//            foreach (ISceneComponent gc in this.compontentsOrder)
//            {
//                gc.Render(this.Context);
//            }
//        }

//        public void Update(GameTime gt)
//        {
//            this.Context.InputManager.Update(gt, this.Context);
//            foreach (ISceneComponent gc in this.compontentsOrder)
//            {
//                gc.Update(this.Context,gt);
//            }
//        }



//        public void Start(Scene previous=null)
//        {
//            foreach (ISceneComponent gc in this.compontentsOrder)
//            {
//                gc.OnStart(this.Context,previous);
//            }
//        }

//        public void Pause(Scene next = null)
//        {
//            foreach (ISceneComponent gc in this.compontentsOrder)
//            {
//                gc.OnPause(this.Context, next);
//            }
//        }
//    }


//    public class SceneContext
//    {
//        public static Texture2D COLOR;

//        #region gameflags
//        private HashSet<string> GameFlags = new HashSet<string>();

//        public bool IsFlagSet(params string[] flags)
//        {
//            foreach (string f in flags)
//            {
//                if (this.GameFlags.Contains(f))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        public void AddFlag(string s)
//        {
//            this.GameFlags.Add(s);
//        }

//        public void RemoveFlag(string s)
//        {
//            this.GameFlags.Remove(s);
//        }

//        #endregion gameflags

//        //spritebatch 
//        //input manager
//        //graphic device

//        //-opt-> cam,hud,worldmanager

       

//        public SpriteBatch SpriteBatch { get; private set; }
//        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
//        public InputManager InputManager { get; private set; }

//        public ContentManager Content { get; private set; }

//        private Dictionary<Type, ISceneComponent> components;

//        public T Get<T>()
//        {
//            return (T)this.components[typeof(T)];
//        }

//        internal SceneContext(Dictionary<Type,ISceneComponent> c, SpriteBatch sb, GraphicsDeviceManager gdm, ContentManager cm, InputManager im)
//        {
//            this.components = c;
//            this.SpriteBatch = sb;
//            this.GraphicsDeviceManager = gdm;
//            this.Content = cm;
//            this.InputManager = im;

//            COLOR = new Texture2D(this.GraphicsDeviceManager.GraphicsDevice, 1, 1);
//            COLOR.SetData<Color>(new Color[] { Color.White });// fill the texture with white

//        }

//        internal void Start()
//        {

//        }

//        /*
//        #region util methods
//        public void DrawLine(Vector2 start, Vector2 end, Color color, float lineThickness = 0.1f)
//        {
//            Vector2 edge = end - start;
//            // calculate angle to rotate line
//            float angle = (float)Math.Atan2(edge.Y, edge.X);

//            this.SpriteBatch.Draw(
//                texture: COLOR, // Texture
//                position: new Vector2(start.X, start.Y),
//                color: color, // If you don't want to add tinting use white
//                rotation: angle,                      // Rotation
//                origin: new Vector2(0, lineThickness / 4),                   // Origin
//                scale: new Vector2(edge.Length(), lineThickness),
//                //  effect: SpriteEffects.None,     // Mirroring effects
//                layerDepth: 0,
//                sourceRectangle: null,
//                destinationRectangle: null

//            );                 // Layer depth
//        }


//        /// <summary>
//        /// Draw a line into a SpriteBatch
//        /// </summary>
//        /// <param name="batch">SpriteBatch to draw line</param>
//        /// <param name="color">The line color</param>
//        /// <param name="point1">Start Point</param>
//        /// <param name="point2">End Point</param>
//        /// <param name="Layer">Layer or Z position</param>
//        public void DrawLine2(Vector2 point1, Vector2 point2, Color color, float thinkness = 0.1f)
//        {
//            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
//            float length = (point2 - point1).Length();

//            Vector2 dir = point2 - point1;
//            dir.Normalize();

//            point1 += dir * thinkness / 4;

//            dir = VectorMath.RotateAroundOrigin(dir, 90, true);

//            point1 += dir * (thinkness / 2);


//            this.SpriteBatch.Draw(COLOR, point1, null, color,
//                       angle, Vector2.Zero, new Vector2(length, thinkness),
//                       SpriteEffects.None, 0);
//        }

//        public void DrawPolyLine(Vector2[] points, Color color, float width = 1, bool closed = false)
//        {
//            for (int i = 0; i < points.Length - 1; i++)
//                this.DrawLine(points[i], points[i + 1], color, width);
//            if (closed)
//                this.DrawLine(points[points.Length - 1], points[0], color, width);
//        }


//        public void DrawCircle(float steps, float radius, Vector2 position, Color color, float thinkness = 0.1f)
//        {
//            double angleStep = steps / radius;

//            List<Vector2> points = new List<Vector2>();

//            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
//            {
//                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
//                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
//                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

//                points.Add(position + new Vector2(x, y));
//            }

//            this.DrawPolyLine(points.ToArray(), color, thinkness, true);
//        }

//        public void DrawSprite(Texture2D tex, Vector2 position, float width, float height, Color color, float rotation = 0)
//        {
//            float w = tex.Width;
//            float h = tex.Height;

//            this.SpriteBatch.Draw(
//              texture: tex, // Texture
//              position: position,             // Position
//              color: color, // If you don't want to add tinting use white
//              rotation: rotation,
//              sourceRectangle: new Rectangle(0, 0, (int)w, (int)h),
//              origin: new Vector2(w / 2, h / 2), //Vector2.One / 2,                     
//              scale: new Vector2(width / w, height / h) // scale        
//            );


//        }

//        public void DrawImage(Texture2D tex, Vector2 position, float width, float height, Color color, float rotation = 0)
//        {
//            float w = tex.Width;
//            float h = tex.Height;

//            this.SpriteBatch.Draw(
//              texture: tex, // Texture
//              position: position,             // Position
//              color: color, // If you don't want to add tinting use white
//              rotation: rotation,
//              sourceRectangle: new Rectangle(0, 0, (int)w, (int)h),
//              origin: new Vector2(0, 0), //Vector2.One / 2,                     
//              scale: new Vector2(width / w, height / h) // scale        
//            );


//        }

//        public void DrawRectangle(Vector2 position, Vector2 size, float thicknessOfBorder, Color borderColor)
//        {

//            // Draw top line
//            //     spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

//            this.SpriteBatch.Draw(
//                texture: COLOR, // Texture
//                position: new Vector2(position.X, position.Y),
//                scale: new Vector2(size.X, thicknessOfBorder),
//                color: borderColor // If you don't want to add tinting use white
//                );


//            // Draw left line
//            // spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

//            this.SpriteBatch.Draw(
//                texture: COLOR, // Texture
//                position: new Vector2(position.X, position.Y),
//                scale: new Vector2(thicknessOfBorder, size.Y),
//                color: borderColor // If you don't want to add tinting use white
//                );

//            // Draw right line
      

//            this.SpriteBatch.Draw(
//                texture: COLOR, // Texture
//                position: new Vector2((position.X + size.X - thicknessOfBorder), position.Y),
//                scale: new Vector2(thicknessOfBorder, size.Y),
//                color: borderColor // If you don't want to add tinting use white
//                );


//            // Draw bottom line
      

//            this.SpriteBatch.Draw(
//                texture: COLOR, // Texture
//                position: new Vector2(position.X, position.Y + size.Y - thicknessOfBorder),
//                scale: new Vector2(size.X, thicknessOfBorder),
//                color: borderColor // If you don't want to add tinting use white
//                );
//        }

//        public Texture2D CreateCircle(int radius)
//        {
//            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
//            Texture2D texture = new Texture2D(this.Graphics.GraphicsDevice, outerRadius, outerRadius);

//            Color[] data = new Color[outerRadius * outerRadius];

//            // Colour the entire texture transparent first.
//            for (int i = 0; i < data.Length; i++)
//                data[i] = Color.Transparent;

//            // Work out the minimum step necessary using trigonometry + sine approximation.
//            double angleStep = 1f / radius;

//            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
//            {
//                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
//                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
//                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

//                data[y * outerRadius + x + 1] = Color.White;
//            }

//            texture.SetData(data);
//            return texture;
//        }

//        #endregion util methods

//        */
//    }

//}
