using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeWheel.Infrastructure.Utilitys;

namespace CodeWheel.Infrastructure.DB
{
    /// <summary>
    /// 表原数据
    /// </summary>
    public class TableMeta
    {

        private string tableName;

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                if (!value.Equals(this.tableName))
                {
                    this.tableName = value;
                    this.UpperCamelName = StringUtils.ToUpperCamel(value);
                    this.LowerCamelName = StringUtils.ToLowerCamel(value);
                }
            }
        }

        /// <summary>
        /// 驼峰表名
        /// </summary>
        public string UpperCamelName { get; set; }

        /// <summary>
        /// 首字母小写的驼峰表名
        /// </summary>
        public string LowerCamelName { get; set; }


        /// <summary>
        /// 主键名称
        /// </summary>
        public ColumnMeta PrimaryKey { get; set; }

        /// <summary>
        /// 表注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public ColumnMetaCollection Columns
        {
            get;set;
        }


    }
}
