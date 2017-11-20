using EliteMMO.API;
using Servus_v2.Characters;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class HealOff : HunterState
    {
        private int _priority;

        public HealOff(Character Character, Options options, Taskstate Taskstate)
            : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun
        {
            get
            {
                Hpup = new Random().Next(Options.TargetHpp, 99);
                Mpup = new Random().Next(Options.TargetMpp, 99);
                return Enabled
                       && Character.Status == EntityStatus.Healing
                       && Api.Player.HPP >= Hpup
                       && Api.Player.MPP >= Mpup
                       && !Character.IsAfflicted(EliteMMO.API.StatusEffect.Weakness);
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
                Log.AddDebugText(TC.rtbDebug, @"Standing Up");

                Api.ThirdParty.SendString("/heal off");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}