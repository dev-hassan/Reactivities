using Dapper;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ISP_Call : IDisposable
    {
        IEnumerable<T> ReturnList<T>(string procedtureName, DynamicParameters param = null);

        void ExecuteWithoutReturn(string procedtureName, DynamicParameters param = null);

        T ExecuteReturnScalar<T>(string procedtureName, DynamicParameters param = null);
    }
}
