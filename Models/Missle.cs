using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UlearnGame.Models
{
    public class Missle
    {
        private const int Width = 25;
        private const int Height = 30;

        public int Damage { get; }

        public int MissleSpeed { get; }

        public Direction Direction { get; set; }

        public PictureBox MissleImage { get; private set; }

        private readonly Timer movingTimer = new Timer();

        private Dictionary<Direction, Image> images = new Dictionary<Direction, Image>()
        {
            {Direction.Top,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.RotateNoneFlipNone)},
            {Direction.Down,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate180FlipNone)},
            {Direction.Left,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate270FlipNone)},
            {Direction.Right,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate90FlipNone)},
        };

        public Missle(Direction direction, int missleSpeed, int x, int y)
        {
            Direction = direction;
            MissleSpeed = missleSpeed;


            MissleImage = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Left = x,
                Top = y,
                Width = Width,
                Height = Height
            };

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

                if (
                MissleImage.Left+ MissleImage.Size.Width < 0 ||
                MissleImage.Top+MissleImage.Size.Height < 0 ||
                MissleImage.Left > Form.ActiveForm.ClientSize.Width + MissleImage.Size.Width ||
                MissleImage.Top > Form.ActiveForm.ClientSize.Height + MissleImage.Size.Height)
                {
                    Direction = Direction.None;
                    movingTimer.Stop();
                }
            });

        }

        public void StartMissle()
        {
            var form = Form.ActiveForm;
            form.Controls.Add(MissleImage);
            movingTimer.Start();
        }

        public void SetCoordinates(int x, int y)
        {
            MissleImage.Left = x;
            MissleImage.Top = y;
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
