using CodeWheel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeWheel
{
    public class ApplicationGlobal
    {
        private ApplicationGlobal()
        {}


        #region 应用程序单实例
        /// <summary>
        /// 应用程序唯一实例
        /// </summary>
        private static Mutex ApplicationSignal = new Mutex(false, "ztimage_codewheel");

        /// <summary>
        /// 得到应用锁
        /// </summary>
        /// <returns></returns>
        public static bool EnsureApplicationSignal()
        {
            if (ApplicationSignal.WaitOne(100))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 释放应用锁
        /// </summary>
        public static void ReleaseApplicationSignal()
        {
            ApplicationSignal.ReleaseMutex();
        }
        #endregion

        private RazorProvider templateProvider;
        /// <summary>
        /// 模板提供器
        /// </summary>
        public RazorProvider TemplateProvider
        {
            get
            {
                if (templateProvider == null)
                {
                    templateProvider = new RazorProvider();
                }
                return templateProvider;
            }
        }

        private StateProvider states;
        /// <summary>
        /// 界面状态提供器
        /// </summary>
        public StateProvider States
        {
            get
            {
                if (states == null)
                {
                    states = new StateProvider();
                }
                return states;
            }
        }


        
        #region 单例
        private static object mInstanceLocker = new object();
        private static ApplicationGlobal mInstance;
        public static ApplicationGlobal Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mInstanceLocker)
                    {
                        if (mInstance == null)
                        {
                            mInstance = new ApplicationGlobal();
                        }
                    }
                }
                return mInstance;
            }
        }
        #endregion




    }
}
