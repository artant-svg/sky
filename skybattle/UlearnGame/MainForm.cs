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
            Movement(mainPlayer);
            enemyController.MoveEnemies(mainPlayer);
            enemyController.CheckForHit(mainPlayer);


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
            {
                Invalidate();
            }
            else
            {
                updateTimer.Stop();
                MessageBox.Show("You lose");
                Hide();
            }
        }

        private void Init()
        {
            mainPlayer = new Player(this);
            enemyController = new EnemyController(EnemyCount, this);
            waveController = new WaveController(enemyController);
            uIController = new UIController(this, waveController, enemyController, mainPlayer);

            waveController.StartWaves();

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

        private static void Movement(Player player)
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

        protected override void OnSizeChanged(EventArgs e) => base.OnSizeChanged(e);
    }
}
