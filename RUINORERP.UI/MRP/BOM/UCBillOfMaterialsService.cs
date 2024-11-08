using FastReport.DevComponents.DotNetBar;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using Krypton.Workspace;
using Netron.GraphLib;
 
using Org.BouncyCastle.Math;
using RUINORERP.Business;
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
using RUINOR.WinFormsUI.CustomPictureBox;
namespace RUINORERP.UI.MRP.BOM
{

    /// <summary>
    /// 产品清单的多版本时。默认清单的指定，以及清单的版本，文件等维护
    /// 即 查询产品表。条件是他有BOM并且有多个版本时。显示出来，并且可以指定默认版本
    /// </summary>
    [MenuAttrAssemblyInfo("产品配方维护", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.MRP基本资料, BizType.BOM物料清单)]
    public partial class UCBillOfMaterialsService : BaseUControl
    {

        /*
         目前这个是用于一个公用的查询模板，对于一些特殊情况下的调用。用枚举类型来区别一下。
        公共查询入口 ，将来可以扩展为多个查询入口，如我的收藏产品，我的产品，我的供应商产品等
        主要返回是产品列表，或者产品ID列表，同时像套装组合时也要返回组合的一个信息，这时套装tab页要显示需要的套装。并返回套装信息和套数。
         */



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

        public UCBillOfMaterialsService()
        {
            InitializeComponent();

            List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
            kvlist.Add(new KeyValuePair<object, string>(true, "男"));
            kvlist.Add(new KeyValuePair<object, string>(false, "女"));
            Expression<Func<tb_Employee, bool?>> expr;
            expr = (p) => p.Gender;// == name;
            var mb = expr.GetMemberInfo();
            string colName = mb.Name;
            ColNameDataDictionary.TryAdd(colName, kvlist);


            List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            System.Linq.Expressions.Expression<Func<tb_Employee, bool?>> expr1;
            expr1 = (p) => p.Is_available;// == name;
            System.Linq.Expressions.Expression<Func<tb_Employee, bool?>> expr2;
            expr2 = (p) => p.Is_enabled;// == name;
            string colName1 = expr1.GetMemberInfo().Name;
            string colName2 = expr2.GetMemberInfo().Name;
            ColNameDataDictionary.TryAdd(colName1, kvlist1);


            List<tb_BOM_S> bomlist = new List<tb_BOM_S>();
            bomlist = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>().ToList();
            List<KeyValuePair<object, string>> kvbomlist = new List<KeyValuePair<object, string>>();
            foreach (var item in bomlist)
            {
                kvbomlist.Add(new KeyValuePair<object, string>(item.BOM_ID, item.BOM_No + ":" + item.BOM_Name));
            }
            System.Linq.Expressions.Expression<Func<View_ProdDetail, object>> expbomID;
            expbomID = (p) => p.BOM_ID;
            ColNameDataDictionary.TryAdd(expbomID.GetMemberInfo().Name, kvbomlist);

            kryptonNavigator1.SelectedPageChanged += KryptonNavigator1_SelectedPageChanged;

            newSumDataGridView产品.CustomRowNo = true;
            newSumDataGridView产品.ShowCellToolTips = true;
            newSumDataGridView产品.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
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
                e.Handled = true;
            }
        }


        private void KryptonDataGridViewBOM_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
                // 例如，检查行的数据是否为默认的配方
                if (newSumDataGridView产品.CurrentRow != null)
                {
                    if (newSumDataGridView产品.CurrentRow.DataBoundItem is View_ProdDetail prodDetail &&
                        newSumDataGridViewBOM.Rows[e.RowIndex].DataBoundItem is tb_BOM_S bom)
                    {
                        if (bom.ProdDetailID == prodDetail.ProdDetailID)
                        {
                            GDIHelper.Instance.DrawPattern(e, Color.DarkGreen);
                        }
                    }
                }


                e.Handled = true;

            }
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


        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridView产品.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = newSumDataGridView产品.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }



            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (newSumDataGridView产品.Columns[e.ColumnIndex].Name == "Images")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    if (image != null)
                    {
                        //缩略图 这里用缓存 ?
                        Image thumbnailthumbnail = this.thumbnail(image, 100, 100);
                        e.Value = thumbnailthumbnail;
                    }

                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }


        // 缩小图片为缩略图
        private Image thumbnail(Image image, int width, int height)
        {
            // 创建缩略图的新图像
            Bitmap thumbnail = new Bitmap(width, height);

            // 使用 Graphics 对象绘制缩略图
            using (Graphics graphics = Graphics.FromImage(thumbnail))
            {
                // 设置绘制质量
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 绘制缩略图
                graphics.DrawImage(image, 0, 0, width, height);
            }

            return thumbnail;
        }


        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                // 例如，检查行的数据是否有箱规信息
                DataGridViewRow dr = newSumDataGridView产品.Rows[e.RowIndex];
                if (dr.DataBoundItem is View_ProdDetail prodDetail)
                {
                    bsBoms.DataSource = prodDetail.tb_BOM_Ss;
                }

            }

        }



        private void btnQueryForGoods_Click(object sender, EventArgs e)
        {
            Query();
        }


        public override void Query()
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
                        QueryBOM();
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


        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            newSumDataGridView产品.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridView产品.XmlFileName = "UCBillOfMaterialsService_" + typeof(View_ProdDetail).Name;
            newSumDataGridView产品.FieldNameList = UIHelper.GetFieldNameColList(typeof(View_ProdDetail));
            newSumDataGridView产品.DataSource = null;
            bindingSourceProdDetail.DataSource = new List<View_ProdDetail>();
            newSumDataGridView产品.DataSource = bindingSourceProdDetail;


            newSumDataGridView产品组合.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridView产品组合.XmlFileName = "UCBillOfMaterialsServiceRelated_" + typeof(tb_Files).Name;
            newSumDataGridView产品组合.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_Files));
            newSumDataGridView产品组合.DataSource = null;
            bindingSourceGroup.DataSource = new List<tb_Files>();
            newSumDataGridView产品组合.DataSource = bindingSourceGroup;

            #region 双击产品显示的BOM表格
            newSumDataGridViewBOM.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridViewBOM.XmlFileName = "UCBillOfMaterialsService_" + typeof(tb_BOM_S).Name;
            newSumDataGridViewBOM.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_BOM_S));

            //这里设置了指定列不可见
            List<Expression<Func<tb_BOM_S, object>>> ChildRelatedInvisibleCols = new List<Expression<Func<tb_BOM_S, object>>>();
            ChildRelatedInvisibleCols.Add(C => C.BOM_ID);
            ChildRelatedInvisibleCols.Add(C => C.ProdDetailID);
            List<string> InvisibleCols = ExpressionHelper.ExpressionListToStringList(ChildRelatedInvisibleCols);
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewBOM.FieldNameList.TryRemove(item, out kv);
            }
            newSumDataGridViewBOM.DataSource = null;
            bsBoms.DataSource = new List<tb_BOM_S>();
            newSumDataGridViewBOM.DataSource = bsBoms;
            #endregion

            kryptonPanelBOM.Visible = false;

            #region 查询BOM的表格
            newSumDataGridViewMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridViewMain.XmlFileName = "UCBillOfMaterialsServiceMain_" + typeof(View_BOM).Name;
            newSumDataGridViewMain.FieldNameList = UIHelper.GetFieldNameColList(typeof(View_BOM));

            //这里设置了指定列不可见
            List<Expression<Func<View_BOM, object>>> ChildRelatedInvisibleMainCols = new List<Expression<Func<View_BOM, object>>>();
            ChildRelatedInvisibleMainCols.Add(C => C.BOM_ID);
            ChildRelatedInvisibleMainCols.Add(C => C.ProdDetailID);
            List<string> InvisibleMianCols = ExpressionHelper.ExpressionListToStringList(ChildRelatedInvisibleMainCols);
            foreach (var item in InvisibleMianCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                newSumDataGridViewMain.FieldNameList.TryRemove(item, out kv);
            }
            newSumDataGridViewMain.DataSource = null;
            bindingSourceBomMain.DataSource = new List<View_BOM>();
            newSumDataGridViewMain.DataSource = bindingSourceBomMain;
            #endregion
        }

        private Expression<Func<View_ProdDetail, bool>> GetQueryProdExp()
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

           .AndIF(txtShortCode.Text.Trim().Length > 0, w => w.ShortCode.Contains(txtShortCode.Text.Trim()))
           .AndIF(txtProp.Text.Trim().Length > 0, w => w.prop.Contains(txtProp.Text.Trim()))

           .AndIF(txtBarCode.Text.Trim().Length > 0, w => w.BarCode.Contains(txtBarCode.Text.Trim()))
           .AndIF(txtSKU码.Text.Trim().Length > 0, w => w.SKU.Contains(txtSKU码.Text.Trim()))
           .AndIF(txtNo.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtNo.Text.Trim()))
           .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
           .AndIF(chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null, c => cateids.ToArray().Contains(c.Category_ID.Value))
           .AndIF(!chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null, w => w.Category_ID.Value == long.Parse(txtcategory_ID.TreeView.SelectedNode.Name.ToString()))


           .ToExpression();
            return exp;
        }

        private Expression<Func<View_BOM, bool>> GetQueryBOMExp()
        {
            tb_ProdCategoriesController<tb_ProdCategories> categoriesController = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
            List<long> cateids = new List<long>();
            if (chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null)
            {
                cateids = categoriesController.GetChildids(long.Parse(txtcategory_ID.TreeView.SelectedNode.Name));
            }
            Expression<Func<View_BOM, bool>> exp = Expressionable.Create<View_BOM>() //创建表达式
           .AndIF(cmbParentProdType.SelectedValue != null && cmbParentProdType.SelectedValue.ToString() != "-1", w => w.Type_ID == long.Parse(cmbParentProdType.SelectedValue.ToString()))
           .AndIF(txtBOM_Name.Text.Trim().Length > 0, w => w.BOM_Name.Contains(txtBOM_Name.Text.Trim()))
           .AndIF(txtSKUCode.Text.Trim().Length > 0, w => w.SKU.Contains(txtSKUCode.Text.Trim()))
           .AndIF(txtBOM_NO.Text.Trim().Length > 0, w => w.BOM_No.Contains(txtBOM_NO.Text.Trim()))
           .AndIF(txtSKUName.Text.Trim().Length > 0, w => w.CNName.Contains(txtSpecifications.Text.Trim()))
           .ToExpression();
            return exp;
        }

        ConcurrentDictionary<string, string> cds = UIHelper.GetFieldNameList<View_ProdDetail>();
        private void Query产品()
        {
            View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            int maxrow = int.Parse(txtMaxRows.Value.ToString());

            var querySqlQueryable = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>()
                .Includes(c => c.tb_BOM_Ss)
                .Includes(c => c.tb_prod)
                .Includes(c => c.tb_proddetail)
                .Includes(c => c.tb_bom_s, e => e.tb_BOM_SDetails)
                .Take(maxrow).Where(GetQueryProdExp())
                .WhereIF(chkMultiBOMProd.Checked, it => SqlFunc.Subqueryable<tb_BOM_S>()
                .Where(x => x.ProdDetailID == it.ProdDetailID).Count() > 1)
                .Where(it => SqlFunc.Subqueryable<tb_BOM_S>()
                .Where(x => x.ProdDetailID == it.ProdDetailID).Any());//并且明细产品ID要存在于BOM表中

            var list = querySqlQueryable.ToList();

            bindingSourceProdDetail.DataSource = list.ToBindingSortCollection();
        }

        private void QueryBOM()
        {
            tb_ProdCategoriesController<tb_ProdCategories> categoriesController = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
            List<long> cateids = new List<long>();
            if (chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null)
            {
                cateids = categoriesController.GetChildids(long.Parse(txtcategory_ID.TreeView.SelectedNode.Name));
            }
            int maxrow = int.Parse(txtMaxRows.Value.ToString());
            var querySqlQueryable = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_BOM>().Take(maxrow)
                .IncludesAllFirstLayer()//自动导航
                .WhereIF(chkMultiBOMProd.Checked, it => SqlFunc.Subqueryable<tb_BOM_S>()
                .Where(x => x.ProdDetailID == it.ProdDetailID).Count() > 1)
                .Where(GetQueryBOMExp());
            var list = querySqlQueryable.ToList();
            bindingSourceBomMain.DataSource = list.ToBindingSortCollection();
            // 原文链接：https://blog.csdn.net/m0_53104033/article/details/129006538
        }

        private void Query产品组合()
        {
            tb_FilesController<tb_Files> dc = Startup.GetFromFac<tb_FilesController<tb_Files>>();
            int maxrow = int.Parse(txtMaxRows.Value.ToString());
            Expression<Func<tb_Files, bool>> exp = Expressionable.Create<tb_Files>() //创建表达式
                                                                                     // .And(w => w.Is_enabled == chkProdBundle_enabled.Checked)
                                                                                     //.And(w => w.Is_available == chkProdBundle_available.Checked)
           .AndIF(txtFilesName.Text.Trim().Length > 0, w => w.FileName.Contains(txtFilesName.Text.Trim()))
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


            chkProdBundle_available.Checked = true;
            chkProdBundle_enabled.Checked = true;

            kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;

            newSumDataGridView产品.MultiSelect = MultipleChoices;
            newSumDataGridView产品.UseSelectedColumn = MultipleChoices;

            newSumDataGridView产品组合.MultiSelect = MultipleChoices;
            newSumDataGridView产品组合.UseSelectedColumn = MultipleChoices;

            List<tb_ProdCategories> list = new List<tb_ProdCategories>(0);
            tb_ProdCategoriesController<tb_ProdCategories> mca = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
            list = await mca.QueryAsync();
            UIProdCateHelper.BindToTreeViewNoRootNode(list, txtcategory_ID.TreeView);
            DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v => v.TypeName, txtType_ID);
            DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v => v.TypeName, cmbParentProdType);

            InitListData();

            ContextMenuStrip newContextMenuStrip = newSumDataGridView产品.GetContextMenu(contextMenuStrip1);
            newSumDataGridView产品.ContextMenuStrip = newContextMenuStrip;
            newSumDataGridView产品组合.ContextMenuStrip = newContextMenuStrip;


            newSumDataGridViewBOM.ContextMenuStrip = newSumDataGridViewBOM.GetContextMenu(contextMenuStrip2); ;

            if (QueryValue != null && QueryValue.ToString().Length > 0)
            {
                QueryObject.SetPropertyValue(QueryField, QueryValue);
                BindData();
            }

            //Query();

            kryptonNavigator1.SelectedPage = kryptonPage产品;
            SetPage();

        }

        private void KryptonNavigator1_SelectedPageChanged(object sender, EventArgs e)
        {
            SetPage();
        }


        private void SetPage()
        {
            if (kryptonNavigator1.SelectedPage == kryptonBOM)
            {
                kryptonPanelBOM.Visible = true;
                kryptonPanelBOM.Location = kryptonPanelProd.Location;
                kryptonPanelBOM.Dock = DockStyle.Fill;
                kryptonPanelBOM.Size = kryptonPanelProd.Size;
                kryptonPanelProd.Visible = false;
                kryptonPanelGroup.Visible = false;
                kryptonPanelProd.Visible = true;
                kryptonPanelProd.Dock = DockStyle.Fill;
                newSumDataGridViewBOM.Visible = false;
            }

            if (kryptonNavigator1.SelectedPage == kryptonPage产品)
            {
                kryptonPanelBOM.Visible = false;
                kryptonPanelGroup.Visible = false;
                kryptonPanelProd.Visible = true;
                kryptonPanelProd.Dock = DockStyle.Fill;
                newSumDataGridViewBOM.Visible = true;
                //kryptonDataGridView产品.ContextMenuStrip = contextMenuStrip1;
                //newSumDataGridView产品.Use是否使用内置右键功能 = true;
                //newSumDataGridView产品.SetContextMenu(contextMenuStrip1);
                //newSumDataGridView产品组合.ContextMenuStrip = null;

            }
            if (kryptonNavigator1.SelectedPage == kryptonPage产品组合)
            {
                kryptonPanelBOM.Visible = false;
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

                        break;
                    case "kryptonPage产品组合":

                        break;
                    case "kryptonBOM":

                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 这里字段是用来可以主动设置查询条件的
        /// 其它 的实际是可以不设置
        /// </summary>
        public  void BindData()
        {
            View_ProdDetail entity = QueryObject as View_ProdDetail;
            if (entity == null)
            {

                return;
            }

            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.SKU, txtSKU码, BindDataType4TextBox.Text, false);

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
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                // 例如，检查行的数据是否有箱规信息
                DataGridViewRow dr = newSumDataGridView产品.Rows[e.RowIndex];
                if (dr.DataBoundItem is View_ProdDetail prodDetail)
                {
                    bsBoms.DataSource = prodDetail.tb_BOM_Ss;
                }

            }

            if (this.Parent.Name != "kryptonPanelQuery")
            {
                return;
            }

        }


        private void chkMultiSelect_CheckedChanged(object sender, EventArgs e)
        {

            newSumDataGridView产品.UseSelectedColumn = MultipleChoices;
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
            if (newSumDataGridView产品组合.CurrentRow != null)
            {

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

        private void newSumDataGridView产品组合_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridView产品组合.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = newSumDataGridView产品组合.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }



            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (newSumDataGridView产品组合.Columns[e.ColumnIndex].Name == "BundleImage")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    if (image != null)
                    {
                        //缩略图 这里用缓存 ?
                        Image thumbnailthumbnail = this.thumbnail(image, 100, 100);
                        e.Value = thumbnailthumbnail;
                    }
                }
            }

        }

        private async void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //为产品设置默认的配方
            if (newSumDataGridView产品.CurrentRow != null && newSumDataGridViewBOM.CurrentRow != null)
            {
                if (newSumDataGridView产品.CurrentRow.DataBoundItem is View_ProdDetail prodDetail &&
                    newSumDataGridViewBOM.CurrentRow.DataBoundItem is tb_BOM_S bom)
                {
                    prodDetail.tb_proddetail.BOM_ID = bom.BOM_ID;
                    int affectedRows = await MainForm.Instance.AppContext.Db.Updateable<tb_ProdDetail>(prodDetail.tb_proddetail)
                           .UpdateColumns(it => new { it.BOM_ID })
                           .Where(it => it.ProdDetailID == prodDetail.ProdDetailID)
                           .ExecuteCommandAsync();
                    if (affectedRows > 0)
                    {
                        MainForm.Instance.uclog.AddLog($"产品{prodDetail.CNName}-{prodDetail.SKU}的默认配方成功设置为：{bom.BOM_No}-{bom.BOM_Name}");
                    }
                }
            }
        }
    }
}

