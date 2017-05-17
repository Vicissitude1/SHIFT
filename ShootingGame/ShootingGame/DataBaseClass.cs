using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ShootingGame
{
    class DataBaseClass
    {
        /// <summary>
        /// Creates database file and table
        /// </summary>
        public void CreateTable()
        {
            //SQLiteConnection.CreateFile("data.db");
            /*
            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();
                SQLiteCommand command = new SQLiteCommand("create table scoretable (id integer primary key, name string, score int)", dbConn);
                command.ExecuteNonQuery();
            }*/
            /*
            using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
            {
                dbConn.Open();
                SQLiteCommand command = new SQLiteCommand("insert into scoretable (id, name, score) values (null, 'Alex', 200)", dbConn);
                command.ExecuteNonQuery();
            }*/
        }
        
        public List<PlayerListRow> GetPlayersList()
        {
            List<PlayerListRow> players = new List<PlayerListRow>();
            if (players.Count > 0) players.Clear();
            try
            {
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
            }
            catch (SQLiteException ex)
            { }

            return players;
        }

        public void SavePlayersList(List<PlayerListRow> players)
        {
            try
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
            catch (SQLiteException ex)
            {  }
        }

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
        }
    }
}
