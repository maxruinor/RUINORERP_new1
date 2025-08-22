using AutoMapper;
using MathNet.Numerics.Distributions;
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
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter.DataParts
{
    public partial class UCTodoList : UserControl
    {
        // 依赖注入的服务
        private readonly MenuPowerHelper _menuPowerHelper;
        private readonly BizTypeMapper _mapper;
        private readonly EnhancedBizTypeMapper _Emapper;
        private readonly EntityLoader _loader;
        public UCTodoList(EnhancedBizTypeMapper mapper, EntityLoader loader)
        {
            InitializeComponent();
            _Emapper = mapper;
            _loader = loader;
            // 通过依赖注入获取服务实例
            _menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            _mapper = new BizTypeMapper();
            _conditionBuilderFactory = new ConditionBuilderFactory();
        }
        


        private readonly ConditionBuilderFactory _conditionBuilderFactory;
        public UCTodoList()
        {
            InitializeComponent();
            // 通过依赖注入获取服务实例
            _menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            _mapper = new BizTypeMapper();
            _conditionBuilderFactory = new ConditionBuilderFactory();
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
        private void UCTodoList_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            // 获取当前用户和角色信息
            tb_RoleInfo currentRole = MainForm.Instance.AppContext.CurrentRole;
            tb_UserInfo currentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;

            // 查找匹配的工作中心配置
            CenterConfig = GetWorkCenterConfig(currentRole, currentUser);


            // 构建待办事项树
            BuilderToDoListTreeView();

            // 设置上下文菜单
            kryptonTreeViewJobList.ContextMenuStrip = contextMenuStrip1;
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

        private void kryptonTreeViewJobList_NodeMouseDoubleClickAsync(object sender, TreeNodeMouseClickEventArgs e)
        {
            // e.Node
            //导航到指向的单据界面
            //找到要打开的菜单  订单查询
            if (kryptonTreeViewJobList.SelectedNode != null)
            {
                if (kryptonTreeViewJobList.SelectedNode.Tag is QueryParameter nodeParameter)
                {

                    var RelatedBillMenuInfos = MainForm.Instance.MenuList
                        .Where(m => m.IsVisble && m.EntityName == nodeParameter.tableType.Name && m.ClassPath.Contains("Query")).ToList();
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



                            _menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, instance, nodeParameter);
                            //要卸载，不然会多次执行
                            _menuPowerHelper.OnSetQueryConditionsDelegate -= MenuPowerHelper_OnSetQueryConditionsDelegate;
                            #endregion
                        }
                        else
                        {
                            #region 指向单一的菜单
                            var RelatedBillMenuInfo = RelatedBillMenuInfos.FirstOrDefault();
                            //要把单据信息传过去
                            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(nodeParameter.tableType.Name + "Processor");
                            var QueryConditionFilter = baseProcessor.GetQueryFilter();
                            nodeParameter.queryFilter = QueryConditionFilter;
                            // 创建实例
                            object instance = Activator.CreateInstance(nodeParameter.tableType);
                            _menuPowerHelper.OnSetQueryConditionsDelegate += MenuPowerHelper_OnSetQueryConditionsDelegate;
                            _menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, instance, nodeParameter);
                            //要卸载，不然会多次执行
                            _menuPowerHelper.OnSetQueryConditionsDelegate -= MenuPowerHelper_OnSetQueryConditionsDelegate;
                            #endregion
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
            if (queryDto == null)
            {
                return;
            }

            // 查询条件给值前先将条件清空
            ClearQueryConditions(queryDto, nodeParameter);

            // 设置查询条件值
            SetQueryConditionValues(queryDto, nodeParameter);
        }
        private void ClearQueryConditions(object queryDto, QueryParameter nodeParameter)
        {
            foreach (var item in nodeParameter.queryFilter.QueryFields)
            {
                if (item.FKTableName.IsNotEmptyOrNull() && item.IsRelated)
                {
                    queryDto.SetPropertyValue(item.FieldName, -1L);
                    continue;
                }

                if (item.FieldPropertyInfo.PropertyType.IsGenericType &&
                    item.FieldPropertyInfo.PropertyType.GetBaseType().Name == "DateTime")
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
        }

        private void SetQueryConditionValues(object queryDto, QueryParameter nodeParameter)
        {
            foreach (ConditionalModel item in nodeParameter.conditionals)
            {
                if (item.ConditionalType == ConditionalType.Equal)
                {
                    object value = ConvertFieldValue(item);
                    queryDto.SetPropertyValue(item.FieldName, value);
                }
            }
        }

        private object ConvertFieldValue(ConditionalModel item)
        {
            switch (item.CSharpTypeName)
            {
                case "int":
                    return item.FieldValue.ToInt();
                case "long":
                    return item.FieldValue.ToLong();
                case "bool":
                    return item.FieldValue.ToBool();
                default:
                    return item.FieldValue;
            }
        }

        private void RefreshData_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Owner is ContextMenuStrip contextMenu)
            {
                BuilderToDoListTreeView();
            }
        }


        /// <summary>
        /// 按查询条件  给 Dto对象给到查询条件的值
        /// </summary>
        /// <param name="QueryDto"></param>
        /// <param name="nodeParameter"></param>
        private void MenuPowerHelper_OnSetQueryConditionsDelegate1(object QueryDto, QueryParameter nodeParameter)
        {
            if (QueryDto == null)
            {
                return;
            }
            //查询条件给值前先将条件清空
            foreach (var item in nodeParameter.queryFilter.QueryFields)
            {
                if (item.FKTableName.IsNotEmptyOrNull() && item.IsRelated)
                {
                    QueryDto.SetPropertyValue(item.FieldName, -1L);
                    continue;
                }
                if (item.FieldPropertyInfo.PropertyType.IsGenericType && item.FieldPropertyInfo.PropertyType.GetBaseType().Name == "DateTime")
                {
                    QueryDto.SetPropertyValue(item.FieldName, null);
                    if (QueryDto.ContainsProperty(item.FieldName + "_Start"))
                    {
                        QueryDto.SetPropertyValue(item.FieldName + "_Start", null);
                    }
                    if (QueryDto.ContainsProperty(item.FieldName + "_End"))
                    {
                        QueryDto.SetPropertyValue(item.FieldName + "_End", null);
                    }
                    continue;
                }

            }



            //传入查询对象的实例，
            foreach (ConditionalModel item in nodeParameter.conditionals)
            {
                if (item.ConditionalType == ConditionalType.Equal)
                {
                    switch (item.CSharpTypeName)
                    {
                        case "int":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToInt());
                            break;
                        case "long":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToLong());
                            break;
                        case "bool":
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToBool());
                            break;
                        default:
                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue);
                            break;
                    }
                }
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
            //    Type tableType = _mapper.GetTableType(bizType);
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

            var nodes = await Task.WhenAll(tasks);
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (var node in nodes.Where(n => n != null))
            {
                treeNodes.Add(node);
            }

            kryptonTreeViewJobList.Nodes.AddRange(treeNodes.ToArray());
            kryptonTreeViewJobList.ExpandAll();
        }





        private async Task<TreeNode> ProcessBizTypeNodeAsync(BizType bizType)
        {
            if (bizType==BizType.预收款单)
            {

            }
            //Type tableType = _mapper.GetTableType(bizType);
            Type tableType = _Emapper.GetEntityType(bizType);

            var bizEntity = Activator.CreateInstance(tableType);
            TreeNode parentNode = new TreeNode(bizType.ToString());

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
                    var parameter = new QueryParameter
                    {
                        conditionals = group.Conditions,
                        tableType = tableType,
                        UIPropertyIdentifier = group.Identifier
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
        private async Task<int> AddSpecialStatusNodes(TreeNode node, Type tableType, BizType bizType)
        {
            int Counter = 0;
            switch (bizType)
            {
                case BizType.采购订单:
                    Counter += await AddNode(node, tableType, bizType, GetNotEndConditions(), "待入库");
                    break;

                case BizType.借出单:
                    Counter += await AddNode(node, tableType, bizType, GetNotEndConditions(), "待归还");
                    break;

                case BizType.销售订单:
                    Counter += await AddNode(node, tableType, bizType, GetNotEndConditions(), "待出库");
                    break;

                case BizType.销售出库单:
                    Counter += await AddNode(node, tableType, bizType, GetWaitingPaymentConditions(), "待收款");
                    break;

                case BizType.采购退货单:
                case BizType.返工退库单:
                    Counter += await AddNode(node, tableType, bizType, GetNotCompletedConditions(), "待返回");
                    break;

                case BizType.预收款单:
                    Counter += await AddNode(node, tableType, bizType, GetPrePaymentToBeVerifiedConditions(ReceivePaymentType.收款), "待核销", SharedFlag.Flag1.ToString());
                    break;
                case BizType.预付款单:
                    Counter += await AddNode(node, tableType, bizType, GetPrePaymentToBeVerifiedConditions(ReceivePaymentType.付款), "待核销", SharedFlag.Flag2.ToString());
                    break;

                case BizType.应收款单:
                    Counter += await AddNode(node, tableType, bizType, GetARAPToBePaidConditions(ReceivePaymentType.收款), "待回款", SharedFlag.Flag1.ToString());
                    break;

                case BizType.应付款单:
                    Counter += await AddNode(node, tableType, bizType, GetARAPToBePaidConditions(ReceivePaymentType.付款), "待付款", SharedFlag.Flag2.ToString());
                    break;

                case BizType.收款单:
                    Counter += await AddNode(node, tableType, bizType, GetPaymentToBeConfirmedConditions(ReceivePaymentType.收款), "待支付", SharedFlag.Flag1.ToString());
                    break;
                case BizType.付款单:
                    Counter += await AddNode(node, tableType, bizType, GetPaymentToBeConfirmedConditions(ReceivePaymentType.付款), "待支付", SharedFlag.Flag2.ToString());
                    break;
            }
            return Counter;
        }
        */


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
