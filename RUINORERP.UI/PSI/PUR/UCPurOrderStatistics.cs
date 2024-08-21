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
    [MenuAttrAssemblyInfo("采购订单统计", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.采购订单统计)]
    public partial class UCPurOrderStatistics : BaseNavigatorGeneric<View_PurOrderItems, View_PurOrderItems>
    {
        public UCPurOrderStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_PurOrderItems);

            base.WithOutlook = true;
        }

        public override List<NavParts[]> AddNavParts()
        {
            List<NavParts[]> strings = new List<NavParts[]>();
            strings.Add(new NavParts[] { NavParts.查询结果, NavParts.分组显示 });
            return strings;
        }

        private void UCPurOrderStatistics_Load(object sender, EventArgs e)
        {
            //不能在上面的构造函数里面初始化
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_PurOrderItems, tb_PurOrder>(c => c.PurOrderNo, r => r.PurOrder_ID);
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_PurOrderItems, tb_SaleOrder>(c => c.SOrder_ID, r => r.SOrder_ID);

            //这个应该是一个组 多个表
            // base._UCBillMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();


            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_PurOrder));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_PurOrderDetail));
            

            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;

            _UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_PurOrderItems, tb_SaleOrder>(c => c.SOrder_ID, r => r.SOrder_ID);

        }
        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            //System.Linq.Expressions.Expression<Func<, int?>> exprApprovalStatus;
            //exprApprovalStatus = (p) => p.ApprovalStatus;
            //base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(ApprovalStatus)));


            //System.Linq.Expressions.Expression<Func<tb_SaleOrder, int?>> exprPayStatus;
            //exprPayStatus = (p) => p.PayStatus;
            //base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(PayStatus)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

            /*
            //View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            List<View_ProdDetail> list = new List<View_ProdDetail>();
            list = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().ToList();
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);

            */
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_PurOrderItems>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            //.And(t => t.isdeleted == false)

                            //.And(t => t.Is_enabled == true)

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_PurOrderItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);
            base.MasterSummaryCols.Add(c => c.SubtotalAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PurOrder_ID);
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }

    }
}
