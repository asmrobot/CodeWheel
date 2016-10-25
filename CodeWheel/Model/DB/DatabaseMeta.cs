using CodeWheel.Model.DB.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model.DB
{

    public enum DBType
    {
        Mysql,
        Sqlserver,
        Sqlite
    }
    /// <summary>
    /// 数据库原数据
    /// </summary>
    public class DatabaseMeta
    {
        /// <summary>
        /// 数据库类型
        /// mysql:MySql
        /// sqlserver:SqlServer
        /// sqlite:Sqlite
        /// </summary>
        public DBType DBType
        {
            get;set;
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DBName { get; set; }


        /// <summary>
        /// 数据表
        /// </summary>
        public List<TableMeta> Tables
        {
            get;
            set;
        }

        /// <summary>
        /// 通过Mysql数据库创建
        /// </summary>
        /// <param name="connectionstr"></param>
        /// <returns></returns>
        public static DatabaseMeta CreateByMysql(string connectionstr,string dbname)
        {
            return CreateDatabaseMeta(DBType.Mysql, connectionstr, dbname);
        }

        /// <summary>
        /// 通过sqlserver创建
        /// </summary>
        /// <param name="connectionstr"></param>
        /// <returns></returns>
        public static DatabaseMeta CreateBySqlserver(string connectionstr,string dbname)
        {
            return CreateDatabaseMeta(DBType.Sqlserver, connectionstr, dbname);
        }

        /// <summary>
        /// 通过sqlite创建
        /// </summary>
        /// <param name="connectionstr"></param>
        /// <returns></returns>
        public static DatabaseMeta CreateBySqlite(string connectionstr,string dbname)
        {
            return CreateDatabaseMeta(DBType.Sqlite, connectionstr, dbname);
        }

        private static DatabaseMeta CreateDatabaseMeta(DBType dbt, string connectionstring, string dbname)
        {
            DatabaseMeta dbm = new DatabaseMeta();
            dbm.ConnectionString = connectionstring;
            dbm.DBName = dbname;
            dbm.DBType = dbt;
            dbm.Tables = new List<TableMeta>();

            IDBProvider provider = ProviderFactory.CreateInstance(dbm.DBType,connectionstring,dbname);

            string[] tables = provider.GetTables();
            for (int i = 0; i < tables.Length; i++)
            {
                TableMeta tableMeta = new TableMeta();
                tableMeta.TableName = tables[i];
                tableMeta.Columns = provider.GetTableSchema(tableMeta.TableName);
                tableMeta.PrimaryKey=tableMeta.Columns.Where(t => t.IsKey).FirstOrDefault();

                dbm.Tables.Add(tableMeta);
            }
            return dbm;
        }
    }
}
