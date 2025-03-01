using AutoMapper;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Common;
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
        public UCTodoList()
        {
            InitializeComponent();
        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private void kryptonTreeViewJobList_NodeMouseDoubleClickAsync(object sender, TreeNodeMouseClickEventArgs e)
        {
            // e.Node
            //导航到指向的单据界面
            //找到要打开的菜单  订单查询
            if (kryptonTreeViewJobList.SelectedNode != null)
            {
                if (kryptonTreeViewJobList.SelectedNode.Tag is QueryParameter nodeParameter)
                {

                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nodeParameter.tableType.Name && m.ClassPath.Contains("Query")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        //tb_PurOrderController<tb_PurOrder> controller = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                        //tb_PurOrder purOrder = await controller.BaseQueryByIdNavAsync(pid);
                        //要把单据信息传过去
                        BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(nodeParameter.tableType.Name + "Processor");
                        var QueryConditionFilter = baseProcessor.GetQueryFilter();
                        nodeParameter.queryFilter = QueryConditionFilter;
                        // 创建实例
                        object instance = Activator.CreateInstance(nodeParameter.tableType);
                        menuPowerHelper.OnSetQueryConditionsDelegate += MenuPowerHelper_OnSetQueryConditionsDelegate;
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, instance, nodeParameter);
                        //要卸载，不然会多次执行
                        menuPowerHelper.OnSetQueryConditionsDelegate -= MenuPowerHelper_OnSetQueryConditionsDelegate;

                    }

                }
            }
        }

        /// <summary>
        /// 按查询条件  给 Dto对象给到查询条件的值
        /// </summary>
        /// <param name="QueryDto"></param>
        /// <param name="nodeParameter"></param>
        private void MenuPowerHelper_OnSetQueryConditionsDelegate(object QueryDto, QueryParameter nodeParameter)
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

        private void RefreshData_Click(object sender, EventArgs e)
        {

            //tb_WorkCenterConfig
            if (sender is ToolStripMenuItem menuItem)
            {
                if (menuItem.Owner is ContextMenuStrip contextMenu)
                {
                  
                        if (kryptonTreeViewJobList.Tag is tb_WorkCenterConfig config)
                        {
                            BuilderToDoListTreeView(config);
                        }
                 
                }
            }
        }


        public void RefreshTreeData()
        {

        }


        BizTypeMapper mapper = new BizTypeMapper();


        /// <summary>
        /// 待办事项
        /// </summary>
        private void BuilderToDoListTreeView(tb_WorkCenterConfig centerConfig)
        {
            kryptonTreeViewJobList.Nodes.Clear();
            TreeNode nd = new TreeNode();
            nd.Text = "待办事项";
            nd.ImageIndex = 0;
            nd.Tag = centerConfig;
            //手动构造已提交未审核
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


            List<BizType> bizTypes = new List<BizType>();
            if (centerConfig != null)
            {
                List<string> ToDoItems = centerConfig.ToDoList.Split(',').ToList();
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
                        default:
                            break;
                    }

                }
            }

            foreach (var item in bizTypes)
            {
                TreeNode node = new TreeNode(item.ToString());
                Type tableType = mapper.GetTableType(item);

                TreeNode SubNode未提交 = new TreeNode(item.ToString());
                DataTable queryList未提交 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未提交).ToDataTable();
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
                DataTable queryList未审核 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未审核).ToDataTable();
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

                //下面是特殊情况。上面是所有单据的情况
                if (item == BizType.借出单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel借出未还清;
                    parameter.tableType = tableType;
                    //未出库
                    TreeNode SubNode借出未还清 = new TreeNode(item.ToString());
                    DataTable queryList借出未还清 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel借出未还清).ToDataTable();
                    if (queryList借出未还清.Rows.Count > 0)
                    {
                        SubNode借出未还清.Text = "未还清【" + queryList借出未还清.Rows.Count + "】";
                        SubNode借出未还清.Tag = parameter;
                        if (!node.Nodes.Contains(SubNode借出未还清))
                        {
                            node.Nodes.Add(SubNode借出未还清);
                        }
                    }
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
                    DataTable queryList待收款 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel待收款).ToDataTable();
                    if (queryList待收款.Rows.Count > 0)
                    {
                        SubNode待收款.Text = "待收款【" + queryList待收款.Rows.Count + "】";
                        SubNode待收款.Tag = conModel待收款;
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
                if (item==BizType.采购退货单||item==BizType.返工退库单)
                {
                    QueryParameter parameter = new QueryParameter();
                    parameter.conditionals = conModel未结案;
                    parameter.tableType = tableType;
                    //未出库
                    TreeNode SubNode采购退货待入库 = new TreeNode(item.ToString());
                    DataTable queryList采购退货待入库 = MainForm.Instance.AppContext.Db.Queryable(tableType.Name, "TN").Where(conModel未结案).ToDataTable();
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

        // 假设您有一个方法，用于将字符串转换为布尔值
        private static object ConvertStringToBoolean(string value)
        {
            // 这里只是一个简单的示例，实际转换可能需要更复杂的逻辑
            return value.Equals("False", StringComparison.OrdinalIgnoreCase);

        }
        private void UCTodoList_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            //月销售，依客戶，依业务
            tb_RoleInfo CurrentRole = MainForm.Instance.AppContext.CurrentRole;
            tb_UserInfo CurrentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            //先取人，无人再取角色。
            tb_WorkCenterConfig centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID && c.User_ID == CurrentUser.User_ID);
            if (centerConfig == null)
            {
                centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
            }
            BuilderToDoListTreeView(centerConfig);
            kryptonTreeViewJobList.ContextMenuStrip = contextMenuStrip1;
            kryptonTreeViewJobList.Tag = centerConfig;
        }
    }
}
