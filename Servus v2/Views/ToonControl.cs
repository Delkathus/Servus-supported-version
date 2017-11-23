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
            Character.SaveSettings();
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
            WPLoadedLB.Text = "--";
        }

        private void SaveWayPointsBtn_Click(object sender, EventArgs e)
        {
            Character.SaveOrLoad.SaveWayPoints();
        }

        private void LoadWayPointsBtn_Click(object sender, EventArgs e)
        {
            Character.SaveOrLoad.LoadWaypoints();
        }

        private void FailedUPdown_ValueChanged(object sender, EventArgs e)
        {
            Character.Tasks.Huntertask.Options.FailedToPathCount = (int)FailedUPdown.Value;
        }

        private void IdleDelayValue_ValueChanged(object sender, EventArgs e)
        {
            Character.Tasks.Huntertask.Options.IdleDelay = (int)IdleDelayValue.Value;
        }

        private void AttackDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (AttackDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseAttackDown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseAttackDown = false;
        }

        private void AddleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (AddleCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseAddle = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseAddle = false;
        }

        private void AccuracyDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (AccuracyDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseAccuracyDown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseAccuracyDown = false;
        }

        private void AgiDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (AgiDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseAGIdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseAGIdown = false;
        }

        private void BlindnessCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BlindnessCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBlindness = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBlindness = false;
        }

        private void BurnCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BurnCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBurn = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBurn = false;
        }

        private void BindCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BindCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBind = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBind = false;
        }

        private void BioCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BioCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBio = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBio = false;
        }

        private void BaneCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BaneCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBane = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBane = false;
        }

        private void ChokeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ChokeCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseChoke = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseChoke = false;
        }

        private void ChrDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ChrDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseCHRdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseCHRdown = false;
        }

        private void CurseCB_CheckedChanged(object sender, EventArgs e)
        {
            if (CurseCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseCurse = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseCurse = false;
        }

        private void Curse2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Curse2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseCurse2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseCurse2 = false;
        }

        private void DrownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DrownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseDrown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseDrown = false;
        }

        private void DoomCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DoomCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseDoom = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseDoom = false;
        }

        private void DiaCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DiaCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseDia = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseDia = false;
        }

        private void DefenseDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DefenseDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseDefenseDown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseDefenseDown = false;
        }

        private void DexDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DexDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseDEXdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseDEXdown = false;
        }

        private void ElegyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ElegyCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseElegy = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseElegy = false;
        }

        private void EvasionDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (EvasionDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseEvaDown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseEvaDown = false;
        }

        private void FlashCB_CheckedChanged(object sender, EventArgs e)
        {
            if (FlashCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseFlash = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseFlash = false;
        }

        private void FrostCB_CheckedChanged(object sender, EventArgs e)
        {
            if (FrostCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseFlash = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseFlash = false;
        }

        private void HelixCB_CheckedChanged(object sender, EventArgs e)
        {
            if (HelixCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseHelix = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseHelix = false;
        }

        private void IntDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (IntDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseIntDown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseIntDown = false;
        }

        private void MndDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MndDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseMNDdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseMNDdown = false;
        }

        private void MagicAccDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MagicAccDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseMagACC = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseMagACC = false;
        }

        private void MagicAtkDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MagicAtkDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseMagATK = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseMagATK = false;
        }

        private void MaxHpDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MaxHpDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseHPdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseHPdown = false;
        }

        private void MaxTpDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MaxTpDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseTPdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseTPdown = false;
        }

        private void MaxMpDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MaxMpDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseMPdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseMPdown = false;
        }

        private void ParalysisCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ParalysisCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseParalysis = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseParalysis = false;
        }

        private void PlagueCB_CheckedChanged(object sender, EventArgs e)
        {
            if (PlagueCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePlague = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePlague = false;
        }

        private void PoisonCB_CheckedChanged(object sender, EventArgs e)
        {
            if (PoisonCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePoison = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePoison = false;
        }

        private void RaspCB_CheckedChanged(object sender, EventArgs e)
        {
            if (RaspCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRasp = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRasp = false;
        }

        private void RequiemCB_CheckedChanged(object sender, EventArgs e)
        {
            if (RequiemCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRequiem = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRequiem = false;
        }

        private void StrDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (StrDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseSTRdown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseSTRdown = false;
        }

        private void ShockCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ShockCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShock = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShock = false;
        }

        private void SilenceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (SilenceCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseSilence = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseSilence = false;
        }

        private void SlowCB_CheckedChanged(object sender, EventArgs e)
        {
            if (SlowCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseSlow = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseSlow = false;
        }

        private void ThrenodyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ThrenodyCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseThrenody = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseThrenody = false;
        }

        private void VitDownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (VitDownCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseVitDown = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseVitDown = false;
        }

        private void WeightCB_CheckedChanged(object sender, EventArgs e)
        {
            if (WeightCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseWeight = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseWeight = false;
        }

        private void AquaveilCB_CheckedChanged(object sender, EventArgs e)
        {
            if (AquaveilCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseAquaveil = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseAquaveil = false;
        }

        private void BlinkCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BlinkCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBlink = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBlink = false;
        }

        private void Hhaste_CheckedChanged(object sender, EventArgs e)
        {
            if (Hhaste.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseHasteSamba = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseHasteSamba = false;
        }

        private void HasteCB_CheckedChanged(object sender, EventArgs e)
        {
            if (HasteCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseHaste = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseHaste = false;
        }

        private void Haste2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Haste2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseHaste2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseHaste2 = false;
        }

        private void PhalanxCB_CheckedChanged(object sender, EventArgs e)
        {
            if (PhalanxCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePhalanx = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePhalanx = false;
        }

        private void ProtectCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ProtectCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePro = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePro = false;
        }

        private void Protect2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Protect2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePro2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePro2 = false;
        }

        private void Protect3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Protect3CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePro3 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePro3 = false;
        }

        private void Protect4CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Protect4CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePro4 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePro4 = false;
        }

        private void Protect5CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Protect5CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UsePro5 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UsePro5 = false;
        }

        private void Refresh1CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Refresh1CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRefresh = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRefresh = false;
        }

        private void Refresh2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Refresh2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRefresh2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRefresh2 = false;
        }

        private void Refresh3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Refresh3CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRefresh3 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRefresh3 = false;
        }

        private void Regen1CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Regen1CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRegen = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRegen = false;
        }

        private void Regen2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Regen2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRegen2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRegen2 = false;
        }

        private void Regen3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Regen3CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRegen3 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRegen3 = false;
        }

        private void Regen5CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Regen5CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRegen5 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRegen5 = false;
        }

        private void Reraise1CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Reraise1CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseReraise = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseReraise = false;
        }

        private void Reraise2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Reraise2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseReraise2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseReraise2 = false;
        }

        private void Reraise3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Reraise3CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseReraise3 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseReraise3 = false;
        }

        private void Reraise4CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Reraise4CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseReraise4 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseReraise4 = false;
        }

        private void Shell1CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Shell1CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShell = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShell = false;
        }

        private void Shell2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Shell2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShell2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShell2 = false;
        }

        private void Shell3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Shell3CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShell3 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShell3 = false;
        }

        private void Shell4CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Shell4CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShell4 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShell4 = false;
        }

        private void Shell5CB_CheckedChanged(object sender, EventArgs e)
        {
            if (shellra5CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShell5 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShell5 = false;
        }

        private void StoneskinCB_CheckedChanged(object sender, EventArgs e)
        {
            if (StoneskinCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseStoneSkin = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseStoneSkin = false;
        }

        private void BlazeSpikesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BlazeSpikesCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBlazeSpikes = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBlazeSpikes = false;
        }

        private void IceSPikesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (IceSPikesCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseIceSpikes = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseIceSpikes = false;
        }

        private void ShockSpikesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ShockSpikesCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShockSPikes = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShockSPikes = false;
        }

        private void ShellraCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ShellraCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShellra = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShellra = false;
        }

        private void Shellra2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Shellra2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShellra2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShellra2 = false;
        }

        private void shellra3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (shellra3CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShellra3 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShellra3 = false;
        }

        private void Shellra4CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Shellra4CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShellra4 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShellra4 = false;
        }

        private void shellra5CB_CheckedChanged(object sender, EventArgs e)
        {
            if (shellra5CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseShellra5 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseShellra5 = false;
        }

        private void ProtectraCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ProtectraCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseProtectra = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseProtectra = false;
        }

        private void Protectra2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Protectra2CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseProtectra2 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseProtectra2 = false;
        }

        private void protectra3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (protectra3CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseProtectra3 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseProtectra3 = false;
        }

        private void Protectra4CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Protectra4CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseProtectra4 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseProtectra4 = false;
        }

        private void Protectra5CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Protectra5CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseProtectra5 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseProtectra5 = false;
        }

        private void BoostAGICB_CheckedChanged(object sender, EventArgs e)
        {
            if (BoostAGICB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBoostagi = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBoostagi = false;
        }

        private void BoostCHRCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BoostCHRCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBoostchr = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBoostchr = false;
        }

        private void BoostDEXCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BoostDEXCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBoostdex = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBoostdex = false;
        }

        private void BoostINTCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BoostINTCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBoostint = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBoostint = false;
        }

        private void BoostSTRCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BoostSTRCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBooststr = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBooststr = false;
        }

        private void BoostMNDCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BoostMNDCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBoostmnd = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBoostmnd = false;
        }

        private void BoostVITCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BoostVITCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseBoostvit = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseBoostvit = false;
        }

        private void UtsusemiNiCB_CheckedChanged(object sender, EventArgs e)
        {
            if (UtsusemiNiCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseNi = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseNi = false;
        }

        private void UtsusemiIchiCB_CheckedChanged(object sender, EventArgs e)
        {
            if (UtsusemiIchiCB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseIchi = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseIchi = false;
        }

        private void Regen4CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Regen4CB.CheckState.Equals(CheckState.Checked))
            {
                Character.Tasks.Huntertask.Options.UseRegen4 = true;
            }
            else
                Character.Tasks.Huntertask.Options.UseRegen4 = false;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Character.LoadSettings();
        }
    }
}