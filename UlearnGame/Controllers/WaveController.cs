using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UlearnGame.Controllers
{
    class WaveController
    {
        private const int startWaveTime = 10;
        public int Wave { get; private set; } = 1;
        public int WaveTime { get; private set; } = 15;

        private EnemyController enemyController;

        private readonly Timer waveTimer;
        private Timer delayTimer;


        public event Action OnWaveEnds;

        public WaveController(EnemyController enemyController)
        {
            this.enemyController = enemyController;

            waveTimer = new Timer
            {
                Interval = 1000
            };

            waveTimer.Tick += (sender, args) =>
            {
                WaveTime = WaveTime == 0 ? 0 : WaveTime - 1;
                if (WaveTime == 0 && enemyController.IsEnd == true)
                {
                    Wave++;
                    WaveTime = startWaveTime * Wave;
                    OnWaveEnds?.Invoke();
                    enemyController.StopSpawn();
                    delayTimer.Start();
                }
            };

            delayTimer = new Timer
            {
                Interval = 5000
            };
            delayTimer.Tick += (sender, args) =>
            {
                enemyController.StartSpawn();
                delayTimer.Stop();
                waveTimer.Start();
            };
        }

        public void StartWaves()
        {
            delayTimer.Start();
            enemyController.StartSpawn();
        }

    }
}
