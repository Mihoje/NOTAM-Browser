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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMap));
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.chkMapElements = new System.Windows.Forms.CheckedListBox();
            this.btnAllOn = new System.Windows.Forms.Button();
            this.btnAllOff = new System.Windows.Forms.Button();
            this.cmbProvider = new System.Windows.Forms.ComboBox();
            this.lblStaticMapDropdown = new System.Windows.Forms.Label();
            this.btnLoadPolys = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.gMapControl1.Location = new System.Drawing.Point(12, 42);
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
            this.gMapControl1.Size = new System.Drawing.Size(643, 448);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 9D;
            // 
            // chkMapElements
            // 
            this.chkMapElements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMapElements.FormattingEnabled = true;
            this.chkMapElements.Location = new System.Drawing.Point(661, 42);
            this.chkMapElements.Name = "chkMapElements";
            this.chkMapElements.Size = new System.Drawing.Size(189, 409);
            this.chkMapElements.TabIndex = 1;
            this.chkMapElements.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkMapElements_ItemCheck);
            // 
            // btnAllOn
            // 
            this.btnAllOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllOn.Location = new System.Drawing.Point(661, 467);
            this.btnAllOn.Name = "btnAllOn";
            this.btnAllOn.Size = new System.Drawing.Size(75, 23);
            this.btnAllOn.TabIndex = 2;
            this.btnAllOn.Text = "Uključi sve";
            this.btnAllOn.UseVisualStyleBackColor = true;
            this.btnAllOn.Click += new System.EventHandler(this.btnAllOn_Click);
            // 
            // btnAllOff
            // 
            this.btnAllOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllOff.Location = new System.Drawing.Point(775, 467);
            this.btnAllOff.Name = "btnAllOff";
            this.btnAllOff.Size = new System.Drawing.Size(75, 23);
            this.btnAllOff.TabIndex = 3;
            this.btnAllOff.Text = "Isključi sve";
            this.btnAllOff.UseVisualStyleBackColor = true;
            this.btnAllOff.Click += new System.EventHandler(this.btnAllOff_Click);
            // 
            // cmbProvider
            // 
            this.cmbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvider.FormattingEnabled = true;
            this.cmbProvider.Location = new System.Drawing.Point(103, 9);
            this.cmbProvider.Name = "cmbProvider";
            this.cmbProvider.Size = new System.Drawing.Size(171, 21);
            this.cmbProvider.TabIndex = 4;
            this.cmbProvider.SelectedValueChanged += new System.EventHandler(this.cmbProvider_SelectedValueChanged);
            // 
            // lblStaticMapDropdown
            // 
            this.lblStaticMapDropdown.AutoSize = true;
            this.lblStaticMapDropdown.Location = new System.Drawing.Point(12, 12);
            this.lblStaticMapDropdown.Name = "lblStaticMapDropdown";
            this.lblStaticMapDropdown.Size = new System.Drawing.Size(85, 13);
            this.lblStaticMapDropdown.TabIndex = 5;
            this.lblStaticMapDropdown.Text = "Dostupne mape:";
            // 
            // btnLoadPolys
            // 
            this.btnLoadPolys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadPolys.Location = new System.Drawing.Point(748, 9);
            this.btnLoadPolys.Name = "btnLoadPolys";
            this.btnLoadPolys.Size = new System.Drawing.Size(102, 23);
            this.btnLoadPolys.TabIndex = 6;
            this.btnLoadPolys.Text = "Učitaj mape...";
            this.btnLoadPolys.UseVisualStyleBackColor = true;
            this.btnLoadPolys.Click += new System.EventHandler(this.btnLoadPolys_Click);
            // 
            // frmMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 502);
            this.Controls.Add(this.btnLoadPolys);
            this.Controls.Add(this.lblStaticMapDropdown);
            this.Controls.Add(this.cmbProvider);
            this.Controls.Add(this.btnAllOff);
            this.Controls.Add(this.btnAllOn);
            this.Controls.Add(this.chkMapElements);
            this.Controls.Add(this.gMapControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMap";
            this.Text = "Mapa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMap_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.CheckedListBox chkMapElements;
        private System.Windows.Forms.Button btnAllOn;
        private System.Windows.Forms.Button btnAllOff;
        private System.Windows.Forms.ComboBox cmbProvider;
        private System.Windows.Forms.Label lblStaticMapDropdown;
        private System.Windows.Forms.Button btnLoadPolys;
    }
}