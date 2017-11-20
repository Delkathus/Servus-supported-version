using EliteMMO.API;
using Servus_v2.Characters;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class Fight : HunterState
    {
        private int _priority;

        public Fight(Character Character, Options options, Taskstate Taskstate)
            : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && Api.Player.MainJob == (byte)JobType.BeastMaster
                                          && Character.HasPet
                                          && (Character.Status == EntityStatus.Idle || Character.Status == EntityStatus.Engaged)
                                          && !Character.Busy
                                          && TS.TargetMobId != 0 && CurrentMobID != TS.TargetMobId
                                          && Navi.DistanceTo(TS.TargetMobId)
                                          < Options.PullDistance
                                          && DateTime.Now > ReuseTime;

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        private int CurrentMobID { get; set; }

        private DateTime ReuseTime { get; set; }

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
                var Target = Api.Target.GetTargetInfo();
                Log.AddDebugText(TC.rtbDebug, "Fight");
                if (Target.TargetIndex == TS.TargetMobId && !Target.LockedOn)
                {
                    Thread.Sleep(400);
                    Api.ThirdParty.SendString("/lockon <t>");
                    Log.AddDebugText(TC.rtbDebug, string.Format(@"Correct target../lockon"));
                    Thread.Sleep(2000);
                }

                if (Target.TargetIndex != TS.TargetMobId)
                {
                    Api.ThirdParty.KeyPress(Keys.ESCAPE);
                    Thread.Sleep(1000);
                    Api.Target.SetTarget(TS.TargetMobId);
                }
                if (Target.TargetIndex != TS.TargetMobId)
                {
                    return;
                }

                CurrentMobID = TS.TargetMobId;
                Api.ThirdParty.SendString(@"/pet ""Fight"" <t> ");
                ReuseTime = ReuseTime.AddSeconds(10);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}