using Krypton.Navigator;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.Helper;
using System.Reflection;

namespace RUINORERP.UI.Common
{
    public class UITools
    {
        /// <summary>
        /// 提取重复的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public static List<T> CheckDuplicateData<T>(List<T> dataSource, List<string> includeProperties) where T : class
        {
            List<T> duplicateData = new List<T>();
            try
            {
                // 转换数据源 BindingSortCollection

                if (!dataSource.Any())
                {
                    MessageBox.Show("没有数据可检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return duplicateData;
                }

                // 获取主键名称
                string pkName = UIHelper.GetPrimaryKeyColName(typeof(T));

                // 创建属性访问器缓存
                var propertiesToCompare = typeof(T)
                    .GetProperties()
                    .Where(p => includeProperties.Contains(p.Name) && p.Name != pkName)
                    //.Select(p => new { Property = p, Getter = ExpressionHelper.CreateGetter(p) })
                    .ToList();

                if (!propertiesToCompare.Any())
                {
                    MessageBox.Show("没有可比较的属性", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return duplicateData;
                }

                // 创建属性值获取器
                Func<T, object[]> propertyValuesGetter = entity =>
                {
                    return propertiesToCompare
                        .Select(p => p.GetValue(entity))
                        .ToArray();
                };

                //List<T> dlist = new List<T>();
                //foreach (var item in propertyAccessors)
                //{
                //    dlist.Add(item as T);
                //}
                //====
                // 使用自定义比较器进行分组
                var duplicatesGroups = dataSource
                    .GroupBy(entity => propertyValuesGetter(entity), new ArrayEqualityComparer())
                    .Where(g => g.Count() > 1)
                    .ToList();

                // 收集所有重复项（排除每组的第一个元素）
                foreach (var group in duplicatesGroups)
                {
                    duplicateData.AddRange(group.Skip(1));
                }
                //Func<T, Tuple<object[]>> keySelector2 = p =>
                //{
                //    PropertyInfo[] properties = typeof(T).GetProperties()
                //        .Where(prop => includeProperties.Contains(prop.Name) && prop.Name != pkName)
                //        .ToArray();
                //    var values = properties.Select(prop => prop.GetValue(p)).ToArray();
                //    return Tuple.Create(values);
                //};



                //// 使用自定义比较器进行分组
                //var duplicatesList = dataSource.GroupBy(
                //    keySelector2,
                //    new CustomTupleEqualityComparer<Tuple<object[]>>(includeProperties.ToArray()) // 使用适当的比较器
                //).Where(g => g.Count() > 1)
                // .Select(g => g.Skip(1))//排除掉第一个元素，这个是第一个重复的元素，要保留
                //.SelectMany(g => g)
                //.ToList();
                ////=====


                //var s1 = dataSource.GroupBy(
                //    keySelector2,
                //    new CustomTupleEqualityComparer<Tuple<object[]>>(includeProperties.ToArray()) // 使用适当的比较器
                //)
                //.Select(g => g.Skip(1))//排除掉第一个元素，这个是第一个重复的元素，要保留
                //.ToList();

                //duplicateData = duplicatesList;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"检查过程中发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return duplicateData;
        }


        public static float CalculateTextWidth(string text, Font font, Graphics graphics)
        {
            if (string.IsNullOrEmpty(text) || font == null || graphics == null)
            {
                return 0;
            }
            // 使用Graphics.MeasureString方法计算文本的宽度
            SizeF size = graphics.MeasureString(text, font);
            return size.Width;
        }
        /// <summary>
        /// 缩小图片为缩略图
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image CreateThumbnail(Image image, int width, int height)
        {
            // 创建缩略图的新图像
            Bitmap thumbnail = new Bitmap(width, height);

            // 使用 Graphics 对象绘制缩略图
            using (Graphics graphics = Graphics.FromImage(thumbnail))
            {
                // 设置绘制质量
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 绘制缩略图
                graphics.DrawImage(image, 0, 0, width, height);
            }

            return thumbnail;
        }



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

    // 自定义数组比较器
    public class ArrayEqualityComparer : IEqualityComparer<object[]>
    {
        public bool Equals(object[] x, object[] y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            if (x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (!Equals(x[i], y[i]))
                    return false;
            }
            return true;
        }

        public int GetHashCode(object[] obj)
        {
            unchecked
            {
                int hash = 17;
                foreach (var item in obj)
                {
                    hash = hash * 23 + (item?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }
    }
}

