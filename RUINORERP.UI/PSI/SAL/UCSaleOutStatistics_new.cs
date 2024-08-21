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

namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("销售出库统计", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理, BizType.销售出库统计)]
    public partial class UCSaleOutStatistics_new : BaseNavigatorPages<View_SaleOutItems, View_SaleOutItems>
    {
        public UCSaleOutStatistics_new()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_SaleOutItems);
            base.WithOutlook = true;

          
        }
        /// <summary>
        /// 构建导航菜单
        /// </summary>
        public override Navigator BuildNavigatorMenu()
        {
            Navigator navigator = new Navigator();
            NavigatorMenu navigator1 = new NavigatorMenu();
            navigator1.MenuLevel = 0;
            navigator1.MenuSort = 0;
            navigator1.menuType = NavigatorMenuType.销售明细分析;
            navigator1.MenuName = navigator1.menuType.ToString();


            NavigatorMenu navigator2 = new NavigatorMenu();
            navigator2.MenuLevel = 0;
            navigator2.MenuSort = 1;
            navigator2.menuType = NavigatorMenuType.销售数据汇总;
            navigator2.MenuName = navigator2.menuType.ToString();
            navigator.NavigatorMenus.Add(navigator1);
            navigator.NavigatorMenus.Add(navigator2);
            return navigator;
        }



        private void UCSaleOutStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCBillMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_SaleOut));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_SaleOutDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Unit));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProdCategories));

            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
        }

        public override void BuildColNameDataDictionary()
        {
            
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_SaleOutItems>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            //.And(t => t.isdeleted == false)

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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_SaleOutItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);
            base.MasterSummaryCols.Add(c => c.SubtotalTransAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }


    }
}
