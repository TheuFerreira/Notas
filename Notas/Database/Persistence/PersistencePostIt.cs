﻿using Notas.Database.Table;
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

                string str = "INSERT INTO postit(content, position) VALUES (@content, @position);";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@content", postIt.Content);
                command.Parameters.AddWithValue("@position", postIt.Position);
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
                string str = "UPDATE postit SET content=@content, position=@position WHERE id=@id;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@id", postIt.Id);
                command.Parameters.AddWithValue("@content", postIt.Content);
                command.Parameters.AddWithValue("@position", postIt.Position);
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

        public static void UpdateColor(long id, string color)
        {
            try
            {
                Connect();

                string str = "UPDATE postit SET color = @color WHERE id = @id;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@color", color);
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

        public static void UpdateFontColor(long id, string color)
        {
            try
            {
                Connect();

                string str = "UPDATE postit SET fontColor = @color WHERE id = @id;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@color", color);
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
                string str = "SELECT p.id, p.content, p.color, p.position, p.fontColor FROM postit AS p ORDER BY p.position DESC;";
                SQLiteCommand command = new SQLiteCommand(str, SQLiteConnection);
                
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    long id = dr.GetInt64(0);
                    string content = dr[1].ToString();
                    SolidColorBrush color = dr.IsDBNull(2) ? null : (SolidColorBrush)new BrushConverter().ConvertFrom(dr.GetString(2).ToString());
                    int position = dr.GetInt32(3);
                    SolidColorBrush fontColor = dr.IsDBNull(4) ? null : (SolidColorBrush)new BrushConverter().ConvertFrom(dr.GetString(4).ToString());

                    PostIt temp = new PostIt(id, content, color, position, fontColor);
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
