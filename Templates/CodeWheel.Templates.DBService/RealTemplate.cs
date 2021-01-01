using CodeWheel.Infrastructure;
using CodeWheel.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeWheel.Templates.DBService
{
    /// <summary>
    /// 数据库实体类生成
    /// </summary>
    public class RealTemplate : TemplateBase
    {
        public const string TEMPLATE_NAME = "Dapper Repository";

        public override string Name
        {
            get
            {
                return TEMPLATE_NAME;
            }
        }

        private string templateContent = string.Empty;
        /// <summary>
        /// razor模板内容
        /// </summary>
        public override string TemplateContent
        {
            get
            {
                if (string.IsNullOrEmpty(templateContent))
                {
                    Assembly asm = System.Reflection.Assembly.GetAssembly(this.GetType());
                    using (Stream stream = asm.GetManifestResourceStream(string.Concat(asm.GetName().Name, ".TemplateFile.txt")))
                    {
                        StreamReader reader = new StreamReader(stream);
                        templateContent = reader.ReadToEnd();
                    }
                }

                return templateContent;
            }
        }

        /// <summary>
        /// model类型
        /// </summary>
        public override Type ViewModelType
        {
            get
            {
                return typeof(UIVO);
            }
        }

        public override  bool CreateFiles(ref string msg, string saveDir, List<TableMeta> tables, GenerateFileDelegate generateFileFunc, UIVOBase vo)
        {
            msg = string.Empty;
            UIVO viewModel = vo as UIVO;

            for (int i = 0; i < tables.Count; i++)
            {
                viewModel.CurrentTable = tables[i];
                string file = Path.Combine(saveDir, viewModel.CurrentTable.UpperCamelName + viewModel.ClassFix + ".cs");

                if (!generateFileFunc(file, TEMPLATE_NAME, typeof(UIVO), viewModel))
                {
                    continue;
                }
            }

            return true;
        }

    }
}
