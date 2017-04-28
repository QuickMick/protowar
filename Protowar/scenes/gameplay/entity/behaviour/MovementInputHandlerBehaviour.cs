using Microsoft.Xna.Framework;
using Protobase.entity;
using Protobase.manager;
using Protowar.scenes.gameplay.entity.attribute;
using Protowar.scenes.gameplay.scenecomponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.entity.behaviour
{
    public class MovementInputHandlerBehaviour : EntityBehaviour
    {
        private ActorDataAttribute actorData;
        public override void Construct(Entity e)
        {
            this.actorData = e.GetAttribute<ActorDataAttribute>();
        }


        public override void Destruct(Entity e)
        {
            this.actorData = null;
        }

        private void Accelerate(Entity e, Vector2 dir)
        {
            float speed = actorData.Speed;

            if (dir == Vector2.Zero)
            {
                e.Mesh.Body.LinearVelocity *= 0.5f;
                return;
            }

            if (e.HasFlag(EntityFlags.IS_RUNNING))
            {
                speed *= actorData.SpeedMultiplicator;
            }

            e.Mesh.Body.ApplyLinearImpulse(dir * speed);

            if (e.Mesh.Body.LinearVelocity.Length() > actorData.MaxSpeed)
            {
                e.Mesh.Body.LinearVelocity.Normalize();
                e.Mesh.Body.LinearVelocity *= actorData.MaxSpeed;
                if (e.HasFlag(EntityFlags.IS_RUNNING))
                {
                    e.Mesh.Body.LinearVelocity *= actorData.SpeedMultiplicator;
                }
            }

        }



        public override void Update(SceneContext c, Entity e, GameTime gt)
        {
            if (c.IsFlagSetOR(GameFlagMask.MENU_OPEN))
            {
                return;
            }

            InputConfig ic = InputConfig.Instance;

            if (ic.RUN.WasReleased && e.HasFlag(EntityFlags.IS_MOVING))
            {
                e.AddFlag(EntityFlags.IS_RUNNING);
                e.AddFlag(EntityFlags.STARTS_RUNNING);
            }

            if (ic.RUN.WasReleased ||
                (!e.HasFlag(EntityFlags.IS_MOVING) && e.HasFlag(EntityFlags.IS_RUNNING)))
            {
                e.AddFlag(EntityFlags.STOPS_RUNNING);
            }

            // laufen starten, aber noch running gedrückt
            if (ic.RUN.IsDown && e.HasFlag(EntityFlags.STARTS_MOVING))
            {
                e.AddFlag(EntityFlags.STARTS_RUNNING);
                e.AddFlag(EntityFlags.IS_RUNNING);
            }

            if (ic.RUN.IsDown)
            {
                e.AddFlag(EntityFlags.IS_RUNNING);
            }

            #region gamepadmovement
            if (ic.LEFT_AXIS.WasMoved)
            {
                this.Accelerate(e, -ic.LEFT_AXIS.Delta);

            }
            #endregion gamepadmovement
            else
            #region keyboardmovement
            {
                Vector2 tempVelocity = Vector2.Zero;
                if (ic.UP.IsDown)  //TODO: wenn mehrere keys down dann ur die hälfte
                {
                    tempVelocity -= new Vector2(0, 1);
                }

                if (ic.DOWN.IsDown)
                {
                    tempVelocity -= new Vector2(0, -1);
                }

                if (ic.LEFT.IsDown)
                {
                    tempVelocity += new Vector2(-1, 0);
                }

                if (ic.RIGHT.IsDown)
                {
                    tempVelocity += new Vector2(1, 0);
                }

                if (tempVelocity.Length() > 0)
                {
                    tempVelocity.Normalize();   // dass wenn man schräg läuft man nicht schneller wird
                }

                this.Accelerate(e, tempVelocity * ic.LEFT_AXIS_SENSITIVITY);
            }
            #endregion keyboardmovement

        }

    

   
    }
}
