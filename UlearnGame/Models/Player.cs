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
        private int maxHealth = 100;
        private int maxArmor = 50;

        public const int ShipSize = 50;

        public int Health { get; set; }
        public int Armor { get; private set; }
        public int Speed { get; private set; } = 5;
        public int Damage { get; private set; } = 10;
        public int MissleSpeed { get; private set; } = 5;

        public readonly int MissleCapacity = 5;

        private Vector position;
        public PictureBox PlayerImage { get; set; }

        private int shootInterval = 600;
        public int CurentShootDelay = 0;

        private readonly Form activeForm;

        public readonly List<PlayerMissle> MisslePool;

        private readonly Dictionary<Direction, Image> PlayerRotations = new Dictionary<Direction, Image>();

        private readonly Image missleSource = Properties.Resources.spaceMissiles_013;

        public readonly PictureBox SpeedEffect;

        public Player(Form form)
        {
            Health = maxHealth;
            Armor = maxArmor;

            activeForm = form;
            position = new Vector(activeForm.ClientSize.Width / 2, activeForm.ClientSize.Height / 2);
            MisslePool = new List<PlayerMissle>(MissleCapacity);

            PlayerImage = new PictureBox
            {
                Image = new Bitmap(Properties.Resources.spaceShips_001, ShipSize, ShipSize),
                BackColor = Color.Transparent,
            };
            SpeedEffect = new PictureBox
            {
                Image = new Bitmap(Properties.Resources.spaceEffects_002, ShipSize / 2, ShipSize / 2)
            };

            PlayerRotations[Direction.Down] = PlayerImage.Image;

            for (var i = 1; i < 4; i++)
                PlayerRotations.Add((Direction)i, PlayerRotations[(Direction)(i - 1)].RotateImage());

            PlayerImage.Image = PlayerRotations[Direction.Top];
            position.Direction = Direction.Top;

            for (var i = 0; i < MisslePool.Capacity; i++)
                MisslePool.Add(new PlayerMissle(missleSource, Direction.None, MissleSpeed, activeForm.ClientSize.Height, activeForm.ClientSize.Width, -2000, -2000));
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
                    return true;
                }
            }
            return false;
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

        public bool UpgradeSpeed(int score)
        {
            if (score >= UpgradePrices.SpeedUpgrade)
            {
                Speed++;
                return true;
            }
            return false;
        }

        public bool UpgradeMissleSpeed(int score)
        {
            if (score >= UpgradePrices.MissleUpgrade)
            {
                MissleSpeed++;
                return true;
            }
            return false;
        }

        public bool UpgradeArmor(int score)
        {
            if (score >= UpgradePrices.ArmorUpgrade)
            {
                maxArmor += 5;

                if (Armor < maxArmor)
                    Armor = maxArmor;

                return true;
            }
            return false;
        }
        public bool UpgradeHealth(int score)
        {
            if (score >= UpgradePrices.HealthUpgrade)
            {
                maxHealth += 20;

                if (Health < maxHealth)
                    Health = maxHealth;

                return true;
            }
            return false;
        }

    }
}
