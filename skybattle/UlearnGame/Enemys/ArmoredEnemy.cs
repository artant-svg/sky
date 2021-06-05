using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class ArmoredEnemy : IEnemy
    {
        public const int MissleWidth = 20;
        public const int MissleHeight = 25;

        public int Damage { get; set; }

        public const int ArmoredEnemySize = 60;
        public int Speed { get; set; }
        public readonly int MissleSpeed;
        public readonly List<IMissle> Missles;
        private Vector position;
        private int health = 30;
        private PictureBox Enemy;
        private readonly Dictionary<Direction, Image> enemyRotations;
        private readonly Timer shootTimer;
        private bool canShoot = false;
        private readonly Form activeForm;

        public ArmoredEnemy(Form form, int missleCount, int missleSpeed = 3, int shootInterval = 1000, int damage = 20, int speed = 1)
        {
            Enemy = new PictureBox
            {
                Size = new Size(ArmoredEnemySize + 20, ArmoredEnemySize + 20),
                Image = new Bitmap(Properties.Resources.alien2, ArmoredEnemySize, ArmoredEnemySize),
            };

            MissleSpeed = missleSpeed;
            Damage = damage;
            Speed = speed;

            enemyRotations = new Dictionary<Direction, Image>
            {
                { Direction.Down, Enemy.Image }
            };
            for (int i = 1; i < 4; i++)
            {
                enemyRotations.Add((Direction)i, enemyRotations[(Direction)(i - 1)].RotateImage());
            }

            activeForm = form;

            Missles = new List<IMissle>(missleCount);
            for (int i = 0; i < Missles.Capacity; i++)
            {
                Missles.Add(
                    new EnemyMissle(
                        Properties.Resources.projectile1,
                        Direction.None,
                        missleSpeed,
                        MissleWidth,
                        MissleHeight,
                        activeForm.ClientSize.Height,
                        activeForm.ClientSize.Width,
                        -2000,
                        -2000)
                    );
            }

            var random = new Random();
            position = new Vector(random.Next(-150, activeForm.ClientSize.Width), -50);

            shootTimer = new Timer
            {
                Interval = shootInterval
            };

            shootTimer.Tick += (sender, args) =>
            {
                canShoot = true;
                shootTimer.Stop();
            };
            shootTimer.Start();
        }

        public void DamageToHealth(int damage) => throw new NotImplementedException();
        public bool OnMissleConflict(IEnumerable<IMissle> missle)
        {
            foreach (var i in missle)
            {
                if (i is PlayerMissle)
                {
                    if (i.GetPosition().Distance(position) < ArmoredEnemySize)
                    {
                        i.StopMissle();
                        health -= i.Damage;
                        return true;
                    }
                }
            }
            return false;
        }
        public int GetHealth() => health;
        public Image GetImage() => Enemy.Image;
        public IEnumerable<IMissle> GetMissles() => Missles;
        public Vector GetPosition() => position;
        public PictureBox GetSource() => Enemy;
        public void MoveFromPoint(Vector position)
        {
            if (position.X >= this.position.X)
            {
                this.position.Direction = Direction.Left;
                this.position.X -= Speed;
            }
            if (position.X < this.position.X)
            {
                this.position.Direction = Direction.Right;
                this.position.X += Speed;
            }
            if (position.Y >= this.position.Y || this.position.Y > activeForm.ClientSize.Height / 2)
            {
                this.position.Direction = Direction.Top;
                this.position.Y -= Speed;
            }
            if (position.Y < this.position.Y)
            {
                this.position.Direction = Direction.Down;
                this.position.Y += Speed;
            }
        }
        public void MoveToPoint(Vector playerPosition)
        {
            int distance = 120;

            if (position.Distance(playerPosition) >= distance)
            {
                if (playerPosition.X > position.X)
                {

                    position.X += Speed;
                }
                else if (playerPosition.X < position.X)
                {

                    position.X -= Speed;
                }
                if (playerPosition.Y > position.Y && position.Y < (activeForm.ClientSize.Height / 2))
                {
                    position.Direction = Direction.Down;
                    Enemy.Image = enemyRotations[Direction.Down];
                    position.Y += Speed;
                }
                else if (playerPosition.Y < position.Y)
                {

                    position.Y -= Speed;
                }
            }
        }
        public void SetSource(PictureBox box) => Enemy = box;
        public void Shoot()
        {
            if (canShoot == true)
            {
                var missle = Missles.FirstOrDefault(missl => missl.GetPosition().Direction == Direction.None);
                if (missle != null)
                {
                    missle.Direction = position.Direction;
                    missle.Damage = Damage;
                    missle.MissleSpeed = MissleSpeed;
                    missle.SetPosition(position.X + MissleWidth, position.Y);
                    missle.StartMissle();
                    canShoot = false;
                    shootTimer.Start();
                }
            }
        }
    }
}
