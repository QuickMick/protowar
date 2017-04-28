using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Protobase.entity;
using Protobase.manager;
using Protobase.util;
using Protowar.scenes.gameplay.entity.attribute;
using Protowar.scenes.gameplay.scenecomponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protowar.scenes.gameplay.hud
{
    /// <summary>
    /// TODO: als ableitung von hudmouse machn
    /// </summary>
    public interface ICrosshairTool
    {
        void Init(Crosshair crosshair);

        void OnSelect();

        void OnPrimaryDown(SceneContext c);

        void OnSecondaryDown(SceneContext c);

        void OnPrimaryUp(SceneContext c);

        void OnSecondaryUp(SceneContext c);

        void OnDeselect();

        void Render(SceneContext context);

        void Update(SceneContext context, GameTime gameTime);
    }

    /// <summary>
    /// Wird immer geupdatet und gezeichnet. kann z.b. waffeninfo an crosshair sein, oder 
    /// zusätzliche benutzereingabe auf die curserposition spezifisch
    /// </summary>
    public interface ICrosshairDrawableProcessor 
    {
        void Init(Crosshair crosshair);

        void Render(SceneContext context);

        void Update(SceneContext context, GameTime gameTime);

        void OnRemove(Crosshair crosshair);
    }

    public delegate void OnSelectionEventHandler(Entity isSelected);

    public delegate void OnSelectionLeaveEventHandler(Entity wasSelected);

    public class Crosshair :  IRenderInGameloopHudElement
    {
        private const float DEGTORAD = (float)(Math.PI / 180);

        #region events
        public event OnSelectionEventHandler OnSelection;
        public event OnSelectionLeaveEventHandler OnSelectionLeave;
        #endregion events

        #region properties
        public Dictionary<Type, ICrosshairTool> tools = new Dictionary<Type, ICrosshairTool>();

        public List<ICrosshairDrawableProcessor> drawableProcessor = new List<ICrosshairDrawableProcessor>();

        public ICrosshairTool SelectedTool { get; private set; }

        public float CursorSize { get; set; }

        public float CursorRange { get; set; }

        public Texture2D CursorImage { get; set; }

        public Color CursorColor { get; set; }

        public float CursorLineStartOffset { get; set; }

        public float CursorLineEndOffset { get; set; }

        public Entity FocusedEntity { get; private set; }

        public float CursorSensitivity { get; set; }

        public Body Sensor { get; private set; }

        public Vector2 CursorDirection { get; private set; }

        public List<Entity> Selected { get; private set; }

        private World world;

        private FixedMouseJoint pj;

        private Vector2 relativePos;

        public Vector2 AbsolutPosition
        {
            get { return ConvertUnits.ToDisplayUnits(this.Sensor.Position); }
        }

        #endregion properties

        public Axis[] ListeningAxis { get; set; }

        public InputAction PrimaryAction { get; set; }
        public InputAction SecondaryAction { get; set; }

        public Crosshair(Axis[] listeningAxis, InputAction primaryAction,InputAction secondaryAction, Entity focusedEntity, World w)
        {
            #region defaults
            this.CursorSensitivity = 1f;
            this.CursorRange = 100f;
            this.CursorSize = 1f;
            this.CursorColor = Color.Red;
            this.CursorSize = 3f;
            this.CursorLineStartOffset = 1f;
            this.CursorLineEndOffset = 1f;
            
            #endregion defaults

            this.ListeningAxis = listeningAxis;

            this.world = w;
            this.FocusedEntity = focusedEntity;  

            this.Selected = new List<Entity>();

            this.Sensor = BodyFactory.CreateCircle(w, ConvertUnits.ToSimUnits(1), 1, this);
            this.Sensor.BodyType = BodyType.Dynamic;
            this.Sensor.IsSensor = true;
            this.Sensor.CollidesWith = Category.All;
            this.Sensor.CollisionCategories = Category.All;// Categories.SENSOR;
            this.Sensor.UserData = this;
            this.Sensor.IgnoreCollisionWith(this.FocusedEntity.Mesh.Body);

            pj = new FixedMouseJoint(this.Sensor, new Vector2(0, 0));

            this.world.AddJoint(pj);

            this.Sensor.OnCollision += (fA, fB, contact) =>
            {
                Entity e = fB.Body.UserData as Entity;
                if (e != null && !this.Selected.Contains(e))
                {
                    this.Selected.Add(e);
                }

                if (this.OnSelection != null)
                {
                    this.OnSelection(e);
                }
                return true;
            };

            this.Sensor.OnSeparation += (fA, fB) =>
            {
                Entity e = fB.Body.UserData as Entity;
                if (this.Selected.Contains(e))
                {
                    if (this.OnSelectionLeave != null)
                    {
                        this.OnSelectionLeave(e);
                    }
                    this.Selected.Remove(e);
                }
            };
        }

        public T GetTool<T>() where T : ICrosshairTool
        {
            return (T)this.tools[typeof(T)];
        }

        public void AddTool(ICrosshairTool t)
        {
            this.tools.Add(t.GetType(), t);
            t.Init(this);
        }

        public void AddDrawableProcessor(ICrosshairDrawableProcessor dp)
        {
            this.drawableProcessor.Add(dp);
            dp.Init(this);
        }

        public void RemoveDrawableProcessor(ICrosshairDrawableProcessor dp)
        {
            this.drawableProcessor.Remove(dp);
            dp.OnRemove(this);
        }

        public void SelectTool<T>() where T : ICrosshairTool
        {
            if (this.SelectedTool != null)
            {
                this.SelectedTool.OnDeselect();
            }

            this.SelectedTool = this.GetTool<T>();

            if (this.SelectedTool != null)
            {
                this.SelectedTool.OnSelect();
            }
        }

        public void Update(SceneContext c, GameTime gt)
        {
            if (c.IsFlagSetOR(GameFlagMask.CROSSHAIR_HIDE_MASK))
            {
                pj.WorldAnchorB = this.FocusedEntity.Mesh.Body.Position;
                return;
            }

            

            float dx = 0;
            float dy = 0;

            //InputConfig ic = InputConfig.Instance;
            //if (ic.RIGHT_AXIS.WasMoved)
            //{
            //    dx = ic.RIGHT_AXIS.DX;
            //    dy = ic.RIGHT_AXIS.DY;
            //}
            //else
            //{
            //    dx = ic.MOUSE.DX;
            //    dy = ic.MOUSE.DY;
            //}


            foreach (Axis a in this.ListeningAxis)
            {
                if (a.WasMoved)
                {
                    dx = a.DX;
                    dy = a.DY;
                    break;
                }
            }

            //   // Ray berechnen -> das ist die richtung/bahn auf der projektiele fliegen werden -> wird vom offsetpunkt an berechnet
            CursorDirection = this.FocusedEntity.Mesh.Body.Position - this.Sensor.Position; //Vector2.Subtract(actorData.RightHandOffset, new Vector2(this.X, this.Y));
            CursorDirection.Normalize();
            CursorDirection = Vector2.Negate(CursorDirection);


            // player rotation setzen
            float bodyAngle = this.FocusedEntity.Mesh.Body.Rotation;

            float desiredAngle = -VectorMath.GetAngle(CursorDirection);

           

            float speed = this.FocusedEntity.GetAttribute<ActorDataAttribute>().RotationSpeed;

            /// source: http://www.iforce2d.net/b2dtut/rotate-to-angle
            float nextAngle = bodyAngle + this.FocusedEntity.Mesh.Body.AngularVelocity / speed;
            float totalRotation = desiredAngle - nextAngle;
            while (totalRotation < -180 * DEGTORAD) totalRotation += 360 * DEGTORAD;
            while (totalRotation > 180 * DEGTORAD) totalRotation -= 360 * DEGTORAD;
            float desiredAngularVelocity = totalRotation * speed;

            float torque = this.FocusedEntity.Mesh.Body.Inertia * desiredAngularVelocity / (1 / speed);   

            this.FocusedEntity.Mesh.Body.ApplyTorque(torque);


            // position:
            this.relativePos -= ConvertUnits.ToSimUnits(new Vector2(dx, dy) * CursorSensitivity);

            if (VectorMath.Distance(new Vector2(), this.relativePos) >= ConvertUnits.ToSimUnits(this.CursorRange))
            {
                //nicht die ray nemen, sondern vom mittelpunkt des spielers ausgehen
                Vector2 rangeDir = this.relativePos;
                rangeDir.Normalize();

                this.relativePos = rangeDir * ConvertUnits.ToSimUnits(this.CursorRange);
            }

            pj.WorldAnchorB = this.FocusedEntity.Mesh.Body.Position + this.relativePos;


            foreach (ICrosshairDrawableProcessor dp in this.drawableProcessor)
            {
                dp.Update(c, gt);
            }

            // Update Tool
            if (this.SelectedTool != null)
            {

                if (this.PrimaryAction.WasPressed)
                {
                    this.SelectedTool.OnPrimaryDown(c);
                }

                if (this.PrimaryAction.WasReleased)
                {
                    this.SelectedTool.OnPrimaryUp(c);
                }

                if (this.SecondaryAction.WasPressed)
                {
                    this.SelectedTool.OnSecondaryDown(c);
                }

                if (this.SecondaryAction.WasReleased)
                {
                    this.SelectedTool.OnSecondaryUp(c);
                }

                this.SelectedTool.Update(c, gt);
            }
        }

        public void Render(SceneContext c)
        {
            if (c.IsFlagSetOR(GameFlagMask.CROSSHAIR_HIDE_MASK))
            {
                return;
            }

            foreach (ICrosshairDrawableProcessor dp in this.drawableProcessor)
            {
                dp.Render(c);
            }

            if (this.SelectedTool != null)
            {
                this.SelectedTool.Render(c);
            }

            Vector2 absoluteCursorPos = ConvertUnits.ToDisplayUnits(this.Sensor.Position);

            //Den offset der Linie berechnen, dass sie nciht bei null anfängt, sondern erst später
            Vector2 cursorLineStartPosition = ConvertUnits.ToDisplayUnits(this.FocusedEntity.Mesh.Body.Position);

            Vector2 dir = new Vector2(this.CursorDirection.X, this.CursorDirection.Y);
            dir.Normalize();

            if (this.CursorLineStartOffset > 0)
            {
                cursorLineStartPosition += dir * this.CursorLineStartOffset;
            }

            Vector2 cursorLineEndPosition = new Vector2(absoluteCursorPos.X, absoluteCursorPos.Y);

            if (this.CursorLineEndOffset > 0)
            {
                cursorLineEndPosition -= dir * this.CursorLineEndOffset;
            }

            //draw direction line
            c.DrawLine(
                cursorLineStartPosition,
                cursorLineEndPosition,
                this.CursorColor,
                0.5f
            );

            float csh = this.CursorSize / 2;
            float x = (absoluteCursorPos.X - csh);
            float y = (absoluteCursorPos.Y - csh);

            float width = this.CursorSize;
            float height = this.CursorSize;

            //crosshair
            c.SpriteBatch.Draw(
                texture: this.CursorImage, // Texture
                position: new Vector2(x, y),
                color: this.CursorColor, // If you don't want to add tinting use white
                scale: new Vector2(width / this.CursorImage.Width, height / this.CursorImage.Height)
            );                 // Layer depth
        }

        public Vector2 Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Render(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
    }
}
