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

namespace RUINORERP.UI.MRP.PPROC
{
    [MenuAttrAssemblyInfo("缴库明细统计", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.缴库明细统计)]
    public partial class UCFinishedGoodsInvStatistics : BaseNavigatorGeneric<View_FinishedGoodsInvItems, View_FinishedGoodsInvItems>
    {
        public UCFinishedGoodsInvStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_FinishedGoodsInvItems);
            base.WithOutlook = true;
          
        }

        private void UCSaleOutStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_FinishedGoodsInvItems,tb_ManufacturingOrder>(c => c.MONo, r => r.MONO);
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_FinishedGoodsInvItems, tb_FinishedGoodsInv>(c => c.DeliveryBillNo, r => r.DeliveryBillNo);
            
            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FinishedGoodsInv));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FinishedGoodsInvDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Unit));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProdCategories));

            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_FinishedGoodsInvItems, tb_ManufacturingOrder>(c => c.MONo, r => r.MONO);
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_FinishedGoodsInvItems, tb_FinishedGoodsInv>(c => c.DeliveryBillNo, r => r.DeliveryBillNo);
 
        }

        public override void BuildColNameDataDictionary()
        {
            
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_FinishedGoodsInvItems>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)

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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_FinishedGoodsInvItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
          
        }

        public override void BuildSummaryCols()
        {
            //视图只有一个实体结果
            base.MasterSummaryCols.Add(c => c.Quantity);
  

        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }


    }
}