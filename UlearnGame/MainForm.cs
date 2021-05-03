using System;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;
using UlearnGame.Utilities;
using UlearnGame.Controllers;
using System.Linq;

namespace UlearnGame
{
    public partial class MainForm : Form
    {
        private Player mainPlayer;
        private readonly Timer updateTimer;
        private EnemyController enemyController;
        private UIController uIController;
        private WaveController waveController;

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
            updateTimer.Start();

        }

        private void OnTimerEvent()
        {
            mainPlayer.MakeMove();
            //Movement(mainPlayer);
            enemyController.MoveEnemies(mainPlayer);
            enemyController.CheckForHit(mainPlayer);


            for (int i = 0; i < enemyController.Enemies.Count; i++)
            {
                enemyController.Enemies[i].Shoot();
            }

            foreach (var enemy in enemyController.Enemies)
            {
                var enemyMissles = enemy.GetMissles();
                if (mainPlayer.DeadInConflict(enemyMissles) && mainPlayer.Health < 0)
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

            waveController.StartWaves();
           

            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);

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
                graph.DrawImage(enemy.GetImage(), enemy.GetPosition().ToPoint());

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

        //TODO Новый способ
        private void Movement(Player player)
        {
            /* if(Keyboard.IsKeyDown(Key.))
             {
                 mainPlayer.MoveToTop();
             }*/
        }


        //Старый
        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            Keys keyCode = args.KeyCode;

            if (keyCode == Keys.W || keyCode == Keys.Up)
            {
                mainPlayer.SpeedEffect.Visible = true;
                mainPlayer.IsUp = true;
            }
            if (keyCode == Keys.S || keyCode == Keys.Down)
            {
                mainPlayer.SpeedEffect.Visible = true;
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
                mainPlayer.Shoot();
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs args)
        {
            Keys keyCode = args.KeyCode;
            if (keyCode == Keys.W || keyCode == Keys.Up)
            {
                mainPlayer.SpeedEffect.Visible = false;

                mainPlayer.IsUp = false;
            }
            if (keyCode == Keys.S || keyCode == Keys.Down)
            {
                mainPlayer.SpeedEffect.Visible = false;

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
