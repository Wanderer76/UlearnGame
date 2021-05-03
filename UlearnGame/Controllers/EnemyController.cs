using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    public class EnemyController
    {
        public bool IsEnd { get; private set; } = false;

        private int currentWave = 1;
        private int MissleCount = 1;
        private int spawnEnemiesCount = 0;

        private int damage = 15;
        private int speed = 1;
        private int missleSpeed = 2;

        private int spawnDelay = 1200;

        private readonly Timer spawnTimer;

        public int CountOfEnemies { get; private set; }
        public int DeadCount { get; private set; } = 0;
        public List<IEnemy> Enemies { get; private set; }


        public EnemyController(int count, Form form)
        {
            CountOfEnemies = count;
            Enemies = new List<IEnemy>(count);

            spawnTimer = new Timer
            {
                Interval = spawnDelay
            };
            spawnTimer.Tick += (sender, args) =>
            {
                if (spawnEnemiesCount < Enemies.Capacity && Enemies.Count < 10)
                {
                    Enemies.Add(new LightEnemy(form, MissleCount, missleSpeed, damage, speed));
                    spawnEnemiesCount++;
                }
                else if (DeadCount == CountOfEnemies)
                    StopSpawn();
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
            spawnEnemiesCount = 0;
            Enemies.Clear();
            DeadCount = 0;
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

                    if (Enemies[i].GetPosition().Distance(Enemies[j].GetPosition()) < 120)
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
                        DeadCount++;
                    }
                }
            }
        }

        public void IncreaseWave()
        {
            CountOfEnemies++;
            Enemies.Capacity = CountOfEnemies;
            currentWave++;
            if (currentWave % 2 == 0)
            {
                MissleCount = MissleCount + 1 > 6 ? 6 : MissleCount++;
                damage = damage + 1 > 30 ? 30 : damage++;
                speed = speed + 1 > 3 ? 3 : speed++;
                missleSpeed = missleSpeed + 1 > 7 ? 7 : missleSpeed++;
            }
            spawnDelay = spawnDelay < 900 ? 900 : spawnDelay - 50;
            spawnTimer.Interval = spawnDelay;
        }

    }
}
