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
using SqlSugar;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.BI;
using RUINORERP.Common.Helper;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using System.Collections;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.MRP.BOM
{
    [MenuAttrAssemblyInfo("产品配方追溯", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.MRP基本资料, BizType.BOM物料清单)]
    public partial class UCBillOfMaterialsTracker : BaseForm.BaseListGeneric<tb_BOM_SDetail>
    {
        public UCBillOfMaterialsTracker()
        {
            InitializeComponent();
            //生成查询条件的相关实体
            base.EditForm = null;
            GridRelated.SetRelatedInfo<tb_BOM_SDetail, tb_BOM_S>(c => c.BOM_ID, r => r.BOM_ID);
 
            // ReladtedEntityType = typeof(tb_Prod);
            this.Load += UCInventoryQuery_Load;
            toolStripButtonSave.Visible = false;
            toolStripButtonModify.Visible = false;
            toolStripButtonAdd.Visible = false;
            toolStripButtonDelete.Visible = false;


        }

        private void UCInventoryQuery_Load(object sender, EventArgs e)
        {
            //表格显示时DataGridView1_CellFormatting 取外键类型
            ColDisplayTypes.Add(typeof(tb_Prod));
            ColDisplayTypes.Add(typeof(tb_Unit));
            ColDisplayTypes.Add(typeof(tb_BOM_S));
        }


        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_BOM_SDetail).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildInvisibleCols()
        {
            base.InvisibleColsExp.Add(c => c.ProdDetailID);
        }


        public override void BuildSummaryCols()
        {
            SummaryCols.Add(c => c.UsedQty);
        }


        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
        //[MustOverride]
        public async override void Query(bool UseAutoNavQuery = false)
        {
            if (Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }
            //MainForm.Instance.logger.LogInformation("查询库存");
            if (QueryConditionFilter == null || QueryConditionFilter.QueryFields == null || QueryConditionFilter.QueryFields.Count == 0)
            {
                dataGridView1.ReadOnly = true;

                //两种条件组合为一起，一种是process中要处理器中设置好的，另一个是UI中 灵活设置的
                Expression<Func<tb_BOM_SDetail, bool>> expression = QueryConditionFilter.GetFilterExpression<tb_BOM_SDetail>();
                List<tb_BOM_SDetail> list = await MainForm.Instance.AppContext.Db.Queryable<tb_BOM_SDetail>().WhereIF(expression != null, expression).ToListAsync();

                List<string> masterlist = ExpressionHelper.ExpressionListToStringList(SummaryCols);
                if (masterlist.Count > 0)
                {
                    dataGridView1.IsShowSumRow = true;
                    dataGridView1.SumColumns = masterlist.ToArray();
                }

                ListDataSoure.DataSource = list.ToBindingSortCollection();
                dataGridView1.DataSource = ListDataSoure;

            }
            else
            {
                ExtendedQuery();
            }
            ToolBarEnabledControl(MenuItemEnums.查询);


        }

        /// <summary>
        /// 扩展带条件查询
        /// </summary>
        protected async override void ExtendedQuery(bool UseAutoNavQuery = false)
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;

            dataGridView1.ReadOnly = true;

            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要

            int pageNum = 1;
            int pageSize = int.Parse(base.txtMaxRows.Text);

            //提取指定的列名，即条件集合
            // List<string> queryConditions = new List<string>();
            //queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
            //list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, queryConditions, QueryConditionFilter.GetFilterExpression<T>(), QueryDto, pageNum, pageSize) as List<T>;
            List<tb_BOM_SDetail> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDto, pageNum, pageSize) as List<tb_BOM_SDetail>;

            List<string> masterlist = ExpressionHelper.ExpressionListToStringList(SummaryCols);
            if (masterlist.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = masterlist.ToArray();
            }

            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;

            ToolBarEnabledControl(MenuItemEnums.查询);
        }



    }
}
