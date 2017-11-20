using Servus_v2.Characters;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class GoHome : HunterState
    {
        private int _priority;

        public GoHome(Character Character, Options options, Taskstate Taskstate)
                    : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => (Options.GoHome
                                           && TC.homepointCB.Checked
                                           && Api.Menu.IsMenuOpen
                                           && Api.Menu.HelpName.Contains("K.O"));

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

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
                Log.AddDebugText(TC.rtbDebug, "Going to HomePoint.");
                Api.ThirdParty.KeyPress(EliteMMO.API.Keys.NUMPADENTER);
                Api.ThirdParty.KeyPress(EliteMMO.API.Keys.LEFT);
                Api.ThirdParty.KeyPress(EliteMMO.API.Keys.NUMPADENTER);
                Character.Tasks.Huntertask.Stop();
                Log.AddDebugText(TC.rtbDebug, "You died and returned home, Stopping Tasks.");

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}