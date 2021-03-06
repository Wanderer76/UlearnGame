using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class CommonMissle : IMissle
    {
        public int Damage { get; set; }
        public int MissleSpeed { get; set; }
        public bool IsActive { get; private set; } = false;
        public Direction Direction { get => position.Direction; set => position.Direction = value; }
        public PictureBox MissleImage { get; private set; }
        private Vector position;

        private readonly int maxHeight;
        private readonly int maxWidth;

        private readonly Dictionary<Direction, Image> images;

        public CommonMissle(Image image, Direction direction, int missleSpeed, int damage, int width, int height, int maxHeight, int maxWidth, int x, int y)
        {
            MissleSpeed = missleSpeed;
            Damage = damage;
            images = new Dictionary<Direction, Image>();
            images.Add(Direction.Top, new Bitmap(image, width, height));
            images.Add(Direction.Right, RotateImage(images[Direction.Top], RotateFlipType.Rotate90FlipNone));
            images.Add(Direction.Down, RotateImage(images[Direction.Top], RotateFlipType.Rotate180FlipNone));
            images.Add(Direction.Left, RotateImage(images[Direction.Down], RotateFlipType.Rotate90FlipNone));

            position = new Vector { X = x, Y = y, Direction = direction };
            this.maxHeight = maxHeight;
            this.maxWidth = maxWidth;

            MissleImage = new PictureBox
            {
                Image = new Bitmap(image, width, height),
                BackColor = Color.Transparent,
                Width = width,
                Height = height
            };


        }

        public void Move()
        {
            switch (Direction)
            {
                case Direction.Top:
                    position.Y -= MissleSpeed;
                    break;
                case Direction.Down:
                    position.Y += MissleSpeed;
                    break;
            }
            if (position.Y > maxHeight || position.Y < 0)
            {
                StopMissle();
            }
        }

        public void StartMissle()
        {
            MissleImage.Image = images[Direction];
            IsActive = true;
        }

        public void SetPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public void StopMissle()
        {
            Direction = Direction.None;
            position = new Vector(-1000, -1000);
            IsActive = false;
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
