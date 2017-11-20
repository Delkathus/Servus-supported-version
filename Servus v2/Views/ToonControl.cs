using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.FFXi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Servus_v2.Views
{
    public partial class ToonControl : UserControl
    {
        #region Constructors

        public ToonControl(MainForm mf, Dictionary<string, EliteAPI> chars, EliteAPI api)
        {
            InitializeComponent();
            Character = new Character(mf.Logger, this, chars, api);
            Initialize();
        }

        #endregion Constructors

        #region Properties

        public Character Character { get; set; }

        #endregion Properties

        #region Methods

        public void Initialize()
        {
            ChatTimer.Start();
            TargetComboBox.SelectedIndex = 0;
            Jacombo.SelectedIndex = 0;
            JATargetCb.SelectedIndex = 0;
            PrefixCB.SelectedIndex = 0;
            Jacombo2.SelectedIndex = 0;
            JATargetCb2.SelectedIndex = 0;
            PrefixCB2.SelectedIndex = 0;
            VersionLable.Text = string.Format(@"Version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            DateTime buildDate =
   new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            LastupdatedLb.Text = string.Format(@"Last updated on: {0}", buildDate.ToString());

            foreach (var str in Character._Abilities.AbilityDictionary)
            {
                var JaName = string.Format(@"{0}", str.Value.En.ToString());

                Jacombo.Items.Add(JaName);
                Jacombo2.Items.Add(JaName);
            }
        }

        private void AddTargetBtn_Click(object sender, EventArgs e)
        {
            if (!Character.Tasks.Huntertask.Options.Targets.Contains(NpcLB.SelectedItem.ToString()))
            {
                Character.Tasks.Huntertask.Options.Targets.Add(NpcLB.SelectedItem.ToString());
                TargetCountLb.Text = string.Format(@"Target count = {0}", Character.Tasks.Huntertask.Options.Targets.Count);
                TargetComboBox.Items.Add(NpcLB.SelectedItem.ToString());
                Character.Logger.AddDebugText(rtbDebug, (string.Format(@"Added {0} to kill list", NpcLB.SelectedItem.ToString())));
            }
            else if (Character.Tasks.Huntertask.Options.Targets.Contains(NpcLB.SelectedItem.ToString()))
            {
                Character.Logger.AddDebugText(rtbDebug, (string.Format(@"{0} Is already in kill list", NpcLB.SelectedItem.ToString())));
            }
        }

        private void ChatTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Character.Chat.ChatWork();
            }
            catch (Exception ex)
            {
                Character.Logger.AddDebugText(rtbDebug, ex.ToString());
                Character.Logger.LogFile("timer", FindForm().Name);
            }
        }

        private void Hpptrackbar_ValueChanged(object sender, EventArgs e)
        {
            HPrestingValueLB.Text = string.Format(@"{0} %", hpptrackbar.Value);
            Character.Tasks.Huntertask.Options.LowHpValue = hpptrackbar.Value;
        }

        private void HpRestingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (HpRestingCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.RestOnLowHpEnabled = true;
            }
            else
                Character.Tasks.Huntertask.Options.RestOnLowHpEnabled = false;
        }

        private void HpUpValue_ValueChanged(object sender, EventArgs e)
        {
            HPupValueLB.Text = string.Format(@"{0} %", HpUpValue.Value);
            Character.Tasks.Huntertask.Options.TargetHpp = HpUpValue.Value;
        }

        private void MppRestingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MppRestingCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.RestOnLowMpEnabled = true;
            }
            else
                Character.Tasks.Huntertask.Options.RestOnLowMpEnabled = false;
        }

        private void Mpptrackbar_ValueChanged(object sender, EventArgs e)
        {
            MppLb.Text = string.Format(@"{0} %", mpptrackbar.Value);
            Character.Tasks.Huntertask.Options.LowMpValue = mpptrackbar.Value;
        }

        private void MpUpValue_ValueChanged(object sender, EventArgs e)
        {
            MPupValueLB.Text = string.Format(@"{0} %", MpUpValue.Value);
            Character.Tasks.Huntertask.Options.TargetMpp = MpUpValue.Value;
        }

        private void NpcLB_DoubleClick(object sender, EventArgs e)
        {
            if (!Character.Tasks.Huntertask.Options.Targets.Contains(NpcLB.SelectedItem.ToString()))
            {
                Character.Tasks.Huntertask.Options.Targets.Add(NpcLB.SelectedItem.ToString());
                TargetCountLb.Text = string.Format(@"Target count = {0}", Character.Tasks.Huntertask.Options.Targets.Count);
                TargetComboBox.Items.Add(NpcLB.SelectedItem.ToString());
                Character.Logger.AddDebugText(rtbDebug, (string.Format(@"Added {0} to kill list", NpcLB.SelectedItem)));
            }
            else if (Character.Tasks.Huntertask.Options.Targets.Contains(NpcLB.SelectedItem.ToString()))
            {
                Character.Logger.AddDebugText(rtbDebug, (string.Format(@"{0} Is already in kill list", NpcLB.SelectedItem)));
            }
        }

        private void PullDistanceTrackBar_ValueChanged(object sender, EventArgs e)
        {
            PullDistanceLb.Text = string.Format(@"Pull Distance = {0}y", PullDistanceTrackBar.Value);
            Character.Tasks.Huntertask.Options.PullDistance = PullDistanceTrackBar.Value;
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            if (Character.Tasks.Huntertask.Options.Targets.Contains(TargetComboBox.SelectedItem.ToString()) && !TargetComboBox.SelectedItem.Equals("View Targets"))
            {
                Character.Tasks.Huntertask.Options.Targets.Remove(TargetComboBox.SelectedItem.ToString());
                TargetCountLb.Text = string.Format(@"Target count = {0}", Character.Tasks.Huntertask.Options.Targets.Count);
                Character.Logger.AddDebugText(rtbDebug, (string.Format(@"Removed {0} from kill list", TargetComboBox.SelectedItem)));
                TargetComboBox.Items.Remove(TargetComboBox.SelectedItem);
                TargetComboBox.SelectedIndex = 0;
            }
            else if (!Character.Tasks.Huntertask.Options.Targets.Contains(NpcLB.SelectedItem.ToString()) && !TargetComboBox.SelectedItem.Equals("View Targets"))
            {
                Character.Logger.AddDebugText(rtbDebug, (string.Format(@"{0} Was not in kill list, Unable to remove", TargetComboBox.SelectedItem)));
            }
        }

        private void RestWhenWeakCB_CheckedChanged(object sender, EventArgs e)
        {
            if (RestWhenWeakCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.RestOnWeakenedEnabled = true;
            }
            else
                Character.Tasks.Huntertask.Options.RestOnWeakenedEnabled = false;
        }

        private void RtbChat_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbChat, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved ChatTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RtbDebug_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbDebug, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved DebugTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RtbParty_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbParty, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved PartyTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RtbSay_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbSay, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved SayTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RtbShell_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbShell, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved ShellTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RtbShout_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbShout, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved ShoutTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RtbTell_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbTell, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved TellTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RtbYell_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.SaveChatLogs(rtbYell, Character.Api.Player.Name);

                Character.Logger.AddDebugText(rtbDebug, "Saved YellTxt");
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            if (RunBtn.Text == "Start Hunter" && !Character.Tasks.Huntertask.IsBusy)
            {
                RunBtn.Text = "Stop Hunter";
                Thread.Sleep(100);
                Character.Tasks.Huntertask.Start();
            }
            else if (RunBtn.Text == "Stop Hunter")
            {
                Character.Tasks.Huntertask.Stop();
                Character.Navi.Reset();
                RunBtn.Text = "Start Hunter";
                Thread.Sleep(200);
                Character.Navi.Reset();
            }
        }

        private void ScanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Character.Logger.AddDebugText(rtbDebug, "Scanning for Npcs");
                Character.Target.GetNpcNames();
                NpcLB.Items.Clear();
                foreach (string name in Character.Target.GetNpcNames())
                {
                    if (!NpcLB.Items.Contains(name))
                    {
                        NpcLB.Items.Add(name);
                    }
                }
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void SearchDistanceTrackBar_ValueChanged(object sender, EventArgs e)
        {
            SearchDistanceLb.Text = string.Format(@"Search Distance = {0}y", SearchDistanceTrackBar.Value);
            Character.Tasks.Huntertask.Options.SearchDistance = SearchDistanceTrackBar.Value;
        }

        #endregion Methods

        private void TPtrackbar_ValueChanged(object sender, EventArgs e)
        {
            Tplbl.Text = string.Format(@"{0} %", TPtrackbar.Value);
            Character.Tasks.Huntertask.Options.WStpValue = TPtrackbar.Value;
        }

        private void RaiseTrB_ValueChanged(object sender, EventArgs e)
        {
            raisedist.Text = string.Format(@"{0}yalms away  >", RaiseTrB.Value);
            Character.Tasks.Huntertask.Options.Raisemobdistance = RaiseTrB.Value;
        }

        private void RaiseCb_CheckedChanged(object sender, EventArgs e)
        {
            if (RaiseCb.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.AcceptRaise = true;
            }
            else
                Character.Tasks.Huntertask.Options.AcceptRaise = false;
        }

        private void HomepointCB_CheckedChanged(object sender, EventArgs e)
        {
            if (homepointCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.GoHome = true;
            }
            else
                Character.Tasks.Huntertask.Options.GoHome = false;
        }

        private void ClearTargetsBtn_Click(object sender, EventArgs e)
        {
            Character.Tasks.Huntertask.Options.Targets.Clear();
            TargetCountLb.Text = string.Format(@"Target count = {0}", Character.Tasks.Huntertask.Options.Targets.Count);
            Character.Logger.AddDebugText(rtbDebug, (@"Cleared Targets"));
            TargetComboBox.Items.Clear();
            TargetComboBox.Items.Add("View Targets");
            TargetComboBox.SelectedIndex = 0;
        }

        private void AddJABtn_Click(object sender, EventArgs e)
        {
            var prefix = PrefixCB.SelectedItem.ToString();
            var ability = Jacombo.SelectedItem.ToString();
            var target = JATargetCb.SelectedItem.ToString();
            var Ja = (string.Format(@"{0} ""{1}"" {2}", prefix, ability, target));
            if (prefix != "Prefix" && ability != "Select Job Ability" && target != "JA Target")
            {
                if (!Character.Tasks.Huntertask.Options.JobAbilityKeepActive.Contains(Ja))
                {
                    Character.Tasks.Huntertask.Options.JobAbilityKeepActive.Add(Ja);
                    JaActiveLB.Items.Add(Ja);
                    Character.Logger.AddDebugText(rtbDebug, string.Format(@"{0}", Ja));
                }
                else if (Character.Tasks.Huntertask.Options.JobAbilityKeepActive.Contains(Ja))
                {
                    Character.Logger.AddDebugText(rtbDebug, (string.Format("{0} is already in the  JA List", Ja)));
                }
            }
            else if (prefix == "Prefix" || ability == "Select Job Ability" || target != "JA Target")
            {
                Character.Logger.AddDebugText(rtbDebug, string.Format(@"Error you can not use ""{0}", Ja));
            }
        }

        public Ability SelectedAbility { get; set; }

        private void Jacombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Jacombo.SelectedItem.ToString() != "Select Job Ability")
            {
                var Janame = Jacombo.SelectedItem.ToString();

                if (Character._Abilities.AbilityDictionary.TryGetValue(Janame, out Ability SelectedAbility))

                {
                    var japrefix = SelectedAbility.Prefix;
                    var jaTarget = SelectedAbility.Targets;
                    switch (japrefix)
                    {
                        case "/ja":
                            PrefixCB.SelectedIndex = 1;
                            break;

                        case "/pet":
                            PrefixCB.SelectedIndex = 2;
                            break;

                        case "/ma":
                            PrefixCB.SelectedIndex = 3;
                            break;
                    }
                    switch (jaTarget)
                    {
                        case 1:
                            JATargetCb.SelectedIndex = 1;
                            break;

                        case 32:
                            JATargetCb.SelectedIndex = 2;
                            break;

                        case 3:
                            JATargetCb.SelectedIndex = 1;
                            break;

                        case 5:
                            JATargetCb.SelectedIndex = 1;
                            break;

                        case 4:
                            JATargetCb.SelectedIndex = 6;
                            break;

                        case 63:
                            JATargetCb.SelectedIndex = 1;
                            break;
                    }
                }
            }
        }

        private void RemoveJABtn_Click(object sender, EventArgs e)
        {
            if (JaActiveLB.SelectedItem != null)
            {
                if (Character.Tasks.Huntertask.Options.JobAbilityKeepActive.Contains(JaActiveLB.SelectedItem.ToString()))
                {
                    Character.Tasks.Huntertask.Options.JobAbilityKeepActive.Remove(JaActiveLB.SelectedItem.ToString());
                    Character.Logger.AddDebugText(rtbDebug, (string.Format(@"Removed {0} from Ja keep active list", JaActiveLB.SelectedItem)));
                    JaActiveLB.Items.Remove(JaActiveLB.SelectedItem);
                }
                else if (!Character.Tasks.Huntertask.Options.JobAbilityKeepActive.Contains(JaActiveLB.SelectedItem.ToString()))
                {
                    Character.Logger.AddDebugText(rtbDebug, (string.Format(@"{0} Was not in ja keep active  list, Unable to remove", JaActiveLB.SelectedItem)));
                }
            }
        }

        private void Jacombo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Jacombo2.SelectedItem.ToString() != "Select Job Ability")
            {
                var Janame = Jacombo2.SelectedItem.ToString();

                if (Character._Abilities.AbilityDictionary.TryGetValue(Janame, out Ability SelectedAbility))

                {
                    var japrefix = SelectedAbility.Prefix;
                    var jaTarget = SelectedAbility.Targets;
                    switch (japrefix)
                    {
                        case "/ja":
                            PrefixCB2.SelectedIndex = 1;
                            break;

                        case "/pet":
                            PrefixCB2.SelectedIndex = 2;
                            break;

                        case "/ma":
                            PrefixCB2.SelectedIndex = 3;
                            break;
                    }
                    switch (jaTarget)
                    {
                        case 1:
                            JATargetCb2.SelectedIndex = 1;
                            break;

                        case 32:
                            JATargetCb2.SelectedIndex = 2;
                            break;

                        case 3:
                            JATargetCb2.SelectedIndex = 1;
                            break;

                        case 5:
                            JATargetCb2.SelectedIndex = 1;
                            break;

                        case 4:
                            JATargetCb2.SelectedIndex = 6;
                            break;

                        case 63:
                            JATargetCb2.SelectedIndex = 1;
                            break;
                    }
                }
            }
        }

        private void AddJABtn2_Click(object sender, EventArgs e)
        {
            {
                var prefix = PrefixCB2.SelectedItem.ToString();
                var ability = Jacombo2.SelectedItem.ToString();
                var target = JATargetCb2.SelectedItem.ToString();
                var Ja = (string.Format(@"{0} ""{1}"" {2}", prefix, ability, target));
                if (prefix != "Prefix" && ability != "Select Job Ability" && target != "JA Target")
                {
                    if (!Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Contains(Ja))
                    {
                        Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Add(Ja);
                        JaFightOnlyLb.Items.Add(Ja);
                        Character.Logger.AddDebugText(rtbDebug, string.Format(@"{0}", Ja));
                    }
                    else if (Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Contains(Ja))
                    {
                        Character.Logger.AddDebugText(rtbDebug, (string.Format("{0} is already in the  JA List", Ja)));
                    }
                }
                else if (prefix == "Prefix" || ability == "Select Job Ability" || target != "JA Target")
                {
                    Character.Logger.AddDebugText(rtbDebug, string.Format(@"Error you can not use ""{0}", Ja));
                }
            }
        }

        private void RemoveJABtn2_Click(object sender, EventArgs e)
        {
            if (JaFightOnlyLb.SelectedItem != null)
            {
                if (Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Contains(JaFightOnlyLb.SelectedItem.ToString()))
                {
                    Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Remove(JaFightOnlyLb.SelectedItem.ToString());
                    Character.Logger.AddDebugText(rtbDebug, (string.Format(@"Removed {0} from Ja keep active list", JaFightOnlyLb.SelectedItem)));
                    JaFightOnlyLb.Items.Remove(JaFightOnlyLb.SelectedItem);
                }
                else if (!Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Contains(JaFightOnlyLb.SelectedItem.ToString()))
                {
                    Character.Logger.AddDebugText(rtbDebug, (string.Format(@"{0} Was not in ja keep active  list, Unable to remove", JaFightOnlyLb.SelectedItem)));
                }
            }
        }

        private void TextToFFxiTB_Click(object sender, EventArgs e)
        {
            if (TextToFFxiTB.Text == "Send text to FFxi here, just type what you want to say & hit enter")
            {
                TextToFFxiTB.Text = null;
            }
        }

        private void StopOnTellCB_CheckedChanged(object sender, EventArgs e)
        {
            {
                if (StopOnTellCB.CheckState.Equals(CheckState.Checked))
                {
                    Character.Tasks.Huntertask.Options.StopOnTell = true;
                }
                else
                    Character.Tasks.Huntertask.Options.StopOnTell = false;
            }
        }

        private void TextToFFxiTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                try
                {
                    if (e.KeyChar == (char)13 && TextToFFxiTB.Text != string.Empty)
                    {
                        Character.Api.ThirdParty.SendString(TextToFFxiTB.Text);
                        Character.Logger.AddDebugText(rtbDebug, string.Format(@"Text sent to game: {0}", TextToFFxiTB.Text));
                        Thread.Sleep(10);
                        TextToFFxiTB.Text = "Send text to FFxi here, just type what you want to say & hit enter";
                        e.Handled = true;
                        Thread.Sleep(20);
                    }
                    if (e.KeyChar == (char)13 && TextToFFxiTB.Text == string.Empty)
                    {
                        Character.Logger.AddDebugText(rtbDebug, "Nothing to send to ffxi. String should contain something");
                    }
                }
                catch (Exception ex)
                {
                    Character.Logger.LogFile(ex.Message, this.FindForm().Name);
                }
            }
        }

        private void InvNothing_CheckedChanged(object sender, EventArgs e)
        {
            if (InvNothing.Checked)
            {
                Character.Tasks.Huntertask.Options.InvIgnore = true;
            }
            else
                Character.Tasks.Huntertask.Options.InvIgnore = false;
        }

        private void InvStop_CheckedChanged(object sender, EventArgs e)
        {
            if (InvStop.Checked)
            {
                Character.Tasks.Huntertask.Options.InvStop = true;
            }
            else
                Character.Tasks.Huntertask.Options.InvStop = false;
        }

        private void InvInstaWarp_CheckedChanged(object sender, EventArgs e)
        {
            if (InvInstaWarp.Checked)
            {
                Character.Tasks.Huntertask.Options.InvInstant = true;
            }
            else
                Character.Tasks.Huntertask.Options.InvInstant = false;
        }

        private void InvWarpCudgel_CheckedChanged(object sender, EventArgs e)
        {
            if (InvWarpCudgel.Checked)
            {
                Character.Tasks.Huntertask.Options.InvWarpCudgel = true;
            }
            else
                Character.Tasks.Huntertask.Options.InvWarpCudgel = false;
        }

        private void InvWarpring_CheckedChanged(object sender, EventArgs e)
        {
            if (InvWarpring.Checked)
            {
                Character.Tasks.Huntertask.Options.InvWarpRing = true;
            }
            else
                Character.Tasks.Huntertask.Options.InvWarpRing = false;
        }

        private void InvWarp_CheckedChanged(object sender, EventArgs e)
        {
            if (InvWarp.Checked)
            {
                Character.Tasks.Huntertask.Options.InvWarp = true;
            }
            else
                Character.Tasks.Huntertask.Options.InvWarp = false;
        }

        private void InvLogout_CheckedChanged(object sender, EventArgs e)
        {
            if (InvLogout.Checked)
            {
                Character.Tasks.Huntertask.Options.InvLogOut = true;
            }
            else
                Character.Tasks.Huntertask.Options.InvLogOut = false;
        }

        private void Clearjafightbtn_Click(object sender, EventArgs e)
        {
            Character.Tasks.Huntertask.Options.JobAbilityToUseOnlyDuringFights.Clear();
        }

        private void UseItemForSilenceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (UseItemForSilenceCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.useSilenceItem = true;
            }
            else
                Character.Tasks.Huntertask.Options.useSilenceItem = false;
        }

        private void UseItemForDoomCB_CheckedChanged(object sender, EventArgs e)
        {
            if (UseItemForDoomCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.useDoomItem = true;
            }
            else
                Character.Tasks.Huntertask.Options.useDoomItem = false;
        }

        private void SilenceItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.Tasks.Huntertask.Options.SilencedItem = SilenceItem.SelectedItem.ToString();
        }

        private void Doomitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.Tasks.Huntertask.Options.DoomItem = Doomitem.SelectedItem.ToString();
        }

        private void Clearativejabtn_Click(object sender, EventArgs e)
        {
            {
                Character.Tasks.Huntertask.Options.JobAbilityKeepActive.Clear();
            }
        }

        private void EliteMMoLinkButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.elitemmonetwork.com");
        }

        private void OnTopToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (OnTopToolStripMenuItem.CheckState == System.Windows.Forms.CheckState.Checked)
            {
                MainForm.ActiveForm.TopMost = true;
            }
            else
                MainForm.ActiveForm.TopMost = false;
        }

        private void TransparentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TransparentToolStripMenuItem.CheckState == System.Windows.Forms.CheckState.Checked)
            {
                MainForm.ActiveForm.Opacity = 0.50;
            }
            else
                MainForm.ActiveForm.Opacity = 1;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Character.SaveOrLoad.SaveSettings();
        }

        private void AshitaLinkl_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://ashita.atom0s.com/");
        }

        private void DarkStarLink_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://forums.dspt.info/index.php");
        }

        private void RecordBtn_Click(object sender, EventArgs e)
        {
            if (Character.Tasks.Huntertask.Options.RecordWaypoints && NaviTimer.Enabled)
            {
                Character.Tasks.Huntertask.Options.RecordWaypoints = false;
                NaviTimer.Enabled = false;
                RecordBtn.Text = "Record";
                Character.Logger.AddDebugText(rtbDebug, "Recording Stopped");
            }
            else if (!Character.Tasks.Huntertask.Options.RecordWaypoints && !NaviTimer.Enabled)
            {
                Character.Tasks.Huntertask.Options.RecordWaypoints = true;
                NaviTimer.Enabled = true;
                RecordBtn.Text = "Stop";
                Character.Logger.AddDebugText(rtbDebug, "Recording Waypoints");
            }
        }

        private void NaviTimer_Tick(object sender, EventArgs e)
        {
            if (!Character.OldPosition.Equals(Character.CurrentPosition))
            {
                if (Character.Tasks.Huntertask.Options.RecordWaypoints)
                {
                    Character.Navi.LearnRoutine();

                    var Item = (string.Format(@"{0}, {1}, {2}", Character.CurrentPosition.X.ToString(), Character.CurrentPosition.Y.ToString(), Character.CurrentPosition.Z.ToString()));
                    if (!WayPointListbox.Items.Contains(Item))
                    {
                        WayPointListbox.Items.Add(Item);
                    }
                }
            }
        }

        private void ClearWaypointsBtn_Click(object sender, EventArgs e)
        {
            Character.Navi.ClearWaypointsAndGrid();
            WayPointListbox.Items.Clear();
        }

        private void SaveWayPointsBtn_Click(object sender, EventArgs e)
        {
            Character.SaveOrLoad.SaveWayPoints();
        }

        private void LoadWayPointsBtn_Click(object sender, EventArgs e)
        {
            Character.Navi.LoadWaypoints();
        }
    }
}