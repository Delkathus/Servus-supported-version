using Servus_v2.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class BUFFS : HunterState
    {
        #region Fields

        private int _priority;
        private DateTime Delay;

        #endregion Fields

        #region Constructors

        public BUFFS(Character Character, Options options, Taskstate Taskstate)
                    : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        #endregion Constructors

        #region Properties

        public override int Frequency => 0;
        private LinkedList<PLAction> actionPriorities = new LinkedList<PLAction>();

        public override bool NeedToRun => Enabled
                                          && !Character.Busy
                                          && !Character.IsMoving
                && !Character.IsSilenced
                                          && selfBuffs(actionPriorities);

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

        public bool selfBuffs(LinkedList<PLAction> actions)
        {
            if (!Character.Busy)
            {
                SpellList[] orderToCheck = new SpellList[43];
                int indexOrder = 0;

                // This list is in priority order, if enabled and effected by a status it will try to
                // go from top to bottom removing debuffs
                orderToCheck[0] = TC.AquaveilCB.Checked ? SpellList.Aquaveil : SpellList.Unknown;
                orderToCheck[1] = TC.BlinkCB.Checked ? SpellList.Blink : SpellList.Unknown;
                orderToCheck[2] = TC.HasteCB.Checked ? SpellList.Haste : SpellList.Unknown;
                orderToCheck[3] = TC.Haste2CB.Checked ? SpellList.Haste_II : SpellList.Unknown;
                orderToCheck[4] = TC.PhalanxCB.Checked ? SpellList.Phalanx : SpellList.Unknown;
                orderToCheck[5] = TC.ProtectCB.Checked ? SpellList.Protect : SpellList.Unknown;
                orderToCheck[6] = TC.Protect2CB.Checked ? SpellList.Protect_II : SpellList.Unknown;
                orderToCheck[7] = TC.Protect3CB.Checked ? SpellList.Protect_III : SpellList.Unknown;
                orderToCheck[8] = TC.Protect4CB.Checked ? SpellList.Protect_IV : SpellList.Unknown;
                orderToCheck[9] = TC.Protect5CB.Checked ? SpellList.Protect_V : SpellList.Unknown;
                orderToCheck[10] = TC.Refresh1CB.Checked ? SpellList.Refresh : SpellList.Unknown;
                orderToCheck[11] = TC.Refresh2CB.Checked ? SpellList.Refresh_II : SpellList.Unknown;
                orderToCheck[12] = TC.Refresh3CB.Checked ? SpellList.Refresh_III : SpellList.Unknown;
                orderToCheck[13] = TC.Regen1CB.Checked ? SpellList.Regen : SpellList.Unknown;
                orderToCheck[14] = TC.Regen2CB.Checked ? SpellList.Regen_II : SpellList.Unknown;
                orderToCheck[15] = TC.Regen3CB.Checked ? SpellList.Regen_III : SpellList.Unknown;
                orderToCheck[16] = TC.Regen4CB.Checked ? SpellList.Regen_IV : SpellList.Unknown;
                orderToCheck[17] = TC.Regen5CB.Checked ? SpellList.Regen_V : SpellList.Unknown;
                orderToCheck[18] = TC.Reraise1CB.Checked ? SpellList.Reraise : SpellList.Unknown;
                orderToCheck[19] = TC.Reraise2CB.Checked ? SpellList.Reraise_II : SpellList.Unknown;
                orderToCheck[20] = TC.Reraise3CB.Checked ? SpellList.Reraise_III : SpellList.Unknown;
                orderToCheck[21] = TC.Reraise4CB.Checked ? SpellList.Reraise_IV : SpellList.Unknown;
                orderToCheck[22] = TC.Shell1CB.Checked ? SpellList.Shell : SpellList.Unknown;
                orderToCheck[23] = TC.Shell2CB.Checked ? SpellList.Shell_II : SpellList.Unknown;
                orderToCheck[24] = TC.Shell3CB.Checked ? SpellList.Shell_III : SpellList.Unknown;
                orderToCheck[25] = TC.Shell4CB.Checked ? SpellList.Shell_IV : SpellList.Unknown;
                orderToCheck[26] = TC.Shell5CB.Checked ? SpellList.Shell_V : SpellList.Unknown;
                orderToCheck[27] = TC.StoneskinCB.Checked ? SpellList.Stoneskin : SpellList.Unknown;
                orderToCheck[28] = TC.BlazeSpikesCB.Checked ? SpellList.Blaze_Spikes : SpellList.Unknown;
                orderToCheck[29] = TC.IceSPikesCB.Checked ? SpellList.Ice_Spikes : SpellList.Unknown;
                orderToCheck[30] = TC.ShockSpikesCB.Checked ? SpellList.Shock_Spikes : SpellList.Unknown;
                orderToCheck[31] = TC.ProtectraCB.Checked ? SpellList.Protectra : SpellList.Unknown;
                orderToCheck[32] = TC.Protectra2CB.Checked ? SpellList.Protectra_II : SpellList.Unknown;
                orderToCheck[33] = TC.protectra3CB.Checked ? SpellList.Protectra_III : SpellList.Unknown;
                orderToCheck[34] = TC.Protectra4CB.Checked ? SpellList.Protectra_IV : SpellList.Unknown;
                orderToCheck[35] = TC.Protectra5CB.Checked ? SpellList.Protectra_V : SpellList.Unknown;
                orderToCheck[36] = TC.ShellraCB.Checked ? SpellList.Shellra : SpellList.Unknown;
                orderToCheck[37] = TC.Shellra2CB.Checked ? SpellList.Shellra_II : SpellList.Unknown;
                orderToCheck[38] = TC.shellra3CB.Checked ? SpellList.Shellra_III : SpellList.Unknown;
                orderToCheck[39] = TC.Shellra4CB.Checked ? SpellList.Shellra_IV : SpellList.Unknown;
                orderToCheck[40] = TC.shellra5CB.Checked ? SpellList.Shellra_V : SpellList.Unknown;
                orderToCheck[41] = TC.UtsusemiNiCB.Checked ? SpellList.Utsusemi_Ni : SpellList.Unknown;
                orderToCheck[42] = TC.UtsusemiIchiCB.Checked ? SpellList.Utsusemi_Ichi : SpellList.Unknown;

                foreach (SpellList spell in orderToCheck)
                {
                    if (spell != SpellList.Unknown)
                    {
                        string _spell = null;
                        string newString = null;
                        StatusEffect effect = getStatusEffectForSpell(spell);
                        if (spell == SpellList.Utsusemi_Ni || spell == SpellList.Utsusemi_Ichi || spell == SpellList.Utsusemi_San)
                        {
                            _spell = spell.ToString();
                            newString = _spell.Replace("_", ": ");
                        }
                        else
                        {
                            _spell = spell.ToString().Replace('_', ' ').ToString();
                        }
                        if (newString != null)
                        {
                            _spell = newString;
                        }
                        if (Character.SpellRecast(_spell.ToString()) == 0)
                        {
                            if ((effect == StatusEffect.Unknown) || !Character.IsAfflicted((EliteMMO.API.StatusEffect)effect))
                            {
                                if (Character.spells.SpellMpCost(_spell.ToString()))
                                {
                                    {
                                        double scaledPriority = PLAction.calculateScaledPriority(PLAction.type.Buff, 1, indexOrder, effect, (int)Character.Api.Player.HPMax);
                                        bool isUtsusemi = (spell == SpellList.Utsusemi_Ni) || (spell == SpellList.Utsusemi_Ichi);
                                        bool hasShadows = (Character.IsAfflicted(EliteMMO.API.StatusEffect.Utsusemi_1_Shadow_Left) || Character.IsAfflicted((EliteMMO.API.StatusEffect.Utsusemi_2_Shadows_Left)) || (Character.IsAfflicted(EliteMMO.API.StatusEffect.Utsusemi_3_Shadows_Left) || Character.IsAfflicted((EliteMMO.API.StatusEffect.Utsusemi_4_Shadows_Left))));

                                        if (!isUtsusemi || !hasShadows)
                                        {
                                            if (actions.Count > 0)
                                            {
                                                foreach (var buff in actions.ToArray())
                                                {
                                                    if (buff.Spell != spell)
                                                    {
                                                        actions.AddLast(new PLAction(PLAction.type.Buff, scaledPriority, PLAction.target.PL, spell, effect, Character));

                                                        return true;
                                                    }
                                                }
                                            }
                                            if (actions.Count == 0)
                                            {
                                                actions.AddLast(new PLAction(PLAction.type.Buff, scaledPriority, PLAction.target.PL, spell, effect, Character));
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        indexOrder++;
                    }
                }
                return false;
            }
            else
                return false;
        }

        private StatusEffect getStatusEffectForSpell(SpellList spell)
        {
            switch (spell)
            {
                case SpellList.Aquaveil: return StatusEffect.Aquaveil;
                case SpellList.Blink: return StatusEffect.Blink;
                case SpellList.Haste: return StatusEffect.Haste;
                case SpellList.Haste_II: return StatusEffect.Haste;
                case SpellList.Phalanx:
                case SpellList.Phalanx_II:
                    return StatusEffect.Phalanx;

                case SpellList.Protect:
                case SpellList.Protect_II:
                case SpellList.Protect_III:
                case SpellList.Protect_IV:
                case SpellList.Protect_V:
                case SpellList.Protectra:
                case SpellList.Protectra_II:
                case SpellList.Protectra_III:
                case SpellList.Protectra_IV:
                case SpellList.Protectra_V:
                    return StatusEffect.Protect;

                case SpellList.Refresh:
                case SpellList.Refresh_II:
                case SpellList.Refresh_III:
                    return StatusEffect.Refresh;

                case SpellList.Regen:
                case SpellList.Regen_II:
                case SpellList.Regen_III:
                case SpellList.Regen_IV:
                case SpellList.Regen_V:
                    return StatusEffect.Regen;

                case SpellList.Reraise:
                case SpellList.Reraise_II:
                case SpellList.Reraise_III:
                case SpellList.Reraise_IV:
                    return StatusEffect.Reraise;

                case SpellList.Shell:
                case SpellList.Shell_II:
                case SpellList.Shell_III:
                case SpellList.Shell_IV:
                case SpellList.Shell_V:
                case SpellList.Shellra:
                case SpellList.Shellra_II:
                case SpellList.Shellra_III:
                case SpellList.Shellra_IV:
                case SpellList.Shellra_V:
                    return StatusEffect.Shell;

                case SpellList.Stoneskin:
                    return StatusEffect.Stoneskin;

                case SpellList.Blaze_Spikes: return StatusEffect.Blaze_Spikes;
                case SpellList.Ice_Spikes: return StatusEffect.Ice_Spikes;
                case SpellList.Shock_Spikes: return StatusEffect.Shock_Spikes;
            }
            return StatusEffect.Unknown;
        }

        public override void Update()
        {
            try
            {
                for (int i = 0; i < actionPriorities.ToArray().Length; i++)
                {
                    if (actionPriorities.Count != 0)
                    {
                        var orderedPriorities = actionPriorities.ToList().OrderByDescending(p => p.Priority);
                        int index = 0;

                        if ((orderedPriorities.ToArray()[i].Priority > 0))
                        {
                            double waitHowLong = orderedPriorities.ToArray()[i].Do(Character.Api, Character.MonitoredAPI, Character.Tasks.Huntertask.Options.SilencedItem, Character.Tasks.Huntertask.Options.DoomItem);

                            if (waitHowLong != 0.0)
                            {
                                Delay = DateTime.Now.AddSeconds(waitHowLong);
                            }
                        }

                        foreach (PLAction action in orderedPriorities)
                        {
                            Character.Logger.AddDebugText(TC.rtbDebug, string.Format(@" {0} {1} {2} ", Character.MonitoredAPI.Player.Name, Character.Api.Player.Name, index));
                            index++;
                        }
                    }
                    actionPriorities.Clear();
                }
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