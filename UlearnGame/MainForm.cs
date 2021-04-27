using System;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;
using UlearnGame.Utilities;
using UlearnGame.Controllers;

namespace UlearnGame
{
    public partial class MainForm : Form
    {
        private Player mainPlayer;
        private readonly Timer updateTimer;
        private EnemyController enemies;

        private readonly Graphics graphics;

        private bool IsDead = false;

        private const int EnemyCount = 7;

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
                enemies.MoveEnemies(mainPlayer);
                enemies.CheckForHit(mainPlayer);

                for (int i = 0; i < enemies.Enemies.Count; i++)
                {
                    enemies.Enemies[i].Shoot();
                }

                foreach (var enemy in enemies.Enemies)
                {
                    var enemyMissles = enemy.GetMissles();
                    if (mainPlayer.DeadInConflict(enemyMissles) && mainPlayer.Health < 0)
                    {
                        IsDead = true;
                        updateTimer.Stop();
                    }
                }

                if (!IsDead)
                    Invalidate();
                else
                    MessageBox.Show("Потрачено");

            };
            updateTimer.Start();

        }

        private void Init()
        {
            mainPlayer = new Player(Properties.Resources.spaceShips_001, this);
            enemies = new EnemyController(EnemyCount, this);

            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);

            Paint += (sender, args) =>
            {
                var graph = args.Graphics;
                graph.DrawImage(mainPlayer.PlayerImage.Image, mainPlayer.GetPosition().ToPoint());

                foreach (var enemy in enemies.Enemies)
                    graph.DrawImage(enemy.GetImage(), enemy.GetPosition().ToPoint());

                foreach (var missle in mainPlayer.MisslePool)
                {
                    if (missle.Direction != Direction.None)
                        graph.DrawImage(missle.MissleImage.Image, missle.GetPosition().ToPoint());
                }

                foreach (var enemy in enemies.Enemies)
                {
                    foreach (var missle in enemy.GetMissles())
                        if (missle.Direction != Direction.None)
                            graph.DrawImage(missle.MissleImage.Image, missle.GetPosition().ToPoint());
                }
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
