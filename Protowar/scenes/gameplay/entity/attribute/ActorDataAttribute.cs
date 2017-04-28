using FarseerPhysics;
using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity.attribute
{
    public class ActorDataAttribute : EntityAttribute
    {
        public float Speed { get; set; }

        public float RotationSpeed { get; set; }

        /// <summary>
        /// Speed beim rennen
        /// </summary>
        public float SpeedMultiplicator { get; set; }

        #region maxValues
     /*   private float maxSpeed;
        public float MaxSpeed
        {
            get { return ConvertUnits.ToDisplayUnits(maxSpeed); }
            set { this.maxSpeed = ConvertUnits.ToSimUnits(value); }
        }*/

        public float MaxSpeed { get; set; }
        #endregion maxValues

        public ActorDataAttribute(float speed, float rotationSpeed, float maxSpeed, float speedMultiplicator)
        {
            this.Speed = speed;
            this.RotationSpeed = rotationSpeed;
            this.SpeedMultiplicator = speedMultiplicator;
            this.MaxSpeed = maxSpeed;
        }
    }
}
