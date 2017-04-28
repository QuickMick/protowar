using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity
{
    /// <summary>
    /// Enthaelt Punkt an dem die Waffe oder das Item gehalten wird ( an den actor body attached wird)
    /// </summary>
    public interface EquipmentAttribute
    {
        
        float AttachPointX { get; set; }
        float AttachPointY { get; set; }
    }
}
