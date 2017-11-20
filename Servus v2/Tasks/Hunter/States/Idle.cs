using Servus_v2.Characters;
using System;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class Idle : State
    {
        public Idle(Character Character)
            : base(Character)
        {
            Priority = int.MinValue;
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun
        {
            get
            {
                if (OnPartyDisband)
                {
                    var solo = true;
                    for (byte i = 1; i < 16; i++)
                    {
                        if (Api.Party.GetPartyMember(i).Active != 0)
                        {
                            solo = false;
                        }
                        else
                            solo = true;
                    }

                    Priority = solo ? int.MaxValue : int.MinValue;
                    return solo;
                }
                if (OnDeath)
                {
                    Priority = Character.IsDead ? int.MaxValue : int.MinValue;
                    return Character.IsDead;
                }

                Priority = int.MaxValue;
                return true;
            }
        }

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
                Log.AddDebugText(TC.rtbDebug, string.Format("{0} Idling..", Api.Player.Name));
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}