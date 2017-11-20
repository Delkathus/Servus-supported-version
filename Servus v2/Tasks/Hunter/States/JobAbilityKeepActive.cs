using Servus_v2.Characters;
using System;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class JobAbilityKeepActive : HunterState
    {
        #region Fields

        private int _priority;

        #endregion Fields

        #region Constructors

        public JobAbilityKeepActive(Character Character, Options options, Taskstate Taskstate)
                    : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        #endregion Constructors

        #region Properties

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && !Character.Busy
                && !Character.IsAfflicteAmnesia
                                          && !Character.IsMoving
                                          && Character._Abilities.AbilityNeeded() != "none";

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        #endregion Properties

        #region Methods

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
                if (Character._Abilities.AbilityNeeded() != "none")
                {
                    var ja = Character._Abilities.AbilityNeeded();
                    Log.AddDebugText(TC.rtbDebug, string.Format("Need to use {0}", ja));
                    var command = Character._Abilities.AbilityCommand(ja);
                    Log.AddDebugText(TC.rtbDebug, string.Format("Using {0}", ja));
                    Api.ThirdParty.SendString(command);
                }
                else
                    Exit();
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }

        #endregion Methods
    }
}