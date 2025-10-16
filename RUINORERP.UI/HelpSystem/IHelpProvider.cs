namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助提供者接口
    /// </summary>
    public interface IHelpProvider
    {
        /// <summary>
        /// 获取帮助页面路径
        /// </summary>
        /// <returns>帮助页面路径</returns>
        string GetHelpPage();

        /// <summary>
        /// 获取帮助页面标题
        /// </summary>
        /// <returns>帮助页面标题</returns>
        string GetHelpTitle();
    }
}