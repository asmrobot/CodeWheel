using CodeWheel.Controls;
using CodeWheel.Dialogs;
using CodeWheel.Model;
using CodeWheel.Model.DB;
using CodeWheel.Utils;
using CodeWheel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeWheel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel ViewModel
        {
            get;set;
        }

        public RazorProvider Provider
        {
            get;set;
        }

        public MainWindow()
        {
            InitializeComponent();
            Provider = new RazorProvider();
            this.ViewModel = new MainWindowViewModel(Provider);
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// 生成收集变量控件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private UserControl CreateElement(VarInfoAttribute info)
        {
            UserControl control = null;
            switch (info.VarType)
            {
                case VarType.SingleString:
                    control = new CWSingleTextControl(info.VarDefault);
                    break;
                case VarType.MultiString:
                    control = new CWMutilTextControl(info.VarDefault);
                    break;
            }

            control.Name = info.VarName;
            this.RegisterName(control.Name, control);
            return control;
        }

        /// <summary>
        /// 清空元素
        /// </summary>
        private void ClearElement()
        {
            //清空原模板变量
            foreach (var item in this._VarPanel.Children)
            {
                UserControl uc = item as UserControl;
                if (uc != null && !string.IsNullOrEmpty(uc.Name))
                {
                    this.UnregisterName(uc.Name);
                }
            }
            this._VarPanel.Children.Clear();
            this._VarPanel.RowDefinitions.Clear();
        }
            

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (this.ViewModel.TemplateSelectIndex >= 0)
            {
                TemplateInfo info = this.ViewModel.TemplateList[this.ViewModel.TemplateSelectIndex];
                if (info == null|| info.Vars==null ||info.Vars.Count<=0)
                {
                    return;
                }
                ClearElement();

                //添加新模板变量
                for (int i = 0; i < info.Vars.Count; i++)
                {
                    RowDefinition def = new RowDefinition();
                    def.Height = new GridLength(30, GridUnitType.Auto);
                    this._VarPanel.RowDefinitions.Add(def);

                    TextBlock title = new TextBlock();
                    title.Text = info.Vars[i].VarTitle;
                    Grid.SetColumn(title, 0);
                    Grid.SetRow(title, i);
                    title.VerticalAlignment = VerticalAlignment.Center;     
                    this._VarPanel.Children.Add(title);

                    UserControl element = CreateElement(info.Vars[i]);
                    Grid.SetColumn(element, 2);
                    Grid.SetRow(element, i);  
                    this._VarPanel.Children.Add(element);
                }                
            }
        }


        private void GenericeCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.TemplateSelectIndex >= 0)
            {
                TemplateInfo info = this.ViewModel.TemplateList[this.ViewModel.TemplateSelectIndex];
                if (info == null || info.Vars == null)
                {
                    return;
                }

                object viewmodel=System.Activator.CreateInstance(info.ViewModelType);
                //向页面传值
                for (int i = 0; i < info.Vars.Count; i++)
                {
                    CWControlBase uc = this.FindName(info.Vars[i].VarName) as CWControlBase;
                    if (uc == null)
                    {
                        info.Vars[i].VarData = null;
                        continue;
                    }
                    PropertyInfo property=info.ViewModelType.GetProperty(info.Vars[i].VarName);
                    if (property == null)
                    {
                        continue;
                    }
                    property.SetValue(viewmodel, uc.GetValue(),null);
                }

                var template = this.Provider.GetTemplate(info.Name);
                if (template == null)
                {
                    return;
                }

                string msg = string.Empty;
                if (template.CreateFiles(out msg,this.Provider.RunService, viewmodel))
                {
                    MessageBox.Show("生成成功");
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.ViewModel.AddTalbe(DateTime.Now.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"已选中表个数:{this.ViewModel.GetSelectedTables().Count.ToString()}");
        }

        /// <summary>
        /// 选择数据库连接字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoiceDB_Click(object sender, RoutedEventArgs e)
        {
            //打开对话框
            DBSelectDialog dialog = new DBSelectDialog(this.ViewModel.DBTypeSelectIndex);

            bool? result = dialog.ShowDialog();
            if (result != null && result.Value == true)
            {
                this.ViewModel.ConnectionString = dialog.ConnectionString;

            }
        }

        /// <summary>
        /// 连接字符串 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectDB_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ViewModel.TestConnectionion())
            {
                MessageBox.Show("选择的数据库连接不上，请重新填写");
                return;
            }

            DatabaseMeta dbMeta = null;
            switch (this.ViewModel.DBTypeSelectIndex)
            {
                case 0:
                    dbMeta = DatabaseMeta.CreateByMysql(this.ViewModel.ConnectionString);
                    break;
                case 1:
                    dbMeta = DatabaseMeta.CreateBySqlserver(this.ViewModel.ConnectionString);
                    break;
                case 2:
                    dbMeta = DatabaseMeta.CreateBySqlite(this.ViewModel.ConnectionString);
                    break;
            }

            foreach (var item in dbMeta.Tables)
            {
                this.ViewModel.Tables.Add(new TableSelectModel() { IsSelected = true, TableName = item.TableName,Meta=item });
            }
        }
    }
}
