using Servus_v2.Characters;
using Servus_v2.Common;
using System;
using System.Timers;

namespace Servus_v2.Tasks.Hunter
{
    public class Taskstate
    {
        private readonly Timer _timer = new Timer { Interval = 100 };

        public Taskstate(Character character, Options options)
        {
            Character = character;
            Options = options;
            PreviousChestIndieces = new LimitedQueue<int>(3);
            _timer.Elapsed += Update;
            DominionSergeantIndex = -1;
        }

        public event Action<string> Stopped = delegate { };

        public int DominionSergeantIndex { get; set; }
        public bool Initialized { get; set; }
        public bool MoveEnabled { get; set; }
        public LimitedQueue<int> PreviousChestIndieces { get; set; }
        public string SignetNpc { get; set; }
        public int TargetChestIndex { get; set; }

        public int TargetMobId => Character.Target.FindBestTarget();

        public bool TrackChests { get; set; }

        private Character Character { get; }

        private Options Options { get; }

        public void Initialize()
        {
            Initialized = false;
            MoveEnabled = false;
        }

        public void Start()
        {
            Initialize();
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Enabled = false;
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            //TargetMobId = Character.Api == null
            //    ? Character.Target.GetClosestAttackableMobId(Options.Targets, null, distance)
            //    : Character.Status == EntityStatus.Engaged
            //        ? Character.Leader.Target.GetClosestAttackableMobId(Options.Targets, null, 15)
            //        : 0;
        }
    }
}