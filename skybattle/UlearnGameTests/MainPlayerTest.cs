using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using UlearnGame.Interfaces;
using UlearnGame.Models;
using UlearnGame.Utilities;
using System.Windows.Forms;

namespace UlearnGameTests
{
    [TestFixture]
    class MainPlayerTest
    {
        [TestCase(100, 100)]
        [TestCase(640, 480)]
        [TestCase(1280, 720)]
        [TestCase(10, 10)]
        public void TestMainPlayerStartPosition(int width, int height)
        {
            var form = new Form
            {
                ClientSize = new Size(width, height)
            };
            var player = new Player(form);
            Assert.AreEqual(form.ClientSize.Width / 2, player.GetPosition().X);
            Assert.AreEqual(form.ClientSize.Height / 2, player.GetPosition().Y);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(10)]
        public void CanMoveToRight(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(1280, 720)
            };
            var player = new Player(form);
            var startPosition = player.GetPosition();
            for (int i = 0; i < count; i++)
            {
                player.MoveToRight();
            }

            Assert.AreEqual(startPosition.X + player.Speed * count, player.GetPosition().X);

        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(10)]
        public void CanMoveToLeft(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(1280, 720)
            };
            var player = new Player(form);
            var startPosition = player.GetPosition();
            for (int i = 0; i < count; i++)
            {
                player.MoveToLeft();
            }

            Assert.AreEqual(startPosition.X - player.Speed * count, player.GetPosition().X);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(10)]
        public void CanMoveToTop(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(1280, 720)
            };
            var player = new Player(form);
            var startPosition = player.GetPosition();
            for (int i = 0; i < count; i++)
            {
                player.MoveToTop();
            }

            Assert.AreEqual(startPosition.Y - player.Speed * count, player.GetPosition().Y);

        }
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(10)]
        public void CanMoveToBottom(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(1280, 720)
            };
            var player = new Player(form);
            var startPosition = player.GetPosition();
            for (int i = 0; i < count; i++)
            {
                player.MoveToDown();
            }

            Assert.AreEqual(startPosition.Y + player.Speed * count, player.GetPosition().Y);

        }
        // ----------------------------------------------------
        [TestCase(6)]
        [TestCase(10)]
        public void CanNotMoveToRight(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(150, 150)
            };
            var player = new Player(form);
            for (int i = 0; i < count; i++)
            {
                player.MoveToRight();
            }

            Assert.LessOrEqual(form.ClientSize.Width - player.GetPosition().X, Player.ShipSize);

        }

        [TestCase(6)]
        [TestCase(10)]
        public void CanNotMoveToLeft(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(150, 150)
            };
            var player = new Player(form);
            for (int i = 0; i < count; i++)
            {
                player.MoveToLeft();
            }

            Assert.LessOrEqual(0 + player.GetPosition().X, Player.ShipSize);
        }

        [TestCase(6)]
        [TestCase(10)]
        public void CanNotMoveToTop(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(150, 150)
            };
            var player = new Player(form);
            for (int i = 0; i < count; i++)
            {
                player.MoveToTop();
            }

            Assert.LessOrEqual(player.GetPosition().Y, Player.ShipSize);

        }
        [TestCase(6)]
        [TestCase(10)]
        public void CanNotMoveToBottom(int count)
        {
            var form = new Form
            {
                ClientSize = new Size(150, 150)
            };
            var player = new Player(form);
            for (int i = 0; i < count; i++)
            {
                player.MoveToDown();
            }

            Assert.LessOrEqual(form.ClientSize.Width - player.GetPosition().Y, Player.ShipSize);
        }
        // ----------------------
        [TestCase(1280 / 2, 720 / 2)]
        [TestCase(1280 / 2 + LightEnemy.MissleWidth, 720 / 2)]
        [TestCase(1280 / 2, 720 / 2 + LightEnemy.MissleHeight)]
        [TestCase(1280 / 2 + LightEnemy.MissleWidth, 720 / 2 + LightEnemy.MissleHeight)]
        public void TestDead(int x, int y)
        {
            var missle = new List<IMissle>
            {
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.None, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,x, y),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.None, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,x, y)
            };
            var form = new Form
            {
                ClientSize = new Size(1280, 720)
            };
            var player = new Player(form);
            Assert.IsTrue(player.DeadInConflict(missle));

        }

        [TestCase(0, 0)]
        [TestCase(1280 / 2 + Player.ShipSize, 720 / 2)]
        [TestCase(1280 / 2, 720 / 2 + Player.ShipSize)]
        [TestCase(1280 / 2 + Player.ShipSize, 720 / 2 + Player.ShipSize)]
        public void TestNotDead(int x, int y)
        {
            var missle = new List<IMissle>
            {
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.None, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,x, y),
                new EnemyMissle(new Bitmap(LightEnemy.MissleWidth, LightEnemy.MissleHeight), Direction.None, 5,LightEnemy.MissleWidth, LightEnemy.MissleHeight, 720, 1280,x, y)

            };
            var form = new Form
            {
                ClientSize = new Size(1280, 720)
            };
            var player = new Player(form);
            Assert.IsFalse(player.DeadInConflict(missle));

        }
    }
}
