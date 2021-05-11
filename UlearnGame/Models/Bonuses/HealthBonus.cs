using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models.Bonuses
{
    public class HealthBonus : IBonus
    {
        public const int Size = 50;
        private readonly PictureBox image;
        private Vector position;
        private readonly int health;
        private readonly Timer moveTimer;

        public HealthBonus(Vector position, Form form, int health)
        {
            image = new PictureBox
            {
                Image = new Bitmap(Properties.Resources.cardiogram, Size, Size)
            };
            this.health = health;
            this.position = position;

            moveTimer = new Timer
            {
                Interval = 10
            };
            moveTimer.Tick += (sender, args) =>
                {
                    position.Direction = Direction.Down;
                    position.Y += 2;
                    if (position.Y > form.ClientSize.Height)
                        moveTimer.Stop();
                };
        }

        public int GetBonus() => health;
        public Vector GetPosition() => position;
        public Image GetImage() => image.Image;
        public void StartMotion()
        {
            if (!moveTimer.Enabled)
                moveTimer.Start();
        }

        public void StopMotion()
        {
            position.Direction = Direction.None;
            moveTimer.Stop();
        }

        public bool OnConflict(Player player)
        {
            if (position.Distance(player.GetPosition()) < Size)
            {
                player.Health += GetBonus();
                return true;
            }
            return false;
        }
    }
}
