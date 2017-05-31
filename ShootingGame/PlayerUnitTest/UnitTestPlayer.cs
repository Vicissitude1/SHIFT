using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootingGame;
using Microsoft.Xna.Framework;

namespace PlayerUnitTest
{
    [TestClass]
    public class UnitTestPlayer
    {
        // Checks the property for Player's health
        [TestMethod]
        public void TestMethodHealthLessThan0()
        {
            Player.Health = -1;
            int result = Player.Health;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethodHealth0()
        {
            Player.Health = 0;
            int result = Player.Health;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethodHealthMoreThan0()
        {
            Player.Health = 1;
            int result = Player.Health;
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestMethodHealthLessThan100()
        {
            Player.Health = 99;
            int result = Player.Health;
            Assert.AreEqual(99, result);
        }

        [TestMethod]
        public void TestMethodHealth100()
        {
            Player.Health = 100;
            int result = Player.Health;
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void TestMethodHealthMoreThan100()
        {
            Player.Health = 101;
            int result = Player.Health;
            Assert.AreEqual(100, result);
        }
    }

}
