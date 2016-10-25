using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model.DB.Providers
{
    public class ProviderFactory
    {
        public static IDBProvider CreateInstance(DBType dbt,string connectionString,string dbname)
        {
            switch (dbt)
            {
                case DBType.Mysql:
                    return new MySqlProvider(connectionString,dbname);
                case DBType.Sqlserver:
                    return new SqlServerProvider(connectionString,dbname);
                case DBType.Sqlite:
                    return new SqliteProvider(connectionString,dbname);
            }
            return null;
        }
    }
}
