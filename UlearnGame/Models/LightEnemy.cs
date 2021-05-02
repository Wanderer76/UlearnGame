using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class LightEnemy : IEnemy
    {
        public const int LightEnemySize = 50;
        public int Speed { get; set; } = 1;
        public int Damage { get; set; } = 15;


        public readonly int MissleSpeed = 2;
        public readonly List<IMissle> Missles;

        private Vector Position;
        private int health = 30;
        private  PictureBox Enemy;
        //private readonly Dictionary<Direction, Image> EnemyRotations;
        private readonly Timer shootTimer;
        private bool canShoot = false;
        private readonly int randDistance;
        private readonly Form activeForm;

        public LightEnemy(Form form, int missleCount)
        {
            Enemy = new PictureBox
            {
                Size = new Size(LightEnemySize+20, LightEnemySize+20),
                Image = new Bitmap(Properties.Resources.spaceShips_004, LightEnemySize, LightEnemySize),
                BackColor = Color.Red
            };

            //EnemyRotations = new Dictionary<Direction, Image>
            //{
            //    { Direction.Down, Enemy.Image }
            //};
            //for (var i = 1; i < 4; i++)
            //    EnemyRotations.Add((Direction)i, EnemyRotations[(Direction)(i - 1)].RotateImage());

            activeForm = form;

            Missles = new List<IMissle>(missleCount);
            for (var i = 0; i < Missles.Capacity; i++)
                Missles.Add(new EnemyMissle(Properties.Resources.spaceMissiles_015, Direction.None, MissleSpeed, activeForm.ClientSize.Height, -2000, -2000));

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
                    missle.SetPosition(Position.X + EnemyMissle.Width, Position.Y);
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
                    Position.Direction = Direction.Right;
                    Position.X += Speed;
                }
                if (playerPosition.X < Position.X)
                {
                    Position.Direction = Direction.Left;
                    Position.X -= Speed;
                }
                if (playerPosition.Y > Position.Y && Position.Y < (activeForm.ClientSize.Height / 2 - randDistance))
                {
                    Position.Direction = Direction.Down;
                    //Enemy.Image = EnemyRotations[Direction.Down];
                    Position.Y += Speed;
                }
                if (playerPosition.Y < Position.Y)
                {
                    Position.Direction = Direction.Top;
                    Position.Y -= Speed;
                }
            }
        }

        public Vector GetPosition() => Position;

        public Image GetImage() => Enemy.Image;

        public void MoveFromPoint(Vector position)
        {
            if (position.X > Position.X)
            {
                Position.Direction = Direction.Left;
                Position.X -= Speed;
            }
            if (position.X < Position.X)
            {
                Position.Direction = Direction.Right;
                Position.X += Speed;
            }
            if (position.Y > Position.Y)
            {
                Position.Direction = Direction.Top;
                Position.Y -= Speed;
            }
            //if (position.Y < Position.Y)
            //{
            //    Position.Direction = Direction.Down;
            //    Position.Y += Speed;
            //}
        }

        public bool DeadInConflict(IEnumerable<IMissle> missle)
        {
            foreach (var i in missle)
            {
                if (i is PlayerMissle)
                {
                    if (i.GetPosition().Distance(Position) < LightEnemySize)
                    {
                        i.StopMissle();
                        health -= i.Damage;
                        return true;
                    }
                }
            }
            return false;
        }

        public PictureBox GetSource() => Enemy;

        public int GetHealth() => health;
        public IEnumerable<IMissle> GetMissles()
        {
            foreach (var missle in Missles)
                yield return missle;
        }

        public void DamageToHealth(int damage)
        {
            health -= damage;
        }

        public void SetSource(PictureBox box) => Enemy = box;
    }
}
