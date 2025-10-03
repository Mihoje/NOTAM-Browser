namespace NOTAM_Browser
{
    partial class frmSettings
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtUrlPre = new System.Windows.Forms.TextBox();
            this.lblStaticUrlPre = new System.Windows.Forms.Label();
            this.lblStaticUrlAft = new System.Windows.Forms.Label();
            this.txtUrlAft = new System.Windows.Forms.TextBox();
            this.lblStaticNotamPre = new System.Windows.Forms.Label();
            this.txtNotamPre = new System.Windows.Forms.TextBox();
            this.lblStaticNotamAft = new System.Windows.Forms.Label();
            this.txtNotamAft = new System.Windows.Forms.TextBox();
            this.lblStaticWarning = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblStaticMapyAPI = new System.Windows.Forms.Label();
            this.txtMapyApiKey = new System.Windows.Forms.TextBox();
            this.lblStaticFaaClientId = new System.Windows.Forms.Label();
            this.txtFaaApiClientId = new System.Windows.Forms.TextBox();
            this.lblStaticFaaApiSecret = new System.Windows.Forms.Label();
            this.txtFaaApiClientSecret = new System.Windows.Forms.TextBox();
            this.grpNotamSource = new System.Windows.Forms.GroupBox();
            this.radSrcDefault = new System.Windows.Forms.RadioButton();
            this.radSrcApi = new System.Windows.Forms.RadioButton();
            this.grpNotamSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(336, 294);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Sačuvaj";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtUrlPre
            // 
            this.txtUrlPre.Location = new System.Drawing.Point(123, 45);
            this.txtUrlPre.Name = "txtUrlPre";
            this.txtUrlPre.Size = new System.Drawing.Size(288, 20);
            this.txtUrlPre.TabIndex = 1;
            // 
            // lblStaticUrlPre
            // 
            this.lblStaticUrlPre.AutoSize = true;
            this.lblStaticUrlPre.Location = new System.Drawing.Point(37, 48);
            this.lblStaticUrlPre.Name = "lblStaticUrlPre";
            this.lblStaticUrlPre.Size = new System.Drawing.Size(80, 13);
            this.lblStaticUrlPre.TabIndex = 2;
            this.lblStaticUrlPre.Text = "FAA Javni URL";
            // 
            // lblStaticUrlAft
            // 
            this.lblStaticUrlAft.AutoSize = true;
            this.lblStaticUrlAft.Location = new System.Drawing.Point(45, 74);
            this.lblStaticUrlAft.Name = "lblStaticUrlAft";
            this.lblStaticUrlAft.Size = new System.Drawing.Size(72, 13);
            this.lblStaticUrlAft.TabIndex = 4;
            this.lblStaticUrlAft.Text = "FAA API URL";
            // 
            // txtUrlAft
            // 
            this.txtUrlAft.Location = new System.Drawing.Point(123, 71);
            this.txtUrlAft.Name = "txtUrlAft";
            this.txtUrlAft.Size = new System.Drawing.Size(288, 20);
            this.txtUrlAft.TabIndex = 3;
            // 
            // lblStaticNotamPre
            // 
            this.lblStaticNotamPre.AutoSize = true;
            this.lblStaticNotamPre.Location = new System.Drawing.Point(21, 100);
            this.lblStaticNotamPre.Name = "lblStaticNotamPre";
            this.lblStaticNotamPre.Size = new System.Drawing.Size(96, 13);
            this.lblStaticNotamPre.TabIndex = 6;
            this.lblStaticNotamPre.Text = "Deo pre NOTAM-a";
            // 
            // txtNotamPre
            // 
            this.txtNotamPre.Location = new System.Drawing.Point(123, 97);
            this.txtNotamPre.Name = "txtNotamPre";
            this.txtNotamPre.Size = new System.Drawing.Size(288, 20);
            this.txtNotamPre.TabIndex = 5;
            // 
            // lblStaticNotamAft
            // 
            this.lblStaticNotamAft.AutoSize = true;
            this.lblStaticNotamAft.Location = new System.Drawing.Point(11, 126);
            this.lblStaticNotamAft.Name = "lblStaticNotamAft";
            this.lblStaticNotamAft.Size = new System.Drawing.Size(106, 13);
            this.lblStaticNotamAft.TabIndex = 8;
            this.lblStaticNotamAft.Text = "Deo posle NOTAM-a";
            // 
            // txtNotamAft
            // 
            this.txtNotamAft.Location = new System.Drawing.Point(123, 123);
            this.txtNotamAft.Name = "txtNotamAft";
            this.txtNotamAft.Size = new System.Drawing.Size(288, 20);
            this.txtNotamAft.TabIndex = 7;
            // 
            // lblStaticWarning
            // 
            this.lblStaticWarning.AutoSize = true;
            this.lblStaticWarning.ForeColor = System.Drawing.Color.IndianRed;
            this.lblStaticWarning.Location = new System.Drawing.Point(67, 9);
            this.lblStaticWarning.Name = "lblStaticWarning";
            this.lblStaticWarning.Size = new System.Drawing.Size(283, 26);
            this.lblStaticWarning.TabIndex = 9;
            this.lblStaticWarning.Text = "Baš ne bih preporucio da se dira ovo ako ti Mihail ne kaže.\r\nOva boja se zove ind" +
    "ian red inače :)";
            this.lblStaticWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(123, 294);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Resetuj";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblStaticMapyAPI
            // 
            this.lblStaticMapyAPI.AutoSize = true;
            this.lblStaticMapyAPI.Location = new System.Drawing.Point(44, 152);
            this.lblStaticMapyAPI.Name = "lblStaticMapyAPI";
            this.lblStaticMapyAPI.Size = new System.Drawing.Size(73, 13);
            this.lblStaticMapyAPI.TabIndex = 12;
            this.lblStaticMapyAPI.Text = "Mapy API key";
            // 
            // txtMapyApiKey
            // 
            this.txtMapyApiKey.Location = new System.Drawing.Point(123, 149);
            this.txtMapyApiKey.Name = "txtMapyApiKey";
            this.txtMapyApiKey.Size = new System.Drawing.Size(288, 20);
            this.txtMapyApiKey.TabIndex = 11;
            // 
            // lblStaticFaaClientId
            // 
            this.lblStaticFaaClientId.AutoSize = true;
            this.lblStaticFaaClientId.Location = new System.Drawing.Point(27, 178);
            this.lblStaticFaaClientId.Name = "lblStaticFaaClientId";
            this.lblStaticFaaClientId.Size = new System.Drawing.Size(90, 13);
            this.lblStaticFaaClientId.TabIndex = 14;
            this.lblStaticFaaClientId.Text = "FAA API Client ID";
            // 
            // txtFaaApiClientId
            // 
            this.txtFaaApiClientId.Location = new System.Drawing.Point(123, 175);
            this.txtFaaApiClientId.Name = "txtFaaApiClientId";
            this.txtFaaApiClientId.Size = new System.Drawing.Size(288, 20);
            this.txtFaaApiClientId.TabIndex = 13;
            // 
            // lblStaticFaaApiSecret
            // 
            this.lblStaticFaaApiSecret.AutoSize = true;
            this.lblStaticFaaApiSecret.Location = new System.Drawing.Point(7, 204);
            this.lblStaticFaaApiSecret.Name = "lblStaticFaaApiSecret";
            this.lblStaticFaaApiSecret.Size = new System.Drawing.Size(110, 13);
            this.lblStaticFaaApiSecret.TabIndex = 16;
            this.lblStaticFaaApiSecret.Text = "FAA API Client Secret";
            // 
            // txtFaaApiClientSecret
            // 
            this.txtFaaApiClientSecret.Location = new System.Drawing.Point(123, 201);
            this.txtFaaApiClientSecret.Name = "txtFaaApiClientSecret";
            this.txtFaaApiClientSecret.Size = new System.Drawing.Size(288, 20);
            this.txtFaaApiClientSecret.TabIndex = 15;
            // 
            // grpNotamSource
            // 
            this.grpNotamSource.Controls.Add(this.radSrcApi);
            this.grpNotamSource.Controls.Add(this.radSrcDefault);
            this.grpNotamSource.Location = new System.Drawing.Point(123, 236);
            this.grpNotamSource.Name = "grpNotamSource";
            this.grpNotamSource.Size = new System.Drawing.Size(288, 49);
            this.grpNotamSource.TabIndex = 17;
            this.grpNotamSource.TabStop = false;
            this.grpNotamSource.Text = "Izvor NOTAMa";
            // 
            // radSrcDefault
            // 
            this.radSrcDefault.AutoSize = true;
            this.radSrcDefault.Location = new System.Drawing.Point(6, 19);
            this.radSrcDefault.Name = "radSrcDefault";
            this.radSrcDefault.Size = new System.Drawing.Size(73, 17);
            this.radSrcDefault.TabIndex = 0;
            this.radSrcDefault.TabStop = true;
            this.radSrcDefault.Text = "FAA Javni";
            this.radSrcDefault.UseVisualStyleBackColor = true;
            // 
            // radSrcApi
            // 
            this.radSrcApi.AutoSize = true;
            this.radSrcApi.Location = new System.Drawing.Point(85, 19);
            this.radSrcApi.Name = "radSrcApi";
            this.radSrcApi.Size = new System.Drawing.Size(192, 17);
            this.radSrcApi.TabIndex = 1;
            this.radSrcApi.TabStop = true;
            this.radSrcApi.Text = "FAA API (Potrebni ClientID i Secret)";
            this.radSrcApi.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 325);
            this.Controls.Add(this.grpNotamSource);
            this.Controls.Add(this.lblStaticFaaApiSecret);
            this.Controls.Add(this.txtFaaApiClientSecret);
            this.Controls.Add(this.lblStaticFaaClientId);
            this.Controls.Add(this.txtFaaApiClientId);
            this.Controls.Add(this.lblStaticMapyAPI);
            this.Controls.Add(this.txtMapyApiKey);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblStaticWarning);
            this.Controls.Add(this.lblStaticNotamAft);
            this.Controls.Add(this.txtNotamAft);
            this.Controls.Add(this.lblStaticNotamPre);
            this.Controls.Add(this.txtNotamPre);
            this.Controls.Add(this.lblStaticUrlAft);
            this.Controls.Add(this.txtUrlAft);
            this.Controls.Add(this.lblStaticUrlPre);
            this.Controls.Add(this.txtUrlPre);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Napredna podešavanja";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.grpNotamSource.ResumeLayout(false);
            this.grpNotamSource.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtUrlPre;
        private System.Windows.Forms.Label lblStaticUrlPre;
        private System.Windows.Forms.Label lblStaticUrlAft;
        private System.Windows.Forms.TextBox txtUrlAft;
        private System.Windows.Forms.Label lblStaticNotamPre;
        private System.Windows.Forms.TextBox txtNotamPre;
        private System.Windows.Forms.Label lblStaticNotamAft;
        private System.Windows.Forms.TextBox txtNotamAft;
        private System.Windows.Forms.Label lblStaticWarning;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblStaticMapyAPI;
        private System.Windows.Forms.TextBox txtMapyApiKey;
        private System.Windows.Forms.Label lblStaticFaaClientId;
        private System.Windows.Forms.TextBox txtFaaApiClientId;
        private System.Windows.Forms.Label lblStaticFaaApiSecret;
        private System.Windows.Forms.TextBox txtFaaApiClientSecret;
        private System.Windows.Forms.GroupBox grpNotamSource;
        private System.Windows.Forms.RadioButton radSrcApi;
        private System.Windows.Forms.RadioButton radSrcDefault;
    }
}