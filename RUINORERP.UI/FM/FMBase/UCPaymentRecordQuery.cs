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
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;
using Org.BouncyCastle.Crypto.Prng;
using LiveChartsCore.Geo;
namespace RUINORERP.UI.FM
{

    public partial class UCPaymentRecordQuery : BaseBillQueryMC<tb_FM_PaymentRecord, tb_FM_PaymentRecordDetail>
    {
        public UCPaymentRecordQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.PaymentNo);
        }
        public ReceivePaymentType PaymentType { get; set; }


        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.反审);
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.提交);
        }

        public override void BuildLimitQueryConditions()
        {
            //这里外层来实现对客户供应商的限制
            string customerVendorId = "".ToFieldName<tb_CustomerVendor>(c => c.CustomerVendor_ID);

            //应收付款中的往来单位额外添加一些条件
            var lambdaCv = Expressionable.Create<tb_CustomerVendor>()
                .AndIF(PaymentType == ReceivePaymentType.收款, t => t.IsCustomer == true)
                .AndIF(PaymentType == ReceivePaymentType.付款, t => t.IsVendor == true)
              .ToExpression();
            QueryField queryField = QueryConditionFilter.QueryFields.Where(c => c.FieldName == customerVendorId).FirstOrDefault();
            queryField.SubFilter.FilterLimitExpressions.Add(lambdaCv);


            var lambda = Expressionable.Create<tb_FM_PaymentRecord>()
                              .And(t => t.isdeleted == false)
                             .And(t => t.ReceivePaymentType == (int)PaymentType)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                             t => t.Created_by == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
           
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalForeignAmount);
            base.MasterSummaryCols.Add(c => c.TotalLocalAmount);
            base.ChildSummaryCols.Add(c => c.ForeignAmount);
            base.ChildSummaryCols.Add(c => c.LocalAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.ChildInvisibleCols.Add(c => c.SourceBilllId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            //如果不用户多币种，则不用显示外币
            AuthorizeController authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
            if (authorizeController.EnableMultiCurrency())
            {
                base.MasterInvisibleCols.Add(c => c.TotalForeignAmount);
                base.ChildInvisibleCols.Add(c => c.ExchangeRate);
            }
            base.BuildInvisibleCols();
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }



        protected async override void Delete(List<tb_FM_PaymentRecord> Datas)
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
                    var paymentStatus = (PaymentStatus)(item.GetPropertyValue(typeof(PaymentStatus).Name).ToInt());
                    if (paymentStatus == PaymentStatus.待审核 || paymentStatus == PaymentStatus.草稿)
                    {
                        BaseController<tb_FM_PaymentRecord> ctr = Startup.GetFromFacByName<BaseController<tb_FM_PaymentRecord>>(typeof(tb_FM_PaymentRecord).Name + "Controller");
                        bool rs = await ctr.BaseDeleteAsync(item);
                        if (rs)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("提示", "【未审核】的单据才能删除");
                    }
                }
                MainForm.Instance.uclog.AddLog("提示", $"成功删除数据：{counter}条.");
            }
        }

        private void UCPaymentRecordQuery_Load(object sender, EventArgs e)
        {

            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCBillChildQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillChildQuery.GridRelated.SetComplexTargetField<tb_FM_PaymentRecordDetail>(c => c.SourceBizType, c => c.SourceBillNo);
            BizTypeMapper mapper = new BizTypeMapper();
            //将枚举中的值循环
            foreach (var biztype in Enum.GetValues(typeof(BizType)))
            {
                var tableName = mapper.GetTableType((BizType)biztype);
                if (tableName == null)
                {
                    continue;
                }
                ////这个参数中指定要双击的列单号。是来自另一组  一对一的指向关系
                //因为后面代码去查找时，直接用的 从一个对象中找这个列的值。但是枚举显示的是名称。所以这里直接传入枚举的值。
                KeyNamePair keyNamePair = new KeyNamePair(((int)((BizType)biztype)).ToString(), tableName.Name);
                base._UCBillChildQuery.GridRelated.SetRelatedInfo<tb_FM_PaymentRecordDetail>(c => c.SourceBillNo, keyNamePair);
            }
            #endregion



        }
    }
}
