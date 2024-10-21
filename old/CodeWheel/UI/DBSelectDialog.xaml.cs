using CodeWheel.Infrastructure.DB;
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


        

        public DBSelectDialog(Int32 dbTypeSelectIndex)
        {
            
            InitializeComponent();
            this.ViewModel = new DBSelectDialogViewModel();
            this.DataContext = this.ViewModel;
            this.ViewModel.DBType = dbTypeSelectIndex;
            SetDBTypeVisibility(this.ViewModel.DBType);
        }

        /// <summary>
        /// 设置某种数据库可见性
        /// </summary>
        /// <param name="dbType"></param>
        private void SetDBTypeVisibility(Int32 dbType)
        {
            this.ViewModel.MySqlVisibility = Visibility.Hidden;
            this.ViewModel.SQLServerVisibility = Visibility.Hidden;
            this.ViewModel.SQLiteVisibility = Visibility.Hidden;
            switch (dbType)
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

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ViewModel.TestConnectionion())
            {
                MessageBox.Show("选择的数据库连接不上");
                this.DialogResult = false;
                return;
            }
            else
            {
                this.DialogResult = true;
            }
            
            
        }


        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.TestConnectionion())
            {
                MessageBox.Show("连接成功");
            }
            else
            {
                MessageBox.Show("连接失败");
            }

        }


        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.ViewModel.GetConnectionString();
            }
        }
    }
}
