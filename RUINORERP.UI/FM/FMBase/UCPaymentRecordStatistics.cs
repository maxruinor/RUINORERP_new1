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
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.FM.FMBase
{

    public partial class UCPaymentRecordStatistics : BaseNavigatorGeneric<View_FM_PaymentRecordItems, View_FM_PaymentRecordItems>
    {
        public UCPaymentRecordStatistics()
        {
            InitializeComponent();
            ReladtedEntityType = typeof(View_FM_PaymentRecordItems);
            base.WithOutlook = true;

        }

        /// <summary>
        /// 收付款方式决定是不收入还是支出。收款=收入， 支出=付款
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }

        private void UCSaleOrderStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_FM_PaymentRecordItems, tb_FM_PaymentRecord>(c => c.PaymentNo, r => r.PaymentNo);

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProjectGroup));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_Subject));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Department));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_Account));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Employee));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_PaymentRecord));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Currency));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_PaymentMethod));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_CustomerVendor));
            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_FM_PaymentRecordItems, tb_FM_PaymentRecord>(c => c.PaymentNo, r => r.PaymentNo);


            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCMasterQuery.GridRelated.SetComplexTargetField<View_FM_PaymentRecordItems>(c => c.SourceBizType, c => c.SourceBillNo);
//            base._UCMasterQuery.GridRelated.SetComplexTargetField<View_FM_PaymentRecordItems>(c => c.TargetBizType, c => c.TargetBillNo);
            //将枚举中的值循环
            foreach (var biztype in Enum.GetValues(typeof(BizType)))
            {
                
                var tableName = Business.BizMapperService.EntityMappingHelper.GetEntityInfo((BizType)biztype);
                if (tableName == null)
                {
                    continue;
                }
                ////这个参数中指定要双击的列单号。是来自另一组  一对一的指向关系
                //因为后面代码去查找时，直接用的 从一个对象中找这个列的值。但是枚举显示的是名称。所以这里直接传入枚举的值。
                KeyNamePair keyNamePair = new KeyNamePair(((int)((BizType)biztype)).ToString(), tableName.TableName);
                base._UCMasterQuery.GridRelated.SetRelatedInfo<View_FM_PaymentRecordItems>(c => c.SourceBillNo, keyNamePair);
                //base._UCMasterQuery.GridRelated.SetRelatedInfo<View_FM_PaymentRecordItems>(c => c.TargetBillNo, keyNamePair);
            }
            #endregion

        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_FM_PaymentRecordItems>()
                           .And(t => t.ReceivePaymentType == (int)PaymentType)
                            .And(t => t.isdeleted == false)
                            //.And(t => t.Is_enabled == true)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_FM_PaymentRecordItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.LocalAmount);
            base.MasterSummaryCols.Add(c => c.ForeignAmount);
            base.MasterSummaryCols.Add(c => c.TotalLocalAmount);
            base.MasterSummaryCols.Add(c => c.TotalForeignAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PaymentId);
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            base.MasterInvisibleCols.Add(c => c.ReversedByPaymentId);
            base.MasterInvisibleCols.Add(c => c.ReversedOriginalId);
            base.MasterInvisibleCols.Add(c => c.PaymentDetailId);
            base.MasterInvisibleCols.Add(c => c.PaymentId);
        }


    }
}
