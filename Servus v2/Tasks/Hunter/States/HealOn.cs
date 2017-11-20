using EliteMMO.API;
using Servus_v2.Characters;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class HealOn : HunterState
    {
        private int _priority;

        public HealOn(Character Character, Options options, Taskstate Taskstate)
            : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && NeedToHeal;

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        private bool NeedToHeal => Character.Status == EntityStatus.Idle
                                   && !Character.Target.HasAggro()
                                   && (RestOnWeakened || RestOnLowMp || RestOnLowHp);

        private bool RestOnLowHp => Options.RestOnLowHpEnabled && Api.Player.HPP <= Options.LowHpValue;

        private bool RestOnLowMp => Options.RestOnLowMpEnabled && Api.Player.MPP <= Options.LowMpValue;

        private bool RestOnWeakened => Options.RestOnWeakenedEnabled && Character.IsAfflicted(EliteMMO.API.StatusEffect.Weakness);

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
                Log.AddDebugText(TC.rtbDebug, "Healing");

                Api.ThirdParty.SendString("/heal on");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}