using EliteMMO.API;
using Servus_v2.Characters;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class AcceptRaise : HunterState
    {
        private int _priority;

        public AcceptRaise(Character Character, Options options, Taskstate Taskstate)
                    : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && Character.Status == EntityStatus.Dead || Character.Status == EntityStatus.DeadEngaged
                                          && Options.AcceptRaise
                                          && Api.Menu.IsMenuOpen
                                          && Character.SafeToGetUP
                                          && Api.Menu.HelpName == "Revival"
                                          && Api.Menu.MenuIndex == 1;

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
                Log.AddDebugText(TC.rtbDebug, "Accepting raise.");
                Api.ThirdParty.KeyPress(Keys.NUMPADENTER);
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}