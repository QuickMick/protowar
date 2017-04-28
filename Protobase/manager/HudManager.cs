using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.manager
{
    public interface IHudElement 
    {
        Vector2 Position { get; set; }

        int Width { get; set; }
        int Height { get; set; }

        void Render(SceneContext context);

        void Update(SceneContext context, GameTime gt);
    }

    /// <summary>
    /// wen ein hudelement sowas implementiert, wird es anstatt im hudspritebatch im gamespritebatch gerendert
    /// </summary>
    public interface IRenderInGameloopHudElement : IHudElement
    {
    }

    public class HudManager
    {
        private List<IHudElement> elements = new List<IHudElement>();

        private List<IRenderInGameloopHudElement> gameloopElemnts = new List<IRenderInGameloopHudElement>();

        public void Add(IHudElement he)
        {
            IRenderInGameloopHudElement r = he as IRenderInGameloopHudElement;

            if (r != null)
                this.gameloopElemnts.Add(r);
            else
                this.elements.Add(he);
        }

        public void Remove(IHudElement he)
        {
            IRenderInGameloopHudElement r = he as IRenderInGameloopHudElement;

            if (r != null)
                this.gameloopElemnts.Remove(r);
            else
                this.elements.Remove(he);
        }

        public void Update(SceneContext c, GameTime gt)
        {
            foreach (IRenderInGameloopHudElement he in this.gameloopElemnts)
            {
                he.Update(c, gt);
            }

            foreach (IHudElement he in this.elements)
            {
                he.Update(c, gt);
            }
        }

        public void Render(SceneContext context)
        {
            foreach (IHudElement he in this.elements)
            {
                he.Render(context);
            }
        }

        public void RenderInGameLoop(SceneContext context)
        {
            foreach (IRenderInGameloopHudElement he in this.gameloopElemnts)
            {
                he.Render(context);
            }
        }
    }
}
