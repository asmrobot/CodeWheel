using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model.DB
{
    /// <summary>
    /// 表原数据
    /// </summary>
    public class TableMeta
    {
        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键名称
        /// </summary>
        public ColumnMeta PrimaryKey { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public ColumnMetaCollection Columns
        {
            get;set;
        }


    }
}
