using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using CodeWheel.Model;
using CodeWheel.Utils;
using CodeWheel.Model.DB;

namespace CodeWheel.ViewModels
{
    public class MainWindowViewModel:NotificationObject
    {

        public MainWindowViewModel(RazorProvider Provider)
        {

            this.m_TemplateList = new ObservableCollection<TemplateInfo>();
            
            //加载模板
            Provider.LoadTemplate();

            for (int i = 0; i < Provider.Templates.Count; i++)
            {
                var tinfo = Provider.Templates[i].GetTemplateInfo();
                if (tinfo != null)
                {
                    tinfo.Vars = Provider.GetVarInfo(tinfo.ViewModelType);
                    this.m_TemplateList.Add(tinfo);
                }
            }
        }

        private ObservableCollection<TemplateInfo> m_TemplateList;

        /// <summary>
        /// 模板列表
        /// </summary>
        public ObservableCollection<TemplateInfo> TemplateList
        {
            get
            {
                return m_TemplateList;
            }
            set
            {
                if (m_TemplateList != value)
                {
                    m_TemplateList = value;
                    this.RaisePropertyChanged("TemplateList");
                }
            }
        }


        private int m_TemplateSelectIndex =-1;
        /// <summary>
        /// 选择模板索引
        /// </summary>
        public int TemplateSelectIndex
        {
            get
            {
                return m_TemplateSelectIndex ;
            }

            set
            {
                if (m_TemplateSelectIndex  != value)
                {
                    m_TemplateSelectIndex  = value;
                    this.RaisePropertyChanged("TemplateSelectIndex");
                }
            }
        }


        private string m_TemplateVarSettingPage = "Dialogs/EmptyPage.xaml";
        /// <summary>
        /// 模板变量设置界面
        /// </summary>
        public string TemplateVarSettingPage
        {
            get
            {
                return m_TemplateVarSettingPage;
            }

            set
            {
                if (m_TemplateVarSettingPage != value)
                {
                    m_TemplateVarSettingPage = value;
                    this.RaisePropertyChanged("TemplateVarSettingPage");
                }
            }
        }



        private ObservableCollection<TableSelectModel> tables = new ObservableCollection<TableSelectModel>();
        /// <summary>
        /// 表列表
        /// </summary>
        public ObservableCollection<TableSelectModel> Tables
        {
            get
            {
                return tables;
            }

            set
            {
                if (tables != value)
                {
                    tables = value;
                    this.RaisePropertyChanged("Tables");
                }
            }
        }

        /// <summary>
        /// 向表列表添加一个表
        /// </summary>
        /// <param name="tableName"></param>
        public void AddTalbe(string tableName)
        {
            TableSelectModel model = new TableSelectModel();
            model.IsSelected = true;
            model.TableName = tableName;
            this.Tables.Add(model);
        }

        /// <summary>
        /// 获取所有选中的表
        /// </summary>
        /// <returns></returns>
        public List<TableMeta> GetSelectedTables()
        {
            List<TableMeta> tables = new List<TableMeta>();
            foreach (var item in Tables)
            {
                if (item.IsSelected)
                {
                    tables.Add(item.Meta);
                }
            }
            return tables;
        }




        private string connectionString = "Server=127.0.0.1;charset=utf8;Database=test;Port=3306;Uid=root;Pwd=root;default command timeout=10;";
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }

            set
            {
                if (connectionString != value)
                {
                    connectionString = value;
                    this.RaisePropertyChanged("ConnectionString");
                }
            }
        }



        private int dbTypeSelectIndex = 0;
        /// <summary>
        /// 数据库类型选择索引
        /// </summary>
        public int DBTypeSelectIndex
        {
            get
            {
                return dbTypeSelectIndex;
            }
            set
            {
                if (this.dbTypeSelectIndex != value)
                {
                    this.dbTypeSelectIndex = value;
                    this.RaisePropertyChanged("DBTypeSelectIndex");
                }
            }
        }

        /// <summary>
        /// 是否能连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnectionion()
        {
            System.Data.Common.DbConnection connection = null;
            try
            {
                switch (this.DBTypeSelectIndex)
                {
                    case 0:
                        connection = new MySql.Data.MySqlClient.MySqlConnection(this.ConnectionString);
                        break;
                    case 1:
                        connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString);
                        break;
                    case 2:
                        connection = new System.Data.SQLite.SQLiteConnection(this.ConnectionString);
                        break;
                }

                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }


    }

    /// <summary>
    /// 表选择模型
    /// </summary>
    public class TableSelectModel : NotificationObject
    {
        private bool isSelected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    this.RaisePropertyChanged("IsSelected");
                }
            }
        }


        private string tableName = string.Empty;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }

            set
            {
                if (tableName != value)
                {
                    tableName = value;
                    this.RaisePropertyChanged("TableName");
                }
            }
        }

        /// <summary>
        /// 元数据
        /// </summary>
        public TableMeta Meta
        {
            get;
            set;
        }
    }
}
