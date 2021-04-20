using System;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;

namespace UlearnGame
{
    public partial class MainForm : Form
    {
        private Player mainPlayer;
        private readonly Timer updateTimer;
       

        public MainForm()
        {
            DoubleBuffered = true;
            Graphics graphics = CreateGraphics();
            InitializeComponent();
            Init();

            

            updateTimer = new Timer
            {
                Interval = 10
            };

            updateTimer.Tick += (sender, args) =>
            {
                mainPlayer.MakeMove();
                Invalidate();
            };
            updateTimer.Start();
           
        }

        private void Init()
        {
            mainPlayer = new Player(Properties.Resources.spaceShips_001, ClientSize.Height, ClientSize.Width)
            {
                X = ClientSize.Width / 2,
                Y = ClientSize.Height / 2
            };

            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);
            Paint += (sender, args) =>
            {
                args.Graphics.DrawImage(mainPlayer.PlayerImage.Image, new Point(mainPlayer.X, mainPlayer.Y));
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
                mainPlayer.Shoot(this);
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
