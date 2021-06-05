using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{

    public class Player
    {
        public const int ShipSize = 50;

        public int Health { get; set; } = 100;
        public int Armor { get; set; } = 50;
        public int Speed { get; set; } = 5;
        public int Damage { get; set; } = 10;

        public readonly int MissleSpeed = 5;
        public readonly int MissleCapacity = 5;

        private Vector position;

        public bool IsRight { get; set; }
        public bool IsLeft { get; set; }
        public bool IsUp { get; set; }
        public bool IsDown { get; set; }
        public PictureBox PlayerImage { get; set; }

        private readonly Timer shootTimer;
        private bool canShoot = false;
        private int shootInterval = 500;

        private readonly Form activeForm;

        public readonly List<IMissle> MisslePool;

        private readonly Dictionary<Direction, Image> PlayerRotations = new Dictionary<Direction, Image>();

        private readonly Image missleSource = Properties.Resources.projectile3;

        public readonly PictureBox SpeedEffect;

        public Player(Form form)
        {
            activeForm = form;
            position = new Vector(activeForm.ClientSize.Width / 2, activeForm.ClientSize.Height / 2);
            MisslePool = new List<IMissle>(MissleCapacity);

            PlayerImage = new PictureBox
            {
                //Size = new Size(ShipSize, ShipSize),
                Image = new Bitmap(Properties.Resources.themainship, ShipSize, ShipSize),
                BackColor = Color.Transparent,
            };
            PlayerRotations[Direction.Down] = PlayerImage.Image;

            for (int i = 1; i < 4; i++)
            {
                PlayerRotations.Add((Direction)i, PlayerRotations[(Direction)(i - 1)].RotateImage());
            }

            PlayerImage.Image = PlayerRotations[Direction.Top];
            position.Direction = Direction.Top;

            for (int i = 0; i < MisslePool.Capacity; i++)
            {
                MisslePool.Add(new PlayerMissle(missleSource, Direction.None, MissleSpeed, activeForm.ClientSize.Height, activeForm.ClientSize.Width, -2000, -2000));
            }

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

        public Vector GetPosition() => position;

        public void MakeMove()
        {
            if (IsUp && position.Y > 0)
            {
                position.Direction = Direction.Top;
                PlayerImage.Image = PlayerRotations[Direction.Top];
                position.Y -= Speed;
            }
            if (IsDown && position.Y + PlayerImage.Height < activeForm.ClientSize.Height)
            {
                //position.Direction = Direction.Down;
                //PlayerImage.Image = PlayerRotations[Direction.Down];
                position.Y += Speed;
            }
            if (IsRight && position.X + PlayerImage.Width < activeForm.ClientSize.Width)
            {
                //position.Direction = Direction.Right;
                //PlayerImage.Image = PlayerRotations[Direction.Right];
                position.X += Speed;
            }
            if (IsLeft && position.X > 0)
            {
                //position.Direction = Direction.Left;
                //PlayerImage.Image = PlayerRotations[Direction.Left];
                position.X -= Speed;
            }
        }

        public void MoveToRight()
        {
            if (position.X + ShipSize < activeForm.ClientSize.Width)
            {
                position.X += Speed;
            }
        }

        public void MoveToLeft()
        {
            if (position.X > 0)
            {
                position.X -= Speed;
            }
        }

        public void MoveToTop()
        {
            if (position.Y > 0)
            {
                position.Y -= Speed;
            }
        }

        public void MoveToDown()
        {
            if (position.Y + ShipSize < activeForm.ClientSize.Height)
            {
                position.Y += Speed;
            }
        }

        public void Shoot()
        {
            if (canShoot == true)
            {
                var missle = MisslePool.FirstOrDefault(missl => missl.GetPosition().Direction == Direction.None);
                if (missle != null)
                {
                    missle.Damage = Damage;
                    missle.MissleSpeed = MissleSpeed;
                    missle.Direction = position.Direction;
                    missle.SetPosition(position.X + PlayerMissle.Width, position.Y);
                    missle.StartMissle();
                    canShoot = false;
                    shootTimer.Start();
                }
            }
        }

        public bool DeadInConflict(IEnumerable<IMissle> missle)
        {
            foreach (var i in missle)
            {
                if (i is EnemyMissle)
                {
                    if (i.GetPosition().Distance(position) < ShipSize)
                    {
                        i.StopMissle();
                        if (Armor <= 0)
                        {
                            Health -= i.Damage;
                        }
                        else
                        {
                            Armor = Armor - i.Damage <= 0 ? 0 : Armor - i.Damage;
                            Health -= i.Damage / 4;

                        }
                        return true;

                    }
                }
            }
            return false;
        }
    }
}
