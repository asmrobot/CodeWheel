using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workstation.T4
{
    /// <summary>
    /// 数据库工具
    /// </summary>
    public class DatabaseUtils
    {
        /// <summary>
        /// 得到表及元数据
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static Dictionary<string, ColumnMetaCollection> GetTableMeta(string connectString,string[] filterTables,bool onlyTable)
        {            
            //读取表原数据
            T4DB db = new T4DB(connectString);
            string[] tables = db.GetTables();
            Dictionary<string, ColumnMetaCollection> tableDirect = new Dictionary<string, ColumnMetaCollection>();
            for (int i = 0; i < tables.Length; i++)
            {
                bool has=filterTables.Has(tables[i]);
                if (onlyTable)
                {
                    if (has)
                    {
                        ColumnMetaCollection metas = db.GetTableSchema(tables[i]);
                        tableDirect.Add(tables[i], metas);
                    }
                }
                else
                {
                    if (!has)
                    {
                        ColumnMetaCollection metas = db.GetTableSchema(tables[i]);
                        tableDirect.Add(tables[i], metas);
                    }
                }
            }
            return tableDirect;
        }

        /// <summary>
        /// 得到所有非主键列
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static ColumnMetaCollection GetNoPrimaryKeyColumns(ColumnMetaCollection columns)
        {
            return new ColumnMetaCollection(columns.Where((col) => { return !col.IsKey; }));
            
        }

        /// <summary>
        /// 得到所有非主键列
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static ColumnMetaCollection GetPrimaryKeyColumns(ColumnMetaCollection columns)
        {
            return new ColumnMetaCollection(columns.Where((col) => { return col.IsKey; }));
        }


        /// <summary>
        /// 得到所有非主键列
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static ColumnMeta GetPrimaryKeyColumn(ColumnMetaCollection columns)
        {
            return columns.Where((col) => { return col.IsKey; }).FirstOrDefault();
        }

        /// <summary>
        /// 创建形参列表
        /// </summary>
        /// <param name="columns">列</param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public static string CreateMethodParameterList(ColumnMetaCollection columns)
        {
            return columns.Concat((col) => { return col.FieldTypeName + " "+col.ColumnName.ToLower(); },",");
        }

        /// <summary>
        /// 创建实参列表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public static string CreateMethodArgumentList(string tableName, ColumnMetaCollection columns)
        {
            return columns.Concat((col) => { return tableName.ToLower()+"."+col.ColumnName; }, ",");
        }


        /// <summary>
        /// 创建插入SQL形参列表
        /// </summary>
        /// <param name="columns">列</param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public static string CreateInsertParameterList(ColumnMetaCollection columns)
        {
            return columns.Concat((col) => { return "`"+col.ColumnName+"`"; }, ",");
        }

        /// <summary>
        /// 创建插入SQL实参列表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public static string CreateInsertArgumentList(ColumnMetaCollection columns)
        {
            return columns.Concat((col) => { return "@" + col.ColumnName ; }, ",");
        }


        /// <summary>
        /// 创建Update语句参数列表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public static string CreateUpdateArgumentList(ColumnMetaCollection columns)
        {
            return columns.Concat((col) => { return col.ColumnName+"=@" + col.ColumnName; }, ",");
        }

        /// <summary>
        /// 创建DBparameter列表
        /// </summary>
        /// <param name="DbHelperInstanceName"></param>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey"></param>
        /// <returns></returns>
        public static string CreateDbParameterList(string DbHelperInstanceName,string tableName, ColumnMetaCollection columns)
        {
            //ZTDB.Instance .MakeInParam ("@#item.ColumnName#",DbType .#item.FieldTypeName#,#item.ColumnSize#,#tolower(item.ColumnName)# )<ad:if test="#notequals(i,fields.Count)#">,
            return columns.Concat((col) => { return CreateDbParameter(DbHelperInstanceName,tableName,col); }, ",\r\n\t\t\t\t");
        }

        /// <summary>
        /// 创建DBparameter
        /// </summary>
        /// <param name="DbHelperInstanceName"></param>
        /// <param name="tableName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string CreateDbParameter(string DbHelperInstanceName, string tableName, ColumnMeta col)
        {
            return DbHelperInstanceName + ".MakeInParam(\"@" + col.ColumnName + "\"," + "DbType." + col.FieldTypeName + "," + col.ColumnSize + "," + (string.IsNullOrEmpty(tableName) ? col.ColumnName.ToLower() : tableName.ToLower() + "." + col.ColumnName) + ")"; ;
        }

        /// <summary>
        /// 拼Where
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string CreateSqlWhere(ColumnMetaCollection columns)
        {
            return columns.Concat((col) => { return " `" + col.ColumnName + "`=@" + col.ColumnName; }, "and");
        }


    }
}
