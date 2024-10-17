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

namespace RUINORERP.UI.PSI.PUR
{

    [MenuAttrAssemblyInfo("采购退货入库查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.采购管理, BizType.采购退货入库)]
    public partial class UCPurReturnEntryQuery : BaseBillQueryMC<tb_PurReturnEntry, tb_PurReturnEntryDetail>
    {
        public UCPurReturnEntryQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PurReEntryNo);
        }
        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_PurReturnEntry, tb_PurEntryRe>(c => c.PurReEntryNo, r => r.PurEntryNo);
            base.SetGridViewDisplayConfig();
        }
        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_PurReturnEntry, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));


            //System.Linq.Expressions.Expression<Func<tb_PurReturnEntry, int?>> exprPayStatus;
            //exprPayStatus = (p) => p.PayStatus;
            //base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(PayStatus)));

            List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, bool?>> expr2;
            expr2 = (p) => p.Gift;// == name;
            base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

            //View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            List<View_ProdDetail> list = new List<View_ProdDetail>();
            list = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().ToList();
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_PurReturnEntryDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }

        public override void BuildInvisibleCols()
        {
        
        }
        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_PurReturnEntry>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)

                               // .And(t => t.Is_enabled == true)
                               .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                                                                                                                                                                                                                                               //.OrIF(AuthorizeController.GetExclusiveLimitedAuth(MainForm.Instance.AppContext), t => t.IsExclusive == true && t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_PurReturnEntry).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalTaxAmount);

            base.MasterSummaryCols.Add(c => c.Deposit);

            base.ChildSummaryCols.Add(c => c.Quantity);


        }







    }



}
