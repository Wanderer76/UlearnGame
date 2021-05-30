using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using UlearnGame.Controllers;
using UlearnGame.Models;

namespace UlearnGameTests
{
    [TestFixture]
    class EnemyControllerTest
    {
        [TestCase]
        public void StartSpawnTest()
        {
            var form = new Form
            {
                ClientSize = new Size(640, 480)
            };
            var enemyController = new EnemyController(5, form);
            enemyController.StartSpawn();
            Assert.IsFalse(enemyController.IsEnd);
        }

        [TestCase]
        public void StopSpawnTest()
        {
            var form = new Form
            {
                ClientSize = new Size(640, 480)
            };
            var enemyController = new EnemyController(104, form);
            enemyController.StopSpawn();
            Assert.IsTrue(enemyController.IsEnd);
        }

        [TestCase]
        public void IncreaseWaveTest()
        {
            var form = new Form
            {
                ClientSize = new Size(640, 480)
            };
            var enemyController = new EnemyController(5, form);

            for (var i = 0; i < 10; i++)
            {
                var enemiesCount = enemyController.CountOfEnemies;
                enemyController.IncreaseWave();
                Assert.IsTrue(enemiesCount < enemyController.CountOfEnemies);
            }
        }


    }
}