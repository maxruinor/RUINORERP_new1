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

namespace RUINORERP.UI.ASS
{
    [MenuAttrAssemblyInfo("售后申请统计", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.售后流程, BizType.售后申请统计)]
    public partial class UCASAfterSaleApplyStatistics : BaseNavigatorGeneric<View_AS_AfterSaleApplyItems, View_AS_AfterSaleApplyItems>
    {
        public UCASAfterSaleApplyStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_AS_AfterSaleApplyItems);
            base.WithOutlook = true;


        }

        


        private void UCSaleOutStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_AS_AfterSaleApplyItems,tb_AS_AfterSaleApply>(c => c.ASApplyNo, r => r.ASApplyNo);

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_AS_AfterSaleApply));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_AS_AfterSaleApplyDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Unit));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProdCategories));

            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_AS_AfterSaleApplyItems, tb_AS_AfterSaleApply>(c => c.ASApplyNo, r => r.ASApplyNo);
        }

        //public override void BuildColNameDataDictionary()
        //{
        //    System.Linq.Expressions.Expression<Func<tb_SaleOut, int?>> exprPayStatus;
        //    exprPayStatus = (p) => p.PayStatus;
        //    base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus)));
        //    base.BuildColNameDataDictionary();
        //}


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_AS_AfterSaleApplyItems>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            //.And(t => t.isd == false)

                            //.And(t => t.Is_enabled == true)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_AS_AfterSaleApplyItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);
            base.MasterSummaryCols.Add(c => c.ConfirmedQuantity);
            base.MasterSummaryCols.Add(c => c.InitialQuantity);
            base.MasterSummaryCols.Add(c => c.DeliveredQty);
            base.MasterSummaryCols.Add(c => c.TotalConfirmedQuantity);
            base.MasterSummaryCols.Add(c => c.TotalDeliveredQty);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }


    }
}
