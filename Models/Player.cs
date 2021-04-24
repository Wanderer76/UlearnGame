using System.Collections.Generic;
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

        public Vector Position;

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

        public readonly List<PlayerMissle> MisslePool = new List<PlayerMissle>(5);

        private readonly Dictionary<Direction, Image> PlayerRotations = new Dictionary<Direction, Image>();


        public Player(Image image, int height, int width)
        {
            WindowHeight = height;
            WindowWidth = width;
            PlayerImage.Size = new Size(ShipSize, ShipSize);
            PlayerImage.Image = new Bitmap(image, ShipSize, ShipSize);
            PlayerRotations[Direction.Down] = PlayerImage.Image;

            for (var i = 1; i < 4; i++)
                PlayerRotations.Add((Direction)i, PlayerRotations[(Direction)(i - 1)].RotateImage());


            for (var i = 0; i < MisslePool.Capacity; i++)
                MisslePool.Add(new PlayerMissle(Direction.None, MissleSpeed, -2000, -2000));

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

        public void MakeMove()
        {
            if (IsUp && Position.Y > 0)
            {
                CurrentDirection = Direction.Top;
                PlayerImage.Image = PlayerRotations[Direction.Top];
                Position.Y -= Speed;
            }
            if (IsDown && Position.Y + PlayerImage.Height < WindowHeight)
            {
              //  CurrentDirection = Direction.Down;
              //  PlayerImage.Image = PlayerRotations[Direction.Down];
                Position.Y += Speed;
            }
            if (IsRight && Position.X + PlayerImage.Width < WindowWidth)
            {
              //  CurrentDirection = Direction.Right;
             //   PlayerImage.Image = PlayerRotations[Direction.Right];
                Position.X += Speed;
            }
            if (IsLeft && Position.X > 0)
            {
             //   CurrentDirection = Direction.Left;
             //   PlayerImage.Image = PlayerRotations[Direction.Left];
                Position.X -= Speed;
            }
        }
     
        public void Shoot(Graphics graphics)
        {
            if (canShoot == true)
            {
                var missle = MisslePool.FirstOrDefault(missle => missle.Direction == Direction.None);
                if (missle != null)
                {
                    missle.Direction = CurrentDirection;
                    missle.Damage = Damage;
                    missle.MissleSpeed = MissleSpeed;
                    missle.SetCoordinates(Position.X, Position.Y);
                    missle.StartMissle(graphics);
                    canShoot = false;
                    shootTimer.Start();
                }
            }
        }
    }
}
