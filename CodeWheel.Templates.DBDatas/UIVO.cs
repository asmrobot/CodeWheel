using CodeWheel.Model;
using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.DBDatas
{
    public class UIVO: UIVOBase
    {
        #region 界面收集


        /// <summary>
        /// 引用空间
        /// </summary>
        [VarInfoAttribute("导入命名空间", "using Namespace.Tables;\r\n", VarType.MultiString)]
        public string ImportNameSpace { get; set; }


        /// <summary>
        /// 命名空间
        /// </summary>
        [VarInfoAttribute("命名空间", "Namespace.Datas", VarType.SingleString)]
        public string NameSpace { get; set; }



        



        /// <summary>
        /// 实体类前缀
        /// </summary>
        [VarInfoAttribute("实体前缀", "T", VarType.SingleString)]
        public string ClassPre { get; set; }


        /// <summary>
        /// 类型后缀
        /// </summary>
        [VarInfoAttribute("类型后缀", "DBDatas", VarType.SingleString)]
        public string ClassFix { get; set; }



        /// <summary>
        /// DBHelper
        /// </summary>
        [VarInfoAttribute("类型后缀", "DB.Instance", VarType.SingleString)]
        public string DBHelper { get; set; }
        #endregion




    }
}
