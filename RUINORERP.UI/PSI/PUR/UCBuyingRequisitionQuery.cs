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
        public override void QueryConditionBuilder()
        {
            //base.QueryConditions.Add(c => c.PurOrderNo);
            //base.QueryConditions.Add(c => c.PurDate);
            //base.QueryConditions.Add(c => c.Employee_ID);
            //base.QueryConditions.Add(c => c.CustomerVendor_ID);
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_BuyingRequisition).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

        }

        /*
        protected override async Task Query()
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

    

      

    }



}
