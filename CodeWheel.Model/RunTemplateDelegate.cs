using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model
{
    /// <summary>
    /// 模板运行方法
    /// </summary>
    /// <param name="saveFilePath"></param>
    /// <param name="modelType"></param>
    /// <param name="model"></param>
    public delegate bool RunTemplateDelegate(string saveFilePath,string key, Type modelType = null, object model = null);
}
