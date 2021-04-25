﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    class LightEnemy : IEnemy
    {
        private const int LightEnemySize = 50;
        public int Speed { get; set; } = 2;
        public int Damage { get; set; } = 15;
        public Direction Direction { get; set; }

        public readonly int MissleSpeed = 3;
        public readonly List<IMissle> Missles;

        private Vector Position;
        private readonly PictureBox Enemy;
        private readonly Dictionary<Direction, Image> EnemyRotations;
        public int health = 30;
        private readonly Timer shootTimer;
        private bool canShoot = false;
        private readonly int randDistance;

        private readonly Form activeForm;

        public LightEnemy(Form form, int missleCount)
        {
            Enemy = new PictureBox
            {
                Size = new Size(LightEnemySize, LightEnemySize),
                Image = new Bitmap(Properties.Resources.spaceShips_004, LightEnemySize, LightEnemySize)
            };

            EnemyRotations = new Dictionary<Direction, Image>
            {
                { Direction.Down, Enemy.Image }
            };
            for (var i = 1; i < 4; i++)
                EnemyRotations.Add((Direction)i, EnemyRotations[(Direction)(i - 1)].RotateImage());

            activeForm = form;

            Missles = new List<IMissle>(missleCount);
            for (var i = 0; i < Missles.Capacity; i++)
                Missles.Add(new EnemyMissle(Properties.Resources.spaceMissiles_015, Direction.None, MissleSpeed, -2000, -2000));

            Random random = new Random();
            Position = new Vector(random.Next(-150, activeForm.ClientSize.Width), -50);
            randDistance = random.Next(LightEnemySize, 200);

            shootTimer = new Timer
            {
                Interval = 1000
            };

            shootTimer.Tick += (sender, args) =>
            {
                canShoot = true;
                shootTimer.Stop();
            };
            shootTimer.Start();
        }

        public void Shoot()
        {
            if (canShoot == true)
            {
                    Debug.WriteLine($"Count - {Missles.Count}");
                var missle = Missles.FirstOrDefault(missle => missle.Direction == Direction.None);
                if (missle != null)
                {
                    missle.Direction = Direction.Down;
                    missle.Damage = Damage;
                    missle.MissleSpeed = MissleSpeed;
                    missle.SetPosition(Position.X, Position.Y);
                    missle.StartMissle();
                    canShoot = false;
                    shootTimer.Start();
                }
            }
        }

        public void MoveToPoint(Vector playerPosition)
        {
            int distance = 120;

            if (playerPosition.Distance(Position) > distance)
            {

                if (playerPosition.X > Position.X)
                {

                    Position.X += Speed;
                }
                if (playerPosition.X < Position.X)
                {
                    Position.X -= Speed;
                }
                if (playerPosition.Y > Position.Y && Position.Y < (activeForm.ClientSize.Height / 2 - randDistance))
                {
                    Enemy.Image = EnemyRotations[Direction.Down];
                    Position.Y += Speed;
                }
                if (playerPosition.Y < Position.Y)
                {
                    Position.Y -= Speed;
                }
            }
            else
                Shoot();
           
        }

        public Vector GetPosition() => Position;

        public Image GetImage() => Enemy.Image;

        public void MoveFromPoint(Vector position)
        {
            if (position.X > Position.X)
            {
                Position.X -= Speed;
            }
            if (position.X < Position.X)
            {
                Position.X += Speed;
            }
            if (position.Y > Position.Y)
            {
                Position.Y -= Speed;
            }
            if (position.Y < Position.Y)
            {
                Position.Y += Speed;
            }
            Shoot();
        }

        public bool DeadInConflict(IEnumerable<IMissle> missle)
        {
            foreach (var i in missle)
            {
                if (i.GetPosition().Distance(Position) < LightEnemySize)
                {
                    i.StopMissle();
                    health -= i.Damage;
                    return true;

                }
            }
            return false;
        }

        public PictureBox GetSource() => Enemy;

        public int GetHealth() => health;
        public List<IMissle> GetMissles() => Missles;

        public void DamageToHealth(int damage)
        {
            health -= damage;
        }
    }
}
