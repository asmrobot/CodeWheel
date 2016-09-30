using CodeWheel.Model.DB;
using CodeWheel.Utils;
using CodeWheel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CodeWheel.Dialogs
{
    /// <summary>
    /// DBSelectDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DBSelectDialog : Window
    {
        public DBSelectDialogViewModel ViewModel
        {
            get;set;
        }

        public DBSelectDialog()
        {
            
            InitializeComponent();
            this.ViewModel = new DBSelectDialogViewModel();
            this.DataContext = this.ViewModel;
        }

        private void DBSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ViewModel == null)
            {
                return;
            }
            this.ViewModel.MySqlVisibility = Visibility.Hidden;
            this.ViewModel.SQLServerVisibility = Visibility.Hidden;
            this.ViewModel.SQLiteVisibility = Visibility.Hidden;
            switch (this.ViewModel.DBTypeSelectIndex)
            {
                case 0:
                    this.ViewModel.MySqlVisibility = Visibility.Visible;
                    break;
                case 1:
                    this.ViewModel.SQLServerVisibility = Visibility.Visible;
                    break;
                case 2:
                    this.ViewModel.SQLiteVisibility = Visibility.Visible;
                    break;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ViewModel.IsConnectionion())
            {
                MessageBox.Show("选择的数据库连接不上，请重新填写");
                return;
            }
            string connectionStr = this.ViewModel.GetConnectionString();
            string dbname = this.ViewModel.DBName;
            switch (this.ViewModel.DBTypeSelectIndex)
            {
                case 0:
                    this.m_Database = DatabaseMeta.CreateByMysql(connectionStr, dbname);
                    break;
                case 1:
                    this.m_Database = DatabaseMeta.CreateBySqlserver(connectionStr, dbname);
                    break;
                case 2:
                    this.m_Database = DatabaseMeta.CreateBySqlite(connectionStr, dbname);
                    break;
            }
            
            this.DialogResult = true;
        }


        private DatabaseMeta m_Database;
        /// <summary>
        /// 数据库
        /// </summary>
        public DatabaseMeta Database
        {
            get
            {
                return m_Database;
            }
        }
    }
}
