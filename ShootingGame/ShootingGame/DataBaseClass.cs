using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace ShootingGame
{
    /// <summary>
    /// Represents the DataBaseClass
    /// </summary>
    class DataBaseClass : IDataBaseClass
    {
        /// <summary>
        /// The singletone pattern
        /// </summary>
        static DataBaseClass instance;

        public static DataBaseClass Instance
        {
            get { return instance ?? (instance = new DataBaseClass()); }
        }

        public bool TableIsCreated { get; set; }

        /// <summary>
        /// The DataBaseClass's constructor
        /// </summary>
        private DataBaseClass()
        { }

        /// <summary>
        /// Creates database file with tables, if it doesn't exist
        /// </summary>
        public void CreateTables()
        {
            if (!File.Exists("data.db"))
            {
                SQLiteConnection.CreateFile("data.db");
                using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
                {
                    dbConn.Open();
                    SQLiteCommand command = new SQLiteCommand("create table scoretable (id integer primary key, name string, score int)", dbConn);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("create table weapons (id integer primary key, name string, maxammo int, damagelevel int, reloadtime int, weapontype string)", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into weapons (id, name, maxammo, damagelevel, reloadtime, weapontype) values (null, 'GUN', 7, 20, 1000, 'BoltAction')", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into weapons (id, name, maxammo, damagelevel, reloadtime, weapontype) values (null, 'RIFLE', 15, 50, 1500, 'SemiAuto')", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into weapons (id, name, maxammo, damagelevel, reloadtime, weapontype) values (null, 'MACHINEGUN', 30, 35, 1500, 'FullAuto')", dbConn);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("create table powerups (id integer primary key, health int, score int, ammo int)", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into powerups (id, health, score, ammo) values (null, 3, 5, 2)", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into powerups (id, health, score, ammo) values (null, 6, 10, 4)", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into powerups (id, health, score, ammo) values (null, 9, 15, 8)", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into powerups (id, health, score, ammo) values (null, 12, 20, 16)", dbConn);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand("insert into powerups (id, health, score, ammo) values (null, 15, 25, 32)", dbConn);
                    command.ExecuteNonQuery();
                }
            }
           /*
            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();
                SQLiteCommand command = new SQLiteCommand("create table scoretable (id integer primary key, name string, score int)", dbConn);
                command.ExecuteNonQuery();
            }
            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();
                SQLiteCommand command = new SQLiteCommand("insert into scoretable (id, name, score) values (null, 'Alex', 200)", dbConn);
                command.ExecuteNonQuery();
            }*/
        }
        
        /// <summary>
        /// Returns the score list from database
        /// </summary>
        /// <returns></returns>
        public List<PlayerListRow> GetPlayersList()
        {
            List<PlayerListRow> players = new List<PlayerListRow>();

            if (players.Count > 0) players.Clear();

            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();
                SQLiteCommand command = new SQLiteCommand("select name,score from scoretable order by score desc", dbConn);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    players.Add(new PlayerListRow(reader["name"].ToString(), (int)reader["score"]));
                }
            }
            return players;
        }

        /// <summary>
        /// Saves the score list to data base
        /// </summary>
        /// <param name="players"></param>
        public void SavePlayersList(List<PlayerListRow> players)
        {
            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();

                SQLiteCommand command = new SQLiteCommand("delete from scoretable", dbConn);
                command.ExecuteReader();

                for (int i = 0; i < players.Count; i++)
                {
                    command = new SQLiteCommand("insert into scoretable (id, name, score) values (null, '" + players[i].Name + "', " + players[i].Score.ToString() + ")", dbConn);
                    command.ExecuteReader();
                }
            }
        }
        /*
        public void ClearPlayersList()
        {
            try
            {
                using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
                {
                    dbConn.Open();
                    SQLiteCommand command = new SQLiteCommand("delete from scoretable", dbConn);
                    command.ExecuteReader();
                }
            }
            catch (SQLiteException ex)
            { }
        }*/

        /// <summary>
        /// Returns the weapon list from data base
        /// </summary>
        /// <returns></returns>
        public Weapon[] GetWeapons()
        {
            Weapon[] weapons = new Weapon[3];

            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();
                for (int i = 0; i < weapons.Length; i++)
                {
                    SQLiteCommand command = new SQLiteCommand("select name, maxammo, damagelevel, reloadtime, weapontype from weapons where id=" + (i + 1).ToString(), dbConn);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        weapons[i] = new Weapon(reader["name"].ToString(), (int)reader["maxammo"], (int)reader["damagelevel"], (int)reader["reloadtime"], (WeaponType)Enum.Parse(typeof(WeaponType), reader["weapontype"].ToString()));
                    }
                }
            }
            return weapons;
        }

        /// <summary>
        /// Returns the bonus value from power ups list from data base corresponding to the current type of the power up
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetBonusValue(string name, int id)
        {
            int bonusValue = 0;

            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();
                SQLiteCommand command = new SQLiteCommand("select " + name + " from powerups where id=" + id.ToString(), dbConn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    bonusValue = ((int)reader[name]);
            }
            return bonusValue;
        }
    }
}
