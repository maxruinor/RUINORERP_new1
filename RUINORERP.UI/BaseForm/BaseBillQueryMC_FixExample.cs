using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// BaseBillQueryMC文件修复示例
    /// 展示如何使用TodoHelper修复CS1503错误
    /// </summary>
    public static class BaseBillQueryMC_FixExample
    {
        /// <summary>
        /// 修复提交菜单项处理代码示例
        /// 替换错误代码: TodoListManager.ProcessUpdate(selectlist);
        /// </summary>
        /// <typeparam name="M">实体类型</typeparam>
        /// <param name="todoListManager">任务列表管理器实例</param>
        /// <param name="selectlist">选中的实体列表</param>
        public static void FixSubmitMenuItem<M>(TodoListManager todoListManager, List<M> selectlist) where M : class, new()
        {
            // 修复后的代码 - 使用辅助方法自动处理类型转换
            if (todoListManager != null && selectlist != null && selectlist.Count > 0)
            {
                todoListManager.ProcessEntityUpdates(selectlist, TodoUpdateType.StatusChanged);
            }
        }

        /// <summary>
        /// 修复审核菜单项处理代码示例
        /// 替换错误代码: TodoListManager.ProcessUpdate(selectlist);
        /// </summary>
        /// <typeparam name="M">实体类型</typeparam>
        /// <param name="todoListManager">任务列表管理器实例</param>
        /// <param name="selectlist">选中的实体列表</param>
        public static void FixAuditMenuItem<M>(TodoListManager todoListManager, List<M> selectlist) where M : class, new()
        {
            // 修复后的代码 - 使用辅助方法自动处理类型转换
            if (todoListManager != null && selectlist != null && selectlist.Count > 0)
            {
                todoListManager.ProcessEntityUpdates(selectlist, TodoUpdateType.Approved);
            }
        }

        /// <summary>
        /// 修复反审菜单项处理代码示例
        /// 替换错误代码: TodoListManager.ProcessUpdate(selectlist);
        /// </summary>
        /// <typeparam name="M">实体类型</typeparam>
        /// <param name="todoListManager">任务列表管理器实例</param>
        /// <param name="selectlist">选中的实体列表</param>
        public static void FixUnauditMenuItem<M>(TodoListManager todoListManager, List<M> selectlist) where M : class, new()
        {
            // 修复后的代码 - 使用辅助方法自动处理类型转换
            if (todoListManager != null && selectlist != null && selectlist.Count > 0)
            {
                todoListManager.ProcessEntityUpdates(selectlist, TodoUpdateType.StatusChanged);
            }
        }

        /// <summary>
        /// 修复结案菜单项处理代码示例
        /// 替换错误代码: TodoListManager.ProcessUpdate(selectlist);
        /// </summary>
        /// <typeparam name="M">实体类型</typeparam>
        /// <param name="todoListManager">任务列表管理器实例</param>
        /// <param name="selectlist">选中的实体列表</param>
        public static void FixCloseBillMenuItem<M>(TodoListManager todoListManager, List<M> selectlist) where M : class, new()
        {
            // 修复后的代码 - 使用辅助方法自动处理类型转换
            if (todoListManager != null && selectlist != null && selectlist.Count > 0)
            {
                todoListManager.ProcessEntityUpdates(selectlist, TodoUpdateType.StatusChanged);
            }
        }

        /// <summary>
        /// 修复删除菜单项处理代码示例
        /// 替换错误代码: TodoListManager.ProcessUpdate(selectlist);
        /// </summary>
        /// <typeparam name="M">实体类型</typeparam>
        /// <param name="todoListManager">任务列表管理器实例</param>
        /// <param name="selectlist">选中的实体列表</param>
        public static void FixDeleteMenuItem<M>(TodoListManager todoListManager, List<M> selectlist) where M : class, new()
        {
            // 修复后的代码 - 使用辅助方法自动处理类型转换
            if (todoListManager != null && selectlist != null && selectlist.Count > 0)
            {
                todoListManager.ProcessEntityUpdates(selectlist, TodoUpdateType.Deleted);
            }
        }
    }
}
