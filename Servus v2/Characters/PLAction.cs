using EliteMMO.API;
using System;
using System.Windows.Forms;

namespace Servus_v2.Characters
{
    #region PLAction

    public class PLAction
    {
        public enum type : short { Cure = 0, Buff = 1, Debuff = 2 }

        public enum target : byte { MonitorPartyMember = 0, PL = 18, Monitor = 19 }

        public type Action { get; set; }
        public double Priority { get; set; }
        public target Focus { get; set; }

        public SpellList Spell { get; set; }
        public StatusEffect Effect { get; set; }
        public Character character { get; set; }

        public PLAction(type action, double priority, target focus, SpellList spell, StatusEffect effect, Character Char)
        {
            character = Char;
            Action = action;
            Priority = priority;
            Focus = focus;
            Spell = spell;
            Effect = effect;
        }

        //Given a spell, what is the string used to represent it
        public string getSpellName(SpellList spell)
        {
            switch (spell)
            {
                case SpellList.Cursna: return "Cursna";
                case SpellList.Silena: return "Silena";
                case SpellList.Stona: return "Stona";
                case SpellList.Paralyna: return "Paralyna";
                case SpellList.Poisona: return "Poisona";
                case SpellList.Erase: return "Erase";
                case SpellList.Blindna: return "Blindna";
                case SpellList.Viruna: return "Viruna";

                case SpellList.Cure: return "Cure";
                case SpellList.Cure_II: return "Cure II";
                case SpellList.Cure_III: return "Cure III";
                case SpellList.Cure_IV: return "Cure IV";
                case SpellList.Cure_V: return "Cure V";
                case SpellList.Cure_VI: return "Cure VI";
                case SpellList.Curaga: return "Curaga";
                case SpellList.Curaga_II: return "Curaga II";
                case SpellList.Curaga_III: return "Curaga III";
                case SpellList.Curaga_IV: return "Curaga IV";
                case SpellList.Curaga_V: return "Curaga V";
                case SpellList.Haste_II: return "Haste II";
                case SpellList.Regen_IV: return "Regen IV";
                case SpellList.Regen_V: return "Regen V";
                case SpellList.Refresh_III: return "Refresh III";

                case SpellList.Aquaveil: return "Aquaveil";
                case SpellList.Blink: return "Blink";
                case SpellList.Haste: return "Haste";
                case SpellList.Phalanx: return "Phalanx";
                case SpellList.Phalanx_II: return "Phalanx II";
                case SpellList.Protect: return "Protect";
                case SpellList.Protect_II: return "Protect II";
                case SpellList.Protect_III: return "Protect III";
                case SpellList.Protect_IV: return "Protect IV";
                case SpellList.Protect_V: return "Protect V";
                case SpellList.Refresh: return "Refresh";
                case SpellList.Refresh_II: return "Refresh II";
                case SpellList.Regen: return "Regen";
                case SpellList.Regen_II: return "Regen II";
                case SpellList.Regen_III: return "Regen III";
                case SpellList.Reraise: return "Reraise";
                case SpellList.Reraise_II: return "Reraise II";
                case SpellList.Reraise_III: return "Reraise III";
                case SpellList.Shell: return "Shell";
                case SpellList.Shell_II: return "Shell II";
                case SpellList.Shell_III: return "Shell III";
                case SpellList.Shell_IV: return "Shell IV";
                case SpellList.Shell_V: return "Shell V";
                case SpellList.Stoneskin: return "Stoneskin";
                case SpellList.Blaze_Spikes: return "Blaze spikes";
                case SpellList.Ice_Spikes: return "Ice spikes";
                case SpellList.Shock_Spikes: return "Shock spikes";
                case SpellList.Shellra: return "Shellra";
                case SpellList.Shellra_II: return "Shellra II";
                case SpellList.Shellra_III: return "Shellra III";
                case SpellList.Shellra_IV: return "Shellra IV";
                case SpellList.Shellra_V: return "Shellra V";
                case SpellList.Protectra: return "Protectra";
                case SpellList.Protectra_II: return "Protectra II";
                case SpellList.Protectra_III: return "Protectra III";
                case SpellList.Protectra_IV: return "Protectra IV";
                case SpellList.Protectra_V: return "Protectra V";
                case SpellList.Utsusemi_Ni: return "Utsusemi: Ni";
                case SpellList.Utsusemi_Ichi: return "Utsusemi: Ichi";
            }
            return "UNKNOWN";
        }

        private string getFocusName(EliteAPI monitorAPI)
        {
            switch (Focus)
            {
                case target.MonitorPartyMember + 0:
                case target.MonitorPartyMember + 1:
                case target.MonitorPartyMember + 2:
                case target.MonitorPartyMember + 3:
                case target.MonitorPartyMember + 4:
                case target.MonitorPartyMember + 5:
                case target.MonitorPartyMember + 6:
                case target.MonitorPartyMember + 7:
                case target.MonitorPartyMember + 8:
                case target.MonitorPartyMember + 9:
                case target.MonitorPartyMember + 10:
                case target.MonitorPartyMember + 11:
                case target.MonitorPartyMember + 12:
                case target.MonitorPartyMember + 13:
                case target.MonitorPartyMember + 14:
                case target.MonitorPartyMember + 15:
                case target.MonitorPartyMember + 16:
                case target.MonitorPartyMember + 17:
                    return monitorAPI.Party.GetPartyMember(Focus - target.MonitorPartyMember).Name;

                case target.PL:
                    return "<me>";

                case target.Monitor:
                    return monitorAPI.Player.Name;
            }
            return "UNKNOWN";
        }

        private string getJobAbilityName()
        {
            switch (Effect)
            {
                case StatusEffect.Afflatus_Solace: return "Afflatus Solace";
                case StatusEffect.Afflatus_Misery: return "Afflatus Misery";
                case StatusEffect.Light_Arts: return "Light Arts";
                case StatusEffect.Addendum_White: return "Addendum: White";
                case StatusEffect.Sublimation_Activated: return "Sublimation";
                case StatusEffect.Sublimation_Complete: return "Sublimation";
            }
            return "UNKNOWN";
        }

        //For a given amount of required HP percent get an importance scale from one(lowest) to three(highest).
        public static int GetImportanceScaleForHPNeeded(int hpp)
        {
            if (hpp < 40)
            {
                return 3;
            }
            if (hpp < 70)
            {
                return 2;
            }
            return 1;
        }

        public static double calculateScaledPriority(type action, int basePriority, int indexOrder, StatusEffect effect, int maxhp)
        {
            switch (action)
            {
                case type.Cure:
                    return 0.0;

                case type.Debuff:
                    {
                        double scaledPriority = (0.5 + (0.5 / (indexOrder + 1))) * basePriority;

                        switch (effect)
                        {
                            case StatusEffect.Doom:
                                scaledPriority = (double)(basePriority * (maxhp * 0.75) * GetImportanceScaleForHPNeeded(25));
                                break;

                            case StatusEffect.Petrification:
                                scaledPriority = (double)(basePriority * (maxhp * 0.60) * GetImportanceScaleForHPNeeded(40));
                                break;

                            case StatusEffect.Curse:
                                scaledPriority = (double)(basePriority * (maxhp * 0.50) * GetImportanceScaleForHPNeeded(50));
                                break;

                            case StatusEffect.Curse2:
                                scaledPriority = (double)(basePriority * (maxhp * 0.50) * GetImportanceScaleForHPNeeded(50));
                                break;

                            case StatusEffect.Max_HP_Down:
                                scaledPriority = (double)(basePriority * (maxhp * 0.50) * GetImportanceScaleForHPNeeded(50));
                                break;

                            case StatusEffect.Silence:
                                scaledPriority = (double)(basePriority * (maxhp * 0.10) * GetImportanceScaleForHPNeeded(90));
                                break;
                        }
                        return scaledPriority;
                    }
                case type.Buff:
                    {
                        double scaledPriority = (0.0 + (0.5 / (indexOrder + 1))) * basePriority;

                        switch (effect)
                        {
                            case StatusEffect.Phalanx:
                                scaledPriority = (double)(basePriority * (maxhp * 0.05) * GetImportanceScaleForHPNeeded(95));
                                break;
                        }

                        return scaledPriority;
                    }
            }
            return 0;
        }

        public static double Distance(EliteAPI monitorAPI, byte member, EliteAPI plAPI)
        {
            if (monitorAPI.Party.GetPartyMember(member).Zone != plAPI.Player.ZoneId)
            {
                return System.Double.MaxValue;
            }
            double x1 = plAPI.Player.X;
            double y1 = plAPI.Player.Y;
            double z1 = plAPI.Player.Z;
            double x2 = plAPI.Entity.GetEntity(Convert.ToInt32(monitorAPI.Party.GetPartyMember(member).ID)).X;
            double y2 = plAPI.Entity.GetEntity(Convert.ToInt32(monitorAPI.Party.GetPartyMember(member).ID)).Y;
            double z2 = plAPI.Entity.GetEntity(Convert.ToInt32(monitorAPI.Party.GetPartyMember(member).ID)).Z;

            if (((x1 == 0.0) && (y1 == 0.0) && (z1 == 0.0)) ||
        ((x2 == 0.0) && (y2 == 0.0) && (z2 == 0.0)))
            {
                return System.Double.MaxValue;
            }

            x1 = x1 - x2;
            y1 = y1 - y2;
            z1 = z1 - z2;

            if ((x1 == 0.0) && (y1 == 0.0) && (z1 == 0.0))
            {
                return 0.0;
            }

            return Math.Sqrt((x1 * x1) + (y1 * y1) + (z1 * z1));
        }

        public void AddToDebug(RichTextBox rtb, EliteAPI monitorAPI, EliteAPI plAPI, string monitorSilenceItem, string monitorDoomItem, int index)
        {
            switch (Action)
            {
                case PLAction.type.Cure:

                    character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Entry({0}) Cure ""{1}"" casting ""{2}"" priority {3} mdist {4}", index, getFocusName(monitorAPI), getSpellName(Spell), Priority, plAPI.Entity.GetEntity((int)plAPI.Party.GetPartyMember(0).TargetIndex).Distance));
                    break;

                case PLAction.type.Buff:
                    if (Spell != SpellList.Unknown)
                    {
                        character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Entry({0}) Buff ""{1}"" casting ""{2}"" priority {3} mdist {4}", index, getFocusName(monitorAPI), getSpellName(Spell), Priority, plAPI.Entity.GetEntity((int)plAPI.Party.GetPartyMember(0).TargetIndex).Distance));
                    }
                    else
                    {
                        character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Entry({0}) Buff ""{1}"" using job ability ""{2}"" priority {3} mdist {4}", index, getFocusName(monitorAPI), getJobAbilityName(), Priority, plAPI.Entity.GetEntity((int)plAPI.Party.GetPartyMember(0).TargetIndex).Distance));
                    }
                    break;

                case PLAction.type.Debuff:
                    if ((Focus == PLAction.target.PL) && (Spell == SpellList.Silena))
                    {
                        character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Entry({0}) Debuff ""{1}"" using item ""{2}"" priority {3} mdist {4}", index, getFocusName(monitorAPI), monitorSilenceItem, Priority, plAPI.Entity.GetEntity((int)plAPI.Party.GetPartyMember(0).TargetIndex).Distance));
                    }
                    else
                    {
                        character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Entry({0}) Debuff ""{1}"" casting ""{2}"" priority {3} mdist {4}", index, getFocusName(monitorAPI), getSpellName(Spell), Priority, plAPI.Entity.GetEntity((int)plAPI.Party.GetPartyMember(0).TargetIndex).Distance));
                    }
                    if ((Focus == PLAction.target.PL) && (Spell == SpellList.Cursna))
                    {
                        character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Entry({0}) Debuff ""{1}"" using item ""{2}"" priority {3} mdist {4}", index, getFocusName(monitorAPI), monitorSilenceItem, Priority, plAPI.Entity.GetEntity((int)plAPI.Party.GetPartyMember(0).TargetIndex).Distance));
                    }
                    else
                    {
                        character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Entry({0}) Debuff ""{1}"" casting ""{2}"" priority {3} mdist {4}", index, getFocusName(monitorAPI), getSpellName(Spell), Priority, plAPI.Entity.GetEntity((int)plAPI.Party.GetPartyMember(0).TargetIndex).Distance));
                    }
                    break;
            }
        }

        public double Do(EliteAPI plAPI, EliteAPI monitorAPI, string monitorSilenceItem, string monitorDoomItem)
        {
            if (!character.Busy)
            {
                switch (Action)
                {
                    case type.Cure:

                        plAPI.ThirdParty.SendString(string.Format(@"/ma ""{0}"" {1}", getSpellName(Spell), getFocusName(monitorAPI)));
                        break;

                    case type.Buff:
                        if (Spell != SpellList.Unknown)
                        {
                            plAPI.ThirdParty.SendString(string.Format(@"/ma ""{0}"" {1}", getSpellName(Spell), getFocusName(monitorAPI)));
                            break;
                        }
                        else
                        {
                            plAPI.ThirdParty.SendString(string.Format(@"/ja ""{0}"" {1}", getJobAbilityName(), getFocusName(monitorAPI)));
                        }
                        break;

                    case type.Debuff:
                        if ((Focus == target.PL) && (Spell == SpellList.Silena && character.Tasks.Huntertask.Options.useSilenceItem))
                        {
                            plAPI.ThirdParty.SendString("/item \"" + monitorSilenceItem + "\" <me>");
                        }
                        else
                        {
                            plAPI.ThirdParty.SendString(string.Format(@"/ma ""{0}"" {1}", getSpellName(Spell), getFocusName(monitorAPI)));
                        }
                        if ((Focus == target.PL) && (Spell == SpellList.Cursna && character.Tasks.Huntertask.Options.useDoomItem))
                        {
                            plAPI.ThirdParty.SendString("/item \"" + monitorDoomItem + "\" <me>");
                        }
                        else
                        {
                            plAPI.ThirdParty.SendString(string.Format(@"/ma ""{0}"" {1}", getSpellName(Spell), getFocusName(monitorAPI)));
                        }
                        break;

                    default:
                        return 0.0;
                }
            }
            return 1.5;
        }
    }

    #endregion PLAction
}