using EliteMMO.API;
using Servus_v2.Characters;
using System;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class PathFinder : HunterState
    {
        private int _priority;

        public PathFinder(Character Character, Options options, Taskstate Taskstate)
            : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun
        {
            get
            {
                return Enabled
                       && !Character.Busy
                       && Character.Status == EntityStatus.Idle
                       && Character.Navi.Waypoints.Count > 0
                       && Character.Navi.FailedToPath < Options.FailedToPathCount
                       && TS.TargetMobId != 0
                       && Character.Navi.DistanceTo(TS.TargetMobId) > Options.PullDistance
                       && !Character.Navi.Desti();
            }
        }

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        private int Hpup { get; set; }
        private int Mpup { get; set; }

        public override void Enter()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Entering {0} State", GetType().Name));
        }

        public override void Exit()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Exiting {0} State", GetType().Name));
        }

        public override void Update()
        {
            try
            {
                var mob = Character.Api.Entity.GetEntity(Character.Target.FindBestTarget());

                var End = Character.Navi.GetWaypointClosestTo(mob.X, mob.Z);

                if (mob != null || End != null)
                {
                    var CurrentPath = Character.Navi.GetPath(End.X, End.Z, Character.Target.FindBestTarget());

                    if (CurrentPath.Count > 0)
                    {
                        Character.Logger.AddDebugText(Character.Tc.rtbDebug, "Path found");

                        foreach (var point in CurrentPath)
                        {
                            if (Character.Navi.DistanceTo(TS.TargetMobId) > Options.PullDistance && !Character.Navi.Desti())
                            {
                                Character.Navi.GoTo(point.X, point.Z);
                                Character.Logger.AddDebugText(Character.Tc.rtbDebug, string.Format(@"Moving to path. {0}, {1}y", mob.Name, mob.Distance));
                            }
                        }
                    }
                    else Character.Navi.Reset();
                    Exit();
                }
                Character.Navi.Reset();
                Exit();
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}