using System;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// MenuHelper 扩展方法
    /// 用于流程导航图功能中的菜单打开
    /// </summary>
    public static class MenuHelperExtensions
    {
        /// <summary>
        /// 打开菜单（扩展方法）
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="logger">日志记录器</param>
        public static async Task OpenMenuAsync(long menuId, ILogger logger = null)
        {
            try
            {
                // 获取菜单信息
                var MenuInfoCtl = Startup.GetFromFac<RUINORERP.Business.tb_MenuInfoController<tb_MenuInfo>>();
                var menuInfo = await MenuInfoCtl.BaseQueryByIdAsync(menuId);

                if (menuInfo == null)
                {
                    logger?.LogWarning($"菜单ID {menuId} 不存在");
                    return;
                }

                // 使用 MenuPowerHelper 执行菜单事件
                var menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                await menuPowerHelper.ExecuteEvents(menuInfo);

                logger?.LogInformation($"成功打开菜单：{menuInfo.MenuName} (ID: {menuId})");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"打开菜单失败：{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 打开菜单（同步版本）
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="logger">日志记录器</param>
        public static void OpenMenu(long menuId, ILogger logger = null)
        {
            try
            {
                // 异步转同步
                OpenMenuAsync(menuId, logger).Wait();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"打开菜单失败：{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 通过菜单信息打开菜单
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <param name="logger">日志记录器</param>
        public static async Task OpenMenuAsync(tb_MenuInfo menuInfo, ILogger logger = null)
        {
            try
            {
                if (menuInfo == null)
                {
                    logger?.LogWarning("菜单信息为空");
                    return;
                }

                // 使用 MenuPowerHelper 执行菜单事件
                var menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                await menuPowerHelper.ExecuteEvents(menuInfo);

                logger?.LogInformation($"成功打开菜单：{menuInfo.MenuName} (ID: {menuInfo.MenuID})");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"打开菜单失败：{ex.Message}");
                throw;
            }
        }
    }
}