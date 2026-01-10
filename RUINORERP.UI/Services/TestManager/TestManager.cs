using Microsoft.Extensions.Logging;
using RUINORERP.UI.Forms;
using RUINORERP.Common.Helper;
using HLH.Lib.Security;
using System;

namespace RUINORERP.UI.Services.TestManager
{
    /// <summary>
    /// 测试管理器实现类
    /// 统一管理系统中的测试功能
    /// </summary>
    public class TestManager : ITestManager
    {
        private readonly ILogger<TestManager> _logger;
        private readonly bool _showTestButtons;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="showTestButtons">是否显示测试按钮，默认false</param>
        public TestManager(ILogger<TestManager> logger, bool showTestButtons = false)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _showTestButtons = showTestButtons;
        }

        /// <summary>
        /// 显示系统测试窗体
        /// </summary>
        public void ShowSystemTest()
        {
            try
            {
                _logger.LogDebug("打开系统测试窗体");
                
                frmTest testForm = new frmTest();
                testForm.Text = "系统测试";
                // testForm.MdiParent = this; // 如果需要MDI子窗体，需要传递父窗体引用
                testForm.ShowDialog();
                
                _logger.LogDebug("系统测试窗体已关闭");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "显示系统测试窗体失败");
                throw;
            }
        }

        /// <summary>
        /// 显示撤销测试窗体
        /// </summary>
        public void ShowUndoTest()
        {
            try
            {
                _logger.LogDebug("打开撤销测试窗体");
                
                frmTest testForm = new frmTest();
                testForm.Text = "撤销测试";
                testForm.ShowDialog();
                
                _logger.LogDebug("撤销测试窗体已关闭");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "显示撤销测试窗体失败");
                throw;
            }
        }

        /// <summary>
        /// 执行加密解密测试
        /// </summary>
        /// <param name="text">待测试文本</param>
        /// <param name="key">加密密钥</param>
        /// <returns>测试结果字符串</returns>
        public string TestEncryption(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "错误：待测试文本不能为空";
            }

            if (string.IsNullOrEmpty(key))
            {
                return "错误：加密密钥不能为空";
            }

            try
            {
                // 执行加密
                string encrypted = EncryptionHelper.AesEncryptByHashKey(text, key);
                
                // 执行解密
                string decrypted = EncryptionHelper.AesDecryptByHashKey(encrypted, key);
                
                // 验证结果
                bool success = text == decrypted;
                
                // 构造结果字符串
                string result = $"原文: {text}\n";
                result += $"密钥: {key}\n";
                result += $"加密: {encrypted}\n";
                result += $"解密: {decrypted}\n";
                result += $"测试结果: {(success ? "成功 ✓" : "失败 ✗")}";
                
                _logger.LogDebug($"加密测试完成，结果: {(success ? "成功" : "失败")}");
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加密测试失败");
                return $"测试失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 判断是否显示测试按钮
        /// </summary>
        /// <returns>true表示显示，false表示隐藏</returns>
        public bool IsTestButtonsVisible()
        {
            // 可以通过配置文件控制
            // 这里使用构造函数传入的值
            // 也可以从配置文件读取：
            // return _configuration.GetValue<bool>("TestSettings:ShowTestButtons", false);
            
            return _showTestButtons;
        }
    }
}
