﻿using CodeWheel.Infrastructure;
using CodeWheel.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.DBService
{
    public class UIVO: UIVOBase
    {
        #region 界面收集


        /// <summary>
        /// 引用空间
        /// </summary>
        [VarInfoAttribute("导入命名空间", "using Namespace.Models;\r\n", VarType.MultiString)]
        public string ImportNameSpace { get; set; }


        /// <summary>
        /// 命名空间
        /// </summary>
        [VarInfoAttribute("命名空间", "Namespace.Business", VarType.SingleString)]
        public string NameSpace { get; set; }


        /// <summary>
        /// 实体类前缀
        /// </summary>
        [VarInfoAttribute("实体前缀", "T", VarType.SingleString)]
        public string ClassPre { get; set; }


        /// <summary>
        /// 类型后缀
        /// </summary>
        [VarInfoAttribute("类型后缀", "Service", VarType.SingleString)]
        public string ClassFix { get; set; }


        #endregion
    }
}
