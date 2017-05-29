using System;
using ShootingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace HighNLowTest
{
    [TestClass]
    public class TestHighNLow
    {
        [TestMethod]
        public void TestRollOneToSix()
        {
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
            Vector2 position = new Vector2(0, 0);
            GameObject go = new GameObject(position);
            Dice d = new Dice(go);
            int a = GameWorld.Instance.RollDices();
            int b = GameWorld.Instance.RollDices();
            int c = GameWorld.Instance.RollDices();
            bool result = a + b + c >= 3 && a + b + c <= 18;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIfHigher()
        {
            TestDice t = new TestDice();
            int a = t.Roll(2);
            int b = t.Roll(4);
            int c = t.Roll(6);
            int d = t.Roll(1);
            int e = t.Roll(3);
            int f = t.Roll(5);
            bool result = a + b + c > d + e + f;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIfLower()
        {
            TestDice t = new TestDice();
            int a = t.Roll(1);
            int b = t.Roll(3);
            int c = t.Roll(5);
            int d = t.Roll(2);
            int e = t.Roll(4);
            int f = t.Roll(6);
            bool result = a + b + c < d + e + f;
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void TestPlayerInputHigh()
        {
            TestPlayer f = new TestPlayer();
            string input = f.PlayerInput("h");
            Assert.AreEqual("h", input);

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
            TestDice t = new TestDice();
            int a = t.Roll(5);
            int b = t.Roll(5);
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
