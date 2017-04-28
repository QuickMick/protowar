using Microsoft.Xna.Framework;
using Protobase.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.entity
{
    public abstract class EntityBehaviour
    {
        public bool IsEnabled { get; set; }
        public EntityBehaviour() { this.IsEnabled = true; }

        public abstract void Construct(Entity e);

        public abstract void Update(SceneContext c, Entity e, GameTime gt);

        public abstract void Destruct(Entity e);
    }
}
