using System;
using System.Runtime.InteropServices;

namespace HLH.Lib.OS
{
    public class OSTime
    {
        public static string GetSysBootTimeInfo()
        {
            DateTime dt = DateTime.Now.AddMilliseconds(0 - Environment.TickCount);
            string rs = "开机时间:" + dt.ToString();
            TimeSpan m_WorkTimeTemp = new TimeSpan(Convert.ToInt64(Environment.TickCount) * 10000);
            rs += " 时长:" + m_WorkTimeTemp.Days + "天 " + m_WorkTimeTemp.Hours + "小时 " + m_WorkTimeTemp.Minutes + "分钟 " + m_WorkTimeTemp.Seconds + "秒 ";
            return rs;
        }
        /// <summary>
        /// 获取系统启动时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetComputerStartTime()
        {
            if (Environment.TickCount < 0)
            {
                throw new Exception("超长时间启动，请测试");
            }
            // int result = Environment.TickCount & Int32.MaxValue;     //获取系统启动后运行的毫秒数
            DateTime dt = DateTime.Now.AddMilliseconds(0 - Environment.TickCount);
            return dt;   //返回转换后的时间
        }

        /// <summary> 检索自系统启动以来经过的毫秒数，最多49.7天。 </summary>
        /// <returns>返回值是自系统启动以来经过的毫秒数。DWORD </returns>
        /// <remarks>GetTickCount功能限于系统定时器，这是通常在10毫秒到16毫秒的范围内的分辨率。
        /// GetTickCount函数的分辨率不受GetSystemTimeAdjustment函数所做的调整的影响 。</remarks>
        [DllImport("kernel32")]
        public static extern uint GetTickCount();

        /// <summary> 检索自系统启动以来经过的毫秒数。 </summary>
        /// <returns>毫秒数。ULONGLONG </returns>
        /// <remarks>GetTickCount64功能限于系统定时器，这是通常在10毫秒到16毫秒的范围内的分辨率。
        /// GetTickCount64函数的分辨率不受GetSystemTimeAdjustment函数所做的调整的影响 。</remarks>
        [DllImport("kernel32")]
        public static extern ulong GetTickCount64();

        /// <summary> 检索系统时间，以毫秒为单位。系统时间是自Windows启动以来经过的时间。 </summary>
        /// <returns>返回系统时间（以毫秒为单位）。DWORD</returns>
        /// <remarks>请注意，timeGetTime函数返回的值是DWORD值。该返回值每2 ^ 32毫秒回绕到0，大约49.71天。这会在直接在计算中使用timeGetTime返回值的代码中引起问题，
        /// 尤其是在该值用于控制代码执行的情况下。您应该始终在计算中使用两个timeGetTime返回值之间的差。
        /// timeGetTime函数的默认精度可以是5毫秒或更多，具体取决于机器。您可以使用timeBeginPeriod和timeEndPeriod函数来提高timeGetTime的精度。
        /// 如果这样做，timeGetTime返回的连续值之间的最小差异可能与使用timeBeginPeriod和timeEndPeriod设置的最小周期值一样大。</remarks>
        [DllImport("winmm", EntryPoint = "timeGetTime")]
        public static extern uint TimeGetTime();

        /// <summary> 要求定期定时器的最小分辨率。 </summary>
        /// <param name="uPeriod">应用程序或设备驱动程序的最小计时器分辨率（以毫秒为单位）。较低的值表示较高的（更准确的）分辨率。UINT </param>
        /// <returns>返回TIMERR_NOERROR(0)如果成功或TIMERR_NOCANDO(97)如果在指定的分辨率uPeriod是超出范围。MMRESULT(uint)</returns>
        [DllImport("winmm", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(int uPeriod);

        /// <summary> 清除先前设定的最小计时器分辨率。 </summary>
        /// <param name="uPeriod">在上一次调用timeBeginPeriod函数中指定的最小计时器分辨率。UINT </param>
        /// <returns>返回TIMERR_NOERROR(0)如果成功或TIMERR_NOCANDO(97)如果在指定的分辨率uPeriod是超出范围。MMRESULT(uint)</returns>
        [DllImport("winmm", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(int uPeriod);

        /// <summary> 检索性能计数器的当前值，这是一个高分辨率（小于1us）时间戳，可用于时间间隔测量。 </summary>
        /// <param name="lpPerformanceCount">指向接收当前性能计数器值（以计数为单位）的变量的指针。LARGE_INTEGER </param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。
        /// 在运行Windows XP或更高版本的系统上，该函数将始终成功执行，因此永远不会返回零。</returns>
        [DllImport("kernel32")]
        public static extern short QueryPerformanceCounter(ref long lpPerformanceCount);

        /// <summary> 检索性能计数器的频率。性能计数器的频率在系统启动时是固定的，并且在所有处理器之间均保持一致。
        /// 因此，只需要在应用程序初始化时查询频率，就可以缓存结果。</summary>
        /// <param name="lpFrequency">指向接收当前性能计数器频率的变量的指针，以每秒计数为单位。LARGE_INTEGER </param>
        /// <returns>如果安装的硬件支持高分辨率性能计数器，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。
        /// 在运行Windows XP或更高版本的系统上，该函数将始终成功执行，因此永远不会返回零。</returns>
        [DllImport("kernel32")]
        public static extern short QueryPerformanceFrequency(ref long lpFrequency);
    }
}
