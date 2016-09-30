using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.NetCSharp
{
    public class DataEntity
    {
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// 引用空间
        /// </summary>
        public string ImportNameSpace { get; set; }

        public DatabaseMeta Database { get; set; }

        /// <summary>
        /// 当前数据表
        /// </summary>
        public TableMeta CurrentTable { get; set; }

        /// <summary>
        /// 类前缀
        /// </summary>
        public string ClassPre { get; set; }
    }
}
