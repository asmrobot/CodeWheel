using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Workstation.T4
{

    public class T4DB 
    {
        private readonly string m_ConnectionString;
        public T4DB(string connectionStr)
        {
            this.m_ConnectionString = connectionStr;
        }

        public string ConnectionString
        {
            get
            {
                return this.m_ConnectionString;
            }
        }


        /// <summary>
        /// 得到本连接库中所有的表
        /// </summary>
        /// <returns></returns>
        public string[] GetTables()
        {
            List<string> tables = new List<string>();
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name FROM SysObjects Where XType='U' ORDER BY Name", connection);
            using(SqlDataReader ddr=cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (!ddr.IsClosed && ddr.Read())
                {
                    tables.Add(ddr.GetString(0));
                }
            }
            return tables.ToArray();
        }



        /// <summary>
        /// 得到表的字段名及类型字典
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public ColumnMetaCollection GetTableSchema(string table)
        {
            ColumnMetaCollection metas = new ColumnMetaCollection();
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("select * from "+table, connection);
            using (SqlDataReader ddr = cmd.ExecuteReader(CommandBehavior.KeyInfo))
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
                        //    builder.AppendLine(dt.Columns[offset].ColumnName+"=>(index:"+offset+")=>"+dt.Rows[i][offset]);
                        //}


                        ColumnMeta columnMeta = new ColumnMeta();

                        columnMeta.ColumnName = dt.Rows[i][0].ToString();
                        columnMeta.ColumnOridinal = ObjectToInt(dt.Rows[i][1], 0);
                        columnMeta.ColumnSize = ObjectToInt(dt.Rows[i][2], 4);
                        columnMeta.IsUnique = ObjectToBool(dt.Rows[i][5], false);
                        columnMeta.IsKey = ObjectToBool(dt.Rows[i][6], false);


                        columnMeta.FieldTypeName = ((Type)dt.Rows[i][12]).Name;
                        columnMeta.FieldType = (Type)dt.Rows[i][12];
                        columnMeta.AllowDBNull = ObjectToBool(dt.Rows[i][13], true);




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
