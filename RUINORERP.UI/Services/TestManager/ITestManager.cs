using System;

namespace RUINORERP.UI.Services.TestManager
{
    /// <summary>
    /// 测试管理器接口
    /// 统一管理系统中的测试功能
    /// </summary>
    public interface ITestManager
    {
        /// <summary>
        /// 显示系统测试窗体
        /// </summary>
        void ShowSystemTest();

        /// <summary>
        /// 显示撤销测试窗体
        /// </summary>
        void ShowUndoTest();

        /// <summary>
        /// 执行加密解密测试
        /// </summary>
        /// <param name="text">待测试文本</param>
        /// <param name="key">加密密钥</param>
        /// <returns>测试结果字符串</returns>
        string TestEncryption(string text, string key);

        /// <summary>
        /// 判断是否显示测试按钮
        /// </summary>
        /// <returns>true表示显示，false表示隐藏</returns>
        bool IsTestButtonsVisible();
    }
}
