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

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("盘点单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.盘点单)]
    public partial class UCStocktakeQuery : BaseBillQueryMC<tb_Stocktake, tb_StocktakeDetail>
    {
        public UCStocktakeQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.CheckNo);
        }

        public override void BuildColNameDataDictionary()
        {

            System.Linq.Expressions.Expression<Func<tb_Stocktake, int?>> exprAdjust_Type;
            exprAdjust_Type = (p) => p.Adjust_Type;
            base.MasterColNameDataDictionary.TryAdd(exprAdjust_Type.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Adjust_Type)));

            System.Linq.Expressions.Expression<Func<tb_Stocktake, int?>> exprCheckMode;
            exprCheckMode = (p) => p.CheckMode;
            base.MasterColNameDataDictionary.TryAdd(exprCheckMode.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(CheckMode)));

            base.BuildColNameDataDictionary();
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_Stocktake>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                            // .And(t => t.Is_enabled == true)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Stocktake).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.CarryingTotalQty);
            base.MasterSummaryCols.Add(c => c.CheckTotalQty);
            base.MasterSummaryCols.Add(c => c.DiffTotalQty);




        }


    }
}
