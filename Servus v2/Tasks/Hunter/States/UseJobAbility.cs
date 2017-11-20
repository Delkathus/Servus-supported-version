using FFACETools;
using Gambits.Model.Characters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gambits.Model.Tasks.Battle.States
{
    class UseJobAbility : BattleState
    {
        public string Type { get; set; }

        public string AbilityName { get; set; }

        public string TargetType { get; set; }

        public List<StatusEffect> StatusEffect { get; set; }

        public double? Delay { get; set; }
        
        private DateTime ReuseTime { get; set; }

        public Status Status { get; set; }

        public bool PetRequired { get; set; }

        public UseJobAbility(Character character, Options options, TaskState taskState)
            : base(character, options, taskState)
        {
            Enabled = true;
            Status = Status.Unknown;
        }

        public override int Frequency
        {
            get
            {
                return 0;
            }
        }

        private int _priority;
        public override int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = int.MaxValue - value;
            }
        }

        public override bool NeedToRun
        {
            get
            {
                return Enabled
                       && (FFACE.Player.Status == Status || Status == Status.Unknown)
                       && CanUseAbility()
                       && NeedToUse;
            }
        }

        public override void Enter()
        {
            Log.WriteLine(string.Format("Entering {0} State", GetType().Name));
        }

        public override void Update()
        {
            Log.WriteLine("Using job ability");
            
            FFACE.Windower.SendString(string.Format(@"{0} ""{1}"" {2}", Type, AbilityName, TargetType));

            if (Delay != null)
            {
                ReuseTime =  DateTime.Now.AddMinutes(Delay.Value);
            }
        }

        public override void Exit()
        {
            Log.WriteLine(string.Format("Exiting {0} State", GetType().Name));
        }

        private bool NeedToUse
        {
            get
            {
                if ((PetRequired && Character.PetId == 0)
                    || (DateTime.Now < ReuseTime)
                    ||(Character.FFACE.Player.Status != Status && Status != Status.Unknown))
                {
                    return false;
                }

                if (TargetType == "<me>" && StatusEffect != null && StatusEffect.Any())
                {
                    return StatusEffect.All(status => !FFACE.Player.StatusEffects.Any(s => s == status));
                }
                return false;
            }
        }

        private bool CanUseAbility()
        {
            if (!Character.Abilities.CanUseAbility(AbilityName))
            {
                return false;
            }

            if (Delay != null)
            {
                return DateTime.Now > ReuseTime;
            }

            var name = Abilities.GetAbilityName(string.Format(@"{0} ""{1}"" {2}", Type, AbilityName, TargetType));
            var id = (byte)Enum.Parse(typeof(AbilityList), name);
            var ability = (AbilityList)Enum.Parse(typeof(AbilityList), id.ToString(CultureInfo.InvariantCulture), true);
            return FFACE.Timer.GetAbilityRecast(ability) == 0;
        }
    }
}
