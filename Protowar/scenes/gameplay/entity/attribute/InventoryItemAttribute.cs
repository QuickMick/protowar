using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity
{
    public class InventoryItemAttribute : EntityAttribute 
    {
        /// <summary>
        /// Anzeigename des Items
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gibt an wie oft das Item im Inventar gestackt werden kann
        /// </summary>
        int Stackable { get; set; }
    }
}
