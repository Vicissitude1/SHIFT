using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootingGame;
using Microsoft.Xna.Framework;

namespace WeaponTest
{
    [TestClass]
    public class UnitTestWeapon
    {
        [TestMethod]
        public void TestMethod1()
        {
            Weapon w = new Weapon("GUN", 10, 30, 1000, WeaponType.BoltAction);
            w.TotalAmmo = 99;
            Assert.AreEqual(99, w.TotalAmmo);
        }
    }
}
