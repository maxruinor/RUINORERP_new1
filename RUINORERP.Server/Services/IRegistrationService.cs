using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Authentication;
using System.Threading.Tasks;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 注册服务接口
    /// 提供系统注册相关的业务逻辑
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// 获取注册信息
        /// </summary>
        /// <returns>注册信息实体</returns>
        Task<tb_sys_RegistrationInfo> GetRegistrationInfoAsync();

        /// <summary>
        /// 保存注册信息
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveRegistrationInfoAsync(tb_sys_RegistrationInfo registrationInfo);

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>机器码</returns>
        string CreateMachineCode(tb_sys_RegistrationInfo registrationInfo);

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="registrationCode">注册码</param>
        /// <returns>验证是否通过</returns>
        Task<bool> ValidateRegistrationAsync(string registrationCode);

        /// <summary>
        /// 检查系统是否已注册
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>是否已注册</returns>
        bool CheckRegistered(tb_sys_RegistrationInfo registrationInfo);

        /// <summary>
        /// 检查注册是否过期
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>是否过期</returns>
        bool IsRegistrationExpired(tb_sys_RegistrationInfo registrationInfo);

        /// <summary>
        /// 获取注册状态（从内存获取）
        /// </summary>
        /// <returns>注册状态</returns>
        Task<RegistrationStatus> GetRegistrationStatusAsync();

        /// <summary>
        /// 检查是否需要到期提醒（从内存判断）
        /// </summary>
        /// <param name="reminderDays">提醒提前天数</param>
        /// <returns>是否需要提醒</returns>
        Task<bool> CheckExpirationReminderAsync(int reminderDays = 30);

        /// <summary>
        /// 获取到期提醒信息（从内存获取）
        /// </summary>
        /// <param name="reminderDays">提醒提前天数</param>
        /// <returns>到期提醒信息</returns>
        Task<ExpirationReminder> GetExpirationReminderInfoAsync(int reminderDays = 30);

        /// <summary>
        /// 更新注册信息缓存（从数据库重新加载到内存）
        /// </summary>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateRegistrationInfoCacheAsync();

        /// <summary>
        /// 验证系统注册状态（统一验证方法）
        /// 此方法整合了所有注册验证逻辑，包括：注册状态、过期检查、用户数限制等
        /// </summary>
        /// <returns>注册验证结果</returns>
        Task<RegistrationValidationResult> ValidateSystemRegistrationAsync();
    }
}