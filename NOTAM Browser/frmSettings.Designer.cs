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
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(336, 151);
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
            this.lblStaticUrlPre.Text = "Prvi deo URL-a";
            // 
            // lblStaticUrlAft
            // 
            this.lblStaticUrlAft.AutoSize = true;
            this.lblStaticUrlAft.Location = new System.Drawing.Point(30, 74);
            this.lblStaticUrlAft.Name = "lblStaticUrlAft";
            this.lblStaticUrlAft.Size = new System.Drawing.Size(87, 13);
            this.lblStaticUrlAft.TabIndex = 4;
            this.lblStaticUrlAft.Text = "Drugi deo URL-a";
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
            this.btnReset.Location = new System.Drawing.Point(123, 151);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Resetuj";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 182);
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
    }
}