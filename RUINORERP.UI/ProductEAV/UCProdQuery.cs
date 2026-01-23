using FastReport.DevComponents.DotNetBar;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using Krypton.Workspace;
using Netron.GraphLib;
using Netron.NetronLight;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Math;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.BI;
using RUINORERP.UI.Common;
using RUINORERP.UI.MRP.MP;
using RUINORERP.UI.ToolForm;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;
using Image = System.Drawing.Image;




namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("产品查询", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCProdQuery : BaseUControl
    {
        /*
         目前这个是用于一个公用的查询模板，对于一些特殊情况下的调用。用枚举类型来区别一下。
        公共查询入口 ，将来可以扩展为多个查询入口，如我的收藏产品，我的产品，我的供应商产品等
        主要返回是产品列表，或者产品ID列表，同时像套装组合时也要返回组合的一个信息，这时套装tab页要显示需要的套装。并返回套装信息和套数。
         */

        public ProdQueryUseType UseType = ProdQueryUseType.None;


        //  this.AcceptButton = this.btnQueryForGoods;
        //this.CancelButton = this.btnCancel;
        private long _LocationID = 0;

        /// <summary>
        /// 默认的仓库,由单据UI带过来
        /// </summary>
        public long LocationID { get => _LocationID; set => _LocationID = value; }


        private object _queryValue = string.Empty;

        private string _queryField = string.Empty;

        private List<View_ProdDetail> _queryObjects = new List<View_ProdDetail>();
        private View_ProdDetail _queryObject = new View_ProdDetail();

        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();




        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }
        public bool CanMultiSelect { get; set; } = true;
        public UCProdQuery()
        {
            InitializeComponent();

            // 初始化网格显示文本解析器
            DisplayTextResolver = new GridViewDisplayTextResolverGeneric<tb_Prod>();
            DisplayTextResolver.AddFixedDictionaryMappingByEnum<tb_Prod>(t => t.PropertyType, typeof(ProductAttributeType));
            ProdBundleDisplayTextResolver = new GridViewDisplayTextResolverGeneric<tb_ProdBundle>();
            newSumDataGridView产品.NeedSaveColumnsXml = true;
            newSumDataGridView产品组合.NeedSaveColumnsXml = true;




            kryptonNavigator1.SelectedPageChanged += KryptonNavigator1_SelectedPageChanged;
            newSumDataGridView产品.CellPainting += KryptonDataGridView产品_CellPainting;
            newSumDataGridView产品.CellMouseMove += KryptonDataGridView产品_CellMouseMove;
            newSumDataGridView产品.CustomRowNo = true;
            newSumDataGridView产品.ShowCellToolTips = true;
            newSumDataGridView产品.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            // 初始化Timer
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            
           
            ProdBundleDisplayTextResolver.Initialize(newSumDataGridView产品组合);
            DisplayTextResolver.AddColumnDisplayType<tb_Prod>(c=>c.Images,ColumnDisplayTypeEnum.Image);
            DisplayTextResolver.Initialize(newSumDataGridView产品);
        }

        private bool isMouseOverHeader = false;
        private int lastMouseOverRowIndex = -1;

        private void timer_Tick(object sender, EventArgs e)
        {
            // 计时器触发，显示工具提示
            if (isMouseOverHeader && lastMouseOverRowIndex != -1)
            {
                // 设置工具提示文本，这里可以根据需要定制
                string toolTipText = "这里是三角形行头的提示信息";
                //toolTip1.Show(toolTipText, kryptonDataGridView产品, kryptonDataGridView产品.GetCellDisplayRectangle(lastMouseOverRowIndex, 0, false).Location);
                timer.Stop();
            }
            else
            {
                toolTip1.Hide(newSumDataGridView产品);
            }
        }

        private Timer timer = new Timer();
        private void KryptonDataGridView产品_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                // 标记鼠标在行头上方
                isMouseOverHeader = true;
                lastMouseOverRowIndex = e.RowIndex;

                // 重置计时器
                timer.Stop();
                timer.Start();

                newSumDataGridView产品.Rows[e.RowIndex].HeaderCell.ToolTipText = $"这是第 {e.RowIndex + 1} 行的提示信息";
                newSumDataGridView产品.Rows[e.RowIndex].HeaderCell.Value = "00";

                // 计算三角形的中心点坐标，整体下移7个像素
                int x = (newSumDataGridView产品.Rows[e.RowIndex].HeaderCell.ContentBounds.Left + newSumDataGridView产品.Rows[e.RowIndex].HeaderCell.ContentBounds.Width / 2);
                int y = newSumDataGridView产品.Rows[e.RowIndex].HeaderCell.ContentBounds.Top + 7; // 顶点在上方，整体下移7个像素

                string toolTipText = string.Empty;


                //toolTip1.SetToolTip(kryptonDataGridView产品.Rows[e.RowIndex]., "当前产品有箱规信息，双击行头的图形可以查看！");
                // 根据行数据设置工具提示文本
                // 例如，获取行头对应的行数据
                DataGridViewRow dr = newSumDataGridView产品.Rows[e.RowIndex];
                tb_Packing tb_Packing = null;
                BoxRuleBasis basis = GDIHelper.Instance.CheckForBoxSpecBasis(dr, out tb_Packing);
                // 绘制图案
                switch (basis)
                {
                    case BoxRuleBasis.Product:
                        toolTipText = $"第 {e.RowIndex + 1} 行的产品有箱规信息，双击行头的图形可以查看！";
                        break;
                    case BoxRuleBasis.Attributes:
                        toolTipText = $"第 {e.RowIndex + 1} 行的产品有箱规信息，双击行头的图形可以查看！";

                        break;
                    case BoxRuleBasis.Product | BoxRuleBasis.Attributes:
                        toolTipText = $"第 {e.RowIndex + 1} 行的产品有箱规信息，双击行头的图形可以查看！";

                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(toolTipText))
                {
                    toolTip1.Show(toolTipText, newSumDataGridView产品, new Point(x, y));
                }

                // 如果需要，也可以根据单元格的数据来定制提示文本 kryptonDataGridView产品.Rows[e.RowIndex].Cells["YourColumnName"].Value.ToString() +
                // e.ToolTipText = row.Cells["YourColumnName"].Value.ToString();
            }
            else
            {
                // 鼠标不在行头，停止计时器，隐藏工具提示
                isMouseOverHeader = false;
                timer.Stop();
                toolTip1.Hide(newSumDataGridView产品);
            }

        }


        private void KryptonDataGridView产品_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 检查是否是行头
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {

                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                // 检查是否需要标记的行
                // 例如，检查行的数据是否有箱规信息
                DataGridViewRow dr = newSumDataGridView产品.Rows[e.RowIndex];
                tb_Packing tb_Packing = null;
                BoxRuleBasis basis = GDIHelper.Instance.CheckForBoxSpecBasis(dr, out tb_Packing);
                // 绘制图案
                switch (basis)
                {
                    case BoxRuleBasis.Product:
                        GDIHelper.Instance.DrawPattern(e, Color.DarkGreen);
                        break;
                    case BoxRuleBasis.Attributes:
                        //DrawPattern(e, Color.DarkMagenta);
                        GDIHelper.Instance.DrawPattern(e);
                        break;
                    case BoxRuleBasis.Product | BoxRuleBasis.Attributes:
                        GDIHelper.Instance.DrawPattern(e, Color.OrangeRed);
                        break;
                    default:
                        break;
                }
                e.Handled = true;

            }
        }




        protected void SelectedData()
        {

            //退出
            Form frm = (this as Control).Parent.Parent as Form;
            frm.DialogResult = DialogResult.OK;
            frm.Close();
            return;


            //if (control != null)
            //{
            //    control.CausesValidation = true;
            //}

            Exit(this);
        }



        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                page.Hide(); //高级查询 如果移除会 工具栏失效一次，找不到原因。目前暂时隐藏处理
                             //如果上一级的窗体关闭则删除？
                             //MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                             //page.Dispose();
            }
            else
            {
                if (thisform is Form)
                {
                    Form frm = (thisform as Form);
                    frm.Close();
                }
                else
                {
                    Form frm = (thisform as Control).Parent.Parent as Form;
                    frm.Close();
                }


            }
        }

        protected virtual void Exit(object thisform)
        {
            CloseTheForm(thisform);
        }


        /// <summary>
        /// 网格显示文本解析器，用于设置特殊的映射关系
        /// </summary>
        public GridViewDisplayTextResolverGeneric<tb_Prod> DisplayTextResolver { get; set; }
        public GridViewDisplayTextResolverGeneric<tb_ProdBundle> ProdBundleDisplayTextResolver { get; set; }

        UITools uiTools = new UITools();
       



        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }

            //图片特殊处理
            if (newSumDataGridView产品.Columns[e.ColumnIndex].Name == "Images")
            {
                if (newSumDataGridView产品.CurrentCell.Value != null)
                {
                    if (newSumDataGridView产品.CurrentCell.Value != null && newSumDataGridView产品.CurrentCell.Value.GetType() == typeof(byte[]))
                    {
                        System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])newSumDataGridView产品.CurrentCell.Value);
                        Image image = Image.FromStream(buf, true);
                        if (image != null)
                        {
                            frmPictureViewer frmShow = new frmPictureViewer();
                            frmShow.PictureBoxViewer.Image = image;
                            frmShow.ShowDialog();
                        }
                    }

                }
            }
        }



        private void btnQueryForGoods_Click(object sender, EventArgs e)
        {
            QueryAsync();
        }


        public override void QueryAsync(bool UseAutoNavQuery = false)
        {
            if (kryptonNavigator1.SelectedPage != null)
            {
                switch (kryptonNavigator1.SelectedPage.Name)
                {
                    case "kryptonPage产品":
                        Query产品();
                        break;
                    case "kryptonPage产品组合":
                        Query产品组合();
                        break;
                    case "kryptonBOM":
                        QueryToBOM();
                        break;
                    default:
                        break;
                }
            }
        }



        private void BindingDataWithCustomColumn<View_ProdDetail>(List<View_ProdDetail> list)
        {
            BindingSource bings = new BindingSource();
            bings.DataSource = list;
            ConcurrentDictionary<string, string> cds = UIHelper.GetFieldNameList<View_ProdDetail>();
            cds.TryAdd("选择", "");

            // PropertyInfo[] fields = typeof(T).GetProperties();
            //   string[] freColumnName = new string[] { "id", "节点id", "设备id", "记录个数" };
            string[] freColumnName = cds.Keys.ToArray();
            /// string[] freColumnName = new string[freColumnNameTemp.Length + 1];
            // freColumnName[0] = "选择";
            // freColumnName
            // freColumnName. = cds.Keys.ToArray();

            newSumDataGridView产品.DataSource = null;

            newSumDataGridView产品.ColumnCount = freColumnName.Length + 1;
            newSumDataGridView产品.RowHeadersVisible = false;
            for (int i = 0; i < freColumnName.Length; i++)
            {
                newSumDataGridView产品.Columns[i].Name = freColumnName[i];
                newSumDataGridView产品.Columns[i].DataPropertyName = freColumnName[i];
                newSumDataGridView产品.Columns[i].HeaderText = freColumnName[i]; //显示名称
            }
            newSumDataGridView产品.Columns[freColumnName.Length - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newSumDataGridView产品.Dock = DockStyle.Fill;
            // panel2.Controls.Add(dataGridView);
            newSumDataGridView产品.Rows?.Clear();
            newSumDataGridView产品.DataSource = bings;
            newSumDataGridView产品.ColumnHeadersVisible = true;
            //kryptonDataGridView1.DataSource = prodCtrol.Query(sb.ToString());
            //  kryptonDataGridView1.ColumnDisplayControl(UIHelper.GetFieldNameList<View_ProdDetail>());

        }

        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            newSumDataGridView产品.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(View_ProdDetail));
            newSumDataGridView产品.XmlFileName = "QueryFormGeneric_" + typeof(View_ProdDetail).Name;
            newSumDataGridView产品.FieldNameList = FieldNameList1;

            List<Expression<Func<View_ProdDetail, object>>> ProdInvisibleCols = new List<Expression<Func<View_ProdDetail, object>>>();
            //超级用户可以查看成本
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                ProdInvisibleCols.Add(c => c.Inv_Cost);
            }
            var InvisibleCols = RuinorExpressionHelper.ExpressionListToStringList(ProdInvisibleCols);
            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridView产品.FieldNameList.TryRemove(item, out kv);
            }
            newSumDataGridView产品.DataSource = null;
            bindingSourceProdDetail.DataSource = new List<View_ProdDetail>();
            newSumDataGridView产品.DataSource = bindingSourceProdDetail;


            newSumDataGridView产品组合.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(tb_ProdBundle));
            newSumDataGridView产品组合.XmlFileName = "QueryFormGenericGroup_" + typeof(tb_ProdBundle).Name;
            newSumDataGridView产品组合.FieldNameList = FieldNameList1;
            newSumDataGridView产品组合.DataSource = null;
            bindingSourceGroup.DataSource = new List<tb_ProdBundle>();
            newSumDataGridView产品组合.DataSource = bindingSourceGroup;

        }

        private Expression<Func<View_ProdDetail, bool>> GetQueryExp()
        {
            tb_ProdCategoriesController<tb_ProdCategories> categoriesController = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
            List<long> cateids = new List<long>();
            if (chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null)
            {
                cateids = categoriesController.GetChildids(long.Parse(txtcategory_ID.TreeView.SelectedNode.Name));
            }
            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
           .AndIF(txtName.Text.Trim().Length > 0, w => w.CNName.Contains(txtName.Text.Trim()))
           .AndIF(txtModel.Text.Trim().Length > 0, w => w.Model.Contains(txtModel.Text.Trim()))
           .AndIF(txtType_ID.SelectedValue != null && txtType_ID.SelectedValue.ToString() != "-1", w => w.Type_ID == long.Parse(txtType_ID.SelectedValue.ToString()))
           .AndIF(cmbdepartment.SelectedValue != null && cmbdepartment.SelectedValue.ToString() != "-1", w => w.DepartmentID == long.Parse(cmbdepartment.SelectedValue.ToString()))
           .AndIF(cmbLocation.SelectedValue != null && cmbLocation.SelectedValue.ToString() != "-1", w => w.Location_ID == long.Parse(cmbLocation.SelectedValue.ToString()))
           .AndIF(txtShortCode.Text.Trim().Length > 0, w => w.ShortCode.Contains(txtShortCode.Text.Trim()))
           .AndIF(txtProp.Text.Trim().Length > 0, w => w.prop.Contains(txtProp.Text.Trim()))
           .AndIF(txtBrand.Text.Trim().Length > 0, w => w.Brand.Contains(txtBrand.Text.Trim()))
           .AndIF(txtBarCode.Text.Trim().Length > 0, w => w.BarCode.Contains(txtBarCode.Text.Trim()))
           .AndIF(txtSKU码.Text.Trim().Length > 0, w => w.SKU.Contains(txtSKU码.Text.Trim()))
           .AndIF(txtNo.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtNo.Text.Trim()))
           .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
           .AndIF(chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null, c => cateids.ToArray().Contains(c.Category_ID.Value))
           .AndIF(!chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null, w => w.Category_ID.Value == long.Parse(txtcategory_ID.TreeView.SelectedNode.Name.ToString()))
           .AndIF(cmbStockJudgement.SelectedItem != null && cmbStockJudgement.SelectedItem.ToString() == "大于零", w => w.Quantity.HasValue && w.Quantity.Value > 0)
           .AndIF(cmbStockJudgement.SelectedItem != null && cmbStockJudgement.SelectedItem.ToString() == " 等于零", w => w.Quantity.HasValue && w.Quantity.Value.Equals(0))
           .AndIF(cmbStockJudgement.SelectedItem != null && cmbStockJudgement.SelectedItem.ToString() == "小于零", w => w.Quantity.HasValue && w.Quantity.Value < 0)
           .AndIF(UseType == ProdQueryUseType.盘点导入 && dtp1.Checked && dtp1 != null, w => w.LastInventoryDate.Value >= dtp1.Value)
           .AndIF(UseType == ProdQueryUseType.盘点导入 && dtp1.Checked && dtp2 != null, w => w.LastInventoryDate.Value <= dtp2.Value)
           .And(w => w.产品启用.Value == chkProd_enabled.Checked)
           .And(w => w.产品可用.Value == chkProd_available.Checked)
           .And(w => w.SKU启用.Value == chksku_enabled.Checked)
           .And(w => w.SKU可用.Value == chksku_available.Checked)
           .ToExpression();
            return exp;
        }



        ConcurrentDictionary<string, string> cds = UIHelper.GetFieldNameList<View_ProdDetail>();
        private void Query产品()
        {
            View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            int maxrow = int.Parse(txtMaxRows.Value.ToString());
            var list = dc.BaseQueryByWhereTop(GetQueryExp(), maxrow);
            bindingSourceProdDetail.DataSource = list.ToBindingSortCollection();
        }


        DynamicTypeColumnSorter typeSorter;

        private void QueryToBOM()
        {
            tb_ProdCategoriesController<tb_ProdCategories> categoriesController = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
            List<long> cateids = new List<long>();
            if (chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null)
            {
                cateids = categoriesController.GetChildids(long.Parse(txtcategory_ID.TreeView.SelectedNode.Name));
            }
            int maxrow = int.Parse(txtMaxRows.Value.ToString());
            var querySqlQueryable = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>().Take(maxrow)
                //.OrderBy(c => c.Type_ID)
                .Where(GetQueryExp());
            // ✅ 修复：所有查询条件构建完成后，再调用 IncludesAllFirstLayer()
            querySqlQueryable = querySqlQueryable.IncludesAllFirstLayer();//自动导航
            var list = querySqlQueryable.ToList();
            treeListView1.Items.Clear();
            AddItems(list);

            // 从数据源提取类型排序优先级（基于Type_ID的顺序）
            var typePriority = GetTypePriorityFromData(list);

            // 初始化并配置排序器
            if (typeSorter == null)
                typeSorter = new DynamicTypeColumnSorter();

            typeSorter.TypePriority = typePriority;
            typeSorter.SetSort(5, SortOrder.Ascending);  // "类型"列索引为5
            this.treeListView1.ListViewItemSorter = typeSorter;
            this.treeListView1.Sort();


            // 原文链接：https://blog.csdn.net/m0_53104033/article/details/129006538
        }
        private void AddItems(List<View_ProdDetail> listDetails)
        {
            List<long> allIds = listDetails.Select(c => c.ProdDetailID).ToList();
            var listboms = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>()
                    .RightJoin<tb_ProdDetail>((a, b) => a.ProdDetailID == b.ProdDetailID)
                    // .Includes(a => a.tb_producttype)
                    //.Includes(a => a.tb_BOM_SDetails)
                    .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                    .Includes(a => a.tb_BOM_SDetails, b => b.view_ProdInfo)
                    .Where(a => allIds.ToArray().Contains(a.ProdDetailID))
                    .ToList();
            foreach (View_ProdDetail row in listDetails)
            {
                TreeListViewItem itemRow = new TreeListViewItem(row.CNName, 0);
                itemRow.Tag = row;
                itemRow.SubItems.Add(row.SKU); //subitems只是从属于itemRow的子项。目前是四列
                itemRow.SubItems.Add(row.prop); //subitems只是从属于itemRow的子项。目前是四列
                itemRow.SubItems.Add(row.Model);
                ////一定会有值
                //tb_BOM_S bOM_S = listboms.Where(c => c.ProdDetailID == row.ProdDetailID).FirstOrDefault();
                //itemRow.SubItems[0].Tag = bOM_S;
                itemRow.SubItems.Add(row.Specifications);
                string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", row.Type_ID);
                itemRow.SubItems.Add(prodType);
                itemRow.SubItems.Add(row.Quantity.ToString());
                itemRow.SubItems.Add(row.Sale_Qty.ToString());
                itemRow.SubItems.Add(row.MakingQty.ToString());
                itemRow.SubItems.Add(row.On_the_way_Qty.ToString());
                itemRow.SubItems.Add(row.NotOutQty.ToString());
                itemRow.SubItems.Add(row.Alert_Quantity.ToString());
                treeListView1.Items.Add(itemRow);
                tb_BOM_S bOM_S = listboms.Where(c => c.ProdDetailID == row.ProdDetailID).FirstOrDefault();
                if (bOM_S != null)
                {
                    Loadbom(bOM_S, row.ProdDetailID, row.Location_ID, itemRow);
                }

            }
        }
        // 从数据源获取类型排序优先级
        private Dictionary<string, int> GetTypePriorityFromData(List<View_ProdDetail> list)
        {
            // 提取所有唯一的Type_ID并按Type_ID排序
            var sortedTypeIds = list.Select(c => c.Type_ID)
                                    .Distinct()
                                    .OrderBy(id => id)  // 按Type_ID升序确定优先级
                                    .ToList();

            // 生成类型名称到优先级的映射
            var priorityDict = new Dictionary<string, int>();
            for (int i = 0; i < sortedTypeIds.Count; i++)
            {
                string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", sortedTypeIds[i]);
                if (!priorityDict.ContainsKey(prodType))
                {
                    priorityDict[prodType] = i + 1;  // 优先级从1开始
                }
            }

            return priorityDict;
        }

        private void Loadbom(tb_BOM_S bOM_S, long ProdDetailID, long? Location_ID, TreeListViewItem listViewItem)
        {
            if (bOM_S != null && bOM_S.tb_BOM_SDetails != null)
            {
                listViewItem.ImageIndex = 1;//如果有配方，则图标不一样
                foreach (var BOM_SDetail in bOM_S.tb_BOM_SDetails)
                {
                    TreeListViewItem itemSub = new TreeListViewItem(BOM_SDetail.view_ProdInfo.CNName, 0);
                    itemSub.Tag = BOM_SDetail.view_ProdInfo;

                    itemSub.SubItems.Add(BOM_SDetail.SKU);//subitems只是从属于itemRow的子项。目前是四列
                    itemSub.SubItems.Add(BOM_SDetail.view_ProdInfo.prop);//subitems只是从属于itemRow的子项。目前是四列
                    itemSub.SubItems.Add(BOM_SDetail.view_ProdInfo.Model);
                    itemSub.SubItems.Add(BOM_SDetail.view_ProdInfo.Specifications);
                    string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", BOM_SDetail.view_ProdInfo.Type_ID);
                    itemSub.SubItems.Add(prodType);
                    if (BOM_SDetail.view_ProdInfo != null && Location_ID.HasValue)
                    {
                        var view_ProdDetail = MainForm.Instance.View_ProdDetailList.FirstOrDefault(c => c.ProdDetailID == BOM_SDetail.ProdDetailID && c.Location_ID == Location_ID);
                        if (view_ProdDetail != null)
                        {
                            itemSub.SubItems.Add(view_ProdDetail.Quantity.ToString());
                            itemSub.SubItems.Add(view_ProdDetail.Sale_Qty.ToString());
                            itemSub.SubItems.Add(view_ProdDetail.MakingQty.ToString());
                            itemSub.SubItems.Add(view_ProdDetail.On_the_way_Qty.ToString());
                            itemSub.SubItems.Add(view_ProdDetail.NotOutQty.ToString());
                            itemSub.SubItems.Add(view_ProdDetail.Alert_Quantity.ToString());
                        }
                    }
                    else
                    {
                        itemSub.SubItems.Add("0".ToString());
                        itemSub.SubItems.Add("0".ToString());
                        itemSub.SubItems.Add("0".ToString());
                        itemSub.SubItems.Add("0".ToString());
                        itemSub.SubItems.Add("0".ToString());
                        itemSub.SubItems.Add("0".ToString());
                    }

                    listViewItem.Items.Add(itemSub);
                    if (BOM_SDetail.tb_bom_s != null)
                    {
                        itemSub.SubItems[0].Tag = BOM_SDetail.tb_bom_s;
                    }
                }
            }
        }




        private void Query产品组合()
        {
            tb_ProdBundleController<tb_ProdBundle> dc = Startup.GetFromFac<tb_ProdBundleController<tb_ProdBundle>>();
            int maxrow = int.Parse(txtMaxRows.Value.ToString());
            Expression<Func<tb_ProdBundle, bool>> exp = Expressionable.Create<tb_ProdBundle>() //创建表达式
            .And(w => w.Is_enabled == chkProdBundle_enabled.Checked)
           .And(w => w.Is_available == chkProdBundle_available.Checked)
           .AndIF(txt组合名称.Text.Trim().Length > 0, w => w.BundleName.Contains(txt组合名称.Text.Trim()))
            .ToExpression();
            var list = dc.BaseQueryByWhereTop(exp, maxrow);
            bindingSourceGroup.DataSource = list.ToBindingSortCollection();
        }
        #region 实际选择列右键全选不选 前提是数据源基础中有一个属性为Selected  没有使用的代码

        public event EventHandler 全选;
        public event EventHandler 全不选;


        /// <summary>
        /// 创建一个右键菜单
        /// </summary>
        /// <returns></returns>
        public ContextMenuStrip SetContextMenuStripForSelect()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            UControls.ContextMenuController cmc1 = new UControls.ContextMenuController("【全选】", true, false, "全选");
            UControls.ContextMenuController cmc2 = new UControls.ContextMenuController("【全不选】", true, false, "全不选");
            this.全选 += QueryForm_全选;
            this.全不选 += QueryForm_全不选;
            contextMenuStrip.Items.Add(cmc1.MenuText, null, 全选);
            contextMenuStrip.Items.Add(cmc2.MenuText, null, 全不选);
            //因为暂时事件无法通过属性中的数据传输，先用名称再从这里搜索来匹配
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(全选);
            ContextClickList.Add(全不选);
            return contextMenuStrip;
        }


        private void QueryForm_全不选(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in newSumDataGridView产品.Rows)
            {
                dr.Cells["Selected"].Value = false;
                dr.Cells["Selected"].Selected = false;
                //RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dr.DataBoundItem, "Selected", false);
            }
        }


        private void QueryForm_全选(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in newSumDataGridView产品.Rows)
            {
                dr.Cells["Selected"].Value = true;
                dr.Cells["Selected"].Selected = true;
                //RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dr.DataBoundItem, "Selected", true);
            }
        }



        #endregion






        /// <summary>
        /// 是否显示选择列，能多行选中。
        /// </summary>
        public bool MultipleChoices { get; set; } = false;


        //改版
        //返回值将单个值对象等，改为数组
        public object QueryValue { get => _queryValue; set => _queryValue = value; }

        /// <summary>
        ///  明细表格时传进来的查询字段条件
        /// </summary>
        public string QueryField { get => _queryField; set => _queryField = value; }

        /// <summary>
        /// 返回查询的对象
        /// </summary>
        public List<View_ProdDetail> QueryObjects { get => _queryObjects; set => _queryObjects = value; }

        /// <summary>
        /// 要返回的单个对象
        /// </summary>
        public View_ProdDetail QueryObject { get => _queryObject; set => _queryObject = value; }

        private async void QueryForm_Load(object sender, EventArgs e)
        {
            if (UseType == ProdQueryUseType.盘点导入)
            {
                lblLastInventoryDate.Visible = true;
                dtp1.Visible = true;
                dtp2.Visible = true;
                kryptonLabel10.Visible = true;
            }
            else
            {
                lblLastInventoryDate.Visible = false;
                dtp1.Visible = false;
                dtp2.Visible = false;
                kryptonLabel10.Visible = false;

            }

            chksku_available.Checked = true;
            chksku_enabled.Checked = true;
            chkProd_available.Checked = true;
            chkProd_enabled.Checked = true;


            chkProdBundle_available.Checked = true;
            chkProdBundle_enabled.Checked = true;

            kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            chkMultiSelect.Checked = MultipleChoices;
            if (!CanMultiSelect)
            {
                chkMultiSelect.Visible = false;
            }
            else
            {
                chkMultiSelect.Visible = true;
            }
            newSumDataGridView产品.MultiSelect = MultipleChoices;
            newSumDataGridView产品.UseSelectedColumn = MultipleChoices;

            newSumDataGridView产品组合.MultiSelect = MultipleChoices;
            newSumDataGridView产品组合.UseSelectedColumn = MultipleChoices;

            List<tb_ProdCategories> list = new List<tb_ProdCategories>(0);
            tb_ProdCategoriesController<tb_ProdCategories> mca = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
            list = await mca.QueryAsync();
            UIProdCateHelper.BindToTreeViewNoRootNode(list, txtcategory_ID.TreeView);
            DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v => v.TypeName, txtType_ID);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbdepartment);
            DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v => v.Name, cmbLocation);
            InitListData();

            ContextMenuStrip newContextMenuStrip = newSumDataGridView产品.GetContextMenu(contextMenuStrip1);
            newSumDataGridView产品.ContextMenuStrip = newContextMenuStrip;
            newSumDataGridView产品组合.ContextMenuStrip = newContextMenuStrip;

            //默认指定一个仓库主仓库
            if (LocationID > 0)
            {
                cmbLocation.SelectedValue = LocationID;
            }
            else
            {
                if (cmbLocation.Items.Count > 1)
                {
                    cmbLocation.SelectedIndex = 1;
                }
            }


            if (QueryValue != null && QueryValue.ToString().Length > 0)
            {
                QueryObject.SetPropertyValue(QueryField, QueryValue);
                BindData();
            }

            chkIncludeNotInStockProd.CheckedChanged += (sender, e) =>
            {
                if (chkIncludeNotInStockProd.Checked)
                {
                    cmbLocation.SelectedIndex = -1;
                }
                else
                {
                    //否则默认选择第一个仓库，0是请选择
                    cmbLocation.SelectedIndex = 1;
                }
            };

            kryptonNavigator1.SelectedPage = kryptonPage产品;
            SetPage();



            #region 加载选择的项目


            #endregion


        }

        private void KryptonNavigator1_SelectedPageChanged(object sender, EventArgs e)
        {
            SetPage();
        }


        private void SetPage()
        {
            if (kryptonNavigator1.SelectedPage == kryptonPage产品 || kryptonNavigator1.SelectedPage == kryptonBOM)
            {
                kryptonPanelGroup.Visible = false;
                kryptonPanelProd.Visible = true;
                kryptonPanelProd.Dock = DockStyle.Fill;
                //kryptonDataGridView产品.ContextMenuStrip = contextMenuStrip1;
                //newSumDataGridView产品.Use是否使用内置右键功能 = true;
                //newSumDataGridView产品.SetContextMenu(contextMenuStrip1);
                //newSumDataGridView产品组合.ContextMenuStrip = null;

            }
            if (kryptonNavigator1.SelectedPage == kryptonPage产品组合)
            {
                kryptonPanelGroup.Visible = true;
                kryptonPanelProd.Visible = false;
                kryptonPanelGroup.Dock = DockStyle.Fill;
                //newSumDataGridView产品.ContextMenuStrip = null;
                //newSumDataGridView产品组合.ContextMenuStrip = contextMenuStrip1;
            }

            if (kryptonNavigator1.SelectedPage != null)
            {
                switch (kryptonNavigator1.SelectedPage.Name)
                {
                    case "kryptonPage产品":
                        chk包含父级节点.Visible = false;
                        break;
                    case "kryptonPage产品组合":
                        chk包含父级节点.Visible = false;
                        break;
                    case "kryptonBOM":
                        chk包含父级节点.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 这里字段是用来可以主动设置查询条件的
        /// 其他 的实际是可以不设置
        /// </summary>
        public void BindData()
        {
            View_ProdDetail entity = QueryObject as View_ProdDetail;
            if (entity == null)
            {

                return;
            }

            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.SKU, txtSKU码, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, cmbLocation);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.CNName.ToString(), txtName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.BarCode.ToString(), txtBarCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.SKU可用, chksku_available, false);
            //DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.SKU启用, chksku_enabled, false);
            //DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.产品可用, chkProd_available, false);
            //DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.产品启用, chkProd_enabled, false);

            //errorProviderForAllInput.DataSource = entity;
            //errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {



            };



        }



        private void kryptonDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //如果双击的是行头，则是否要弹出箱规信息呢？
            // 检查是否是行头
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                // 例如，检查行的数据是否有箱规信息
                DataGridViewRow dr = newSumDataGridView产品.Rows[e.RowIndex];

                tb_Packing tb_Packing = null;
                BoxRuleBasis basis = GDIHelper.Instance.CheckForBoxSpecBasis(dr, out tb_Packing);

                MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                // 例如，检查行的数据是否有包装信息
                tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_Packing).Name && m.ClassPath.Contains("RUINORERP.UI.ProductEAV.UCPacking")).FirstOrDefault();
                if (RelatedBillMenuInfo == null)
                {
                    MessageBox.Show("请确认您拥有包装信息编辑权限");
                }
                if (RelatedBillMenuInfo != null && basis != BoxRuleBasis.None)
                {
                    //如果是给值。不在这处理。在生成时处理的。 这里只是调用到UI
                    menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, tb_Packing);
                    //隐藏提示信息
                    toolTip1.Hide(newSumDataGridView产品);
                }
            }

            if (this.Parent.Name != "kryptonPanelQuery")
            {
                return;
            }
            if (newSumDataGridView产品.CurrentRow != null && !chkMultiSelect.Checked)
            {
                GetQueryResults();
            }
        }




        /// <summary>
        /// 多选
        /// </summary>
        private void GetQueryResults()
        {
            QueryObjects.Clear();
            newSumDataGridView产品.EndEdit();
            if (newSumDataGridView产品.SelectedRows != null)
            {
                if (MultipleChoices)
                {
                    foreach (DataGridViewRow dr in newSumDataGridView产品.Rows)
                    {
                        if (!(dr.DataBoundItem is View_ProdDetail))
                        {
                            MessageBox.Show("TODO:请调试这里");
                        }
                        if (MultipleChoices && (bool)dr.Cells["Selected"].Value)
                        {
                            QueryObjects.Add((View_ProdDetail)dr.DataBoundItem);
                        }
                    }
                }
                else
                {
                    #region
                    foreach (DataGridViewRow dr in newSumDataGridView产品.SelectedRows)
                    {
                        if (!(dr.DataBoundItem is View_ProdDetail))
                        {
                            MessageBox.Show("TODO:请调试这里");
                        }
                        QueryObjects.Add((View_ProdDetail)dr.DataBoundItem);
                    }
                    #endregion
                }


                if (QueryObjects.Count > 0)
                {
                    if (!string.IsNullOrEmpty(QueryField))
                    {
                        QueryValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(QueryObjects[0], QueryField).ToString();
                    }

                    //更新缓存
                    for (int i = 0; i < QueryObjects.Count; i++)
                    {
                        var queryObjectItem = QueryObjects[i];
                        var v_ProductSharePart = MainForm.Instance.View_ProdDetailList.Find(x => x.ProdDetailID == queryObjectItem.ProdDetailID && x.Location_ID == queryObjectItem.Location_ID);
                        if (v_ProductSharePart != null)
                        {
                            v_ProductSharePart = queryObjectItem;
                        }
                        else
                        {
                            MainForm.Instance.View_ProdDetailList.Add(queryObjectItem);
                        }
                    }
                }

                //退出
                Form frm = (this as Control).Parent.Parent as Form;
                if (frm == null)
                {
                    return;
                }
                frm.DialogResult = DialogResult.OK;
                frm.Close();
                return;
            }
            else
            {
                //退出
                Form frm = (this as Control).Parent.Parent as Form;
                if (frm == null)
                {
                    return;
                }
                frm.DialogResult = DialogResult.OK;
                frm.Close();
                return;
            }

        }

        /// <summary>
        /// 多选
        /// </summary>
        private void GetQueryGroupResults()
        {
            QueryObjects.Clear();
            newSumDataGridView产品组合.EndEdit();
            if (newSumDataGridView产品组合.SelectedRows != null)
            {
                if (MultipleChoices)
                {
                    foreach (DataGridViewRow dr in newSumDataGridView产品组合.Rows)
                    {
                        if (!(dr.DataBoundItem is tb_ProdBundle))
                        {
                            MessageBox.Show("TODO:请调试这里");
                        }
                        if (MultipleChoices && (bool)dr.Cells["Selected"].Value)
                        {
                            tb_ProdBundle bundle = (tb_ProdBundle)dr.DataBoundItem as tb_ProdBundle;
                            View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();

                            List<View_ProdDetail> details = new List<View_ProdDetail>();
                            // Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                            // .And(c => c.ProdDetailID == bundle.ProdDetailID)
                            //.ToExpression();
                            // int maxrow = int.Parse(txtMaxRows.Value.ToString());
                            // var list = dc.BaseQueryByWhereTop(GetQueryExp(), maxrow);



                            var _detail_ids = bundle.tb_ProdBundleDetails.Select(x => new { x.ProdDetailID }).ToList();
                            List<long> longids = new List<long>();
                            foreach (var item in _detail_ids)
                            {
                                if (!longids.Contains(item.ProdDetailID))
                                {
                                    longids.Add(item.ProdDetailID);
                                }
                            }

                            details = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>()
                            .Where(m => longids.ToArray().Contains(m.ProdDetailID)).ToList();

                            foreach (var item in details)
                            {
                                QueryObjects.Add((View_ProdDetail)item);
                            }

                        }
                    }
                }
                else
                {
                    #region
                    foreach (DataGridViewRow dr in newSumDataGridView产品组合.SelectedRows)
                    {
                        if (!(dr.DataBoundItem is tb_ProdBundle))
                        {
                            MessageBox.Show("TODO:请调试这里");
                        }
                        tb_ProdBundle bundle = (tb_ProdBundle)dr.DataBoundItem as tb_ProdBundle;
                        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();

                        List<View_ProdDetail> details = new List<View_ProdDetail>();
                        // Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
                        // .And(c => c.ProdDetailID == bundle.ProdDetailID)
                        //.ToExpression();
                        // int maxrow = int.Parse(txtMaxRows.Value.ToString());
                        // var list = dc.BaseQueryByWhereTop(GetQueryExp(), maxrow);
                        //这里想将数量和销售单价带过去。目前传入的是产品视图。暂时将数量和单价放到产品视图中的库存和标准价中


                        var _detail_ids = bundle.tb_ProdBundleDetails.Select(x => new { x.ProdDetailID }).ToList();
                        List<long> longids = new List<long>();
                        foreach (var item in _detail_ids)
                        {
                            if (!longids.Contains(item.ProdDetailID))
                            {
                                longids.Add(item.ProdDetailID);
                            }
                        }

                        details = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>()
                        .Where(m => longids.ToArray().Contains(m.ProdDetailID)).ToList();

                        foreach (var item in details.ToArray())
                        {
                            //找到数量和标准价
                            var prodbundleDetail = bundle.tb_ProdBundleDetails.FirstOrDefault(c => c.ProdDetailID == item.ProdDetailID);
                            item.Quantity = prodbundleDetail.Quantity;
                            item.Standard_Price = prodbundleDetail.SaleUnitPrice;

                            QueryObjects.Add((View_ProdDetail)item);
                        }

                    }
                    #endregion
                }


                if (QueryObjects.Count > 0)
                {
                    if (!string.IsNullOrEmpty(QueryField))
                    {
                        QueryValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(QueryObjects[0], QueryField).ToString();
                    }
                }

                //退出
                Form frm = (this as Control).Parent.Parent as Form;
                if (frm == null)
                {
                    return;
                }
                frm.DialogResult = DialogResult.OK;
                frm.Close();
                return;
            }
            else
            {
                //退出
                Form frm = (this as Control).Parent.Parent as Form;
                if (frm == null)
                {
                    return;
                }
                frm.DialogResult = DialogResult.OK;
                frm.Close();
                return;
            }

        }

        /// <summary>
        /// 多选
        /// </summary>
        private void GetQueryBOMResults()
        {
            QueryObjects.Clear();
            if (treeListView1.Items.Count > 0)
            {
                #region
                foreach (TreeListViewItem dr in treeListView1.Items)
                {
                    if (!(dr.Tag is View_ProdDetail))
                    {
                        MessageBox.Show("TODO:请调试这里");
                    }
                    if (dr.Checked)
                    {
                        if (dr.CheckStatus == CheckState.Checked)
                        {
                            QueryObjects.Add((View_ProdDetail)dr.Tag);
                        }
                        GetBOMResults(dr);
                    }

                }
                #endregion
                if (QueryObjects.Count > 0)
                {
                    if (!string.IsNullOrEmpty(QueryField))
                    {
                        QueryValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(QueryObjects[0], QueryField).ToString();
                    }
                }

                //退出
                Form frm = (this as Control).Parent.Parent as Form;
                if (frm == null)
                {
                    return;
                }
                frm.DialogResult = DialogResult.OK;
                frm.Close();
                return;
            }
            else
            {
                //退出
                Form frm = (this as Control).Parent.Parent as Form;
                frm.DialogResult = DialogResult.OK;
                frm.Close();
                return;
            }

        }


        /// <summary>
        /// 配方显示下拉。但是又与库存挂沟了
        /// 所以默认是优先显示库存最多的。如果一个产品View_ProdDetail在有多行的话。
        /// </summary>
        /// <param name="listViewItem"></param>
        private void GetBOMResults(TreeListViewItem listViewItem)
        {
            if (listViewItem.Items.Count > 0)
            {
                foreach (TreeListViewItem dr in listViewItem.Items)
                {
                    if (dr.Checked)
                    {
                        View_ProdDetail prodDetail = null;
                        if (dr.Tag is View_ProdInfo prodInfo)
                        {
                            prodDetail = MainForm.Instance.View_ProdDetailList.FirstOrDefault(c => c.ProdDetailID == prodInfo.ProdDetailID);
                        }
                        else if (dr.Tag is View_ProdDetail _ProdDetail)
                        {
                            prodDetail = _ProdDetail;
                        }
                        if (dr.CheckStatus == CheckState.Checked)
                        {
                            QueryObjects.Add(prodDetail);
                        }
                    }
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            QueryGetRs();
        }


        private void QueryGetRs()
        {
            if (kryptonNavigator1.SelectedPage != null)
            {
                switch (kryptonNavigator1.SelectedPage.Name)
                {
                    case "kryptonPage产品":
                        GetQueryResults();
                        break;
                    case "kryptonPage产品组合":
                        GetQueryGroupResults();
                        break;
                    case "kryptonBOM":
                        GetQueryBOMResults();
                        break;
                    default:
                        break;
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            //CloseTheForm(thisform);
            //退出
            //Form frm = (this as Control).Parent.Parent as Form;
            //if (frm == null)
            //{
            //    frm = (this as Control).Parent.Parent.Parent as Form;
            //}
            //frm.DialogResult = DialogResult.OK;
            //frm.Close();
            //return;
        }

        private void chkMultiSelect_CheckedChanged(object sender, EventArgs e)
        {
            MultipleChoices = chkMultiSelect.Checked;
            newSumDataGridView产品.UseSelectedColumn = MultipleChoices;
        }

        private void treeListView1_BeforeExpand(object sender, TreeListViewCancelEventArgs e)
        {
            if (e.Item.Items.Count > 0)
            {
                foreach (TreeListViewItem item in e.Item.Items)
                {
                    if (item.SubItems[0].Tag != null)
                    {
                        if (item.SubItems[0].Tag is tb_BOM_S bom)
                        {
                            e.Item.ImageIndex = 1;
                        }
                    }

                    /* TODO这里只是不再展开  不需要三级选择
                    ///item.ImageIndex == 1 认为已经绑定过。1是有配方
                    if (item.Tag is View_ProdDetail prodDetail && item.ImageIndex != 1)
                    {
                        ////裸机下面有配方时
                        var listboms = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>()
                            .IncludesAllFirstLayer()
                            .Includes(a => a.tb_BOM_SDetails, c => c.view_ProdDetail)
                            .Where(c => c.ProdDetailID == prodDetail.ProdDetailID)
                            .ToListAsync();
                        foreach (tb_BOM_S bom_s in listboms)
                        {
                            Loadbom(bom_s, bom_s.ProdDetailID, item);
                        }
                    }
                    */



                }
            }
        }

        private void treeListView1_BeforeCollapse(object sender, TreeListViewCancelEventArgs e)
        {

        }

        private void treeListView1_DoubleClick(object sender, EventArgs e)
        {
            GetQueryBOMResults();
            if (treeListView1.SelectedItems != null)
            {
                foreach (TreeListViewItem item in treeListView1.SelectedItems)
                {
                    if (item.Tag is View_ProdDetail prodDetail)
                    {
                        QueryObjects.Add((View_ProdDetail)item.Tag);
                    }
                    else if (item.Tag is View_ProdInfo prodInfo)
                    {
                        var _prodDetail = MainForm.Instance.View_ProdDetailList.FirstOrDefault(c => c.ProdDetailID == prodInfo.ProdDetailID);
                        {
                            QueryObjects.Add(_prodDetail);
                        }
                    }

                    //if (!QueryObjects.Contains((View_ProdDetail)item.Tag))
                    //{
                    //    QueryObjects.Add((View_ProdDetail)item.Tag);
                    //}
                }
            }
            if (QueryObjects.Count > 0)
            {
                if (!string.IsNullOrEmpty(QueryField))
                {
                    QueryValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(QueryObjects[0], QueryField).ToString();
                }
            }

            //退出
            Form frm = (this as Control).Parent.Parent as Form;
            if (frm == null)
            {
                return;
            }
            frm.DialogResult = DialogResult.OK;
            frm.Close();
            return;
        }



        private void 箱规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPackingAndBoxRulesInfo(BoxRuleBasis.Product);
        }

        private void 按SKU添加箱规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPackingAndBoxRulesInfo(BoxRuleBasis.Attributes);
        }


        private void AddPackingAndBoxRulesInfo(BoxRuleBasis boxReuleBasis)
        {
            MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            //暂时的思路 是用 纵向库存跟踪的存储过程，查出来后再将进出明细和最后结余分别总计对比。不同的就有问题。
            if (newSumDataGridView产品.CurrentRow != null)
            {
                tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_Packing).Name && m.ClassPath.Contains("RUINORERP.UI.ProductEAV.UCPacking")).FirstOrDefault();
                if (RelatedBillMenuInfo == null)
                {
                    MessageBox.Show("请确认您拥有包装信息编辑权限");
                }
                tb_Packing packing = new tb_Packing();
                packing.Is_enabled = true;

                if (newSumDataGridView产品.CurrentRow.DataBoundItem is View_ProdDetail)
                {
                    var prodDetail = newSumDataGridView产品.CurrentRow.DataBoundItem as View_ProdDetail;
                    if (boxReuleBasis == BoxRuleBasis.Product)
                    {
                        //如果是已经有包装信息，则提示后进入编辑模式，如果是确定要添加多个包装信息。请在包装信息管理中添加
                        if (prodDetail.tb_prod.tb_Packings.Count > 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("当前产品有包装信息\r\n如果要编辑请选择【是】，如果要添加请选择【否】", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                            if (dialogResult == DialogResult.Yes)
                            {
                                packing = prodDetail.tb_prod.tb_Packings.FirstOrDefault();
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                packing = new tb_Packing();
                            }
                            else
                            {
                                return;
                            }
                        }
                        packing.ProdBaseID = prodDetail.ProdBaseID;
                        packing.tb_prod = prodDetail.tb_prod;
                        packing.PackagingName = $"{prodDetail.CNName}的包装情况";
                    }
                    if (boxReuleBasis == BoxRuleBasis.Attributes)
                    {
                        if (prodDetail.tb_Packing_forSku.Count > 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("当前产品SKU有包装信息\r\n如果要编辑请选择【是】，如果要添加请选择【否】", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                            if (dialogResult == DialogResult.Yes)
                            {
                                packing = prodDetail.tb_Packing_forSku.FirstOrDefault();
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                packing = new tb_Packing();
                            }
                            else
                            {
                                return;
                            }
                        }

                        packing.ProdDetailID = prodDetail.ProdDetailID;
                        packing.PackagingName = $"{prodDetail.CNName}中{prodDetail.SKU}的包装情况";
                        packing.SKU = prodDetail.SKU;
                        packing.property = prodDetail.prop;
                        packing.tb_prod = prodDetail.tb_prod;
                    }
                }

                if (boxReuleBasis == BoxRuleBasis.Combination)
                {
                    if (newSumDataGridView产品.CurrentRow.DataBoundItem is tb_ProdBundle)
                    {
                        var prodBundle = newSumDataGridView产品.CurrentRow.DataBoundItem as tb_ProdBundle;
                        if (prodBundle.tb_Packings.Count > 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("当前产品组合有包装信息\r\n 如果要编辑请选择【是】，如果要添加请选择【否】", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                            if (dialogResult == DialogResult.Yes)
                            {
                                packing = prodBundle.tb_Packings.FirstOrDefault();
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                packing = new tb_Packing();
                            }
                            else
                            {
                                return;
                            }
                        }

                        packing.PackagingName = $"{prodBundle.BundleName}组合的包装情况";
                        packing.BundleID = prodBundle.BundleID;

                    }
                }
                if (RelatedBillMenuInfo != null && packing != null)
                {
                    //如果是给值。不在这处理。在生成时处理的。 这里只是调用到UI
                    menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, packing);
                }

                /*

                       //直接保存到DB
                       BaseController<tb_BoxRules> ctr = Startup.GetFromFacByName<BaseController<tb_BoxRules>>(typeof(tb_BoxRules).Name + "Controller");
                       ReturnResults<tb_BoxRules> rr = new ReturnResults<tb_BoxRules>();
                       rr = await ctr.BaseSaveOrUpdate(boxRule);
                       if (rr.Succeeded)
                       {
                           kryptonDataGridView产品.Refresh();
                           switch (boxReuleBasis)
                           {
                               case BoxRuleBasis.None:
                                   break;
                               case BoxRuleBasis.Product:
                                   (kryptonDataGridView产品.CurrentRow.DataBoundItem as View_ProdDetail).tb_prod.tb_BoxRuleses.Add(rr.ReturnObject);
                                   break;
                               case BoxRuleBasis.Attributes:
                                   (kryptonDataGridView产品.CurrentRow.DataBoundItem as View_ProdDetail).tb_Packing_forSku.Add(rr.ReturnObject);
                                   break;
                               case BoxRuleBasis.Combination:
                                   break;
                               default:
                                   break;
                           }
                       }
                */
            }
        }

        private void newSumDataGridView产品组合_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //如果双击的是行头，则是否要弹出箱规信息呢？
            // 检查是否是行头
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                // 例如，检查行的数据是否有箱规信息
                DataGridViewRow dr = newSumDataGridView产品组合.Rows[e.RowIndex];

                tb_Packing tb_Packing = null;
                BoxRuleBasis basis = GDIHelper.Instance.CheckForBoxSpecBasis(dr, out tb_Packing);

                MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                // 例如，检查行的数据是否有包装信息
                tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_Packing).Name && m.ClassPath.Contains("RUINORERP.UI.ProductEAV.UCPacking")).FirstOrDefault();
                if (RelatedBillMenuInfo == null)
                {
                    MessageBox.Show("请确认您拥有包装信息编辑权限");
                }
                if (RelatedBillMenuInfo != null && basis != BoxRuleBasis.None)
                {
                    //如果是给值。不在这处理。在生成时处理的。 这里只是调用到UI
                    menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, tb_Packing);
                    //隐藏提示信息
                    toolTip1.Hide(newSumDataGridView产品组合);
                }
            }

            if (this.Parent.Name != "kryptonPanelQuery")
            {
                return;
            }
            if (newSumDataGridView产品组合.CurrentRow != null && !chkMultiSelect.Checked)
            {
                GetQueryGroupResults();
            }

        }

        private void newSumDataGridView产品组合_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }

            //图片特殊处理
            if (newSumDataGridView产品组合.Columns[e.ColumnIndex].Name == "BundleImage")
            {
                if (newSumDataGridView产品组合.CurrentCell.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])newSumDataGridView产品组合.CurrentCell.Value);
                    Image image = Image.FromStream(buf, true);
                    if (image != null)
                    {
                        frmPictureViewer frmShow = new frmPictureViewer();
                        frmShow.PictureBoxViewer.Image = image;
                        frmShow.ShowDialog();
                    }
                }
            }
        }

    }




    // 动态类型排序比较器
    public class DynamicTypeColumnSorter : IComparer
    {
        // 排序的列索引
        private int sortColumn = 0;
        // 排序顺序
        private SortOrder sortOrder = SortOrder.None;

        // 动态类型排序优先级（从数据源获取）
        public Dictionary<string, int> TypePriority { get; set; } = new Dictionary<string, int>();

        // 设置排序的列和顺序
        public void SetSort(int column, SortOrder order)
        {
            sortColumn = column;
            sortOrder = order;
        }

        // 比较逻辑实现
        public int Compare(object x, object y)
        {
            // 将比较对象转换为TreeListViewItem
            var itemX = x as TreeListViewItem;
            var itemY = y as TreeListViewItem;

            // 处理空值情况
            if (itemX == null && itemY == null) return 0;
            if (itemX == null) return -1;
            if (itemY == null) return 1;

            // 对于树形结构，先保持父子关系，只对同层级项排序
            if (itemX.Parent != itemY.Parent)
            {
                // 不同父项的节点不改变原有顺序
                return 0;
            }

            // 获取"类型"列的值
            string typeX = itemX.SubItems[sortColumn].Text;
            string typeY = itemY.SubItems[sortColumn].Text;

            // 检查是否都在动态排序字典中
            bool hasX = TypePriority.ContainsKey(typeX);
            bool hasY = TypePriority.ContainsKey(typeY);

            // 都在字典中则按优先级排序
            if (hasX && hasY)
            {
                int compareResult = TypePriority[typeX].CompareTo(TypePriority[typeY]);

                // 根据排序顺序调整结果
                if (sortOrder == SortOrder.Descending)
                    compareResult = -compareResult;

                return compareResult;
            }

            // 不在字典中的项放在后面
            if (hasX) return -1;
            if (hasY) return 1;

            // 都不在字典中则按字符串默认规则排序
            return string.Compare(typeX, typeY, StringComparison.Ordinal) *
                   (sortOrder == SortOrder.Descending ? -1 : 1);
        }
    }


}

