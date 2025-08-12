using NOTAM_Browser.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{
    public partial class frmMain : Form
    {
        private readonly Notams nos;
        private Font notamFont;
        private readonly frmAckNotams frmAckNotams;
        private readonly frmMap mapForm;
        private string notamIdClicked;

        public frmMain()
        {
            InitializeComponent();

            notamFont = Settings.Default.notamFont;

            if (notamFont == null)
            {
                Settings.Default.notamFont = (Font)Settings.Default.Properties["notamFont"].DefaultValue;
                Settings.Default.Save();

                notamFont = Settings.Default.notamFont;
            }

            nos = new Notams();
            frmAckNotams = new frmAckNotams(nos);
            mapForm = new frmMap(nos);

            notamIdClicked = string.Empty;

            nos.NotamAcknowledged += NewNotamAcknowledged;
            nos.NotamUnacknowledged += NewNotamUnacknowledged;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            showNotams(nos.CurrentNotams);
            txtSearch.Text = nos.LastSearch;
            txtSearch.SelectionStart = txtSearch.TextLength;
            slblLatestNotam.Text = $"Poslednje ažuriranje: {nos.CurrentNotamsTime}";

            tooltipDesignators.SetToolTip(txtSearch, "Unesi ICAO designator ili više njih razdvojenih razmakom.\n\nPrimer:'LYBT' ili 'LYBT,LYBA'");
        }

        private async void pretraziNotame()
        {
            if (txtSearch.Text.Trim() == "") return;

            slblLatestNotam.Text = "Povlačim NOTAM-e...";

            await nos.GetFromInternet(txtSearch.Text);

            if (chkFilterDatum.Checked)
                showNotams(nos.GetNotamsForDate(dtpFilterDatum.Value));
            else
                showNotams(nos.CurrentNotams);

            slblLatestNotam.Text = $"Poslednje ažuriranje: {nos.CurrentNotamsTime}";
        }

        private string NormalizeNewLines(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        private void SetTextBoxHeights()
        {
            foreach (var txt in tlpMain.Controls.OfType<RichTextBox>())
            {
                txt.Font = notamFont;

                SizeF textSize = txt.CreateGraphics().MeasureString(txt.Text + Environment.NewLine + "a" + Environment.NewLine + "a", txt.Font, 10000, new StringFormat(0));

                txt.Height = (int)textSize.Height + 1; //nekad se desava da zbog nekog razloga malo zakine pa dodajem 2 px for good measure
            }
        }

        private void showNotams(Dictionary<string, string> Notams)
        {
            tlpMain.Controls.Clear();
            tlpMain.RowStyles.Clear();
            tlpMain.RowCount = 0;

            foreach (var pair in Notams)
            {
                if (chkFilterAck.Checked && nos.AcknowledgedNotams.ContainsKey(pair.Key)) continue;

                string notamText = NormalizeNewLines(pair.Value);

                var coordinates = NotamParser.ParseCoordinates(notamText);

                CheckBox chk = new CheckBox()
                {
                    Name = $"chkNotam{pair.Key}",
                    Checked = nos.AcknowledgedNotams.ContainsKey(pair.Key)
                };

                RichTextBox txt = new ScrollTransparentTextBox()
                {
                    Name = $"txtNotam{pair.Key}",
                    Text = notamText,
                    WordWrap = false,
                    ScrollBars = RichTextBoxScrollBars.Horizontal,//ScrollBars.Horizontal,
                    Multiline = true,
                    ReadOnly = true,
                    ForeColor = nos.AcknowledgedNotams.ContainsKey(pair.Key) ? Color.Gray : DefaultForeColor,
                    BackColor = BackColor,
                    Font = notamFont,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                foreach (var c in coordinates)
                {
                    if (string.IsNullOrEmpty(c.ConvertedString)) continue;

                    if (c.Index < 0 || c.Index >= notamText.Length) continue;

                    string temp = txt.Text.Substring(0, c.Index);

                    int diff = temp.Length - temp.Replace("\n", "").Length;

                    txt.SelectionStart = c.Index - diff;
                    txt.SelectionLength = c.ConvertedStringLength;
                    //txt.SelectionColor = Color.Green;
                    txt.SelectionFont = new Font(txt.Font, FontStyle.Underline);
                }

                txt.SelectionStart = 0;
                txt.SelectionLength = 0;

                chk.CheckedChanged += Chk_CheckedChanged;
                txt.MouseDown += Txt_Click;

                tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpMain.RowCount += 1;

                tlpMain.Controls.Add(chk, 0, tlpMain.RowCount - 1);
                tlpMain.Controls.Add(txt, 1, tlpMain.RowCount - 1);

#if DEBUG
                Debug.WriteLine($"frmMain: Dodao red za {pair.Key}\tControl count: {tlpMain.Controls.Count}\t Row count: {tlpMain.RowCount}");
                Debug.WriteLine($"frmMain: RowStyle {tlpMain.RowStyles[tlpMain.RowCount-1].SizeType} {tlpMain.RowStyles[tlpMain.RowCount-1].Height}");
#endif
            }

            SetTextBoxHeights();

            slblData.Text = $"NOTAM-i za: {nos.LastSearch} | {tlpMain.RowCount} NOTAM{(tlpMain.RowCount == 1 ? "" : "-a")}";
        }

        private void RefreshNotams()
        {
            if (chkFilterDatum.Checked)
                showNotams(nos.GetNotamsForDate(dtpFilterDatum.Value));
            else
                showNotams(nos.CurrentNotams);
        }

        #region "Event Handlers"

        private void Txt_Click(object sender, MouseEventArgs e)
        {
#if DEBUG
            Debug.WriteLine($"frmMain: Txt_Click {e.Button}");
#endif
            if (e.Button == MouseButtons.Right)
            {
                if (!(sender is RichTextBox)) return;
                RichTextBox txt = (RichTextBox)sender;
                if (!txt.Name.StartsWith("txtNotam")) return;

                notamIdClicked = txt.Name.Substring(8); // remove "txtNotam" from the start

                cmsNotam.Show(Cursor.Position);
            }
        }

        private void NewNotamUnacknowledged(string NotamID)
        {
#if DEBUG
            Debug.WriteLine($"frmMain: New notam unack event: {NotamID}");
#endif
            var results = tlpMain.Controls.Find($"txtNotam{NotamID}", false);

            if (results.Length == 0)
            {
#if DEBUG
                Debug.WriteLine($"frmMain: Got new notam unack event but can't find TextBox for the NOTAM. [{NotamID}]");
#endif
                return;
            }

            if (!(results[0] is RichTextBox))
            {
#if DEBUG
                Debug.WriteLine($"frmMain: Got new notam unack event but txtNotam{NotamID} is not a TextBox!? [{NotamID}]");
#endif
                return;
            }

            RichTextBox txt = (RichTextBox)results[0];

            txt.ForeColor = DefaultForeColor;
        }

        private void NewNotamAcknowledged(string NotamID)
        {
#if DEBUG
            Debug.WriteLine($"frmMain: New notam ack event: {NotamID}");
#endif
            var results = tlpMain.Controls.Find($"txtNotam{NotamID}", false);

            if (results.Length == 0)
            {
#if DEBUG
                Debug.WriteLine($"frmMain: Got new notam ack event but can't find TextBox for the NOTAM. [{NotamID}]");
#endif
                return;
            }

            if (!(results[0] is RichTextBox))
            {
#if DEBUG
                Debug.WriteLine($"frmMain: Got new notam ack event but txtNotam{NotamID} is not a TextBox!? [{NotamID}]");
#endif
                return;
            }

            RichTextBox txt = (RichTextBox)results[0];

            txt.ForeColor = Color.Gray;
            txt.BackColor = txt.BackColor;
        }

        private void Chk_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is CheckBox)) return;

            CheckBox chk = (CheckBox)sender;

            if (!chk.Name.StartsWith("chkNotam")) return;

            // remove "chkNotam"
            string notamId = chk.Name.Substring(8);

            if (chk.Checked)
            {
                bool success = nos.AcknowledgeNotam(notamId);

                if (success && chkFilterAck.Checked)
                {
                    var results = tlpMain.Controls.Find($"txtNotam{notamId}", false);

                    if (results.Length == 0)
                    {
#if DEBUG
                        Debug.WriteLine($"frmMain: Can't find txtNotam in chk_chkeckChanged [{notamId}]");
#endif  
                        return;
                    }

                    int row = tlpMain.GetRow(chk);

                    tlpMain.Controls.Remove(results[0]);
                    tlpMain.Controls.Remove(chk);

                    if(tlpMain.RowStyles.Count > 0)
                        tlpMain.RowStyles.RemoveAt(0);

                    tlpMain.RowCount--;

                    slblData.Text = $"NOTAM-i za: {nos.LastSearch} | {tlpMain.RowCount} NOTAM{(tlpMain.RowCount == 1 ? "" : "-a")}";
                }
            }
            else
            {
                nos.UnacknowledgeNotam(notamId);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var chk in tlpMain.Controls.OfType<CheckBox>())
            {
                // removing "chkNotam" from the start
                string notamId = chk.Name.Substring(8, chk.Name.Length - 8);

                if (chk.Checked)
                {
                    bool success = nos.AcknowledgeNotam(notamId);
#if DEBUG
                    if (success)
                        Debug.WriteLine($"frmMain: Adding {notamId} to acknowledged");
                    else
                        Debug.WriteLine($"frmMain: Attempted to add {notamId} to acknowledged without success");
#endif
                }
                else
                {
                    bool success = nos.UnacknowledgeNotam(notamId);
#if DEBUG
                    if (success)
                        Debug.WriteLine($"frmMain: Removed {notamId} from acknowledged");
                    else
                        Debug.WriteLine($"frmMain: Attempted to remove {notamId} from acknowledged without success");
#endif
                }
            }

            if (nos.SaveToFile())
            {
                MessageBox.Show("Uspešno sačuvano!", "Sacuvan fajl", MessageBoxButtons.OK);
            }
        }

        private void prikaziProcitaneNOTAMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAckNotams.Show();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog fd = new FontDialog())
            {
                fd.Font = Properties.Settings.Default.notamFont;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    notamFont = fd.Font;

                    Properties.Settings.Default.notamFont = notamFont;
                    Properties.Settings.Default.Save();


                    RefreshNotams();
                    frmAckNotams.RefreshNotams();
                }
            }
        }

        private void dtpFilterDatum_ValueChanged(object sender, EventArgs e)
        {
            if (chkFilterDatum.Checked)
                showNotams(nos.GetNotamsForDate(dtpFilterDatum.Value));
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pretraziNotame();
                e.SuppressKeyPress = true;
            }
        }
        private void btnGetNotams_Click(object sender, EventArgs e)
        {
            pretraziNotame();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            int cursorPosition = txtSearch.SelectionStart;

            txtSearch.Text = txtSearch.Text.ToUpper();

            txtSearch.SelectionStart = cursorPosition;
        }

        private void chkFilterDatum_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterDatum.Checked)
                showNotams(nos.GetNotamsForDate(dtpFilterDatum.Value));
            else
                showNotams(nos.CurrentNotams);
        }

        private void chkFilterAck_CheckedChanged(object sender, EventArgs e)
        {
            RefreshNotams();
        }

        private void promeniPodešavanjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmSettings f = new frmSettings())
            {
                f.ShowDialog();
            }
        }


        #endregion

        private void mapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapForm.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            mapForm.ClosingApp();
            //Environment.Exit(0);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mapForm.DrawNotam(notamIdClicked);
        }
    }
}
