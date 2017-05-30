using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class MockDataBaseClass :IDataBaseClass
    {
        public bool TableIsCreated { get; set; }

        public List<PlayerListRow> players { get; set; } = new List<PlayerListRow>();

        
        public void CreateTables()
        {
            if(!TableIsCreated)
            TableIsCreated = true;
        }
        
        public int GetBonusValue(string name, int id)
        {
            if (name == "player" && id == 1) return 5;
            else return 0;
        }

        public void SavePlayersList(List<PlayerListRow> players)
        {
            this.players.Clear();
            this.players.AddRange(players);
        }

        public List<PlayerListRow> GetPlayersList()
        {
            return players;
        }

        public Weapon[] GetWeapons()
        {
            Weapon[] weapons = new Weapon[] { new Weapon("GUN", 7, 20, 1000, WeaponType.BoltAction),
                                              new Weapon("RIFLE", 15, 50, 1500, WeaponType.SemiAuto),
                                              new Weapon("MACHINEGUN", 30, 35, 1500, WeaponType.FullAuto)};
            return weapons;
        }

    }
}
