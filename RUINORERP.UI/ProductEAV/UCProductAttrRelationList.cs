using System;
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
using RUINORERP.Model.Base;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("产品属性关联管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCProductAttrRelationList : BaseForm.BaseListGeneric<tb_Prod_Attr_Relation>
    {
        public UCProductAttrRelationList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCProductAttrRelationEdit);

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

        /// <summary>
        /// 自定义查询，根据选定的产品查询其属性关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryByProduct_Click(object sender, EventArgs e)
        {
            // 这里可以根据实际需要实现按产品查询的功能
            Query();
        }
    }
}