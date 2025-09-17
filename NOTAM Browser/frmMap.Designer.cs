namespace NOTAM_Browser
{
    partial class frmMap
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
                this.printManager.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMap));
            this.cmbProvider = new System.Windows.Forms.ComboBox();
            this.lblStaticMapDropdown = new System.Windows.Forms.Label();
            this.btnLoadPolys = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.numZoomLevel = new System.Windows.Forms.NumericUpDown();
            this.lblStaticZoomLevel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stsZoomLevel = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.listTabControl = new System.Windows.Forms.TabControl();
            this.ctxChkList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.promeniBojuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delPolyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.zoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upravljajZonamaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDisplayLevels = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numZoomLevel)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.ctxChkList.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbProvider
            // 
            this.cmbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvider.FormattingEnabled = true;
            this.cmbProvider.Location = new System.Drawing.Point(103, 27);
            this.cmbProvider.Name = "cmbProvider";
            this.cmbProvider.Size = new System.Drawing.Size(171, 21);
            this.cmbProvider.TabIndex = 4;
            this.cmbProvider.SelectedValueChanged += new System.EventHandler(this.cmbProvider_SelectedValueChanged);
            // 
            // lblStaticMapDropdown
            // 
            this.lblStaticMapDropdown.AutoSize = true;
            this.lblStaticMapDropdown.Location = new System.Drawing.Point(12, 30);
            this.lblStaticMapDropdown.Name = "lblStaticMapDropdown";
            this.lblStaticMapDropdown.Size = new System.Drawing.Size(85, 13);
            this.lblStaticMapDropdown.TabIndex = 5;
            this.lblStaticMapDropdown.Text = "Dostupne mape:";
            // 
            // btnLoadPolys
            // 
            this.btnLoadPolys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadPolys.Location = new System.Drawing.Point(874, 27);
            this.btnLoadPolys.Name = "btnLoadPolys";
            this.btnLoadPolys.Size = new System.Drawing.Size(102, 23);
            this.btnLoadPolys.TabIndex = 6;
            this.btnLoadPolys.Text = "Učitaj mape...";
            this.btnLoadPolys.UseVisualStyleBackColor = true;
            this.btnLoadPolys.Click += new System.EventHandler(this.btnLoadPolys_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(435, 27);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(81, 23);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Štampaj";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // numZoomLevel
            // 
            this.numZoomLevel.Location = new System.Drawing.Point(380, 28);
            this.numZoomLevel.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numZoomLevel.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numZoomLevel.Name = "numZoomLevel";
            this.numZoomLevel.Size = new System.Drawing.Size(49, 20);
            this.numZoomLevel.TabIndex = 8;
            this.numZoomLevel.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblStaticZoomLevel
            // 
            this.lblStaticZoomLevel.AutoSize = true;
            this.lblStaticZoomLevel.Location = new System.Drawing.Point(280, 30);
            this.lblStaticZoomLevel.Name = "lblStaticZoomLevel";
            this.lblStaticZoomLevel.Size = new System.Drawing.Size(94, 13);
            this.lblStaticZoomLevel.TabIndex = 9;
            this.lblStaticZoomLevel.Text = "Željeni zoom level:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsZoomLevel,
            this.stsPosition});
            this.statusStrip1.Location = new System.Drawing.Point(0, 643);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(988, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stsZoomLevel
            // 
            this.stsZoomLevel.Name = "stsZoomLevel";
            this.stsZoomLevel.Size = new System.Drawing.Size(53, 17);
            this.stsZoomLevel.Text = "stsZoom";
            // 
            // stsPosition
            // 
            this.stsPosition.Name = "stsPosition";
            this.stsPosition.Size = new System.Drawing.Size(64, 17);
            this.stsPosition.Text = "stsPosition";
            // 
            // listTabControl
            // 
            this.listTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTabControl.Location = new System.Drawing.Point(674, 83);
            this.listTabControl.Name = "listTabControl";
            this.listTabControl.SelectedIndex = 0;
            this.listTabControl.Size = new System.Drawing.Size(302, 549);
            this.listTabControl.TabIndex = 11;
            // 
            // ctxChkList
            // 
            this.ctxChkList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.promeniBojuToolStripMenuItem,
            this.delPolyToolStripMenuItem});
            this.ctxChkList.Name = "ctxDelete";
            this.ctxChkList.Size = new System.Drawing.Size(181, 70);
            // 
            // promeniBojuToolStripMenuItem
            // 
            this.promeniBojuToolStripMenuItem.Name = "promeniBojuToolStripMenuItem";
            this.promeniBojuToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.promeniBojuToolStripMenuItem.Text = "Promeni boju...";
            this.promeniBojuToolStripMenuItem.Click += new System.EventHandler(this.promeniBojuToolStripMenuItem_Click);
            // 
            // delPolyToolStripMenuItem
            // 
            this.delPolyToolStripMenuItem.Name = "delPolyToolStripMenuItem";
            this.delPolyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.delPolyToolStripMenuItem.Text = "Obriši selektovano";
            this.delPolyToolStripMenuItem.Click += new System.EventHandler(this.obrišToolStripMenuItem_Click);
            // 
            // gMapControl1
            // 
            this.gMapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(12, 83);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 18;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomEnabled = true;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(656, 549);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 9D;
            this.gMapControl1.OnPositionChanged += new GMap.NET.PositionChanged(this.gMapControl1_OnPositionChanged);
            this.gMapControl1.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.gMapControl1_OnMapZoomChanged);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoneToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(988, 24);
            this.menuMain.TabIndex = 12;
            this.menuMain.Text = "menuStrip1";
            // 
            // zoneToolStripMenuItem
            // 
            this.zoneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upravljajZonamaToolStripMenuItem});
            this.zoneToolStripMenuItem.Name = "zoneToolStripMenuItem";
            this.zoneToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.zoneToolStripMenuItem.Text = "Zone";
            // 
            // upravljajZonamaToolStripMenuItem
            // 
            this.upravljajZonamaToolStripMenuItem.Name = "upravljajZonamaToolStripMenuItem";
            this.upravljajZonamaToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.upravljajZonamaToolStripMenuItem.Text = "Upravljaj zonama...";
            this.upravljajZonamaToolStripMenuItem.Click += new System.EventHandler(this.upravljajZonamaToolStripMenuItem_Click);
            // 
            // chkDisplayLevels
            // 
            this.chkDisplayLevels.AutoSize = true;
            this.chkDisplayLevels.Location = new System.Drawing.Point(12, 58);
            this.chkDisplayLevels.Name = "chkDisplayLevels";
            this.chkDisplayLevels.Size = new System.Drawing.Size(144, 17);
            this.chkDisplayLevels.TabIndex = 13;
            this.chkDisplayLevels.Text = "Prikaži vertikalne granice";
            this.chkDisplayLevels.UseVisualStyleBackColor = true;
            this.chkDisplayLevels.CheckedChanged += new System.EventHandler(this.chkDisplayLevels_CheckedChanged);
            // 
            // frmMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 665);
            this.Controls.Add(this.chkDisplayLevels);
            this.Controls.Add(this.listTabControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.lblStaticZoomLevel);
            this.Controls.Add(this.numZoomLevel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnLoadPolys);
            this.Controls.Add(this.lblStaticMapDropdown);
            this.Controls.Add(this.cmbProvider);
            this.Controls.Add(this.gMapControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(655, 482);
            this.Name = "frmMap";
            this.Text = "Mapa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMap_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numZoomLevel)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ctxChkList.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.ComboBox cmbProvider;
        private System.Windows.Forms.Label lblStaticMapDropdown;
        private System.Windows.Forms.Button btnLoadPolys;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.NumericUpDown numZoomLevel;
        private System.Windows.Forms.Label lblStaticZoomLevel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stsZoomLevel;
        private System.Windows.Forms.ToolStripStatusLabel stsPosition;
        private System.Windows.Forms.TabControl listTabControl;
        private System.Windows.Forms.ContextMenuStrip ctxChkList;
        private System.Windows.Forms.ToolStripMenuItem delPolyToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem zoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upravljajZonamaToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkDisplayLevels;
        private System.Windows.Forms.ToolStripMenuItem promeniBojuToolStripMenuItem;
    }
}