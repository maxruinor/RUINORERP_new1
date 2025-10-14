namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 安全服务接口
    /// 提供加密、解密和验证等安全相关功能
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        string AesEncrypt(string plainText, string key);

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        string AesDecrypt(string cipherText, string key);

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="machineCode">机器码</param>
        /// <param name="registrationCode">注册码</param>
        /// <param name="key">密钥</param>
        /// <returns>验证是否通过</returns>
        bool ValidateRegistrationCode(string machineCode, string registrationCode, string key);
    }
}