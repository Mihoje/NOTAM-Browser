namespace NOTAM_Browser
{
    partial class frmEditPolys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditPolys));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabAddPoly = new System.Windows.Forms.TabPage();
            this.grpCircle = new System.Windows.Forms.GroupBox();
            this.lblStaticCircleRadius = new System.Windows.Forms.Label();
            this.nudCircleRadius = new System.Windows.Forms.NumericUpDown();
            this.btnAddCircle = new System.Windows.Forms.Button();
            this.lblStaticCircleColor = new System.Windows.Forms.Label();
            this.pnlCircleColor = new System.Windows.Forms.Panel();
            this.lblStaticCircleCoordinateConverted = new System.Windows.Forms.Label();
            this.lblStaticCircleCoordinates = new System.Windows.Forms.Label();
            this.lblStaticCircleName = new System.Windows.Forms.Label();
            this.txtCircleName = new System.Windows.Forms.TextBox();
            this.txtCircleCoordinateConverted = new System.Windows.Forms.TextBox();
            this.txtCircleCoordinate = new System.Windows.Forms.TextBox();
            this.grpPolygon = new System.Windows.Forms.GroupBox();
            this.lblStaticPolyCoordinates = new System.Windows.Forms.Label();
            this.lblStaticPolygonColor = new System.Windows.Forms.Label();
            this.txtPolyCoordinates = new System.Windows.Forms.TextBox();
            this.pnlPolyColor = new System.Windows.Forms.Panel();
            this.txtPolyCoordinatesConverted = new System.Windows.Forms.TextBox();
            this.lblStaticPolyRecognizedCoords = new System.Windows.Forms.Label();
            this.btnAddPoly = new System.Windows.Forms.Button();
            this.lblStaticPolygonName = new System.Windows.Forms.Label();
            this.txtPolygonName = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControlMain.SuspendLayout();
            this.tabAddPoly.SuspendLayout();
            this.grpCircle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCircleRadius)).BeginInit();
            this.grpPolygon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabAddPoly);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(796, 605);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabAddPoly
            // 
            this.tabAddPoly.Controls.Add(this.pictureBox1);
            this.tabAddPoly.Controls.Add(this.grpCircle);
            this.tabAddPoly.Controls.Add(this.grpPolygon);
            this.tabAddPoly.Location = new System.Drawing.Point(4, 22);
            this.tabAddPoly.Name = "tabAddPoly";
            this.tabAddPoly.Padding = new System.Windows.Forms.Padding(3);
            this.tabAddPoly.Size = new System.Drawing.Size(788, 579);
            this.tabAddPoly.TabIndex = 1;
            this.tabAddPoly.Text = "Dodaj zonu";
            this.tabAddPoly.UseVisualStyleBackColor = true;
            // 
            // grpCircle
            // 
            this.grpCircle.Controls.Add(this.lblStaticCircleRadius);
            this.grpCircle.Controls.Add(this.nudCircleRadius);
            this.grpCircle.Controls.Add(this.btnAddCircle);
            this.grpCircle.Controls.Add(this.lblStaticCircleColor);
            this.grpCircle.Controls.Add(this.pnlCircleColor);
            this.grpCircle.Controls.Add(this.lblStaticCircleCoordinateConverted);
            this.grpCircle.Controls.Add(this.lblStaticCircleCoordinates);
            this.grpCircle.Controls.Add(this.lblStaticCircleName);
            this.grpCircle.Controls.Add(this.txtCircleName);
            this.grpCircle.Controls.Add(this.txtCircleCoordinateConverted);
            this.grpCircle.Controls.Add(this.txtCircleCoordinate);
            this.grpCircle.Location = new System.Drawing.Point(458, 6);
            this.grpCircle.Name = "grpCircle";
            this.grpCircle.Size = new System.Drawing.Size(318, 219);
            this.grpCircle.TabIndex = 10;
            this.grpCircle.TabStop = false;
            this.grpCircle.Text = "Kružna zona";
            // 
            // lblStaticCircleRadius
            // 
            this.lblStaticCircleRadius.AutoSize = true;
            this.lblStaticCircleRadius.Location = new System.Drawing.Point(9, 106);
            this.lblStaticCircleRadius.Name = "lblStaticCircleRadius";
            this.lblStaticCircleRadius.Size = new System.Drawing.Size(125, 13);
            this.lblStaticCircleRadius.TabIndex = 15;
            this.lblStaticCircleRadius.Text = "Poluprečnik kruga u NM:";
            // 
            // nudCircleRadius
            // 
            this.nudCircleRadius.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudCircleRadius.Location = new System.Drawing.Point(140, 104);
            this.nudCircleRadius.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudCircleRadius.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.nudCircleRadius.Name = "nudCircleRadius";
            this.nudCircleRadius.Size = new System.Drawing.Size(172, 20);
            this.nudCircleRadius.TabIndex = 14;
            this.nudCircleRadius.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // btnAddCircle
            // 
            this.btnAddCircle.Location = new System.Drawing.Point(6, 185);
            this.btnAddCircle.Name = "btnAddCircle";
            this.btnAddCircle.Size = new System.Drawing.Size(306, 23);
            this.btnAddCircle.TabIndex = 13;
            this.btnAddCircle.Text = "Dodaj krug";
            this.btnAddCircle.UseVisualStyleBackColor = true;
            this.btnAddCircle.Click += new System.EventHandler(this.btnAddCircle_Click);
            // 
            // lblStaticCircleColor
            // 
            this.lblStaticCircleColor.AutoSize = true;
            this.lblStaticCircleColor.Location = new System.Drawing.Point(274, 132);
            this.lblStaticCircleColor.Name = "lblStaticCircleColor";
            this.lblStaticCircleColor.Size = new System.Drawing.Size(31, 13);
            this.lblStaticCircleColor.TabIndex = 12;
            this.lblStaticCircleColor.Text = "Boja:";
            // 
            // pnlCircleColor
            // 
            this.pnlCircleColor.BackColor = System.Drawing.Color.Red;
            this.pnlCircleColor.Location = new System.Drawing.Point(277, 148);
            this.pnlCircleColor.Name = "pnlCircleColor";
            this.pnlCircleColor.Size = new System.Drawing.Size(35, 20);
            this.pnlCircleColor.TabIndex = 11;
            this.pnlCircleColor.Click += new System.EventHandler(this.pnlColor_Click);
            // 
            // lblStaticCircleCoordinateConverted
            // 
            this.lblStaticCircleCoordinateConverted.AutoSize = true;
            this.lblStaticCircleCoordinateConverted.Location = new System.Drawing.Point(6, 62);
            this.lblStaticCircleCoordinateConverted.Name = "lblStaticCircleCoordinateConverted";
            this.lblStaticCircleCoordinateConverted.Size = new System.Drawing.Size(117, 13);
            this.lblStaticCircleCoordinateConverted.TabIndex = 10;
            this.lblStaticCircleCoordinateConverted.Text = "Prepoznata koordinata:";
            // 
            // lblStaticCircleCoordinates
            // 
            this.lblStaticCircleCoordinates.AutoSize = true;
            this.lblStaticCircleCoordinates.Location = new System.Drawing.Point(6, 23);
            this.lblStaticCircleCoordinates.Name = "lblStaticCircleCoordinates";
            this.lblStaticCircleCoordinates.Size = new System.Drawing.Size(94, 13);
            this.lblStaticCircleCoordinates.TabIndex = 9;
            this.lblStaticCircleCoordinates.Text = "Koodrinate centra:";
            // 
            // lblStaticCircleName
            // 
            this.lblStaticCircleName.AutoSize = true;
            this.lblStaticCircleName.Location = new System.Drawing.Point(6, 132);
            this.lblStaticCircleName.Name = "lblStaticCircleName";
            this.lblStaticCircleName.Size = new System.Drawing.Size(63, 13);
            this.lblStaticCircleName.TabIndex = 8;
            this.lblStaticCircleName.Text = "Naziv zone:";
            // 
            // txtCircleName
            // 
            this.txtCircleName.Location = new System.Drawing.Point(6, 148);
            this.txtCircleName.Name = "txtCircleName";
            this.txtCircleName.Size = new System.Drawing.Size(265, 20);
            this.txtCircleName.TabIndex = 7;
            // 
            // txtCircleCoordinateConverted
            // 
            this.txtCircleCoordinateConverted.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCircleCoordinateConverted.Location = new System.Drawing.Point(6, 78);
            this.txtCircleCoordinateConverted.Name = "txtCircleCoordinateConverted";
            this.txtCircleCoordinateConverted.ReadOnly = true;
            this.txtCircleCoordinateConverted.Size = new System.Drawing.Size(306, 20);
            this.txtCircleCoordinateConverted.TabIndex = 6;
            // 
            // txtCircleCoordinate
            // 
            this.txtCircleCoordinate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCircleCoordinate.Location = new System.Drawing.Point(6, 39);
            this.txtCircleCoordinate.Name = "txtCircleCoordinate";
            this.txtCircleCoordinate.Size = new System.Drawing.Size(306, 20);
            this.txtCircleCoordinate.TabIndex = 5;
            this.txtCircleCoordinate.TextChanged += new System.EventHandler(this.txtCircleCoordinate_TextChanged);
            // 
            // grpPolygon
            // 
            this.grpPolygon.Controls.Add(this.lblStaticPolyCoordinates);
            this.grpPolygon.Controls.Add(this.lblStaticPolygonColor);
            this.grpPolygon.Controls.Add(this.txtPolyCoordinates);
            this.grpPolygon.Controls.Add(this.pnlPolyColor);
            this.grpPolygon.Controls.Add(this.txtPolyCoordinatesConverted);
            this.grpPolygon.Controls.Add(this.lblStaticPolyRecognizedCoords);
            this.grpPolygon.Controls.Add(this.btnAddPoly);
            this.grpPolygon.Controls.Add(this.lblStaticPolygonName);
            this.grpPolygon.Controls.Add(this.txtPolygonName);
            this.grpPolygon.Location = new System.Drawing.Point(8, 6);
            this.grpPolygon.Name = "grpPolygon";
            this.grpPolygon.Size = new System.Drawing.Size(444, 558);
            this.grpPolygon.TabIndex = 9;
            this.grpPolygon.TabStop = false;
            this.grpPolygon.Text = "Poligon zona";
            // 
            // lblStaticPolyCoordinates
            // 
            this.lblStaticPolyCoordinates.AutoSize = true;
            this.lblStaticPolyCoordinates.Location = new System.Drawing.Point(16, 23);
            this.lblStaticPolyCoordinates.Name = "lblStaticPolyCoordinates";
            this.lblStaticPolyCoordinates.Size = new System.Drawing.Size(104, 13);
            this.lblStaticPolyCoordinates.TabIndex = 1;
            this.lblStaticPolyCoordinates.Text = "Koordinate poligona:";
            // 
            // lblStaticPolygonColor
            // 
            this.lblStaticPolygonColor.AutoSize = true;
            this.lblStaticPolygonColor.Location = new System.Drawing.Point(224, 509);
            this.lblStaticPolygonColor.Name = "lblStaticPolygonColor";
            this.lblStaticPolygonColor.Size = new System.Drawing.Size(31, 13);
            this.lblStaticPolygonColor.TabIndex = 8;
            this.lblStaticPolygonColor.Text = "Boja:";
            // 
            // txtPolyCoordinates
            // 
            this.txtPolyCoordinates.Location = new System.Drawing.Point(16, 39);
            this.txtPolyCoordinates.Multiline = true;
            this.txtPolyCoordinates.Name = "txtPolyCoordinates";
            this.txtPolyCoordinates.Size = new System.Drawing.Size(202, 461);
            this.txtPolyCoordinates.TabIndex = 0;
            this.txtPolyCoordinates.TextChanged += new System.EventHandler(this.txtPolyCoordinates_TextChanged);
            // 
            // pnlPolyColor
            // 
            this.pnlPolyColor.BackColor = System.Drawing.Color.Red;
            this.pnlPolyColor.Location = new System.Drawing.Point(227, 525);
            this.pnlPolyColor.Name = "pnlPolyColor";
            this.pnlPolyColor.Size = new System.Drawing.Size(35, 20);
            this.pnlPolyColor.TabIndex = 7;
            this.pnlPolyColor.Click += new System.EventHandler(this.pnlColor_Click);
            // 
            // txtPolyCoordinatesConverted
            // 
            this.txtPolyCoordinatesConverted.Location = new System.Drawing.Point(224, 39);
            this.txtPolyCoordinatesConverted.Multiline = true;
            this.txtPolyCoordinatesConverted.Name = "txtPolyCoordinatesConverted";
            this.txtPolyCoordinatesConverted.ReadOnly = true;
            this.txtPolyCoordinatesConverted.Size = new System.Drawing.Size(202, 461);
            this.txtPolyCoordinatesConverted.TabIndex = 2;
            // 
            // lblStaticPolyRecognizedCoords
            // 
            this.lblStaticPolyRecognizedCoords.AutoSize = true;
            this.lblStaticPolyRecognizedCoords.Location = new System.Drawing.Point(221, 23);
            this.lblStaticPolyRecognizedCoords.Name = "lblStaticPolyRecognizedCoords";
            this.lblStaticPolyRecognizedCoords.Size = new System.Drawing.Size(117, 13);
            this.lblStaticPolyRecognizedCoords.TabIndex = 6;
            this.lblStaticPolyRecognizedCoords.Text = "Prepoznate koordinate:";
            // 
            // btnAddPoly
            // 
            this.btnAddPoly.Location = new System.Drawing.Point(268, 525);
            this.btnAddPoly.Name = "btnAddPoly";
            this.btnAddPoly.Size = new System.Drawing.Size(158, 23);
            this.btnAddPoly.TabIndex = 3;
            this.btnAddPoly.Text = "Dodaj poligon";
            this.btnAddPoly.UseVisualStyleBackColor = true;
            this.btnAddPoly.Click += new System.EventHandler(this.btnAddPoly_Click);
            // 
            // lblStaticPolygonName
            // 
            this.lblStaticPolygonName.AutoSize = true;
            this.lblStaticPolygonName.Location = new System.Drawing.Point(16, 509);
            this.lblStaticPolygonName.Name = "lblStaticPolygonName";
            this.lblStaticPolygonName.Size = new System.Drawing.Size(63, 13);
            this.lblStaticPolygonName.TabIndex = 5;
            this.lblStaticPolygonName.Text = "Naziv zone:";
            // 
            // txtPolygonName
            // 
            this.txtPolygonName.Location = new System.Drawing.Point(16, 525);
            this.txtPolygonName.Name = "txtPolygonName";
            this.txtPolygonName.Size = new System.Drawing.Size(202, 20);
            this.txtPolygonName.TabIndex = 4;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 20;
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(489, 314);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(251, 203);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // frmEditPolys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 605);
            this.Controls.Add(this.tabControlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditPolys";
            this.Text = "Upravljaj zonama";
            this.tabControlMain.ResumeLayout(false);
            this.tabAddPoly.ResumeLayout(false);
            this.grpCircle.ResumeLayout(false);
            this.grpCircle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCircleRadius)).EndInit();
            this.grpPolygon.ResumeLayout(false);
            this.grpPolygon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabAddPoly;
        private System.Windows.Forms.Label lblStaticPolyCoordinates;
        private System.Windows.Forms.TextBox txtPolyCoordinates;
        private System.Windows.Forms.TextBox txtPolyCoordinatesConverted;
        private System.Windows.Forms.Button btnAddPoly;
        private System.Windows.Forms.Label lblStaticPolygonName;
        private System.Windows.Forms.TextBox txtPolygonName;
        private System.Windows.Forms.Label lblStaticPolyRecognizedCoords;
        private System.Windows.Forms.Label lblStaticPolygonColor;
        private System.Windows.Forms.Panel pnlPolyColor;
        private System.Windows.Forms.GroupBox grpCircle;
        private System.Windows.Forms.Label lblStaticCircleCoordinateConverted;
        private System.Windows.Forms.Label lblStaticCircleCoordinates;
        private System.Windows.Forms.Label lblStaticCircleName;
        private System.Windows.Forms.TextBox txtCircleName;
        private System.Windows.Forms.TextBox txtCircleCoordinateConverted;
        private System.Windows.Forms.TextBox txtCircleCoordinate;
        private System.Windows.Forms.GroupBox grpPolygon;
        private System.Windows.Forms.Label lblStaticCircleColor;
        private System.Windows.Forms.Panel pnlCircleColor;
        private System.Windows.Forms.Button btnAddCircle;
        private System.Windows.Forms.Label lblStaticCircleRadius;
        private System.Windows.Forms.NumericUpDown nudCircleRadius;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}