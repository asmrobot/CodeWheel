using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model.DB.Providers
{
    public interface IDBProvider
    {
        /// <summary>
        /// 得到库所有表
        /// </summary>
        /// <returns></returns>
        string[] GetTables();

        /// <summary>
        /// 得到表所有字段
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        ColumnMetaCollection GetTableSchema(string table);
    }
}
