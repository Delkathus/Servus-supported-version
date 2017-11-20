using Servus_v2.Characters;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Servus_v2.Tasks
{
    public abstract class State : TaskBase, IComparable<State>, IComparer<State>
    {
        protected State(Character Character)
            : base(Character)
        {
        }

        /// <summary>
        /// Is this state enabled?
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Determines the frequency (Frame count) between each attempt to check, and run, this
        /// state. Default 0 is to run it every frame
        /// </summary>
        public virtual int Frequency => 0;

        /// <summary>
        /// Is this state running?
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Does this state need to run?
        /// </summary>
        public abstract bool NeedToRun { get; }

        /// <summary>
        /// Priority of the state, higher = higher ( between int.MinValue and int.MaxValue)
        /// </summary>
        public abstract int Priority { get; set; }

        /// <summary>
        /// Allow us to cancel our state
        /// </summary>
        public CancellationToken Token { get; set; }

        public int Compare(State x, State y)
        {
            return -x.Priority.CompareTo(y.Priority);
        }

        public int CompareTo(State other)
        {
            // We want the highest first. int, by default, chooses the lowest to be sorted at the
            // bottom of the list. We want the opposite.
            return -Priority.CompareTo(other.Priority);
        }

        /// <summary>
        /// Called when we enter this state.
        /// </summary>
        public abstract void Enter();

        /// <summary>
        /// Called when we exit this state
        /// </summary>
        public abstract void Exit();

        /// <summary>
        /// Main state loop, called every pulse, Logic goes here.
        /// </summary>
        public abstract void Update();
    }
}