using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;
using UlearnGame.Interfaces;
using UlearnGame.Models;
using UlearnGame.Utilities;

namespace UlearnGameTests
{
    [TestFixture]
    class MissleTest
    {

        [TestCase(10, 10)]
        [TestCase(10, 20)]
        [TestCase(10, 30)]
        public void StartMissleToTopTest(int startX, int startY)
        {
            var missles = new List<IMissle>
            {
                new PlayerMissle(new Bitmap(PlayerMissle.Width, PlayerMissle.Height), Direction.Top, 5,1203,123, startX, startY),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.Down, 5, LightEnemy.MissleWidth, LightEnemy.MissleHeight,30,123,startX, startY)
            };
            var timer = new Timer
            {
                Interval = 30
            };

            foreach (var missle in missles)
            {
                missle.StartMissle();
            }
            timer.Tick += (sender, args) =>
            {
                foreach (var missle in missles)
                {
                    Assert.AreEqual(new Vector(startX, startY - missle.MissleSpeed), missle.GetPosition());
                }
            };
            timer.Start();

        }
        [TestCase(10, 10)]
        [TestCase(10, 20)]
        [TestCase(10, 30)]
        public void StartMissleToDownTest(int startX, int startY)
        {
            var missles = new List<IMissle>
            {
                new PlayerMissle(new Bitmap(PlayerMissle.Width, PlayerMissle.Height), Direction.Down, 5,1203,123, startX, startY),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.Down, 5, LightEnemy.MissleWidth, LightEnemy.MissleHeight,30,123,startX, startY),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.Down, 5, LightEnemy.MissleWidth, LightEnemy.MissleHeight,30,123,startX, startY)
            };
            var timer = new Timer
            {
                Interval = 30
            };

            foreach (var missle in missles)
            {
                missle.StartMissle();
            }
            timer.Tick += (sender, args) =>
            {
                foreach (var missle in missles)
                {
                    Assert.AreEqual(new Vector(startX, startY + missle.MissleSpeed), missle.GetPosition());
                }
            };
            timer.Start();

        }

        [TestCase(10, 0)]
        [TestCase(10, 10)]
        [TestCase(10, 30)]
        public void StopMissleTest(int startX, int startY)
        {
            var missles = new List<IMissle>
            {
                new PlayerMissle(new Bitmap(PlayerMissle.Width,PlayerMissle.Height),Direction.Top,5,1203,123, startX,startY),
                new PlayerMissle(new Bitmap(PlayerMissle.Width,PlayerMissle.Height),Direction.Down,5,1203,123, startX,startY),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.Top, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,startX, startY),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.Down, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,startX, startY)

            };
            var timer = new Timer
            {
                Interval = 15
            };

            foreach (var missle in missles)
            {
                missle.StartMissle();
            }
            timer.Tick += (sender, args) =>
            {

                foreach (var missle in missles)
                {
                    Assert.AreEqual(new Vector(-1000, -1000, missle.Direction), missle.GetPosition());
                }

            };
            timer.Start();

        }

        [TestCase(1, 1)]
        [TestCase(123, 123)]
        [TestCase(123, 32)]
        [TestCase(1, 32)]
        public void SetPositionTest(int x, int y)
        {
            var missles = new List<IMissle>
            {
                new PlayerMissle(new Bitmap(PlayerMissle.Width,PlayerMissle.Height),Direction.None,5,1203,1233 ,0,0),
                new PlayerMissle(new Bitmap(PlayerMissle.Width,PlayerMissle.Height),Direction.None,5,1203,123, 0,0),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.None, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,0, 0),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.None, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,0, 0)

            };

            foreach (var missle in missles)
            {
                missle.SetPosition(x, y);
                Assert.AreEqual(new Vector(x, y), missle.GetPosition());
            }

        }
    }
}
