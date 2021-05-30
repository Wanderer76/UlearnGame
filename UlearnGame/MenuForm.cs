using System;
using System.Drawing;
using System.Windows.Forms;

namespace UlearnGame
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
            MinimumSize = new Size(720, 720);
            MaximumSize = new Size(720, 720);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Hide();
            var mainForm = new MainForm();
            mainForm.Show();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
