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
using CodeWheel.Infrastructure;
using CodeWheel.Infrastructure.DB;

namespace CodeWheel.Utils
{
    public class RazorProvider
    {

        public RazorProvider()
        {
            
        }
        /// <summary>
        /// 模板
        /// </summary>
        public List<Infrastructure.TemplateBase> Templates
        {
            get; set;
        }


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="name"></param>
        /// <param name="modelType"></param>
        /// <param name="model"></param>
        public bool GenerateFile(string savePath,string name, Type modelType, UIVOBase model)
        {
            try
            {
                var content = Engine.Razor.Run(name, modelType, model);
                File.WriteAllText(savePath, content, System.Text.Encoding.UTF8);
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
        public Infrastructure.TemplateBase GetTemplate(string templateName)
        {
            for (int i = 0; i < this.Templates.Count; i++)
            {
                if (this.Templates[i].Name == templateName)
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
        public bool Initialize()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException("template base dir");
            }

            DirectoryInfo dir = new DirectoryInfo(basePath);

            FileInfo[] files = dir.GetFiles("*.dll", SearchOption.AllDirectories);
            if (files == null || files.Length <= 0)
            {
                return false;
            }

            Templates = new List<Infrastructure.TemplateBase>();
            foreach (var file in files)
            {
                try
                {
                    //加载DLL，每个DLL里查找ITemplate
                    Assembly asm = Assembly.LoadFrom(file.FullName);
                    Type[] templates = asm.GetTypes().Where(t => t.BaseType==typeof(Infrastructure.TemplateBase)).ToArray();

                    if (templates == null)
                    {
                        continue;
                    }
                    for (int i = 0; i < templates.Length; i++)
                    {
                        //找到后实例化
                        Infrastructure.TemplateBase template = Activator.CreateInstance(templates[i]) as Infrastructure.TemplateBase;
                        if (template == null)
                        {
                            continue;
                        }

                        //编译模板
                        if (!Compile(template))
                        {
                            continue;
                        }


                        //添加到模板列表
                        Templates.Add(template);
                    }
                }
                catch (Exception e)
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
        /// 编译
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Compile(Infrastructure.TemplateBase template)
        {
            if (template == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(template.TemplateContent))
            {
                return false;
            }

            InitRazorEngine();
            Engine.Razor.Compile(template.TemplateContent, template.Name, template.ViewModelType);
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
