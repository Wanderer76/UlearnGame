using System.Windows.Forms;

namespace UlearnGame.Controllers
{
    public class WaveController
    {
        public int Wave { get; private set; } = 1;

        private readonly EnemyController enemyController;
        private readonly Timer delayTimer;

        public WaveController(EnemyController enemyController)
        {
            this.enemyController = enemyController;
            delayTimer = new Timer
            {
                Interval = 5000
            };
            delayTimer.Tick += (sender, args) =>
            {
                delayTimer.Stop();
                enemyController.StartSpawn();
            };
        }

        public bool IsWaveEnd()
        {
            if (enemyController.DeadCount == enemyController.CountOfEnemies)
            {
                enemyController.StopSpawn();
                Wave++;
                delayTimer.Start();
                return true;
            }
            return false;
        }

        public void StartWaves()
        {
            delayTimer.Start();
            enemyController.StartSpawn();
        }

        public void StopWaves()
        {
            delayTimer.Stop();
            enemyController.StopSpawn();
        }

    }
}
