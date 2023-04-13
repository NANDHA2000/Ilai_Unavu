using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EleOota.Repository.Infrastructure.Interface
{
    public interface IConnectionFactory : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
