using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using System.Windows.Forms;
#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{
    public partial class frmPrint : Form
    {
        PrintManager printManager;
        internal frmPrint(PrintManager printManager)
        {
            InitializeComponent();
            this.printManager = printManager;
        }

        public void SetLabelText(string text)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate {
                    SetLabelText(text);
                }));
                return;
            }
            lblMain.Text = text;
        }

        public void HideSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate {
                    HideSafe();
                }));
                return;
            }
            this.Hide();
        }

        public void SetControlSize(Control control, Size size)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate {
                    SetControlSize(control, size);
                }));
                return;
            }
            control.Size = size;
        }

        public Bitmap GetImageFromMap(GMapControl map)
        {
            try
            {
                Bitmap bmp = new Bitmap(map.Width, map.Height);

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate {
                        try
                        {
                            map.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Debug.WriteLine($"frmPrint: Error GetImageFromMap Invoke. {ex}");
#endif
                            throw ex;
                        }
                    } ));
                }
                else
                {
                    map.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                }

                return bmp;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"frmPrint: Error GetImageFromMap. {ex}");
#endif
                throw ex;
            }
            //return null;
        }

        private void btnExtend_Click(object sender, EventArgs e)
        {
            printManager.PrintTimeoutSeconds += 10;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            printManager.PrintTimeoutSeconds = -1;
        }
    }
}
