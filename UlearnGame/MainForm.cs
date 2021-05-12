using System;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;
using UlearnGame.Utilities;
using UlearnGame.Controllers;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using UlearnGame.Interfaces;
using UlearnGame.Models.Bonuses;

namespace UlearnGame
{
    public partial class MainForm : Form
    {

        private Player mainPlayer;
        private readonly Timer updateTimer;
        private EnemyController enemyController;
        private UIController uIController;
        private WaveController waveController;
        private List<IBonus> bonuses;
        private bool IsDead = false;

        private const int EnemyCount = 15;
        private const double healthSpawnProbability = 0.00001;
        private const double armorSpawnProbability = 0.0005;

        public MainForm()
        {
            DoubleBuffered = true;
            InitializeComponent();
            ControlLearnedPanel.Visible = true;
            Init();
            updateTimer = new Timer
            {
                Interval = 15
            };

            updateTimer.Tick += (sender, args) =>
            {
                OnTimerEvent();
            };
            //updateTimer.Start();

            FormClosed += (sender, args) =>
            {
                Application.Exit();
            };

            Activated += (sender, args) =>
            {
                var timer = new Timer
                {
                    Interval = 5000
                };
                timer.Tick += (sender, args) =>
                {
                    ControlLearnedPanel.Visible = false;
                    ControlLearnedPanel.Dispose();
                    waveController.StartWaves();
                    updateTimer.Start();
                    timer.Stop();
                    timer.Dispose();

                };
                timer.Start();
            };

        }

        private void OnTimerEvent()
        {
            Movement(mainPlayer);
            enemyController.MoveEnemies(mainPlayer);
            enemyController.CheckForHit(mainPlayer);
            RandomBonusGenerate();
            for (int i = 0; i < bonuses.Count; i++)
            {
                bonuses[i].StartMotion();
                bonuses[i].OnConflict(mainPlayer);
                if (bonuses[i].OnConflict(mainPlayer))
                {
                    bonuses.RemoveAt(i);
                    i--;
                }
                else if(bonuses[i].GetPosition().Y > ClientSize.Height)
                {
                    bonuses.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < enemyController.Enemies.Count; i++)
            {
                var enemyPosition = enemyController.Enemies[i].GetPosition();
                if (Math.Abs(enemyPosition.X - mainPlayer.GetPosition().X) <= Player.ShipSize * 2)
                {
                    enemyController.Enemies[i].Shoot();
                }
            }

            foreach (var enemy in enemyController.Enemies)
            {
                var enemyMissles = enemy.GetMissles();
                if (mainPlayer.OnMissleConflict(enemyMissles) && mainPlayer.Health < 0)
                {
                    IsDead = true;
                    updateTimer.Stop();
                }
            }

            if (waveController.IsWaveEnd())
            {
                uIController.Wave = waveController.Wave;
                enemyController.IncreaseWave();
            }

            if (!IsDead)
                Invalidate();
            else
                MessageBox.Show("Потрачено");
        }

        private void Init()
        {
            mainPlayer = new Player(this);
            enemyController = new EnemyController(EnemyCount, this);
            waveController = new WaveController(enemyController);
            uIController = new UIController(this, waveController, enemyController, mainPlayer);
            bonuses = new List<IBonus>(3);
            MinimumSize = new Size(720, 720);
            MaximumSize = new Size(720, 720);


            Paint += (sender, args) =>
            {
                OnPaintEvent(args);
            };
        }

        private void OnPaintEvent(PaintEventArgs args)
        {
            var graph = args.Graphics;

            graph.DrawImage(mainPlayer.PlayerImage.Image, mainPlayer.GetPosition().ToPoint());

            foreach (var bonus in bonuses.Where(bonus => bonus.GetPosition().Direction != Direction.None))
            {
                graph.DrawImage(bonus.GetImage(), bonus.GetPosition().ToPoint());
            }
            foreach (var enemy in enemyController.Enemies)
            {
                graph.DrawImage(enemy.GetImage(), enemy.GetPosition().ToPoint());
            }

            var playerMissleImage = mainPlayer.MisslePool[0].MissleImage.Image;

            foreach (var point in mainPlayer.MisslePool.Where(missle => missle.Direction != Direction.None).Select(m => m.GetPosition().ToPoint()))
            {
                graph.DrawImage(playerMissleImage, point);
            }


            DrawEnemyMissles(graph);
            uIController.Update();
        }

        private void DrawEnemyMissles(Graphics graph)
        {
            var missles = enemyController.Enemies
                .SelectMany(enemy => enemy.GetMissles().Where(missle => missle.Direction != Direction.None))
                .ToDictionary(tuple => tuple.MissleImage.Image, tuple => tuple.GetPosition().ToPoint());

            foreach (var missle in missles)
            {
                graph.DrawImage(missle.Key, missle.Value);
            }
        }

        private void Movement(Player player)
        {
            if (Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up))
            {
                player.MoveToTop();
            }
            if (Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down))
            {
                player.MoveToDown();
            }
            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
            {
                player.MoveToRight();
            }
            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
            {
                player.MoveToLeft();
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                player.Shoot();
            }
        }

        private void RandomBonusGenerate()
        {
            var random = new Random();
            if (random.NextDouble() < healthSpawnProbability)
            {
                if (bonuses.Count != bonuses.Capacity)
                    bonuses.Add(new HealthBonus(this, 25));
            }
        }

    }
}
