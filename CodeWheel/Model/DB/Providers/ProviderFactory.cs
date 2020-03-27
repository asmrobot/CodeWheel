using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model.DB.Providers
{
    public class ProviderFactory
    {
        public static IDBProvider CreateInstance(DBType dbt,string connectionString)
        {
            switch (dbt)
            {
                case DBType.Mysql:
                    return new MySqlProvider(connectionString);
                case DBType.Sqlserver:
                    return new SqlServerProvider(connectionString);
                case DBType.Sqlite:
                    return new SqliteProvider(connectionString);
            }
            return null;
        }
    }
}
