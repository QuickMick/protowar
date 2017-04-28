using FarseerPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.entity
{
    public class Transformation
    {
        private Entity parent;

        public void Construct(Entity e)
        {
            this.parent = e;
        }

        public void Destruct()
        {
            this.parent = null;
        }

        public Vector2 Position
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(this.parent.Mesh.Body.Position);
            }
            set
            {
                //this.parent.Mesh.Body.SetTransform(ConvertUnits.ToSimUnits(value),this.Rotation);
                this.parent.Mesh.Body.Position = ConvertUnits.ToSimUnits(value);
            }
        }

        public float Rotation
        {
            get
            {
                return this.parent.Mesh.Body.Rotation;
            }
            set
            {
                //this.parent.Mesh.Body.Rotation = value;
               // this.parent.Mesh.Body.SetTransform(this.parent.Mesh.Body.Position, value);
                this.parent.Mesh.Body.Rotation = value;
            }
        }
    }
}
