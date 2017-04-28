using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Protobase.manager;
using Protobase.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.entity.materials
{
    public class SimpleTexturedMaterial : Material
    {
        private Texture2D texture;

        public SimpleTexturedMaterial(IResourceManager rm, string textureName)
        {
            this.texture = rm.GetTexture(textureName);
        }

        public override void Render(SceneContext g)
        {
            if(this.texture == null){
                return;
            }

            float width = this.texture.Width;
            float height = this.texture.Height;

            g.SpriteBatch.Draw(
                texture: this.texture, // texture
                position: this.Parent.Transform.Position,             // position
                color: this.Color, 
                rotation: this.Parent.Transform.Rotation, //+ tex.rotation,
                sourceRectangle: new Rectangle(0, 0, (int)width, (int)height),
                origin: new Vector2(width *0.5f, height *0.5f), //vector2.one / 2,                     
                scale: new Vector2(this.Parent.Mesh.Width / width, this.Parent.Mesh.Height / height) // scale        
            );    
        }

        public override void Update(GameTime gt)
        {
        }
    }
}
