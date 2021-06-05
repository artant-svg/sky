using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class EnemyMissle : IMissle
    {

        public int Damage { get; set; }
        public int MissleSpeed { get; set; }
        public Direction Direction { get => position.Direction; set => position.Direction = value; }
        public PictureBox MissleImage { get; private set; }

        private Vector position;

        private readonly Timer movingTimer;
        private readonly Dictionary<Direction, Image> images;

        public EnemyMissle(Image image, Direction direction, int missleSpeed,int width ,int height,int maxHeight, int maxWidth, int x, int y)
        {
            MissleSpeed = missleSpeed;

            images = new Dictionary<Direction, Image>
            {
                { Direction.Top, new Bitmap(image, width, height) }
            };
            images.Add(Direction.Right, RotateImage(images[Direction.Top], RotateFlipType.Rotate90FlipNone));
            images.Add(Direction.Down, RotateImage(images[Direction.Top], RotateFlipType.Rotate180FlipNone));
            images.Add(Direction.Left, RotateImage(images[Direction.Down], RotateFlipType.Rotate90FlipNone));

            position = new Vector { X = x, Y = y, Direction = direction };

            MissleImage = new PictureBox
            {
                Image = new Bitmap(image, width, height),
                BackColor = Color.Transparent,
                Width = width,
                Height = height
            };

            movingTimer = new Timer
            {
                Interval = MissleSpeed
            };

            movingTimer.Tick += new EventHandler((sender, args) =>
            {
                switch (Direction)
                {
                    case Direction.Top:
                        position.Y -= MissleSpeed;
                        break;
                    case Direction.Down:
                        position.Y += MissleSpeed;
                        break;
                    case Direction.Left:
                        position.X -= MissleSpeed;
                        break;
                    case Direction.Right:
                        position.X += MissleSpeed;
                        break;
                }
                if (position.Y > maxHeight || position.Y < 0)
                {
                    StopMissle();
                }
                if (position.X > maxWidth || position.X < 0)
                {
                    StopMissle();
                }
            });
        }

        public void StartMissle()
        {
            MissleImage.Image = images[Direction];
            movingTimer.Start();
        }

        public void SetPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public void StopMissle()
        {
            Direction = Direction.None;
            position = new Vector(-1000, -1000);
            movingTimer.Stop();
        }

        private static Image RotateImage(Image img, RotateFlipType angle)
        {
            var bmp = new Bitmap(img);
            using (var graph = Graphics.FromImage(bmp))
            {
                graph.Clear(Color.Transparent);
                graph.DrawImage(img, 0, 0, img.Width, img.Height);
            }
            bmp.RotateFlip(angle);
            return bmp;
        }
        public Vector GetPosition() => position;
    }
}
