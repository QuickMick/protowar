using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity.attribute
{
    public class BulletDataAttribute : EntityAttribute
    {
        public float Damage { get; set; }

        public float BulletSpeed { get; set; }

        /// <summary>
        /// wert fürs abprallen - normal 1 -> keine kraft verloren
        /// </summary>
        public float Restitution { get; set; }

        /// <summary>
        /// Anzahl durch wie viele gegner durchgeschossen werden kann
        /// </summary>
        public int SlugCount { get; set; }

        /// <summary>
        /// gibt an, wie oft die kugel abprallen kann von wänden
        /// </summary>
        public int BounceCount { get; set; }
   
    }
}
