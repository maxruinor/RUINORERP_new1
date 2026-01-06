﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;

using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using SqlSugar;
using Krypton.Navigator;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;



using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using RUINORERP.Global.Model;

using RUINORERP.Business.CommService;
using FastReport.Table;

namespace RUINORERP.UI.EOP
{
    /// <summary>
    /// 蓄水查询
    /// </summary>
    [MenuAttrAssemblyInfo("蓄水查询", ModuleMenuDefine.模块定义.电商运营, ModuleMenuDefine.电商运营.蓄水管理, BizType.蓄水订单)]
    public partial class UCEOPWaterStorageQuery : BaseBillQueryMC<tb_EOP_WaterStorage, tb_EOP_WaterStorage>
    {
        public UCEOPWaterStorageQuery()
        {
            InitializeComponent();
            //可以设置双击打开单据的字段 关联单据字段设置
            base.RelatedBillEditCol = (c => c.WSRNo);

            //标记没有明细子表
            HasChildData = false;
            ResultAnalysis = true;
        }

        public override void AddExcludeMenuList()
        {
            //base.AddExcludeMenuList(MenuItemEnums.反结案);
            //base.AddExcludeMenuList(MenuItemEnums.结案);
        }


        public override void BuildLimitQueryConditions()
        {


            //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_EOP_WaterStorage).Name + "Processor");
            //QueryConditionFilter = baseProcessor.GetQueryFilter();
            ////非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            //var lambda = Expressionable.Create<tb_EOP_WaterStorage>()
            //                .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
            //  .ToExpression();
            //QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            var lambda = Expressionable.Create<tb_EOP_WaterStorage>()
                              .And(t => t.isdeleted == false)
                              .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                         .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }







        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.WSR_ID);
            base.ChildInvisibleCols.Add(c => c.WSR_ID);
        }



        protected async override void Delete(List<tb_EOP_WaterStorage> Datas)
        {
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
                        BaseController<tb_EOP_WaterStorage> ctr = Startup.GetFromFacByName<BaseController<tb_EOP_WaterStorage>>(typeof(tb_EOP_WaterStorage).Name + "Controller");
                        bool rs = await ctr.BaseDeleteAsync(item);
                        if (rs)
                        {
                            counter++;
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

        private void UCPreReceivedPaymentQuery_Load(object sender, EventArgs e)
        {
            if (ResultAnalysis && _UCOutlookGridAnalysis1 != null)
            {
                _UCOutlookGridAnalysis1.kryptonOutlookGrid1.SetSubtotalColumns<tb_EOP_WaterStorage>(e => e.TotalAmount);
            }

        }
    }
}
