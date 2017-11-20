using EliteMMO.API;
using Servus_v2.Characters;
using System;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class TrackNextTarget : HunterState
    {
        private int _priority;

        public TrackNextTarget(Character Character, Options options, Taskstate Taskstate)
                    : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && Character.Status == EntityStatus.Idle
                                          && !Character.Busy
                                          && TS.TargetMobId != 0
                                          && !Character.IsCasting
                                          && Navi.DistanceTo(TS.TargetMobId) > 3
                                          && Navi.DistanceTo(TS.TargetMobId) > Options.PullDistance
                                          && Navi.DistanceTo(TS.TargetMobId) < Options.SearchDistance;

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        public override void Enter()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Entering {0} State", GetType().Name));
            Character.Navi.DistanceTolerance = 1.5;
        }

        public override void Exit()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Exiting {0} State", GetType().Name));
        }

        public override void Update()
        {
            try
            {
                Log.AddDebugText(TC.rtbDebug, "Tracking");

                while (Navi.DistanceTo(TS.TargetMobId) > Options.PullDistance && !Token.IsCancellationRequested)
                {
                    Navi.GotoNPC(TS.TargetMobId, true);
                    Log.AddDebugText(TC.rtbDebug, string.Format(@"Moving to {0}, {1}y away", Character.Api.Entity.GetEntity(TS.TargetMobId).Name, Character.Api.Entity.GetEntity(TS.TargetMobId).Distance));
                }
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}