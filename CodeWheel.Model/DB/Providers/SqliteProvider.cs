using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace CodeWheel.Model.DB.Providers
{
    public class SqliteProvider:IDBProvider
    {

        public string ConnectionString { get; set; }

        public string DBName { get; set; }
        public SqliteProvider(string dbconstr, string dbname)
        {
            this.ConnectionString = dbconstr;
            this.DBName = dbname;
        }

        public string[] GetTables()
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";
            List<string> tables = new List<string>();
            SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
            connection.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            using (SQLiteDataReader ddr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
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
            ColumnMetaCollection metas = new ColumnMetaCollection();
            SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand("select * from " + table+" limit 0,1", connection);
            using (SQLiteDataReader ddr = cmd.ExecuteReader(CommandBehavior.KeyInfo))
            {

                if (ddr != null && !ddr.IsClosed)
                {
                    //得元数据
                    DataTable dt = ddr.GetSchemaTable();

                    //StringBuilder builder = new StringBuilder();
                    for (int i = 0, len = dt.Rows.Count; i < len; i++)
                    {

                        //for (int offset = 0; offset < dt.Columns.Count; offset++)
                        //{
                        //    builder.AppendLine(dt.Columns[offset].ColumnName + "=>(index:" + offset + ")=>" + dt.Rows[i][offset]);
                        //}


                        ColumnMeta columnMeta = new ColumnMeta();

                        columnMeta.ColumnName = dt.Rows[i][0].ToString();
                        columnMeta.ColumnOridinal = TypeConverter.ObjectToInt(dt.Rows[i][1], 0);
                        columnMeta.ColumnSize = TypeConverter.ObjectToInt(dt.Rows[i][2], 4);
                        columnMeta.IsUnique = TypeConverter.ObjectToBool(dt.Rows[i][5], false);
                        columnMeta.IsKey = TypeConverter.ObjectToBool(dt.Rows[i][6], false);


                        columnMeta.FieldTypeName = ((Type)dt.Rows[i][12]).Name;
                        columnMeta.FieldType = (Type)dt.Rows[i][12];
                        columnMeta.AllowDBNull = TypeConverter.ObjectToBool(dt.Rows[i][13], true);




                        if (dt.Rows[i][0].ToString().IndexOf(" ") > -1)
                        {
                            continue;
                        }

                        metas.Add(columnMeta);
                    }

                    //File.WriteAllText("d:\\struct.txt", builder.ToString());

                }
            }
            return metas;
        }
    }
}
