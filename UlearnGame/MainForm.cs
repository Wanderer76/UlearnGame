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

namespace UlearnGame
{
    public partial class MainForm : Form
    {
        public static int scores = 0;

        private Player mainPlayer;
        private readonly Timer updateTimer;
        private EnemyController enemyController;
        private UiView uIController;
        private WaveController waveController;
        private bool IsDead = false;

        private const int EnemyCount = 15;

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

            FormClosed += (sender, args) =>
            {
                Application.Exit();
            };

            Activated += (sender, args) =>
            {
                var timer = new Timer
                {
                    Interval = 1
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
            mainPlayer.CurentShootDelay += updateTimer.Interval;
            
            enemyController.MoveEnemies(mainPlayer);
            enemyController.CheckForHit(mainPlayer);
            Movement(mainPlayer);


            foreach(var missle in mainPlayer.MisslePool.Where(m=>m.IsActive))
            {
                missle.Move();
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
                enemy.CurrentShootDelay += updateTimer.Interval;

                var enemyMissles = enemy.GetMissles();
                foreach(var missle in enemyMissles.Where(m=>m.IsActive))
                {
                    missle.Move();
                }
                if (mainPlayer.OnMissleConflict(enemyMissles) && mainPlayer.Health < 0)
                {
                    IsDead = true;
                }
            }

            if (waveController.IsWaveEnd())
            {
                uIController.Wave = waveController.Wave;
                enemyController.IncreaseWave();
            }

            if (IsDead)
            {
                updateTimer.Stop();
                MessageBox.Show("Потрачено");
            }
            else
                Invalidate();
        }

        private void Init()
        {
            mainPlayer = new Player(this);
            enemyController = new EnemyController(EnemyCount, this);
            waveController = new WaveController(enemyController);
            uIController = new UiView(this, waveController, enemyController, mainPlayer);
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
                player.MakeMove(Key.W);
            }
            if (Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down))
            {
                player.MakeMove(Key.S);
            }
            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
            {
                player.MakeMove(Key.D);
            }
            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
            {
                player.MakeMove(Key.A);
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                player.MakeMove(Key.Space);
            }
        }
    }
}
