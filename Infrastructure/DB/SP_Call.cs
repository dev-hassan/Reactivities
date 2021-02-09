using Application.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data;

namespace Infrastructure.DB
{
    public class SP_Call : ISP_Call
    {
        private readonly DataContext _db;
        private static string ConnectionString = string.Empty;

        public SP_Call(DataContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public T ExecuteReturnScalar<T>(string procedtureName, DynamicParameters param = null)
        {
            using (var sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return (T)Convert.ChangeType(sqlCon.ExecuteScalar<T>(procedtureName, param, commandType: CommandType.StoredProcedure), typeof(T));
            }
        }

        public void ExecuteWithoutReturn(string procedtureName, DynamicParameters param = null)
        {
            using (var sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(procedtureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> ReturnList<T>(string procedtureName, DynamicParameters param = null)
        {
            using (var sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(procedtureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
