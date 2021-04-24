using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class PlayerMissle : IMissle
    {
        private const int Width = 20;
        private const int Height = 25;

        public int Damage { get; set; }
        public int MissleSpeed { get; set; }
        public Direction Direction { get; set; }
        public PictureBox MissleImage { get; private set; }
        public Vector Position;

        private readonly Timer movingTimer = new Timer();


        private Dictionary<Direction, Image> images = new Dictionary<Direction, Image>()
        {
            {Direction.Top,new Bitmap(Properties.Resources.spaceMissiles_013,Width,Height)},
            {Direction.Down,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate180FlipNone)},
            {Direction.Left,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate270FlipNone)},
            {Direction.Right,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate90FlipNone)},
        };

        public PlayerMissle(Direction direction, int missleSpeed, int x, int y)
        {
            Direction = direction;
            MissleSpeed = missleSpeed;

            Position.X = x;
            Position.Y = y;

            MissleImage = new PictureBox
            {
                BackColor = Color.Transparent,
                //Left = x + Width,
                //Top = y,
                Width = Width,
                Height = Height
            };
            MissleImage.Image = images[Direction.Top];

            movingTimer.Interval = MissleSpeed;

            movingTimer.Tick += new EventHandler((sender, args) =>
            {

                /*switch (Direction)
                 {
                     case Direction.Down:
                         MissleImage.Image = images[Direction.Down];
                         MissleImage.Top += MissleSpeed;
                         break;
                     case Direction.Left:
                         MissleImage.Image = images[Direction.Left];

                         MissleImage.Left -= MissleSpeed;
                         break;
                     case Direction.Top:
                         MissleImage.Image = images[Direction.Top];
                         break;
                     case Direction.Right:
                         MissleImage.Image = images[Direction.Right];
                         MissleImage.Left += MissleSpeed;
                         break;
                 }*/
                Position.Y -= MissleSpeed;
                Direction = Direction.Top;

                /*if (
                MissleImage.Left + MissleImage.Size.Width < 0 ||
                MissleImage.Top + MissleImage.Size.Height < 0 ||
                MissleImage.Left > Form.ActiveForm.ClientSize.Width + MissleImage.Size.Width ||
                MissleImage.Top > Form.ActiveForm.ClientSize.Height + MissleImage.Size.Height)*/
                if (Position.Y < 0)
                {
                    StopMissle();
                }
            });

        }

        public void StartMissle()
        {
            movingTimer.Start();
        }

        public void SetCoordinates(int x, int y)
        {
            //MissleImage.Left = x + Width;
            //MissleImage.Top = y;
            Position.X = x + Width;
            Position.Y = y;
        }

        public void StopMissle()
        {
            Position = new Vector(-1000, -1000);
            Direction = Direction.None;
            movingTimer.Stop();

        }

        private static Image RotateImage(Image img, RotateFlipType angle)
        {
            var bmp = new Bitmap(img);

            using (var graph = Graphics.FromImage(bmp))
            {
                graph.Clear(Color.Transparent);
                graph.DrawImage(img, 0, 0, img.Width, img.Height);
            }
            bmp.RotateFlip(angle);
            return bmp;
        }
        public Vector GetPosition() => Position;
    }
}
