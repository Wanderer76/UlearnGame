using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Models;

namespace UlearnGame.Controllers
{
    class EnemyController
    {
        public List<IEnemy> Enemies { get; private set; }
        private readonly Form activeForm;

        public EnemyController(int count, Form form)
        {
            Enemies = new List<IEnemy>(count);
            activeForm = form;
            for (var i = 0; i < count; i++)
            {
                Enemies.Add(new LightEnemy(activeForm, 3));
            }
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
                Enemies[i].MoveToPoint(mainPlayer.Position);
            }
        }

        public void CheckForHit(Player mainPlayer)
        {
            int deadCount = 0;

            for (int i = 0; i < Enemies.Count; i++)
            {

                if (Enemies[i].DeadInConflict(mainPlayer.MisslePool))
                {
                    if (Enemies[i].GetHealth() <= 0)
                    {
                        Enemies.RemoveAt(i);
                        deadCount++;
                    }
                }
            }
            if (deadCount > 0)
            {
                for (var i = 0; i < deadCount; i++)
                    Enemies.Add(new LightEnemy(activeForm, 3));
            }
        }


    }
}
