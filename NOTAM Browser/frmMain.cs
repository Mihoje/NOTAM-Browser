using NOTAM_Browser.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NOTAM_Browser.Helpers;
using System.Threading;
using System.Web.UI;
using System.Threading.Tasks;

#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{
    /*
     * 
     * TODO: Print preset. Da moze isto da korisnik napravi preset za stampu kao sto moze za poly.
     * Da mogu da se sacuvaju vise preseta i da se biraju iz liste.
     * Da mogu da se importuju.
     * 
     */
    public partial class frmMain : Form
    {
        public int FirstTimeLoadedNotams { get; private set; }
        public int FirstTimeNotamCount { get; private set; }

        public bool FirstTimeFinishedLoading { get; private set; }

        public frmAckNotams FrmAckNotams => frmAckNotams;
        public int AckownledgedNotamsCount => nos.AcknowledgedNotams.Count;

        private bool _startedLoading = false;
        private readonly Notams nos;
        private Font notamFont;
        public frmAckNotams frmAckNotams;
        public frmMap mapForm;
        private string notamIdClicked;

        public struct DisplayedNotam 
        {
            public string NotamID;
            public List<HighlightedCoordinate> HighlightedCoordinates;
            public string Text;
        }
        
        public struct HighlightedCoordinate
        {
            public int Index;
            public int ConvertedStringLength;
        }

        public frmMain(Notams nos)
        {
            FirstTimeLoadedNotams = 0;
            FirstTimeNotamCount = -1;
            FirstTimeFinishedLoading = false;

            InitializeComponent();
            Visible = false;

            notamFont = Settings.Default.notamFont;

            if (notamFont == null)
            {
                Settings.Default.notamFont = (Font)Settings.Default.Properties["notamFont"].DefaultValue;
                Settings.Default.Save();

                notamFont = Settings.Default.notamFont;
            }

            updateSearchHistory();

            this.nos = nos;
            FirstTimeNotamCount = nos.CurrentNotams.Count;

            notamIdClicked = string.Empty;

            nos.NotamAcknowledged += NewNotamAcknowledged;
            nos.NotamUnacknowledged += NewNotamUnacknowledged;
        }
        protected override void SetVisibleCore(bool value)
        {
            if (!IsHandleCreated)
            {
                base.SetVisibleCore(false);
                return;
            }
            base.SetVisibleCore(value);
        }

        public void FinishedLoading()
        {
            frmAckNotams = new frmAckNotams(nos);
            frmAckNotams.LoadNotams();
            mapForm = new frmMap(nos);

            UpdateFooter();

            // bring to front hack
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.TopMost = true;
            this.TopMost = false;

            FirstTimeFinishedLoading = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            tooltipDesignators.SetToolTip(cmbSearch, "Unesi ICAO designator ili više njih razdvojenih zarezom.\n\nPrimer:'LYBT' ili 'LYBT,LYBA'");

            if (!_startedLoading)
            {
                _startedLoading = true;
                showNotams(new Dictionary<string, string>(nos.CurrentNotams));
                FinishedLoading();
            }
        }

        private void updateSearchHistory()
        {
            updateSearchHistory(SettingsManager.GetSearchHistory());
        }
        private void updateSearchHistory(List<string> searchHistory)
        {
            cmbSearch.Items.Clear();

            foreach (var item in searchHistory)
            {
                cmbSearch.Items.Add(item);
            }
        }

        private async void pretraziNotame()
        {
            string searchTerm = cmbSearch.Text.Trim().ToUpper();

            if (searchTerm == "") return;
            if (nos.BusyPullingNotams) return;

            slblLatestNotam.Text = "Povlačim NOTAM-e...";

            var serachHistory = SettingsManager.AddToSearchHistory(searchTerm);

            updateSearchHistory(serachHistory);

            await nos.GetFromInternet(searchTerm);

            if (chkFilterDatum.Checked)
                showNotams(nos.GetNotamsForDate(dtpFilterDatum.Value));
            else
                showNotams(new Dictionary<string, string>(nos.CurrentNotams));

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

                txt.Height = (int)textSize.Height + 1; //nekad se desava da zbog nekog razloga malo zakine pa dodajem 1 px for good measure
            }
        }

        public List<DisplayedNotam> _getNotamsReadyToShow(Dictionary<string, string> Notams)
        {
            _startedLoading = true;

            List<DisplayedNotam> displayedNotams = new List<DisplayedNotam>();

            foreach (var pair in Notams)
            {
                if (chkFilterAck.Checked && nos.AcknowledgedNotams.ContainsKey(pair.Key)) continue;

                string notamText = NormalizeNewLines(pair.Value);

                var coordinates = NotamParser.ParseCoordinates(notamText);

                var zone = NotamParser.TryFindZone(notamText);

                var highlightedCoordinates = new List<HighlightedCoordinate>();

                foreach (var c in coordinates)
                {
                    if (string.IsNullOrEmpty(c.ConvertedString)) continue;

                    if (c.Index < 0 || c.Index >= notamText.Length) continue;

                    string temp = notamText.Substring(0, c.Index);

                    int diff = temp.Length - temp.Replace("\n", "").Length;

                    highlightedCoordinates.Add(new HighlightedCoordinate()
                    {
                        Index = c.Index - diff,
                        ConvertedStringLength = c.ConvertedStringLength
                    });
                }

                if (zone != null && coordinates.Count == 0)
                {
                    if (string.IsNullOrEmpty(zone.ConvertedString)) continue;

                    if (zone.Index < 0 || zone.Index >= notamText.Length) continue;

                    string temp = notamText.Substring(0, zone.Index);

                    int diff = temp.Length - temp.Replace("\n", "").Length;

                    highlightedCoordinates.Add(new HighlightedCoordinate()
                    {
                        Index = zone.Index - diff,
                        ConvertedStringLength = zone.ConvertedStringLength
                    });
                }


                displayedNotams.Add(new DisplayedNotam()
                {
                    NotamID = pair.Key,
                    HighlightedCoordinates = highlightedCoordinates,
                    Text = notamText
                });

                if (!FirstTimeFinishedLoading)
                    FirstTimeLoadedNotams++;
            }

            return displayedNotams;
        }

        public void _displayNotams(List<DisplayedNotam> Notams)
        {
            tlpMain.SuspendLayout();
            tlpMain.Controls.Clear();
            tlpMain.RowStyles.Clear();
            tlpMain.RowCount = 0;
            
            foreach (DisplayedNotam item in Notams)
            {
                CheckBox chk = new CheckBox()
                {
                    Name = $"chkNotam{item.NotamID}",
                    Checked = nos.AcknowledgedNotams.ContainsKey(item.NotamID)
                };

                RichTextBox txt = new ScrollTransparentTextBox()
                {
                    Name = $"txtNotam{item.NotamID}",
                    Text = item.Text,
                    WordWrap = false,
                    ScrollBars = RichTextBoxScrollBars.Horizontal,//ScrollBars.Horizontal,
                    Multiline = true,
                    ReadOnly = true,
                    ForeColor = nos.AcknowledgedNotams.ContainsKey(item.NotamID) ? Color.Gray : DefaultForeColor,
                    BackColor = BackColor,
                    Font = notamFont,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                Font highlightFont = new Font(txt.Font, FontStyle.Underline);

                foreach (var hc in item.HighlightedCoordinates)
                {
                    txt.Select(hc.Index, hc.ConvertedStringLength);
                    txt.SelectionFont = highlightFont;
                }

                chk.CheckedChanged += Chk_CheckedChanged;
                txt.MouseDown += Txt_Click;

                tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpMain.RowCount += 1;

                tlpMain.Controls.Add(chk, 0, tlpMain.RowCount - 1);
                tlpMain.Controls.Add(txt, 1, tlpMain.RowCount - 1);

#if DEBUG
                Debug.WriteLine($"frmMain: Dodao red za {item.NotamID}\tControl count: {tlpMain.Controls.Count}\t Row count: {tlpMain.RowCount}");
                Debug.WriteLine($"frmMain: RowStyle {tlpMain.RowStyles[tlpMain.RowCount - 1].SizeType} {tlpMain.RowStyles[tlpMain.RowCount - 1].Height}");
#endif
            }


            SetTextBoxHeights();


            tlpMain.ResumeLayout();


            slblData.Text = $"NOTAM-i za: {nos.LastSearch} | {tlpMain.RowCount} NOTAM{(tlpMain.RowCount == 1 ? "" : "-a")}";
        }

        private void showNotams(Dictionary<string, string> Notams)
        {
            var notamsToShow = _getNotamsReadyToShow(Notams);
            _displayNotams(notamsToShow);
        }

        private void RefreshNotams()
        {
            if (chkFilterDatum.Checked)
                showNotams(nos.GetNotamsForDate(dtpFilterDatum.Value));
            else
                showNotams(new Dictionary<string, string>(nos.CurrentNotams));
        }

        public void UpdateFooter()
        {
            cmbSearch.Text = nos.LastSearch;
            cmbSearch.SelectionStart = cmbSearch.Text.Length;
            slblLatestNotam.Text = $"Poslednje ažuriranje: {nos.CurrentNotamsTime}";            
        }

        #region "Event Handlers"

        private void mapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapForm.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            mapForm.ClosingApp();
            Application.Exit();
            //Environment.Exit(0);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mapForm.DrawNotam(notamIdClicked);
        }

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
                MessageBox.Show("Uspešno sačuvano!", "Sačuvan fajl", MessageBoxButtons.OK);
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

        private void cmbSearch_KeyDown(object sender, KeyEventArgs e)
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

        private void cmbSearch_TextChanged(object sender, EventArgs e)
        {
            int cursorPosition = cmbSearch.SelectionStart;

            cmbSearch.Text = cmbSearch.Text.ToUpper();

            cmbSearch.SelectionStart = cursorPosition;
        }

        private void chkFilterDatum_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterDatum.Checked)
                showNotams(nos.GetNotamsForDate(dtpFilterDatum.Value));
            else
                showNotams(new Dictionary<string, string>(nos.CurrentNotams));
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

        private void prikažiSveSelektovaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var chk in tlpMain.Controls.OfType<CheckBox>())
            {

                if (!chk.Name.StartsWith("chkNotam")) continue;

                if (!chk.Checked) continue;

                // remove "chkNotam"
                string notamId = chk.Name.Substring(8);

                mapForm.DrawNotam(notamId);
            }
        }

        #endregion

    }
}
