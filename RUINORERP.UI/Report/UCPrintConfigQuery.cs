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
using SqlSugar;
using RUINORERP.Common.Extensions;
using System.Collections;
using RUINORERP.Model.Base;
using RUINORERP.Business.Security;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Report
{

    [MenuAttrAssemblyInfo("打印配置查询", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具, BizType.默认数据)]
    public partial class UCPrintConfigQuery : BaseBillQueryMC<tb_PrintConfig, tb_PrintTemplate>
    {
        public UCPrintConfigQuery()
        {
            InitializeComponent();

            //显示转出库单
            tsbtnBatchConversion.Visible = false;
            toolStripButton结案.Visible = false;
            toolStripbtnSubmit.Visible = false;
            toolStripbtnApprove.Visible = false;
            tsbtnAntiApproval.Visible = false;

        }

        protected async override void Delete(List<tb_PrintConfig> Datas)
        {
            return;
            if (Datas == null || Datas.Count == 0)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                int counter = 0;
                foreach (var item in Datas)
                {
                    //https://www.runoob.com/w3cnote/csharp-enum.html
                    var dataStatus = (DataStatus)(item.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                    if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                    {
                        BaseController<tb_PrintConfig> ctr = Startup.GetFromFacByName<BaseController<tb_PrintConfig>>(typeof(tb_PrintConfig).Name + "Controller");
                        object PKValue = item.GetPropertyValue(UIHelper.GetPrimaryKeyColName(typeof(tb_PrintConfig)));
                        bool rs = await ctr.BaseDeleteByNavAsync(item as tb_PrintConfig);
                        if (rs)
                        {
                            counter++;
                            MainForm.Instance.logger.LogInformation($"查询列表中删除:{typeof(tb_PrintConfig).Name}，主键值：{PKValue.ToString()} ");
                        }
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                    }
                }
                MainForm.Instance.uclog.AddLog("提示", $"成功删除数据：{counter}条.");
            }
        }


        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给

            System.Linq.Expressions.Expression<Func<tb_SaleOrder, int?>> exprDataStatus;
            exprDataStatus = (p) => p.DataStatus;
            base.MasterColNameDataDictionary.TryAdd(exprDataStatus.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));


            System.Linq.Expressions.Expression<Func<tb_SaleOrder, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));

            System.Linq.Expressions.Expression<Func<tb_SaleOrder, int?>> exprPayStatus;
            exprPayStatus = (p) => p.PayStatus;
            base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);


            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }


        public override void BuildLimitQueryConditions()
        {
            ////创建表达式
            //var lambda = Expressionable.Create<tb_SaleOrder>()
            //                 //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
            //                 // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
            //                 .And(t => t.isdeleted == false)

            //                   // .And(t => t.Is_enabled == true)
            //                   .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制

            //                .ToExpression();//注意 这一句 不能少
            //base.LimitQueryConditions = lambda;
            //QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            //QueryConditionFilter = baseProcessor.GetQueryFilter();

            ////非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            //var lambda = Expressionable.Create<tb_SaleOrder>()
            //    .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            //  .ToExpression();

            //QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void BuildSummaryCols()
        {


        }

        public override void BuildInvisibleCols()
        {

        }







    }
}