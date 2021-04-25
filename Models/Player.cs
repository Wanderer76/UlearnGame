﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{

    public class Player
    {
        public const int ShipSize = 50;

        public int Health { get; set; } = 100;
        public int Armor { get; set; } = 50;
        public int Speed { get; set; } = 5;
        public int Damage { get; set; } = 10;

        private int MissleSpeed = 5;

        private Vector position;

        public bool IsRight { get; set; }
        public bool IsLeft { get; set; }
        public bool IsUp { get; set; }
        public bool IsDown { get; set; }
        public PictureBox PlayerImage { get; set; }

        private readonly Timer shootTimer;
        private bool canShoot = false;

        private int WindowHeight { get; }
        private int WindowWidth { get; }

        public readonly List<IMissle> MisslePool = new List<IMissle>(5);

        private readonly Dictionary<Direction, Image> PlayerRotations = new Dictionary<Direction, Image>();

        private readonly Image missleSource = Properties.Resources.spaceMissiles_013;

        public Player(Image image, int height, int width)
        {
            WindowHeight = height;
            WindowWidth = width;
            position = new Vector(width / 2, height / 2);

            PlayerImage = new PictureBox
            {
                Size = new Size(ShipSize, ShipSize),
                Image = new Bitmap(image, ShipSize, ShipSize)
            };

            PlayerRotations[Direction.Down] = PlayerImage.Image;

            for (var i = 1; i < 4; i++)
                PlayerRotations.Add((Direction)i, PlayerRotations[(Direction)(i - 1)].RotateImage());


            for (var i = 0; i < MisslePool.Capacity; i++)
                MisslePool.Add(new PlayerMissle(missleSource, Direction.None, MissleSpeed, -2000, -2000));

            shootTimer = new Timer
            {
                Interval = 250
            };

            shootTimer.Tick += (sender, args) =>
            {
                canShoot = true;
                shootTimer.Stop();
            };
            shootTimer.Start();
        }

        public Vector GetPosition() => position;

        public void MakeMove()
        {
            if (IsUp && position.Y > 0)
            {
                position.Direction = Direction.Top;
                PlayerImage.Image = PlayerRotations[Direction.Top];
                position.Y -= Speed;
            }
            if (IsDown && position.Y + PlayerImage.Height < WindowHeight)
            {
                //  CurrentDirection = Direction.Down;
                //  PlayerImage.Image = PlayerRotations[Direction.Down];
                position.Y += Speed;
            }
            if (IsRight && position.X + PlayerImage.Width < WindowWidth)
            {
                //  CurrentDirection = Direction.Right;
                //   PlayerImage.Image = PlayerRotations[Direction.Right];
                position.X += Speed;
            }
            if (IsLeft && position.X > 0)
            {
                //   CurrentDirection = Direction.Left;
                //   PlayerImage.Image = PlayerRotations[Direction.Left];
                position.X -= Speed;
            }
        }

        public void Shoot(Graphics graphics)
        {
            if (canShoot == true)
            {
                var missle = MisslePool.FirstOrDefault(missle => missle.Direction == Direction.None);
                if (missle != null)
                {
                    missle.Direction = position.Direction;
                    missle.Damage = Damage;
                    missle.MissleSpeed = MissleSpeed;
                    missle.SetPosition(position.X, position.Y);
                    missle.StartMissle();
                    canShoot = false;
                    shootTimer.Start();
                }
            }
        }

        public bool DeadInConflict(IEnumerable<IMissle> missle)
        {
            foreach (var i in missle)
            {
                if (i is EnemyMissle)
                {
                    if (i.GetPosition().Distance(position) < ShipSize)
                    {
                        i.StopMissle();
                        Health -= i.Damage;
                        return true;

                    }
                }
            }
            return false;
        }
    }
}
