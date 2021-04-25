﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    class EnemyMissle : IMissle
    {
        private const int Width = 20;
        private const int Height = 25;

        public int Damage { get; set; }
        public int MissleSpeed { get; set; }
        public Direction Direction { get; set; }
        public PictureBox MissleImage { get; private set; }

        public Vector Position;

        private readonly Timer movingTimer = new Timer();


        private readonly Dictionary<Direction, Image> images = new Dictionary<Direction, Image>()
        {
            {Direction.Top,new Bitmap(Properties.Resources.spaceMissiles_013,Width,Height)},
            {Direction.Down,RotateImage(new Bitmap(Properties.Resources.spaceMissiles_013,Width,Height),RotateFlipType.Rotate180FlipNone)},
            {Direction.Left,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate270FlipNone)},
            {Direction.Right,RotateImage(Properties.Resources.spaceMissiles_013,RotateFlipType.Rotate90FlipNone)},
        };

        public EnemyMissle(Image image, Direction direction, int missleSpeed, int x, int y)
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
            MissleImage.Image = images[Direction.Down];

            movingTimer.Interval = MissleSpeed;

            movingTimer.Tick += new EventHandler((sender, args) =>
            {
                Position.Y += MissleSpeed;
                Direction = Direction.Down;

                if (Position.Y > Form.ActiveForm.ClientSize.Height)
                {
                    StopMissle();
                }
            });

        }

        public void StartMissle()
        {
            movingTimer.Start();
        }

        public void SetPosition(int x, int y)
        {
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
