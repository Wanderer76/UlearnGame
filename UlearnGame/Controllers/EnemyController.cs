using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    class EnemyController
    {
        public int Wave = 1;

        private int countOfEnemies = 5;

        private  int MissleCount = 1;
        public readonly int spawnDelay = 1200;
        public int WaveTime { get; private set; } = 10000;


        public List<IEnemy> Enemies { get; private set; }
        private readonly Form activeForm;
        private readonly Timer spawnTimer;

        private readonly Timer waveTimer;

        public EnemyController(int count, Form form)
        {
            countOfEnemies = count;
            Enemies = new List<IEnemy>(countOfEnemies);
            activeForm = form;
            spawnTimer = new Timer
            {
                Interval = spawnDelay
            };

            waveTimer = new Timer
            {
                Interval = WaveTime
            };

            waveTimer.Tick += (sender, args) =>
            {
                Wave++;
                WaveTime *= Wave;
                MissleCount = MissleCount == 5 ? 5 : MissleCount++;
                countOfEnemies++;
                Enemies.Capacity = countOfEnemies;
                spawnTimer.Interval = (int)(spawnDelay * (1f / Wave));
            };

            spawnTimer.Tick += (sender, args) =>
            {
                if (Enemies.Count < Enemies.Capacity)
                    Enemies.Add(new LightEnemy(form, MissleCount));
            };
            spawnTimer.Start();
            waveTimer.Start();
        }

        public void MoveEnemies(Player mainPlayer)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                for (var j = 0; j < Enemies.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (Enemies[i].GetPosition().Distance(Enemies[j].GetPosition()) < 100)
                    {
                        Enemies[i].MoveFromPoint(Enemies[j].GetPosition());
                    }
                }
                Enemies[i].MoveToPoint(mainPlayer.GetPosition());
            }
        }

        public void CheckForHit(Player mainPlayer)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].DeadInConflict(mainPlayer.MisslePool))
                {
                    var img = Enemies[i].GetSource();
                    img.BackColor = Color.Red;
                    Enemies[i].SetSource(img);
                    if (Enemies[i].GetHealth() <= 0)
                    {
                        Enemies.RemoveAt(i);
                    }
                }
            }
        }

    }
}
