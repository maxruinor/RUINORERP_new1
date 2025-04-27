using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.AutoMapper;
using AutoMapper;
using RUINORERP.Common.CollectionExtension;
using SqlSugar;
using RUINORERP.Common.Extensions;
using System.Collections;
using RUINORERP.Model.Base;
using RUINORERP.Business.Security;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;
using Microsoft.Extensions.Logging;
using RulesEngine.Models;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using System.IO;
using Rule = RulesEngine.Models.Rule;
using RUINORERP.Model.CommonModel;
using System.Linq.Expressions;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using WorkflowCore.Interface;
using RUINORERP.UI.SS;
using RulesEngine;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using Fireasy.Common.Extensions;
using Netron.NetronLight;
using RUINORERP.UI.UControls;
namespace RUINORERP.UI.PSI.SAL
{

    [MenuAttrAssemblyInfo("销售订单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售订单)]
    public partial class UCSaleOrderQuery : BaseBillQueryMC<tb_SaleOrder, tb_SaleOrderDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCSaleOrderQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.SOrderNo);
        }

        public List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为销售出库单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为出库单】", true, false, "NewSumDataGridView_转为销售出库单"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为销售出库单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list = AddContextMenu();

            UIHelper.ControlContextMenuInvisible(CurMenuInfo, list);

            if (_UCBillMasterQuery != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = _UCBillMasterQuery.newSumDataGridViewMaster.GetContextMenu(_UCBillMasterQuery.newSumDataGridViewMaster.ContextMenuStrip
                    , ContextClickList, list, true
                    );
                _UCBillMasterQuery.newSumDataGridViewMaster.ContextMenuStrip = newContextMenuStrip;
            }
        }

        //public override void BuildLimitQueryConditions()
        //{
        //    //// 替换占位符为参数名
        //    //.Replace("@EmployeeId", ":employeeId");
        //    //// SqlSugar自动处理参数
        //    //lambda.And(t => t.Employee_ID == SqlFunc.Param("employeeId", user.EmployeeId));
        //    //创建表达式
        //    var lambda = Expressionable.Create<tb_SaleOrder>()
        //                     //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
        //                     // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
        //                     .And(t => t.isdeleted == false)
        //                       // .And(t => t.Is_enabled == true)
        //                       .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
        //                    .ToExpression();//注意 这一句 不能少
        //    base.LimitQueryConditions = lambda;
        //    QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        //}

        public List<RuleResultWithFilter> ExecuteRulesWithFilter(RulesEngine.RulesEngine re, tb_UserInfo user, tb_MenuInfo menu)
        {
            var results = re.ExecuteAllRulesAsync("QueryFilterRules", user, menu).Result;
            return results.Select(r => new RuleResultWithFilter
            {
                IsSuccess = r.IsSuccess,
                FilterExpression = r.IsSuccess ?
                    r.Rule.Expression.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Trim('"')
                    : null
            }).ToList();
        }




        private void NewSumDataGridView_转为销售出库单(object sender, EventArgs e)
        {
            List<tb_SaleOrder> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_SaleOuts != null && item.tb_SaleOuts.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.SOrderNo}：已经生成过出库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }

                    tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                    //tb_SaleOut saleOut = SaleOrderToSaleOut(item);
                    tb_SaleOut saleOut = ctr.SaleOrderToSaleOut(item);
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_SaleOut) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, saleOut);
                    }
                    return;
                }
                else
                {
                    if (item.DataStatus == (int)DataStatus.完结)
                    {
                        // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                        MessageBox.Show($"当前订单{item.SOrderNo}：已结案，无法生成出库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (item.DataStatus == (int)DataStatus.草稿 || item.DataStatus == (int)DataStatus.新建)
                    {
                        // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                        MessageBox.Show($"当前订单{item.SOrderNo}：未审核，无法生成出库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
            }
        }

        /*
        public override void BuildLimitQueryConditions()
        {

            // 注册自定义函数
            var reSettings = new ReSettings
            {
                CustomTypes = new Type[] { typeof(FilterHelper), typeof(tb_UserInfo), typeof(tb_MenuInfo) }
            };

            // 加载规则配置
            //var rulesJson = File.ReadAllText("permission_rules.json");
            //var workflows = new List<Workflow> {
            //       new Workflow {
            //           WorkflowName = "QueryFilterRules",
            //           Rules = JsonConvert.DeserializeObject<List<Rule>>(rulesJson)
            //       }
            //   };
            // 1. 从JSON文件加载工作流配置
            //var json = File.ReadAllText("permission_rules.json");
            var json = @"[
                          {
                            ""WorkflowName"": ""QueryFilterRules"", // 工作流名称必须与代码中执行时一致
                            ""Rules"": [ // 必须包含Rules数组
                              {
                                ""RuleName"": ""DepartmentFilter"",
                                ""Expression"": ""t => t.Employee_ID == @EmployeeId &&  t.DepartmentId == @DepartmentId""
                              },
                              {
                                ""RuleName"": ""SoftDelete"",
                                ""Expression"": ""t => t.isdeleted == false""
                              }
                            ]
                          }
                        ]";

            var workflows = JsonConvert.DeserializeObject<List<Workflow>>(json); // 注意：反序列化为List<Workflow>

            var re = new RulesEngine.RulesEngine(workflows.ToArray(), reSettings);
            //var re = new RulesEngine.RulesEngine(workflows.ToArray(), null);

            var user = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            var menu = CurMenuInfo;

            // 执行规则
            // var resultList = await re.ExecuteAllRulesAsync("QueryFilterRules", user, menu);
            // 执行规则并获取过滤条件
            var ruleResults = ExecuteRulesWithFilter(re, user, menu);

            //var successResults = resultList.Where(r =>
            //    r.IsSuccess &&
            //    r.f == "ApplyFilter" // 根据事件类型过滤
            //);

            var lambda = Expressionable.Create<tb_SaleOrder>();

            var param = Expression.Parameter(typeof(tb_SaleOrder), "t");


            foreach (var result in ruleResults.Where(r => r.IsSuccess && !string.IsNullOrEmpty(r.FilterExpression)))
            {
                var resolvedExpr = ReplacePlaceholders(result.FilterExpression, menu, user);
                //var exprE = DynamicExpressionParser.ParseLambda<tb_SaleOrder, bool>(
                //    ParsingConfig.Default,
                //    resolvedExpr
                //);
            }

            // 应用基础条件（如无规则时）
            if (!ruleResults.Any(r => r.IsSuccess))
            {
                lambda.And(t => t.isdeleted == false);
            }

            var results = re.ExecuteAllRulesAsync("QueryFilterRules",
            new RuleParameter("user", user),
            new RuleParameter("menu", menu)
                 ).Result;

            //foreach (var result in ruleResults.Where(r => r.IsSuccess))
            foreach (var result in results)
            {
                #region 解析Expression

                // 从规则中获取过滤表达式字符串
                var Expr = result.Rule.Expression.ToString();// result.Rule.Actions["FilterExpression"].ToString();

                // 替换占位符（如@UserId → 实际值） 实际值是 由哪个对象来决定的呢？
                var resolvedExpr = ReplacePlaceholders(Expr, menu, user);


                // 解析为Lambda表达式

                var paramUser = Expression.Parameter(typeof(tb_UserInfo), "user");
                var paramMenu = Expression.Parameter(typeof(tb_MenuInfo), "menu");
                var paramT = Expression.Parameter(typeof(tb_SaleOrder), "t");

                var resolvedExpr1 = "t => t.Employee_ID == \"121\" && t.DepartmentId == \"121\"";

                // 使用ParsingConfig放宽解析限制
                var config = new ParsingConfig
                {
                    ResolveTypesBySimpleName = true // 允许简单类型名称解析
                };

                var filterExprLambda1 = DynamicExpressionParser.ParseLambda(
                    config,
                    new[] { paramT },
                    typeof(bool),
                    resolvedExpr1
                ) as Expression<Func<tb_SaleOrder, bool>>;



                if (result.Rule.RuleName == "SalesOrderAccess")
                {
                    // 解析条件表达式（如 menu.CaptionCN.Contains("客户")）
                    var conditionExpr = DynamicExpressionParser.ParseLambda(
                        new[] { paramUser, paramMenu },
                        typeof(bool),
                        resolvedExpr
                    ) as Expression<Func<tb_SaleOrder, bool>>;
                    lambda.And(conditionExpr);
                }
                else
                {
                    resolvedExpr = resolvedExpr1;
                    // 解析过滤表达式（如 t => t.Employee_ID == 123）
                    var filterExprLambda = DynamicExpressionParser.ParseLambda(
                        new[] { paramT },
                        typeof(bool),
                        resolvedExpr
                    ) as Expression<Func<tb_SaleOrder, bool>>;

                    // 添加到SqlSugar的条件组合器
                    lambda.And(filterExprLambda);
                }

                #endregion
                // 从规则中获取过滤表达式字符串
                var filterExpr = result.Rule.Expression.ToString();// result.Rule.Actions["FilterExpression"].ToString();
                                                                   // 从Actions中获取过滤条件

                // 替换占位符（如@UserId → 实际值）
                var resolvedFilterExpr = ReplacePlaceholders(filterExpr, menu, user);

                // 解析为Lambda表达式
                var Filterparam = Expression.Parameter(typeof(tb_SaleOrder), "t");
                var Filterexpr = DynamicExpressionParser.ParseLambda(
                    new[] { Filterparam },
                    typeof(bool),
                    resolvedExpr
                ) as Expression<Func<tb_SaleOrder, bool>>;

                // 添加到SqlSugar的条件组合器
                lambda.And(Filterexpr);
            }

            // 必须添加的基础条件（如软删除）
            lambda.And(t => t.isdeleted == false);

            base.LimitQueryConditions = lambda.ToExpression();
            //QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }
        */
        /// <summary>
        /// 可以通过  注册 数字要"引号" 转换不然会失败
        /// </summary>
        private void lambdaTest()
        {
            // 构造 lambda 表达式的字符串形式
            string exprString = "t => t.Employee_ID == \"121\" && t.DepartmentId == \"11\"";
            // 解析 lambda 表达式字符串，生成表达式树
            var parameter = Expression.Parameter(typeof(tb_SaleOrder), "t");
            var lambdaExpr = DynamicExpressionParser.ParseLambda(new[] { parameter }, typeof(bool), exprString);

            // 编译表达式树为委托
            var func = (Func<tb_SaleOrder, bool>)lambdaExpr.Compile();

            var lambda = Expressionable.Create<tb_SaleOrder>();
            var user = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            var menu = CurMenuInfo;


            // 示例值（根据实际类型调整）
            var resolvedExpr = "t => t.Employee_ID == \"112\" && t.DepartmentId == \"3121\"";

            // 解析为 Lambda 表达式
            var config = new ParsingConfig
            {
                ResolveTypesBySimpleName = true,
                CustomTypeProvider = new DefaultDynamicLinqCustomTypeProvider() // 确保识别自定义类型
            };

            var paramT = Expression.Parameter(typeof(tb_SaleOrder), "t");
            var filterExprLambda = DynamicExpressionParser.ParseLambda(
                config,
                new[] { paramT },
                typeof(bool),
                resolvedExpr
            ) as Expression<Func<tb_SaleOrder, bool>>;

            lambda.And(filterExprLambda);
            lambda.And(t => t.isdeleted == false);

            // 验证生成的表达式
            Console.WriteLine(lambda.ToExpression().ToString());
            // 输出示例: t => ((t.Employee_ID == 121) && (t.DepartmentId == 11)) && (t.isdeleted == False)
        }

        // 占位符替换方法
        private string ReplacePlaceholders(string expr, tb_MenuInfo menu, tb_UserInfo user)
        {
            return expr
                //.Replace("@UserId", user.User_ID.ToString())
                .Replace("@EmployeeId", user.Employee_ID.ToString())
                .Replace("@DepartmentId", user.tb_employee.tb_department.ID.ToString());
        }
        private string ReplacePlaceholders(string expr, tb_SaleOrder t)
        {
            return expr
                .Replace("@EmployeeId", t.Employee_ID.ToString())
                .Replace("@DepartmentId", t.tb_employee.tb_department.ID.ToString());
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            base.BuildQueryCondition();

            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_SaleOrder>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalTaxAmount);
            base.MasterSummaryCols.Add(c => c.ForeignTotalAmount);
            base.MasterSummaryCols.Add(c => c.ShipCost);
            base.MasterSummaryCols.Add(c => c.Deposit);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.CommissionAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTaxAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTransAmount);

        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.TotalCost);
            base.ChildInvisibleCols.Add(c => c.Cost);
            base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


        /// <summary>
        /// 批量转换为销售出库单
        /// </summary>
        public async void BatchConversion()
        {
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            List<tb_SaleOrder> selectlist = GetSelectResult();
            int conter = 0;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_SaleOuts != null && item.tb_SaleOuts.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.SOrderNo}：已经生成过出库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }

                    }

                    // tb_SaleOut saleOut = SaleOrderToSaleOut(item);
                    tb_SaleOrderController<tb_SaleOrder> ctrOrder = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                    tb_SaleOut saleOut = ctrOrder.SaleOrderToSaleOut(item);

                    ReturnMainSubResults<tb_SaleOut> rsrs = await ctr.BaseSaveOrUpdateWithChild<tb_SaleOut>(saleOut);
                    if (rsrs.Succeeded)
                    {
                        conter++;
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("转换出错:" + rsrs.ErrorMsg);
                    }
                }
                else
                {
                    MainForm.Instance.uclog.AddLog(string.Format("当前订单:{0}的状态为{1},不能转换为销售出库单。", item.SOrderNo, ((DataStatus)item.DataStatus).ToString()));
                    continue;
                }

            }
            MainForm.Instance.uclog.AddLog("转换完成,成功订单数量:" + conter);
            MainForm.Instance.logger.LogInformation("转换完成,成功订单数量:" + conter);
        }

        /* 放到了 订单的业务层 
        /// <summary>
        /// 转换为销售出库单
        /// </summary>
        /// <param name="saleorder"></param>
        public tb_SaleOut SaleOrderToSaleOut(tb_SaleOrder saleorder)
        {
            tb_SaleOut entity = new tb_SaleOut();
            //转单
            if (saleorder != null)
            {
                IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
                entity = mapper.Map<tb_SaleOut>(saleorder);
                List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
                //转单要TODO
                //转换时，默认认为订单出库数量就等于这次出库数量，是否多个订单累计？，如果是UI录单。则只是默认这个数量。也可以手工修改
                List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Quantity = details[i].Quantity - item.TotalDeliveredQty;// 减掉已经出库的数量
                    details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                    details[i].SubtotalCostAmount = details[i].Cost * details[i].Quantity;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                }

                entity.tb_SaleOutDetails = NewDetails;
                entity.ApprovalOpinions = "批量转单";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                if (NewDetails.Count != details.Count)
                {
                    //已经出库过，第二次不包括 运费
                    entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                    entity.TotalCost = NewDetails.Sum(c => c.Cost * c.Quantity);
                    entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                    entity.CollectedMoney = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                    entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
                    entity.TotalUntaxedAmount = NewDetails.Sum(c => c.SubtotalUntaxedAmount);
                }

                if (saleorder.DeliveryDate.HasValue)
                {
                    entity.OutDate = saleorder.DeliveryDate.Value;
                    entity.DeliveryDate = saleorder.DeliveryDate;
                }
                else
                {
                    entity.OutDate = System.DateTime.Now;
                    entity.DeliveryDate = System.DateTime.Now;
                }

                BusinessHelper.Instance.InitEntity(entity);

                if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = saleorder.CustomerVendor_ID;
                    entity.SaleOrderNo = saleorder.SOrderNo;
                }
                entity.SaleOutNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售出库单);
                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }
        */

        //public override List<tb_SaleOrder> GetPrintDatas(List<tb_SaleOrder> EditEntitys)
        //{
        //    List<tb_SaleOrder> datas = new List<tb_SaleOrder>();
        //    foreach (var item in EditEntitys)
        //    {
        //        tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
        //        var PrintData = ctr.GetPrintData(item.SOrder_ID);
        //        datas.Add(PrintData[0] as tb_SaleOrder);
        //    }
        //    return datas;
        //}


        //public override List<tb_SaleOrder> GetPrintDatas(tb_SaleOrder EditEntity)
        //{
        //    List<tb_SaleOrder> datas = new List<tb_SaleOrder>();
        //    tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
        //    List<tb_SaleOrder> PrintData = ctr.GetPrintData(EditEntity.SOrder_ID);
        //    return PrintData;
        //}


        /*
 /// <summary>
 /// 销售订单审核，审核成功后，库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
 /// </summary>
 /// <returns></returns>
 protected async override Task<ApprovalEntity> Review(tb_SaleOrder EditEntity)
 {
     if (EditEntity == null)
     {
         return null;
     }

     //同时检查数量和金额，总数量和总金额不能小于明细小计的和
     if (EditEntity.TotalQty < EditEntity.tb_SaleOrderDetails.Sum(c => c.Quantity))
     {
         MainForm.Instance.PrintInfoLog($"订单：{EditEntity.SOrderNo}:总数量不能小于明细小计之和！");
         return null;
     }

     if (EditEntity.TotalAmount < EditEntity.tb_SaleOrderDetails.Sum(c => c.TransactionPrice * c.Quantity))
     {
         MainForm.Instance.PrintInfoLog($"订单：{EditEntity.SOrderNo}:总金额不能小于明细小计之和！");
         return null;
     }
     ApprovalEntity ae =await base.Review(EditEntity);

     return ae;
 }


 /// <summary>
 /// 销售订单反审
 /// </summary>
 /// <param name="EditEntitys"></param>
 /// <returns></returns>
 public async override Task<bool> ReReview(List<tb_SaleOrder> EditEntitys)
 {
     if (EditEntitys == null)
     {
         return false;
     }
     foreach (tb_SaleOrder EditEntity in EditEntitys)
     {
         #region 反审
         //反审，要审核过，并且通过了，才能反审。
         if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
         {
             MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
             continue;
         }


         if (EditEntity.tb_SaleOrderDetails == null || EditEntity.tb_SaleOrderDetails.Count == 0)
         {
             MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
             continue;
         }

         RevertCommand command = new RevertCommand();
         //缓存当前编辑的对象。如果撤销就回原来的值
         tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
         command.UndoOperation = delegate ()
         {
             //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
             CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
         };

         tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

         ReturnResults<bool> rr = await ctr.AntiApprovalAsync(EditEntity);
         if (rr.Succeeded)
         {
             //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
             //{

             //}
             //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
             //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
             //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
             //MainForm.Instance.ecs.AddSendData(od);

             //审核成功
         }
         else
         {
             //审核失败 要恢复之前的值
             command.Undo();
             MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}反审失败{rr.ErrorMsg},请联系管理员！", Color.Red);
         }

         #endregion
     }
     return true;
 }
 */

        public async override Task<bool> CloseCase(List<tb_SaleOrder> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_SaleOrder> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
            {
                MainForm.Instance.PrintInfoLog($"结案操作成功！", Color.Red);
                MainForm.Instance.logger.LogInformation($"结案操作成功！");
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);
                base.Query(QueryDto);
            }
            else
            {
                MainForm.Instance.PrintInfoLog($"结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }





    }
}
