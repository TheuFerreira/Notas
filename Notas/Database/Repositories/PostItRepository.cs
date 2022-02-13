using Notas.Database.Interfaces;
using Notas.Database.Models;
using Notas.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Media;

namespace Notas.Database.Repositories
{
    public class PostItRepository : IPostItRepository
    {
        private readonly IDbRepository _dbRepository;

        public PostItRepository(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public void Insert(PostIt postIt)
        {
            try
            {
                string sql = "INSERT INTO postit(content, position) VALUES (@content, @position);";

                SQLiteParameter contentParameter = new SQLiteParameter("@content", postIt.Content);
                SQLiteParameter positionParameter = new SQLiteParameter("@position", postIt.Position);

                _dbRepository.ExecuteNonQuery(sql, contentParameter, positionParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(PostIt postIt)
        {
            try
            {
                string sql = "DELETE FROM postit WHERE id = @id";
                SQLiteParameter idParameter = new SQLiteParameter("@id", postIt.Id);
                _dbRepository.ExecuteNonQuery(sql, idParameter);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(PostIt postIt)
        {
            try
            {
                string sql = "UPDATE postit SET content=@content, position=@position WHERE id=@id;";

                SQLiteParameter idParameter = new SQLiteParameter("@id", postIt.Id);
                SQLiteParameter contentParameter = new SQLiteParameter("@content", postIt.Content);
                SQLiteParameter positionParameter = new SQLiteParameter("@position", postIt.Position);

                _dbRepository.ExecuteNonQuery(sql, idParameter, contentParameter, positionParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateColor(long id, string color)
        {
            try
            {
                string sql = "UPDATE postit SET color = @color WHERE id = @id;";

                SQLiteParameter idParameter = new SQLiteParameter("@id", id);
                SQLiteParameter colorParameter = new SQLiteParameter("@color", color);

                _dbRepository.ExecuteNonQuery(sql, idParameter, colorParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateFontColor(long id, string color)
        {
            try
            {
                string sql = "UPDATE postit SET fontColor = @color WHERE id = @id;";

                SQLiteParameter idParameter = new SQLiteParameter("@id", id);
                SQLiteParameter colorParameter = new SQLiteParameter("@color", color);

                _dbRepository.ExecuteNonQuery(sql, idParameter, colorParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PostIt> GetAll()
        {
            List<PostIt> postIts = new List<PostIt>();

            try
            {
                string sql = "SELECT p.id, p.content, p.color, p.position, p.fontColor FROM postit AS p ORDER BY p.position DESC;";
                List<List<object>> result = _dbRepository.ExecuteReader(sql);

                foreach (List<object> list in result)
                {
                    long id = Convert.ToInt64(list[0]);
                    string content = list[1].ToString();
                    SolidColorBrush color = ConvertToColor(list[2]);
                    int position = Convert.ToInt32(list[3]);
                    SolidColorBrush fontColor = ConvertToColor(list[4]);

                    PostIt temp = new PostIt(id, content, color, position, fontColor);
                    postIts.Add(temp);
                }

                return postIts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SolidColorBrush ConvertToColor(object obj)
        {
            return obj == null ? null : (SolidColorBrush)new BrushConverter().ConvertFrom(obj);
        }
    }
}
