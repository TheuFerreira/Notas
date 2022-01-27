using Notas.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Notas.Database.Repositories
{
    public class DbRepository : IDbRepository
    {
        public string ConnectionString
        {
            get
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ferreira\Notas\";
                Directory.CreateDirectory(folder);

                string file = folder + "notas.db";
                if (!File.Exists(file))
                {
                    string temp = Directory.GetCurrentDirectory() + @"\notas.db";
                    File.Copy(temp, file, true);
                }

                string path = $"DataSource={file}";
                return path;
            }
        }

        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteConnection conn = new SQLiteConnection(ConnectionString);
            conn.Open();

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.AddRange(parameters);

            int result = command.ExecuteNonQuery();
            conn.Close();

            return result;
        }

        public List<List<object>> ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteConnection conn = new SQLiteConnection(ConnectionString);
            conn.Open();

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.AddRange(parameters);

            List<List<object>> result = new List<List<object>>();
            SQLiteDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                List<object> list = new List<object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.IsDBNull(i))
                        list.Add(null);
                    else
                        list.Add(reader[i]);
                }

                result.Add(list);
            }

            conn.Close();

            return result;
        }
    }
}
