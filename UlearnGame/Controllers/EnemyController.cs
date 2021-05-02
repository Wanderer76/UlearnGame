using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    class EnemyController
    {
        public bool IsEnd { get; private set; } = false;

        private int currentWave = 1;
        private int countOfEnemies = 5;
        private int MissleCount = 1;
        public int spawnDelay = 1200;

        private int deadCount = 0;

        public List<IEnemy> Enemies { get; private set; }
        private readonly Timer spawnTimer;


        public EnemyController(int count, Form form)
        {
            countOfEnemies = count;
            Enemies = new List<IEnemy>(countOfEnemies);
            spawnTimer = new Timer
            {
                Interval = spawnDelay
            };

            spawnTimer.Tick += (sender, args) =>
            {
                if (deadCount != countOfEnemies)
                    Enemies.Add(new LightEnemy(form, MissleCount));
                else
                    IsEnd = true;
            };
        }
        public void StartSpawn()
        {
            IsEnd = false;
            spawnTimer.Start();
        }
        public void StopSpawn()
        {
            IsEnd = true;
            spawnTimer.Stop();
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
                        deadCount++;
                        Enemies.RemoveAt(i);
                    }
                }
            }
        }

        public void IncreaseWave()
        {
            countOfEnemies++;
            Enemies.Capacity = countOfEnemies;
            currentWave++;
            MissleCount = currentWave % 2 == 0 ? MissleCount++ : MissleCount;
            spawnDelay = spawnDelay < 900 ? 900 : spawnDelay - 50;
            spawnTimer.Interval = spawnDelay;
        }

    }
}
