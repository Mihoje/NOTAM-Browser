using NOTAM_Browser.Properties;
using System;
using System.Windows.Forms;

namespace NOTAM_Browser
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            txtUrlPre.Text = Settings.Default.urlPre ?? "";
            txtUrlAft.Text = Settings.Default.urlAft ?? "";
            txtNotamPre.Text = Settings.Default.notamPre ?? "";
            txtNotamAft.Text = Settings.Default.notamAft ?? "";
            txtMapyApiKey.Text = Settings.Default.mapyApiKey ?? "";
            txtFaaApiClientId.Text = Settings.Default.faaApiClientId ?? "";
            txtFaaApiClientSecret.Text = Settings.Default.faaApiClientSecret ?? "";

            radSrcDefault.Checked = Settings.Default.notamSource == 0;
            radSrcApi.Checked = Settings.Default.notamSource == 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (radSrcApi.Checked && 
                (string.IsNullOrEmpty(txtFaaApiClientId.Text.Trim()) || 
                string.IsNullOrEmpty(txtFaaApiClientSecret.Text.Trim()))
                )
            {
                MessageBox.Show("Moraš uneti Client ID i Client Secret kako bi koristion FAA API izvor.", "Nepotpuni podaci", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Settings.Default.urlPre = txtUrlPre.Text;
            Settings.Default.urlAft = txtUrlAft.Text;
            Settings.Default.notamPre = txtNotamPre.Text;
            Settings.Default.notamAft = txtNotamAft.Text;
            Settings.Default.mapyApiKey = txtMapyApiKey.Text;
            Settings.Default.faaApiClientId = txtFaaApiClientId.Text;
            Settings.Default.faaApiClientSecret = txtFaaApiClientSecret.Text;


            int source = -1;
            if (radSrcDefault.Checked)
                source = 0;
            else if (radSrcApi.Checked)
                source = 1;

            Settings.Default.notamSource = source;

            Settings.Default.Save();
            showRestartNotification();
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Da li si siguran da želiš da resetujes sva podešavanja?", "Resetovanje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                Settings.Default.urlPre = (string)Settings.Default.Properties["urlPre"].DefaultValue;
                Settings.Default.urlAft = (string)Settings.Default.Properties["urlAft"].DefaultValue;
                Settings.Default.notamPre = (string)Settings.Default.Properties["notamPre"].DefaultValue;
                Settings.Default.notamAft = (string)Settings.Default.Properties["notamAft"].DefaultValue;
                Settings.Default.mapyApiKey = (string)Settings.Default.Properties["mapyApiKey"].DefaultValue;
                Settings.Default.Save();
                loadData();
                showRestartNotification();
            }
        }

        private void showRestartNotification()
        {
            DialogResult dr = MessageBox.Show("Da bi nova podešavanja radila, moraš restartovati aplikaciju. Da li želiš da se aplikacija restartuje?", "Restart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                Application.Restart();
                Environment.Exit(0);
            }
        }
    }
}
