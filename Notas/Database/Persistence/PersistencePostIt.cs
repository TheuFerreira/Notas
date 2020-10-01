using Notas.Database.Table;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Media;

namespace Notas.Database.Persistence
{
    public class PersistencePostIt : Connection
    {
        public static void TestConnection()
        {
            try
            {
                Connect();
                Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Add(PostIt postIt)
        {
            Connect();

            try
            {
                Connect();

                string str = "INSERT INTO postit(content, color) VALUES (@content, @color);";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@content", postIt.Content);
                command.Parameters.AddWithValue("@color", postIt.Color);
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
                string str = "UPDATE postit SET content=@content, color=@color WHERE id=@id;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@id", postIt.Id);
                command.Parameters.AddWithValue("@content", postIt.Content);
                command.Parameters.AddWithValue("@color", postIt.Color.ToString());
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

        public static void UpdateColor(PostIt postIt)
        {
            Connect();

            try
            {
                string str = "UPDATE postit SET color=@color WHERE id=@id;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@id", postIt.Id);
                command.Parameters.AddWithValue("@color", postIt.Color.ToString());
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
                string str = "SELECT id, content, color FROM postit;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    long id = dr.GetInt64(0);
                    string content = dr.GetString(1);
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom(dr.GetString(2).ToString());
                    
                    PostIt temp = new PostIt(id, content, color);
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
