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
    class EnemyTest
    {
        [TestCase(10, 10)]
        [TestCase(640, 10)]
        [TestCase(640, 230)]
        [TestCase(500, 100)]
        [TestCase(640 / 2, 480 / 2)]
        public void MoveToPointTest(int x, int y)
        {
            var form = new Form { ClientSize = new Size(640, 480) };
            var enemy = new LightEnemy(form, 1, 2, 15, 10);
            var position = new Vector(x, y);

            for (int i = 0; i < 640 * 480; i++)
            {
                enemy.MoveToPoint(position);
            }

            Assert.IsTrue(enemy.GetPosition().Distance(position) <= 120);
        }

        [Test]
        public void MoveFromPointTest()
        {
            var form = new Form { ClientSize = new Size(640, 480) };
            var enemy = new LightEnemy(form, 1, 2, 15, 10);

            for (int i = 0; i < 10; i++)
            {
                var enemyPosition = enemy.GetPosition();
                enemy.MoveFromPoint(enemyPosition);
                Assert.IsTrue(enemyPosition != enemy.GetPosition());
            }
        }

        [Test]
        public void DeadInConflictTest()
        {
            var form = new Form { ClientSize = new Size(640, 480) };
            var enemy = new LightEnemy(form, 1, 2, 15, 10);
            var enemyPosition = enemy.GetPosition();

            var missles = new List<IMissle>();

            for (int i = 0; i < 2; i++)
            {
                missles.Add(new PlayerMissle(new Bitmap(PlayerMissle.Width, PlayerMissle.Height), Direction.Top, 5, 480, 640, enemyPosition.X, enemyPosition.Y));
            }

            Assert.IsTrue(enemy.OnMissleConflict(missles));
        }   

        [Test]
        public void NotDeadInConflictTest()
        {
            var form = new Form { ClientSize = new Size(640, 480) };
            var enemy = new LightEnemy(form, 1, 2, 15, 10);
            var enemyPosition = enemy.GetPosition();

            var missles = new List<IMissle>();

            for (int i = 0; i < 2; i++)
            {
                missles.Add(new PlayerMissle(new Bitmap(PlayerMissle.Width, PlayerMissle.Height), Direction.Top, 5, 480, 640, enemyPosition.X + 100, enemyPosition.Y+100));
            }

            Assert.IsFalse(enemy.OnMissleConflict(missles));
        }

    }
}