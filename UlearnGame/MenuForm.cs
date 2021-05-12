﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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