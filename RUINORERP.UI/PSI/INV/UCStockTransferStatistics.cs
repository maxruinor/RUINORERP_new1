﻿using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.ReportData;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Report;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.PSI.INV
{

    [MenuAttrAssemblyInfo("调拨明细统计", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.调拨明细统计)]
    public partial class UCStockTransferStatistics : BaseNavigatorGeneric<View_StockTransferItems, View_StockTransferItems>
    {
        public UCStockTransferStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 视图时要指定
            ReladtedEntityType = typeof(View_StockTransferItems);
        }

        public override List<NavParts[]> AddNavParts()
        {
            List<NavParts[]> strings = new List<NavParts[]>();
            strings.Add(new NavParts[] { NavParts.查询结果, NavParts.分组显示 });
            return strings;
        }



        private void UCPurEntryStatistics_Load(object sender, EventArgs e)
        {
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_StockTransferItems, tb_StockTransfer>(c => c.StockTransferNo, r => r.StockTransferNo);
            
            
            ////这个应该是一个组 多个表
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail)); 
            ////是否能通过一两个主表，通过 外键去找多级关联的表？
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_PurOrder));
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_PurOrderDetail));

            //base._UCBillOutlookGridAnalysis.ColDisplayTypes = base._UCBillMasterQuery.ColDisplayTypes;
            //base._UCMasterQuery.newSumDataGridViewMaster.Use是否使用内置右键功能 = false;
            //base._UCMasterQuery.newSumDataGridViewMaster.ContextMenuStrip = contextMenuStrip1;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_StockTransferItems, tb_StockTransfer>(c => c.StockTransferNo, r => r.StockTransferNo);
        }


        public override void BuildColDisplayTypes()
        {
            MasterColDisplayTypes = new List<Type>();
            MasterColDisplayTypes.Add(typeof(tb_Prod));
            MasterColDisplayTypes.Add(typeof(tb_ProductType));
            MasterColDisplayTypes.Add(typeof(tb_PaymentMethod));
            MasterColDisplayTypes.Add(typeof(View_ProdDetail));
        }

        public override void BuildColNameDataDictionary()
        {
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_StockTransferItems>()
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_StockTransferItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Qty);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }

        /*
        //根据客户生成对帐单 数据模式是 客户信息为主，明细为子表

        public override async void Print(RptMode rptMode)
        {
            //构建对账单的数据
            PurEntryStatementByCV statementByCV = new PurEntryStatementByCV();


            List<View_StockTransferItems> selectlist = GetSelectResult();
            if (selectlist.Count == 0)
            {
                MessageBox.Show("没有需要打印的数据，请将客户作为查询条件查询出结果后再打印。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //配置还是当前类，菜单这块定义的
            if (_PrintConfig == null || _PrintConfig.tb_PrintTemplates == null)
            {
                _PrintConfig = PrintHelper<View_StockTransferItems>.GetPrintConfig<View_StockTransferItems>();
            }


            tb_CustomerVendor customerVendor = new tb_CustomerVendor();
            var dto = QueryDto as View_StockTransferItems;
            if (!dto.CustomerVendor_ID.HasValue)
            {
                MessageBox.Show("缺少客户信息，请将客户作为查询统计的条件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                customerVendor = await Startup.GetFromFac<tb_CustomerVendorController<tb_CustomerVendor>>().BaseQueryByIdAsync(dto.CustomerVendor_ID.Value);
                statementByCV.CustomerVendor = customerVendor;
                statementByCV.PurEntryItems = selectlist;
            }
            bool rs = PrintHelper<View_StockTransferItems>.PrintCustomData<PurEntryStatementByCV>(statementByCV, rptMode, _PrintConfig);
            if (rs)
            {
                toolStripSplitButtonPrint.Enabled = false;
            }
        }

        */
    }
}