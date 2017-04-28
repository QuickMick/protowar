using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.util
{
    public delegate void OnTimerFinishedEventHandler(Timer sender);
    public delegate void OnTimerPauseEventHandler(Timer sender);
    public delegate void OnTimerResumeEventHandler(Timer sender);
    public delegate void OnTimerStartEventHandler(Timer sender);
    public delegate void OnTimerStopEventHandler(Timer sender);

    public class Timer
    {
        public event OnTimerFinishedEventHandler OnTimerFinish;
        public event OnTimerPauseEventHandler OnPause;
        public event OnTimerResumeEventHandler OnResume;
        public event OnTimerStartEventHandler OnStart;
        public event OnTimerStopEventHandler OnStop;

        public bool IsLooping { get; set; }
        public bool IsPaused { get; private set; }

        public float TimeBuffer { get; private set; }

        private float time = 0;

        private static List<Timer> timer = new List<Timer>();
        private static List<Timer> remove = new List<Timer>();

        public static void UpdateAll(GameTime gameTime)
        {
            foreach (Timer t in timer)
            {
                t.Update(gameTime);
            }

            foreach (Timer t in remove)
            {
                timer.Remove(t);
            }
        }

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Setzt den Timer auf erzeugungszustand zurück,
        /// dabei wird der Zeitintervall auf 0 gesetzt.
        /// </summary>
        public void Reset()
        {
            this.OnTimerFinish = null;
            this.OnPause = null;
            this.OnResume = null;
            this.OnStart = null;
            this.OnStart = null;
            this.time = 0;
            this.TimeBuffer = 0;
        }

        /// <summary>
        /// Returns the waitingtime of the Timer.
        /// Setting a new Timeintervall will reset the timer.
        /// </summary>
        public float TimeInverval
        {
            get
            {
                return this.time;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("time cannot be smaller than zero");
                }

                if (value != this.time)
                {
                    this.TimeBuffer = 0;
                }

                this.time = value;
            }
        }

        public Timer()
            : this(0, false)
        {

        }

        public Timer(float time, bool isLooping = false)
        {
            this.TimeInverval = time;
            this.IsLooping = isLooping;
            this.TimeBuffer = 0;
            this.IsPaused = true;
        }

        public void Start()
        {
            timer.Add(this);
            this.IsPaused = false;
            this.TimeBuffer = 0;
            if (this.OnStart != null)
            {
                this.OnStart(this);
            }
        }

        public void Stop()
        {
            remove.Add(this);
            this.IsPaused = true;
            this.TimeBuffer = 0;
            if (this.OnStop != null)
            {
                this.OnStop(this);
            }
        }

        public void Resume()
        {
            this.IsPaused = false;
            if (this.OnResume != null)
            {
                this.OnResume(this);
            }
        }

        public void Pause()
        {
            this.IsPaused = true;
            if (this.OnPause != null)
            {
                this.OnPause(this);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsPaused)
            {
                return;
            }

            if (this.TimeBuffer < this.time)
            {
                this.TimeBuffer += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                this.TimeBuffer = 0;

                if (this.IsLooping)
                {
                    this.Start();
                    if (this.OnTimerFinish != null)
                    {
                        this.OnTimerFinish(this);
                    }
                }
                else
                {
                    this.Stop();
                    if (this.OnTimerFinish != null)
                    {
                        this.OnTimerFinish(this);
                    }
                }
            }
        }
    }
}
