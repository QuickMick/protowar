using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity.attribute
{
    public class WeaponDataAttribute : EntityAttribute, EquipmentAttribute
    {

       /* public string Name { get; set; }

        public int Stackable { get; set; }*/

        /// <summary>
        /// Der Rückstoß der Waffe
        /// </summary>
        public float Recoil { get; set; }

        /// <summary>
        /// Anzahl durch wie viele gegner durchgeschossen werden kann
        /// </summary>
        public int SlugCount { get; set; }

        /// <summary>
        /// gibt an, wie oft die kugel abprallen kann von wänden
        /// </summary>
        public int BounceCount { get; set; }

        /// <summary>
        /// Größe vom magazin
        /// </summary>
        public int MagazineSize { get; set; }

        /// <summary>
        /// Präzision der waffe
        /// </summary>
        public float Precission { get; set; }

        /// <summary>
        /// Gibt an wie sehr die waffe repariert ist
        /// </summary>
        public float Condition { get; set; }

        /// <summary>
        /// Typ der kugel - die genauere spezification bzw groesse damage usw,
        /// kann aus der STaticsLib geholt werden.
        /// </summary>
        public string BulletType { get; set; }

        /// <summary>
        /// Wie viel ist im Magazin
        /// </summary>
        private int magazinLoad;
        public int MagazinLoad
        {
            get { return this.magazinLoad; }
            set
            {
                if (value > this.MagazineSize)
                {
                    this.magazinLoad = this.MagazineSize;
                }
                else
                {
                    this.magazinLoad = value;
                }
            }
        }


        public int Damage { get; set; }

        public float FireRate { get; set; }


        public float AttachPointX { get; set; }

        public float AttachPointY { get; set; }
    }
}
