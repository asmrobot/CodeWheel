using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Infrastructure
{
    /// <summary>
    /// 模板运行方法
    /// </summary>
    /// <param name="savePath"></param>
    /// <param name="modelType"></param>
    /// <param name="model"></param>
    public delegate bool GenerateFileDelegate(string savePath,string name, Type modelType = null, UIVOBase model = null);
}
