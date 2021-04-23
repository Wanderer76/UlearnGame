using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Models;
using UlearnGame.Utilities;

namespace UlearnGame
{
    public partial class MainForm : Form
    {
        private Player mainPlayer;
        private Timer updateTimer;
        private List<IEnemy> enemies;

        private const int EnemyCount = 5;

        public MainForm()
        {
            DoubleBuffered = true;
            Graphics graphics = CreateGraphics();
            InitializeComponent();
            Init();

            updateTimer = new Timer
            {
                Interval = 15
            };

            updateTimer.Tick += (sender, args) =>
            {
                mainPlayer.MakeMove();
                for (int i = 0; i < enemies.Count; i++)
                {
                    for (var j = 0; j < enemies.Count; j++)
                    {
                        if (i != j)
                        {
                            if (enemies[i].GetPosition().Distance(enemies[j].GetPosition()) < 100)
                                enemies[i].MoveFromPoint(enemies[j].GetPosition());

                        }
                    }
                            enemies[i].MoveToPoint(mainPlayer.Position);
                }

                Invalidate();
            };
            updateTimer.Start();

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
                mainPlayer.Shoot();
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
