using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NOTAM_Browser
{
    class ScrollTransparentTextBox : RichTextBox
    {
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        protected override void WndProc(ref Message m)
        {
            const int WM_MOUSEWHEEL = 0x020A;
            const int WM_MOUSEHWHEEL = 0x020E;

            switch (m.Msg)
            {
                case WM_MOUSEWHEEL:
                case WM_MOUSEHWHEEL:
                    Control p = this;
                    do
                    {
                        p = p.Parent;
                    } while (p != null && !(p is ScrollableControl));

                    if (p != null)
                    {
                        SendMessage(p.Handle, m.Msg, m.WParam, m.LParam);
                        return; // Don't call base.WndProc – we handled it
                    }
                    break;
            }

            base.WndProc(ref m);
        }
    }
}
