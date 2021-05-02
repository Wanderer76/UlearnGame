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

        public UIController(Form form,Player player)
        {
            this.form = form;
            this.player = player;
            healthLabel = new Label();
            healthLabel.Text = $"Health:{player.Health}";
            healthLabel.ForeColor = Color.White;
            healthLabel.Anchor = AnchorStyles.Top;
            healthLabel.Size = new Size(form.ClientSize.Width, 50);
            healthLabel.TextAlign = ContentAlignment.MiddleCenter;
            healthLabel.Font = new Font(FontFamily.GenericSansSerif, 15);
            form.Controls.Add(healthLabel);
        }

        public void Update()
        {
            healthLabel.Text = $"Health:{player.Health}";
        }

    }
}
