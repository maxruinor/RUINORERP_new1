using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 工作台任务列表管理器
    /// 负责处理单据状态变更并管理工作台节点更新逻辑
    /// </summary>
    public class TodoListManager
    {
        #region 单例模式
    private static readonly Lazy<TodoListManager> _instance = 
        new Lazy<TodoListManager>(() => new TodoListManager());

    public static TodoListManager Instance => _instance.Value;

    /// <summary>
    /// 私有构造函数，用于单例模式内部实例化
    /// </summary>
    private TodoListManager()
    {
        _conditionBuilderFactory = new ConditionBuilderFactory();
    }

    /// <summary>
    /// 公共构造函数，用于依赖注入容器实例化
    /// </summary>
    public TodoListManager(ConditionBuilderFactory conditionBuilderFactory = null)
    {
        _conditionBuilderFactory = conditionBuilderFactory ?? new ConditionBuilderFactory();
    }
    #endregion

    #region 字段
    private readonly ConditionBuilderFactory _conditionBuilderFactory;
    /// <summary>
    /// 工作台列表控件引用
    /// </summary>
    private UCTodoList _todoListControl;
    #endregion

    #region 公共方法
    /// <summary>
    /// 设置工作台列表控件引用
    /// </summary>
    /// <param name="control">工作台列表控件实例</param>
    public void SetTodoListControl(UCTodoList control)
    {
        _todoListControl = control;
    }
    #endregion

        #region 公共方法

        /// <summary>
        /// 处理单据状态更新并确定其所属工作台节点
        /// </summary>
        /// <param name="updateData">单据状态更新数据</param>
        /// <param name="treeNodes">工作台树节点集合</param>
        /// <returns>应更新的目标节点列表</returns>
        public List<TreeNode> DetermineTargetNodesForUpdate(
            BillStatusUpdateData updateData,
            TreeNodeCollection treeNodes)
        {
            var targetNodes = new List<TreeNode>();

            // 查找对应的业务类型节点
            var bizTypeNode = FindBizTypeNode(updateData.BusinessType, treeNodes);
            if (bizTypeNode == null)
                return targetNodes;

            // 根据更新类型确定处理策略
            switch (updateData.UpdateType)
            {
                case TodoUpdateType.Deleted:
                    // 删除操作：从所有状态节点中移除
                    foreach (TreeNode statusNode in bizTypeNode.Nodes)
                    {
                        if (IsNodeContainsBill(statusNode, updateData.BillId))
                            targetNodes.Add(statusNode);
                    }
                    break;

                case TodoUpdateType.Created:
                case TodoUpdateType.StatusChanged:
                    // 创建或状态变更：查找匹配条件的目标节点
                    FindMatchingStatusNodes(updateData, bizTypeNode, targetNodes);
                    break;
            }

            return targetNodes;
        }

        /// <summary>
        /// 根据单据ID查找对应的树节点
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="treeNodes">树节点集合</param>
        /// <returns>包含该单据的节点列表</returns>
        public List<TreeNode> FindNodesContainingBill(
            long billId,
            BizType businessType,
            TreeNodeCollection treeNodes)
        {
            var containingNodes = new List<TreeNode>();

            var bizTypeNode = FindBizTypeNode(businessType, treeNodes);
            if (bizTypeNode == null)
                return containingNodes;

            foreach (TreeNode statusNode in bizTypeNode.Nodes)
            {
                if (IsNodeContainsBill(statusNode, billId))
                    containingNodes.Add(statusNode);
            }

            return containingNodes;
        }

        /// <summary>
        /// 检查单据是否匹配指定节点的条件
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="queryParameter">查询参数</param>
        /// <returns>是否匹配</returns>
        public bool CheckBillMatchesConditions(
            long billId,
            BizType businessType,
            List<ConditionGroup> conditionGroups)
        {
            if (conditionGroups == null || !conditionGroups.Any())
                return false;

            // 获取单据的条件值
            var billConditionValues = GetBillConditionValues(billId, businessType);

            // 检查是否满足任一条件组
            return conditionGroups.Any(group => CheckConditionGroup(group, billConditionValues));
        }

        /// <summary>
        /// 检查单据是否匹配条件列表（IConditionalModel版本）
        /// </summary>
        public bool CheckBillMatchesConditions(
            long billId,
            BizType businessType,
            List<IConditionalModel> conditions)
        {
            if (conditions == null || !conditions.Any())
                return false;

            // 获取单据的条件值
            var billConditionValues = GetBillConditionValues(billId, businessType);

            // 所有条件必须满足（AND关系）
            return conditions.All(condition => CheckSqlSugarCondition(condition, billConditionValues));
        }

        /// <summary>
        /// 从节点中移除指定单据
        /// </summary>
        /// <param name="node">树节点</param>
        /// <param name="billId">单据ID</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveBillFromNode(TreeNode node, long billId)
        {
            var parameter = node.Tag as QueryParameter;
            if (parameter == null)
                return false;

            // 从BillIds列表中移除
            if (parameter.BillIds != null && parameter.BillIds.Contains(billId))
            {
                parameter.BillIds.Remove(billId);
                node.Text = UpdateNodeTextWithCount(node.Text, parameter.BillIds.Count);
                return true;
            }

            // 如果有Data表，也从表中移除
            //if (parameter.Data is DataTable dataTable)
            //{
            //    string primaryKeyField = !string.IsNullOrEmpty(parameter.PrimaryKeyFieldName) 
            //        ? parameter.PrimaryKeyFieldName : "ID";

            //    DataRow[] rows = dataTable.Select($"{primaryKeyField} = {billId}");
            //    if (rows.Length > 0)
            //    {
            //        foreach (DataRow row in rows)
            //            dataTable.Rows.Remove(row);
                    
            //        dataTable.AcceptChanges();
            //        return true;
            //    }
            //}

            return false;
        }

        /// <summary>
        /// 添加单据到指定节点
        /// </summary>
        /// <param name="node">树节点</param>
        /// <param name="billId">单据ID</param>
        /// <returns>是否添加成功</returns>
        public bool AddBillToNode(TreeNode node, long billId)
        {
            var parameter = node.Tag as QueryParameter;
            if (parameter == null)
                return false;

            // 初始化BillIds列表（如果不存在）
            if (parameter.BillIds == null)
                parameter.BillIds = new List<long>();

            // 避免重复添加
            if (!parameter.BillIds.Contains(billId))
            {
                parameter.BillIds.Add(billId);
                node.Text = UpdateNodeTextWithCount(node.Text, parameter.BillIds.Count);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 处理来自TodoSyncManager的任务状态更新
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        public void ProcessUpdate(TodoUpdate update)
        {
            // 空检查
            if (update == null)
                return;
                
            // 验证和确保状态字段的完整性
            ValidateUpdateStatusFields(update);

            // 创建BillStatusUpdateData对象
            // 使用CreateFromUpdate方法确保OldStatus和NewStatus等所有字段都被正确复制
            var updateData = BillStatusUpdateData.CreateFromUpdate(
                update,
                new BaseEntity()
                // 不传递StatusType参数，因为我们不需要覆盖这个值
                // 让CreateFromUpdate方法自动复制所有状态信息
            );
            
            // 缓存并根据更新类型执行对应操作
            CacheStatusUpdate(updateData);
            
            switch (updateData.UpdateType)
            {
                case TodoUpdateType.Created:
                    HandleCreation(updateData);
                    break;
                case TodoUpdateType.Deleted:
                    HandleDeletion(updateData);
                    break;
                case TodoUpdateType.StatusChanged:
                    HandleStatusChange(updateData);
                    break;
            }
            
            // 简化UI刷新调用
            if (_todoListControl != null)
            {
                try
                {
                    _todoListControl.BeginInvoke(new Action(() => _todoListControl.RefreshDataNodes(updateData)));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"UI更新委托异常: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 缓存状态更新记录
        /// </summary>
        /// <param name="updateData">状态更新数据</param>
        private void CacheStatusUpdate(BillStatusUpdateData updateData)
        {
            // 这里应该实现缓存逻辑，例如使用内存缓存或Redis缓存
            // 为了简化，这里省略具体实现
            // 实际项目中，应该使用项目中的缓存管理框架来实现
        }

        /// <summary>
        /// 处理创建操作
        /// </summary>
        private void HandleCreation(BillStatusUpdateData updateData)
        {
            // 实现创建逻辑
        }

        /// <summary>
        /// 处理删除操作
        /// </summary>
        private void HandleDeletion(BillStatusUpdateData updateData)
        {
            // 实现删除逻辑
        }

        /// <summary>
        /// 处理状态变更操作
        /// 确保原状态节点和新状态节点都能正确更新
        /// </summary>
        private void HandleStatusChange(BillStatusUpdateData updateData)
        {
            // 对于状态变更，我们主要依赖UCtodoList中的RefreshDataNodes方法来处理UI更新
            // 在这里我们只做一些额外的日志记录和状态缓存
            
            // 记录状态变更信息用于调试
            if (!string.IsNullOrEmpty(updateData.OldStatus) && !string.IsNullOrEmpty(updateData.NewStatus))
            {
                System.Diagnostics.Debug.WriteLine($"状态变更: 单据ID={updateData.BillId}, 业务类型={updateData.BusinessType}, 从状态'{updateData.OldStatus}'变更为'{updateData.NewStatus}'");
            }
            
            // 为状态变更更新添加额外信息，确保UCTodoList能正确处理
            // 添加OldStatus和NewStatus到AdditionalData中作为冗余保障
            updateData.AdditionalData["OriginalOldStatus"] = updateData.OldStatus;
            updateData.AdditionalData["OriginalNewStatus"] = updateData.NewStatus;
            
            // 确保所有状态信息都被正确传递到UI更新流程
            // 实际的节点更新逻辑将由UCtodoList中的RefreshDataNodes和UpdateTreeNodeForTask方法处理
        }
        #endregion

        #region 私有方法
        
        /// <summary>
        /// 验证并确保TodoUpdate对象中状态字段的完整性
        /// </summary>
        /// <param name="update">需要验证的TodoUpdate对象</param>
        private void ValidateUpdateStatusFields(TodoUpdate update)
        {
            // 根据更新类型确保状态字段的完整性
            switch (update.UpdateType)
            {
                case TodoUpdateType.Created:
                    // 对于创建操作，确保NewStatus有值
                    if (string.IsNullOrEmpty(update.NewStatus))
                    {
                        update.NewStatus = "新建"; // 设置默认状态
                    }
                    break;
                    
                case TodoUpdateType.StatusChanged:
                    // 对于状态变更，确保OldStatus和NewStatus都有值且不同
                    if (string.IsNullOrEmpty(update.OldStatus))
                    {
                        // 如果没有旧状态，但有新状态，将旧状态设为空字符串表示未知
                        update.OldStatus = "";
                    }
                    
                    if (string.IsNullOrEmpty(update.NewStatus))
                    {
                        // 如果没有新状态，但有旧状态，将新状态设为旧状态
                        // 这是一种保守处理，避免状态丢失
                        update.NewStatus = update.OldStatus;
                    }
                    break;
                    
                case TodoUpdateType.Deleted:
                    // 对于删除操作，确保至少有一个状态字段有值
                    // 如果都没有，则设置为"已删除"
                    if (string.IsNullOrEmpty(update.OldStatus) && string.IsNullOrEmpty(update.NewStatus))
                    {
                        update.OldStatus = "已删除"; // 标记删除前的最后状态
                    }
                    break;
            }
        }

        /// <summary>
        /// 查找匹配条件的状态节点
        /// </summary>
        private void FindMatchingStatusNodes(
            BillStatusUpdateData updateData,
            TreeNode bizTypeNode,
            List<TreeNode> targetNodes)
        {
            foreach (TreeNode statusNode in bizTypeNode.Nodes)
            {
                var parameter = statusNode.Tag as QueryParameter;
                if (parameter == null || parameter.conditionals == null)
                    continue;

                // 检查是否匹配条件
                if (CheckBillMatchesConditions(
                    updateData.BillId,
                    updateData.BusinessType,
                    parameter.conditionals))
                {
                    targetNodes.Add(statusNode);
                }
            }
        }

        /// <summary>
        /// 查找业务类型节点
        /// </summary>
        private TreeNode FindBizTypeNode(BizType businessType, TreeNodeCollection treeNodes)
        {
            // 遍历所有顶级节点查找对应的业务类型节点
            return treeNodes.Cast<TreeNode>()
                .FirstOrDefault(node =>
                {
                    var tag = node.Tag as object[];
                    return tag != null && tag.Length > 0 && tag[0] is BizType && (BizType)tag[0] == businessType;
                });
        }

        /// <summary>
        /// 检查节点是否包含指定单据
        /// </summary>
        private bool IsNodeContainsBill(TreeNode node, long billId)
        {
            var parameter = node.Tag as QueryParameter;
            if (parameter == null)
                return false;

            // 检查BillIds列表
            if (parameter.BillIds != null && parameter.BillIds.Contains(billId))
                return true;

            // 检查Data表
            //if (parameter.Data is DataTable dataTable)
            //{
            //    string primaryKeyField = !string.IsNullOrEmpty(parameter.PrimaryKeyFieldName) 
            //        ? parameter.PrimaryKeyFieldName : "ID";

            //    DataRow[] rows = dataTable.Select($"{primaryKeyField} = {billId}");
            //    return rows.Length > 0;
            //}

            return false;
        }

        /// <summary>
        /// 获取单据的条件值
        /// </summary>
        private Dictionary<string, object> GetBillConditionValues(long billId, BizType businessType)
        {
            // 这里简化处理，实际应用中应该根据业务类型从数据库获取完整的单据信息
            // 并提取条件字段的值
            var conditionValues = new Dictionary<string, object>();

            // 根据业务类型和单据ID查询数据库获取完整信息
            // 此处省略具体实现

            return conditionValues;
        }

        /// <summary>
        /// 检查条件组
        /// </summary>
        private bool CheckConditionGroup(ConditionGroup group, Dictionary<string, object> billValues)
        {
            if (group == null || group.Conditions == null || !group.Conditions.Any())
                return false;

            // 所有条件必须满足（AND关系）
            foreach (var condition in group.Conditions)
            {
                if (!CheckSqlSugarCondition(condition, billValues))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 检查SqlSugar条件
        /// </summary>
        private bool CheckSqlSugarCondition(IConditionalModel condition, Dictionary<string, object> billValues)
        {
            if (condition == null || !(condition is ConditionalModel))
                return false;

            var sqlCondition = condition as ConditionalModel;
            
            // 检查是否存在对应字段的值
            if (!billValues.TryGetValue(sqlCondition.FieldName, out var billValue))
                return false;

            // 获取比较值并转换为适当类型
            var compareValue = ConvertToType(sqlCondition.FieldValue, sqlCondition.CSharpTypeName);

            // 根据条件类型比较
            switch (sqlCondition.ConditionalType)
            {
                case ConditionalType.Equal:
                    return Equals(billValue, compareValue);
                case ConditionalType.NoEqual:
                    return !Equals(billValue, compareValue);
                case ConditionalType.GreaterThan:
                    return CompareValues(billValue, compareValue) > 0;
                case ConditionalType.LessThan:
                    return CompareValues(billValue, compareValue) < 0;
                case ConditionalType.GreaterThanOrEqual:
                    return CompareValues(billValue, compareValue) >= 0;
                case ConditionalType.LessThanOrEqual:
                    return CompareValues(billValue, compareValue) <= 0;
                case ConditionalType.Like:
                    return billValue.ToString().Contains(compareValue.ToString());
                case ConditionalType.In:
                    // 处理In条件的特殊逻辑
                    // 注意：这里假设FieldValue是逗号分隔的字符串或已转换为集合
                    var inValuesStr = compareValue as string;
                    if (inValuesStr != null)
                    {
                        var inValues = inValuesStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        return inValues.Contains(billValue.ToString());
                    }
                    return false;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// 根据CSharpTypeName将字符串值转换为对应类型
        /// </summary>
        private object ConvertToType(string value, string typeName)
        {
            try
            {
                switch (typeName.ToLower())
                {
                    case "int":
                        return int.Parse(value);
                    case "long":
                        return long.Parse(value);
                    case "bool":
                        return bool.Parse(value);
                    case "decimal":
                        return decimal.Parse(value);
                    case "double":
                        return double.Parse(value);
                    case "datetime":
                        return DateTime.Parse(value);
                    default:
                        return value; // 默认返回字符串
                }
            }
            catch
            {
                return value; // 如果转换失败，返回原始值
            }
        }
        

        /// <summary>
        /// 比较两个值
        /// </summary>
        private int CompareValues(object value1, object value2)
        {
            if (value1 == null && value2 == null)
                return 0;
            if (value1 == null)
                return -1;
            if (value2 == null)
                return 1;

            // 尝试转换为IComparable进行比较
            if (value1 is IComparable comparable1 && value1.GetType() == value2.GetType())
                return comparable1.CompareTo(value2);

            // 转换为字符串比较
            return string.Compare(value1.ToString(), value2.ToString(), StringComparison.Ordinal);
        }

        /// <summary>
        /// 更新节点文本显示计数
        /// </summary>
        private string UpdateNodeTextWithCount(string originalText, int count)
        {
            // 移除现有的计数
            int countStartIndex = originalText.IndexOf(" (");
            string baseText = countStartIndex > 0 ? originalText.Substring(0, countStartIndex) : originalText;

            // 添加新的计数
            return $"{baseText} ({count})";
        }
        #endregion
    }
}
