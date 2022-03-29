using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;

namespace CodeWheel.Infrastructure.DB.Providers
{
    public class MySqlProvider:IDBProvider
    {

        public string ConnectionString { get; set; }

        
        public MySqlProvider(string dbconstr)
        {
            this.ConnectionString = dbconstr;
        }

        public string[] GetTables()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(ConnectionString);
            string sql = $"select table_name from information_schema.tables where table_schema='{builder.Database}'";
            List<string> tables = new List<string>();
            MySqlConnection connection = new MySqlConnection(this.ConnectionString);
            connection.Open();



            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            using (MySqlDataReader ddr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (!ddr.IsClosed && ddr.Read())
                {
                    tables.Add(ddr.GetString(0));
                }
            }
            return tables.ToArray();
        }

        public ColumnMetaCollection GetTableSchema(string table)
        {
            Dictionary<string, string> comments = GetColumnComment(table);
            List<string> indexColumns = GetIndexColumns(table);
            ColumnMetaCollection metas = new ColumnMetaCollection();
            MySqlConnection connection = new MySqlConnection(this.ConnectionString);
            connection.Open();


            
            MySqlCommand cmd = new MySqlCommand("select * from " + table+" limit 0,1", connection);
            using (MySqlDataReader ddr = cmd.ExecuteReader(CommandBehavior.KeyInfo|CommandBehavior.CloseConnection))
            {

                if (ddr != null && !ddr.IsClosed)
                {
                    //得元数据
                    DataTable dt = ddr.GetSchemaTable();
                    for (int i = 0, len = dt.Rows.Count; i < len; i++)
                    {
                        ColumnMeta columnMeta = new ColumnMeta();

                        columnMeta.ColumnName = dt.Rows[i][0].ToString();
                        columnMeta.ColumnOridinal = TypeConverter.ObjectToInt(dt.Rows[i][1], 0);
                        columnMeta.ColumnSize = TypeConverter.ObjectToInt(dt.Rows[i][2], 4);
                        columnMeta.IsUnique = TypeConverter.ObjectToBool(dt.Rows[i][5], false);
                        columnMeta.IsKey = TypeConverter.ObjectToBool(dt.Rows[i][6], false);

                        columnMeta.FieldType = (Type)dt.Rows[i][11];
                        columnMeta.FieldTypeName = columnMeta.FieldType.Name;
                        columnMeta.AllowDBNull = TypeConverter.ObjectToBool(dt.Rows[i][12], true);
                        if (comments.ContainsKey(columnMeta.ColumnName))
                        {
                            columnMeta.Comment = comments[columnMeta.ColumnName];
                            if (!string.IsNullOrEmpty(columnMeta.Comment))
                            {
                                columnMeta.Comment = columnMeta.Comment.Replace("\r", "").Replace("\n", "");
                            }
                        }

                        //主键没查询
                        if (columnMeta.IsKey == false && indexColumns.Contains(columnMeta.ColumnName))
                        {
                            columnMeta.IsKey = true;
                        }



                        if (dt.Rows[i][0].ToString().IndexOf(" ") > -1)
                        {
                            continue;
                        }

                        metas.Add(columnMeta);
                    }

                }
            }

            connection.Close();
            return metas;
        }



        /// <summary>
        /// 获取每列的注释
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetColumnComment(string table)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();
            MySqlConnection connection = new MySqlConnection(this.ConnectionString);
            connection.Open();

            MySqlCommand cmd = new MySqlCommand($"show full columns from {table}", connection);
            using (MySqlDataReader ddr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {

                while (ddr != null && !ddr.IsClosed&&ddr.Read())
                {
                    //得元数据
                    Int32 ordinal = ddr.GetOrdinal("Field");
                    string field = ddr.GetString(ordinal);

                    ordinal = ddr.GetOrdinal("Comment");
                    string comment = ddr.IsDBNull(ordinal) ? string.Empty : ddr.GetString(ordinal);

                    if (dict.ContainsKey(field) || string.IsNullOrEmpty(comment))
                    {
                        continue;
                    }

                    dict.Add(field, comment);

                }
            }

            connection.Close();

            return dict;
        }


        /// <summary>
        /// 获取每列的注释
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<string> GetIndexColumns(string table)
        {

            List<string> columns = new List<string>();
            MySqlConnection connection = new MySqlConnection(this.ConnectionString);
            connection.Open();

            MySqlCommand cmd = new MySqlCommand($"show index from {table};", connection);
            using (MySqlDataReader ddr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {

                while (ddr != null && !ddr.IsClosed && ddr.Read())
                {
                    //得元数据
                    Int32 ordinal = ddr.GetOrdinal("Non_unique");
                    Int32 nonUnique = ddr.GetInt32(ordinal);
                    if (nonUnique == 0)
                    {
                        //唯一索引
                        ordinal = ddr.GetOrdinal("Column_name");
                        string columnName = ddr.IsDBNull(ordinal) ? string.Empty : ddr.GetString(ordinal);

                        if(columns.Contains(columnName)||string.IsNullOrEmpty(columnName))
                        {
                            continue;
                        }
                        columns.Add(columnName);
                    }
                }
            }

            connection.Close();

            return columns;
        }
    }
}
