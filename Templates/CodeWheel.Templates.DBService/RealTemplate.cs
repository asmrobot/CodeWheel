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

        private const string Generate_Dir = "Generates";
        public override  bool CreateFiles(ref string msg, string saveDir, List<TableMeta> tables, GenerateFileDelegate generateFileFunc, UIVOBase vo)
        {
            msg = string.Empty;
            UIVO viewModel = vo as UIVO;

            //生成基类
            GenerateBaseClassFile(saveDir, viewModel);

            for (int i = 0; i < tables.Count; i++)
            {
                viewModel.CurrentTable = tables[i];
                string file = Path.Combine(saveDir, Generate_Dir, viewModel.CurrentTable.UpperCamelName + viewModel.ClassFix + ".cs");

                if (!generateFileFunc(file, TEMPLATE_NAME, typeof(UIVO), viewModel))
                {
                    continue;
                }

                GeneratePartialClassFile(saveDir, viewModel);
            }

            return true;
        }

        /// <summary>
        /// 生成部分类
        /// </summary>
        /// <param name="saveDir"></param>
        /// <param name="viewModel"></param>
        private void GeneratePartialClassFile(string saveDir, UIVO viewModel)
        {
            const string template = @"$import_namespace$
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using ZTImage;
using System.Threading.Tasks;
using Dapper;
using ZTImage.DbLite;


namespace Zhuomi.Business
{
    public partial class $classname$
    {
        
        public $classname$(DbConnectionFactory connectionFactory):base(connectionFactory)
        {
            
        }
    }
}";
            string dir = Path.Combine(saveDir, viewModel.CurrentTable.UpperCamelName + viewModel.ClassFix + ".cs");
            File.WriteAllText(dir, template
                .Replace("$import_namespace$", viewModel.ImportNameSpace)
                .Replace("$classname$", viewModel.CurrentTable.UpperCamelName + viewModel.ClassFix)
                );

        }

        /// <summary>
        /// 生成基类文件
        /// </summary>
        /// <param name="saveDir"></param>
        /// <param name="viewModel"></param>
        private void GenerateBaseClassFile(string saveDir,UIVO viewModel)
        {
            var path = Path.Combine(saveDir, Generate_Dir);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            const string baseClassText = @"using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZTImage.DbLite;

namespace $namespace$
{
    public class ServiceBase
    {
        public ServiceBase(DbConnectionFactory factory)
        {
            this.conFactory = factory;
        }

        protected DbConnectionFactory conFactory;
        protected IDbConnection ConnectionWrap(IDbTransaction transaction, out bool needClose)
        {
            IDbConnection connection = null;
            if (transaction != null)
            {
                needClose = false;
                connection = transaction.Connection;
            }
            else
            {
                needClose = true;
                connection = conFactory.CreateConnection();
            }

            return connection;
        }
    }
}
";
            string dir = Path.Combine(saveDir, Generate_Dir,"ServiceBase.cs");

            File.WriteAllText(dir, baseClassText
                .Replace("$namespace$", viewModel.NameSpace)
                );
        }
    }
}
