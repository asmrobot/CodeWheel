using CodeWheel.Infrastructure;
using CodeWheel.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.DBModel
{
    public class UIVO:UIVOBase
    {
        /// <summary>
        /// 引用空间
        /// </summary>
        [VarInfoAttribute("导入命名空间", "using System;", VarType.MultiString)]
        public string ImportNameSpace { get; set; }


        /// <summary>
        /// 命名空间
        /// </summary>
        [VarInfoAttribute("命名空间", "Namespace.Tables", VarType.SingleString)]
        public string NameSpace { get; set; }

        

        /// <summary>
        /// 类前缀
        /// </summary>
        [VarInfoAttribute("类型前缀", "T", VarType.SingleString)]
        public string ClassPre { get; set; }
    }
}
