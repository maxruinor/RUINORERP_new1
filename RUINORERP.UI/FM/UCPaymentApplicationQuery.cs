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
using RUINORERP.Model.TransModel;
using RUINORERP.UI.SuperSocketClient;
using TransInstruction;
using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("付款申请单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.付款管理, BizType.付款申请单)]
    public partial class UCPaymentApplicationQuery : BaseBillQueryMC<tb_FM_PaymentApplication, tb_FM_PaymentApplication>
    {
        public UCPaymentApplicationQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ApplicationNo);
            //标记没有明细子表
            HasChildData = false;
        }

        public override void BuildLimitQueryConditions()
        {
            var lambda = Expressionable.Create<tb_FM_PaymentApplication>()
                              .And(t => t.isdeleted == false)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                             t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.OverpaymentAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrePaymentBill_id);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }



        protected async override void Delete(List<tb_FM_PaymentApplication> Datas)
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
                        BaseController<tb_FM_PaymentApplication> ctr = Startup.GetFromFacByName<BaseController<tb_FM_PaymentApplication>>(typeof(tb_FM_PaymentApplication).Name + "Controller");
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




    }
}
