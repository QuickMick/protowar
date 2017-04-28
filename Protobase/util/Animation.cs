using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.util
{
    public class AnimationSequence
    {
        //TODO: events rein
        public string Name { get; private set; }
        public int FirstFrame { get; private set; }
        public int LastFrame { get; private set; }
        public int Row { get; private set; }
        public int CurrentFrame { get; set; }

        public bool IsLooping { get; set; }

        /// <summary>
        /// für jeden frame gibt es einge geschwindigkeit.
        /// </summary>
        public float[] Speed { get; private set; }

        public float CurrentFrameSpeed
        {
            get
            {
                return this.Speed[this.CurrentFrame];
            }
        }

        public int FrameCount
        {
            get
            {
                return this.LastFrame - this.FirstFrame;
            }
        }

        public AnimationSequence(string name, int firstFrame, int lastFrame, int row, bool isLooping, float[] speed)
        {
            if (lastFrame < firstFrame)
            {
                throw new Exception("last frame cannot be before first Frame");
            }
            this.init(name, firstFrame, lastFrame, row, isLooping, speed);
        }

        public AnimationSequence(string name, int firstFrame, int lastFrame, int row, bool isLooping, float speed)
        {
            if (lastFrame < firstFrame)
            {
                throw new Exception("last frame cannot be before first Frame");
            }
            int count = lastFrame - firstFrame;
            float[] s = new float[count];

            for (int i = 0; i < count; i++)
            {
                s[i] = speed;
            }

            this.init(name, firstFrame, lastFrame, row, isLooping, s);
        }

        private void init(string name, int firstFrame, int lastFrame, int row, bool isLooping, float[] speed)
        {
            if (row < 0)
            {
                throw new Exception("row cannot be smaller than zero!");
            }

            this.Name = name;

            this.FirstFrame = firstFrame;
            this.LastFrame = lastFrame;
            this.Row = row;
            this.IsLooping = isLooping;
            this.Speed = speed;
        }

        public void NextFrame()
        {
            this.CurrentFrame++;
            if (this.CurrentFrame >= this.LastFrame)
            {
                this.CurrentFrame = this.FirstFrame;
            }
        }
    }

    public delegate void AnimationFinishedEventHandler(Animation sender);
    public delegate void AnimationStartedEventHandler(Animation sender);
    public delegate void AnimationStopedEventHandle(Animation sender);
    public delegate void AnimationResumedEventHandler(Animation sender);
    public delegate void AnimationPausedEventHandler(Animation sender);

    /// <summary>
    /// TUT:     
    /// AAnimation ad = new AAnimation();
    /// ad.Value =  new Animation(sprite, 8, 8);

    /// ad.Value.AddSequence(new AnimationSequence("Walk_UP", 0, 0, 0, true, 1000));
    /// ad.Value.AddSequence(new AnimationSequence("Walk_LEFT", 1, 3, 0, true, 100));
    /// ad.Value.AddSequence(new AnimationSequence("Walk_DOWN", 1, 3, 0, true, 40));
    /// ad.Value.AddSequence(new AnimationSequence("Walk_RIGHT", 4, 4, 0, true, 1000));
    /// ad.Value.SetAnimation("Walk_UP");
    /// </summary>


    public class Animation//, IRenderable
    {
        // TODO: events auf richtigkeit prüfn (dass sie an der richtigen stelle gefeuert werden
        #region Events
        public event AnimationFinishedEventHandler OnFinished;
        public event AnimationStartedEventHandler OnStart;
        public event AnimationStopedEventHandle OnStop;
        public event AnimationResumedEventHandler OnResume;
        public event AnimationPausedEventHandler OnPause;
        #endregion Events

        #region Fields
        public Texture2D Sprite { get; private set; }
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public AnimationSequence CurrentSequence { get; private set; }
        private Dictionary<string, AnimationSequence> sequences = new Dictionary<string, AnimationSequence>();

        private Timer timer = new Timer();

        /// <summary>
        /// Gibt an ob aktuell eine animation läuft
        /// </summary>
        public bool IsAnimationRunning { get; private set; }
        #endregion Fields

        public Animation(Texture2D sprite, int frameWidth, int frameHeight)
        {
            this.Sprite = sprite;

            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
        }

        #region Methods
        public void AddSequence(AnimationSequence sequence)
        {
            if (this.sequences.ContainsKey(sequence.Name))
            {
                throw new Exception("Animation already contains a sequence with this name!");
            }

            this.sequences.Add(sequence.Name, sequence);
        }

        public void RemoveSequence(AnimationSequence sequence)
        {
            this.sequences.Remove(sequence.Name);
        }

        public AnimationSequence GetSequence(string name)
        {
            if (this.sequences.ContainsKey(name))
            {
                return this.sequences[name];
            }

            return null;
        }

        public void SetAnimation(string name)
        {
            if (String.IsNullOrEmpty(name) || !this.sequences.ContainsKey(name))
            {
                this.CurrentSequence = null;
                return;
            }

            this.CurrentSequence = this.sequences[name];
          //  this.Stop(); 
        }

        private void nextFrame(Timer t)
        {
            if (this.CurrentSequence.CurrentFrame == this.CurrentSequence.LastFrame-1 && !this.CurrentSequence.IsLooping)
            {
                this.Pause();
                return;
            }

            this.CurrentSequence.NextFrame();
            t.Reset();
            this.timer.TimeInverval = this.CurrentSequence.CurrentFrameSpeed;
            this.timer.OnTimerFinish += this.nextFrame;
            this.timer.Start();
        }

        /// <summary>
        /// startet die aktuelle animation von vorne
        /// </summary>
        public void Start()
        {
            if (this.CurrentSequence == null)
            {
                return;
            }
            
            this.IsAnimationRunning = true;
            this.CurrentSequence.CurrentFrame = -1;
            this.nextFrame(this.timer);
            if (this.OnStart != null)
            {
                this.OnStart(this);
            }
        }

        /// <summary>
        /// Pausiert die Animation und setzt den frame zurück
        /// </summary>
        public void Stop()
        {
            if (this.CurrentSequence == null)
            {
                return;
            }
            this.CurrentSequence.CurrentFrame = 0;
            this.IsAnimationRunning = false;
            this.timer.Stop();
            if (this.OnStop != null)
            {
                this.OnStop(this);
            }
        }

        /// <summary>
        /// Pausiert die Animation
        /// </summary>
        public void Pause()
        {
            if (this.CurrentSequence == null)
            {
                return;
            }
            this.IsAnimationRunning = false;
            this.timer.Pause();
            if (this.OnPause != null)
            {
                this.OnPause(this);
            }
        }

        /// <summary>
        /// Führt die Animation vom aktuellen frame aus weiter
        /// </summary>
        public void Resume()
        {
            if (this.CurrentSequence == null)
            {
                return;
            }
            this.IsAnimationRunning = true;
            this.timer.Resume();
            if (this.OnResume != null)
            {
                this.OnResume(this);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.timer != null)
            {
                this.timer.Update(gameTime);
            }

            if (this.OnFinished != null)
            {
                this.OnFinished(this);
            }
        }
        #endregion Methods

        public void Draw(SpriteBatch sb, Vector2 position, Color color)
        {
            this.Draw(sb, position, 1, color);
        }

        public void Draw(SpriteBatch sb, Vector2 position, float scale, Color color)
        {
            this.Draw(sb, position, scale, color, 0, Vector2.Zero);
        }

        public void Draw(SpriteBatch sb, Vector2 position, float scale, Color color, float rotation, Vector2 origin)
        {
            sb.Draw(this.Sprite, position, this.CurrentFrameBounds, color, rotation, origin, scale, SpriteEffects.None,0f);
        }

        public Rectangle CurrentFrameBounds
        {
            get
            {
                if(this.CurrentSequence == null)
                {
                    throw new Exception("No sequence is set!");
                }
                return new Rectangle(this.CurrentSequence.CurrentFrame * this.FrameWidth, this.CurrentSequence.Row * this.FrameHeight, this.FrameWidth, this.FrameHeight);
            }
        }
    }
}
