﻿using System;
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
using RUINORERP.Common.Extensions;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using RUINORERP.Common.Helper;
using System.Linq.Expressions;

namespace RUINORERP.UI.PSI.PUR
{

    [MenuAttrAssemblyInfo("请购单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.请购单)]
    public partial class UCBuyingRequisitionQuery : BaseBillQueryMC<tb_BuyingRequisition, tb_BuyingRequisitionDetail>
    {
        public UCBuyingRequisitionQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PuRequisitionNo);
            //显示转出库单
           // tsbtnBatchConversion.Visible = true;
           // tsbtnBatchConversion.Text = MenuItemEnums.转入库单.ToString();
        }


        public override void BuildColNameDataDictionary()
        {
            System.Linq.Expressions.Expression<Func<tb_BuyingRequisition, int?>> exprPayStatus;
            exprPayStatus = (p) => p.RefBizType;
            base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(BizType)));
            base.BuildColNameDataDictionary();
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_BuyingRequisition>()
                             .And(t => t.isdeleted == false)
                              .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            //base.QueryConditions.Add(c => c.PurOrderNo);
            //base.QueryConditions.Add(c => c.PurDate);
            //base.QueryConditions.Add(c => c.Employee_ID);
            //base.QueryConditions.Add(c => c.CustomerVendor_ID);
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_BuyingRequisition).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

        }

        /*
        protected override async void Query()
        {

            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;

            BaseController<tb_BuyingRequisition> ctr = Startup.GetFromFacByName<BaseController<tb_BuyingRequisition>>(typeof(tb_BuyingRequisition).Name + "Controller");


            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRow.Text);
            List<tb_BuyingRequisition> list = new List<tb_BuyingRequisition>();
            //提取指定的列名，即条件集合
            List<string> queryConditions = new List<string>();
            queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());

            ISugarQueryable<tb_BuyingRequisition> querySqlQueryable;

            if (typeof(tb_BuyingRequisition).GetProperties().ContainsProperty("isdeleted"))
            {
                querySqlQueryable = MainForm.Instance.AppContext.Db.Queryable<tb_BuyingRequisition>()
            //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
            .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .Where(true, queryConditions, base.QueryDto)
                           .WhereIF(LimitQueryConditions != null, LimitQueryConditions)
                            .Where("isdeleted=@isdeleted", new { isdeleted = 0 });
            }
            else
            {
                var cv = QueryConditionFilter.QueryFields.Where(c => c.FieldName == "CustomerVendor_ID").FirstOrDefault().SubFilter.GetFilterExpression<tb_CustomerVendor>();
                ExpressionToSql expressionToSql = new();
                string sql = expressionToSql.GetSql<tb_CustomerVendor>(cv);
                //添加供应商的限制（专属责任人）


                //两种条件组合为一起，一种是process中要处理器中设置好的，另一个是UI中 灵活设置的
                Expression<Func<tb_BuyingRequisition, bool>> expression = QueryConditionFilter.GetFilterExpression<tb_BuyingRequisition>();


                //这部分要不要重构到时再看
                querySqlQueryable = MainForm.Instance.AppContext.Db.Queryable<tb_BuyingRequisition>()
                   //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                   .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                   .WhereIF(LimitQueryConditions != null, LimitQueryConditions)
                   .Where(true, queryConditions, QueryDto);
                //.Where(it => SqlFunc.Subqueryable<tb_CustomerVendor>().Where(sql).Where(s => s.CustomerVendor_ID == it.CustomerVendor_ID).Any());

                // querySqlQueryable = querySqlQueryable.InnerJoinIF<tb_CustomerVendor>(true, (a, b) => a.CustomerVendor_ID == b.CustomerVendor_ID).Where(sql);
                querySqlQueryable = querySqlQueryable.Where(@" EXISTS (SELECT * FROM [tb_CustomerVendor] WHERE ( isdeleted = '0' AND Is_available = '1' AND IsVendor = '1' AND Is_enabled = '1' AND IsExclusive = '0' ) or ( IsExclusive = '1' AND Employee_ID = 1740614448332279808 )) ");
            }


            list = await querySqlQueryable.ToPageListAsync(pageNum, pageSize) as List<tb_BuyingRequisition>;
            _UCBillMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();
            _UCBillMasterQuery.ShowSum();


        }
        */


        public override void BuildInvisibleCols()
        {
            //base.ChildInvisibleCols.Add(c => c.Cost);
            base.MasterInvisibleCols.Add(c => c.RefBillID);
            base.BuildInvisibleCols();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(e => e.TotalQty);
            base.ChildSummaryCols.Add(e => e.Quantity);
        }

        /*
        /// <summary>
        /// 批量转换为采购入库单
        /// </summary>
        public async override void BatchConversion()
        {
            tb_PurEntryController<tb_PurEntry> ctr = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
            List<tb_BuyingRequisition> selectlist = GetSelectResult();
            int conter = 0;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_PurEntries != null && item.tb_PurEntries.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.PurOrderNo}：已经生成过入库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }

                    }

                    tb_PurEntry purEntry = PurOrderTotb_PurEntry(item);
                    if (purEntry.tb_PurEntryDetails.Count > 0)
                    {
                        ReturnMainSubResults<tb_PurEntry> rsrs = await ctr.BaseSaveOrUpdateWithChild<tb_PurEntry>(purEntry);
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
                        MainForm.Instance.uclog.AddLog("转换出错,出库明细转换结果为0行，请检查后重试。");
                    }

                }
                else
                {
                    MainForm.Instance.uclog.AddLog(string.Format("当前订单:{0}的状态为{1},不能转换为采购入库单。", item.PurOrderNo, ((DataStatus)item.DataStatus).ToString()));
                    continue;
                }

            }
            MainForm.Instance.uclog.AddLog("转换完成,成功转换的订单数量:" + conter);
        }
        */

        /*
        /// <summary>
        /// 转换为采购入库单,注意一个订单可以多次转成入库单。
        /// </summary>
        /// <param name="order"></param>
        public tb_PurEntry PurOrderTotb_PurEntry(tb_BuyingRequisition order)
        {
            tb_PurEntry entity = new tb_PurEntry();
            //转单
            if (order != null)
            {
                

                entity = mapper.Map<tb_PurEntry>(order);
                List<tb_PurEntryDetail> details = mapper.Map<List<tb_PurEntryDetail>>(order.tb_BuyingRequisitionDetails);
                //转单要TODO
                //转换时，默认认为订单出库数量就等于这次出库数量，是否多个订单累计？，如果是UI录单。则只是默认这个数量。也可以手工修改
                List<tb_PurEntryDetail> NewDetails = new List<tb_PurEntryDetail>();
                foreach (tb_PurEntryDetail item in details)
                {
                    tb_BuyingRequisitionDetail orderDetail = new tb_BuyingRequisitionDetail();
                    orderDetail = order.tb_BuyingRequisitionDetails.FirstOrDefault<tb_BuyingRequisitionDetail>(c => c.ProdDetailID == item.ProdDetailID);
                    if (orderDetail != null)
                    {
                        //已经入库数量等于已经入库数量则认为这项全入库了，不再出
                        if (orderDetail.DeliveredQuantity == item.Quantity)
                        {
                            continue;
                        }
                        else
                        {
                            item.Quantity = item.Quantity - orderDetail.DeliveredQuantity;
                            NewDetails.Add(item);
                        }
                    }

                }
                entity.tb_PurEntryDetails = NewDetails;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;

                if (NewDetails.Count != details.Count)
                {
                    //已经出库过，第二次不包括 运费
                    entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                    entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                    entity.TotalTaxAmount = NewDetails.Sum(c => c.TaxAmount);
                    entity.ActualAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                }

                if (order.Arrival_date.HasValue)
                {
                    entity.EntryDate = order.Arrival_date.Value;
                }
                else
                {
                    entity.EntryDate = System.DateTime.Now;
                }

                entity.PrintStatus = 0;
                BusinessHelper.Instance.InitEntity(entity);

                if (entity.PurOrder_ID.HasValue && entity.PurOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = order.CustomerVendor_ID;
                    entity.PurOrder_NO = order.PurOrderNo;
                }
                entity.PurEntryNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购入库单);
                //保存到数据库
                BusinessHelper.Instance.InitEntity(entity);
            }
            return entity;
        }
        */

    }



}
