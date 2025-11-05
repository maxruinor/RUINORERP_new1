/*************************************************************
 * 文件名：ConfigEncryption.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置加密/解密组件
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RUINORERP.Common.Config.Encryption
{
    /// <summary>
    /// 加密算法类型
    /// </summary>
    public enum EncryptionAlgorithm
    {
        /// <summary>
        /// AES加密算法（默认）
        /// </summary>
        AES,
        
        /// <summary>
        /// 3DES加密算法
        /// </summary>
        TripleDES,
        
        /// <summary>
        /// RSA加密算法（非对称加密）
        /// </summary>
        RSA
    }

    /// <summary>
    /// 配置加密选项
    /// </summary>
    public class ConfigEncryptionOptions
    {
        /// <summary>
        /// 加密算法类型
        /// 默认AES
        /// </summary>
        public EncryptionAlgorithm Algorithm { get; set; } = EncryptionAlgorithm.AES;
        
        /// <summary>
        /// 加密密钥
        /// 注意：生产环境应使用环境变量或安全存储服务提供密钥
        /// </summary>
        public string EncryptionKey { get; set; } = "RUINORERP_DEFAULT_KEY";
        
        /// <summary>
        /// 初始化向量（IV）
        /// 对于AES等算法需要
        /// </summary>
        public string InitializationVector { get; set; } = "RUINORERP_IV_16";
        
        /// <summary>
        /// 加密文本前缀
        /// 用于标识已加密的文本
        /// 默认ENC[
        /// </summary>
        public string EncryptedPrefix { get; set; } = "ENC[";
        
        /// <summary>
        /// 加密文本后缀
        /// 用于标识已加密的文本
        /// 默认]
        /// </summary>
        public string EncryptedSuffix { get; set; } = "]";
        
        /// <summary>
        /// 密钥派生迭代次数
        /// 默认10000
        /// </summary>
        public int KeyDerivationIterations { get; set; } = 10000;
        
        /// <summary>
        /// 是否启用加密
        /// 默认true
        /// </summary>
        public bool Enabled { get; set; } = true;
    }

    /// <summary>
    /// 配置加密器接口
    /// 定义配置加密和解密的基本操作
    /// </summary>
    public interface IConfigEncryptor
    {
        /// <summary>
        /// 加密配置值
        /// </summary>
        /// <param name="plainText">明文配置值</param>
        /// <returns>加密后的配置值</returns>
        string Encrypt(string plainText);
        
        /// <summary>
        /// 解密配置值
        /// </summary>
        /// <param name="encryptedText">加密的配置值</param>
        /// <returns>解密后的明文配置值</returns>
        string Decrypt(string encryptedText);
        
        /// <summary>
        /// 检查文本是否已加密
        /// </summary>
        /// <param name="text">要检查的文本</param>
        /// <returns>是否已加密</returns>
        bool IsEncrypted(string text);
    }

    /// <summary>
    /// 默认配置加密器
    /// 提供配置值的加密和解密功能
    /// </summary>
    public class DefaultConfigEncryptor : IConfigEncryptor
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogger<DefaultConfigEncryptor> _logger;
        
        /// <summary>
        /// 配置加密选项
        /// </summary>
        protected readonly ConfigEncryptionOptions _options;
        
        /// <summary>
        /// 加密文本正则表达式
        /// 用于检测是否为加密文本
        /// </summary>
        protected readonly Regex _encryptedTextRegex;
        
        /// <summary>
        /// 用于AES加密的提供者
        /// </summary>
        protected readonly Aes _aesProvider;
        
        /// <summary>
        /// 用于3DES加密的提供者
        /// </summary>
        protected readonly TripleDES _tripleDesProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="options">配置加密选项</param>
        public DefaultConfigEncryptor(
            ILogger<DefaultConfigEncryptor> logger,
            IOptions<ConfigEncryptionOptions> options = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new ConfigEncryptionOptions();
            
            // 验证并设置加密选项
            ValidateOptions();
            
            // 编译加密文本检测正则表达式
            _encryptedTextRegex = new Regex(
                $"^{Regex.Escape(_options.EncryptedPrefix)}(.+?){Regex.Escape(_options.EncryptedSuffix)}$", 
                RegexOptions.Compiled);
            
            // 初始化加密提供者
            _aesProvider = Aes.Create();
            _tripleDesProvider = TripleDES.Create();
            
            _logger.LogInformation("配置加密器初始化完成，算法: {Algorithm}", _options.Algorithm);
        }

        /// <summary>
        /// 验证加密选项
        /// </summary>
        protected virtual void ValidateOptions()
        {
            if (!_options.Enabled)
            {
                _logger.LogWarning("配置加密已禁用");
                return;
            }
            
            if (string.IsNullOrEmpty(_options.EncryptionKey))
            {
                throw new ArgumentException("加密密钥不能为空", nameof(_options.EncryptionKey));
            }
            
            // 检查默认密钥警告
            if (_options.EncryptionKey == "RUINORERP_DEFAULT_KEY")
            {
                _logger.LogWarning("使用默认加密密钥，生产环境中请更改此密钥");
            }
            
            // 根据算法验证密钥和IV要求
            switch (_options.Algorithm)
            {
                case EncryptionAlgorithm.AES:
                    if (string.IsNullOrEmpty(_options.InitializationVector))
                    {
                        throw new ArgumentException("AES算法需要初始化向量(IV)", nameof(_options.InitializationVector));
                    }
                    break;
                
                case EncryptionAlgorithm.TripleDES:
                    // TripleDES也需要IV
                    if (string.IsNullOrEmpty(_options.InitializationVector))
                    {
                        throw new ArgumentException("TripleDES算法需要初始化向量(IV)", nameof(_options.InitializationVector));
                    }
                    break;
                
                case EncryptionAlgorithm.RSA:
                    // RSA是非对称加密，通常使用密钥对
                    _logger.LogWarning("RSA算法在配置加密中使用有限，建议用于密钥交换");
                    break;
            }
        }

        /// <summary>
        /// 加密配置值
        /// </summary>
        /// <param name="plainText">明文配置值</param>
        /// <returns>加密后的配置值，带有前缀和后缀</returns>
        public string Encrypt(string plainText)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置加密已禁用，返回原文: {PlainText}", plainText);
                return plainText;
            }
            
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            
            // 如果文本已经是加密格式，直接返回
            if (IsEncrypted(plainText))
            {
                _logger.LogDebug("文本已经是加密格式，跳过加密: {PlainText}", plainText);
                return plainText;
            }
            
            _logger.LogDebug("开始加密配置值");
            
            string encryptedData;
            try
            {
                // 根据选择的算法进行加密
                switch (_options.Algorithm)
                {
                    case EncryptionAlgorithm.AES:
                        encryptedData = EncryptAES(plainText);
                        break;
                    case EncryptionAlgorithm.TripleDES:
                        encryptedData = EncryptTripleDES(plainText);
                        break;
                    case EncryptionAlgorithm.RSA:
                        // RSA主要用于密钥交换，这里简化为演示
                        _logger.LogWarning("RSA加密不推荐直接用于配置加密，改用AES");
                        encryptedData = EncryptAES(plainText);
                        break;
                    default:
                        throw new NotSupportedException($"不支持的加密算法: {_options.Algorithm}");
                }
                
                // 添加加密标识前缀和后缀
                string result = $"{_options.EncryptedPrefix}{encryptedData}{_options.EncryptedSuffix}";
                
                _logger.LogDebug("配置值加密成功");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置值加密失败");
                throw;
            }
        }

        /// <summary>
        /// 解密配置值
        /// </summary>
        /// <param name="encryptedText">加密的配置值，可能带有前缀和后缀</param>
        /// <returns>解密后的明文配置值</returns>
        public string Decrypt(string encryptedText)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置加密已禁用，返回原文: {EncryptedText}", encryptedText);
                return encryptedText;
            }
            
            if (string.IsNullOrEmpty(encryptedText))
                return encryptedText;
            
            // 检查是否为加密格式
            if (!IsEncrypted(encryptedText))
            {
                _logger.LogDebug("文本不是加密格式，跳过解密: {EncryptedText}", encryptedText);
                return encryptedText;
            }
            
            // 提取加密数据部分
            string encryptedData = ExtractEncryptedData(encryptedText);
            
            _logger.LogDebug("开始解密配置值");
            
            try
            {
                // 根据选择的算法进行解密
                string decryptedData;
                switch (_options.Algorithm)
                {
                    case EncryptionAlgorithm.AES:
                        decryptedData = DecryptAES(encryptedData);
                        break;
                    case EncryptionAlgorithm.TripleDES:
                        decryptedData = DecryptTripleDES(encryptedData);
                        break;
                    case EncryptionAlgorithm.RSA:
                        // RSA主要用于密钥交换，这里简化为演示
                        _logger.LogWarning("RSA解密不推荐直接用于配置解密，改用AES");
                        decryptedData = DecryptAES(encryptedData);
                        break;
                    default:
                        throw new NotSupportedException($"不支持的加密算法: {_options.Algorithm}");
                }
                
                _logger.LogDebug("配置值解密成功");
                return decryptedData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置值解密失败: {EncryptedData}", encryptedData);
                throw;
            }
        }

        /// <summary>
        /// 检查文本是否已加密
        /// </summary>
        /// <param name="text">要检查的文本</param>
        /// <returns>是否已加密</returns>
        public bool IsEncrypted(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            
            return _encryptedTextRegex.IsMatch(text);
        }

        /// <summary>
        /// 提取加密数据部分
        /// 去掉前缀和后缀
        /// </summary>
        /// <param name="encryptedText">完整的加密文本</param>
        /// <returns>纯加密数据部分</returns>
        protected virtual string ExtractEncryptedData(string encryptedText)
        {
            var match = _encryptedTextRegex.Match(encryptedText);
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            
            // 如果正则表达式匹配失败，尝试手动提取
            if (encryptedText.StartsWith(_options.EncryptedPrefix) && 
                encryptedText.EndsWith(_options.EncryptedSuffix))
            {
                return encryptedText.Substring(
                    _options.EncryptedPrefix.Length, 
                    encryptedText.Length - _options.EncryptedPrefix.Length - _options.EncryptedSuffix.Length);
            }
            
            return encryptedText;
        }

        /// <summary>
        /// 使用AES算法加密文本
        /// </summary>
        /// <param name="plainText">明文文本</param>
        /// <returns>Base64编码的加密文本</returns>
        protected virtual string EncryptAES(string plainText)
        {
            // 从加密选项派生密钥和IV
            using (var keyDerivation = new Rfc2898DeriveBytes(
                _options.EncryptionKey, 
                Encoding.UTF8.GetBytes(_options.InitializationVector), 
                _options.KeyDerivationIterations, 
                HashAlgorithmName.SHA256))
            {
                byte[] keyBytes = keyDerivation.GetBytes(32); // AES-256 密钥
                byte[] ivBytes = keyDerivation.GetBytes(16); // AES 块大小
                
                using (var encryptor = _aesProvider.CreateEncryptor(keyBytes, ivBytes))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    // 将明文写入加密流
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    
                    // 获取加密后的字节
                    byte[] encryptedBytes = memoryStream.ToArray();
                    
                    // 返回Base64编码的加密数据
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        /// <summary>
        /// 使用AES算法解密文本
        /// </summary>
        /// <param name="encryptedData">Base64编码的加密文本</param>
        /// <returns>解密后的明文文本</returns>
        protected virtual string DecryptAES(string encryptedData)
        {
            try
            {
                // 将Base64字符串转换为字节数组
                byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
                
                // 从加密选项派生密钥和IV
                using (var keyDerivation = new Rfc2898DeriveBytes(
                    _options.EncryptionKey, 
                    Encoding.UTF8.GetBytes(_options.InitializationVector), 
                    _options.KeyDerivationIterations, 
                    HashAlgorithmName.SHA256))
                {
                    byte[] keyBytes = keyDerivation.GetBytes(32); // AES-256 密钥
                    byte[] ivBytes = keyDerivation.GetBytes(16); // AES 块大小
                    
                    using (var decryptor = _aesProvider.CreateDecryptor(keyBytes, ivBytes))
                    using (var memoryStream = new MemoryStream(encryptedBytes))
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (var resultStream = new MemoryStream())
                    {
                        // 解密数据
                        cryptoStream.CopyTo(resultStream);
                        
                        // 获取解密后的字节
                        byte[] decryptedBytes = resultStream.ToArray();
                        
                        // 返回UTF8编码的解密文本
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new CryptographicException("无效的Base64编码加密数据", ex);
            }
        }

        /// <summary>
        /// 使用TripleDES算法加密文本
        /// </summary>
        /// <param name="plainText">明文文本</param>
        /// <returns>Base64编码的加密文本</returns>
        protected virtual string EncryptTripleDES(string plainText)
        {
            // 从加密选项派生密钥和IV
            using (var keyDerivation = new Rfc2898DeriveBytes(
                _options.EncryptionKey, 
                Encoding.UTF8.GetBytes(_options.InitializationVector), 
                _options.KeyDerivationIterations, 
                HashAlgorithmName.SHA256))
            {
                byte[] keyBytes = keyDerivation.GetBytes(24); // TripleDES 密钥
                byte[] ivBytes = keyDerivation.GetBytes(8);   // TripleDES 块大小
                
                using (var encryptor = _tripleDesProvider.CreateEncryptor(keyBytes, ivBytes))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    // 将明文写入加密流
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    
                    // 获取加密后的字节
                    byte[] encryptedBytes = memoryStream.ToArray();
                    
                    // 返回Base64编码的加密数据
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        /// <summary>
        /// 使用TripleDES算法解密文本
        /// </summary>
        /// <param name="encryptedData">Base64编码的加密文本</param>
        /// <returns>解密后的明文文本</returns>
        protected virtual string DecryptTripleDES(string encryptedData)
        {
            try
            {
                // 将Base64字符串转换为字节数组
                byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
                
                // 从加密选项派生密钥和IV
                using (var keyDerivation = new Rfc2898DeriveBytes(
                    _options.EncryptionKey, 
                    Encoding.UTF8.GetBytes(_options.InitializationVector), 
                    _options.KeyDerivationIterations, 
                    HashAlgorithmName.SHA256))
                {
                    byte[] keyBytes = keyDerivation.GetBytes(24); // TripleDES 密钥
                    byte[] ivBytes = keyDerivation.GetBytes(8);   // TripleDES 块大小
                    
                    using (var decryptor = _tripleDesProvider.CreateDecryptor(keyBytes, ivBytes))
                    using (var memoryStream = new MemoryStream(encryptedBytes))
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (var resultStream = new MemoryStream())
                    {
                        // 解密数据
                        cryptoStream.CopyTo(resultStream);
                        
                        // 获取解密后的字节
                        byte[] decryptedBytes = resultStream.ToArray();
                        
                        // 返回UTF8编码的解密文本
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new CryptographicException("无效的Base64编码加密数据", ex);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _aesProvider?.Dispose();
                _tripleDesProvider?.Dispose();
                _logger.LogDebug("配置加密器资源已释放");
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~DefaultConfigEncryptor()
        {
            Dispose(false);
        }
    }

    /// <summary>
    /// 无操作配置加密器
    /// 不执行实际的加密和解密操作
    /// 用于开发或测试环境
    /// </summary>
    public class NoOpConfigEncryptor : IConfigEncryptor
    {
        /// <summary>
        /// 加密配置值
        /// 直接返回原文
        /// </summary>
        /// <param name="plainText">明文配置值</param>
        /// <returns>原文</returns>
        public string Encrypt(string plainText)
        {
            return plainText;
        }

        /// <summary>
        /// 解密配置值
        /// 直接返回原文
        /// </summary>
        /// <param name="encryptedText">加密的配置值</param>
        /// <returns>原文</returns>
        public string Decrypt(string encryptedText)
        {
            return encryptedText;
        }

        /// <summary>
        /// 检查文本是否已加密
        /// 始终返回false
        /// </summary>
        /// <param name="text">要检查的文本</param>
        /// <returns>false</returns>
        public bool IsEncrypted(string text)
        {
            return false;
        }
    }

    /// <summary>
    /// 配置加密异常
    /// </summary>
    public class ConfigEncryptionException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigEncryptionException() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public ConfigEncryptionException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public ConfigEncryptionException(string message, Exception innerException) : base(message, innerException) { }
    }

    
}