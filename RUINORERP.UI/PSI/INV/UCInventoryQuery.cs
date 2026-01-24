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
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.PSI.INV
{
   
    [MenuAttrAssemblyInfo("库存查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理, BizType.库存查询)]
    public partial class UCInventoryQuery : BaseForm.BaseListGeneric<View_Inventory>, IToolStripMenuInfoAuth
    {
        public UCInventoryQuery()
        {
            InitializeComponent();
            //生成查询条件的相关实体
            base.EditForm = null;
            System.Linq.Expressions.Expression<Func<tb_Prod, int?>> expr;
            expr = (p) => p.SourceType;
            base.ColNameDataDictionary.TryAdd(expr.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource)));

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
            ColDisplayTypes.Add(typeof(tb_ProdDetail));
        }

        #region 添加成本确认

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            ToolStripButton toolStripButton成本确认 = new System.Windows.Forms.ToolStripButton();
            toolStripButton成本确认.Text = "成本确认";
            toolStripButton成本确认.Image = global::RUINORERP.UI.Properties.Resources.MakeSureCost;
            toolStripButton成本确认.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton成本确认.Name = "成本确认MakesureCost";
            toolStripButton成本确认.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton成本确认);
            toolStripButton成本确认.ToolTipText = "快速确认成本。";
            toolStripButton成本确认.Click += new System.EventHandler(this.toolStripButton成本确认_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton成本确认 };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;

        }

        private async void toolStripButton成本确认_Click(object sender, EventArgs e)
        {
            if (bindingSourceList.Current != null && dataGridView1.CurrentCell != null)
            {
                //  弹出提示说：您确定将这个公司回收投入到公海吗？
                if (bindingSourceList.Current is View_Inventory ViewInventory)
                {
                    var amountRule = new AmountValidationRule();
                    using (var inputForm = new frmInputObject(amountRule))
                    {
                        inputForm.DefaultTitle = "请输入成本价格";
                        tb_Inventory Inventory = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>().Where(c => c.Inventory_ID == ViewInventory.Inventory_ID).SingleAsync();
                        if (inputForm.ShowDialog() == DialogResult.OK)
                        {
                            if (MessageBox.Show($"确定将产品:【{ViewInventory.SKU + "-" + ViewInventory.CNName}】库存成本设为：{inputForm.InputContent}吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                Inventory.CostFIFO = inputForm.InputContent.ToDecimal();
                                Inventory.CostMonthlyWA = inputForm.InputContent.ToDecimal();
                                Inventory.CostMovingWA = inputForm.InputContent.ToDecimal();
                                Inventory.Inv_AdvCost = inputForm.InputContent.ToDecimal();
                                Inventory.Inv_Cost = inputForm.InputContent.ToDecimal();

                                int result = await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(Inventory).UpdateColumns(t => new { t.CostFIFO, t.CostMonthlyWA, t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                                if (result > 0)
                                {
                                    MainForm.Instance.ShowStatusText("成本确认成功!");
                                    QueryAsync();
                                }
                            }
                        }
                    }

                }
            }
        }


        #endregion


        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_Inventory).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildInvisibleCols()
        {
            base.InvisibleColsExp.Add(c => c.ProdDetailID);
        }


        public override void BuildSummaryCols()
        {
            SummaryCols.Add(c => c.Quantity);
            SummaryCols.Add(c => c.Inv_SubtotalCostMoney);
        }


        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
        //[MustOverride]
        public async override void QueryAsync(bool UseNavQuery = false)
        {
            if (Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }
            if (QueryConditionFilter == null || QueryConditionFilter.QueryFields == null || QueryConditionFilter.QueryFields.Count == 0)
            {
                dataGridView1.ReadOnly = true;

                //两种条件组合为一起，一种是process中要处理器中设置好的，另一个是UI中 灵活设置的
                Expression<Func<View_Inventory, bool>> expression = QueryConditionFilter.GetFilterExpression<View_Inventory>();
                List<View_Inventory> list = await MainForm.Instance.AppContext.Db.Queryable<View_Inventory>().WhereIF(expression != null, expression).ToListAsync();

                List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
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
            List<View_Inventory> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDtoProxy, pageNum, pageSize) as List<View_Inventory>;

            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
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
