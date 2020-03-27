using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CodeWheel.ViewModels
{
    public class DBSelectDialogViewModel:NotificationObject
    {

        public DBSelectDialogViewModel()
        {
            this.SelectFile = new DelegateCommand(this.SelectFileAction);
        }


        /// <summary>
        /// 数据库类型 ,0:mysql,1:sqlserver,2:sqlite
        /// </summary>
        public Int32 DBType
        {
            get;
            set;
        }

        private string ip = "127.0.0.1";
        public string IP {
            get
            {
                return ip;
            }
            set
            {
                if (ip != value)
                {
                    this.ip = value;
                    this.RaisePropertyChanged("IP");
                    this.ConnectionString = GetConnectionString();
                }
            }
        }

        private string port = "3306";
        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                if (this.port != value)
                {
                    
                    this.port = value;
                    this.RaisePropertyChanged("Port");
                    this.ConnectionString = GetConnectionString();
                }
            }
        }


        private string userName = "root";
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (userName != value)
                {
                    
                    this.userName = value;
                    this.RaisePropertyChanged("UserName");
                    this.ConnectionString = GetConnectionString();
                }
            }
        }

        private string password;

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    
                    this.password = value;
                    this.RaisePropertyChanged("Password");
                    this.ConnectionString = GetConnectionString();
                }
            }
        }


        private string dbName;
        public string DBName
        {
            get
            {
                return dbName;
            }
            set
            {
                if (dbName != value)
                {
                    
                    this.dbName = value;
                    this.RaisePropertyChanged("DBName");
                    this.ConnectionString = GetConnectionString();
                }
            }
        }




        private string connectionString;
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
                    this.connectionString = value;
                    this.RaisePropertyChanged("ConnectionString");
                }
            }
        }


        private Visibility mySqlVisibility = Visibility.Visible;
        public Visibility MySqlVisibility
        {
            get
            {
                return mySqlVisibility;
            }
            set
            {
                if (mySqlVisibility != value)
                {
                    this.mySqlVisibility = value;
                    this.RaisePropertyChanged("MySqlVisibility");
                }
            }
        }

        private Visibility sqlServerVisibility = Visibility.Hidden;
        public Visibility SQLServerVisibility
        {
            get
            {
                return sqlServerVisibility;
            }
            set
            {
                if (sqlServerVisibility != value)
                {
                    this.sqlServerVisibility = value;
                    this.RaisePropertyChanged("SQLServerVisibility");
                }
            }
        }






        private Visibility sqliteVisibility = Visibility.Hidden;
        public Visibility SQLiteVisibility
        {
            get
            {
                return sqliteVisibility;
            }
            set
            {
                if (sqliteVisibility != value)
                {
                    this.sqliteVisibility = value;
                    this.RaisePropertyChanged("SQLiteVisibility");
                }
            }
        }

        #region Events

        /// <summary>
        /// 选择文件
        /// </summary>
        public DelegateCommand SelectFile
        {
            get;set;
        }

        private void SelectFileAction(object parameter)
        {
            //打开对话框
            // 在WPF中， OpenFileDialog位于Microsoft.Win32名称空间
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "文件|*.*";
            if (dialog.ShowDialog() == true)
            {
                this.DBName = dialog.FileName;
            }
        }


        #endregion
        
        /// <summary>
        /// 是否能连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnectionion()
        {
            string dbconnection = GetConnectionString();
            System.Data.Common.DbConnection connection=null;
            try
            {
                switch (this.DBType)
                {
                    case 0:
                        connection = new MySql.Data.MySqlClient.MySqlConnection(dbconnection);
                    break;
                    case 1:
                        connection = new System.Data.SqlClient.SqlConnection(dbconnection);
                        break;
                    case 2:
                        connection = new System.Data.SQLite.SQLiteConnection(dbconnection);
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


        public string GetConnectionString()
        {
            string dbconnection = string.Empty;

            switch (this.DBType)
            {
                case 0:
                    dbconnection = "Server="+IP+";charset=utf8;Database="+DBName+ ";Port="+Port+";Uid=" + UserName+";Pwd="+Password+";default command timeout=10;";
                    break;
                case 1:
                    dbconnection = "Data Source="+IP+";Initial Catalog="+DBName+";User Id="+UserName+";Password="+Password+";";
                    break;
                case 2:
                    dbconnection = "Data Source=" + this.DBName + ";Version=3;";
                    break;
            }

            return dbconnection;
        }

        


    }
}
