using Microsoft.Xna.Framework;
using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.manager
{
    public interface IGameMap
    {
        void Init(WorldManager wm);

        void Update(SceneContext c, GameTime gameTime);

        void RenderGround(SceneContext c);

        void RenderTop(SceneContext c);
    }
}
