using System;
using System.Data.SQLite;
using System.IO;

namespace Notas.Database
{
    public class Connection
    {
        public static SQLiteConnection SQLiteConnection;

        public static string ConnectionString
        {
            get
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ferreira\Notas\";
                Directory.CreateDirectory(folder);

                string file = folder + "notas.db";
                if (File.Exists(file) == false)
                {
                    string temp = Directory.GetCurrentDirectory() + @"\notas.db";
                    File.Copy(temp, file, true);
                }

                string path = $"DataSource={file}";
                return path;
            }
        }

        public static void Connect()
        {
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ferreira\Notas\";
                Directory.CreateDirectory(folder);

                string file = folder + "notas.db";
                if (File.Exists(file) == false)
                {
                    string temp = Directory.GetCurrentDirectory() + @"\notas.db";
                    File.Copy(temp, file, true);
                }

                string path = $"DataSource={file}";
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
