using AutoMapper;
using Castle.Core.Logging;
using MathNet.Numerics.Distributions;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using Netron.GraphLib.Entitology;
using NPOI.SS.Formula.Functions;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.UI.UserCenter.DataParts; // 添加对TodoListManager和BillStatusUpdateData的引用
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
using RUINORERP.UI.FM;
using RulesEngine.Models;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using RUINORERP.UI.BaseForm;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 待办事项列表组件
    /// 集成任务状态实时同步功能
    /// </summary>
    public partial class UCTodoList : UserControl
    {
        // 依赖注入的服务
        private  MenuPowerHelper _menuPowerHelper;
        private readonly IEntityMappingService _mapper;
        private readonly EntityLoader _loader;
        private readonly ILogger<UCTodoList> _logger;
        private Guid _syncSubscriberKey;
        private  ConditionBuilderFactory _conditionBuilderFactory;

        
        public UCTodoList(IEntityMappingService mapper, EntityLoader loader, ILogger<UCTodoList> logger)
        {
            InitializeComponent();
            _logger = logger;
            _mapper = mapper;
            _loader = loader;
            
            // 初始化通用组件
            InitializeCommonComponents();
        }

        public UCTodoList()
        {
            InitializeComponent();
            // 通过依赖注入获取服务实例
            _mapper = Startup.GetFromFac<IEntityMappingService>();
            
            // 初始化通用组件
            InitializeCommonComponents();
        }
        
        /// <summary>
        /// 初始化通用组件和服务
        /// </summary>
        private void InitializeCommonComponents()
        {
            // 获取通用服务
            _menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            _conditionBuilderFactory = new ConditionBuilderFactory();
            
            // 初始化TodoListManager
            InitializeTodoListManager();
        }
        
        /// <summary>
        /// 初始化TodoListManager
        /// </summary>
        private void InitializeTodoListManager()
        {
            try
            {
                // 将当前组件引用传递给TodoListManager，便于其更新UI
                TodoListManager.Instance.SetTodoListControl(this);
                
                if (_logger != null)
                {
                    _logger.LogInformation("TodoListManager初始化完成");
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex, "初始化TodoListManager失败");
                }
            }
        }

        public tb_WorkCenterConfig CenterConfig { get; set; }

        #region 创建一个缓存列表 保存表名和主键字段名的集合
        // 使用 ConcurrentDictionary 实现线程安全的缓存
        private static readonly ConcurrentDictionary<string, HashSet<string>> TablePrimaryKeyCache =
            new ConcurrentDictionary<string, HashSet<string>>();

        // 添加或更新表的主键列表
        public void AddOrUpdateTablePrimaryKeys(string tableName, IEnumerable<string> primaryKeys)
        {
            // 使用线程安全的方式添加或更新缓存项
            TablePrimaryKeyCache.AddOrUpdate(
                tableName,
                key => new HashSet<string>(primaryKeys), // 新增项
                (key, existingSet) =>
                {
                    existingSet.Clear();
                    foreach (var pk in primaryKeys)
                        existingSet.Add(pk);
                    return existingSet;
                });
        }

        public bool DoesTableHaveData(string tableName)
        {
            // 检查是否存在表名且对应主键列表不为空
            return TablePrimaryKeyCache.TryGetValue(tableName, out var primaryKeys)
                && primaryKeys?.Count > 0;
        }
        // 获取表的主键列表
        public IReadOnlyCollection<string> GetPrimaryKeys(string tableName)
        {
            if (TablePrimaryKeyCache.TryGetValue(tableName, out var primaryKeys))
                return primaryKeys;

            // 如果缓存中不存在，从数据源加载并添加到缓存
            primaryKeys = LoadPrimaryKeysFromDatabase(tableName);
            AddOrUpdateTablePrimaryKeys(tableName, primaryKeys);

            return primaryKeys;
        }

        // 从数据库加载主键列表的方法（需自行实现）
        private HashSet<string> LoadPrimaryKeysFromDatabase(string tableName)
        {
            // 实际项目中，这里应该从数据库或其他数据源获取主键列表
            // 示例代码仅作演示
            return new HashSet<string>();
        }

        #endregion
        private async void UCTodoList_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            // 获取当前用户和角色信息
            tb_RoleInfo currentRole = MainForm.Instance.AppContext.CurrentRole;
            tb_UserInfo currentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;

            // 查找匹配的工作中心配置
            CenterConfig = GetWorkCenterConfig(currentRole, currentUser);

            // 初始化同步订阅者
            InitializeSyncSubscriber();

            // 构建待办事项树
            await BuilderToDoListTreeView();

            // 设置上下文菜单
            kryptonTreeViewJobList.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// 初始化同步订阅者
        /// 订阅任务状态更新事件
        /// </summary>
        private void InitializeSyncSubscriber()
        {
            // 生成订阅者唯一标识
            _syncSubscriberKey = Guid.NewGuid();

            // 订阅所有业务类型的更新
            var allBizTypes = Enum.GetValues(typeof(BizType))
                .Cast<BizType>()
                .Where(t => t != BizType.无对应数据)
                .ToList();

            // 注册状态更新回调
            TodoSyncManager.Instance.Subscribe(_syncSubscriberKey, HandleTodoUpdates, allBizTypes);

            // 注册需要监控的业务类型
            // 注意：现在TodoMonitor不再执行数据库轮询，而是完全依赖网络通知
            TodoMonitor.Instance.StartMonitoring(allBizTypes);

            _logger.LogInformation("待办事项列表实时同步已初始化");
        }

        /// <summary>
        /// 处理任务状态更新列表 - 实时同步处理
        /// 从网络接收到任务状态更新时，委托给TodoListManager进行处理
        /// </summary>
        /// <param name="updates">任务状态更新信息列表</param>
        private void HandleTodoUpdates(List<TodoUpdate> updates)
        {
            // 空检查
            if (updates == null || updates.Count == 0)
                return;

            // 确保在UI线程中更新
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<List<TodoUpdate>>(HandleTodoUpdates), updates);
                return;
            }

            // 记录日志
            if (_logger != null)
            {
                _logger.LogTrace($"收到{updates.Count}条任务状态更新");
            }

            // 将所有更新委托给TodoListManager处理
            foreach (var update in updates)
            {
                TodoListManager.Instance.ProcessUpdate(update);
            }
        }

        /// <summary>
        /// 更新任务对应的树节点
        /// 采用本地数据更新方式，避免重复查询数据库
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        private void UpdateTreeNodeForTask(BillStatusUpdateData update)
        {
            if (kryptonTreeViewJobList.Nodes.Count == 0)
                return;

            // 使用BillId字段
            long billId = update.BillId;

            // 日志记录
            _logger.LogTrace($"处理任务更新: 业务类型={update.BusinessType}, 更新类型={update.UpdateType}, " +
                            $"单据ID={billId}, 状态={update.NewStatus}");

            // 查找业务类型节点
            var bizTypeNode = FindBizTypeNode(update.BusinessType);
            if (bizTypeNode == null)
                return;

            // 在UI线程上执行更新操作
            this.Invoke((Action)(() =>
            {
                try
                {
                    // 遍历业务类型节点下的所有状态节点
                    foreach (TreeNode statusNode in bizTypeNode.Nodes)
                    {
                        // 获取节点的QueryParameter
                        var parameter = statusNode.Tag as QueryParameter;
                        if (parameter == null)
                            continue;

                        bool nodeUpdated = false;
                        string primaryKeyFieldName = !string.IsNullOrEmpty(parameter.PrimaryKeyFieldName)
                            ? parameter.PrimaryKeyFieldName : "ID";

                        switch (update.UpdateType)
                        {
                            case TodoUpdateType.Created:
                                // 创建操作：检查该节点的条件是否匹配新增单据，如果匹配则更新
                                if (CheckBillMatchesConditions(billId, update.BusinessType, parameter.conditionals))
                                {
                                    // 在本地数据中添加一条记录（简化处理，实际应添加完整记录）
                                    // 这里主要更新BillIds列表和重新计算数量
                                    if (parameter.BillIds == null)
                                        parameter.BillIds = new List<long>();

                                    if (!parameter.BillIds.Contains(billId))
                                    {
                                        parameter.BillIds.Add(billId);
                                        parameter.IncludeBillIds = true;
                                        nodeUpdated = true;
                                    }
                                }
                                break;

                            case TodoUpdateType.Deleted:
                                // 删除操作：从所有包含该单据的节点中移除
                                if (parameter.BillIds != null && parameter.BillIds.Contains(billId))
                                {
                                    parameter.BillIds.Remove(billId);
                                    parameter.IncludeBillIds = parameter.BillIds.Any();
                                    nodeUpdated = true;

                                    // 如果节点没有单据了，应该移除该节点
                                    if (parameter.BillIds.Count == 0)
                                    {
                                        bizTypeNode.Nodes.Remove(statusNode);
                                        continue; // 继续循环下一个节点
                                    }
                                }
                                break;

                            case TodoUpdateType.StatusChanged:
                                // 状态变化：从原状态节点移除，添加到新状态节点
                                // 这里需要检查该单据是否属于当前节点，如果是则移除
                                if (parameter.BillIds != null && parameter.BillIds.Contains(billId))
                                {
                                    // 从当前节点移除
                                    parameter.BillIds.Remove(billId);
                                    parameter.IncludeBillIds = parameter.BillIds.Any();
                                    nodeUpdated = true;

                                    // 如果节点没有单据了，应该移除该节点
                                    if (parameter.BillIds.Count == 0)
                                    {
                                        bizTypeNode.Nodes.Remove(statusNode);
                                        continue; // 继续循环下一个节点
                                    }
                                }

                                // 尝试查找新状态应该属于哪个节点并添加
                                foreach (TreeNode otherNode in bizTypeNode.Nodes)
                                {
                                    var otherParameter = otherNode.Tag as QueryParameter;
                                    if (otherParameter != null &&
                                        otherParameter.conditionals != null &&
                                        CheckBillMatchesConditions(billId, update.BusinessType, otherParameter.conditionals))
                                    {
                                        if (otherParameter.BillIds == null)
                                            otherParameter.BillIds = new List<long>();

                                        if (!otherParameter.BillIds.Contains(billId))
                                        {
                                            otherParameter.BillIds.Add(billId);
                                            otherParameter.IncludeBillIds = true;

                                            // 更新节点文本
                                            UpdateNodeText(otherNode, otherParameter.BillIds.Count);
                                        }
                                    }
                                }
                                break;
                        }

                        // 如果节点有更新，刷新节点文本显示
                        if (nodeUpdated)
                        {
                            UpdateNodeText(statusNode, parameter.BillIds.Count);
                        }
                    }

                    // 更新业务类型节点的文本显示
                    UpdateBizTypeNodeText(bizTypeNode);

                    // 确保树视图展开
                    kryptonTreeViewJobList.ExpandAll();
                }
                catch (Exception ex)
                {
                    _logger.Error("本地更新任务节点失败", ex);
                }
            }));
        }

        /// <summary>
        /// 检查单据是否匹配指定条件
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="conditions">条件列表</param>
        /// <returns>是否匹配</returns>
        private bool CheckBillMatchesConditions(long billId, BizType bizType, List<IConditionalModel> conditions)
        {
            try
            {
                // 将IConditionalModel列表包装成ConditionGroup列表
                var conditionGroups = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Conditions = conditions,
                        StatusName = "DynamicCondition",
                        Identifier = $"Group_{DateTime.Now.Ticks}"
                    }
                };
                
                // 使用TodoListManager来进行更准确的条件匹配
                return TodoListManager.Instance.CheckBillMatchesConditions(billId, bizType, conditionGroups);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.Error($"检查单据条件匹配失败: BillId={billId}", ex);
                }
                return false;
            }
        }
        
        /// <summary>
        /// 刷新数据节点
        /// 由TodoListManager调用，用于在状态更新后刷新UI节点
        /// </summary>
        /// <param name="updateData">状态更新数据</param>
        public void RefreshDataNodes(BillStatusUpdateData updateData)
        {
            try
            {
                // 验证输入参数
                if (updateData == null || _logger == null)
                    return;
                
                _logger.Info($"刷新数据节点，业务类型：{updateData.BusinessType}，单据ID：{updateData.BillId}");
                
                // 确保在UI线程执行
                if (InvokeRequired)
                {
                    BeginInvoke(new Action<BillStatusUpdateData>(RefreshDataNodes), updateData);
                    return;
                }
                
                // 调用UpdateTreeNodeForTask方法处理更新
                UpdateTreeNodeForTask(updateData);
                
            }
            catch (Exception ex)
            {
                _logger.Error($"刷新数据节点时发生错误，业务类型：{updateData?.BusinessType}，单据ID：{updateData?.BillId}", ex);
            }
        }
        

        /// <summary>
        /// 更新节点文本，显示最新的单据数量
        /// </summary>
        /// <param name="node">要更新的节点</param>
        /// <param name="count">单据数量</param>
        private void UpdateNodeText(TreeNode node, int count)
        {
            // 保留节点原有的状态名称，只更新数量部分
            string nodeText = node.Text;
            int bracketStartIndex = nodeText.LastIndexOf('【');
            if (bracketStartIndex >= 0)
            {
                string statusName = nodeText.Substring(0, bracketStartIndex);
                node.Text = $"{statusName}【{count}】";
            }
            else
            {
                // 如果没有找到括号，直接添加数量
                node.Text = $"{nodeText}【{count}】";
            }
        }

        /// <summary>
        /// 更新业务类型节点的文本，显示所有子节点的总单据数量
        /// </summary>
        /// <param name="node">业务类型节点</param>
        private void UpdateBizTypeNodeText(TreeNode node)
        {
            int totalCount = 0;
            foreach (TreeNode childNode in node.Nodes)
            {
                var parameter = childNode.Tag as QueryParameter;
                if (parameter != null && parameter.BillIds != null)
                {
                    totalCount += parameter.BillIds.Count;
                }
            }

            // 更新业务类型节点文本
            UpdateNodeText(node, totalCount);
        }

        /// <summary>
        /// 查找业务类型对应的树节点
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>树节点，如果未找到返回null</returns>
        private TreeNode FindBizTypeNode(BizType bizType)
        {
            foreach (TreeNode node in kryptonTreeViewJobList.Nodes)
            {
                if (node.Name == bizType.ToString() && node.Text.Contains(bizType.ToString()))
                {
                    return node;
                }
            }
            return null;
        }



        private tb_WorkCenterConfig GetWorkCenterConfig(tb_RoleInfo currentRole, tb_UserInfo currentUser)
        {
            // 先尝试按用户和角色查找配置
            var config = MainForm.Instance.AppContext.WorkCenterConfigList
                .FirstOrDefault(c => c.RoleID == currentRole.RoleID && c.User_ID == currentUser.User_ID);

            if (config != null && !string.IsNullOrEmpty(config.ToDoList))
            {
                return config;
            }

            // 再尝试按角色查找配置
            config = MainForm.Instance.AppContext.WorkCenterConfigList
                .FirstOrDefault(c => c.RoleID == currentRole.RoleID);

            return config ?? new tb_WorkCenterConfig();
        }

        private async void kryptonTreeViewJobList_NodeMouseDoubleClickAsync(object sender, TreeNodeMouseClickEventArgs e)
        {
            // e.Node
            //导航到指向的单据界面
            //找到要打开的菜单  订单查询
            if (kryptonTreeViewJobList.SelectedNode != null)
            {
                if (kryptonTreeViewJobList.SelectedNode.Tag is QueryParameter nodeParameter)
                {

                    var RelatedBillMenuInfos = MainForm.Instance.MenuList
                        .Where(m => m.IsVisble && m.EntityName == nodeParameter.tableType.Name
                        && m.BizType == (int)nodeParameter.bizType
                        && m.ClassPath.Contains("Query")).ToList();
                    if (RelatedBillMenuInfos != null)
                    {
                        if (RelatedBillMenuInfos.Count > 1)
                        {
                            #region 共用的菜单
                            var RelatedBillMenuInfo = RelatedBillMenuInfos.Where(c => c.UIPropertyIdentifier == nodeParameter.UIPropertyIdentifier).FirstOrDefault();
                            //要把单据信息传过去
                            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(nodeParameter.tableType.Name + "Processor");
                            var QueryConditionFilter = baseProcessor.GetQueryFilter();
                            nodeParameter.queryFilter = QueryConditionFilter;
                            // 创建实例
                            object instance = Activator.CreateInstance(nodeParameter.tableType);
                            _menuPowerHelper.OnSetQueryConditionsDelegate += MenuPowerHelper_OnSetQueryConditionsDelegate;



                            // 为了保持一致性，也使用相同的模式
                            _menuPowerHelper.OnSetQueryConditionsDelegate -= MenuPowerHelper_OnSetQueryConditionsDelegate;
                            _menuPowerHelper.OnSetQueryConditionsDelegate += MenuPowerHelper_OnSetQueryConditionsDelegate;
                            await _menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, instance, nodeParameter);
                            #endregion
                        }
                        else if (RelatedBillMenuInfos.Count == 1)
                        {
                            #region 指向单一的菜单
                            var RelatedBillMenuInfo = RelatedBillMenuInfos.FirstOrDefault();
                            //要把单据信息传过去
                            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(nodeParameter.tableType.Name + "Processor");
                            var QueryConditionFilter = baseProcessor.GetQueryFilter();
                            nodeParameter.queryFilter = QueryConditionFilter;
                            // 创建实例
                            object instance = Activator.CreateInstance(nodeParameter.tableType);
                            // 先移除已注册的处理程序（如果有），确保不会重复注册
                            _menuPowerHelper.OnSetQueryConditionsDelegate -= MenuPowerHelper_OnSetQueryConditionsDelegate;
                            // 重新注册事件处理程序
                            _menuPowerHelper.OnSetQueryConditionsDelegate += MenuPowerHelper_OnSetQueryConditionsDelegate;
                            // 执行事件，由于内部使用BeginInvoke，实际执行是异步的
                            await _menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, instance, nodeParameter);

                            #endregion
                        }
                        else if (RelatedBillMenuInfos.Count == 0)
                        {
                            MessageBox.Show("请确定你拥有打开当前菜单的权限！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }




                    }

                }
            }
        }


        /// <summary>
        /// 按查询条件  给 Dto对象给到查询条件的值
        /// </summary>
        private void MenuPowerHelper_OnSetQueryConditionsDelegate(object queryDto, QueryParameter nodeParameter)
        {
            try
            {
                //参数验证
                if (queryDto == null)
                {
                    _logger.Debug("查询Dto对象为空，无法设置查询条件");
                    return;
                }

                if (nodeParameter == null)
                {
                    _logger.Debug("节点参数为空，无法设置查询条件");
                    return;
                }

                // 查询条件给值前先将条件清空
                ClearQueryConditions(queryDto, nodeParameter);

                // 设置查询条件值
                SetQueryConditionValues(queryDto, nodeParameter);

                _logger.Debug($"成功设置查询条件: 表类型={nodeParameter.tableType?.Name}");
            }
            catch (Exception ex)
            {
                _logger.Error("设置查询条件时发生错误", ex);
                //不抛出异常，避免影响UI流程
            }
        }

        private void ClearQueryConditions(object queryDto, QueryParameter nodeParameter)
        {
            if (nodeParameter.queryFilter?.QueryFields == null)
            {
                _logger.Debug("查询过滤器或查询字段集合为空");
                return;
            }

            try
            {
                foreach (var item in nodeParameter.queryFilter.QueryFields)
                {
                    try
                    {
                        if (item.FKTableName.IsNotEmptyOrNull() && item.IsRelated)
                        {
                            queryDto.SetPropertyValue(item.FieldName, -1L);
                            continue;
                        }

                        //打印状态设计得很特殊。int类型用的下拉。
                        if (item.FieldType == QueryFieldType.CmbEnum)
                        {
                            if (item.FieldPropertyInfo != null && item.FieldPropertyInfo.PropertyType.FullName.Contains("System.Int32"))
                            {
                                queryDto.SetPropertyValue(item.FieldName, -1);
                            }
                            if (item.FieldPropertyInfo != null && item.FieldPropertyInfo.PropertyType.FullName.Contains("System.Int64"))
                            {
                                queryDto.SetPropertyValue(item.FieldName, -1L);
                            }
                            if (item.FieldPropertyInfo != null && item.FieldPropertyInfo.PropertyType.Name == "Int32")
                            {
                                queryDto.SetPropertyValue(item.FieldName, -1);
                            }
                            if (item.FieldPropertyInfo != null && item.FieldPropertyInfo.PropertyType.Name == "Int64")
                            {
                                queryDto.SetPropertyValue(item.FieldName, -1L);
                            }


                            continue;
                        }

                        // 修复DateTime类型判断，使用更准确的类型检查
                        if (item.FieldPropertyInfo?.PropertyType != null)
                        {
                            bool isDateTimeType = item.FieldPropertyInfo.PropertyType == typeof(DateTime) ||
                                                (item.FieldPropertyInfo.PropertyType.IsGenericType &&
                                                 item.FieldPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                                                 item.FieldPropertyInfo.PropertyType.GetGenericArguments()[0] == typeof(DateTime));

                            if (isDateTimeType)
                            {
                                queryDto.SetPropertyValue(item.FieldName, null);

                                if (queryDto.ContainsProperty(item.FieldName + "_Start"))
                                {
                                    queryDto.SetPropertyValue(item.FieldName + "_Start", null);
                                }

                                if (queryDto.ContainsProperty(item.FieldName + "_End"))
                                {
                                    queryDto.SetPropertyValue(item.FieldName + "_End", null);
                                }

                                continue;
                            }
                        }

                        // 清空其他类型字段的值
                        queryDto.SetPropertyValue(item.FieldName, null);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn($"清空字段 {item.FieldName} 值时发生错误", ex);
                        //继续处理下一个字段
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("清空查询条件时发生错误", ex);
            }
        }

        private void SetQueryConditionValues(object queryDto, QueryParameter nodeParameter)
        {
            if (nodeParameter.conditionals == null)
            {
                _logger.Debug("条件列表为空，无需设置查询条件值");
                return;
            }

            try
            {
                foreach (ConditionalModel item in nodeParameter.conditionals)
                {
                    try
                    {
                        if (item.ConditionalType == ConditionalType.Equal)
                        {
                            object value = ConvertFieldValue(item);
                            if (queryDto.ContainsProperty(item.FieldName))
                            {
                                queryDto.SetPropertyValue(item.FieldName, value);
                                _logger.Debug($"设置字段 {item.FieldName} 值: {value}");
                            }
                            else
                            {
                                _logger.Warn($"查询Dto对象不包含字段: {item.FieldName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"设置条件字段 {item.FieldName} 值时发生错误", ex);
                        //继续处理下一个条件
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("设置查询条件值时发生错误", ex);
            }
        }

        private object ConvertFieldValue(ConditionalModel item)
        {
            try
            {
                // 参数验证
                if (item == null)
                    return null;

                string fieldValue = item.FieldValue;
                if (string.IsNullOrEmpty(fieldValue))
                    return fieldValue;

                // 使用更健壮的类型转换，处理可能的转换失败
                switch (item.CSharpTypeName)
                {
                    case "int":
                        if (int.TryParse(fieldValue, out int intValue))
                            return intValue;
                        _logger.Warn($"无法将值 {fieldValue} 转换为int类型");
                        return 0;
                    case "long":
                        if (long.TryParse(fieldValue, out long longValue))
                            return longValue;
                        _logger.Warn($"无法将值 {fieldValue} 转换为long类型");
                        return 0L;
                    case "bool":
                        if (bool.TryParse(fieldValue, out bool boolValue))
                            return boolValue;
                        _logger.Warn($"无法将值 {fieldValue} 转换为bool类型");
                        return false;
                    case "DateTime":
                        if (DateTime.TryParse(fieldValue, out DateTime dateValue))
                            return dateValue;
                        _logger.Warn($"无法将值 {fieldValue} 转换为DateTime类型");
                        return null;
                    default:
                        return fieldValue;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("转换字段值时发生错误", ex);
                return item?.FieldValue;
            }
        }

        private async void RefreshData_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Owner is ContextMenuStrip contextMenu)
            {
                await BuilderToDoListTreeView();
            }
        }



        private async Task BuilderToDoListTreeView()
        {
            kryptonTreeViewJobList.Nodes.Clear();
            //TreeNode rootNode = new TreeNode("待办事项") { ImageIndex = 0 };

            var bizTypes = GetConfiguredBizTypes();
            //根据类型查找主键。暂时不需要
            //foreach (var bizType in bizTypes)
            //{
            //    Type tableType = _Business.BizMapperService.EntityMappingHelper.GetEntityType(bizType);
            //    if (!DoesTableHaveData(tableType.Name))
            //    {
            //        string pkfeildName = UIHelper.GetPrimaryKeyColName(tableType);
            //        // 添加缓存项
            //        AddOrUpdateTablePrimaryKeys(tableType.Name, new[] { pkfeildName });
            //    }
            //}

            var tasks = new List<Task<TreeNode>>();

            // 并行处理每个业务类型
            foreach (var bizType in bizTypes)
            {
                tasks.Add(ProcessBizTypeNodeAsync(bizType));
            }

            // 过滤掉可能的null任务，防止ArgumentException异常
            var validTasks = tasks.Where(t => t != null).ToList();
            var nodes = validTasks.Any() ? await Task.WhenAll(validTasks) : Array.Empty<TreeNode>();
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (var node in nodes.Where(n => n != null))
            {
                treeNodes.Add(node);
            }

            kryptonTreeViewJobList.Nodes.AddRange(treeNodes.ToArray());
            kryptonTreeViewJobList.ExpandAll();
        }





        /// <summary>
        /// 处理业务类型节点的异步方法
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>构建的树节点</returns>
        private async Task<TreeNode> ProcessBizTypeNodeAsync(BizType bizType)
        {
            if (bizType == BizType.预收款单)
            {

            }
            Type tableType = _mapper.GetEntityType(bizType);
            if (tableType == null)
            {
                throw new Exception($"工作台中，对应的业务没有注册对应的实体，:{bizType.ToString()}");
            }
            var bizEntity = Activator.CreateInstance(tableType);
            TreeNode parentNode = new TreeNode(bizType.ToString());
            parentNode.Name = bizType.ToString();
            var queryResult = await GetCombinedDataAsync(tableType, bizType, bizEntity);
            if (queryResult.Data == null) return null;

            // 内存中处理状态分组
            ProcessStatusNodes(parentNode, tableType, bizType, queryResult.ConditionGroups, queryResult.Data);

            return parentNode.Nodes.Count > 0 ? parentNode : null;
        }


        /// <summary>
        /// 主要的处理方法
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="bizType"></param>
        /// <param name="bizEntity"></param>
        /// <returns></returns>
        private async Task<(DataTable Data, List<ConditionGroup> ConditionGroups)> GetCombinedDataAsync(
        Type tableType, BizType bizType, object bizEntity)
        {
            var conditionGroups = new List<ConditionGroup>();

            // 公共状态条件（DataStatus相关）
            if (bizEntity.ContainsProperty(typeof(DataStatus).Name))
            {
                conditionGroups.AddRange(_conditionBuilderFactory.GetCommonConditionGroups(bizType));
                //等回
                if (bizType == BizType.采购退货单)
                {
                    conditionGroups.AddRange(_conditionBuilderFactory.GetNeedBackConditionGroups(bizType));
                }
            }

            // 预付款/预收款状态条件
            if (bizEntity.ContainsProperty(typeof(PrePaymentStatus).Name))
            {

                if (bizType == BizType.预付款单)
                {
                    var paymentType = ReceivePaymentType.付款;
                    conditionGroups.AddRange(_conditionBuilderFactory.GetPrePaymentConditionGroups(paymentType));
                }
                else
                {
                    var paymentType = ReceivePaymentType.收款;
                    conditionGroups.AddRange(_conditionBuilderFactory.GetPrePaymentConditionGroups(paymentType));
                }

            }
            //  GetPrePaymentStatusConditions
            // 应收应付状态条件
            if (bizEntity.ContainsProperty(typeof(ARAPStatus).Name))
            {
                if (bizType == BizType.应付款单)
                {
                    var paymentType = ReceivePaymentType.付款;
                    conditionGroups.AddRange(_conditionBuilderFactory.GetARAPConditionGroups(paymentType));
                }
                else
                {
                    var paymentType = ReceivePaymentType.收款;
                    conditionGroups.AddRange(_conditionBuilderFactory.GetARAPConditionGroups(paymentType));
                }
            }


            // 付款/收款状态条件
            if (bizEntity.ContainsProperty(typeof(PaymentStatus).Name))
            {
                if (bizType == BizType.付款单)
                {
                    var paymentType = ReceivePaymentType.付款;
                    conditionGroups.AddRange(_conditionBuilderFactory.GetPaymentConditionGroups(paymentType));
                }
                else
                {
                    var paymentType = ReceivePaymentType.收款;
                    conditionGroups.AddRange(_conditionBuilderFactory.GetPaymentConditionGroups(paymentType));
                }
            }

            if (bizEntity.ContainsProperty(typeof(StatementStatus).Name))
            {
                if (bizType == BizType.对账单)
                {
                    var paymentType = ReceivePaymentType.付款;
                    conditionGroups.AddRange(_conditionBuilderFactory.GetStatementConditionGroups(paymentType));
                }
            }


            // 特殊业务类型条件（示例）
            switch (bizType)
            {
                case BizType.采购订单:
                    conditionGroups.AddRange(_conditionBuilderFactory.GetPurchaseOrderSpecialConditions());
                    break;
                case BizType.销售订单:
                    var grouplist = _conditionBuilderFactory.GetSalesOrderSpecialConditions();

                    conditionGroups.AddRange(grouplist);

                    break;
                case BizType.借出单:
                    conditionGroups.AddRange(_conditionBuilderFactory.GetNeedReturnSpecialConditions("待归还"));
                    break;
                case BizType.返工退库单:
                    conditionGroups.AddRange(_conditionBuilderFactory.GetNeedReturnSpecialConditions("待返回"));
                    break;
                    // 其他业务类型补充...
            }


            // 其他状态条件收集逻辑（根据实际业务补充）
            if (conditionGroups.Count == 0) return (null, null);

            var conModels = BuildConditionalModels(conditionGroups);


            // 最终组合条件
            //var finalCondition = new ConditionalCollections
            //{
            //    ConditionalList = conModels
            //        .Select(c => new KeyValuePair<WhereType, ConditionalModel>(
            //            WhereType.Or,
            //            c))
            //        .ToList()
            //};

            /*
            //  var conModels = new List<IConditionalModel>();
            // 构建组合查询条件（使用OR连接不同状态组:小组第一项目用or，第二项开始就用and）
            foreach (var item in conditionGroups)
            {
                // 每个状态组内部使用AND连接
                // 不同状态组之间用OR连接
                var combinedStatusCondition = new ConditionalCollections();
                List<KeyValuePair<WhereType, ConditionalModel>> ConditionalList = new List<KeyValuePair<WhereType, ConditionalModel>>();
                var conditions = item.Conditions.Cast<ConditionalModel>().ToList();
                for (int i = 0; i < conditions.Count; i++)
                {
                    var conditionalModel = new ConditionalModel()
                    {
                        FieldName = conditions[i].FieldName,
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = conditions[i].FieldValue
                    };
                    if (i == 0)
                    {
                        KeyValuePair<WhereType, ConditionalModel> kv = new KeyValuePair<WhereType, ConditionalModel>(WhereType.Or, conditionalModel);
                        ConditionalList.Add(kv);
                    }
                    else
                    {
                        KeyValuePair<WhereType, ConditionalModel> kv = new KeyValuePair<WhereType, ConditionalModel>(WhereType.And, conditionalModel);
                        ConditionalList.Add(kv);
                    }

                }
                combinedStatusCondition.ConditionalList = ConditionalList;

                //集合中的第一个 or 则接上前面
                //ConditionalList = item.Conditions.Cast<ConditionalModel>().ToList().Select(g =>
                //   new KeyValuePair<WhereType, ConditionalModel>(
                //   whereType,
                //   new ConditionalModel()
                //   {
                //       FieldName = g.FieldName,
                //       ConditionalType = ConditionalType.Equal,
                //       FieldValue = g.FieldValue
                //   }

                //   )).ToList();

                //注意：合并两种状态的。
                conModels.Add(combinedStatusCondition);
            }
            */

            // 执行查询
            //var data = await MainForm.Instance.AppContext.Db.CopyNew()
            //.Queryable(tableType.Name, "TN")
            // .Where(conModels)
            //.ToDataTableAsync();

            //查找主键列各
            string SelectFieldName = "t.*";
            List<string> fields = new List<string>();
            // 获取主键缓存项
            //var primaryKeys = GetPrimaryKeys(tableType.Name);

            //foreach (var item in primaryKeys)
            //{
            //    if (!pks.Contains("t." + item))
            //    {
            //        pks.Add("t." + item);
            //    }
            //}
            foreach (var Conditional in conModels.Cast<ConditionalCollections>().ToList())
            {
                foreach (var item in Conditional.ConditionalList)
                {
                    if (!fields.Contains("t." + item.Value.FieldName))
                    {
                        fields.Add("t." + item.Value.FieldName);
                    }
                }
            }

            //添加查询出主键，因为后面动态更新不会查数据库。要根据操作单据时的ID来判断状态数量
            var entityInfo = RUINORERP.Business.BizMapperService.EntityMappingHelper.GetEntityInfo(bizType);
            if (entityInfo != null && !string.IsNullOrEmpty(entityInfo.IdField))
            {
                string primaryKeyFieldName = entityInfo.IdField;
                fields.Add("t." + primaryKeyFieldName);
            }


            //主键用,连接起来。格式是o.
            SelectFieldName = string.Join(",", fields);


            var data = await MainForm.Instance.AppContext.Db.CopyNew()
            .Queryable<dynamic>("t").AS(tableType.Name)
             .Where(conModels)
             .Select(SelectFieldName)
            .ToDataTableAsync();

            return (data, conditionGroups);
        }


        /// <summary>
        /// 构建组合查询条件（使用OR连接不同状态组: 小组第一项目用or，第二项开始就用and）
        /// 为了减少查询次数。特意将分组的条件 用or全部查出来。最后内存中再分开。
        /// </summary>
        /// <param name="conditionGroups"></param>
        /// <returns></returns>
        private List<IConditionalModel> BuildConditionalModels(List<ConditionGroup> conditionGroups)
        {
            var conModels = new List<IConditionalModel>();
            foreach (var item in conditionGroups)
            {
                var combinedStatusCondition = new ConditionalCollections();
                List<KeyValuePair<WhereType, ConditionalModel>> ConditionalList = new List<KeyValuePair<WhereType, ConditionalModel>>();
                if (item.Conditions.Count > 0)
                {
                    if (item.Conditions[0] is ConditionalCollections)
                    {

                        #region 已经是组合条件

                        for (int i = 0; i < item.Conditions.Count; i++)
                        {
                            #region ConditionalCollections
                            if (item.Conditions[i] is ConditionalCollections)
                            {
                                ConditionalCollections ccs = item.Conditions[i] as ConditionalCollections;
                                for (int c = 0; c < ccs.ConditionalList.Count; c++)
                                {
                                    //如果是第一个条件是。要转为or 因为要将按组的条件 合在一起查
                                    if (ccs.ConditionalList[c].Key == WhereType.And && c == 0)
                                    {
                                        KeyValuePair<WhereType, ConditionalModel> kv = new KeyValuePair<WhereType, ConditionalModel>(WhereType.Or, ccs.ConditionalList[c].Value);
                                        ConditionalList.Add(kv);
                                    }
                                    else
                                    {
                                        KeyValuePair<WhereType, ConditionalModel> kv = new KeyValuePair<WhereType, ConditionalModel>(ccs.ConditionalList[c].Key, ccs.ConditionalList[c].Value);
                                        ConditionalList.Add(kv);
                                    }
                                }
                            }
                            else
                            {
                                KeyValuePair<WhereType, ConditionalModel> kv = new KeyValuePair<WhereType, ConditionalModel>(WhereType.And, item.Conditions[i] as ConditionalModel);
                                ConditionalList.Add(kv);
                            }


                            #endregion


                        }
                        combinedStatusCondition.ConditionalList = ConditionalList;
                        conModels.Add(combinedStatusCondition);
                        #endregion
                    }
                    else
                    {
                        var conditions = item.Conditions.Cast<ConditionalModel>().ToList();
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            var conditionalModel = new ConditionalModel()
                            {
                                FieldName = conditions[i].FieldName,
                                ConditionalType = ConditionalType.Equal,
                                FieldValue = conditions[i].FieldValue
                            };
                            if (i == 0)
                            {
                                ConditionalList.Add(new KeyValuePair<WhereType, ConditionalModel>(WhereType.Or, conditionalModel));
                            }
                            else
                            {
                                ConditionalList.Add(new KeyValuePair<WhereType, ConditionalModel>(WhereType.And, conditionalModel));
                            }
                        }
                        combinedStatusCondition.ConditionalList = ConditionalList;
                        conModels.Add(combinedStatusCondition);

                    }

                }


            }
            return conModels;
        }






        #region new


        private bool CheckRowConditions(DataRow row, List<IConditionalModel> conditions)
        {
            foreach (var condition in conditions)
            {
                if (condition is ConditionalCollections collection)
                {
                    if (!CheckConditionCollection(row, collection))
                        return false;
                }
                else if (condition is ConditionalModel model)
                {
                    if (!CheckSingleCondition(row, model))
                        return false;
                }
            }
            return true;
        }

        private bool CheckConditionCollection(DataRow row, ConditionalCollections collection)
        {
            bool result = collection.ConditionalList[0].Key == WhereType.And;
            foreach (var item in collection.ConditionalList)
            {
                var conditionResult = CheckSingleCondition(row, item.Value as ConditionalModel);
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

        private bool CheckSingleCondition(DataRow row, ConditionalModel model)
        {
            try
            {
                var actualValue = ConvertValue(row[model.FieldName].ToString(), model.CSharpTypeName);
                var expectedValue = ConvertValue(model.FieldValue, model.CSharpTypeName);

                return model.ConditionalType switch
                {
                    ConditionalType.Equal => actualValue.Equals(expectedValue),
                    ConditionalType.GreaterThan => Convert.ToDouble(actualValue) > Convert.ToDouble(expectedValue),
                    // 其他条件类型处理...
                    _ => throw new NotSupportedException($"不支持的查询类型: {model.ConditionalType}")
                };
            }
            catch
            {
                return false;
            }
        }






        #endregion

        /// <summary>
        /// 内存中再分组
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="tableType"></param>
        /// <param name="bizType"></param>
        /// <param name="conditionGroups"></param>
        /// <param name="data"></param>
        private void ProcessStatusNodes(TreeNode parentNode, Type tableType, BizType bizType,
            List<ConditionGroup> conditionGroups, DataTable data)
        {
            foreach (var group in conditionGroups)
            {
                if (BizType.应收款单 == bizType)
                {

                }


                var count = data.AsEnumerable()
                    .Count(row => CheckRowConditions(row, group.Conditions));

                if (count > 0)
                {
                    // 使用BizMapperService获取主键字段名
                    string primaryKeyFieldName = "ID"; // 默认值
                    try
                    {
                        var entityInfo = RUINORERP.Business.BizMapperService.EntityMappingHelper.GetEntityInfo(bizType);
                        if (entityInfo != null && !string.IsNullOrEmpty(entityInfo.IdField))
                        {
                            primaryKeyFieldName = entityInfo.IdField;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"获取业务类型{bizType}的主键字段名时发生错误");
                    }

                    // 收集符合条件的单据主键ID
                    var billIds = data.AsEnumerable()
                        .Where(row => CheckRowConditions(row, group.Conditions))
                        .Select(row => Convert.ToInt64(row[primaryKeyFieldName]))
                        .ToList();

                    // 创建筛选后的数据集合
                    DataTable filteredData = data.Clone();
                    data.AsEnumerable()
                        .Where(row => CheckRowConditions(row, group.Conditions))
                        .CopyToDataTable(filteredData, LoadOption.OverwriteChanges);

                    var parameter = new QueryParameter
                    {
                        bizType = bizType,
                        conditionals = group.Conditions,
                        tableType = tableType,
                        UIPropertyIdentifier = group.Identifier,
                        BillIds = billIds,
                        IncludeBillIds = billIds.Any(),
                        PrimaryKeyFieldName = primaryKeyFieldName,
                        // Data = filteredData // 保存筛选后的数据集合到Data属性
                    };

                    parentNode.Nodes.Add(new TreeNode($"{group.StatusName}【{count}】")
                    {
                        Tag = parameter,
                        Name = $"{tableType.Name}_{bizType}_{group.StatusName}"
                    });
                }
            }
        }





        private object ConvertValue(string value, string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return value;

            return typeName.ToLower() switch
            {
                "int" => int.Parse(value),
                "long" => long.Parse(value),
                "bool" => bool.Parse(value),
                "string" => value,
                _ => Convert.ChangeType(value, Type.GetType($"System.{typeName}"))
            };
        }





        //==
        private List<BizType> GetConfiguredBizTypes()
        {
            List<BizType> bizTypes = new List<BizType>();

            if (CenterConfig != null && !string.IsNullOrEmpty(CenterConfig.ToDoList))
            {
                List<string> toDoItems = CenterConfig.ToDoList.Split(',').ToList();

                foreach (var item in toDoItems)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    //待办事项 ToDoItem = (待办事项)Enum.Parse(typeof(待办事项), item);
                    if (Enum.TryParse<待办事项>(item, out 待办事项 enumValue))
                    {
                        bizTypes.AddRange(MapToDoItemToBizTypes(enumValue));
                    }
                }
            }
            return bizTypes;
        }

        private IEnumerable<BizType> MapToDoItemToBizTypes(待办事项 toDoItem)
        {
            switch (toDoItem)
            {
                case 待办事项.采购_采购订单:
                    yield return BizType.采购订单;
                    break;
                case 待办事项.采购_退款退货处理:
                    yield return BizType.采购退货单;
                    yield return BizType.采购退货入库;
                    break;
                case 待办事项.销售_销售订单:
                    yield return BizType.销售订单;
                    break;
                case 待办事项.销售_销售出库单:
                    yield return BizType.销售出库单;
                    break;
                case 待办事项.销售_退款退货处理:
                    yield return BizType.销售退回单;
                    break;
                case 待办事项.仓库_采购入库单:
                    yield return BizType.采购入库单;
                    break;
                case 待办事项.仓库_盘点单:
                    yield return BizType.盘点单;
                    break;
                case 待办事项.仓库_缴库单:
                    yield return BizType.缴库单;
                    break;
                case 待办事项.仓库_退料单:
                    yield return BizType.生产退料单;
                    break;
                case 待办事项.仓库_领料单:
                    yield return BizType.生产领料单;
                    break;
                case 待办事项.仓库_分割组合:
                    yield return BizType.产品分割单;
                    yield return BizType.产品组合单;
                    yield return BizType.产品转换单;
                    break;
                case 待办事项.生产_计划单:
                    yield return BizType.生产计划单;
                    break;
                case 待办事项.生产_制令单:
                    yield return BizType.制令单;
                    break;
                case 待办事项.财务_费用报销单:
                    yield return BizType.费用报销单;
                    yield return BizType.付款申请单;
                    break;
                case 待办事项.其他_入库单:
                    yield return BizType.其他入库单;
                    break;
                case 待办事项.其他_出库单:
                    yield return BizType.其他出库单;
                    break;
                case 待办事项.请购单:
                    yield return BizType.请购单;
                    break;
                case 待办事项.借出单:
                    yield return BizType.借出单;
                    break;
                case 待办事项.归还单:
                    yield return BizType.归还单;
                    break;
                case 待办事项.返工退库:
                    yield return BizType.返工退库单;
                    yield return BizType.返工入库单;
                    break;
                case 待办事项.损溢费用单:
                    yield return BizType.损失确认单;
                    yield return BizType.溢余确认单;
                    break;
                case 待办事项.预付款单:
                    yield return BizType.预付款单;
                    break;
                case 待办事项.预收款单:
                    yield return BizType.预收款单;
                    break;
                case 待办事项.应付款单:
                    yield return BizType.应付款单;
                    break;
                case 待办事项.应收款单:
                    yield return BizType.对账单;
                    yield return BizType.应收款单;
                    break;
                case 待办事项.对账单:
                    yield return BizType.对账单;
                    break;
                case 待办事项.付款单:
                    yield return BizType.付款单;
                    break;
                case 待办事项.收款单:
                    yield return BizType.收款单;
                    break;
                default:
                    break;
            }
        }

        private void AddSaleLimitedCondition(List<IConditionalModel> conditions)
        {
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conditions.Add(new ConditionalModel
                {
                    FieldName = "Employee_ID",
                    ConditionalType = ConditionalType.Equal,
                    FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(),
                    CSharpTypeName = "long"
                });
            }
        }

        private async Task<int> AddStatusNodes(TreeNode parentNode, Type tableType, BizType bizType, Dictionary<string, List<IConditionalModel>> statusConditions, string UIPropertyIdentifier = "")
        {
            int counter = 0;
            foreach (var status in statusConditions)
            {
                int rows = await AddNode(parentNode, tableType, bizType, status.Value, status.Key, UIPropertyIdentifier);
                counter += rows;
            }
            return counter;
        }

        private async Task<int> AddNode(TreeNode parentNode, Type tableType, BizType bizType, List<IConditionalModel> conditions, string statusText, string UIPropertyIdentifier = "")
        {
            TreeNode subNode = new TreeNode(bizType.ToString());

            // 查询数据
            DataTable queryList = await MainForm.Instance.AppContext.Db.CopyNew()
                .Queryable(tableType.Name, "TN")
                .Where(conditions)

                .ToDataTableAsync();

            if (queryList.Rows.Count > 0)
            {
                // 创建查询参数
                QueryParameter parameter = new QueryParameter
                {
                    conditionals = conditions,
                    tableType = tableType,
                    UIPropertyIdentifier = UIPropertyIdentifier
                };


                string subnodeName = $"{tableType.Name}_{bizType}_{statusText}";
                subNode.Name = subnodeName;
                // 设置节点文本和标签
                subNode.Text = $"{statusText}【{queryList.Rows.Count}】";
                subNode.Tag = parameter;
                if (parameter.UIPropertyIdentifier.Trim().Length > 0)
                {

                }
                // 添加到父节点
                if (!parentNode.Nodes.Contains(subNode))
                {
                    parentNode.Nodes.Add(subNode);
                }
                //else
                //{
                //    parentNode.Nodes.Find(subnodeName, true)[0].Tag = parameter;
                //}
            }
            return queryList.Rows.Count;
        }


        /*

        private List<IConditionalModel> GetNotEndConditions()
        {
            var conditions = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue =((int)DataStatus.确认).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            AddSaleLimitedCondition(conditions);

            return conditions;
        }

        private List<IConditionalModel> GetWaitingPaymentConditions()
        {
            var conditions = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "PayStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            AddSaleLimitedCondition(conditions);

            return conditions;
        }

        private List<IConditionalModel> GetNotCompletedConditions()
        {
            return new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "ApprovalResults", ConditionalType = ConditionalType.Equal, FieldValue = "True", CSharpTypeName = "bool" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };
        }

        private List<IConditionalModel> GetPrePaymentToBeVerifiedConditions(ReceivePaymentType paymentType)
        {
            var conditions = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = PrePaymentStatus.待核销.ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            AddSaleLimitedCondition(conditions);

            return conditions;
        }

        private List<IConditionalModel> GetARAPToBePaidConditions(ReceivePaymentType paymentType)
        {
            var conditions = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((int)ARAPStatus.待支付).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            AddSaleLimitedCondition(conditions);

            return conditions;
        }

        private List<IConditionalModel> GetPaymentToBeConfirmedConditions(ReceivePaymentType paymentType)
        {
            return new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((int)PaymentStatus.待审核).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };
        }

        */

    }


    /*
     var conModels = new List<IConditionalModel>();
 
//name='jack'
conModels.Add(new ConditionalModel{FieldName="name",ConditionalType=ConditionalType.Equal,FieldValue="jack"});  
 ！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
//OR用法: And(id=1 or id=2 and id=3) 
//如果第一个WhereType是OR那么就是 Or(id=1 or id=2 and id=3)
//如果第一个WhereType是And那么就是 And (id=1 or id=2 and id=3)
conModels.Add(new ConditionalCollections()
 {
  ConditionalList = new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>() 
  {
    
   new KeyValuePair<WhereType, ConditionalModel>( 
   WhereType.And,
   new ConditionalModel(){FieldName ="id",ConditionalType=ConditionalType.Equal,FieldValue="1"}),
 
   new KeyValuePair<WhereType, ConditionalModel> (
   WhereType.Or,
   new ConditionalModel() {FieldName ="id",ConditionalType=ConditionalType.Equal,FieldValue="2"}),
 
    
   new KeyValuePair<WhereType, ConditionalModel> ( 
   WhereType.
   And,new ConditionalModel() {FieldName="id",ConditionalType=ConditionalType.Equal,FieldValue="3"})
  }
 });
  
 //name='jack' And ( id=1 or id=2 and id=3 )
 var student = db.Queryable<Student>().Where(conModels).ToList();
     
     */

}
