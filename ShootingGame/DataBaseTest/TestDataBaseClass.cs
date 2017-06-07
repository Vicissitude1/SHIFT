using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootingGame;
using System.Collections.Generic;

namespace DataBaseTest
{
    [TestClass]
    public class TestDataBaseClass
    {
        [TestMethod]
        public void TestMethodCreatTables()
        {
            IDataBaseClass dbClass = new MockDataBaseClass();
            dbClass.TableIsCreated = false;
            dbClass.CreateTables();
            bool result = dbClass.TableIsCreated;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethodGetBonusValueCorrect()
        {
            IDataBaseClass dbClass = new MockDataBaseClass();
            int result = dbClass.GetBonusValue("player", 1);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void TestMethodGetBonusValueNotCorrect()
        {
            IDataBaseClass dbClass = new MockDataBaseClass();
            int result = dbClass.GetBonusValue("player", 2);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethodSaveAndGetScoeList()
        {
            IDataBaseClass dbClass = new MockDataBaseClass();
            List<PlayerListRow> players = new List<PlayerListRow>() { new PlayerListRow("player1", 100), new PlayerListRow("player2", 200) };
            dbClass.SavePlayersList(players);
            List<PlayerListRow> newPlayers = dbClass.GetPlayersList();
            string resultName1 = newPlayers[0].Name;
            int resultScore1 = newPlayers[0].Score;
            string resultName2 = newPlayers[1].Name;
            int resultScore2 = newPlayers[1].Score;
            Assert.IsNotNull(newPlayers);
            Assert.AreEqual("player1", resultName1);
            Assert.AreEqual(100, resultScore1);
            Assert.AreEqual("player2", resultName2);
            Assert.AreEqual(200, resultScore2);
        }

        [TestMethod]
        public void TestMethodGetWeapons()
        {
            IDataBaseClass dbClass = new MockDataBaseClass();
            Weapon[] weapons = dbClass.GetWeapons();
            string resultName1 = weapons[0].Name;
            string resultName2 = weapons[1].Name;
            string resultName3 = weapons[2].Name;
            Assert.IsNotNull(weapons);
            Assert.AreEqual("GUN", resultName1);
            Assert.AreEqual("RIFLE", resultName2);
            Assert.AreEqual("MACHINEGUN", resultName3);
        }
    }
}
