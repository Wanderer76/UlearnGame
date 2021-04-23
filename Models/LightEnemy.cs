using System;
using System.Collections.Generic;
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

        public int Speed { get; set; } = 5;

        public int Damage { get; set; } = 1;
        private Vector Position;

        public Direction Direction { get; set; }

        private PictureBox Enemy;

        private Dictionary<Direction, Image> EnemyRotations;

        public LightEnemy(Image image,Form form)
        {
            Enemy = new PictureBox();
            Enemy.Size = new Size(LightEnemySize, LightEnemySize);
            Enemy.Image = new Bitmap(image, LightEnemySize, LightEnemySize);

            EnemyRotations = new Dictionary<Direction, Image>();
            EnemyRotations.Add(Direction.Down, Enemy.Image);
            for (var i = 1; i < 4; i++)
                EnemyRotations.Add((Direction)i, EnemyRotations[(Direction)(i - 1)].RotateImage());
            Random random = new Random();
            Position = new Vector(random.Next(-100, form.ClientSize.Width), random.Next(-100, form.ClientSize.Height));
        }

        public void Shoot()
        {
            Console.WriteLine("Shoot");
        }

        public void MoveToPoint(Vector playerPosition)
        {
            int distance = 200;

            if (playerPosition.Distance(Position) > distance)
            { 
                if (playerPosition.X > Position.X )
                {
                    Position.X += Speed;
                }
                if (playerPosition.X < Position.X )
                {
                    Position.X -= Speed;
                }
                if (playerPosition.Y > Position.Y)
                {
                    Position.Y += Speed;
                }
                if (playerPosition.Y < Position.Y )
                {
                    Position.Y -= Speed;
                }
            }
        }

        public Vector GetPosition() => Position;

        public Image GetImage() => Enemy.Image;

    }
}
