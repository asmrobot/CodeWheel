using CodeWheel.Model;
using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.DBModel
{
    public class DataEntity
    {
        /// <summary>
        /// 保存路径
        /// </summary>
        [VarInfoAttribute("保存路径", "D:\\Codegen", VarType.V_Path)]
        public string SavePath { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [VarInfoAttribute("命名空间", "Surezen.Inone.Domain.Tables", VarType.V_String)]
        public string NameSpace { get; set; }

        /// <summary>
        /// 引用空间
        /// </summary>
        [VarInfoAttribute("导入命名空间", "using System;", VarType.V_String)]
        public string ImportNameSpace { get; set; }

        [VarInfoAttribute("数据库", "", VarType.V_DB)]
        public DatabaseMeta Database { get; set; }

        /// <summary>
        /// 当前数据表
        /// </summary>
        public TableMeta CurrentTable { get; set; }

        /// <summary>
        /// 类前缀
        /// </summary>
        [VarInfoAttribute("类型前缀", "T_", VarType.V_String)]
        public string ClassPre { get; set; }
    }
}
