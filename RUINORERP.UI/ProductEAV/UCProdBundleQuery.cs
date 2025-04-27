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
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("套装组合查询", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料, BizType.套装组合)]
    public partial class UCProdBundleQuery : BaseBillQueryMC<tb_ProdBundle, tb_ProdBundleDetail>
    {

        public UCProdBundleQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.BundleName);
  

        }

        /*
        public override Task<bool> ReReview(List<tb_ProdBundle> EditEntitys)
        {
            MessageBox.Show("请在单据明细中使用反审功能。");
            return null;
        }
        */
     

        public override void BuildColNameDataDictionary()
        {
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_ProdBundleDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);
            base.BuildColNameDataDictionary();
        }

     
        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_ProdBundle>()
                             .And(t => t.isdeleted == false)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdBundle).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            //base.MasterSummaryCols.Add(c => c.Market_Price);
            base.ChildSummaryCols.Add(c => c.Quantity);
            
        }

        public override void BuildInvisibleCols()
        {
          
        }


    }
}
