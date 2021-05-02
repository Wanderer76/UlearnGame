using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class PlayerMissle : IMissle
    {
        public const int Width = 20;
        public const int Height = 25;

        public int Damage { get; set; }
        public int MissleSpeed { get; set; }
        public Direction Direction { get; set; }
        public PictureBox MissleImage { get; private set; }

        public Vector Position;

        private readonly Timer movingTimer = new Timer();


        private readonly Dictionary<Direction, Image> images = new Dictionary<Direction, Image>();

        public PlayerMissle(Image image, Direction direction, int missleSpeed, int x, int y)
        {
            Direction = direction;
            MissleSpeed = missleSpeed;

            Position.X = x;
            Position.Y = y;

            MissleImage = new PictureBox
            {
                Image = new Bitmap(image, Width, Height),
                BackColor = Color.Transparent,
                Width = Width,
                Height = Height
            };

            images.Add(Direction.Top, MissleImage.Image);
            images.Add(Direction.Right, RotateImage(images[Direction.Top], RotateFlipType.Rotate90FlipNone));
            images.Add(Direction.Down, RotateImage(images[Direction.Right], RotateFlipType.Rotate90FlipNone));
            images.Add(Direction.Left, RotateImage(images[Direction.Down], RotateFlipType.Rotate90FlipNone));


            movingTimer.Interval = MissleSpeed;

            movingTimer.Tick += (sender, args) =>
            {
                if (Direction == Direction.Top)
                    Position.Y -= MissleSpeed;

               /* if (Direction == Direction.Left)
                    Position.X -= MissleSpeed;

                if (Direction == Direction.Right)
                    Position.X += MissleSpeed;

                if (Direction == Direction.Down)
                    Position.Y += MissleSpeed;
               */
                if (Position.Y < 0)
                {
                    StopMissle();
                }
            };

        }

        public void StartMissle()
        {
           // MissleImage.Image = images[Direction];
            movingTimer.Start();
        }

        public void SetPosition(int x, int y)
        {
            Position.X = x;// + Width;
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
