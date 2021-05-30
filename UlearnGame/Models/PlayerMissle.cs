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

        public bool IsActive { get; private set; } = false;
        public int Damage { get; set; }
        public int MissleSpeed { get; set; }
        public Direction Direction { get => position.Direction; set => position.Direction = value; }
        public PictureBox MissleImage { get; private set; }

        private readonly int maxHeight;
        private readonly int maxWidth;
        private readonly Dictionary<Direction, Image> images;

        private Vector position;

        public PlayerMissle(Image image, Direction direction, int missleSpeed, int maxHeight, int maxWidth, int x, int y)
        {
            MissleSpeed = missleSpeed;
            this.maxHeight = maxHeight;
            this.maxWidth = maxWidth;

            position = new Vector
            {
                X = x,
                Y = y,
                Direction = direction
            };

            MissleImage = new PictureBox
            {
                BackColor = Color.Transparent,
                Width = Width,
                Height = Height
            };

            images = new Dictionary<Direction, Image>();
            images.Add(Direction.Top, new Bitmap(image, Width, Height));
            images.Add(Direction.Down, RotateImage(images[Direction.Top], RotateFlipType.Rotate180FlipNone));
        }

        public void Move()
        {
            if (position.Direction == Direction.Top)
                position.Y -= MissleSpeed;

            if (position.Direction == Direction.Left)
                position.X -= MissleSpeed;

            if (position.Y < 0 || position.Y > maxHeight)
            {
                StopMissle();
            }
        }

        public void StartMissle()
        {
            MissleImage.Image = images[position.Direction];
            IsActive = true;
        }

        public void SetPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public void StopMissle()
        {
            IsActive = false;
            position = new Vector
            {
                X = -1000,
                Y = -1000,
                Direction = Direction.None
            };
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

        public Vector GetPosition() => position;
    }
}
