using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    class UIController
    {
        private Form form;
        private Player player;
        private EnemyController enemyController;
        private Label healthLabel;
        private Label armorLabel;
        private Label waveLabel;

        public int Wave { get; set; }

        public int WaveTime { get; set; }

        public UIController(Form form, WaveController waveController, EnemyController enemyController, Player player)
        {
            this.form = form;
            this.player = player;
            this.enemyController = enemyController;
            Wave = waveController.Wave;

            healthLabel = new Label
            {
                Text = $"Health:{player.Health}",
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 15),
                Dock = DockStyle.Fill
            };
            armorLabel = new Label
            {
                Text = $"Armor:{player.Armor}",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 15)
            };
            waveLabel = new Label
            {
                Text = $"Wave:{waveController.Wave} Count of enemies:{enemyController.CountOfEnemies - enemyController.DeadCount}",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 15)
            };

            var tableLayout = new TableLayoutPanel
            {
                Size = new Size(form.ClientSize.Width, 50),
            };
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.Controls.Add(armorLabel, 0, 0);
            tableLayout.Controls.Add(healthLabel, 1, 0);
            tableLayout.Controls.Add(waveLabel, 2, 0);

            form.Controls.Add(tableLayout);
        }

        public void Update()
        {
            healthLabel.Text = $"Health:{player.Health}";
            armorLabel.Text = $"Armor:{player.Armor}";
            waveLabel.Text = $"Wave:{Wave} Count of enemies:{enemyController.CountOfEnemies - enemyController.DeadCount}";
        }

    }

}
