using System;
using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.Common;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class Follow : HunterState
    {
        #region Fields

        private int _priority;

        #endregion Fields

        #region Constructors

        public Follow(Character character, Options options, TaskState taskState)
            : base(character, options, taskState)
        {
            Enabled = true;
        }

        #endregion Constructors

        #region Properties

        public int FollowMaximum { get; set; }
        public int FollowMinimum { get; set; }

        public override int Frequency
        {
            get
            {
                return 0;
            }
        }

        public override bool NeedToRun
        {
            get
            {
                return Enabled
                       && Character.Api != null
                       && Character.Status != EntityStatus.Engaged
                       && Character.Status == EntityStatus.Idle
                       && Navi.DistanceTo( > FollowMinimum
                       && Character.FFACE.Navigator.DistanceTo(Character.Leader.FFACE.Player.Position) < FollowMaximum;
                //&& DateTime.Now > Delay;
            }
        }

        public override int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = int.MaxValue - value;
            }
        }

        private DateTime Delay { get; set; }

        #endregion Properties

        #region Methods

        public override void Enter()
        {
            Log.WriteLine(string.Format("Entering {0} State", GetType().Name));
        }

        public override void Exit()
        {
            Log.WriteLine(string.Format("Exiting {0} State", GetType().Name));
        }

        public override void Update()
        {
            Log.WriteLine(string.Format("Following {0}", Character.Leader.Name));
            FFACE.Windower.SendString(string.Format("/follow {0}", Character.Leader.Name));
            Delay = DateTime.Now.AddSeconds(2);
        }

        #endregion Methods
    }
}