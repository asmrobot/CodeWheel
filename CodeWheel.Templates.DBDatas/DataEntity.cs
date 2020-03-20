using CodeWheel.Model;
using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.DBDatas
{
    public class DataEntity: DataViewModelBase
    {
        #region 界面收集
        /// <summary>
        /// 保存路径
        /// </summary>
        [VarInfoAttribute("保存路径", "D:\\workspace\\codegen", VarType.V_Path)]
        public string SavePath { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [VarInfoAttribute("命名空间", "Namespace.Datas", VarType.V_String)]
        public string NameSpace { get; set; }



        /// <summary>
        /// 引用空间
        /// </summary>
        [VarInfoAttribute("导入命名空间", "using Namespace.Tables;\r\n", VarType.V_String)]
        public string ImportNameSpace { get; set; }

        [VarInfoAttribute("数据库", "", VarType.V_DB)]
        public DatabaseMeta Database { get; set; }


        /// <summary>
        /// 实体类前缀
        /// </summary>
        [VarInfoAttribute("实体前缀", "T", VarType.V_String)]
        public string ClassPre { get; set; }


        /// <summary>
        /// 类型后缀
        /// </summary>
        [VarInfoAttribute("类型后缀", "DBDatas", VarType.V_String)]
        public string ClassFix { get; set; }



        /// <summary>
        /// DBHelper
        /// </summary>
        [VarInfoAttribute("类型后缀", "DB.Instance", VarType.V_String)]
        public string DBHelper { get; set; }
        #endregion




    }
}
