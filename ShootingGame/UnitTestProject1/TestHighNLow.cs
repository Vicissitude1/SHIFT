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
        [TestMethod]
        public void TestRollOneToSix()
        {
            GameWorld.Instance.IsTesting = true;
            Vector2 position = new Vector2(0,0);
            GameObject go = new GameObject(position);
            Dice d = new Dice(go);
            int a = d.Roll();
            bool result = a >= 1 && a <= 6;
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void TestThreeDies()
        {
            GameWorld.Instance.IsTesting = true;
            Vector2 position = new Vector2(0, 0);
            GameObject go = new GameObject(position);
            Dice d = new Dice(go);
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
            bool result = a + b + c < d + e + f;
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void TestHighIsCorrect()
        {
            GameWorld.Instance.Dies = new List<IDice>() { new TestDice(2) };
            TestDice td1 = new TestDice(6);
            GameWorld.Instance.Dies.Add(td1);
            TestDice td2 = new TestDice(6);
            GameWorld.Instance.Dies.Add(td2);
            //Vector2 position = new Vector2(0, 0);
            //GameObject go = new GameObject(position);
            //Player p = new Player(go);
            Player.CurrentWeapon = new Weapon("GUN", 7, 20, 1000, WeaponType.BoltAction);
            GameWorld.Instance.High();
            int result = Player.CurrentWeapon.TotalAmmo;
            Assert.AreEqual(12, result);
        }
        [TestMethod]
        public void TestPlayerInputLow()
        {
            TestPlayer f = new TestPlayer();
            string input = f.PlayerInput("l");
            Assert.AreEqual("l", input);
        }
        [TestMethod]
        public void TestPlayerInputSame()
        {
            TestDice t = new TestDice(3);
            int a = t.Roll();
            int b = t.Roll();
            bool result = a == b;
            Assert.IsTrue(result);
        }
        //[TestMethod]
        //public void TestPlayerIsRightHigh()
        //{
        //    Vector2 position = new Vector2(0, 0);
        //    GameObject go = new GameObject(position);
        //    Player t = new Player(go);
        //    bool result = t.IsRight(2, "h");
        //    Assert.IsTrue(result);
        //}
        //[TestMethod]
        //public void TestPlayerIsRightLow()
        //{
        //    Vector2 position = new Vector2(0, 0);
        //    GameObject go = new GameObject(position);
        //    Player t = new Player(go);
        //    bool result = t.IsRight(2, "l");
        //    Assert.IsTrue(result);
        //}
        //[TestMethod]
        //public void TestPlayerIsWrongHigh()
        //{
        //    Vector2 position = new Vector2(0, 0);
        //    GameObject go = new GameObject(position);
        //    Player t = new Player(go);
        //    bool result = t.IsRight(2, "h");
        //    Assert.IsFalse(result);
        //}
        //[TestMethod]
        //public void TestPlayerIsWrongLow()
        //{
        //    Vector2 position = new Vector2(0, 0);
        //    GameObject go = new GameObject(position);
        //    Player t = new Player(go);
        //    bool result = t.IsRight(2, "l");
        //    Assert.IsFalse(result);
        //}
    }
}
