using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootingGame;
using Microsoft.Xna.Framework;

namespace WeaponTest
{
    [TestClass]
    public class UnitTestWeapon
    {
        // Checks property for totalAmmo in Weapon class
        [TestMethod]
        public void TestMethodTotalAmmoLess()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.TotalAmmo = 99;
            int result = w.TotalAmmo;
            Assert.AreEqual(99, result);
        }
        [TestMethod]
        public void TestMethodTotalAmmo()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.TotalAmmo = 100;
            int result = w.TotalAmmo;
            Assert.AreEqual(100, result);
        }
        [TestMethod]
        public void TestMethodTotalAmmoOver()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.TotalAmmo = 101;
            int result = w.TotalAmmo;
            Assert.AreEqual(100, result);
        }

        // Checks reloading process in the method Reload()
        [TestMethod]
        public void TestMethodCurrentReloadTimeMinus20()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 40;
            w.CanPlayGunCockingSound = false;
            w.Reload();
            int result = w.CurrentReloadTime;
            Assert.AreEqual(20, result);
        }
  
        [TestMethod]
        public void TestMethodCurrentReloadTimeMoreThenZero()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 1;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 6;
            w.Reload();
            int result = w.Ammo;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethodAmmoWhenTotalAmmoLessThenMaxAmmo()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 6;
            w.Reload();
            int result = w.Ammo;
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void TestMethodTotalAmmoMoreThenMaxAmmo()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 8;
            w.Reload();
            int result = w.Ammo;
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void TestMethodTotalAmmoWhenTotalAmmoLessThenMaxAmmo()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 6;
            w.Reload();
            int result = w.TotalAmmo;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethodTotalAmmoWhenTotalAmmoLikeMaxAmmo()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 7;
            w.Reload();
            int result = w.TotalAmmo;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethodTotalAmmoWhenTotalAmmoMoreThenMaxAmmo()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 8;
            w.Reload();
            int result = w.TotalAmmo;
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestMethodSetupCurrentReloadTime()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Reload();
            int result = w.CurrentReloadTime;
            Assert.AreEqual(500, result);
        }

        [TestMethod]
        public void TestMethodSetupIsReloadingToFalse()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Reload();
            bool result = w.IsReloading;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMethodSetupCanPlayGunCockingSoundToTrue()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 0;
            w.CanPlayGunCockingSound = false;
            w.Reload();
            bool result = w.CanPlayGunCockingSound;
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void TestMethodUpdateWeaponStatusCheckCanShoot()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.IsReloading = false;
            w.CanShoot = true;
            w.UpdateWeaponStatus();
            bool result = w.CanShoot;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMethodUpdateWeaponStatusCheckIsReloading()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.IsReloading = false;
            w.CanShoot = true;
            w.UpdateWeaponStatus();
            bool result = w.IsReloading;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethodStartReloading()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 20;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 1;
            w.IsReloading = true;
            w.UpdateWeaponStatus();
            int result = w.CurrentReloadTime;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethodStartReloadingIfTotalAmmoIs0()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.CurrentReloadTime = 20;
            w.CanPlayGunCockingSound = false;
            w.Ammo = 0;
            w.TotalAmmo = 0;
            w.IsReloading = true;
            w.UpdateWeaponStatus();
            int result = w.CurrentReloadTime;
            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void TestMethodRestartWeapon()
        {
            Weapon w = new Weapon("GUN", 7, 25, 500, WeaponType.BoltAction);
            w.TotalAmmo = 1;
            w.Ammo = 0;
            w.CurrentReloadTime = 0;
            w.IsReloading = true;
            w.CanShoot = true;
            w.CanPlayGunCockingSound = false;
            w.RestartWeapon();
            int totalAmmo = w.TotalAmmo;
            int ammo = w.Ammo;
            int currentReloadTime = w.CurrentReloadTime;
            bool isReloading = w.IsReloading;
            bool canShoot = w.CanShoot;
            bool CanPlaySound = w.CanPlayGunCockingSound;
            Assert.AreEqual(0, totalAmmo);
            Assert.AreEqual(7, ammo);
            Assert.AreEqual(500, currentReloadTime);
            Assert.IsFalse(canShoot);
            Assert.IsTrue(CanPlaySound);
        }
    }
}
