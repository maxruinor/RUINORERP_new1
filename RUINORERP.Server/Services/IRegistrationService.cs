using RUINORERP.Model;
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

 
    }
}