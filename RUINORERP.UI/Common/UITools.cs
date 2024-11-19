using Krypton.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    public class UITools
    {

 

        /// <summary>
        /// 创建或打开一个可等待的计时器对象
        /// </summary>
        /// <param name="lpTimerAttributes"></param>
        /// <param name="bManualReset"></param>
        /// <param name="lpTimerName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int CreateWaitableTimer(int lpTimerAttributes, bool bManualReset, string lpTimerName);

        /// <summary>
        /// 激活指定的等待计时器
        /// </summary>
        /// <param name="hTimer"></param>
        /// <param name="ft"></param>
        /// <param name="lPeriod"></param>
        /// <param name="pfnCompletionRoutine"></param>
        /// <param name="pArgToCompletionRoutine"></param>
        /// <param name="fResume"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        static extern bool SetWaitableTimer(int hTimer, [In] ref long pDueTime, int lPeriod, int pfnCompletionRoutine, int pArgToCompletionRoutine, bool fResume);

        /// <summary>
        /// 等待直到一个或所有指定对象处于信号状态或超时间隔过去
        /// </summary>
        /// <param name="nCount"></param>
        /// <param name="pHandles"></param>
        /// <param name="fWaitAll"></param>
        /// <param name="dwMilliseconds"></param>
        /// <param name="dwWakeMask"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int MsgWaitForMultipleObjects(int nCount, ref int pHandles, bool fWaitAll, int dwMilliseconds, int dwWakeMask);

        /// <summary>
        /// 关闭打开的对象句柄。
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(int hObject);

        /// <summary>
        /// 高精度延时,窗口程序不卡死延时
        /// </summary>
        /// <param name="time">1000微秒 = 1毫秒 ； 1000毫秒 = 1秒</param>
        /// <param name="type">可空:毫秒  0：毫秒  1：微秒  2：秒  3：分  4：小时  5：天</param>
        public static void SuperSleep(int time, int type = 0)
        {
            if (time < 1)
            {
                return;
            }

            int hTimer = 0;
            long Interval = 0;
            int i = 0;

            int INFINITE = -1;
            int QS_ALLINPUT = 255;
            int WAIT_OBJECT_0 = 0;

            if (type == 1)
            {
                Interval = -10 * time;
                hTimer = CreateWaitableTimer(0, true, "WaitableTimer");
                SetWaitableTimer(hTimer, ref Interval, 0, 0, 0, false);

                while (MsgWaitForMultipleObjects(1, ref hTimer, false, INFINITE, QS_ALLINPUT) != WAIT_OBJECT_0)
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                CloseHandle(hTimer);
                return;
            }
            if (type == 0)
            {
                type = 1;
            }
            if (type == 2)
            {
                type = 1000;
            }
            if (type == 3)
            {
                type = 1000 * 60;
            }
            if (type == 4)
            {
                type = 1000 * 60 * 60;
            }
            if (type == 5)
            {
                type = 1000 * 60 * 60 * 24;
            }

            Interval = -10 * time * 1000 * type;
            hTimer = CreateWaitableTimer(0, true, "WaitableTimer");
            SetWaitableTimer(hTimer, ref Interval, 0, 0, 0, false);
            while (MsgWaitForMultipleObjects(1, ref hTimer, false, INFINITE, QS_ALLINPUT) != WAIT_OBJECT_0)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            CloseHandle(hTimer);
        }

        public static ComPareResult EasyComPare<T>(T t, T s)
        {
            ComPareResult result = new ComPareResult();
            var comparer = new ObjectsComparer.Comparer<T>();
            IEnumerable<ObjectsComparer.Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                //string msg = string.Empty;
                //var listdiff = differences.ToList();
                //foreach (var item in listdiff)
                //{
                //    msg += "旧值：" + item.Value1 + " => 新值：" + item.Value2;
                //    msg += "\r\n";
                //}
                //// string differencesMsg = string.Join(Environment.NewLine, differences);
                //result.Msg = msg.Trim();
            }
            return result;
        }


        public static ComPareResult ComPare<T>(T t, T s)
        {
            ComPareResult result = new ComPareResult();
            var comparer = new ObjectsComparer.Comparer<T>();
            IEnumerable<ObjectsComparer.Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                //string msg = string.Empty;
                //var listdiff = differences.ToList();
                //foreach (var item in listdiff)
                //{
                //    msg += "旧值：" + item.Value1 + " => 新值：" + item.Value2;
                //    msg += "\r\n";
                //}
                //// string differencesMsg = string.Join(Environment.NewLine, differences);
                //result.Msg = msg.Trim();
            }
            return result;
        }

        public static ComPareResult ObjectComparer<T>(T t, T s)
        {
            ComPareResult result = new ComPareResult();

            var comparer = new ObjectsComparer.Comparer<T>();
            IEnumerable<ObjectsComparer.Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                //string msg = string.Empty;
                //var listdiff = differences.ToList();
                //foreach (var item in listdiff)
                //{
                //    msg += "旧值：" + item.Value1 + " => 新值：" + item.Value2;
                //    msg += "\r\n";
                //}
                //// string differencesMsg = string.Join(Environment.NewLine, differences);
                //result.Msg = msg.Trim();
            }
            return result;
        }

        public static ComPareResult ComPareRetunMsg<T>(T t, T s)
        {
            ComPareResult result = new ComPareResult();
            var comparer = new ObjectsComparer.Comparer<T>();
            IEnumerable<ObjectsComparer.Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                string msg = string.Empty;
                var listdiff = differences.ToList();
                foreach (var item in listdiff)
                {
                    msg += "旧值：" + item.Value1 + " => 新值：" + item.Value2;
                    msg += "\r\n";
                }
                // string differencesMsg = string.Join(Environment.NewLine, differences);
                result.Msg = msg.Trim();
            }
            return result;
        }
    }

    public class ComPareResult
    {
        public bool IsEqual { get; set; }
        public string Msg { get; set; }

    }
}
