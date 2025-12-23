using RUINORERP.PacketSpec.Models.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 工作台任务更新辅助类
    /// 用于解决List<M>到TodoUpdate的转换问题
    /// </summary>
    public static class TodoHelper
    {
        /// <summary>
        /// 将实体列表转换为TodoUpdate对象列表
        /// </summary>
        /// <typeparam name="M">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        /// <param name="updateType">更新类型</param>
        /// <returns>TodoUpdate对象列表</returns>
        public static List<TodoUpdate> ConvertToTodoUpdates<M>(this List<M> entities, TodoUpdateType updateType) where M : class, new()
        {
            List<TodoUpdate> updates = new List<TodoUpdate>();
            
            if (entities == null || entities.Count == 0)
                return updates;
            
            string bizType = typeof(M).Name;
            
            foreach (var entity in entities)
            {
                try
                {
                    // 获取主键属性
                    var idProp = typeof(M).GetProperty("ID") ?? typeof(M).GetProperty("id");
                    if (idProp != null)
                    {
                        object pkValue = idProp.GetValue(entity);
                        if (pkValue != null)
                        {
                            long billId = Convert.ToInt64(pkValue);
                            if (billId > 0)
                            {
                                // 创建TodoUpdate对象
                                TodoUpdate update = new TodoUpdate
                                {
                                    UpdateType = updateType,
                                    BusinessType = bizType,
                                    BillId = billId
                                };
                                updates.Add(update);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"转换实体为TodoUpdate失败: {ex.Message}");
                }
            }
            
            return updates;
        }
        
        /// <summary>
        /// 处理实体列表的状态更新
        /// </summary>
        /// <typeparam name="M">实体类型</typeparam>
        /// <param name="todoListManager">任务列表管理器</param>
        /// <param name="entities">实体列表</param>
        /// <param name="updateType">更新类型</param>
        public static void ProcessEntityUpdates<M>(this TodoListManager todoListManager, List<M> entities, TodoUpdateType updateType) where M : class, new()
        {
            if (todoListManager == null || entities == null || entities.Count == 0)
                return;
            
            // 转换为TodoUpdate列表
            List<TodoUpdate> updates = ConvertToTodoUpdates(entities, updateType);
            
            // 逐个处理更新
            foreach (var update in updates)
            {
                try
                {
                    todoListManager.ProcessUpdate(update);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"处理任务更新失败: {ex.Message}");
                }
            }
        }
    }
}
