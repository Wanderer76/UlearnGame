using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    class LightEnemy : IEnemy
    {
        private const int LightEnemySize = 50;

        public int Speed { get; set; } = 2;

        public int Damage { get; set; } = 1;
        private Vector Position;

        public Direction Direction { get; set; }

        private PictureBox Enemy;

        private Dictionary<Direction, Image> EnemyRotations;

        private readonly int randDistance;

        public LightEnemy(Image image, Form form)
        {
            Enemy = new PictureBox();
            Enemy.Size = new Size(LightEnemySize, LightEnemySize);
            Enemy.Image = new Bitmap(image, LightEnemySize, LightEnemySize);

            EnemyRotations = new Dictionary<Direction, Image>();
            EnemyRotations.Add(Direction.Down, Enemy.Image);
            for (var i = 1; i < 4; i++)
                EnemyRotations.Add((Direction)i, EnemyRotations[(Direction)(i - 1)].RotateImage());
            Random random = new Random();
            Position = new Vector(random.Next(-100, form.ClientSize.Width), -10);
            randDistance = random.Next(LightEnemySize, 200);
        }

        public void Shoot()
        {
            Console.WriteLine("Shoot");
        }

        public void MoveToPoint(Vector playerPosition)
        {
            int distance = 120;

            if (playerPosition.Distance(Position) > distance)
            {

                if (playerPosition.X > Position.X)
                {

                    //Enemy.Image = EnemyRotations[Direction.Right];
                    Position.X += Speed;
                }
                if (playerPosition.X < Position.X)
                {
                    //  Enemy.Image = EnemyRotations[Direction.Left];

                    Position.X -= Speed;
                }
                if (playerPosition.Y > Position.Y && Position.Y < (Form.ActiveForm.ClientSize.Height / 2 - randDistance))
                {
                    Enemy.Image = EnemyRotations[Direction.Down];
                    Position.Y += Speed;
                }
                if (playerPosition.Y < Position.Y)
                {
                    //Enemy.Image = EnemyRotations[Direction.Top];

                    Position.Y -= Speed;
                }
            }
            // Debug.WriteLine($"Enemy Position - {Position.X}:{Position.Y}\n Player Position - {playerPosition.X}:{playerPosition.Y}");
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
            /* if (position.Y > Position.Y)
             {
                 Position.Y -= Speed;
             }
             if (position.Y < Position.Y)
             {
                 Position.Y += Speed;
             }*/
        }

        public bool DeadInConflict(IEnumerable<PlayerMissle> missle)
        {

            foreach (var i in missle)
            {
                //if (i.MissleImage.Bounds.IntersectsWith(Enemy.Bounds))
                if(i.Position.Distance(Position) < 100)
                {
                    Debug.WriteLine($"true");
                    return true;

                }
            }
            return false;
        }

        public PictureBox GetSource()
        {
            return Enemy;
        }
    }
}
