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
using System.Diagnostics;

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
            updateTimer.Start();

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
                };
                timer.Start();
            };

        }

        private void OnTimerEvent()
        {
            Movement(mainPlayer);
            enemyController.MoveEnemies(mainPlayer);
            enemyController.CheckForHit(mainPlayer);

            for (int i = 0; i < bonuses.Count; i++)
            {
                var bonus = bonuses[i];
                bonus.StartMotion();
                bonus.OnConflict(mainPlayer);
                //if (bonus.OnConflict(mainPlayer))
                //{
                //    bonuses.RemoveAt(i);
                //    i--;
                //}
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
            MinimumSize = new Size(720, 720);
            MaximumSize = new Size(720, 720);

            bonuses = new List<IBonus>(3)
            {
               // new HealthBonus( this, 25),
               // new HealthBonus( this, 25),
               // new HealthBonus( this, 25)
            };

            //waveController.StartWaves();

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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
