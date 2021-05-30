using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class Player
    {
        private const int maxHealth = 100;
        private const int maxArmor = 50;
        public const int ShipSize = 50;

        public int Health { get; set; }
        public int Armor { get; private set; }
        public int Speed { get; private set; } = 5;
        public int Damage { get; private set; } = 10;
        public int MissleSpeed { get; private set; } = 5;
        public PictureBox PlayerImage { get; set; }

        private readonly Form activeForm;
        private readonly Image missleSource = Properties.Resources.spaceMissiles_013;
        public readonly int MissleCapacity = 5;
        public readonly List<PlayerMissle> MisslePool;

        private Vector position;
        private int shootInterval = 600;
        public int CurentShootDelay = 0;

        public Player(Form form)
        {
            Health = maxHealth;
            Armor = maxArmor;

            activeForm = form;
            position = new Vector
            {
                X = activeForm.ClientSize.Width / 2,
                Y = activeForm.ClientSize.Height / 2,
                Direction = Direction.Top
            };

            MisslePool = new List<PlayerMissle>(MissleCapacity);

            PlayerImage = new PictureBox
            {
                Image = new Bitmap(Properties.Resources.spaceShips_001, ShipSize, ShipSize),
                BackColor = Color.Transparent,
            };
            PlayerImage.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);

            for (var i = 0; i < MisslePool.Capacity; i++)
            {
                MisslePool.Add(new PlayerMissle(
                        missleSource,
                        Direction.None,
                        MissleSpeed,
                        activeForm.ClientSize.Height,
                        activeForm.ClientSize.Width,
                        -2000,
                        -2000
                        ));
            }
        }

        public Vector GetPosition() => position;

        public void MakeMove(Key key)
        {
            if (key == Key.W)
            {
                MoveToTop();
            }
            if (key == Key.S)
            {
                MoveToDown();
            }
            if (key == Key.D)
            {
                MoveToRight();
            }
            if (key == Key.A)
            {
                MoveToLeft();
            }
            if (key == Key.Space)
            {
                Shoot();
            }
        }

        private void MoveToRight()
        {
            if (position.X + ShipSize < activeForm.ClientSize.Width)
                position.X += Speed;
        }

        private void MoveToLeft()
        {
            if (position.X > 0)
                position.X -= Speed;
        }

        private void MoveToTop()
        {
            if (position.Y > 0)
                position.Y -= Speed;
        }

        private void MoveToDown()
        {
            if (position.Y + ShipSize < activeForm.ClientSize.Height)
                position.Y += Speed;
        }

        private void Shoot()
        {
            if (CurentShootDelay >= shootInterval)
            {
                var missle = MisslePool.FirstOrDefault(missle => missle.GetPosition().Direction == Direction.None);
                if (missle != null)
                {
                    CurentShootDelay = 0;
                    missle.Damage = Damage;
                    missle.MissleSpeed = MissleSpeed;
                    missle.Direction = position.Direction;
                    missle.SetPosition(position.X + PlayerMissle.Width, position.Y);
                    missle.StartMissle();
                }
            }
        }

        public bool OnMissleConflict(IEnumerable<IMissle> missle)
        {
            var isHited = false;
            foreach (var i in missle)
            {
                if (i.GetPosition().Distance(position) < ShipSize - 20)
                {
                    i.StopMissle();
                    if (Armor <= 0)
                        Health -= i.Damage;
                    else
                    {
                        Armor = Armor - i.Damage <= 0 ? 0 : Armor - i.Damage;
                        Health -= i.Damage / 4;

                    }
                    isHited = true;
                }
            }
            return isHited;
        }

        public bool UpgradeDamage(int score)
        {
            if (score >= UpgradePrices.DamageUpgrade)
            {
                Damage += 2;
                return true;
            }
            return false;
        }

        public bool UpgradeMissleSpeed(int score)
        {
            if (score >= UpgradePrices.MissleSpeedUpgrade)
            {
                MissleSpeed++;
                return true;
            }
            return false;
        }

        public bool FillArmor(int score)
        {
            if (score >= UpgradePrices.ArmorFill)
            {
                if (Armor < maxArmor)
                    Armor = maxArmor;
                return true;
            }
            return false;
        }

        public bool FillHealth(int score)
        {
            if (score >= UpgradePrices.HealthFill)
            {
                if (Health < maxHealth)
                    Health = maxHealth;
                return true;
            }
            return false;
        }

    }
}
