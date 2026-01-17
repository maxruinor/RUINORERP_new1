using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.UI.UCSourceGrid;
using static OpenTK.Graphics.OpenGL.GL;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.CommService;
using Netron.GraphLib;
using NPOI.SS.Formula.Functions;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using HLH.WinControl.MyTypeConverter;
using System.Linq.Expressions;
using RUINORERP.Common.Helper;



namespace RUINORERP.UI.SmartReminderClient
{
    [MenuAttrAssemblyInfo("安全库存配置编辑", true, UIType.单表数据)]
    public partial class UCSafetyStockConfigEdit : BaseEditGeneric<SafetyStockConfig>
    {
        public UCSafetyStockConfigEdit()
        {
            InitializeComponent();
        }


        public SafetyStockConfig safetyStockConfig { get; set; }

        public void BindData(SafetyStockConfig entity)
        {
            if (entity == null)
            {
                entity = new SafetyStockConfig();
            }
            //entity._ProductIds = safetyStockConfig.ProductIds.ToJson();
            safetyStockConfig = entity;
            DataBindingHelper.BindData4TextBox<SafetyStockConfig>(entity, t => t.CheckIntervalByMinutes, txtCheckIntervalByMinutes, BindDataType4TextBox.Qty, false);

            //txt_ProductIds.ReadOnly = true;//要选取，不能手输入。不然格式错误1
            // 绑定到CheckedListBox

            //CheckedListBoxHelper.BindData4CheckedListBox<SafetyStockConfig, long, View_ProdDetail>(
            //    entity: entity,
            //    propertyExpression: e => e.ProductIds,
            //    checkedList: clbProds,
            //    dataSource: MainForm.Instance.list,
            //    idExpression: u => u.ProdDetailID,
            //    displayExpression: u => u.CNName
            //);

            //CheckedListBoxHelper.BindData4CheckedListBox<SafetyStockConfig, long, View_ProdDetail>(
            //    entity,
            //    e => e.ProductIds,
            //    clbProds,
            //    MainForm.Instance.View_ProdDetailList,
            //    u => u.ProdDetailID,
            //    u => u.CNName
            //    );
            dataGridView1.ReadOnly = true;
            var list = MainForm.Instance.View_ProdDetailList.Where(c => entity.ProductIds.Contains(c.ProdDetailID)).ToList();
            bindingSourceList.DataSource = ListExtension.ToBindingSortCollection<View_ProdDetail>(list);//这句是否能集成到上一层生成
            dataGridView1.DataSource = bindingSourceList;

            CheckedListBoxHelper.BindData4CheckedListBox<SafetyStockConfig, long, tb_Location>(
            entity,
            e => e.LocationIds,
            clbLocation_IDs,
           _cacheManager.GetEntityList<tb_Location>(),
            u => u.Location_ID,
            u => u.Name
            );


            DataBindingHelper.BindData4TextBox<SafetyStockConfig>(entity, t => t.MinStock, txtMinStock, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<SafetyStockConfig>(entity, t => t.MaxStock, txtMaxStock, BindDataType4TextBox.Qty, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.EndEdit();
            if (bindingSourceEdit.Current is SafetyStockConfig stockConfig)
            {
                //stockConfig.ProductIds = bindingSourceList.DataSource.ToList<View_ProdDetail>().Select(x => x.ProductID).ToList();

                // 执行验证
                var result = stockConfig.Validate();
                if (!result.IsValid)
                {
                    MessageBox.Show($"错误:\r\n{result.GetCombinedErrors()}", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void UCBoxRulesEdit_Load(object sender, EventArgs e)
        {
            InitDataGridVIew();
            BindData(safetyStockConfig);
        }

        public GridViewDisplayTextResolver DisplayTextResolver;
        private Type entityType = typeof(View_ProdDetail);
        /// <summary>
        /// 主表表不可见的列
        /// </summary>
        public List<Expression<Func<View_ProdDetail, object>>> MasterInvisibleCols { get; set; } = new List<Expression<Func<View_ProdDetail, object>>>();

        private void InitDataGridVIew()
        {
            DisplayTextResolver = new GridViewDisplayTextResolver(entityType);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.XmlFileName = this.Name + entityType.Name + "BaseMasterQueryWithCondition";
            dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(entityType);
       
            //这里设置了指定列不可见
            foreach (var item in RuinorExpressionHelper.ExpressionListToHashSet(MasterInvisibleCols))
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
            }

            dataGridView1.BizInvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(MasterInvisibleCols);

            dataGridView1.DataSource = bindingSourceList;
            DisplayTextResolver.Initialize(dataGridView1);
        }
        private void btnSeleted_Click(object sender, EventArgs e)
        {
            using (QueryFormGeneric dg = new QueryFormGeneric())
            {
                dg.Text = "安全库存检测产品对象导入";
                dg.StartPosition = FormStartPosition.CenterScreen;
                dg.prodQuery.MultipleChoices = true;
                //dg.prodQuery.QueryField = EditEntity.GetPropertyName<tb_Stocktake>(c => c.Location_ID);
                //dg.prodQuery.QueryValue = EditEntity.Location_ID;
                dg.prodQuery.UseType = ProdQueryUseType.盘点导入;

                if (dg.ShowDialog() == DialogResult.OK)
                {
                    /// Control.Tag = dg.QueryObjects;
                    // Control.Value = dg.QueryValue;
                    //将查询到的结果转换为单据明细 ,货加号来自于库存，后面添加查询对象除产品，等还可以依库存查
                    safetyStockConfig.ProductIds.Clear();
                    foreach (View_ProdDetail item in dg.prodQuery.QueryObjects)
                    {
                        safetyStockConfig.ProductIds.Add(item.ProdDetailID);
                    }
                }
            }
            //将集合用,号隔开后拼起来
            //txt_ProductIds.Text = safetyStockConfig.ProductIds.ToJson();
        }

        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }

        private void txtCheckIntervalByMinutes_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
