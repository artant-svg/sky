using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class PlayerMissle : IMissle
    {
        public const int Width = 15;
        public const int Height = 20;

        public int Damage { get; set; }
        public int MissleSpeed { get; set; }
        public Direction Direction { get => position.Direction; set => position.Direction = value; }
        public PictureBox MissleImage { get; private set; }

        private Vector position;
        private readonly Timer movingTimer;
        private readonly Dictionary<Direction, Image> images;

        public PlayerMissle(Image image, Direction direction, int missleSpeed, int maxHeight, int maxWidth, int x, int y)
        {
            //Direction = direction;
            MissleSpeed = missleSpeed;

            position = new Vector
            {
                X = x,
                Y = y,
                Direction = direction
            };

            MissleImage = new PictureBox
            {
                BackColor = Color.Transparent,
                Width = Width,
                Height = Height
            };

            images = new Dictionary<Direction, Image>
            {
                { Direction.Top, new Bitmap(image, Width, Height) }
            };
            images.Add(Direction.Right, RotateImage(images[Direction.Top], RotateFlipType.Rotate90FlipNone));
            images.Add(Direction.Down, RotateImage(images[Direction.Right], RotateFlipType.Rotate90FlipNone));
            images.Add(Direction.Left, RotateImage(images[Direction.Down], RotateFlipType.Rotate90FlipNone));

            movingTimer = new Timer
            {
                Interval = MissleSpeed
            };

            movingTimer.Tick += (sender, args) =>
            {
                if (position.Direction == Direction.Top)
                {
                    position.Y -= MissleSpeed;
                }

                if (position.Direction == Direction.Left)
                {
                    position.X -= MissleSpeed;
                }

                if (position.Direction == Direction.Right)
                {
                    position.X += MissleSpeed;
                }

                if (position.Direction == Direction.Down)
                {
                    position.Y += MissleSpeed;
                }

                if (position.Y < 0 || position.Y > maxHeight)
                {
                    StopMissle();
                }
                if (position.X < 0 || position.X > maxWidth)
                {
                    StopMissle();
                }
            };
        }

        public void StartMissle()
        {
            MissleImage.Image = images[position.Direction];
            movingTimer.Start();
        }

        public void SetPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public void StopMissle()
        {
            position = new Vector(-1000, -1000)
            {
                Direction = Direction.None
            };
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
