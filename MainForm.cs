using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Models;
using UlearnGame.Utilities;
using System.Linq;
using System.Diagnostics;

namespace UlearnGame
{
    public partial class MainForm : Form
    {
        private Player mainPlayer;
        private Timer updateTimer;
        private List<IEnemy> enemies;
        public Graphics graphics;

        private const int EnemyCount = 5;

        public MainForm()
        {
            DoubleBuffered = true;
            InitializeComponent();
            graphics = CreateGraphics();
            Init();

            updateTimer = new Timer
            {
                Interval = 15
            };

            updateTimer.Tick += (sender, args) =>
            {



                mainPlayer.MakeMove();

                MoveEnemy();


                var deadEnemies = new List<int>();
                for (int i = 0; i < enemies.Count; i++)
                {
                    IEnemy enemy = enemies[i];
                    if (enemy.DeadInConflict(mainPlayer.MisslePool))
                        deadEnemies.Add(i);
                }
                if (deadEnemies.Count > 0)
                {
                    Debug.WriteLine($"Count - {deadEnemies.Count}");
                    for (var i = deadEnemies[0]; i < deadEnemies.Count; i++)
                        enemies.RemoveAt(i);
                }


                Invalidate();
            };
            updateTimer.Start();

        }

        private void MoveEnemy()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (var j = 0; j < enemies.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (enemies[i].GetPosition().Distance(enemies[j].GetPosition()) < 100)
                        enemies[i].MoveFromPoint(enemies[j].GetPosition());
                }
                enemies[i].MoveToPoint(mainPlayer.Position);
            }
        }

        private void Init()
        {
            mainPlayer = new Player(Properties.Resources.spaceShips_001, ClientSize.Height, ClientSize.Width)
            {
                Position = new Vector(ClientSize.Width / 2, ClientSize.Height / 2)
            };

            enemies = new List<IEnemy>();
            for (var i = 0; i < EnemyCount; i++)
            {
                enemies.Add(new LightEnemy(Properties.Resources.spaceShips_004, this));
            }


            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);
            Paint += (sender, args) =>
            {
                args.Graphics.DrawImage(mainPlayer.PlayerImage.Image, mainPlayer.Position.ToPoint());
                foreach (var enemy in enemies)
                    args.Graphics.DrawImage(enemy.GetImage(), enemy.GetPosition().ToPoint());

                foreach (var missle in mainPlayer.MisslePool)
                    if (missle.Direction != Direction.None)
                        args.Graphics.DrawImage(missle.MissleImage.Image, missle.Position.ToPoint());
            };
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            Keys keyCode = args.KeyCode;

            if (keyCode == Keys.W || keyCode == Keys.Up)
            {
                mainPlayer.IsUp = true;
            }
            if (keyCode == Keys.S || keyCode == Keys.Down)
            {
                mainPlayer.IsDown = true;
            }
            if (keyCode == Keys.A || keyCode == Keys.Left)
            {
                mainPlayer.IsLeft = true;
            }
            if (keyCode == Keys.D || keyCode == Keys.Right)
            {
                mainPlayer.IsRight = true;
            }
            if (keyCode == Keys.Space)
            {
                mainPlayer.Shoot(graphics);

            }
        }

        private void OnKeyUp(object sender, KeyEventArgs args)
        {
            Keys keyCode = args.KeyCode;
            if (keyCode == Keys.W || keyCode == Keys.Up)
            {
                mainPlayer.IsUp = false;
            }
            if (keyCode == Keys.S || keyCode == Keys.Down)
            {
                mainPlayer.IsDown = false;
            }
            if (keyCode == Keys.A || keyCode == Keys.Left)
            {
                mainPlayer.IsLeft = false;
            }
            if (keyCode == Keys.D || keyCode == Keys.Right)
            {
                mainPlayer.IsRight = false;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
    }


}
