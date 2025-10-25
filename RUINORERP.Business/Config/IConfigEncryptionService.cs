using RUINORERP.Model.ConfigModel;
using System.Threading.Tasks;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置加密服务接口
    /// 提供敏感配置的加密和解密功能
    /// </summary>
    public interface IConfigEncryptionService
    {
        /// <summary>
        /// 加密配置对象中的敏感字段
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>加密后的配置对象</returns>
        T EncryptConfig<T>(T config) where T : BaseConfig;

        /// <summary>
        /// 异步加密配置对象中的敏感字段
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>加密后的配置对象</returns>
        Task<T> EncryptConfigAsync<T>(T config) where T : BaseConfig;

        /// <summary>
        /// 解密配置对象中的敏感字段
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>解密后的配置对象</returns>
        T DecryptConfig<T>(T config) where T : BaseConfig;

        /// <summary>
        /// 异步解密配置对象中的敏感字段
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>解密后的配置对象</returns>
        Task<T> DecryptConfigAsync<T>(T config) where T : BaseConfig;

        /// <summary>
        /// 对单个字符串进行加密
        /// </summary>
        /// <param name="value">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        string EncryptValue(string value);

        /// <summary>
        /// 对单个字符串进行解密
        /// </summary>
        /// <param name="value">要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        string DecryptValue(string value);

        /// <summary>
        /// 检查字符串是否已加密
        /// </summary>
        /// <param name="value">要检查的字符串</param>
        /// <returns>是否已加密</returns>
        bool IsValueEncrypted(string value);

        /// <summary>
        /// 生成字段掩码
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="showFirst">显示前几个字符</param>
        /// <param name="showLast">显示后几个字符</param>
        /// <param name="maskChar">掩码字符</param>
        /// <returns>掩码后的字符串</returns>
        string GenerateMask(string value, int showFirst = 2, int showLast = 2, char maskChar = '*');
    }
}
