using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.Common;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class TrackWhileFighting : HunterState
    {
        private int _priority;

        public TrackWhileFighting(Character Character, Options options, Taskstate Taskstate)
                    : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && Character.Status == EntityStatus.Engaged
                                          && !Character.Busy
                                          && !Character.IsCasting
                                          && (Navi.DistanceTo(TS.TargetMobId) > 3);

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        public override void Enter()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Entering {0} State", GetType().Name));
            Navi.DistanceTolerance = 1.0;
        }

        public override void Exit()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Exiting {0} State", GetType().Name));
        }

        public override void Update()
        {
            try
            {
                var MobPos = new Node { X = Api.Entity.GetEntity(TS.TargetMobId).X, Z = Api.Entity.GetEntity(TS.TargetMobId).Z, Y = Api.Entity.GetEntity(TS.TargetMobId).Y };
                Log.AddDebugText(TC.rtbDebug, "Tracking While Fighting");

                if (Navi.DistanceTo(TS.TargetMobId) > 3)
                {
                    if (Navi.DistanceTo(TS.TargetMobId) > 3)
                    {
                        Navi.GotoNPC(TS.TargetMobId, true);
                        Log.AddDebugText(TC.rtbDebug, string.Format(@"Moving to {0}, {1}y away", Character.Api.Entity.GetEntity(TS.TargetMobId).Name, Character.Api.Entity.GetEntity(TS.TargetMobId).Distance));
                    }
                }
                else if (Navi.DistanceTo(TS.TargetMobId) < 1.0)
                {
                    Api.ThirdParty.KeyDown(Keys.NUMPAD2);
                    Thread.Sleep(350);
                    Api.ThirdParty.KeyUp(Keys.NUMPAD2);
                }
                Navi.FaceHeading(MobPos);
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}