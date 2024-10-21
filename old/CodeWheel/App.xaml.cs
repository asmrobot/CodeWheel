using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace CodeWheel
{
    public class Startup
    {
        private static App app; //这才是真正的Application

        [STAThread]
        public static void Main(string[] args)
        {
            //启动数据库
            if (!ApplicationGlobal.EnsureApplicationSignal())
            {
                System.Windows.MessageBox.Show("应用程序已经运行请不要重复运行");
                System.Windows.Application.Current.Shutdown(0);
                return;
            }

            app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            ApplicationGlobal.ReleaseApplicationSignal();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //加载模板
            try
            {
                ApplicationGlobal.Instance.TemplateProvider.Initialize();
            }
            catch
            {
                MessageBox.Show("加载模板失败");
            }

            //加载状态
            try
            {
                ApplicationGlobal.Instance.States.Initialize();
            }
            catch(Exception  ex)
            {
                MessageBox.Show("加载状态失败");
            }
            

            //设为对话框关闭程序关闭
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            MainWindow mainWindow = new MainWindow();
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Current.MainWindow = mainWindow;


            mainWindow.Show();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
        }
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
