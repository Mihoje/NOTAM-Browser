using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOTAM_Browser
{
    public partial class frmSplash : Form
    {
        private readonly int spinnerFps = 60;
        private readonly int timerInterval;
        private readonly System.Windows.Forms.Timer timer;
        private readonly RectangleF rectLoader;
        private readonly RectangleF rectProgressBar;
        private readonly RectangleF rectImage;
        private readonly Bitmap splashImage;

        public frmMain MainForm { get; set; }
        
        private int frameCount = 0;

        public frmSplash()
        {
            InitializeComponent();

            rectLoader = new RectangleF(this.Width * 0.25F, this.Height * 0.6F, this.Width * 0.5F, this.Height * 0.15F);
            rectProgressBar = new RectangleF(this.Width * 0.1F, this.Height * 0.8F, this.Width * 0.8F, this.Height * 0.1F);

            float imageSize = Math.Min(this.Width, this.Height * 0.6F);

            rectImage = new RectangleF((this.Width - imageSize) / 2, (this.Height * 0.6F - imageSize) / 2, imageSize, imageSize);

            timer = new System.Windows.Forms.Timer();

            DoubleBuffered = true;

            timerInterval = 1000 / spinnerFps;

            timer.Tick += Timer_Tick;
            timer.Interval = timerInterval;

            this.TransparencyKey = Color.DimGray;
            
            this.BackColor = Color.DimGray;

            splashImage = new Bitmap(Properties.Resources.splash_icon);
        }

        public frmSplash(frmMain frmMain) : this()
        {
            this.MainForm = frmMain;
        }

        private void FinishSplash()
        {
            timer.Enabled = false;
            this.Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            frameCount++;// = (frameCount + 1) % (spinnerFps * 2);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

            e.Graphics.DrawImage(splashImage, rectImage);

            RectangleF box = rectLoader;   // your render area

            float cx = box.X + box.Width * 0.5f;
            float cy = box.Y + box.Height * 0.5f;

            float maxR = Math.Min(box.Width, box.Height) * 0.29f * 0.5f;

            float Cycle(float phase, float shift)
            {
                float p = (phase + shift) % 1f;
                return p <= 0.5f ? p * 2f : (1f - p) * 2f;
            }

            float basePhase = (frameCount % spinnerFps) / (float)spinnerFps;
            float[] shifts = { 0f, 0.15f, 0.30f };

            for (int i = 0; i < 3; i++)
            {
                float t = Cycle(basePhase, shifts[i]);
                float r = maxR * t;
                float d = r * 2f;

                float x = cx + (i - 1) * (maxR * 2.2f);
                float y = cy;

                e.Graphics.FillEllipse(
                    Brushes.LightBlue,
                    x - r,
                    y - r,
                    d,
                    d
                );
            }

            if(MainForm == null) return;

            float toLoad = MainForm.AckownledgedNotamsCount + MainForm.FirstTimeNotamCount;
            float loaded = MainForm.FirstTimeLoadedNotams;

            if (MainForm.FrmAckNotams != null)
            {
                loaded += MainForm.FrmAckNotams.LoadedNotamsCount;
            }

            float progress = loaded / toLoad;

            float progressWidth = rectProgressBar.Width * progress;

            if (MainForm.FirstTimeFinishedLoading)
            {
                progressWidth = rectProgressBar.Width;
                FinishSplash();
            }

            e.Graphics.FillRectangle(
                Brushes.Green,
                rectProgressBar.X,
                rectProgressBar.Y,
                progressWidth,
                rectProgressBar.Height
            );
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void frmSplash_Shown(object sender, EventArgs e)
        {

        }
    }
}
