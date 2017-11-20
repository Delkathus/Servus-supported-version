using Servus_v2.FFXi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servus_v2.Characters
{
    public class Abilities
    {
        public Abilities(Character chars)
        {
            Character = chars;

            ParseAbilitiesFile();
        }

        public Character Character { get; set; }

        public SortedDictionary<string, List<Ability>> JobsAbilityListDictionary = new SortedDictionary<string, List<Ability>>();

        public SortedDictionary<string, Ability> AbilityDictionary = new SortedDictionary<string, Ability>();

        /// <summary>
        /// Checks if Character has enough MP.
        /// </summary>
        /// <param name="abilityName">Name of the ability.</param>
        /// <returns></returns>
        public bool AbilityMp(string abilityName)
        {
            var mainQuery = from ability in AbilityDictionary
                            where string.Compare(ability.Value.En.ToString(), abilityName, StringComparison.OrdinalIgnoreCase) == 0

                                  && ability.Value.Mp_cost <= Character.Api.Player.MP
                            select ability;

            if (mainQuery.Any())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if character has enough TP.
        /// </summary>
        /// <param name="abilityName">Name of the ability.</param>
        /// <returns></returns>
        public bool AbilityTp(string abilityName)
        {
            var mainQuery = from ability in AbilityDictionary
                            where string.Compare(ability.Value.En.ToString(), abilityName, StringComparison.OrdinalIgnoreCase) == 0

                                  && ability.Value.Tp_cost <= Character.Api.Player.TP
                            select ability;

            if (mainQuery.Any())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the type of the ja target.
        /// </summary>
        /// <param name="abilityname">The abilityname.</param>
        /// <returns></returns>
        public string GetJATargetType(string abilityname)
        {
            if (Character._Abilities.JobsAbilityListDictionary.ContainsKey(abilityname))
            {
                List<Ability> abilities = Character._Abilities.JobsAbilityListDictionary[abilityname];

                var mainQuery = from ability in abilities
                                where string.Compare(ability.En.ToString(), abilityname, StringComparison.OrdinalIgnoreCase) == 0
                                select ability;

                foreach (var a in mainQuery)
                {
                    var jaTarget = a.Targets;
                    switch (jaTarget)
                    {
                        case 1:
                            return "<me>";

                        case 32:
                            return "<t>";

                        case 3:
                            return "<me>";

                        case 5:
                            return "<me>";

                        case 4:
                            return "<p1>";

                        case 63:
                            return "<me>";
                    }
                }
            }
            return "<me>";
        }

        /// <summary>
        /// Parses the abilities file.
        /// </summary>
        private void ParseAbilitiesFile()
        {
            for (uint x = 0; x < 2000; x++)
            {
                Ability ability = new Ability();
                ability.En = Character.Api.Resources.GetAbility(x).Name[0];

                if (ability.En != "")
                {
                    var type = Character.Api.Resources.GetAbility(x).Type;

                    ability.Id = Character.Api.Resources.GetAbility(x).ID;
                    ability.Targets = Character.Api.Resources.GetAbility(x).ValidTargets;
                    ability.Mp_cost = Character.Api.Resources.GetAbility(x).MP;
                    ability.Tp_cost = Character.Api.Resources.GetAbility(x).TP;
                    ability.Recast_id = Character.Api.Resources.GetAbility(x).TimerID;
                    if (!AbilityDictionary.ContainsKey(ability.En) && type != 4)
                    {
                        switch (type)
                        {
                            case 0:
                                ability.Prefix = "/ja";
                                break;

                            case 1:
                                ability.Prefix = "/ja";
                                break;

                            case 2:
                                ability.Prefix = "/ma";
                                break;

                            case 3:
                                ability.Prefix = "/ja";
                                break;

                            case 5:
                                ability.Prefix = "/ja";
                                break;

                            case 6:
                                ability.Prefix = "/pet";
                                break;

                            case 7:
                                ability.Prefix = "/ja";
                                break;
                        }
                        AbilityDictionary.Add(ability.En, ability);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the name of the ability.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static string GetAbilityName(string command)
        {
            var start = 5;
            if (command.Substring(0, 4) == "/nin")
            {
                start = 6;
            }

            var lastIndex = command.LastIndexOf("\"", StringComparison.Ordinal);
            return command.Substring(start, lastIndex - start)
                          .Replace(" ", "_")
                          .Replace("'", "")
                          .Replace(":", "");
        }

        /// <summary>
        /// Gets the ability recast.
        /// </summary>
        /// <param name="AbilityName">Name of the ability.</param>
        /// <returns></returns>
        public int GetAbilityRecast(string AbilityName)
        {
            int id = Character.Api.Resources.GetAbility(AbilityName, 0).TimerID;
            var IDs = Character.Api.Recast.GetAbilityIds();
            for (var x = 0; x < IDs.Count; x++)
            {
                if (IDs[x] == id)
                    return Character.Api.Recast.GetAbilityRecast(x);
            }
            return 0;
        }

        public List<EliteMMO.API.StatusEffect> Seffect = new List<EliteMMO.API.StatusEffect>();

        /// <summary>
        /// Checks if ability buffs are needed during fight only.
        /// </summary>
        /// <returns></returns>
        public string FightAbilityNeeded()
        {
            foreach (var buff in Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights)
            {
                string[] JA = buff.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                var prefix = JA[0].ToString();
                var ja1 = JA[1].ToString();
                var ja2 = JA[1].Replace(' ', '_').ToString();
                var jatarget = JA[2].ToString();
                if (Enum.IsDefined(typeof(EliteMMO.API.StatusEffect), ja2.ToString()))
                {
                    var AbilityName = (EliteMMO.API.StatusEffect)Enum.Parse(typeof(EliteMMO.API.StatusEffect), ja2.ToString());

                    if (!Character.IsAfflictedJA(AbilityName))
                    {
                        if (Character._Abilities.GetAbilityRecast(ja1.ToString()) == 0 && AbilityMp(ja1) && AbilityTp(ja1))
                        {
                            return ja1.ToString();
                        }
                    }
                }
                else if (!Enum.IsDefined(typeof(EliteMMO.API.StatusEffect), ja2.ToString()))
                {
                    if (Character._Abilities.GetAbilityRecast(ja1.ToString()) == 0 && AbilityMp(ja1) && AbilityTp(ja1))
                    {
                        return ja1.ToString();
                    }
                }
            }
            return "none";
        }

        /// <summary>
        /// Checks if ability buffs are needed, keep active.
        /// </summary>
        /// <returns></returns>
        public string AbilityNeeded()
        {
            foreach (var buff in Character.Tasks.Huntertask.Options.JobAbilityKeepActive)
            {
                string[] JA = buff.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                var prefix = JA[0].ToString();
                var ja1 = JA[1].ToString();
                var ja2 = JA[1].Replace(' ', '_').ToString();
                var jatarget = JA[2].ToString();
                if (Enum.IsDefined(typeof(EliteMMO.API.StatusEffect), ja2.ToString()))
                {
                    var AbilityName = (EliteMMO.API.StatusEffect)Enum.Parse(typeof(EliteMMO.API.StatusEffect), ja2.ToString());

                    if (!Character.IsAfflictedJA(AbilityName))
                    {
                        if (Character._Abilities.GetAbilityRecast(ja1.ToString()) == 0 && AbilityMp(ja1) && AbilityTp(ja1))
                        {
                            return ja1.ToString();
                        }
                    }
                }
                else if (!Enum.IsDefined(typeof(EliteMMO.API.StatusEffect), ja2.ToString()))
                {
                    if (Character._Abilities.GetAbilityRecast(ja1.ToString()) == 0 && AbilityMp(ja1) && AbilityTp(ja1))
                    {
                        return ja1.ToString();
                    }
                }
            }
            return "none";
        }

        /// <summary>
        /// Returns the abilitty command. Kepp active.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string AbilityCommand(string name)
        {
            var match = Character.Tasks.Huntertask.Options.JobAbilityKeepActive
            .FirstOrDefault(stringToCheck => stringToCheck.Contains(name));

            if (match != null)
            {
                return match.ToString();
            }

            return null;
        }

        /// <summary>
        /// Returns the ability command,fight only.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string AbilityCommandFightOnly(string name)
        {
            var match = Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights
            .FirstOrDefault(stringToCheck => stringToCheck.Contains(name));

            if (match != null)
            {
                return match.ToString();
            }

            return null;
        }
    }
}