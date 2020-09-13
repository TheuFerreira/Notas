using System;
using System.Data.SQLite;

namespace Notas.Database
{
    public class Connection
    {
        public static SQLiteConnection SQLiteConnection;

        public static void Connect()
        {
            try
            {
                string path = "DataSource=notas.db";
                SQLiteConnection = new SQLiteConnection(path);
                SQLiteConnection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Disconnect()
        {
            try
            {
                SQLiteConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
