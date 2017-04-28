using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Protobase.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.manager
{
    /// <summary>
    /// TODO: für andere 3 spieler erweitern
    /// TODO: mit SlimDX support für andere controller auser xbox controller anbieten
    /// </summary>
    public class InputManager
    {
        public PlayerIndex PlayerIndex { get; private set; }

        public  InputManager(PlayerIndex pi)
        {
                    this.PlayerIndex = pi;
                    this.MouseGrabbed = true;
        }

        public bool MouseGrabbed { get; set; }

        private List<InputAction> actions = new List<InputAction>();
        private List<Axis> axises = new List<Axis>();

        public InputAction AddAction(String ActionName)
        {
            InputAction a = new InputAction(this, ActionName);
            actions.Add(a);
            return a;
        }

        public Axis GetAxis(string s)
        {
            return axises.Find((Axis a) => { return a.Name == s; });
        }


        private bool isMouseAxisAdded = false;

        public Axis AddAxis(String AxisName, Axis.AxisType type, float sensitivity = 1.0f)
        {
            Axis a = new Axis(this, AxisName, type, sensitivity);

            if (type == Axis.AxisType.Mouse && this.isMouseAxisAdded)
            {
                throw new Exception("Mouse-Axis can only be added once!");
            }

            this.axises.Add(a);

            if (type == Axis.AxisType.Mouse)
            {
                this.isMouseAxisAdded = true;

            }
            return a;
        }

        public InputAction this[String ActionName]
        {
            get
            {
                return actions.Find((InputAction a) => { return a.Name == ActionName; });
            }
        }

        public void Update(GameTime gameTime , SceneContext c)
        {
            KeyboardState kbState = Keyboard.GetState();
            GamePadState gpState = GamePad.GetState(this.PlayerIndex);   /// TODO: für ander 3 spieler erweitern
            MouseState mState = Mouse.GetState();

            foreach (InputAction a in actions)
            {
                a.Update(kbState, gpState, mState);
            }

            foreach (Axis a in this.axises)
            {
                a.Update(gpState, mState, c, gameTime);
            }

            if (this.isMouseAxisAdded && this.MouseGrabbed)  // TODO: wirklich mit dem dass mouse geadded wurde?
            {
                int width = c.GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2;
                int height = c.GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2;
                try
                {
                    Mouse.SetPosition(width, height);   // maus immer zentrieren, damit man das delta berechnen kann
                }
                catch
                {
                    Console.WriteLine("RANZ MAUS SET POSITON FEHLER");
                }
            }
        }

    }

    public class Axis
    {
        // dass dei bewegung net zu gros wird
        public const float MouseDeltaRange = 100;


        public enum AxisType
        {
            LeftThumbStick,
            RightThumbStick,
            Mouse
        }

        private InputManager parent = null;
        public string Name { get; private set; }
        public AxisType Type { get; private set; }

        public float Sensitivity { get; set; }

        public float DX { get; private set; }
        public float DY { get; private set; }

        public Vector2 Delta
        {
            get { return new Vector2(this.DX, this.DY); }
        }

        public bool WasMoved { get; private set; }

        internal Axis(InputManager p, string n, AxisType type, float sensitivity = 1.0f)
        {
            parent = p;
            this.Name = n;
            this.Type = type;
            this.Sensitivity = sensitivity;
        }

        public float Length()
        {
            return new Vector2(this.DX, this.DY).Length();
        }

        internal void Update(GamePadState gpState, MouseState mState, SceneContext c, GameTime gameTime)
        {
            //  float oldDX = this.DX;
            //  float oldDY = this.DY;

            switch (this.Type)
            {
                case AxisType.LeftThumbStick:
                    this.DX = -gpState.ThumbSticks.Left.X;
                    this.DY = gpState.ThumbSticks.Left.Y;
                    break;

                case AxisType.RightThumbStick:
                    this.DX = -gpState.ThumbSticks.Right.X;
                    this.DY = gpState.ThumbSticks.Right.Y;
                    break;

                case AxisType.Mouse:
                    if (!this.parent.MouseGrabbed)
                    {
                        this.DX = 0;
                        this.DY = 0;
                        break;
                    }

                    float width = c.GraphicsDeviceManager.GraphicsDevice.Viewport.Width *0.5f;
                    float height = c.GraphicsDeviceManager.GraphicsDevice.Viewport.Height *0.5f;

                    // da die maus zentriert ist kann man einfach von der hälfte der bildschirmgröße die neue mausposition abziehen und hat so das neue mausdelta

                    float dx = width - mState.X;
                    float dy = (height - mState.Y);
                  
                    // mouse grabben

                  //  mState.X = (int)width;
                  //  mState.Y = (int)height;
                    try
                    {
                        Mouse.SetPosition((int)width, (int)height);     //TODO wirklich hier? warum ruckelt des so - voll ranzig
                    }
                    catch
                    {
                        Console.WriteLine("ranz maus setz position in input manager spackt wieder rum weils spiel beendet wurde");
                    }
                    if (dx > MouseDeltaRange)
                    {
                        dx = MouseDeltaRange;
                    }
                    if (dx < -MouseDeltaRange)
                    {
                        dx = -MouseDeltaRange;
                    }

                    if (dy > MouseDeltaRange)
                    {
                        dy = MouseDeltaRange;
                    }
                    if (dy < -MouseDeltaRange)
                    {
                        dy = -MouseDeltaRange;
                    }

                    this.DX = dx / MouseDeltaRange;
                    this.DY = dy / MouseDeltaRange;

                    break;
            }


            // this.DX *= gameTime.ElapsedGameTime.Milliseconds;
            // this.DY *= gameTime.ElapsedGameTime.Milliseconds;

            this.DX *= this.Sensitivity;
            this.DY *= this.Sensitivity;

            // wenn nicht gleich null beide dann event schickn dass sich was bewegen soll
            if (this.DX != 0 || this.DY != 0)
            {
                this.WasMoved = true;
           //     this.parent.sendEvent(new BasicEvent(this));
            }
            else
            {
                this.WasMoved = false;
            }

        }
    }


    public enum MouseButtons
    {
        LEFT_BUTTON,
        RIGHT_BUTTON,
        MIDDLE_BUTTON,
        X_BUTTON1,
        X_BUTTON2
    }

    public class InputAction
    {
        private String name;
        private List<Keys> keyList = new List<Keys>();
        private List<Buttons> buttonList = new List<Buttons>();
        List<MouseButtons> mouseList = new List<MouseButtons>();
        private InputManager parent = null;
        private bool currentStatus = false;
        private bool previousStatus = false;

        public bool IsDown { get { return currentStatus; } }
        public bool WasPressed { get { return (currentStatus) && (!previousStatus); } }
        public bool WasReleased { get { return (!currentStatus) && (previousStatus); } }
        public String Name { get { return name; } }

        internal InputAction(InputManager p, string n)
        {
            parent = p;
            name = n;
        }

        public void Add(Keys key)
        {
            if (!keyList.Contains(key))
                keyList.Add(key);
        }

        public void Add(Buttons button)
        {
            if (!buttonList.Contains(button))
                buttonList.Add(button);
        }

        public void Add(MouseButtons mb)
        {
            if (!this.mouseList.Contains(mb))
            {
                this.mouseList.Add(mb);
            }
        }


        internal void Update(KeyboardState kbState, GamePadState gpState, MouseState mState)
        {
            previousStatus = currentStatus;
            currentStatus = false;
            foreach (Keys k in keyList)
                if (kbState.IsKeyDown(k))
                    currentStatus = true;
            foreach (Buttons b in buttonList)
                if (gpState.IsButtonDown(b))
                    currentStatus = true;

            foreach (MouseButtons ms in this.mouseList)
            {
                switch (ms)
                {
                    case MouseButtons.LEFT_BUTTON:
                        if (mState.LeftButton == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.RIGHT_BUTTON:
                        if (mState.RightButton == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.MIDDLE_BUTTON:
                        if (mState.MiddleButton == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.X_BUTTON1:
                        if (mState.XButton1 == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.X_BUTTON2:
                        if (mState.XButton2 == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                }
            }
        }
    }

  /*  public class InputAction
    {
        String name;
        List<Keys> keyList = new List<Keys>();
        List<Buttons> buttonList = new List<Buttons>();
        List<MouseButtons> mouseList = new List<MouseButtons>();

        InputManager parent = null;
        bool currentStatus = false;
        bool previousStatus = false;

        public bool IsDown { get { return currentStatus; } }
        public bool IsTapped { get { return (currentStatus) && (!previousStatus); } }
        public String Name { get { return name; } }

        public InputAction(InputManager p, string n)
        {
            parent = p;
            name = n;
        }

        public void Add(Keys key)
        {
            if (!keyList.Contains(key))
                keyList.Add(key);
        }

        public void Add(Buttons button)
        {
            if (!buttonList.Contains(button))
                buttonList.Add(button);
        }

        public void Add(MouseButtons mb)
        {
            if (!this.mouseList.Contains(mb))
            {
                this.mouseList.Add(mb);
            }
        }

        internal void Update(KeyboardState kbState, GamePadState gpState, MouseState mState)
        {
            previousStatus = currentStatus;
            currentStatus = false;

            foreach (Keys k in keyList)
            {
                if (kbState.IsKeyDown(k))
                {
                    currentStatus = true;
                }
            }
            foreach (Buttons b in buttonList)
            {
                if (gpState.IsButtonDown(b))
                {
                    currentStatus = true;
                }
            }

            foreach (MouseButtons ms in this.mouseList)
            {
                switch(ms){
                    case MouseButtons.LEFT_BUTTON:
                        if (mState.LeftButton == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.RIGHT_BUTTON:
                        if (mState.RightButton == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.MIDDLE_BUTTON:
                        if (mState.MiddleButton == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.X_BUTTON1:
                        if (mState.XButton1 == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                    case MouseButtons.X_BUTTON2:
                        if (mState.XButton2 == ButtonState.Pressed)
                            currentStatus = true;
                        break;
                }
            }
        }
    }*/
}
