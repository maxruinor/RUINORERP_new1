using HLH.Lib.Security;
using Microsoft.Extensions.Logging;
using System;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 安全服务实现
    /// 提供加密、解密和验证等安全相关功能
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private readonly ILogger<SecurityService> _logger;

        public SecurityService(ILogger<SecurityService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public string AesEncrypt(string plainText, string key)
        {
            try
            {
                return EncryptionHelper.AesEncrypt(plainText, key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AES加密失败");
                throw;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public string AesDecrypt(string cipherText, string key)
        {
            try
            {
                return EncryptionHelper.AesDecrypt(cipherText, key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AES解密失败");
                throw;
            }
        }

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="machineCode">机器码</param>
        /// <param name="registrationCode">注册码</param>
        /// <param name="key">密钥</param>
        /// <returns>验证是否通过</returns>
        public bool ValidateRegistrationCode(string machineCode, string registrationCode, string key)
        {
            try
            {
                return HLH.Lib.Security.SecurityService.ValidateRegistrationCode(registrationCode, key, machineCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注册码验证失败");
                return false;
            }
        }
    }
}