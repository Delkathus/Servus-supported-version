using Servus_v2.Characters;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class Idle : HunterState
    {
        public Idle(Character Character, Options options, Taskstate Taskstate)
            : base(Character, options, Taskstate)
        {
            Priority = int.MinValue;
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => TS.TargetMobId == 0;

        public bool OnDeath { get; set; }
        public bool OnPartyDisband { get; set; }
        public sealed override int Priority { get; set; }

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
                var rand = new Random().Next(Options.IdleDelay, 99);

                var later = DateTime.Now.AddSeconds(rand);

                Log.AddDebugText(TC.rtbDebug, string.Format("{0} Idling..", Api.Player.Name));
                while (DateTime.Now < later && TS.TargetMobId == 0)
                {
                    Thread.Sleep(100);
                }
                Character.Navi.FailedToPath = 0;
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"Reset failed to path count.")));
                if (Character.Target.BlockedTargets.Count > 0)
                {
                    Character.Target.BlockedTargets.Clear();
                    Log.AddDebugText(TC.rtbDebug, (string.Format(@"Cleared blocked ids.")));
                }
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}