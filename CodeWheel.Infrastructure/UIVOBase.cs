using CodeWheel.Infrastructure.Extensions;
using CodeWheel.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Infrastructure
{
    /// <summary>
    /// 界面view object基类
    /// </summary>
    public class UIVOBase
    {

        /// <summary>
        /// 当前数据表
        /// </summary>
        public TableMeta CurrentTable { get; set; }


        /// <summary>
        /// 创建形参列表
        /// </summary>
        /// <param name="columns">列</param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public string MethodParameterList(bool hasKey)
        {
            ColumnMetaCollection collection = this.CurrentTable.Columns;
            if (!hasKey)
            {
                collection = collection.GetNoKeyCollection();
            }


            return collection.Concat((col) => { return col.FieldTypeName + " " + col.ColumnName.ToLower(); }, ",");
        }

        /// <summary>
        /// 创建实参列表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public string MethodArgumentList(string tableName, bool hasKey)
        {
            ColumnMetaCollection collection = this.CurrentTable.Columns;
            if (!hasKey)
            {
                collection = collection.GetNoKeyCollection();
            }
            return collection.Concat((col) => { return tableName.ToLower() + "." + col.ColumnName; }, ",");
        }

        

        /// <summary>
        /// 创建插入SQL形参列表,默认列
        /// </summary>
        /// <param name="columns">列</param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public string InsertParameterList(bool hasKey)
        {
            ColumnMetaCollection collection = this.CurrentTable.Columns;
            if (!hasKey)
            {
                collection = collection.GetNoKeyCollection();
            }


            return collection.Concat((col) => { return "`" + col.ColumnName + "`"; }, ",");
        }
        

        /// <summary>
        /// 创建插入SQL实参列表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public string InsertArgumentList(bool hasKey)
        {
            ColumnMetaCollection collection = this.CurrentTable.Columns;
            if (!hasKey)
            {
                collection = collection.GetNoKeyCollection();
            }
            return collection.Concat((col) => { return "@" + col.ColumnName; }, ",");
        }


        /// <summary>
        /// 创建Update语句参数列表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey">是否包含主键</param>
        /// <returns></returns>
        public string CreateUpdateArgumentList(bool hasKey)
        {
            ColumnMetaCollection collection = this.CurrentTable.Columns;
            if (!hasKey)
            {
                collection = collection.GetNoKeyCollection();
            }

            return collection.Concat((col) => { return "`"+col.ColumnName + "`=@" + col.ColumnName; }, ",");
        }

        

        /// <summary>
        /// 创建DBparameter列表
        /// </summary>
        /// <param name="DbHelperInstanceName"></param>
        /// <param name="columns"></param>
        /// <param name="hasPrimaryKey"></param>
        /// <returns></returns>
        public string CreateDbParameterList(string DbHelperInstanceName, string varName, bool hasKey)
        {
            ColumnMetaCollection collection = this.CurrentTable.Columns;
            if (!hasKey)
            {
                collection = collection.GetNoKeyCollection();
            }


            return collection.Concat((col) => { return CreateDbParameter(DbHelperInstanceName, varName, col); }, ",\r\n\t\t\t\t");
        }

        /// <summary>
        /// 创建DBparameter
        /// </summary>
        /// <param name="DbHelperInstanceName"></param>
        /// <param name="varName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string CreateDbParameter(string DbHelperInstanceName, string varName, ColumnMeta col)
        {
            return DbHelperInstanceName + ".MakeInParam(\"@" + col.ColumnName + "\"," + "DbType." + col.FieldTypeName + "," + col.ColumnSize + "," + (string.IsNullOrEmpty(varName) ? col.LowerCamelColumnName : varName + "." + col.UpperCamelColumnName) + ")"; ;
        }

        /// <summary>
        /// 拼Where
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public string CreateSqlWhere()
        {
            return this.CurrentTable.Columns.GetKeyCollection().Concat((col) => { return " `" + col.ColumnName + "`=@" + col.ColumnName; }, "and");
        }

        /// <summary>
        /// 得到主键列表
        /// </summary>
        /// <returns></returns>
        public ColumnMetaCollection GetKeyColumn()
        {
            return this.CurrentTable.Columns.GetKeyCollection();
        }

        /// <summary>
        /// 得到非主键列表
        /// </summary>
        /// <returns></returns>
        public ColumnMetaCollection GetNoKeyColumn()
        {
            return this.CurrentTable.Columns.GetNoKeyCollection();
        }


    }
}
