using Notas.Database.Table;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Notas.Database.Persistence
{
    public class PersistencePostIt : Connection
    {
        public static void Add(PostIt postIt)
        {
            Connect();

            try
            {
                Connect();

                string str = "INSERT INTO postit(content) VALUES (@content);";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@content", postIt.Content);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public static bool Delete(PostIt postIt)
        {
            Connect();
            
            try
            {
                string str = "DELETE FROM postit WHERE id = @id";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@id", postIt.Id);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public static void Update(PostIt postIt)
        {
            Connect();

            try
            {
                string str = "UPDATE postit SET content=@content WHERE id=@id;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@id", postIt.Id);
                command.Parameters.AddWithValue("@content", postIt.Content);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }

        public static List<PostIt> GetAll()
        {
            Connect();
            List<PostIt> postIts = new List<PostIt>();

            try
            {
                string str = "SELECT id, content FROM postit;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    long id = dr.GetInt64(0);
                    string content = dr.GetString(1);
                    
                    PostIt temp = new PostIt(id, content);
                    postIts.Add(temp);
                }
                dr.Close();

                return postIts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
