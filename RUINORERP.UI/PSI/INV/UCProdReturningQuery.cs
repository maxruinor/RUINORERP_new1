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
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("归还单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.归还单)]
    public partial class UCProdReturningQuery : BaseBillQueryMC<tb_ProdReturning, tb_ProdReturningDetail>
    {

        public UCProdReturningQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ReturnNo);
            base.tsbtnAntiApproval.Visible = false;
            base.tsbtnBatchConversion.Visible = false;

        }
        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_ProdReturning, tb_ProdBorrowing>(c => c.BorrowNO, r => r.BorrowNo);
            base.SetGridViewDisplayConfig();
        }
        

        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_ProdReturning, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));

            System.Linq.Expressions.Expression<Func<tb_ProdReturning, int?>> exprDataStatus;
            exprDataStatus = (p) => p.DataStatus;
            base.MasterColNameDataDictionary.TryAdd(exprDataStatus.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));

            //System.Linq.Expressions.Expression<Func<tb_ProdReturning, int?>> exprPayStatus;
            //exprPayStatus = (p) => p.;
            //base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(PayStatus)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_ProdReturningDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

           
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_ProdReturningDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_ProdReturning>()
                             .And(t => t.isdeleted == false)
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }
 
        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdReturning).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_ProdReturning>()
                .And(t => t.isdeleted == false)
                .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
              .ToExpression();

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.ChildSummaryCols.Add(c => c.Qty);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.TotalCost);
            base.ChildInvisibleCols.Add(c => c.Cost);
            base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


    }
}
