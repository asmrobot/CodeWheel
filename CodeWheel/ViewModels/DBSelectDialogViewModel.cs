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
            this.ConnectionTest = new DelegateCommand(this.ConnectionTestAction);
            this.SelectFile = new DelegateCommand(this.SelectFileAction);
        }
        private int m_DBTypeSelectIndex = 0;
        public int DBTypeSelectIndex
        {
            get
            {
                return m_DBTypeSelectIndex;
            }
            set
            {
                if (this.m_DBTypeSelectIndex != value)
                {
                    this.m_DBTypeSelectIndex = value;
                    this.RaisePropertyChanged("DBTypeSelectIndex");
                }
            }
        }



        


        private string m_IP = "127.0.0.1";
        public string IP {
            get
            {
                return m_IP;
            }
            set
            {
                if (m_IP != value)
                {
                    this.m_IP = value;
                    this.RaisePropertyChanged("IP");
                }
            }
        }

        private string m_Port = "3306";
        public string Port
        {
            get
            {
                return m_Port;
            }
            set
            {
                if (this.m_Port != value)
                {
                    this.m_Port = value;
                    this.RaisePropertyChanged("Port");
                }
            }
        }


        private string m_UserName = "root";
        public string UserName
        {
            get
            {
                return m_UserName;
            }
            set
            {
                if (m_UserName != value)
                {
                    this.m_UserName = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }

        private string m_Password;

        public string Password
        {
            get
            {
                return m_Password;
            }
            set
            {
                if (m_Password != value)
                {
                    this.m_Password = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }


        private string m_DBName;
        public string DBName
        {
            get
            {
                return m_DBName;
            }
            set
            {
                if (m_DBName != value)
                {
                    this.m_DBName = value;
                    this.RaisePropertyChanged("DBName");
                }
            }
        }


        private Visibility m_MySqlVisibility = Visibility.Visible;
        public Visibility MySqlVisibility
        {
            get
            {
                return m_MySqlVisibility;
            }
            set
            {
                if (m_MySqlVisibility != value)
                {
                    this.m_MySqlVisibility = value;
                    this.RaisePropertyChanged("MySqlVisibility");
                }
            }
        }

        private Visibility m_SQLServerVisibility = Visibility.Hidden;
        public Visibility SQLServerVisibility
        {
            get
            {
                return m_SQLServerVisibility;
            }
            set
            {
                if (m_SQLServerVisibility != value)
                {
                    this.m_SQLServerVisibility = value;
                    this.RaisePropertyChanged("SQLServerVisibility");
                }
            }
        }






        private Visibility m_SQLiteVisibility = Visibility.Hidden;
        public Visibility SQLiteVisibility
        {
            get
            {
                return m_SQLiteVisibility;
            }
            set
            {
                if (m_SQLiteVisibility != value)
                {
                    this.m_SQLiteVisibility = value;
                    this.RaisePropertyChanged("SQLiteVisibility");
                }
            }
        }








        #region Events
        /// <summary>
        /// 测试连接
        /// </summary>
        public DelegateCommand ConnectionTest
        {
            get;set;
        }

        private void ConnectionTestAction(object parameter)
        {
            if (IsConnectionion())
            {
                MessageBox.Show("连接成功");
            }
            else
            {
                MessageBox.Show("连接失败");
            }
        }

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
        public bool IsConnectionion()
        {
            string dbconnection = GetConnectionString();
            System.Data.Common.DbConnection connection=null;
            try
            {
                switch (this.DBTypeSelectIndex)
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

            switch (this.DBTypeSelectIndex)
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
