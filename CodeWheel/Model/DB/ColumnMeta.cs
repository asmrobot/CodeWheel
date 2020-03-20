using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeWheel.Utils;

namespace CodeWheel.Model.DB
{
    /// <summary>
    /// 列原数据
    /// </summary>
    public class ColumnMeta
    {
        private string columnName;

        public string ColumnName
        {
            get
            {
                return this.columnName;
            }
            set
            {
                if (!value.Equals(this.columnName))
                {
                    this.columnName = value;
                    this.UpperCamelColumnName = StringUtils.ToUpperCamel(value);
                    this.LowerCamelColumnName = StringUtils.ToLowerCamel(value);
                }
            }
        }

        /// <summary>
        /// 首字母小写的驼峰列名
        /// </summary>
        public string LowerCamelColumnName { get; set; }

        /// <summary>
        /// 驼峰列名
        /// </summary>
        public string UpperCamelColumnName { get; set; }


        /// <summary>
        /// 列序号
        /// </summary>
        public int ColumnOridinal
        {
            get;
            set;
        }


        /// <summary>
        /// 字段占用空间大小
        /// </summary>
        public int ColumnSize
        {
            get;
            set;
        }


        /// <summary>
        /// 是否唯一标识
        /// </summary>
        public bool IsUnique
        {
            get;
            set;
        }

        /// <summary>
        /// 是否主键或外键
        /// </summary>
        public bool IsKey
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool AllowDBNull
        {
            get;
            set;
        }

        /// <summary>
        /// 字段类型名
        /// </summary>
        public string FieldTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public Type FieldType
        {
            get;
            set;
        }

        

        
    }
}
