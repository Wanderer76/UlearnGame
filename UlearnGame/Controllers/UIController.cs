using System;
using System.Collections.Generic;
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
        private Label healthLabel;
        private Label armorLabel;
        private Label waveLabel;

        public UIController(Form form, Player player)
        {
            this.form = form;
            this.player = player;

            healthLabel = new Label
            {
                Text = $"Health:{player.Health}",
                ForeColor = Color.White,
                //Anchor = AnchorStyles.Top,
                //Size = new Size(form.ClientSize.Width, 50),
                //TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(FontFamily.GenericSansSerif, 15),
                Dock = DockStyle.Fill
            };
            armorLabel = new Label
            {
                Text = $"Armor:{player.Armor}",
                ForeColor = Color.White,
                //Anchor = AnchorStyles.Left,
                //TextAlign = ContentAlignment.MiddleLeft,
                //Size = new Size(form.ClientSize.Width, 50),
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 15)
            };

            var tableLayout = new TableLayoutPanel();
            tableLayout.Size = new Size(form.ClientSize.Width, 50);
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tableLayout.Controls.Add(armorLabel, 0, 0);
            tableLayout.Controls.Add(healthLabel, 1, 0);

            //form.Controls.Add(healthLabel);
            //form.Controls.Add(armorLabel);
            form.Controls.Add(tableLayout);
        }

        public void Update()
        {
            healthLabel.Text = $"Health:{player.Health}";
            armorLabel.Text = $"Armor:{player.Armor}";
        }

    }
}
