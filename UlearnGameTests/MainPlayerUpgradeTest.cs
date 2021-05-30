using System.Collections.Generic;
using NUnit.Framework;
using UlearnGame.Models;
using System.Windows.Forms;
using UlearnGame.Utilities;
using UlearnGame.Interfaces;
using System.Drawing;

namespace UlearnGameTests
{
    [TestFixture]
    class MainPlayerUpgradeTest
    {

        [TestCase(UpgradePrices.MissleSpeedUpgrade)]
        [TestCase(5446)]
        [TestCase(1024)]
        public void CanUpgradeMissleSpeedTest(int score)
        {
            var player = new Player(new Form());
            var currentSpeed = player.MissleSpeed;
            Assert.IsTrue(player.UpgradeMissleSpeed(score));
            Assert.IsTrue(currentSpeed < player.MissleSpeed);
        }

        [TestCase(UpgradePrices.MissleSpeedUpgrade - 1)]
        [TestCase(10)]
        [TestCase(-1223)]
        public void CanNotUpgradeMissleSpeedTest(int score)
        {
            var player = new Player(new Form());
            Assert.IsFalse(player.UpgradeMissleSpeed(score));
        }

        [TestCase(UpgradePrices.DamageUpgrade)]
        [TestCase(5446)]
        [TestCase(1024)]
        public void CanUpgradeDamageTest(int score)
        {
            var player = new Player(new Form());
            var currentSpeed = player.Damage;
            Assert.IsTrue(player.UpgradeDamage(score));
            Assert.IsTrue(currentSpeed < player.Damage);
        }

        [TestCase(UpgradePrices.DamageUpgrade - 1)]
        [TestCase(-5446)]
        [TestCase(-1024)]
        [TestCase(34)]
        public void CanNotUpgradeDamageTest(int score)
        {
            var player = new Player(new Form());
            Assert.IsFalse(player.UpgradeDamage(score));
        }

        [TestCase(UpgradePrices.ArmorFill)]
        [TestCase(5446)]
        [TestCase(1024)]
        [TestCase(32434)]
        public void CanFillArmorTest(int score)
        {
            var player = new Player(new Form());
            var missles = new List<IMissle>
            {
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
            };

            player.OnMissleConflict(missles);
            var armor = player.Armor;

            Assert.IsTrue(player.FillArmor(score));
            Assert.IsTrue(player.Armor > armor);
        }

        [TestCase(UpgradePrices.ArmorFill - 1)]
        [TestCase(-5446)]
        [TestCase(0)]
        [TestCase(12)]
        public void CanNotFillArmorTest(int score)
        {
            var player = new Player(new Form());
            var missles = new List<IMissle>
            {
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
            };
            Assert.IsFalse(player.FillArmor(score));
            player.OnMissleConflict(missles);
            Assert.IsFalse(player.FillArmor(score));
        }

        [TestCase(UpgradePrices.HealthFill)]
        [TestCase(5446)]
        [TestCase(1024)]
        [TestCase(32434)]
        public void CanFillHealthTest(int score)
        {
            var player = new Player(new Form());
            var missles = new List<IMissle>
            {
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
                new EnemyMissle(new Bitmap(45,45),Direction.Down,231,15,955,955,945,945,player.GetPosition().X,player.GetPosition().Y),
            };

            player.OnMissleConflict(missles);
            var health = player.Health;
            Assert.IsTrue(player.FillHealth(score));
            Assert.IsTrue(player.Health > health);
        }
    }
}
