using System.Collections.Generic;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    class EnemyController
    {
        public const int MissleCount = 1;
        public readonly int spawnDelay = 800;

        public List<IEnemy> Enemies { get; private set; }
        private readonly Form activeForm;
        private readonly Timer spawnTimer;
        
        public EnemyController(int count, Form form)
        {
            Enemies = new List<IEnemy>(count);
            activeForm = form;
            spawnTimer = new Timer
            {
                Interval = spawnDelay
            };

            spawnTimer.Tick += (sender, args) => 
            {
                if (Enemies.Count < Enemies.Capacity)
                    Enemies.Add(new LightEnemy(form, MissleCount));
            };
            for (var i = 0; i < count; i++)
            {
                Enemies.Add(new LightEnemy(activeForm, MissleCount));
            }

            spawnTimer.Start();
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
                    if (Enemies[i].GetHealth() <= 0)
                    {
                        Enemies.RemoveAt(i);
                    }
                }
            }
        }

    }
}
