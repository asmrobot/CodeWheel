using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model
{
    /// <summary>
    /// 模板接口
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// 得到基本信息
        /// </summary>
        /// <returns></returns>
        TemplateInfo GetTemplateInfo();

        /// <summary>
        /// 使用得到的信息生成模板
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool CreateFiles(out string msg,RunTemplateDelegate method,object parameters);
    }
}
