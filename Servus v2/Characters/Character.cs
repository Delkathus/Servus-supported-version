using EliteMMO.API;
using Servus_v2.Common;
using Servus_v2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Servus_v2.Characters
{
    public class Character
    {
        #region Fields

        public Dictionary<string, EliteAPI> _CharacterDictionary;
        public float LastcastingValue;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);
        public SaveAndLoad SaveOrLoad { get; set; }

        #endregion Fields

        #region Constructors

        public spells spells { get; set; }
        public EliteAPI MonitoredAPI { get; set; }

        public Character(Log Log, ToonControl tc, Dictionary<string, EliteAPI> chars, EliteAPI api)
        {
            Logger = Log;
            Tc = tc;
            _CharacterDictionary = chars;
            Api = api;
            CreateFolders();
            MonitoredAPI = api;
            LastcastingValue = CastPercentEx;
            Navi = new Navigation(this);
            Chat = new Chat(this);

            Tasks = new Tasks(this);
            Target = new Target(this);
            _Abilities = new Abilities(this);
            spells = new spells(this);
            SaveOrLoad = new SaveAndLoad(this);
            _timer.Interval = 200;
            _timer.Elapsed += Tick;
            Start();
        }

        public void SetMonitoredAPI(int p)
        {
            MonitoredAPI = new EliteAPI(p);
        }

        #endregion Constructors

        #region Events

        public Abilities _Abilities { get; set; }

        public event EventHandler<NavigationEventArgs> Moved = delegate { };

        public event EventHandler StatusChanged = delegate { };

        public event EventHandler ZoneChanged = delegate { };

        #endregion Events

        #region Properties

        public EliteAPI Api { get; set; }

        public float CastPercentEx => (Api.CastBar.Percent);

        public Chat Chat { get; set; }

        private delegate string[] GetRichTextBoxLinesInvoker();

        private string[] GetRichTextBoxLines()
        {
            string[] lines;

            if (Tc.WStextBox.InvokeRequired)
            {
                lines = (string[])Tc.WStextBox.Invoke(new GetRichTextBoxLinesInvoker(GetRichTextBoxLines));
            }
            else
            {
                lines = Tc.WStextBox.Lines;
            }
            Tasks.Huntertask.Options.WSScript.Clear();
            foreach (var str in lines)
            {
                Tasks.Huntertask.Options.WSScript.Add(str);
            }

            return lines;
        }

        public void LoadSettings()
        {
            try
            {
                OpenFileDialog OpenDialog = new OpenFileDialog();

                string startpath = Path.GetDirectoryName(Application.ExecutablePath);
                string PATH = (String.Format(@"{0}Documents\\{1}\\Config\\", startpath, Api.Player.Name));
                OpenDialog.InitialDirectory = PATH;
                OpenDialog.FilterIndex = 0;

                if (OpenDialog.ShowDialog() == DialogResult.OK)
                {
                    Tasks.Huntertask.Options.Targets.Clear();
                    Tasks.Huntertask.Options.JobAbilityKeepActive.Clear();
                    Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Clear();
                    Tc.WStextBox.Text = null;
                    Tc.NpcLB.Items.Clear();
                    string Filename = OpenDialog.FileName;
                    Tasks.Huntertask.LoadSettings(Filename);
                    foreach (var T in Tasks.Huntertask.Options.Targets)
                    {
                        Tc.NpcLB.Items.Add(T.ToString());
                    }
                    foreach (var ja in Tasks.Huntertask.Options.JobAbilityKeepActive)
                    {
                        Tc.JaActiveLB.Items.Add(ja.ToString());
                    }
                    foreach (var jaf in Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights)
                    {
                        Tc.JaFightOnlyLb.Items.Add(jaf.ToString());
                    }
                    Tc.TargetCountLb.Text = string.Format(@"Target count = {0}", Tasks.Huntertask.Options.Targets.Count.ToString());
                    Tc.SearchDistanceTrackBar.Value = Tasks.Huntertask.Options.SearchDistance;
                    Tc.PullDistanceTrackBar.Value = Tasks.Huntertask.Options.PullDistance;
                    Tc.pullTb.Text = Tasks.Huntertask.Options.PullCommand;
                    Tc.HpRestingCB.Checked = Tasks.Huntertask.Options.RestOnLowHpEnabled;
                    Tc.hpptrackbar.Value = Tasks.Huntertask.Options.LowHpValue;
                    Tc.HpUpValue.Value = Tasks.Huntertask.Options.TargetHpp;
                    Tc.mpptrackbar.Value = Tasks.Huntertask.Options.LowMpValue;
                    Tc.MpUpValue.Value = Tasks.Huntertask.Options.TargetMpp;
                    Tc.RestWhenWeakCB.Checked = Tasks.Huntertask.Options.RestOnWeakenedEnabled;
                    Tc.RaiseCb.Checked = Tasks.Huntertask.Options.AcceptRaise;
                    Tc.homepointCB.Checked = Tasks.Huntertask.Options.GoHome;
                    Tc.RaiseTrB.Value = Tasks.Huntertask.Options.Raisemobdistance;
                    Tc.InvNothing.Checked = Tasks.Huntertask.Options.InvIgnore;
                    Tc.InvLogout.Checked = Tasks.Huntertask.Options.InvLogOut;
                    Tc.InvStop.Checked = Tasks.Huntertask.Options.InvStop;
                    Tc.InvInstaWarp.Checked = Tasks.Huntertask.Options.InvInstant;
                    Tc.InvWarp.Checked = Tasks.Huntertask.Options.InvWarp;
                    Tc.InvWarpring.Checked = Tasks.Huntertask.Options.InvWarpRing;
                    Tc.InvWarpCudgel.Checked = Tasks.Huntertask.Options.InvWarpCudgel;
                    Tc.StopOnTellCB.Checked = Tasks.Huntertask.Options.StopOnTell;
                    Tc.IdleDelayValue.Value = Tasks.Huntertask.Options.IdleDelay;
                    Tc.WStextBox.Lines = Tasks.Huntertask.Options.WSScript.ToArray();
                    Tc.TPtrackbar.Value = Tasks.Huntertask.Options.WStpValue;
                    Tc.AttackDownCB.Checked = Tasks.Huntertask.Options.UseAttackDown;
                    Tc.AddleCB.Checked = Tasks.Huntertask.Options.UseAddle;
                    Tc.AccuracyDownCB.Checked = Tasks.Huntertask.Options.UseAccuracyDown;
                    Tc.AgiDownCB.Checked = Tasks.Huntertask.Options.UseAGIdown;
                    Tc.BlindnessCB.Checked = Tasks.Huntertask.Options.UseBlindness;
                    Tc.BurnCB.Checked = Tasks.Huntertask.Options.UseBurn;
                    Tc.BindCB.Checked = Tasks.Huntertask.Options.UseBind;
                    Tc.BioCB.Checked = Tasks.Huntertask.Options.UseBio;
                    Tc.BaneCB.Checked = Tasks.Huntertask.Options.UseBane;
                    Tc.ChokeCB.Checked = Tasks.Huntertask.Options.UseChoke;
                    Tc.ChrDownCB.Checked = Tasks.Huntertask.Options.UseCHRdown;
                    Tc.CurseCB.Checked = Tasks.Huntertask.Options.UseCurse;
                    Tc.Curse2CB.Checked = Tasks.Huntertask.Options.UseCurse2;
                    Tc.DrownCB.Checked = Tasks.Huntertask.Options.UseDrown;
                    Tc.DoomCB.Checked = Tasks.Huntertask.Options.UseDoom;
                    Tc.DiaCB.Checked = Tasks.Huntertask.Options.UseDia;
                    Tc.DefenseDownCB.Checked = Tasks.Huntertask.Options.UseDefenseDown;
                    Tc.DexDownCB.Checked = Tasks.Huntertask.Options.UseDEXdown;
                    Tc.ElegyCB.Checked = Tasks.Huntertask.Options.UseElegy;
                    Tc.EvasionDownCB.Checked = Tasks.Huntertask.Options.UseEvaDown;
                    Tc.FlashCB.Checked = Tasks.Huntertask.Options.UseFlash;
                    Tc.FrostCB.Checked = Tasks.Huntertask.Options.UseFrost;
                    Tc.HelixCB.Checked = Tasks.Huntertask.Options.UseHelix;
                    Tc.IntDownCB.Checked = Tasks.Huntertask.Options.UseIntDown;
                    Tc.MndDownCB.Checked = Tasks.Huntertask.Options.UseMNDdown;
                    Tc.MagicAccDownCB.Checked = Tasks.Huntertask.Options.UseMagACC;
                    Tc.MagicAtkDownCB.Checked = Tasks.Huntertask.Options.UseMagATK;
                    Tc.MaxHpDownCB.Checked = Tasks.Huntertask.Options.UseHPdown;
                    Tc.MaxTpDownCB.Checked = Tasks.Huntertask.Options.UseTPdown;
                    Tc.MaxMpDownCB.Checked = Tasks.Huntertask.Options.UseMPdown;
                    Tc.ParalysisCB.Checked = Tasks.Huntertask.Options.UseParalysis;
                    Tc.PlagueCB.Checked = Tasks.Huntertask.Options.UsePlague;
                    Tc.PoisonCB.Checked = Tasks.Huntertask.Options.UsePoison;
                    Tc.RaspCB.Checked = Tasks.Huntertask.Options.UseRasp;
                    Tc.RequiemCB.Checked = Tasks.Huntertask.Options.UseRequiem;
                    Tc.StrDownCB.Checked = Tasks.Huntertask.Options.UseSTRdown;
                    Tc.ShockCB.Checked = Tasks.Huntertask.Options.UseShock;
                    Tc.SilenceCB.Checked = Tasks.Huntertask.Options.UseSilence;
                    Tc.SlowCB.Checked = Tasks.Huntertask.Options.UseSlow;
                    Tc.ThrenodyCB.Checked = Tasks.Huntertask.Options.UseThrenody;
                    Tc.VitDownCB.Checked = Tasks.Huntertask.Options.UseVitDown;
                    Tc.WeightCB.Checked = Tasks.Huntertask.Options.UseWeight;
                    Tc.UseItemForSilenceCB.Checked = Tasks.Huntertask.Options.useSilenceItem;

                    Tc.UseItemForDoomCB.Checked = Tasks.Huntertask.Options.useDoomItem;
                    if (Tasks.Huntertask.Options.DoomItem == "Holy Water")
                    {
                        Tc.Doomitem.SelectedIndex = 0;
                    }
                    if (Tasks.Huntertask.Options.DoomItem == "Hallowed Water")
                    {
                        Tc.Doomitem.SelectedIndex = 1;
                    }
                    if (Tasks.Huntertask.Options.SilencedItem == "Catholicon")
                    {
                        Tc.SilenceItem.SelectedIndex = 0;
                    }
                    if (Tasks.Huntertask.Options.SilencedItem == "Echo Drops")
                    {
                        Tc.SilenceItem.SelectedIndex = 1;
                    }
                    if (Tasks.Huntertask.Options.SilencedItem == "Remedy")
                    {
                        Tc.SilenceItem.SelectedIndex = 2;
                    }
                    if (Tasks.Huntertask.Options.SilencedItem == "Remedy Ointment")
                    {
                        Tc.SilenceItem.SelectedIndex = 3;
                    }
                    if (Tasks.Huntertask.Options.SilencedItem == "Vicar's Drink")
                    {
                        Tc.SilenceItem.SelectedIndex = 4;
                    }
                    Tc.AquaveilCB.Checked = Tasks.Huntertask.Options.UseAquaveil;
                    Tc.BlinkCB.Checked = Tasks.Huntertask.Options.UseBlink;
                    Tc.Hhaste.Checked = Tasks.Huntertask.Options.UseHasteSamba;
                    Tc.HasteCB.Checked = Tasks.Huntertask.Options.UseHaste;
                    Tc.Haste2CB.Checked = Tasks.Huntertask.Options.UseHaste2;
                    Tc.PhalanxCB.Checked = Tasks.Huntertask.Options.UsePhalanx;
                    Tc.ProtectCB.Checked = Tasks.Huntertask.Options.UsePro;
                    Tc.Protect2CB.Checked = Tasks.Huntertask.Options.UsePro2;
                    Tc.Protect3CB.Checked = Tasks.Huntertask.Options.UsePro3;
                    Tc.Protect4CB.Checked = Tasks.Huntertask.Options.UsePro4;
                    Tc.Protect5CB.Checked = Tasks.Huntertask.Options.UsePro5;
                    Tc.Refresh1CB.Checked = Tasks.Huntertask.Options.UseRefresh;
                    Tc.Refresh2CB.Checked = Tasks.Huntertask.Options.UseRefresh2;
                    Tc.Refresh3CB.Checked = Tasks.Huntertask.Options.UseRefresh3;
                    Tc.Regen1CB.Checked = Tasks.Huntertask.Options.UseRegen;
                    Tc.Regen2CB.Checked = Tasks.Huntertask.Options.UseRegen2;
                    Tc.Regen3CB.Checked = Tasks.Huntertask.Options.UseRegen3;
                    Tc.Regen4CB.Checked = Tasks.Huntertask.Options.UseRegen4;
                    Tc.Regen5CB.Checked = Tasks.Huntertask.Options.UseRegen5;
                    Tc.Reraise1CB.Checked = Tasks.Huntertask.Options.UseReraise;
                    Tc.Reraise2CB.Checked = Tasks.Huntertask.Options.UseReraise2;
                    Tc.Reraise3CB.Checked = Tasks.Huntertask.Options.UseRefresh3;
                    Tc.Reraise4CB.Checked = Tasks.Huntertask.Options.UseReraise4;
                    Tc.Shell1CB.Checked = Tasks.Huntertask.Options.UseShell;
                    Tc.Shell2CB.Checked = Tasks.Huntertask.Options.UseShell2;
                    Tc.Shell3CB.Checked = Tasks.Huntertask.Options.UseShell3;
                    Tc.Shell4CB.Checked = Tasks.Huntertask.Options.UseShell4;
                    Tc.Shell5CB.Checked = Tasks.Huntertask.Options.UseShell5;
                    Tc.StoneskinCB.Checked = Tasks.Huntertask.Options.UseStoneSkin;
                    Tc.BlazeSpikesCB.Checked = Tasks.Huntertask.Options.UseBlazeSpikes;
                    Tc.IceSPikesCB.Checked = Tasks.Huntertask.Options.UseIceSpikes;
                    Tc.ShellraCB.Checked = Tasks.Huntertask.Options.UseShellra;
                    Tc.Shellra2CB.Checked = Tasks.Huntertask.Options.UseShellra2;
                    Tc.shellra3CB.Checked = Tasks.Huntertask.Options.UseShellra3;
                    Tc.Shellra4CB.Checked = Tasks.Huntertask.Options.UseShellra4;
                    Tc.shellra5CB.Checked = Tasks.Huntertask.Options.UseShellra5;
                    Tc.ProtectraCB.Checked = Tasks.Huntertask.Options.UseProtectra;
                    Tc.Protectra2CB.Checked = Tasks.Huntertask.Options.UseProtectra2;
                    Tc.protectra3CB.Checked = Tasks.Huntertask.Options.UseProtectra3;
                    Tc.Protectra4CB.Checked = Tasks.Huntertask.Options.UseProtectra4;
                    Tc.Protectra5CB.Checked = Tasks.Huntertask.Options.UsePrptectra5;
                    Tc.BoostAGICB.Checked = Tasks.Huntertask.Options.UseBoostagi;
                    Tc.BoostCHRCB.Checked = Tasks.Huntertask.Options.UseBoostchr;
                    Tc.BoostDEXCB.Checked = Tasks.Huntertask.Options.UseBoostdex;
                    Tc.BoostINTCB.Checked = Tasks.Huntertask.Options.UseBoostint;
                    Tc.BoostMNDCB.Checked = Tasks.Huntertask.Options.UseBoostmnd;
                    Tc.BoostSTRCB.Checked = Tasks.Huntertask.Options.UseBooststr;
                    Tc.BoostVITCB.Checked = Tasks.Huntertask.Options.UseBoostvit;
                    Tc.UtsusemiIchiCB.Checked = Tasks.Huntertask.Options.UseIchi;
                    Tc.UtsusemiNiCB.Checked = Tasks.Huntertask.Options.UseNi;
                }
            }
            catch (Exception ex)
            {
                Logger.AddDebugText(Tc.rtbDebug, string.Format(@"Error loading {0}", ex));
            }
        }

        public void SaveSettings()
        {
            try
            {
                GetRichTextBoxLines();
                SaveFileDialog fdgSave = new SaveFileDialog();

                string startpath = Path.GetDirectoryName(Application.ExecutablePath);
                string PATH = (String.Format(@"{0}Documents\{1}\Config\", startpath, Api.Player.Name));

                SaveFileDialog SaveDialog = new SaveFileDialog();
                SaveDialog.InitialDirectory = PATH;
                SaveDialog.Filter = "xml |*.xml";
                SaveDialog.FilterIndex = 1;
                string Filename;
                if (SaveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (SaveDialog.FileName.Contains(".xml"))
                        Filename = SaveDialog.FileName;
                    else
                        Filename = SaveDialog.FileName + ".xml";
                    Tasks.Huntertask.Options.PullCommand = Tc.pullTb.Text;
                    XmlSerializationHelper.Serialize(Filename, Tasks.Huntertask.Options);
                }
                fdgSave.Dispose();
                SaveDialog.Dispose();
            }
            catch (Exception ex)
            {
                Logger.AddDebugText(Tc.rtbDebug, string.Format(@"Error Saving {0}", ex));
            }
        }

        public Position CurrentPosition => new Position { X = Api.Player.X, Y = Api.Player.Y, Z = Api.Player.Z, H = Api.Player.H };

        public Zone CurrentZone => (Zone)Api.Player.ZoneId;

        public bool HasPet => (Api.Player.MainJob == (byte)JobType.BeastMaster || Api.Player.MainJob == (byte)JobType.Summon)
                              && Api.Player.PetIndex != 0;

        public bool SafeToGetUP
        {
            get
            {
                for (var i = 0; i < 768; i++)
                {
                    if (Navi.DistanceTo(i) < Tc.RaiseTrB.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsCasting { get; set; }

        public bool IsDead => Status == EntityStatus.Dead || Status == EntityStatus.DeadEngaged || Api.Player.HP <= 0;

        public bool Zoning()
        {
            return Api.Player.LoginStatus == (int)LoginStatus.Loading || Api.Player.LoginStatus == (int)LoginStatus.LoginScreen;
        }

        public bool IsAfflicteAmnesia => IsAfflicted(EliteMMO.API.StatusEffect.Amnesia);
        public bool IsSilenced => IsAfflicted(EliteMMO.API.StatusEffect.Silence);

        public bool Busy => IsDead || IsAfflicted(EliteMMO.API.StatusEffect.Charm1) ||
                            IsAfflicted(EliteMMO.API.StatusEffect.Charm2)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Paralysis) || IsAfflicted(EliteMMO.API.StatusEffect.Sleep)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Sleep2) || IsAfflicted(EliteMMO.API.StatusEffect.Stun)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Stun)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Petrification)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Terror)
                            || IsCasting
                            || Zoning()
                            || Status == EntityStatus.Healing;

        public bool IsMoving { get; set; }
        public EliteAPI Leader { get; set; }
        public Log Logger { get; set; }
        public Navigation Navi { get; set; }
        public Position OldPosition { get; set; }

        public int PetsIndex => !HasPet ? 0 : Api.Player.PetIndex;

        public string PetsName
        {
            get
            {
                if (!HasPet)
                {
                    return string.Empty;
                }

                return Api.Entity.GetEntity(PetsIndex).Name;
            }
        }

        public EntityStatus Status { get; private set; }
        public Target Target { get; set; }
        public Tasks Tasks { get; set; }
        public ToonControl Tc { get; set; }
        private Zone Zone { get; set; }

        #endregion Properties

        #region Methods

        public void CastCheck()
        {
            if (CastPercentEx.Equals(0))
            {
                if (IsCasting)
                {
                    Logger.AddDebugText(Tc.rtbDebug, "Cast stop");
                }
                IsCasting = false; // Not casting
            }
            else if (CastPercentEx.Equals(LastcastingValue)) // we were interupted
            {
                if (IsCasting)
                {
                    Logger.AddDebugText(Tc.rtbDebug, "Cast stop");
                }
                IsCasting = false;
            }
            else // we are casting
            {
                if (!IsCasting)
                {
                    Logger.AddDebugText(Tc.rtbDebug, "Cast start");
                }
                LastcastingValue = CastPercentEx;
                IsCasting = true;
            }
        }

        public void CreateFolders()
        {
            if (!System.IO.Directory.Exists(string.Format(@"Documents\{0}\ChatLog", Api.Player.Name)))
            {
                System.IO.Directory.CreateDirectory(string.Format(@"Documents\{0}\ChatLog", Api.Player.Name));
            }
            if (!System.IO.Directory.Exists(string.Format(@"Documents\{0}\Nav", Api.Player.Name)))
            {
                System.IO.Directory.CreateDirectory(string.Format(@"Documents\{0}\Nav", Api.Player.Name));
            }
            if (!System.IO.Directory.Exists(string.Format(@"Documents\{0}\Config", Api.Player.Name)))
            {
                System.IO.Directory.CreateDirectory(string.Format(@"Documents\{0}\Config", Api.Player.Name));
            }
        }

        public bool IsAfflictedJA(EliteMMO.API.StatusEffect effect)
        {
            foreach (var s in Api.Player.Buffs)
            {
                var status = (EliteMMO.API.StatusEffect)s;
                if (status == effect)
                {
                    return true;
                }
            }
            return false;
        }

        public int SpellRecast(string spell)
        {
            var NeededSpell = Api.Resources.GetSpell(spell, 0);
            if (Api.Recast.GetSpellRecast(NeededSpell.Index) == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public bool IsAfflicted(EliteMMO.API.StatusEffect effect)
        {
            foreach (var s in Api.Player.Buffs)
            {
                var status = (EliteMMO.API.StatusEffect)s;
                if (status == effect)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEngaged()
        {
            return Api.Player.Status == (ulong)EntityStatus.Engaged;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        private void Tick(object sender, EventArgs e)
        {
            if (Status != (EntityStatus)Api.Player.Status)
            {
                Status = (EntityStatus)Api.Player.Status;
                StatusChanged(this, new StatusChangedEventArgs { Status = Status });
            }
            if (Zone != CurrentZone)
            {
                Zone = CurrentZone;
                ZoneChanged(this, EventArgs.Empty);
            }
            CastCheck();
            if (OldPosition.Equals(CurrentPosition))
            {
                IsMoving = false;
            }
            else if (!OldPosition.Equals(CurrentPosition))
            {
                IsMoving = true;

                OldPosition = CurrentPosition;
                if (Tasks.Huntertask.Options.RecordWaypoints)
                {
                    Navi.LearnRoutine();
                }
            }
        }

        #endregion Methods

        public bool IsTargetLocked()
        {
            var _Target = Api.Target.GetTargetInfo();
            if (_Target.TargetIndex == (uint)Target.FindBestTarget() && _Target.LockedOn)
            {
                return true;
            }
            return false;
        }

        protected virtual void OnMoved(NavigationEventArgs e)
        {
            Moved(this, e);
        }
    }
}