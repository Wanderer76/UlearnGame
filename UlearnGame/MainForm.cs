using System;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;
using UlearnGame.Utilities;
using UlearnGame.Controllers;
using System.Linq;
using System.Windows.Input;

namespace UlearnGame
{
    public partial class MainForm : Form
    {
        public static int scores = 0;

        private Player mainPlayer;
        private EnemyController enemyController;
        private WaveController waveController;
        private UiView uiView;

        private readonly Timer updateTimer;
        private bool IsDead = false;

        private const int EnemyCount = 15;

        public MainForm()
        {
            DoubleBuffered = true;
            InitializeComponent();
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
                updateTimer.Start();
                waveController.StartWaves();
            };
        }

        private void OnTimerEvent()
        {
            mainPlayer.CurentShootDelay += updateTimer.Interval;

            enemyController.MoveEnemiesToPlayer(mainPlayer);
            enemyController.CheckForHit(mainPlayer);
            Movement();

            foreach (var missle in mainPlayer.MisslePool.Where(m => m.IsActive))
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
                foreach (var missle in enemyMissles.Where(m => m.IsActive))
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
                uiView.Wave = waveController.Wave;
                enemyController.IncreaseWave();
            }

            if (enemyController.IsEnd)
            {
                uiView.ShowUpgradePanel();
            }
            else
            {
                uiView.HideUpgradePanel();
            }

            if (IsDead)
            {
                OnDead();
            }
            else
                Invalidate();
        }

        private void OnDead()
        {
            updateTimer.Stop();
            scores = 0;
            foreach (var form in Application.OpenForms)
            {
                if (form is MenuForm menuForm)
                {
                    menuForm.Show();
                }
            }
            Hide();
            MessageBox.Show("Потрачено");
        }

        private void Init()
        {
            mainPlayer = new Player(this);
            enemyController = new EnemyController(EnemyCount, this);
            waveController = new WaveController(enemyController);
            uiView = new UiView(this, waveController, enemyController, mainPlayer);
            MinimumSize = new Size(720, 720);
            MaximumSize = new Size(720, 720);

            upgradeDamageButton.Text += $"\n {UpgradePrices.DamageUpgrade} scores";
            upgradeSpeedButton.Text += $"\n {UpgradePrices.MissleSpeedUpgrade} scores";
            fillHealthButton.Text += $"\n {UpgradePrices.HealthFill} scores";
            fillArmorButton.Text += $"\n {UpgradePrices.ArmorFill} scores";


            upgradeDamageButton.Click += (sender, args) =>
            {
                if (mainPlayer.UpgradeDamage(scores))
                    scores -= UpgradePrices.DamageUpgrade;
            };

            upgradeSpeedButton.Click += (sender, args) =>
            {
                if (mainPlayer.UpgradeMissleSpeed(scores))
                    scores -= UpgradePrices.MissleSpeedUpgrade;

            };

            fillArmorButton.Click += (sender, args) =>
            {
                if (mainPlayer.FillArmor(scores))
                    scores -= UpgradePrices.ArmorFill;
            };
            fillHealthButton.Click += (sender, args) =>
            {
                if (mainPlayer.FillHealth(scores))
                    scores -= UpgradePrices.HealthFill;
            };


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
            uiView.Update();
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

        private void Movement()
        {
            if (Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up))
            {
                mainPlayer.MakeMove(Key.W);
            }
            if (Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down))
            {
                mainPlayer.MakeMove(Key.S);
            }
            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
            {
                mainPlayer.MakeMove(Key.D);
            }
            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
            {
                mainPlayer.MakeMove(Key.A);
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                mainPlayer.MakeMove(Key.Space);
            }
        }
    }
}
