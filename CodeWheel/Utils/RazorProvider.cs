using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using CodeWheel.Model;

namespace CodeWheel.Utils
{
    public class RazorProvider
    {

        public RazorProvider()
        {
            Templates = new List<Model.ITemplate>();
            RunService = RunServiceAction;
                
        }
        /// <summary>
        /// 模板
        /// </summary>
        public List<Model.ITemplate> Templates
        {
            get; set;
        }

        /// <summary>
        /// 生成模板函数
        /// </summary>
        public RunTemplateDelegate RunService
        {
            get;set;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="saveFilePath"></param>
        /// <param name="key"></param>
        /// <param name="modelType"></param>
        /// <param name="model"></param>
        private bool RunServiceAction(string saveFilePath,string key, Type modelType, object model)
        {
            try
            {
                var content = Engine.Razor.Run(key, modelType, model);
                File.WriteAllText(saveFilePath, content, System.Text.Encoding.UTF8);
                return true;
            }
            catch
            {
                //出错
                return false;
            }
            
        }

        /// <summary>
        /// 通过模板名称得到模板
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public Model.ITemplate GetTemplate(string templateName)
        {
            for (int i = 0; i < this.Templates.Count; i++)
            {
                if (this.Templates[i].GetTemplateInfo().Name == templateName)
                {
                    return this.Templates[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public bool LoadTemplate()
        {
            string BasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            DirectoryInfo dir = new DirectoryInfo(BasePath);

            FileInfo[] files = dir.GetFiles("*.dll", SearchOption.AllDirectories);
            if (files == null || files.Length <= 0)
            {
                return false;
            }

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    //加载DLL，每个DLL里查找ITemplate
                    Assembly asm = Assembly.LoadFrom(files[i].FullName);
                    Type[] ts = asm.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(Model.ITemplate))).ToArray();

                    if (ts == null)
                    {
                        continue;
                    }
                    for (int t = 0; t < ts.Length; t++)
                    {
                        //找到后实例化
                        Model.ITemplate template = Activator.CreateInstance(ts[t]) as Model.ITemplate;
                        if (template == null)
                        {
                            continue;
                        }
                        
                        
                        

                        //编译模板
                        TemplateInfo tinf = template.GetTemplateInfo();
                        if (tinf == null)
                        {
                            continue;
                        }
                        tinf.Vars = GetVarInfo(tinf.ViewModelType);

                        if (!Compile(tinf))
                        {
                            continue;
                        }


                        //添加到模板列表
                        Templates.Add(template);
                    }
                }
                catch(Exception e)
                {
                    if (e is TemplateCompilationException)
                    {
                        throw e;
                    }
                    continue;
                }

            }

            return true;
        }

        /// <summary>
        /// 得到类型的变量信息
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        internal List<VarInfoAttribute> GetVarInfo(Type modelType)
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
        /// 编译
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Compile(TemplateInfo info)
        {
            if (string.IsNullOrEmpty(info.TemplateContent))
            {
                return false;
            }

            InitRazorEngine();
            Engine.Razor.Compile(info.TemplateContent, info.Name, info.ViewModelType);
            return true;
        }

        private bool RazorEngineIsInit = false;
        private void InitRazorEngine()
        {
            if (RazorEngineIsInit)
            {
                return;
            }
            TemplateServiceConfiguration conf = new TemplateServiceConfiguration();
            conf.Language = Language.CSharp;
            conf.EncodedStringFactory = new RawStringFactory();
            //conf.EncodedStringFactory = new HtmlEncodedStringFactory();

            conf.Debug = true;



            var service = RazorEngineService.Create(conf);
            Engine.Razor = service;
            RazorEngineIsInit = true;
        }

    }
}
