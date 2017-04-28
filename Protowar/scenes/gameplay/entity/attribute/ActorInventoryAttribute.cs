using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity.attribute
{
    public class ActorInventoryAttribute : EntityAttribute
    {

        public EquipmentAttribute left { get; set; }

        public EquipmentAttribute right { get; set; }
    }
}
