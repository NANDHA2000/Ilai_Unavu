using EleOota.Repository.Infrastructure.Interface;

namespace EleOota.Repository.Infrastructure
{
    public class SqlQueryBuilder : IQueryBuilder
    {
        public string GetAllQuery(string tablename)
        {
            return $"select * from{tablename}";
        }
    }
}
