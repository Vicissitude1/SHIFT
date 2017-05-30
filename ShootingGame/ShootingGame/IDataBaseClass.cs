using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public interface IDataBaseClass
    {
        bool TableIsCreated { get; set; }

        void CreateTables();
        List<PlayerListRow> GetPlayersList();
        void SavePlayersList(List<PlayerListRow> players);
        Weapon[] GetWeapons();
        int GetBonusValue(string name, int id);
    }
}
