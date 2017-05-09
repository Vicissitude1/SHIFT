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
        private static DataBaseClass dataBaseInstance;
        public static DataBaseClass DataBaseInstance
        {
            get { return dataBaseInstance ?? (dataBaseInstance = new DataBaseClass()); }
        }
        //List<ListRow> activities = new List<ListRow>();
        string[,] roomNotes = new string[8, 8];

        /// <summary>
        /// private construktor
        /// </summary>
        private DataBaseClass()
        { }

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
        /*
        public List<ListRow> GetList()
        {
            if (activities.Count > 0) activities.Clear();

            try
            {
                using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
                {
                    dbConn.Open();
                    SQLiteCommand command = new SQLiteCommand("select * from schooltable", dbConn);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        activities.Add(new ListRow(reader["time"].ToString(), reader["activity"].ToString(), reader["room"].ToString()));
                    }
                }
            }
            catch (SQLiteException ex)
            {
                //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return activities;
        }*/
        /*
        public string[,] GetRoomNotes()
        {
            try
            {
                using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
                {
                    dbConn.Open();
                    //SQLiteCommand command = new SQLiteCommand("select column1, column2, column3, column4, column5, column6, column7, column8 from roomnotestable where id =" + y, dbConn);

                    for (int y = 0; y < roomNotes.GetLength(1); y++)
                    {
                        for (int x = 0; x < roomNotes.GetLength(0); x++)
                        {
                            SQLiteCommand command = new SQLiteCommand("select column" + (x + 1).ToString() + " from roomnotestable where id=" + (y + 1).ToString(), dbConn);
                            SQLiteDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                roomNotes[x, y] = reader["column" + (x + 1).ToString()].ToString();
                            }
                            if (roomNotes[x, y] == null) roomNotes[x, y] = "";
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return roomNotes;
        }*/
        /*
        public string[,] ReadRoomNotes()
        {
            try
            {
                using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
                {
                    dbConn.Open();
                    for (int y = 0; y < roomNotes.GetLength(1); y++)
                    {
                        SQLiteCommand command = new SQLiteCommand("select column1, column2, column3, column4, column5, column6, column7, column8 from roomnotestable where id =" + (y + 1).ToString(), dbConn);
                        SQLiteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            for (int x = 0; x < roomNotes.GetLength(0); x++)
                            {
                                roomNotes[x, y] = reader["column" + (x + 1).ToString()].ToString();
                                if (roomNotes[x, y] == null) roomNotes[x, y] = "";
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return roomNotes;
        }

        public void SaveRoomNotes(string[,] roomNotes)
        {
            try
            {
                using (SQLiteConnection dbConn = new SQLiteConnection("Data Source = data.db; Version = 3"))
                {
                    dbConn.Open();
                    for (int y = 0; y < roomNotes.GetLength(1); y++)
                    {
                        for (int x = 0; x < roomNotes.GetLength(0); x++)
                        {
                            SQLiteCommand command = new SQLiteCommand("update roomnotestable set column" + (x + 1).ToString() + " ='" + roomNotes[x, y] + "' where id=" + (y + 1).ToString(), dbConn);
                            command.ExecuteReader();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
    }
}
