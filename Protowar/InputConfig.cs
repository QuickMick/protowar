using Microsoft.Xna.Framework.Input;
using Protobase.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar
{
    public class InputConfig
    {
        #region KEYS
        public float LEFT_AXIS_SENSITIVITY { get; private set; }

        public Axis MOUSE { get; private set; }
        public Axis LEFT_AXIS { get; private set; }
        public Axis RIGHT_AXIS { get; private set; }

        public InputAction LEFT_MOUSE { get; private set; }
        public InputAction RIGHT_MOUSE { get; private set; }

        public InputAction UP { get; private set; }
        public InputAction DOWN { get; private set; }
        public InputAction LEFT { get; private set; }
        public InputAction RIGHT { get; private set; }

        public InputAction INVENTORY { get; private set; }

        public InputAction RUN { get; private set; }


        public InputAction INVENTORY_LEFT_CLICK { get; private set; }
        public InputAction INVENTORY_RIGHT_CLICK { get; private set; }

        public InputAction SHOOT_RIGHT { get; private set; }
        public InputAction SHOOT_LEFT { get; private set; }

        public InputAction USE { get; private set; }



        #endregion KEYS

        private InputConfig() { }

        private static InputConfig instance;
        public static InputConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputConfig();
                }

                return instance;
            }
        }

        public void Init(InputManager im)
        {
            this.LEFT_AXIS_SENSITIVITY = 0.8f;

            this.MOUSE = im.AddAxis("MOUSE", Axis.AxisType.Mouse, 1f);
            this.LEFT_AXIS = im.AddAxis("LeftThumbStick", Axis.AxisType.LeftThumbStick, LEFT_AXIS_SENSITIVITY);
            this.RIGHT_AXIS = im.AddAxis("RightThumbStick", Axis.AxisType.RightThumbStick, 0.3f);

            this.UP = im.AddAction("UP");
            this.UP.Add(Buttons.DPadUp);
            this.UP.Add(Keys.W);
            this.UP.Add(Keys.Up);

            this.DOWN = im.AddAction("DOWN");
            this.DOWN.Add(Buttons.DPadDown);
            this.DOWN.Add(Keys.S);
            this.DOWN.Add(Keys.Down);

            this.LEFT = im.AddAction("LEFT");
            this.LEFT.Add(Buttons.DPadLeft);
            this.LEFT.Add(Keys.A);
            this.LEFT.Add(Keys.Left);

            this.RIGHT = im.AddAction("RIGHT");
            this.RIGHT.Add(Buttons.DPadRight);
            this.RIGHT.Add(Keys.D);
            this.RIGHT.Add(Keys.Right);

            this.RUN = im.AddAction("RUN");
            this.RUN.Add(Buttons.LeftStick);
            this.RUN.Add(Keys.LeftShift);
            this.RUN.Add(Keys.V);

            this.SHOOT_RIGHT = im.AddAction("SHOOT_RIGHT");
            this.SHOOT_RIGHT.Add(Buttons.RightTrigger);
            this.SHOOT_RIGHT.Add(MouseButtons.RIGHT_BUTTON);

            this.SHOOT_LEFT = im.AddAction("SHOOT_LEFT");
            this.SHOOT_LEFT.Add(Buttons.LeftTrigger);
            this.SHOOT_LEFT.Add(MouseButtons.LEFT_BUTTON);

            this.INVENTORY = im.AddAction("INVENTORY");
            this.INVENTORY.Add(Keys.I);

            this.LEFT_MOUSE = im.AddAction("LEFT_MOUSE");
            this.LEFT_MOUSE.Add(MouseButtons.LEFT_BUTTON);
            this.LEFT_MOUSE.Add(Buttons.RightTrigger);

            this.RIGHT_MOUSE = im.AddAction("RIGHT_MOUSE");
            this.RIGHT_MOUSE.Add(MouseButtons.RIGHT_BUTTON);
            this.RIGHT_MOUSE.Add(Buttons.LeftTrigger);

            this.INVENTORY_LEFT_CLICK = im.AddAction("INVENTORY_LEFT_CLICK");
            this.INVENTORY_LEFT_CLICK.Add(MouseButtons.LEFT_BUTTON);

            this.INVENTORY_RIGHT_CLICK = im.AddAction("INVENTORY_RIGHT_CLICK");
            this.INVENTORY_RIGHT_CLICK.Add(MouseButtons.RIGHT_BUTTON);


            this.USE = im.AddAction("USE");
            this.USE.Add(Buttons.Y);
            this.USE.Add(Keys.E);
        }
    }
}
