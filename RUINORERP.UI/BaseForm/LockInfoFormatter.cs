using System;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 锁定信息格式化工具类
    /// 提供锁定时长计算、状态格式化等公共方法
    /// v2.1.1: 提取公共工具方法,避免代码重复
    /// </summary>
    public static class LockInfoFormatter
    {
        /// <summary>
        /// 计算锁定时长
        /// </summary>
        /// <param name="lockTime">锁定时间</param>
        /// <returns>格式化的锁定时长字符串</returns>
        public static string CalculateLockDuration(DateTime lockTime)
        {
            try
            {
                var duration = DateTime.Now - lockTime;
                if (duration.TotalHours >= 1)
                {
                    return $"{(int)duration.TotalHours}小时{duration.Minutes}分钟";
                }
                else if (duration.TotalMinutes >= 1)
                {
                    return $"{duration.Minutes}分钟";
                }
                else
                {
                    return $"{duration.Seconds}秒";
                }
            }
            catch
            {
                return "未知";
            }
        }

        /// <summary>
        /// 格式化锁定信息用于显示
        /// </summary>
        /// <param name="lockInfo">锁信息</param>
        /// <returns>格式化的锁定信息字符串</returns>
        public static string FormatLockInfoMessage(PacketSpec.Models.Lock.LockInfo lockInfo)
        {
            if (lockInfo == null)
                return "锁信息为空";

            var lockTimeStr = lockInfo.LockTime.ToString("yyyy-MM-dd HH:mm:ss");
            var lockDuration = CalculateLockDuration(lockInfo.LockTime);

            return $"单据已被锁定\n\n" +
                   $"📋 单据编号: {lockInfo.BillNo ?? "未知"}\n" +
                   $"🆔 单据ID: {lockInfo.BillID}\n" +
                   $"👤 锁定用户: {lockInfo.LockedUserName}\n" +
                   $"⏰ 锁定时间: {lockTimeStr}\n" +
                   $"⏱️ 已锁定时长: {lockDuration}\n" +
                   $"💡 提示: 您可以点击按钮请求解锁";
        }
    }
}
