using Microsoft.Xna.Framework;
using Protobase.entity;
using Protobase.manager;
using Protowar.scenes.gameplay.entity.attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity.behaviour
{
    public class InventoryInputHandlerBehaviour : EntityBehaviour
    {
        private ActorInventoryAttribute actorInventory;
        public override void Construct(Entity e)
        {
            this.actorInventory = e.GetAttribute<ActorInventoryAttribute>();
        }

        public override void Update(SceneContext c, Entity e, GameTime gt)
        {
            //TODO: equipt weapon
        }

        public override void Destruct(Entity e)
        {
            this.actorInventory = null;
        }
    }
}
