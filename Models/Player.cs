using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UlearnGame.Models
{

    public enum Direction
    {
        Down,
        Left,
        Top,
        Right,
        None
    };

    public class Player
    {
        public const int ShipSize = 50;
        private const int MissleSpeed = 6;

        public int Health { get; set; } = 100;
        public int Armor { get; set; } = 50;
        public int Speed { get; set; } = 5;
        public int X { get; set; }
        public int Y { get; set; }

        public Direction CurrentDirection { get; set; }
        public bool IsRight { get; set; }
        public bool IsLeft { get; set; }
        public bool IsUp { get; set; }
        public bool IsDown { get; set; }
        public PictureBox PlayerImage { get; set; } = new PictureBox();

        private readonly Timer shootTimer;

        private bool canShoot = false;

        private int WindowHeight { get; }
        private int WindowWidth { get; }

        private readonly List<Missle> MisslePool = new List<Missle>(10);

        private readonly Dictionary<Direction, Image> rotations = new Dictionary<Direction, Image>();


        public Player(Image image, int height, int width)
        {
            WindowHeight = height;
            WindowWidth = width;
            PlayerImage.Image = new Bitmap(image, ShipSize, ShipSize);
            rotations[Direction.Down] = PlayerImage.Image;

            for (var i = 1; i < 4; i++)
                rotations.Add((Direction)i, RotateImage(rotations[(Direction)(i - 1)]));

            for (var i = 0; i < MisslePool.Capacity; i++)
            {
                MisslePool.Add(new Missle(Direction.None, MissleSpeed, X, Y));
            }

            shootTimer = new Timer
            {
                Interval = 250
            };

            shootTimer.Tick += (sender, args) =>
            {
                canShoot = true;
                shootTimer.Stop();
            }; shootTimer.Start();
        }

        public void MakeMove()
        {
            if (IsUp && Y > 10)
            {
                CurrentDirection = Direction.Top;
                PlayerImage.Image = rotations[Direction.Top];
                Y -= Speed;
            }
            if (IsDown && Y + PlayerImage.Height < WindowHeight)
            {
                CurrentDirection = Direction.Down;
                PlayerImage.Image = rotations[Direction.Down];
                Y += Speed;
            }
            if (IsRight && X + PlayerImage.Width < WindowWidth)
            {
                CurrentDirection = Direction.Right;
                PlayerImage.Image = rotations[Direction.Right];
                X += Speed;
            }
            if (IsLeft && X > 0)
            {
                CurrentDirection = Direction.Left;
                PlayerImage.Image = rotations[Direction.Left];
                X -= Speed;
            }
        }
        private Image RotateImage(Image img)
        {
            var bmp = new Bitmap(img);
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }

        public void Shoot()
        {
            if (canShoot == true)
            {
                var current = MisslePool.FirstOrDefault(missle => missle.Direction == Direction.None);
                if (current != null)
                {
                   
                    current.Direction = CurrentDirection;
                    current.SetCoordinates(X, Y);
                    current.StartMissle();
                    canShoot = false;
                    shootTimer.Start();
                }
            }
        }

    }
}
