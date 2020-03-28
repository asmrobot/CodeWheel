using CodeWheel.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeWheel.Infrastructure
{
    /// <summary>
    /// 模板接口
    /// </summary>
    public abstract class TemplateBase
    {
        public TemplateBase()
        {
            this.MustChoiceSaveDir = true;
            this.MustChoiceTables = true;
        }

        /// <summary>
        /// 模板名称
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// razor模板内容
        /// </summary>
        public abstract string TemplateContent
        {
            get;
        }

        /// <summary>
        /// 传递到模板的实体类型
        /// </summary>
        public abstract Type ViewModelType
        {
            get;
        }

        /// <summary>
        /// 必须选择保存路径
        /// </summary>
        public bool MustChoiceSaveDir { get; set; }

        /// <summary>
        /// 必须选择数据表
        /// </summary>
        public bool MustChoiceTables { get; set; }

        private List<VarInfoAttribute> vars;
        /// <summary>
        /// 变量列表
        /// </summary>
        public  List<VarInfoAttribute> Vars
        {
            get
            {
                if (ViewModelType == null)
                {
                    throw new NullReferenceException("ViweModelType is null");
                }
                if (vars == null)
                {
                    vars = GetVarInfos(this.ViewModelType);
                }
                return vars;
            }

        }




        /// <summary>
        /// 得到类型的变量信息
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        private List<VarInfoAttribute> GetVarInfos(Type modelType)
        {
            List<VarInfoAttribute> list = new List<VarInfoAttribute>();

            if (modelType != null)
            {
                PropertyInfo[] propertys = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo item in propertys)
                {
                    //填充tinf.Vars
                    object[] attribtes = item.GetCustomAttributes(typeof(VarInfoAttribute), false);
                    if (attribtes.Length >= 1)
                    {
                        VarInfoAttribute attr = attribtes[0] as VarInfoAttribute;
                        if (attr != null)
                        {
                            attr.VarName = item.Name;
                            list.Add(attr);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="generateFileFunc"></param>
        /// <param name="vo"></param>
        /// <returns></returns>
        public abstract bool CreateFiles(ref string msg,String saveDir,List<TableMeta> tables,GenerateFileDelegate generateFileFunc,UIVOBase vo);
    }
}
