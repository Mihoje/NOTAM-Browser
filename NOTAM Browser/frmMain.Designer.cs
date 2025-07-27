namespace NOTAM_Browser
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblStaticIcaoDesignators = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.grpFilters = new System.Windows.Forms.GroupBox();
            this.dtpFilterDatum = new System.Windows.Forms.DateTimePicker();
            this.chkFilterDatum = new System.Windows.Forms.CheckBox();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGetNotams = new System.Windows.Forms.Button();
            this.stsStripMain = new System.Windows.Forms.StatusStrip();
            this.slblData = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblLatestNotam = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.nOTAMiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prikaziProcitaneNOTAMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.podesavanjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.promeniPodešavanjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkFilterAck = new System.Windows.Forms.CheckBox();
            this.tooltipDesignators = new System.Windows.Forms.ToolTip(this.components);
            this.grpFilters.SuspendLayout();
            this.stsStripMain.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStaticIcaoDesignators
            // 
            this.lblStaticIcaoDesignators.AutoSize = true;
            this.lblStaticIcaoDesignators.Location = new System.Drawing.Point(12, 30);
            this.lblStaticIcaoDesignators.Name = "lblStaticIcaoDesignators";
            this.lblStaticIcaoDesignators.Size = new System.Drawing.Size(89, 13);
            this.lblStaticIcaoDesignators.TabIndex = 0;
            this.lblStaticIcaoDesignators.Text = "ICAO designators";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(107, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(161, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // grpFilters
            // 
            this.grpFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFilters.Controls.Add(this.dtpFilterDatum);
            this.grpFilters.Controls.Add(this.chkFilterDatum);
            this.grpFilters.Location = new System.Drawing.Point(291, 30);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new System.Drawing.Size(323, 44);
            this.grpFilters.TabIndex = 2;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Filteri";
            // 
            // dtpFilterDatum
            // 
            this.dtpFilterDatum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFilterDatum.Location = new System.Drawing.Point(99, 14);
            this.dtpFilterDatum.Name = "dtpFilterDatum";
            this.dtpFilterDatum.Size = new System.Drawing.Size(200, 20);
            this.dtpFilterDatum.TabIndex = 1;
            this.dtpFilterDatum.ValueChanged += new System.EventHandler(this.dtpFilterDatum_ValueChanged);
            // 
            // chkFilterDatum
            // 
            this.chkFilterDatum.AutoSize = true;
            this.chkFilterDatum.Location = new System.Drawing.Point(6, 19);
            this.chkFilterDatum.Name = "chkFilterDatum";
            this.chkFilterDatum.Size = new System.Drawing.Size(87, 17);
            this.chkFilterDatum.TabIndex = 0;
            this.chkFilterDatum.Text = "Filtriraj datum";
            this.chkFilterDatum.UseVisualStyleBackColor = true;
            this.chkFilterDatum.CheckedChanged += new System.EventHandler(this.chkFilterDatum_CheckedChanged);
            // 
            // tlpMain
            // 
            this.tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.AutoScroll = true;
            this.tlpMain.AutoScrollMargin = new System.Drawing.Size(0, 20);
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Location = new System.Drawing.Point(15, 88);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0, 0, 23, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(3);
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(599, 419);
            this.tlpMain.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(521, 510);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(93, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Sačuvaj";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetNotams
            // 
            this.btnGetNotams.Location = new System.Drawing.Point(156, 51);
            this.btnGetNotams.Name = "btnGetNotams";
            this.btnGetNotams.Size = new System.Drawing.Size(112, 23);
            this.btnGetNotams.TabIndex = 5;
            this.btnGetNotams.Text = "Učitaj NOTAM-e";
            this.btnGetNotams.UseVisualStyleBackColor = true;
            this.btnGetNotams.Click += new System.EventHandler(this.btnGetNotams_Click);
            // 
            // stsStripMain
            // 
            this.stsStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slblData,
            this.slblLatestNotam});
            this.stsStripMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.stsStripMain.Location = new System.Drawing.Point(0, 536);
            this.stsStripMain.Name = "stsStripMain";
            this.stsStripMain.Size = new System.Drawing.Size(626, 22);
            this.stsStripMain.TabIndex = 6;
            this.stsStripMain.Text = "statusStrip1";
            // 
            // slblData
            // 
            this.slblData.Name = "slblData";
            this.slblData.Size = new System.Drawing.Size(49, 17);
            this.slblData.Text = "slblData";
            // 
            // slblLatestNotam
            // 
            this.slblLatestNotam.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.slblLatestNotam.Name = "slblLatestNotam";
            this.slblLatestNotam.Size = new System.Drawing.Size(149, 17);
            this.slblLatestNotam.Text = "Poslednje ažuriranje: Nikad";
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nOTAMiToolStripMenuItem,
            this.podesavanjaToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(626, 24);
            this.menuMain.TabIndex = 7;
            this.menuMain.Text = "menuStrip1";
            // 
            // nOTAMiToolStripMenuItem
            // 
            this.nOTAMiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prikaziProcitaneNOTAMeToolStripMenuItem});
            this.nOTAMiToolStripMenuItem.Name = "nOTAMiToolStripMenuItem";
            this.nOTAMiToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.nOTAMiToolStripMenuItem.Text = "NOTAM";
            // 
            // prikaziProcitaneNOTAMeToolStripMenuItem
            // 
            this.prikaziProcitaneNOTAMeToolStripMenuItem.Name = "prikaziProcitaneNOTAMeToolStripMenuItem";
            this.prikaziProcitaneNOTAMeToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.prikaziProcitaneNOTAMeToolStripMenuItem.Text = "Prikaži pročitane NOTAM-e";
            this.prikaziProcitaneNOTAMeToolStripMenuItem.Click += new System.EventHandler(this.prikaziProcitaneNOTAMeToolStripMenuItem_Click);
            // 
            // podesavanjaToolStripMenuItem
            // 
            this.podesavanjaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontToolStripMenuItem,
            this.promeniPodešavanjaToolStripMenuItem});
            this.podesavanjaToolStripMenuItem.Name = "podesavanjaToolStripMenuItem";
            this.podesavanjaToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.podesavanjaToolStripMenuItem.Text = "Podešavanja";
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.fontToolStripMenuItem.Text = "Font...";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // promeniPodešavanjaToolStripMenuItem
            // 
            this.promeniPodešavanjaToolStripMenuItem.Name = "promeniPodešavanjaToolStripMenuItem";
            this.promeniPodešavanjaToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.promeniPodešavanjaToolStripMenuItem.Text = "Napredna podešavanja...";
            this.promeniPodešavanjaToolStripMenuItem.Click += new System.EventHandler(this.promeniPodešavanjaToolStripMenuItem_Click);
            // 
            // chkFilterAck
            // 
            this.chkFilterAck.AutoSize = true;
            this.chkFilterAck.Location = new System.Drawing.Point(15, 53);
            this.chkFilterAck.Name = "chkFilterAck";
            this.chkFilterAck.Size = new System.Drawing.Size(99, 17);
            this.chkFilterAck.TabIndex = 2;
            this.chkFilterAck.Text = "Sakrij pročitane";
            this.chkFilterAck.UseVisualStyleBackColor = true;
            this.chkFilterAck.CheckedChanged += new System.EventHandler(this.chkFilterAck_CheckedChanged);
            // 
            // tooltipDesignators
            // 
            this.tooltipDesignators.AutoPopDelay = 5000;
            this.tooltipDesignators.InitialDelay = 100;
            this.tooltipDesignators.IsBalloon = true;
            this.tooltipDesignators.ReshowDelay = 100;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 558);
            this.Controls.Add(this.chkFilterAck);
            this.Controls.Add(this.stsStripMain);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.btnGetNotams);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.grpFilters);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblStaticIcaoDesignators);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(642, 377);
            this.Name = "frmMain";
            this.Text = "NOTAMI";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpFilters.ResumeLayout(false);
            this.grpFilters.PerformLayout();
            this.stsStripMain.ResumeLayout(false);
            this.stsStripMain.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStaticIcaoDesignators;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.GroupBox grpFilters;
        private System.Windows.Forms.DateTimePicker dtpFilterDatum;
        private System.Windows.Forms.CheckBox chkFilterDatum;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGetNotams;
        private System.Windows.Forms.StatusStrip stsStripMain;
        private System.Windows.Forms.ToolStripStatusLabel slblLatestNotam;
        private System.Windows.Forms.ToolStripStatusLabel slblData;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem nOTAMiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prikaziProcitaneNOTAMeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem podesavanjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkFilterAck;
        private System.Windows.Forms.ToolStripMenuItem promeniPodešavanjaToolStripMenuItem;
        private System.Windows.Forms.ToolTip tooltipDesignators;
    }
}

