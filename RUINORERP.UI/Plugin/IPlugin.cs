using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 插件接口，所有插件必须实现此接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 插件描述
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// 插件版本
        /// </summary>
        Version Version { get; }
        
        /// <summary>
        /// 初始化插件
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// 执行插件功能
        /// </summary>
        void Execute();
        
        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <returns>插件菜单项</returns>
        ToolStripMenuItem GetMenuItem();
    }
}