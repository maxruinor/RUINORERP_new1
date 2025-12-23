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
            TodoUpdate updateData,
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
        /// 检查单据是否匹配条件列表（支持AND/OR混合）
        /// </summary>
        public bool CheckBillMatchesConditions(
            object entity,
            List<IConditionalModel> conditions)
        {
            if (entity == null || conditions == null || !conditions.Any())
                return false;

            // 如果是ConditionalCollections，则按照AND/OR逻辑处理
            if (conditions.Count == 1 && conditions[0] is ConditionalCollections collection)
            {
                return CheckConditionalCollectionsForEntity(entity, collection);
            }

            // 否则按照所有条件都为AND逻辑处理
            var billConditionValues = GetBillConditionValues(entity, conditions);
            return conditions.All(condition => CheckSqlSugarCondition(condition, billConditionValues));
        }


        /// <summary>
        /// 检查实体是否匹配ConditionalCollections（支持AND/OR混合）
        /// </summary>
        private bool CheckConditionalCollectionsForEntity(
            object entity,
            ConditionalCollections collection)
        {
            if (collection?.ConditionalList == null || collection.ConditionalList.Count == 0)
                return false;

            // 获取所有条件值一次
            var billConditionValues = GetBillConditionValues(entity,
                collection.ConditionalList.Select(kv => kv.Value).Cast<IConditionalModel>().ToList());

            // 初始化结果
            bool result = collection.ConditionalList[0].Key == WhereType.And;

            // 逐个条件按照指定的AND/OR运算符进行判断
            foreach (var item in collection.ConditionalList)
            {
                var conditionResult = CheckSqlSugarCondition(item.Value, billConditionValues);

                switch (item.Key)
                {
                    case WhereType.And:
                        result &= conditionResult;
                        break;
                    case WhereType.Or:
                        result |= conditionResult;
                        break;
                }
            }

            return result;
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

            // 简化UI刷新调用
            if (_todoListControl != null)
            {
                try
                {
                    _todoListControl.BeginInvoke(new Action(() => _todoListControl.RefreshDataNodes(update)));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"UI更新委托异常: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 批量处理来自TodoSyncManager的任务状态更新
        /// 高效处理多个单据状态同步到工作台
        /// </summary>
        /// <param name="updates">任务状态更新信息列表</param>
        public void ProcessUpdates(List<TodoUpdate> updates)
        {
            // 空检查
            if (updates == null || updates.Count == 0)
                return;

            // 简化UI刷新调用
            if (_todoListControl != null)
            {
                try
                {
                    _todoListControl.BeginInvoke(new Action(() => _todoListControl.RefreshDataNodes(updates)));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"批量UI更新委托异常: {ex.Message}");
                }
            }
        }


        #endregion

        #region 私有方法


        /// <summary>
        /// 查找匹配条件的状态节点
        /// </summary>
        private void FindMatchingStatusNodes(
            TodoUpdate updateData,
            TreeNode bizTypeNode,
            List<TreeNode> targetNodes)
        {
            foreach (TreeNode statusNode in bizTypeNode.Nodes)
            {
                var parameter = statusNode.Tag as QueryParameter;
                if (parameter == null || parameter.conditionals == null)
                    continue;

                // 检查是否匹配条件，使用entity进行匹配
                if (CheckBillMatchesConditions(updateData.entity, parameter.conditionals))
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

            return false;
        }

        /// <summary>
        /// 获取单据的条件值
        /// 仅从传入的entity中提取conditions所需的属性，避免全量反射
        /// </summary>
        private Dictionary<string, object> GetBillConditionValues(
            object entity,
            List<IConditionalModel> conditions)
        {
            var conditionValues = new Dictionary<string, object>();

            if (entity == null || conditions == null || !conditions.Any())
                return conditionValues;
            if (entity is not BaseEntity)
            {
                return conditionValues;
            }
            var baseEntity = entity as BaseEntity;
            try
            {
                // 仅提取conditions中需要的字段
                foreach (var condition in conditions)
                {
                    if (condition is ConditionalModel sqlCondition)
                    {
                        var fieldName = sqlCondition.FieldName;
                      

                        // 从entity中反射获取对应字段的值
                        if (baseEntity.ContainsProperty(fieldName))
                        {
                            try
                            {
                                conditionValues[fieldName] = baseEntity.GetPropertyValue(fieldName);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"获取字段{fieldName}值失败: {ex.Message}");
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取单据条件值时出错: {ex.Message}");
            }

            return conditionValues;
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


        #endregion
    }
}