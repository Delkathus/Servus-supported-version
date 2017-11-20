using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.Common;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class MoveToMob : HunterState
    {
        #region Fields

        private readonly Timer _timer = new Timer { Interval = 500 };

        private int _priority;

        #endregion Fields

        #region Constructors

        public MoveToMob(Character character, Options options, TaskState taskState)
                    : base(character, options, taskState)
        {
            Enabled = true;
        }

        #endregion Constructors

        #region Properties

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
                    && Character.Status == EntityStatus.Idle
                    && TS.TargetMobId != 0
                    && !Character.IsCasting
                    && Navi.DistanceTo(TS.TargetMobId) > Options.PullDistance
                    && Navi.DistanceTo(TS.TargetMobId) < Options.SearchDistance;
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

        #endregion Properties

        #region Methods

        public override void Enter()
        {
            log.AddDebugText(TC.rtbDebug, string.Format("Entering {0} State", GetType().Name));
            Character.navi.DistanceTolerance = 1.5;
        }

        public override void Exit()
        {
            log.AddDebugText(TC.rtbDebug, string.Format("Exiting {0} State", GetType().Name));
        }

        public override void Update()
        {
            log.AddDebugText(TC.rtbDebug, "Moving to Mob");

            var Target = Api.Target.GetTargetInfo();
            var MobPos = new Node { X = Api.Entity.GetEntity(TS.TargetMobId).X, Z = Api.Entity.GetEntity(TS.TargetMobId).Z, Y = Api.Entity.GetEntity(TS.TargetMobId).Y };

            Navi.FaceHeading(MobPos);
            if (Target.TargetIndex != TS.TargetMobId)
            {
                Api.ThirdParty.KeyPress(Keys.ESCAPE);
                Thread.Sleep(1000);
                Api.Target.SetTarget(TS.TargetMobId);
            }
            if (Target.TargetIndex != TS.TargetMobId)
            {
                log.AddDebugText(TC.rtbDebug, "Unable to target mob.");
                return;
            }

            while (Navi.DistanceTo(TS.TargetMobId) > 3 && Options.PullWhilstMoving && !Token.IsCancellationRequested)
            {
                Navi.GotoNPC(TS.TargetMobId, true);
                if (Options.PullWhilstMoving && Navi.DistanceTo(TS.TargetMobId) < Options.PullDistance)
                {
                    Api.ThirdParty.SendString(string.Format("{0}", TC.pullTb.Text));
                    log.AddDebugText(TC.rtbDebug, "Pulling");
                }
            }
            while (Navi.DistanceTo(TS.TargetMobId) > Options.PullDistance && !Options.PullWhilstMoving && !Token.IsCancellationRequested)
            {
                Navi.GotoNPC(TS.TargetMobId, true);
                if (!Options.PullWhilstMoving && Navi.DistanceTo(TS.TargetMobId) < Options.PullDistance)
                {
                    Navi.Reset();
                    Api.ThirdParty.SendString(string.Format("{0}", TC.pullTb.Text));
                    log.AddDebugText(TC.rtbDebug, "Pulling");
                }
                while (Navi.DistanceTo(TS.TargetMobId) > 3 && Navi.DistanceTo(TS.TargetMobId) < Options.PullDistance && !Token.IsCancellationRequested)
                {
                    Navi.GotoNPC(TS.TargetMobId, true);
                }
            }

            #endregion Methods
        }
    }
}