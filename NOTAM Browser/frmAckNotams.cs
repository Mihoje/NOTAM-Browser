using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{
    /*
     * 
     * TODO: Dosta whitespace ima ispod svih notama. Mora se to popraviti
     * 
     * */
    public partial class frmAckNotams : Form
    {
        private readonly Notams nos;
        public frmAckNotams(Notams nos)
        {
            InitializeComponent();

            this.nos = nos;

            showNotams(this.nos.AcknowledgedNotams);

            this.nos.NotamAcknowledged += NewNotamAcknowledged;
            this.nos.NotamUnacknowledged += NewNotamUnacknowledged;
        }

        private void NewNotamUnacknowledged(string NotamID)
        {
            var results = tlpMain.Controls.Find($"txtNotam{NotamID}", false);

            if (results.Length == 0)
            {
#if DEBUG
                Debug.WriteLine($"frmAckNotams: Didn't find TextBox for the removed NOTAM! [{NotamID}]");
#endif
                return;
            }

            RichTextBox txt = (RichTextBox)results[0];

            tlpMain.Controls.Remove(txt);

            if (tlpMain.RowStyles.Count > 0)
                tlpMain.RowStyles.RemoveAt(0);

            tlpMain.RowCount--;
#if DEBUG
            Debug.WriteLine($"frmAckNotams: Removed notam {NotamID}. Rows { tlpMain.RowCount }. Row styles: {tlpMain.RowStyles.Count}");
#endif
        }

        private void NewNotamAcknowledged(string NotamID)
        {
            addNotamRow(NotamID, nos.AcknowledgedNotams[NotamID]);
#if DEBUG
            Debug.WriteLine($"frmAckNotams: Added notam { NotamID }. Rows: { tlpMain.RowCount }. Row styles: {tlpMain.RowStyles.Count}");
#endif
        }

        private void addNotamRow(string NotamID, string NotamText)
        {
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowCount++;

            var txt = new ScrollTransparentTextBox
            {
                Name = $"txtNotam{NotamID}",
                Text = NormalizeNewLines(NotamText),
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Horizontal,//ScrollBars.Horizontal,
                Multiline = true,
                ReadOnly = true,
                Font = Properties.Settings.Default.notamFont ?? new Font("Consolas", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            tlpMain.Controls.Add(txt, 0, tlpMain.RowCount - 1);

            SizeF textSize = txt.CreateGraphics().MeasureString(txt.Text + Environment.NewLine + "a" + Environment.NewLine + "a", txt.Font, 10000, new StringFormat(0));
            txt.Height = (int)textSize.Height + 2;
        }

        private void showNotams(Dictionary<string, string> Notams)
        {
            tlpMain.RowStyles.Clear();
            tlpMain.Controls.Clear();
            tlpMain.RowCount = 0;

            foreach (var notam in Notams)
            {
                addNotamRow(notam.Key, notam.Value);
            }
        }

        private string NormalizeNewLines(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        private void frmAckNotams_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void RefreshNotams()
        {
            showNotams(nos.AcknowledgedNotams);
        }
    }
}
