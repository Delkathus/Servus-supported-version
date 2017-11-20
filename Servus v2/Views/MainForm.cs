using MetroFramework.Forms;
using Servus_v2.Common;
using System;
using System.Drawing;

namespace Servus_v2.Views
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            try
            {
                InitializeComponent();

                Logger = new Log();
                ffxiprocess = new ffxiProcess(this);
                CNF = new CheckNeededFiles(this);
                Check();
                ThemeComboBox.SelectedIndex = 0;
                StyleCombobox.SelectedIndex = 0;
                this.Text = string.Format(@"Servus v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            }
            catch (Exception ex)
            {
                Logger.AddDebugText(CheckedItemsRTB, ex.ToString());
                Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        public CheckNeededFiles CNF { get; set; }
        public ffxiProcess ffxiprocess { get; set; }
        public Log Logger { get; set; }

        public void Check()
        {
            try
            {
                if (CNF.DoWeHaveAllNeededFiles())
                {
                    ffxiprocess.GetProcess();
                }
            }
            catch (Exception ex)
            {
                Logger.AddDebugText(CheckedItemsRTB, ex.ToString());
            }
        }

        private void checkAPIForUpdatesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CNF.CheckAPI();
        }

        private void DonateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.elitemmonetwork.com/forums/donate.php");
            }
            catch (Exception ex)
            {
                Logger.AddDebugText(CheckedItemsRTB, ex.ToString());
                Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ffxiprocess._CharacterDictionary.Count < 1 && NextBtn.Text == "Refresh")
                {
                    NextBtn.Text = "Next";
                    ffxiprocess.GetProcess();
                }
                if (ffxiprocess._CharacterDictionary.Count > 0 && NextBtn.Text == "Next")
                {
                    ffxiprocess.AddToons();
                    DonatePanel.Visible = false;
                    checkAPIForUpdatesToolStripMenuItem1.Visible = false;
                    NextBtn.Visible = false;
                    CheckedItemsRTB.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogFile(ex.Message, FindForm().Name);
            }
        }

        private void StyleCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                try
                {
                    if (StyleCombobox.SelectedIndex != 0)
                    {
                        MetroStyler.Style = (MetroFramework.MetroColorStyle)Convert.ToInt32(StyleCombobox.SelectedIndex - 1);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogFile(ex.Message, FindForm().Name);
                }
            }
        }

        private void ThemeComboBox_Click(object sender, EventArgs e)
        {
        }

        private void ThemeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ThemeComboBox.SelectedIndex != 0)
                {
                    if (ThemeComboBox.SelectedItem.Equals("Dark"))
                    {
                        MetroStyler.Theme = MetroFramework.MetroThemeStyle.Dark;
                        menuStrip1.ForeColor = Color.White;
                        label2.ForeColor = Color.White;
                        changeThemeToolStripMenuItem.ForeColor = Color.White;
                    }
                    if (ThemeComboBox.SelectedItem.Equals("Light"))
                    {
                        MetroStyler.Theme = MetroFramework.MetroThemeStyle.Light;
                        changeThemeToolStripMenuItem.ForeColor = Color.Black;
                        menuStrip1.ForeColor = Color.Black;
                        label2.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogFile(ex.Message, FindForm().Name);
            }
        }
    }
}