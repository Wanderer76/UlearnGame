using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UlearnGame.Models
{
    class Missle
    {
        private const int Width = 25;
        private const int Height = 30;

        public int Damage { get; }
        public int X { get; }
        public int Y { get; }

        public int MissleSpeed { get; }

        public Direction Direction { get; }

        public PictureBox MissleImage { get; private set; }

        private Timer movingTimer = new Timer();


        public Dictionary<Direction, Image> images = new Dictionary<Direction, Image>()
        {
            {Direction.Top,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.RotateNoneFlipNone)},
            {Direction.Down,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate180FlipNone)},
            {Direction.Left,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate270FlipNone)},
            {Direction.Right,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate90FlipNone)},
        };

        public Missle(Form form, Direction direction, int bulletSpeed, int x, int y)
        {
            MissleSpeed = bulletSpeed;
            MissleImage = new PictureBox();
            MissleImage.SizeMode = PictureBoxSizeMode.StretchImage;
            MissleImage.BackColor = Color.Transparent;
            MissleImage.Left = x;
            MissleImage.Top = y;
            MissleImage.Width = Width;
            MissleImage.Height = Height;
            Direction = direction;

            form.Controls.Add(MissleImage);

            movingTimer.Interval = MissleSpeed;
            movingTimer.Tick += new EventHandler((sender, args) =>
            {
                switch (Direction)
                {
                    case Direction.Down:
                        MissleImage.Image = images[Direction.Down];
                        MissleImage.Top += MissleSpeed;
                        break;
                    case Direction.Left:
                        MissleImage.Image = images[Direction.Left];
                        MissleImage.Width = Height;
                        MissleImage.Height = Width;
                        MissleImage.Left -= MissleSpeed;
                        break;
                    case Direction.Top:
                        MissleImage.Image = images[Direction.Top];
                        MissleImage.Top -= MissleSpeed;
                        break;
                    case Direction.Right:
                        MissleImage.Width = Height;
                        MissleImage.Height = Width;
                        MissleImage.Image = images[Direction.Right];
                        MissleImage.Left += MissleSpeed;
                        break;
                }

                if (MissleImage.Left < 0 ||
                MissleImage.Top < 0 ||
                MissleImage.Left > form.ClientSize.Width + MissleImage.Size.Width ||
                MissleImage.Top > form.ClientSize.Height + MissleImage.Size.Height)
                {
                    movingTimer.Stop();
                    movingTimer.Dispose();
                    MissleImage.Dispose();
                    MissleImage = null;
                    movingTimer = null;
                }

            });
            movingTimer.Start();
        }
        private static Image RotateImage(Image img, RotateFlipType angle)
        {

            var bmp = new Bitmap(img);

            using (var hraph = Graphics.FromImage(bmp))
            {
                hraph.Clear(Color.Transparent);
                hraph.DrawImage(img, 0, 0, img.Width, img.Height);
            }
            bmp.RotateFlip(angle);
            return bmp;
        }
    }
}
