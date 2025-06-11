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
using Org.BouncyCastle.Crypto.Prng;

namespace RUINORERP.UI.PSI.PUR
{
    [MenuAttrAssemblyInfo("采购订单统计", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购订单统计)]
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

            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_PurOrderItems, tb_PurOrder>(c => c.PurOrderNo, r => r.PurOrder_ID);
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_PurOrderItems, tb_SaleOrder>(c => c.SOrder_ID, r => r.SOrder_ID);
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_PurOrderItems>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            //.And(t => t.isdeleted == false)
                            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
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
            base.MasterInvisibleCols.Add(c => c.SOrder_ID);
        }

    }
}
