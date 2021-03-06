using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public abstract class CommonEnemy : IEnemy
    {
        public readonly Size MissleSize;
        public readonly Size EnemySize;
        
        public int Speed { get; set; }
        public int Damage { get; set; }
        public int CurrentShootDelay { get; set; } = 0;

        public readonly int MissleSpeed;
        public readonly List<IMissle> Missles;
        private int health = 30;
        private readonly int shootInterval = 1000;
        private readonly PictureBox Enemy;
        private readonly Dictionary<Direction, Image> enemyRotations;
        private readonly Form activeForm;

        private Vector position;


        public CommonEnemy(Form form, int missleCount, int health, Size missleSize, Size enemySize, int missleSpeed = 2, int shootInterval = 1000, int damage = 15, int speed = 1)
        {
            Enemy = new PictureBox
            {
                Size = new Size(enemySize.Width + 20, enemySize.Width + 20),
                Image = new Bitmap(Properties.Resources.spaceShips_004, enemySize),
            };

            MissleSize = missleSize;
            EnemySize = enemySize;
            this.health = health;

            MissleSpeed = missleSpeed;
            Damage = damage;
            Speed = speed;
            this.shootInterval = shootInterval;

            enemyRotations = new Dictionary<Direction, Image>
            {
                { Direction.Down, Enemy.Image }
            };
            for (var i = 1; i < 4; i++)
            {
                enemyRotations.Add((Direction)i, enemyRotations[(Direction)(i - 1)].RotateImage());
            }

            activeForm = form;

            Missles = new List<IMissle>(missleCount);
            for (var i = 0; i < Missles.Capacity; i++)
            {
                Missles.Add(
                    new EnemyMissle(
                        Properties.Resources.spaceMissiles_015,
                        Direction.None,
                        missleSpeed,
                        damage,
                        missleSize.Width,
                        missleSize.Height,
                        activeForm.ClientSize.Height,
                        activeForm.ClientSize.Width,
                        -2000,
                        -2000)
                    );
            }

            var random = new Random();
            position = new Vector(random.Next(-150, activeForm.ClientSize.Width), -50);
        }

        public void Shoot()
        {
            if (CurrentShootDelay >= shootInterval)
            {
                var missle = Missles.FirstOrDefault(missl => missl.GetPosition().Direction == Direction.None);
                if (missle != null)
                {
                    missle.Direction = position.Direction;
                    missle.Damage = Damage;
                    missle.MissleSpeed = MissleSpeed;
                    missle.SetPosition(position.X + MissleSize.Width, position.Y);
                    missle.StartMissle();
                    CurrentShootDelay = 0;
                }
            }
        }

        public void MoveToPoint(Vector playerPosition)
        {
            int distance = 120;

            if (position.Distance(playerPosition) >= distance)
            {
                if (playerPosition.X > position.X)
                {
                    position.X += Speed;
                }
                else if (playerPosition.X < position.X)
                {
                    position.X -= Speed;
                }
                if (playerPosition.Y > position.Y && position.Y < activeForm.ClientSize.Height / 2 - EnemySize.Width)
                {
                    position.Direction = Direction.Down;
                    Enemy.Image = enemyRotations[Direction.Down];
                    position.Y += Speed;
                }
                else if (playerPosition.Y < position.Y)
                {
                    position.Direction = Direction.Top;
                    Enemy.Image = enemyRotations[Direction.Top];
                    position.Y -= Speed;
                }
            }
        }

        public Vector GetPosition() => position;

        public Image GetImage() => Enemy.Image;

        public void MoveFromPoint(Vector position)
        {
            if (position.X >= this.position.X)
            {
                this.position.Direction = Direction.Left;
                this.position.X -= Speed;
            }
            if (position.X < this.position.X)
            {
                this.position.Direction = Direction.Right;
                this.position.X += Speed;
            }
            if (position.Y >= this.position.Y || this.position.Y > activeForm.ClientSize.Height / 2 - EnemySize.Width)
            {
                this.position.Direction = Direction.Top;
                this.position.Y -= Speed;
            }
            if (position.Y < this.position.Y)
            {
                this.position.Direction = Direction.Down;
                this.position.Y += Speed;
            }
        }

        public bool OnMissleConflict(IEnumerable<PlayerMissle> missle)
        {
            foreach (var i in missle)
            {
                if (i.GetPosition().Distance(position) < EnemySize.Height)
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
        public IEnumerable<IMissle> GetMissles() => Missles;
    }
}
