using FastReport.DevComponents.DotNetBar;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Toolkit.Suite.Extended.TreeGridView;

using RUINORERP.Business;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Common;
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

namespace RUINORERP.UI.UCSourceGrid
{
    [MenuAttrAssemblyInfo("依BOM查询库存", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.库存查询)]
    public partial class UCInventoryQueryByBOM : BaseForm.BaseListGeneric<View_Inventory>
    {


        private string _queryValue = string.Empty;

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

        public UCInventoryQueryByBOM()
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
        }


        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!kryptonDataGridView产品.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = kryptonDataGridView产品.Columns[e.ColumnIndex].Name;
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
            if (kryptonDataGridView产品.Columns[e.ColumnIndex].Name == "Images")
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
            //图片特殊处理
            if (kryptonDataGridView产品.Columns[e.ColumnIndex].Name == "Images")
            {
                if (kryptonDataGridView产品.CurrentCell.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])kryptonDataGridView产品.CurrentCell.Value);
                    Image image = Image.FromStream(buf, true);
                    if (image != null)
                    {
                        frmShowImage frmShow = new frmShowImage();
                        frmShow.kryptonPictureBox1.Image = image;
                        frmShow.ShowDialog();
                    }
                }
            }

        }

        View_ProdDetailController<View_ProdDetail> prodCtrol = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();

        private void btnQueryForGoods_Click(object sender, EventArgs e)
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

            kryptonDataGridView产品.DataSource = null;

            kryptonDataGridView产品.ColumnCount = freColumnName.Length + 1;
            kryptonDataGridView产品.RowHeadersVisible = false;
            for (int i = 0; i < freColumnName.Length; i++)
            {
                kryptonDataGridView产品.Columns[i].Name = freColumnName[i];
                kryptonDataGridView产品.Columns[i].DataPropertyName = freColumnName[i];
                kryptonDataGridView产品.Columns[i].HeaderText = freColumnName[i]; //显示名称
            }
            kryptonDataGridView产品.Columns[freColumnName.Length - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            kryptonDataGridView产品.Dock = DockStyle.Fill;
            // panel2.Controls.Add(dataGridView);
            kryptonDataGridView产品.Rows?.Clear();
            kryptonDataGridView产品.DataSource = bings;
            kryptonDataGridView产品.ColumnHeadersVisible = true;
            //kryptonDataGridView1.DataSource = prodCtrol.Query(sb.ToString());
            //  kryptonDataGridView1.ColumnDisplayControl(UIHelper.GetFieldNameList<View_ProdDetail>());

        }

        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            kryptonDataGridView产品.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(View_ProdDetail));
            kryptonDataGridView产品.XmlFileName = "QueryFormGenericByBOM_" + typeof(View_ProdDetail).Name;
            kryptonDataGridView产品.FieldNameList = FieldNameList1;
            kryptonDataGridView产品.DataSource = null;
            bindingSource1.DataSource = new List<View_ProdDetail>();
            kryptonDataGridView产品.DataSource = bindingSource1;
            //if (MultipleChoices)
            //{
            //    kryptonDataGridView1.ColumnDisplayControl(cds, true);
            //    kryptonDataGridView1.Columns["Selected"].HeaderCell.ContextMenuStrip = SetContextMenuStripForSelect();
            //}

        }


        private Expression<Func<View_ProdDetail, bool>> GetQueryExp()
        {
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
           .AndIF(chk实际库存大于零.Checked, w => w.Quantity.HasValue && w.Quantity > 0)
           .ToExpression();
            return exp;
        }

        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        tb_ProdCategoriesController<tb_ProdCategories> categoriesController = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();
        ConcurrentDictionary<string, string> cds = UIHelper.GetFieldNameList<View_ProdDetail>();
        private void Query产品()
        {

            int maxrow = int.Parse(txtMaxRows.Value.ToString());
            var list = dc.BaseQueryByWhereTop(GetQueryExp(), maxrow);

            bindingSource1.DataSource = list.ToBindingSortCollection();
        }

        private void QueryToBOM()
        {
            List<long> cateids = new List<long>();
            if (chkIncludingChild.Checked && txtcategory_ID.Text.Length > 0 && txtcategory_ID.TreeView.SelectedNode != null)
            {
                cateids = categoriesController.GetChildids(long.Parse(txtcategory_ID.TreeView.SelectedNode.Name));
            }
            int maxrow = int.Parse(txtMaxRows.Value.ToString());
            var querySqlQueryable = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>().Take(maxrow)
                .IncludesAllFirstLayer()//自动导航
                .Where(GetQueryExp());
            var list = querySqlQueryable.ToList();
            treeListView1.Items.Clear();
            AddItems(list);

            // 原文链接：https://blog.csdn.net/m0_53104033/article/details/129006538
        }
        private void AddItems(List<View_ProdDetail> listDetails)
        {
            List<long> allIds = listDetails.Select(c => c.ProdDetailID).ToList();
            var listboms = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>()
                    .RightJoin<tb_ProdDetail>((a, b) => a.ProdDetailID == b.ProdDetailID)
                    .Includes(a => a.tb_producttype)
                    .Includes(a => a.tb_BOM_SDetails)
                    .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                    .Includes(a => a.tb_BOM_SDetails, b => b.view_ProdDetail)
                    .Where(a => allIds.ToArray().Contains(a.ProdDetailID))
                    .ToList();
            foreach (View_ProdDetail row in listDetails)
            {
                TreeListViewItem itemRow = new TreeListViewItem(row.CNName, 0);
                itemRow.Tag = row;
                itemRow.SubItems.Add(row.prop); //subitems只是从属于itemRow的子项。目前是四列
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
                Loadbom(bOM_S, row.ProdDetailID, itemRow);
            }
        }


        private void Loadbom(tb_BOM_S bOM_S, long ProdDetailID, TreeListViewItem listViewItem)
        {
            if (bOM_S != null && bOM_S.tb_BOM_SDetails != null)
            {
                listViewItem.ImageIndex = 1;//如果有配方，则图标不一样
                foreach (var BOM_SDetail in bOM_S.tb_BOM_SDetails)
                {
                    TreeListViewItem itemSub = new TreeListViewItem(BOM_SDetail.SubItemName, 0);
                    itemSub.Tag = BOM_SDetail.view_ProdDetail;
                    itemSub.SubItems.Add(BOM_SDetail.property);//subitems只是从属于itemRow的子项。目前是四列
                    itemSub.SubItems.Add(BOM_SDetail.SubItemSpec);
                    string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", BOM_SDetail.Type_ID);
                    itemSub.SubItems.Add(prodType);
                    if (BOM_SDetail.view_ProdDetail != null)
                    {
                        itemSub.SubItems.Add(BOM_SDetail.view_ProdDetail.Quantity.ToString());
                        itemSub.SubItems.Add(BOM_SDetail.view_ProdDetail.Sale_Qty.ToString());
                        itemSub.SubItems.Add(BOM_SDetail.view_ProdDetail.MakingQty.ToString());
                        itemSub.SubItems.Add(BOM_SDetail.view_ProdDetail.On_the_way_Qty.ToString());
                        itemSub.SubItems.Add(BOM_SDetail.view_ProdDetail.NotOutQty.ToString());
                        itemSub.SubItems.Add(BOM_SDetail.view_ProdDetail.Alert_Quantity.ToString());
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
            foreach (DataGridViewRow dr in kryptonDataGridView产品.Rows)
            {
                dr.Cells["Selected"].Value = false;
                dr.Cells["Selected"].Selected = false;
                //RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dr.DataBoundItem, "Selected", false);
            }
        }


        private void QueryForm_全选(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in kryptonDataGridView产品.Rows)
            {
                dr.Cells["Selected"].Value = true;
                dr.Cells["Selected"].Selected = true;
                //RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dr.DataBoundItem, "Selected", true);
            }
        }



        #endregion



        tb_ProdCategoriesController<tb_ProdCategories> mca = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();


        /// <summary>
        /// 是否显示选择列，能多行选中。
        /// </summary>
        public bool MultipleChoices { get; set; } = false;


        //改版
        //返回值将单个值对象等，改为数组
        public string QueryValue { get => _queryValue; set => _queryValue = value; }

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
            kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            chkMultiSelect.Checked = MultipleChoices;
            kryptonDataGridView产品.MultiSelect = MultipleChoices;
            kryptonDataGridView产品.UseSelectedColumn = MultipleChoices;
            List<tb_ProdCategories> list = new List<tb_ProdCategories>(0);
            list = await mca.QueryAsync();
            UIProdCateHelper.BindToTreeViewNoRootNode(list, txtcategory_ID.TreeView);
            DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v => v.TypeName, txtType_ID);
            InitListData();
            if (!string.IsNullOrEmpty(QueryValue))
            {
                QueryObject.SetPropertyValue(QueryField, QueryValue);
                BindData();
            }

            //Query();

            kryptonNavigator1.SelectedPage = kryptonPage产品;
        }


        public void BindData()
        {
            View_ProdDetail entity = QueryObject as View_ProdDetail;
            if (entity == null)
            {
                MainForm.Instance.uclog.AddLog("实体不能为空", UILogType.警告);
                return;
            }


            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.SKU, txtSKU码, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v => v.TypeName, txtType_ID);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.CNName.ToString(), txtName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.BarCode.ToString(), txtBarCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text, false);

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
            if (kryptonDataGridView产品.CurrentRow != null && !chkMultiSelect.Checked)
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
            kryptonDataGridView产品.EndEdit();
            if (kryptonDataGridView产品.SelectedRows != null)
            {

                if (MultipleChoices)
                {
                    foreach (DataGridViewRow dr in kryptonDataGridView产品.Rows)
                    {
                        if (!(dr.DataBoundItem is View_ProdDetail))
                        {
                            MessageBox.Show("TODO:请调试这里");
                        }
                        if (MultipleChoices && (bool)dr.Cells["Selected"].Value)
                        {
                            QueryObjects.Add(dr.DataBoundItem as View_ProdDetail);
                        }
                    }
                }
                else
                {
                    #region
                    foreach (DataGridViewRow dr in kryptonDataGridView产品.SelectedRows)
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
                }


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


            }
            else
            {

            }

        }

        private void GetBOMResults(TreeListViewItem listViewItem)
        {
            if (listViewItem.Items.Count > 0)
            {
                foreach (TreeListViewItem dr in listViewItem.Items)
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

        }

        private void chkMultiSelect_CheckedChanged(object sender, EventArgs e)
        {
            MultipleChoices = chkMultiSelect.Checked;
            kryptonDataGridView产品.UseSelectedColumn = MultipleChoices;
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
                    if (!QueryObjects.Contains((View_ProdDetail)item.Tag))
                    {
                        QueryObjects.Add((View_ProdDetail)item.Tag);
                    }
                }
            }
            if (QueryObjects.Count > 0)
            {
                if (!string.IsNullOrEmpty(QueryField))
                {
                    QueryValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(QueryObjects[0], QueryField).ToString();
                }
            }


        }

        private void kryptonNavigator1_SelectedPageChanged(object sender, EventArgs e)
        {
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
    }
}
