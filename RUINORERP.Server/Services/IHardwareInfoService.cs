namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 硬件信息服务接口
    /// 提供系统硬件信息的获取功能
    /// </summary>
    public interface IHardwareInfoService
    {
        /// <summary>
        /// 获取唯一的硬件信息
        /// </summary>
        /// <returns>硬件信息字符串</returns>
        string GetUniqueHardwareInfo();
    }
}