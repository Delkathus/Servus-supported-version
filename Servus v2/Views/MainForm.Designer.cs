namespace Servus_v2.Views
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.NextBtn = new MetroFramework.Controls.MetroButton();
            this.ToonPanel = new MetroFramework.Controls.MetroPanel();
            this.CheckedItemsRTB = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DonatePanel = new MetroFramework.Controls.MetroPanel();
            this.DonateBtn = new MetroFramework.Controls.MetroButton();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.changeThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ThemeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.StyleCombobox = new System.Windows.Forms.ToolStripComboBox();
            this.checkAPIForUpdatesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MetroStyler = new MetroFramework.Components.MetroStyleManager(this.components);
            this.DonatePanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetroStyler)).BeginInit();
            this.SuspendLayout();
            // 
            // NextBtn
            // 
            this.NextBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.NextBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NextBtn.Location = new System.Drawing.Point(0, 598);
            this.NextBtn.Margin = new System.Windows.Forms.Padding(4);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(972, 37);
            this.NextBtn.TabIndex = 1;
            this.NextBtn.Text = "Next";
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // ToonPanel
            // 
            this.ToonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToonPanel.HorizontalScrollbarBarColor = true;
            this.ToonPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.ToonPanel.HorizontalScrollbarSize = 12;
            this.ToonPanel.Location = new System.Drawing.Point(0, 76);
            this.ToonPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ToonPanel.Name = "ToonPanel";
            this.ToonPanel.Padding = new System.Windows.Forms.Padding(1);
            this.ToonPanel.Size = new System.Drawing.Size(972, 559);
            this.ToonPanel.TabIndex = 4;
            this.ToonPanel.VerticalScrollbarBarColor = true;
            this.ToonPanel.VerticalScrollbarHighlightOnWheel = false;
            this.ToonPanel.VerticalScrollbarSize = 13;
            // 
            // CheckedItemsRTB
            // 
            this.CheckedItemsRTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.CheckedItemsRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckedItemsRTB.Location = new System.Drawing.Point(0, 128);
            this.CheckedItemsRTB.Margin = new System.Windows.Forms.Padding(0);
            this.CheckedItemsRTB.Name = "CheckedItemsRTB";
            this.CheckedItemsRTB.Size = new System.Drawing.Size(972, 470);
            this.CheckedItemsRTB.TabIndex = 5;
            this.CheckedItemsRTB.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(350, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(521, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Please consider donating to EliteMMO, without them this application wouldn’t work" +
    "";
            // 
            // DonatePanel
            // 
            this.DonatePanel.BackColor = System.Drawing.Color.Transparent;
            this.DonatePanel.Controls.Add(this.label2);
            this.DonatePanel.Controls.Add(this.DonateBtn);
            this.DonatePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DonatePanel.HorizontalScrollbarBarColor = true;
            this.DonatePanel.HorizontalScrollbarHighlightOnWheel = false;
            this.DonatePanel.HorizontalScrollbarSize = 12;
            this.DonatePanel.Location = new System.Drawing.Point(0, 103);
            this.DonatePanel.Margin = new System.Windows.Forms.Padding(0);
            this.DonatePanel.Name = "DonatePanel";
            this.DonatePanel.Padding = new System.Windows.Forms.Padding(1);
            this.DonatePanel.Size = new System.Drawing.Size(972, 25);
            this.DonatePanel.TabIndex = 7;
            this.DonatePanel.VerticalScrollbarBarColor = true;
            this.DonatePanel.VerticalScrollbarHighlightOnWheel = false;
            this.DonatePanel.VerticalScrollbarSize = 13;
            // 
            // DonateBtn
            // 
            this.DonateBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.DonateBtn.Location = new System.Drawing.Point(871, 1);
            this.DonateBtn.Margin = new System.Windows.Forms.Padding(0);
            this.DonateBtn.Name = "DonateBtn";
            this.DonateBtn.Size = new System.Drawing.Size(100, 23);
            this.DonateBtn.TabIndex = 7;
            this.DonateBtn.Text = "Donate";
            this.DonateBtn.Click += new System.EventHandler(this.DonateBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label1.Location = new System.Drawing.Point(60, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "By Xenonsmurf";
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeThemeToolStripMenuItem,
            this.checkAPIForUpdatesToolStripMenuItem1});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 76);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip1.Size = new System.Drawing.Size(972, 27);
            this.menuStrip1.Stretch = false;
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // changeThemeToolStripMenuItem
            // 
            this.changeThemeToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.changeThemeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ThemeComboBox,
            this.StyleCombobox});
            this.changeThemeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.changeThemeToolStripMenuItem.Name = "changeThemeToolStripMenuItem";
            this.changeThemeToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2);
            this.changeThemeToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.changeThemeToolStripMenuItem.Size = new System.Drawing.Size(109, 27);
            this.changeThemeToolStripMenuItem.Text = "&Change Theme";
            // 
            // ThemeComboBox
            // 
            this.ThemeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThemeComboBox.Items.AddRange(new object[] {
            "Theme",
            "Dark",
            "Light"});
            this.ThemeComboBox.Name = "ThemeComboBox";
            this.ThemeComboBox.Size = new System.Drawing.Size(121, 28);
            this.ThemeComboBox.SelectedIndexChanged += new System.EventHandler(this.ThemeComboBox_SelectedIndexChanged);
            this.ThemeComboBox.Click += new System.EventHandler(this.ThemeComboBox_Click);
            // 
            // StyleCombobox
            // 
            this.StyleCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StyleCombobox.Items.AddRange(new object[] {
            "Style",
            "Black",
            "White",
            "Silver",
            "Blue",
            "Green",
            "Lime",
            "Teal",
            "Orange",
            "Brown",
            "Pink",
            "Magenta",
            "Purple",
            "Red",
            "Yellow"});
            this.StyleCombobox.Name = "StyleCombobox";
            this.StyleCombobox.Size = new System.Drawing.Size(121, 28);
            this.StyleCombobox.SelectedIndexChanged += new System.EventHandler(this.StyleCombobox_SelectedIndexChanged);
            // 
            // checkAPIForUpdatesToolStripMenuItem1
            // 
            this.checkAPIForUpdatesToolStripMenuItem1.Name = "checkAPIForUpdatesToolStripMenuItem1";
            this.checkAPIForUpdatesToolStripMenuItem1.Size = new System.Drawing.Size(166, 24);
            this.checkAPIForUpdatesToolStripMenuItem1.Text = "Check API for updates";
            this.checkAPIForUpdatesToolStripMenuItem1.Click += new System.EventHandler(this.checkAPIForUpdatesToolStripMenuItem1_Click);
            // 
            // MetroStyler
            // 
            this.MetroStyler.Owner = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 635);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckedItemsRTB);
            this.Controls.Add(this.DonatePanel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.ToonPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 76, 0, 0);
            this.Text = "Servus ";
            this.DonatePanel.ResumeLayout(false);
            this.DonatePanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetroStyler)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public MetroFramework.Controls.MetroButton NextBtn;
        public System.Windows.Forms.RichTextBox CheckedItemsRTB;
        public MetroFramework.Controls.MetroPanel ToonPanel;
        public System.Windows.Forms.Label label1;
        public MetroFramework.Controls.MetroPanel DonatePanel;
        public System.Windows.Forms.Label label2;
        public MetroFramework.Controls.MetroButton DonateBtn;
        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem changeThemeToolStripMenuItem;
        public System.Windows.Forms.ToolStripComboBox ThemeComboBox;
        public System.Windows.Forms.ToolStripComboBox StyleCombobox;
        public MetroFramework.Components.MetroStyleManager MetroStyler;
        private System.Windows.Forms.ToolStripMenuItem checkAPIForUpdatesToolStripMenuItem1;
    }
}

