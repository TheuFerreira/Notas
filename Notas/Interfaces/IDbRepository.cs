using System.Collections.Generic;
using System.Data.SQLite;

namespace Notas.Interfaces
{
    public interface IDbRepository
    {
        string ConnectionString { get; }
        int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters);
        List<List<object>> ExecuteReader(string sql, params SQLiteParameter[] parameters);
    }
}
