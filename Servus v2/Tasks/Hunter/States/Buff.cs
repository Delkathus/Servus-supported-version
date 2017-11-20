using Servus_v2.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class Buff : HunterState
    {
        #region Fields

        private int _priority;
        private DateTime Delay;

        #endregion Fields

        #region Constructors

        public Buff(Character Character, Options options, Taskstate Taskstate)
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
        public void selfBuffs(LinkedList<PLAction> actions)
        {
            if (cha)
            {
                SpellList[] orderToCheck = new SpellList[39];
                int indexOrder = 0;

                // This list is in priority order, if enabled and effected by a status it will try to go from top to bottom removing debuffs
                orderToCheck[0] = AquaveilCB.Checked ? SpellList.Aquaveil : SpellList.Unknown;
                orderToCheck[1] = BlinkCB.Checked ? SpellList.Blink : SpellList.Unknown;
                orderToCheck[2] = HasteCB.Checked ? SpellList.Haste : SpellList.Unknown;
                orderToCheck[3] = PhalanxCB.Checked ? SpellList.Phalanx : SpellList.Unknown;
                orderToCheck[4] = ProtectCB.Checked ? SpellList.Protect : SpellList.Unknown;
                orderToCheck[5] = Protect2CB.Checked ? SpellList.Protect_II : SpellList.Unknown;
                orderToCheck[6] = Protect3CB.Checked ? SpellList.Protect_III : SpellList.Unknown;
                orderToCheck[7] = Protect4CB.Checked ? SpellList.Protect_IV : SpellList.Unknown;
                orderToCheck[8] = Protect5CB.Checked ? SpellList.Protect_V : SpellList.Unknown;
                orderToCheck[9] = Refresh1CB.Checked ? SpellList.Refresh : SpellList.Unknown;
                orderToCheck[10] = Refresh2CB.Checked ? SpellList.Refresh_II : SpellList.Unknown;
                orderToCheck[11] = Regen1CB.Checked ? SpellList.Regen : SpellList.Unknown;
                orderToCheck[12] = Regen2CB.Checked ? SpellList.Regen_II : SpellList.Unknown;
                orderToCheck[13] = Regen3CB.Checked ? SpellList.Regen_III : SpellList.Unknown;
                orderToCheck[14] = Regen4CB.Checked ? SpellList.Regen_IV : SpellList.Unknown;
                orderToCheck[15] = Reraise1CB.Checked ? SpellList.Reraise : SpellList.Unknown;
                orderToCheck[16] = Reraise2CB.Checked ? SpellList.Reraise_II : SpellList.Unknown;
                orderToCheck[17] = Reraise3CB.Checked ? SpellList.Reraise_III : SpellList.Unknown;
                orderToCheck[18] = Shell1CB.Checked ? SpellList.Shell : SpellList.Unknown;
                orderToCheck[19] = Shell2CB.Checked ? SpellList.Shell_II : SpellList.Unknown;
                orderToCheck[20] = Shell3CB.Checked ? SpellList.Shell_III : SpellList.Unknown;
                orderToCheck[21] = Shell4CB.Checked ? SpellList.Shell_IV : SpellList.Unknown;
                orderToCheck[22] = Shell5CB.Checked ? SpellList.Shell_V : SpellList.Unknown;
                orderToCheck[23] = StoneskinCB.Checked ? SpellList.Stoneskin : SpellList.Unknown;
                orderToCheck[24] = BlazeSpikesCB.Checked ? SpellList.Blaze_Spikes : SpellList.Unknown;
                orderToCheck[25] = IceSPikesCB.Checked ? SpellList.Ice_Spikes : SpellList.Unknown;
                orderToCheck[26] = ShockSpikesCB.Checked ? SpellList.Shock_Spikes : SpellList.Unknown;
                orderToCheck[27] = ProtectraCB.Checked ? SpellList.Protectra : SpellList.Unknown;
                orderToCheck[28] = Protectra2CB.Checked ? SpellList.Protectra_II : SpellList.Unknown;
                orderToCheck[29] = protectra3CB.Checked ? SpellList.Protectra_III : SpellList.Unknown;
                orderToCheck[30] = Protectra4CB.Checked ? SpellList.Protectra_IV : SpellList.Unknown;
                orderToCheck[31] = Protectra5CB.Checked ? SpellList.Protectra_V : SpellList.Unknown;
                orderToCheck[32] = ShellraCB.Checked ? SpellList.Shellra : SpellList.Unknown;
                orderToCheck[33] = Shellra2CB.Checked ? SpellList.Shellra_II : SpellList.Unknown;
                orderToCheck[34] = shellra3CB.Checked ? SpellList.Shellra_III : SpellList.Unknown;
                orderToCheck[35] = Shellra4CB.Checked ? SpellList.Shellra_IV : SpellList.Unknown;
                orderToCheck[36] = shellra5CB.Checked ? SpellList.Shellra_V : SpellList.Unknown;
                orderToCheck[37] = UtsusemiNiCB.Checked ? SpellList.Utsusemi_Ni : SpellList.Unknown;
                orderToCheck[38] = UtsusemiIchiCB.Checked ? SpellList.Utsusemi_Ichi : SpellList.Unknown;

                foreach (SpellList spell in orderToCheck)
                {
                    if (spell != SpellList.Unknown)
                    {
                        StatusEffect effect = getStatusEffectForSpell(spell);
                        if ((_FFACEPL.Timer.GetSpellRecast(spell) == 0) && ((effect == StatusEffect.Unknown) || !plStatusCheck(effect)))
                        {
                            double scaledPriority = PLAction.calculateScaledPriority(PLAction.type.Buff, 1, indexOrder, effect, _FFACEPL.Player.HPMax);
                            bool isUtsusemi = (spell == SpellList.Utsusemi_Ni) || (spell == SpellList.Utsusemi_Ichi);
                            bool hasShadows = plStatusCheck(StatusEffect.Utsusemi_1_Shadow_Left) || plStatusCheck(StatusEffect.Utsusemi_2_Shadows_Left) || plStatusCheck(StatusEffect.Utsusemi_3_Shadows_Left) || plStatusCheck(StatusEffect.Utsusemi_4_Shadows_Left);

                            if (!isUtsusemi || !hasShadows)
                            {
                                actions.AddLast(new PLAction(PLAction.type.Buff, scaledPriority, PLAction.target.PL, spell, effect));
                            }
                        }
                    }
                    indexOrder++;
                }
            }
        }

        public bool SelfDebuffs(LinkedList<PLAction> actions)
        {
            if (!Character.Busy)
            {
                StatusEffect[] orderToCheck = new StatusEffect[43];
                int indexOrder = 0;

                // This list is in priority order, if enabled and effected by a status it will try to go from top to bottom removing debuffs
                orderToCheck[0] = TC.DoomCB.Checked ? StatusEffect.Doom : StatusEffect.Unknown;
                orderToCheck[1] = StatusEffect.Unknown; //plPetrification.Checked ? StatusEffect.Petrification : StatusEffect.Unknown;
                orderToCheck[2] = TC.SilenceCB.Checked ? StatusEffect.Silence : StatusEffect.Unknown;
                orderToCheck[3] = TC.CurseCB.Checked ? StatusEffect.Curse : StatusEffect.Unknown;
                orderToCheck[4] = TC.Curse2CB.Checked ? StatusEffect.Curse2 : StatusEffect.Unknown;
                orderToCheck[5] = TC.MaxHpDownCB.Checked ? StatusEffect.Max_HP_Down : StatusEffect.Unknown;
                orderToCheck[6] = TC.MaxMpDownCB.Checked ? StatusEffect.Max_MP_Down : StatusEffect.Unknown;
                orderToCheck[7] = TC.ParalysisCB.Checked ? StatusEffect.Paralysis : StatusEffect.Unknown;
                orderToCheck[8] = TC.SlowCB.Checked ? StatusEffect.Slow : StatusEffect.Unknown;
                orderToCheck[9] = TC.PoisonCB.Checked ? StatusEffect.Poison : StatusEffect.Unknown;
                orderToCheck[10] = TC.AttackDownCB.Checked ? StatusEffect.Attack_Down : StatusEffect.Unknown;
                orderToCheck[11] = TC.BlindnessCB.Checked ? StatusEffect.Blindness : StatusEffect.Unknown;
                orderToCheck[12] = TC.BindCB.Checked ? StatusEffect.Bind : StatusEffect.Unknown;
                orderToCheck[13] = TC.WeightCB.Checked ? StatusEffect.Weight : StatusEffect.Unknown;
                orderToCheck[14] = TC.AddleCB.Checked ? StatusEffect.Addle : StatusEffect.Unknown;
                orderToCheck[15] = TC.BaneCB.Checked ? StatusEffect.Bane : StatusEffect.Unknown;
                orderToCheck[16] = TC.PlagueCB.Checked ? StatusEffect.Plague : StatusEffect.Unknown;
                orderToCheck[17] = TC.BurnCB.Checked ? StatusEffect.Burn : StatusEffect.Unknown;
                orderToCheck[18] = TC.FrostCB.Checked ? StatusEffect.Frost : StatusEffect.Unknown;
                orderToCheck[19] = TC.ChokeCB.Checked ? StatusEffect.Choke : StatusEffect.Unknown;
                orderToCheck[20] = TC.RaspCB.Checked ? StatusEffect.Rasp : StatusEffect.Unknown;
                orderToCheck[21] = TC.ShockCB.Checked ? StatusEffect.Shock : StatusEffect.Unknown;
                orderToCheck[22] = TC.DrownCB.Checked ? StatusEffect.Drown : StatusEffect.Unknown;
                orderToCheck[23] = TC.DiaCB.Checked ? StatusEffect.Dia : StatusEffect.Unknown;
                orderToCheck[24] = TC.BioCB.Checked ? StatusEffect.Bio : StatusEffect.Unknown;
                orderToCheck[25] = TC.StrDownCB.Checked ? StatusEffect.STR_Down : StatusEffect.Unknown;
                orderToCheck[26] = TC.DexDownCB.Checked ? StatusEffect.DEX_Down : StatusEffect.Unknown;
                orderToCheck[27] = TC.VitDownCB.Checked ? StatusEffect.VIT_Down : StatusEffect.Unknown;
                orderToCheck[28] = TC.AgiDownCB.Checked ? StatusEffect.AGI_Down : StatusEffect.Unknown;
                orderToCheck[29] = TC.IntDownCB.Checked ? StatusEffect.INT_Down : StatusEffect.Unknown;
                orderToCheck[30] = TC.MndDownCB.Checked ? StatusEffect.MND_Down : StatusEffect.Unknown;
                orderToCheck[31] = TC.ChrDownCB.Checked ? StatusEffect.CHR_Down : StatusEffect.Unknown;
                orderToCheck[32] = TC.AccuracyDownCB.Checked ? StatusEffect.Accuracy_Down : StatusEffect.Unknown;
                orderToCheck[33] = TC.EvasionDownCB.Checked ? StatusEffect.Evasion_Down : StatusEffect.Unknown;
                orderToCheck[34] = TC.DefenseDownCB.Checked ? StatusEffect.Defense_Down : StatusEffect.Unknown;
                orderToCheck[35] = TC.FlashCB.Checked ? StatusEffect.Flash : StatusEffect.Unknown;
                orderToCheck[36] = TC.MagicAccDownCB.Checked ? StatusEffect.Magic_Acc_Down : StatusEffect.Unknown;
                orderToCheck[37] = TC.MagicAtkDownCB.Checked ? StatusEffect.Magic_Atk_Down : StatusEffect.Unknown;
                orderToCheck[38] = TC.HelixCB.Checked ? StatusEffect.Helix : StatusEffect.Unknown;
                orderToCheck[39] = TC.MaxTpDownCB.Checked ? StatusEffect.Max_TP_Down : StatusEffect.Unknown;
                orderToCheck[40] = TC.RequiemCB.Checked ? StatusEffect.Requiem : StatusEffect.Unknown;
                orderToCheck[41] = TC.ElegyCB.Checked ? StatusEffect.Elegy : StatusEffect.Unknown;
                orderToCheck[42] = TC.ThrenodyCB.Checked ? StatusEffect.Threnody : StatusEffect.Unknown;

                foreach (StatusEffect effectToCheck in orderToCheck)
                {
                    if (effectToCheck != StatusEffect.Unknown)
                    {
                        foreach (StatusEffect monitoredEffect in Character.Api.Player.Buffs)
                        {
                            if (monitoredEffect == effectToCheck)
                            {
                                SpellList spellToUse = getSpellToRemoveDebuff(monitoredEffect);

                                if (spellToUse == SpellList.Silena)
                                {
                                    actions.AddLast(new PLAction(PLAction.type.Debuff, System.Double.MaxValue, PLAction.target.PL, spellToUse, monitoredEffect, Character));
                                }
                                else if (Character.SpellRecast(spellToUse.ToString()) == 0 && Character.spells.SpellMpCost(spellToUse.ToString()))
                                {
                                    if (Character.IsAfflicted(EliteMMO.API.StatusEffect.Silence))
                                    {
                                        double scaledPriority = PLAction.calculateScaledPriority(PLAction.type.Debuff, 1, indexOrder, monitoredEffect, (int)Character.Api.Player.HPMax);

                                        actions.AddLast(new PLAction(PLAction.type.Debuff, scaledPriority, PLAction.target.PL, spellToUse, monitoredEffect, Character));
                                    }
                                    if (Character.IsAfflicted(EliteMMO.API.StatusEffect.Doom))
                                    {
                                        double scaledPriority = PLAction.calculateScaledPriority(PLAction.type.Debuff, 1, indexOrder, monitoredEffect, (int)Character.Api.Player.HPMax);

                                        actions.AddLast(new PLAction(PLAction.type.Debuff, scaledPriority, PLAction.target.PL, spellToUse, monitoredEffect, Character));
                                    }
                                }
                                return true;
                            }
                        }
                    }
                    indexOrder++;
                    return true;
                }
                return false;
                Exit();
            }
            else
                return false;
            Exit();
        }

        private SpellList getSpellToRemoveDebuff(StatusEffect effect)
        {
            switch (effect)
            {
                case StatusEffect.Sleep: return SpellList.Cure;
                case StatusEffect.Sleep2: return SpellList.Cure;
                case StatusEffect.Doom: return SpellList.Cursna;
                case StatusEffect.Silence: return SpellList.Silena;
                case StatusEffect.Petrification: return SpellList.Stona;
                case StatusEffect.Paralysis: return SpellList.Paralyna;
                case StatusEffect.Poison: return SpellList.Poisona;
                case StatusEffect.Attack_Down: return SpellList.Erase;
                case StatusEffect.Blindness: return SpellList.Blindna;
                case StatusEffect.Bind: return SpellList.Erase;
                case StatusEffect.Weight: return SpellList.Erase;
                case StatusEffect.Slow: return SpellList.Erase;
                case StatusEffect.Curse: return SpellList.Cursna;
                case StatusEffect.Curse2: return SpellList.Cursna;
                case StatusEffect.Addle: return SpellList.Erase;
                case StatusEffect.Bane: return SpellList.Cursna;
                case StatusEffect.Plague: return SpellList.Viruna;
                case StatusEffect.Burn: return SpellList.Erase;
                case StatusEffect.Frost: return SpellList.Erase;
                case StatusEffect.Choke: return SpellList.Erase;
                case StatusEffect.Rasp: return SpellList.Erase;
                case StatusEffect.Shock: return SpellList.Erase;
                case StatusEffect.Drown: return SpellList.Erase;
                case StatusEffect.Dia: return SpellList.Erase;
                case StatusEffect.Bio: return SpellList.Erase;
                case StatusEffect.STR_Down: return SpellList.Erase;
                case StatusEffect.DEX_Down: return SpellList.Erase;
                case StatusEffect.VIT_Down: return SpellList.Erase;
                case StatusEffect.AGI_Down: return SpellList.Erase;
                case StatusEffect.INT_Down: return SpellList.Erase;
                case StatusEffect.MND_Down: return SpellList.Erase;
                case StatusEffect.CHR_Down: return SpellList.Erase;
                case StatusEffect.Max_HP_Down: return SpellList.Erase;
                case StatusEffect.Max_MP_Down: return SpellList.Erase;
                case StatusEffect.Accuracy_Down: return SpellList.Erase;
                case StatusEffect.Evasion_Down: return SpellList.Erase;
                case StatusEffect.Defense_Down: return SpellList.Erase;
                case StatusEffect.Flash: return SpellList.Erase;
                case StatusEffect.Magic_Acc_Down: return SpellList.Erase;
                case StatusEffect.Magic_Atk_Down: return SpellList.Erase;
                case StatusEffect.Helix: return SpellList.Erase;
                case StatusEffect.Max_TP_Down: return SpellList.Erase;
                case StatusEffect.Requiem: return SpellList.Erase;
                case StatusEffect.Elegy: return SpellList.Erase;
                case StatusEffect.Threnody: return SpellList.Erase;
            }
            return SpellList.Unknown;
        }

        public override void Update()
        {
            try
            {
                if (actionPriorities.Count != 0)
                {
                    var orderedPriorities = actionPriorities.ToList().OrderByDescending(p => p.Priority);
                    int index = 0;

                    if ((orderedPriorities.ToArray()[0].Priority > 3))
                    {
                        double waitHowLong = orderedPriorities.ToArray()[0].Do(Character.Api, Character.MonitoredAPI, Character.Tasks.Huntertask.Options.SilencedItem, Character.Tasks.Huntertask.Options.DoomItem);
                        if (waitHowLong != 0.0)
                        {
                            Delay = DateTime.Now.AddSeconds(waitHowLong);
                        }
                    }

                    foreach (PLAction action in orderedPriorities)
                    {
                        Character.Logger.AddDebugText(TC.rtbDebug, string.Format(@" {0} {1} {2} {3}", Character.MonitoredAPI.Player.Name, Character.Api.Player.Name, Character.Tasks.Huntertask.Options.SilencedItem, index));
                        index++;
                    }
                }
                actionPriorities.Count.Equals(0);
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }

        #endregion Methods
    }
}