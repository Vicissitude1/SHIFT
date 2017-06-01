using System;
using ShootingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ShootingGame.Interfaces;

namespace HighNLowTest
{
    [TestClass]
    public class TestHighNLow
    {
        GameObject go;
        Vector2 position;
        Dice d;
        DiceControl dc;
        

        [TestInitialize]
        public void Initialize()
        {
            position = new Vector2(0, 0);
            go = new GameObject(position);
            d = new Dice(go);
            dc = new DiceControl(DiceControl.Dies);
            DiceControl.Dies = new List<IDice>();
        }

        [TestMethod]
        public void TestRollOneToSix()
        {
            
            int a = d.Roll();
            bool result = a >= 1 && a <= 6;
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void TestThreeDies()
        {
            int a = d.Roll();
            int b = d.Roll();
            int c = d.Roll();
            bool result = a + b + c >= 3 && a + b + c <= 18;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIfHigher()
        {
            TestDice t1 = new TestDice(2);
            TestDice t2 = new TestDice(4);
            TestDice t3 = new TestDice(6);
            TestDice t4 = new TestDice(1);
            TestDice t5 = new TestDice(3);
            TestDice t6 = new TestDice(5);
            int a = t1.Roll();
            int b = t2.Roll();
            int c = t3.Roll();
            int d = t4.Roll();
            int e = t5.Roll();
            int f = t6.Roll();
            bool result = a + b + c > d + e + f;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIfLower()
        {
            TestDice t1 = new TestDice(1);
            TestDice t2 = new TestDice(3);
            TestDice t3 = new TestDice(5);
            TestDice t4 = new TestDice(2);
            TestDice t5 = new TestDice(4);
            TestDice t6 = new TestDice(6);
            int a = t1.Roll();
            int b = t2.Roll();
            int c = t3.Roll();
            int d = t4.Roll();
            int e = t5.Roll();
            int f = t6.Roll();
            bool result = a + b + c < d + e + f;
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void TestHighIsCorrect()
        {
            TestDice td1 = new TestDice(6);
            DiceControl.Dies.Add(td1);
            TestDice td2 = new TestDice(6);
            DiceControl.Dies.Add(td2);
            TestDice td3 = new TestDice(6);
            DiceControl.Dies.Add(td3);
            dc.Reserve = 0;
            DiceControl.Result = 12;
            Player.CurrentWeapon = new Weapon("GUN", 7, 20, 1000, WeaponType.BoltAction);
            dc.High();
            int result = Player.CurrentWeapon.TotalAmmo;
            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void TestHighIsIncorrect()
        {
            TestDice td1 = new TestDice(1);
            DiceControl.Dies.Add(td1);
            TestDice td2 = new TestDice(1);
            DiceControl.Dies.Add(td2);
            TestDice td3 = new TestDice(1);
            DiceControl.Dies.Add(td3);
            dc.Reserve = 0;
            DiceControl.Result = 12;
            Player.CurrentWeapon = new Weapon("GUN", 7, 20, 1000, WeaponType.BoltAction);
            dc.High();
            int result = dc.Reserve;
            Assert.AreEqual(12, result);
        }

        public void TestLowIsCorrect()
        {
            TestDice td1 = new TestDice(1);
            DiceControl.Dies.Add(td1);
            TestDice td2 = new TestDice(1);
            DiceControl.Dies.Add(td2);
            TestDice td3 = new TestDice(1);
            DiceControl.Dies.Add(td3);
            dc.Reserve = 0;
            DiceControl.Result = 12;
            Player.CurrentWeapon = new Weapon("GUN", 7, 20, 1000, WeaponType.BoltAction);
            dc.Low();
            int result = Player.CurrentWeapon.TotalAmmo;
            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void TestLowIsIncorrect()
        {
            TestDice td1 = new TestDice(6);
            DiceControl.Dies.Add(td1);
            TestDice td2 = new TestDice(6);
            DiceControl.Dies.Add(td2);
            TestDice td3 = new TestDice(6);
            DiceControl.Dies.Add(td3);
            dc.Reserve = 0;
            DiceControl.Result = 12;
            Player.CurrentWeapon = new Weapon("GUN", 7, 20, 1000, WeaponType.BoltAction);
            dc.Low();
            int result = dc.Reserve;
            Assert.AreEqual(12, result);
        }
        
    }
}
