using AutoMapper;
using Netron.GraphLib;
using Netron.GraphLib.Entitology;
using NPOI.SS.Formula.Functions;
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
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter.DataParts
{
    public partial class UCTodoList : UserControl
    {
        // 依赖注入的服务
        private readonly MenuPowerHelper _menuPowerHelper;
        private readonly BizTypeMapper _mapper;
        public UCTodoList()
        {
            InitializeComponent();
            // 通过依赖注入获取服务实例
            _menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            _mapper = new BizTypeMapper();
        }

        public tb_WorkCenterConfig CenterConfig { get; set; }
        private void UCTodoList_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

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



        /// <summary>
        /// 待办事项
        /// </summary>
        private async void BuilderToDoListTreeView()
        {
            kryptonTreeViewJobList.Nodes.Clear();

            // 创建根节点
            TreeNode rootNode = new TreeNode("待办事项") { ImageIndex = 0 };

            // 获取配置的待办事项类型
            List<BizType> bizTypes = GetConfiguredBizTypes();
            // 为每种业务类型构建节点
            foreach (var bizType in bizTypes)
            {
                TreeNode node = new TreeNode(bizType.ToString());
                Type tableType = _mapper.GetTableType(bizType);

                var BizEntity = Activator.CreateInstance(tableType);

                // 根据表类型添加不同状态的子节点
                if (BizEntity.ContainsProperty(typeof(DataStatus).Name))
                {
                    await AddStatusNodes(node, tableType, bizType, GetCommonStatusConditions());
                }
                else if (BizEntity.ContainsProperty(typeof(PrePaymentStatus).Name))
                {
                    if (bizType == BizType.预付款单)
                    {
                        await AddStatusNodes(node, tableType, bizType, GetPrePaymentStatusConditions(ReceivePaymentType.付款));
                    }
                    if (bizType == BizType.预收款单)
                    {
                        await AddStatusNodes(node, tableType, bizType, GetPrePaymentStatusConditions(ReceivePaymentType.收款));
                    }

                }
                else if (BizEntity.ContainsProperty(typeof(ARAPStatus).Name))
                {
                    if (bizType == BizType.应付款单)
                    {
                        await AddStatusNodes(node, tableType, bizType, GetARAPStatusConditions(ReceivePaymentType.付款));
                    }
                    if (bizType == BizType.应收款单)
                    {
                        await AddStatusNodes(node, tableType, bizType, GetARAPStatusConditions(ReceivePaymentType.收款));
                    }
                }
                else if (BizEntity.ContainsProperty(typeof(PaymentStatus).Name))
                {
                    if (bizType == BizType.付款单)
                    {
                        await AddStatusNodes(node, tableType, bizType, GetPaymentStatusConditions(ReceivePaymentType.付款));
                    }
                    if (bizType == BizType.收款单)
                    {
                        await AddStatusNodes(node, tableType, bizType, GetPaymentStatusConditions(ReceivePaymentType.收款));
                    }
                }

                // 添加特定业务类型的特殊状态节点
                await AddSpecialStatusNodes(node, tableType, bizType);

                // 如果有子节点，添加到根节点
                if (node.Nodes.Count > 0)
                {
                    rootNode.Nodes.Add(node);
                }
            }

            // 将根节点的子节点直接添加到树视图
            foreach (TreeNode item in rootNode.Nodes)
            {
                kryptonTreeViewJobList.Nodes.Add(item);
            }

            // 展开所有节点
            kryptonTreeViewJobList.ExpandAll();

            return;



            TreeNode nd = new TreeNode("待办事项") { ImageIndex = 0 };
            #region 通常业务的 未提交 未审核
            var conModel未审核 = new List<IConditionalModel>();
            conModel未审核.Add(new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "0", CSharpTypeName = "int" }); //设置类型 和C#名称一样常用的支持
            conModel未审核.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "2", CSharpTypeName = "int" });
            // conModel未审核.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool", FieldValueConvertFunc = ConvertStringToBoolean });
            //这里的类型转换只要符合后，下面还会有一个case判断转换。将FieldValue.Tolong()
            conModel未审核.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });

            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel未审核.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }

            var conModel未提交 = new List<IConditionalModel>();
            conModel未提交.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel未提交.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModel未提交.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });


            #endregion

            #region 财务模块的 未提交 未审核

            #region 预收付款
            var conModelFM预收付款未审核 = new List<IConditionalModel>();
            conModelFM预收付款未审核.Add(new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.草稿).ToString(), CSharpTypeName = "long" });
            conModelFM预收付款未审核.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModelFM预收付款未审核.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }

            var conModelFM预收付款未提交 = new List<IConditionalModel>();
            conModelFM预收付款未提交.Add(new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModelFM预收付款未提交.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModelFM预收付款未提交.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });

            #endregion

            #region 应收付款
            var conModelFM应收付款未审核 = new List<IConditionalModel>();
            conModelFM应收付款未审核.Add(new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" });
            conModelFM应收付款未审核.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModelFM应收付款未审核.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }

            var conModelFM应收付款未提交 = new List<IConditionalModel>();
            conModelFM应收付款未提交.Add(new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModelFM应收付款未提交.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModelFM应收付款未提交.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });

            #endregion

            #region 收付款
            var conModelFM收付款未审核 = new List<IConditionalModel>();
            conModelFM收付款未审核.Add(new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "int" });
            conModelFM收付款未审核.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModelFM收付款未审核.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }

            var conModelFM收付款未提交 = new List<IConditionalModel>();
            conModelFM应收付款未提交.Add(new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "int" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModelFM收付款未提交.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModelFM收付款未提交.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });

            #endregion

            #endregion



            var conModel未出库 = new List<IConditionalModel>();
            conModel未出库.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel未出库.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }

            conModel未出库.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });


            var conModel待收款 = new List<IConditionalModel>();
            conModel待收款.Add(new ConditionalModel { FieldName = "PayStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" });
            conModel待收款.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel待收款.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModel待收款.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });

            //未入库
            var conModel未入库 = new List<IConditionalModel>();
            conModel未入库.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "3", CSharpTypeName = "int" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel未入库.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModel未入库.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });



            //暂时这样没有完结就是没有还。
            var conModel借出未还清 = new List<IConditionalModel>();
            conModel借出未还清.Add(new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" });
            conModel借出未还清.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" });
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel借出未还清.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModel借出未还清.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });




            //返工退库没有回来的数据是 审核通过未结案 数量
            var conModel返工待完成 = new List<IConditionalModel>();
            conModel返工待完成.Add(new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" });
            conModel返工待完成.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" });

            conModel返工待完成.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });


            //审核通过未结案，像返工退库后还没有入库，采购退货也没有入库
            var conModel未结案 = new List<IConditionalModel>();
            conModel未结案.Add(new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" });
            conModel未结案.Add(new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" });
            conModel未结案.Add(new ConditionalModel { FieldName = "ApprovalResults", ConditionalType = ConditionalType.Equal, FieldValue = "True", CSharpTypeName = "bool" });
            conModel未结案.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });


            #region 预收付款

            var conModel预收付款待核销 = new List<IConditionalModel>();
            conModel预收付款待核销.Add(new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = (1 << 12).ToString(), CSharpTypeName = "long" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel预收付款待核销.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModel预收付款待核销.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });


            #endregion

            #region 应收应付 已生效

            var conModel等待回款付款 = new List<IConditionalModel>();
            conModel等待回款付款.Add(new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)ARAPStatus.已生效).ToString(), CSharpTypeName = "long" });
            //如果限制
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                conModel等待回款付款.Add(new ConditionalModel { FieldName = "Employee_ID", ConditionalType = ConditionalType.Equal, FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(), CSharpTypeName = "long" });
            }
            conModel等待回款付款.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });


            #endregion


            #region 收款付款 待确认支付

            var conModel待确认支付 = new List<IConditionalModel>();
            conModel待确认支付.Add(new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PaymentStatus.待审核).ToString(), CSharpTypeName = "long" });
            conModel待确认支付.Add(new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" });


            #endregion


            if (CenterConfig != null)
            {
                List<string> ToDoItems = CenterConfig.ToDoList.Split(',').ToList();
                foreach (var item in ToDoItems)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    待办事项 ToDoItem = (待办事项)Enum.Parse(typeof(待办事项), item);
                    switch (ToDoItem)
                    {
                        case 待办事项.采购_采购订单:
                            bizTypes.Add(BizType.采购订单);
                            break;
                        case 待办事项.采购_退款退货处理:
                            bizTypes.Add(BizType.采购退货单);
                            bizTypes.Add(BizType.采购退货入库);
                            break;
                        case 待办事项.销售_销售订单:
                            bizTypes.Add(BizType.销售订单);
                            break;
                        case 待办事项.销售_销售出库单:
                            bizTypes.Add(BizType.销售出库单);
                            break;
                        case 待办事项.销售_退款退货处理:
                            bizTypes.Add(BizType.销售退回单);
                            break;
                        case 待办事项.仓库_采购入库单:
                            bizTypes.Add(BizType.采购入库单);
                            break;
                        case 待办事项.仓库_盘点单:
                            bizTypes.Add(BizType.盘点单);
                            break;
                        case 待办事项.仓库_缴库单:
                            bizTypes.Add(BizType.缴库单);
                            break;
                        case 待办事项.仓库_退料单:
                            bizTypes.Add(BizType.生产退料单);
                            break;
                        case 待办事项.仓库_领料单:
                            bizTypes.Add(BizType.生产领料单);
                            break;
                        case 待办事项.仓库_分割组合:
                            bizTypes.Add(BizType.产品分割单);
                            bizTypes.Add(BizType.产品组合单);
                            bizTypes.Add(BizType.产品转换单);
                            break;
                        case 待办事项.生产_计划单:
                            bizTypes.Add(BizType.生产计划单);
                            break;
                        case 待办事项.财务_费用报销单:
                            bizTypes.Add(BizType.费用报销单);
                            bizTypes.Add(BizType.付款申请单);
                            break;
                        case 待办事项.其他_入库单:
                            bizTypes.Add(BizType.其他入库单);
                            break;
                        case 待办事项.其他_出库单:
                            bizTypes.Add(BizType.其他出库单);
                            break;
                        case 待办事项.请购单:
                            bizTypes.Add(BizType.请购单);
                            break;
                        case 待办事项.借出单:
                            bizTypes.Add(BizType.借出单);
                            break;
                        case 待办事项.归还单:
                            bizTypes.Add(BizType.归还单);
                            break;
                        case 待办事项.返工退库:
                            bizTypes.Add(BizType.返工退库单);
                            bizTypes.Add(BizType.返工入库单);
                            break;
                        case 待办事项.预付款单:
                            bizTypes.Add(BizType.预付款单);
                            break;
                        case 待办事项.预收款单:
                            bizTypes.Add(BizType.预收款单);
                            break;
                        case 待办事项.应付款单:
                            bizTypes.Add(BizType.应付款单);
                            break;
                        case 待办事项.应收款单:
                            bizTypes.Add(BizType.应收款单);
                            break;
                        case 待办事项.付款单:
                            bizTypes.Add(BizType.付款单);
                            break;
                        case 待办事项.收款单:
                            bizTypes.Add(BizType.收款单);
                            break;
                        default:
                            break;
                    }

                }
            }

            foreach (var item in bizTypes)
            {
                TreeNode node = new TreeNode(item.ToString());
                Type tableType = _mapper.GetTableType(item);
                //可以修改
                if (tableType.ContainsProperty(typeof(DataStatus).Name))
                {
                    #region 通常业务的 未提交 未审核

                    TreeNode SubNode未提交 = new TreeNode(item.ToString());
                    DataTable queryList未提交 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未提交).ToDataTableAsync();
                    if (queryList未提交.Rows.Count > 0)
                    {
                        QueryParameter parameter = new QueryParameter();
                        parameter.conditionals = conModel未提交;
                        parameter.tableType = tableType;

                        SubNode未提交.Text = "未提交【" + queryList未提交.Rows.Count + "】";
                        SubNode未提交.Tag = parameter;
                        node.Tag = parameter;
                        //  MainForm.Instance.AppContext.Db.CopyNew().Queryable(tableType.Name.ToString(), "a").Where().ToList();
                        if (!node.Nodes.Contains(SubNode未提交))
                        {
                            node.Nodes.Add(SubNode未提交);
                        }

                    }

                    TreeNode SubNode未审核 = new TreeNode(item.ToString());
                    DataTable queryList未审核 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未审核).ToDataTableAsync();
                    if (queryList未审核.Rows.Count > 0)
                    {
                        QueryParameter parameter = new QueryParameter();
                        parameter.conditionals = conModel未审核;
                        parameter.tableType = tableType;

                        SubNode未审核.Text = "未审核【" + queryList未审核.Rows.Count + "】";
                        SubNode未审核.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode未审核))
                        {
                            node.Nodes.Add(SubNode未审核);
                        }
                    }

                    #endregion
                }
                else if (tableType.ContainsProperty(typeof(PrePaymentStatus).Name))
                {
                    #region 财务的 未提交 未审核

                    AddNode(item, node, conModelFM预收付款未提交, "未提交");

                    AddNode(item, node, conModelFM预收付款未审核, "未审核");

                    #endregion
                }
                else if (tableType.ContainsProperty(typeof(ARAPStatus).Name))
                {
                    #region 财务的 未提交 未审核

                    AddNode(item, node, conModelFM应收付款未提交, "未提交");

                    AddNode(item, node, conModelFM应收付款未审核, "未审核");

                    #endregion
                }
                else if (tableType.ContainsProperty(typeof(PaymentStatus).Name))
                {
                    #region 财务的 未提交 未审核

                    AddNode(item, node, conModelFM收付款未提交, "未提交");

                    AddNode(item, node, conModelFM收付款未审核, "未审核");

                    #endregion
                }



                //下面是特殊情况。上面是所有单据的情况
                if (item == BizType.借出单)
                {
                    //QueryParameter parameter = new QueryParameter();
                    //parameter.conditionals = conModel借出未还清;
                    //parameter.tableType = tableType;

                    //TreeNode SubNode借出未还清 = new TreeNode(item.ToString());
                    //DataTable queryList借出未还清 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel借出未还清).ToDataTableAsync();
                    //if (queryList借出未还清.Rows.Count > 0)
                    //{
                    //    SubNode借出未还清.Text = "未还清【" + queryList借出未还清.Rows.Count + "】";
                    //    SubNode借出未还清.Tag = parameter;
                    //    if (!node.Nodes.Contains(SubNode借出未还清))
                    //    {
                    //        node.Nodes.Add(SubNode借出未还清);
                    //    }
                    //}

                    AddNode(item, node, conModel借出未还清, "未还清");
                }

                //下面是特殊情况。上面是所有单据的情况
                if (item == BizType.销售订单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel未出库;
                    parameter.tableType = tableType;
                    //未出库
                    TreeNode SubNode未出库 = new TreeNode(item.ToString());
                    DataTable queryList未出库 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未出库).ToDataTable();
                    if (queryList未出库.Rows.Count > 0)
                    {
                        SubNode未出库.Text = "未出库【" + queryList未出库.Rows.Count + "】";
                        SubNode未出库.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode未出库))
                        {
                            node.Nodes.Add(SubNode未出库);
                        }
                    }
                }


                if (item == BizType.销售出库单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel待收款;
                    parameter.tableType = tableType;

                    // 待收款
                    TreeNode SubNode待收款 = new TreeNode(item.ToString());
                    DataTable queryList待收款 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel待收款).ToDataTableAsync();
                    if (queryList待收款.Rows.Count > 0)
                    {
                        SubNode待收款.Text = "待收款【" + queryList待收款.Rows.Count + "】";
                        SubNode待收款.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode待收款))
                        {
                            node.Nodes.Add(SubNode待收款);
                        }
                    }
                }


                if (item == BizType.采购订单)
                {
                    // 待付款
                    //TreeNode SubNode待付款 = new TreeNode(item.ToString());
                    //DataTable queryList待付款 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where("ApprovalStatus = 0 and DataStatus = 1").ToDataTable();
                    //if (queryList待付款.Rows.Count > 0)
                    //{
                    //    SubNode待付款.Text = "待付款【" + queryList待付款.Rows.Count + "】";
                    //    SubNode待付款.Tag = new NodeParameter();
                    //    if (!node.Nodes.Contains(SubNode待付款))
                    //    {
                    //        node.Nodes.Add(SubNode待付款);
                    //    }
                    //}


                    //NodeParameter parameter = new NodeParameter();
                    //parameter.conditionals = conModel未入库;
                    //parameter.tableType = tableType;

                    ////未入库
                    //TreeNode SubNode未入库 = new TreeNode(item.ToString());
                    //DataTable queryList未入库 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未入库).ToDataTable();
                    //if (queryList未入库.Rows.Count > 0)
                    //{
                    //    SubNode未入库.Text = "未入库【" + queryList未入库.Rows.Count + "】";
                    //    SubNode未入库.Tag = parameter;
                    //    if (!node.Nodes.Contains(SubNode未入库))
                    //    {
                    //        node.Nodes.Add(SubNode未入库);
                    //    }
                    //}
                }
                if (item == BizType.采购退货单 || item == BizType.返工退库单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel未结案;
                    parameter.tableType = tableType;
                    //未出库
                    TreeNode SubNode采购退货待入库 = new TreeNode(item.ToString());
                    DataTable queryList采购退货待入库 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未结案).ToDataTableAsync();
                    if (queryList采购退货待入库.Rows.Count > 0)
                    {
                        SubNode采购退货待入库.Text = "待返回【" + queryList采购退货待入库.Rows.Count + "】";
                        SubNode采购退货待入库.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode采购退货待入库))
                        {
                            node.Nodes.Add(SubNode采购退货待入库);
                        }
                    }
                }


                if (item == BizType.预收款单 || item == BizType.预付款单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel预收付款待核销;
                    parameter.tableType = tableType;

                    // 待收款
                    TreeNode SubNode待收款 = new TreeNode(item.ToString());
                    DataTable queryList待核销 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel预收付款待核销).ToDataTableAsync();
                    if (queryList待核销.Rows.Count > 0)
                    {
                        SubNode待收款.Text = "待核销【" + queryList待核销.Rows.Count + "】";
                        SubNode待收款.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode待收款))
                        {
                            node.Nodes.Add(SubNode待收款);
                        }
                    }
                }

                if (item == BizType.应收款单 || item == BizType.应付款单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel等待回款付款;
                    parameter.tableType = tableType;

                    string paytypeName = "回款";
                    if (item == BizType.应付款单)
                    {
                        paytypeName = "付款";
                    }

                    // 待收款
                    TreeNode SubNode待回款付款 = new TreeNode(item.ToString());
                    DataTable queryList待回款付款 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel等待回款付款).ToDataTableAsync();
                    if (queryList待回款付款.Rows.Count > 0)
                    {
                        SubNode待回款付款.Text = $"待{paytypeName}【" + queryList待回款付款.Rows.Count + "】";
                        SubNode待回款付款.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode待回款付款))
                        {
                            node.Nodes.Add(SubNode待回款付款);
                        }
                    }
                }

                if (item == BizType.收款单 || item == BizType.付款单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel待确认支付;
                    parameter.tableType = tableType;



                    // 待收款
                    TreeNode SubNode待确认支付 = new TreeNode(item.ToString());
                    DataTable queryList待确认支付 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel待确认支付).ToDataTableAsync();
                    if (queryList待确认支付.Rows.Count > 0)
                    {
                        SubNode待确认支付.Text = $"待确认支付【" + queryList待确认支付.Rows.Count + "】";
                        SubNode待确认支付.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode待确认支付))
                        {
                            node.Nodes.Add(SubNode待确认支付);
                        }
                    }
                }


                //没有数据的不用显示
                if (node.Nodes.Count > 0)
                {
                    nd.Nodes.Add(node);
                }

            }

            //添加重新加载的菜单
            // nd.ContextMenuStrip = contextMenuStrip1;
            // kryptonTreeViewJobList.Nodes.Add(nd);
            //直接添加子节点
            foreach (TreeNode item in nd.Nodes)
            {
                kryptonTreeViewJobList.Nodes.Add(item);
            }

            //  kryptonTreeViewJobList.Nodes[0].Expand();
            kryptonTreeViewJobList.ExpandAll();
        }

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

        private Dictionary<string, List<IConditionalModel>> GetCommonStatusConditions()
        {
            var conditions = new Dictionary<string, List<IConditionalModel>>();

            // 未提交条件
            var conModel未提交 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未提交);

            conditions.Add("未提交", conModel未提交);

            // 未审核条件
            var conModel未审核 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "0", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "2", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未审核);

            conditions.Add("未审核", conModel未审核);

            return conditions;
        }

        private Dictionary<string, List<IConditionalModel>> GetPrePaymentStatusConditions(ReceivePaymentType paymentType)
        {
            var conditions = new Dictionary<string, List<IConditionalModel>>();

            // 未提交条件
            var conModel未提交 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未提交);

            conditions.Add("未提交", conModel未提交);

            // 未审核条件
            var conModel未审核 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.草稿).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未审核);

            conditions.Add("未审核", conModel未审核);

            // 待核销条件
            var conModel待核销 = new List<IConditionalModel>
            {new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = (1 << 12).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel待核销);

            conditions.Add("待核销", conModel待核销);

            return conditions;
        }

        private Dictionary<string, List<IConditionalModel>> GetARAPStatusConditions(ReceivePaymentType paymentType)
        {
            var conditions = new Dictionary<string, List<IConditionalModel>>();

            // 未提交条件
            var conModel未提交 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未提交);

            conditions.Add("未提交", conModel未提交);

            // 未审核条件
            var conModel未审核 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未审核);

            conditions.Add("未审核", conModel未审核);

            // 等待回款/付款条件
            var conModel等待回款付款 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)ARAPStatus.已生效).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel等待回款付款);

            conditions.Add("等待回款付款", conModel等待回款付款);

            return conditions;
        }

        private Dictionary<string, List<IConditionalModel>> GetPaymentStatusConditions(ReceivePaymentType paymentType)
        {
            var conditions = new Dictionary<string, List<IConditionalModel>>();

            // 未提交条件
            var conModel未提交 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未提交);

            conditions.Add("未提交", conModel未提交);

            // 未审核条件
            var conModel未审核 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PrePaymentStatus.待审核).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            // 添加销售限制条件
            AddSaleLimitedCondition(conModel未审核);

            conditions.Add("未审核", conModel未审核);

            // 待确认支付条件
            var conModel待确认支付 = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ReceivePaymentType", ConditionalType = ConditionalType.Equal, FieldValue = ((int)paymentType).ToString(), CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PaymentStatus.待审核).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            conditions.Add("待确认支付", conModel待确认支付);

            return conditions;
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

        private async Task<int> AddStatusNodes(TreeNode parentNode, Type tableType, BizType bizType, Dictionary<string, List<IConditionalModel>> statusConditions)
        {
            int counter = 0;
            foreach (var status in statusConditions)
            {
                int rows = await AddNode(parentNode, tableType, bizType, status.Value, status.Key);
                counter += rows;
            }
            return counter;
        }

        private async Task<int> AddNode(TreeNode parentNode, Type tableType, BizType bizType, List<IConditionalModel> conditions, string statusText, string UIPropertyIdentifier = "")
        {
            TreeNode subNode = new TreeNode(bizType.ToString());

            // 查询数据
            DataTable queryList = await MainForm.Instance.AppContext.Db
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

                // 设置节点文本和标签
                subNode.Text = $"{statusText}【{queryList.Rows.Count}】";
                subNode.Tag = parameter;

                // 添加到父节点
                if (!parentNode.Nodes.Contains(subNode))
                {
                    parentNode.Nodes.Add(subNode);
                }
            }
            return queryList.Rows.Count;
        }

        private async Task<int> AddSpecialStatusNodes(TreeNode node, Type tableType, BizType bizType)
        {
            int Counter = 0;
            switch (bizType)
            {
                case BizType.借出单:
                    Counter += await AddNode(node, tableType, bizType, GetLendNotReturnedConditions(), "未还清");
                    break;

                case BizType.销售订单:
                    Counter += await AddNode(node, tableType, bizType, GetNotShippedConditions(), "未出库");
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
                    Counter += await AddNode(node, tableType, bizType, GetPaymentToBeConfirmedConditions(ReceivePaymentType.收款), "待确认支付", SharedFlag.Flag1.ToString());
                    break;
                case BizType.付款单:
                    Counter += await AddNode(node, tableType, bizType, GetPaymentToBeConfirmedConditions(ReceivePaymentType.付款), "待确认支付", SharedFlag.Flag2.ToString());
                    break;
            }
            return Counter;
        }

        private List<IConditionalModel> GetLendNotReturnedConditions()
        {
            var conditions = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };

            AddSaleLimitedCondition(conditions);

            return conditions;
        }

        private List<IConditionalModel> GetNotShippedConditions()
        {
            var conditions = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue = "4", CSharpTypeName = "int" },
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
                new ConditionalModel { FieldName = "PrePaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = (1 << 12).ToString(), CSharpTypeName = "long" },
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
                new ConditionalModel { FieldName = "ARAPStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)ARAPStatus.已生效).ToString(), CSharpTypeName = "long" },
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
                new ConditionalModel { FieldName = "PaymentStatus", ConditionalType = ConditionalType.Equal, FieldValue = ((long)PaymentStatus.待审核).ToString(), CSharpTypeName = "long" },
                new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="node"></param>
        /// <param name="conModel"></param>
        /// <param name="SubNodeText">未审核</param>
        private async void AddNode(BizType item, TreeNode node, List<IConditionalModel> conModel, string SubNodeText)
        {
            Type tableType = _mapper.GetTableType(item);
            TreeNode SubNodeeFM预收付款未审核 = new TreeNode(item.ToString());
            DataTable queryListeFM预收付款未审核 = await MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel).ToDataTableAsync();
            if (queryListeFM预收付款未审核.Rows.Count > 0)
            {
                QueryParameter parameter = new QueryParameter();
                parameter.conditionals = conModel;
                parameter.tableType = tableType;

                SubNodeeFM预收付款未审核.Text = $"{SubNodeText}【" + queryListeFM预收付款未审核.Rows.Count + "】";
                SubNodeeFM预收付款未审核.Tag = parameter;
                if (!node.Nodes.Contains(SubNodeeFM预收付款未审核))
                {
                    node.Nodes.Add(SubNodeeFM预收付款未审核);
                }
            }
        }






    }
}
