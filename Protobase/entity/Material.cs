using Microsoft.Xna.Framework;
using Protobase.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.entity
{
    public abstract class Material
    {
        private Color color = Color.White;
        public Color Color { 
            get{return this.color;}
            set
            {
               // this.color = value==null?Color.White:value;
                this.color = value;
            }
        }


        public Entity Parent { get; private set; }

        public void Construct(Entity e)
        {
            this.Parent = e;
        }

        public void Destruct()
        {
            this.Parent = null;
        }
        public abstract void Render(SceneContext g);

        public abstract void Update(GameTime gt);

    }
}
