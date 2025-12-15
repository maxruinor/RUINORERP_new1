using AutoMapper;
using NPOI.SS.Formula.Functions;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business.Processor;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("产品属性关联管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCProductAttrRelationList : BaseForm.BaseListGeneric<tb_Prod_Attr_Relation>
    {
        public UCProductAttrRelationList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCProductAttrRelationEdit);
            DisplayTextResolver.AddReferenceKeyMapping<View_ProdDetail, tb_Prod_Attr_Relation>(t => t.ProdDetailID, s => s.ProdDetailID, t => t.SKU);
            // 初始化产品主信息下拉框
            InitProductComboBox();
        }

        /// <summary>
        /// 初始化产品下拉框
        /// </summary>
        private void InitProductComboBox()
        {
            // 这里可以根据实际需要实现产品下拉框的初始化
        }

        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Prod_Attr_Relation).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            QueryConditionFilter.FilterLimitExpressions.Clear();
        }
    }
}