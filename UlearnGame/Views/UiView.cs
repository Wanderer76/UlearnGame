using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    public class UiView
    {
        private MainForm mainForm;
        private readonly Player player;
        private readonly EnemyController enemyController;
        private readonly Label healthLabel;
        private readonly Label armorLabel;
        private readonly Label waveLabel;
        private readonly Label scoresLabel;

        public int Wave { get; set; }

        public int WaveTime { get; set; }

        public UiView(MainForm mainForm, WaveController waveController, EnemyController enemyController, Player player)
        {
            this.player = player;
            this.enemyController = enemyController;
            this.mainForm = mainForm;
            Wave = waveController.Wave;

            healthLabel = new Label
            {
                Text = $"Health:{player.Health}",
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 10),
                Dock = DockStyle.Fill
            };
            armorLabel = new Label
            {
                Text = $"Armor:{player.Armor}",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 10)
            };
            scoresLabel = new Label
            {
                Text = $"Scores:{MainForm.scores}",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 10)
            };
            waveLabel = new Label
            {
                Text = $"Wave:{waveController.Wave} Count of enemies:{enemyController.CountOfEnemies - enemyController.DeadCount}",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 10)
            };

            var tableLayout = new TableLayoutPanel
            {
                Size = new Size(mainForm.ClientSize.Width, 50),
            };
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.Controls.Add(armorLabel, 0, 0);
            tableLayout.Controls.Add(healthLabel, 1, 0);
            tableLayout.Controls.Add(scoresLabel, 2, 0);
            tableLayout.Controls.Add(waveLabel, 3, 0);

            mainForm.Controls.Add(tableLayout);
        }

        public void Update()
        {
            healthLabel.Text = $"Health:{player.Health}";
            armorLabel.Text = $"Armor:{player.Armor}";
            scoresLabel.Text = $"Scores:{MainForm.scores}";
            waveLabel.Text = $"Wave:{Wave} Count of enemies:{enemyController.CountOfEnemies - enemyController.DeadCount}";
        }

        public void ShowUpgradePanel()
        {
            mainForm.upgradePanel.Visible = true;

        }
        public void HideUpgradePanel()
        {
            mainForm.upgradePanel.Visible = false;
        }

    }

}
