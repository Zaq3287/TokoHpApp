using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace eNota
{
    public class Database
    {
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // fields
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        SQLiteConnection database;


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // properties
        // -------------------------------------------------------------
        // -------------------------------------------------------------



        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public Database()
        {
            database = GetConnection();
            database.CreateTable<tbl_settings>();
            database.CreateTable<tbl_nota>();
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public SQLiteConnection GetConnection()
        {
            SQLiteConnection sqliteConnection;
            var sqliteFileName = "eNota.db3";
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Path.Combine(folder, sqliteFileName);
            sqliteConnection = new SQLiteConnection(path);
            return sqliteConnection;
        }

        public void reConnectDB()
        {
            database = GetConnection();
        }

        public void closeDB()
        {
            database.Close();
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // settings
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public tbl_settings getSettings()
        {
            return database.Query<tbl_settings>("SELECT * FROM tbl_settings").FirstOrDefault();
        }

        public int insertTable(tbl_settings tbl_Settings)
        {
            return database.Insert(tbl_Settings);
        }

        public void updateDevice(string strDevice)
        {
            var update = database.Query<tbl_settings>("UPDATE tbl_settings SET strDevice = '" + strDevice + "'").FirstOrDefault();

            database.Update(update);
        }

        public void updateTable(tbl_settings tbl_settings)
        {
            database.Update(tbl_settings);
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // nota
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public IEnumerable<tbl_nota> getNota()
        {
            return database.Query<tbl_nota>("SELECT * FROM tbl_nota ORDER BY dtOrder DESC");
        }

        public IEnumerable<tbl_nota> getNotaSold(DateTime dtStart, DateTime dtEnd)
        {
            dtEnd = dtEnd.AddDays(1);
            return database.Table<tbl_nota>().OrderByDescending(x => x.dtOrder).Where(x => x.dtOrder >= dtStart && x.dtOrder < dtEnd && x.strStatus == "Sold").ToList();
        }

        public IEnumerable<tbl_nota> getNotaStock()
        {
            return database.Table<tbl_nota>().OrderByDescending(x => x.dtOrder).Where(x => x.strStatus == "Stock").ToList();
        }

        public IEnumerable<tbl_nota> getNotaAll()
        {
            return database.Table<tbl_nota>().OrderByDescending(x => x.dtOrder).ToList();
        }

        public tbl_nota getNota(int intID)
        {
            return database.Query<tbl_nota>("SELECT * FROM tbl_nota WHERE intID = " + intID ).FirstOrDefault();
        }

        public int insertTable(tbl_nota tbl_nota)
        {
            return database.Insert(tbl_nota);
        }

        public void updateTable(tbl_nota tbl_nota)
        {
            database.Update(tbl_nota);
        }

        public void updateNotaStatus()
        {
            var update = database.Query<tbl_nota>("UPDATE tbl_nota SET strStatus = 'Sold'").FirstOrDefault();

            database.Update(update);
        }

        public void deleteTableNota(int intID)
        {
            database.Table<tbl_nota>().Delete(x => x.intID == intID);
        }

        public bool recordExists(string tableName)
        {
            return database.ExecuteScalar<int>("SELECT COUNT(*) FROM " + tableName) > 0;
        }

        public bool recordExists(string tableName, string strWhere)
        {
            return database.ExecuteScalar<int>("SELECT COUNT(*) FROM " + tableName + " WHERE " + strWhere) > 0;
        }
    }
}
