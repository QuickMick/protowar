using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Protobase.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.util.hud
{
    public class HudMouse : IHudElement
    {
        public const string FLAG_SHOW_HUD_MOUSE = "FLAG_SHOW_HUD_MOUSE";


        public Vector2 Position { get; set; }

        public Texture2D MouseTexture { get; set; }


        public int Width { get; set; }

        public int Height { get; set; }

        public Color Color { get; set; }

        public float Sensitivity { get; set; }

        private Axis[] listeningAxises;

        public HudMouse(Axis[] axises,Texture2D mt,int width=20, int height=30)
        {
            this.listeningAxises = axises;
            this.MouseTexture = mt;
            this.Sensitivity = 5;
            this.Color = Color.White;
            this.Width = width;
            this.Height = Height;
            this.Position = new Vector2(0);
        }

        public void Render(SceneContext c)
        {
            if (!c.IsFlagSet(HudMouse.FLAG_SHOW_HUD_MOUSE))
            {
                return;
            }

            c.DrawImage(this.MouseTexture, this.Position, this.Width, this.Height, this.Color);
        }

        public void Update(SceneContext c, GameTime gt)
        {
            if (!c.IsFlagSet(HudMouse.FLAG_SHOW_HUD_MOUSE))
            {
                return;
            }

          //  InputConfig ic = InputConfig.Instance;

            float dx = 0;
            float dy = 0;

            foreach (Axis a in this.listeningAxises)
            {
                if (a.WasMoved)
                {
                    dx = a.DX;
                    dy = a.DY;
                    break;
                }
            }

            //if (ic.RIGHT_AXIS.WasMoved)
            //{
            //    dx = ic.RIGHT_AXIS.DX;
            //    dy = ic.RIGHT_AXIS.DY;
            //}
            //else
            //{
            //    dx = ic.MOUSE.DX;
            //    dy = -ic.MOUSE.DY;
            //}

            this.Position -= new Vector2(dx, dy) * gt.ElapsedGameTime.Milliseconds * this.Sensitivity;

            if (Position.X < 0)
                this.Position = new Vector2(0, this.Position.Y);

            if (this.Position.Y < 0)
                this.Position = new Vector2(this.Position.X, 0);


            int w = c.GraphicsDeviceManager.GraphicsDevice.Viewport.Width;
            int h = c.GraphicsDeviceManager.GraphicsDevice.Viewport.Height;

            if (Position.X > w)
                this.Position = new Vector2(w, this.Position.Y);

            if (Position.Y > h)
                this.Position = new Vector2(this.Position.X, h);
        }
    }
}
