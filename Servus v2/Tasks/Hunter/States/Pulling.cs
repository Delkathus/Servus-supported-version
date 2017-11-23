using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.Common;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class Pulling : HunterState
    {
        private int _priority;

        public Pulling(Character character, Options options, Taskstate Taskstate)
                    : base(character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && Character.Status == EntityStatus.Idle
                                          && !Character.Busy
                                          && TS.TargetMobId != 0
                                          && !Character.IsCasting
                                          && Navi.DistanceTo(TS.TargetMobId) < Options.PullDistance;

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

        public void FaceMob()
        {
            var Target = Api.Target.GetTargetInfo();
            var MobPos = new Node { X = Api.Entity.GetEntity(TS.TargetMobId).X, Z = Api.Entity.GetEntity(TS.TargetMobId).Z, Y = Api.Entity.GetEntity(TS.TargetMobId).Y };
            Log.AddDebugText(TC.rtbDebug, "Engaging");
            Navi.FaceHeading(MobPos);

            if (Target.TargetIndex != TS.TargetMobId)
            {
                Api.ThirdParty.KeyPress(Keys.ESCAPE);
                Thread.Sleep(1000);
                Api.Target.SetTarget(TS.TargetMobId);
            }
            if (Target.TargetIndex == TS.TargetMobId && !Target.LockedOn)
            {
                Thread.Sleep(400);
                Api.ThirdParty.SendString("/lockon <t>");
                Log.AddDebugText(TC.rtbDebug, string.Format(@"Correct target../lockon"));
                Thread.Sleep(2000);
            }
            if (Target.TargetIndex != TS.TargetMobId)
            {
                Log.AddDebugText(TC.rtbDebug, "Unable to Target mob.");
            }
        }

        public override void Update()
        {
            try
            {
                if (Navi.DistanceTo(TS.TargetMobId) < Options.PullDistance && !Token.IsCancellationRequested)
                {
                    FaceMob();
                    var Target = Api.Target.GetTargetInfo();
                    if (Target.TargetIndex == TS.TargetMobId)
                    {
                        Log.AddDebugText(TC.rtbDebug, string.Format(@"Using Pull command {0}", TC.pullTb.Text));
                        Api.ThirdParty.SendString(TC.pullTb.Text);
                        Thread.Sleep(2000);
                        Api.ThirdParty.SendString("/attack <t>");
                        Thread.Sleep(1000);
                        Navi.GotoNPC(TS.TargetMobId, true);
                        Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}