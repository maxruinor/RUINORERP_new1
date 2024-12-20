using RUINORERP.Business;
using RUINORERP.Model;
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
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.Common;
using System.Collections.Concurrent;
using RUINORERP.UI.UCSourceGrid;
using RUINORERP.Model.Dto;
using RUINORERP.UI.BaseForm;
using Netron.GraphLib;
using RUINORERP.Global;
using MathNet.Numerics.LinearAlgebra.Factorization;
using RUINORERP.Common.Extensions;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINOR.WinFormsUI.TileListView;
using RUINOR.WinFormsUI;

using System.Security.Cryptography;
using SiteRules.aliexpress;
using static OfficeOpenXml.ExcelErrorValue;
using SqlSugar.SplitTableExtensions;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Common.Helper;
using ObjectsComparer;
using System.Collections;
using Force.DeepCloner;
using AutoUpdateTools;
namespace RUINORERP.UI.ProductEAV
{

    [MenuAttrAssemblyInfo("多属性编辑", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCMultiPropertyEditor : UCBaseClass
    {
        public UCMultiPropertyEditor()
        {
            InitializeComponent();
        }

        private void btnQueryForGoods_Click(object sender, EventArgs e)
        {
            Query();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Exit(this);
                        break;
                    case Keys.F1:

                        break;
                    case Keys.Enter:
                        Query();
                        break;
                }

            }
            return false;
        }
        public void Query()
        {
            tb_ProdController<tb_Prod> dc = Startup.GetFromFac<tb_ProdController<tb_Prod>>();

            Expression<Func<tb_Prod, bool>> exp = Expressionable.Create<tb_Prod>() //创建表达式
           .AndIF(txtModel.Text.Trim().Length > 0, w => w.Model.Contains(txtModel.Text.Trim()))
           .AndIF(txtName.Text.Trim().Length > 0, w => w.CNName.Contains(txtName.Text.Trim()))
           .AndIF(txtNo.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtNo.Text.Trim()))
           .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtSpecifications.Text.Trim()))
            .ToExpression();
            var list = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_Prod>()
                .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                .Includes(a => a.tb_ProdDetails, b => b.tb_Prod_Attr_Relations, c => c.tb_prodpropertyvalue, d => d.tb_prodproperty)
                .Includes(c => c.tb_Prod_Attr_Relations, d => d.tb_prodpropertyvalue, e => e.tb_prodproperty)
                .Includes(c => c.tb_Prod_Attr_Relations, d => d.tb_proddetail)
                .Where(exp)
                .Take(50)
                .ToList();


            //var list = dc.BaseQueryByWhereTop(exp, 100);


            bindingSourc产品.DataSource = list.ToBindingSortCollection();
        }
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;


        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();


        private void UCMultiPropertyEditor_Load(object sender, EventArgs e)
        {
            ImgCol.DefaultCellStyle.NullValue = null;

            // load image strip
            this.imageStrip.ImageSize = new System.Drawing.Size(16, 16);
            this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
            //var imageNew = Properties.Resources.add1.ToBitmap();
            var imageLoad = Properties.Resources._1616_selected_ok_;
            this.imageStrip.Images.AddStrip(imageLoad);//0 加载，1 新增 2 编辑 3 删除 4 下拉
            var imageNew = Properties.Resources._1616_add_;
            this.imageStrip.Images.AddStrip(imageNew);
            var imageEdit = Properties.Resources._1616_edit_;
            this.imageStrip.Images.AddStrip(imageEdit);
            var imageDelete = Properties.Resources._1616_delete_;
            this.imageStrip.Images.AddStrip(imageDelete);

            var image = Properties.Resources._1616_down_;
            this.imageStrip.Images.AddStrip(image);


            treeGridView1.ImageList = imageStrip;
            // attachment header cell
            this.ImgCol.HeaderCell = new AttachmentColumnHeader(imageStrip.Images[4]);
            this.ImgCol.Visible = false;//隐藏图片列 关系ID列

            EnumBindingHelper bindingHelper = new EnumBindingHelper();
            //https://www.cnblogs.com/cdaniu/p/15236857.html
            //加载枚举，并且可以过虑不需要的项
            List<int> exclude = new List<int>();
            exclude.Add((int)ProductAttributeType.虚拟);
            exclude.Add((int)ProductAttributeType.捆绑);
            bindingHelper.InitDataToCmbByEnumOnWhere<tb_Prod>(typeof(ProductAttributeType).GetListByEnum(2, exclude.ToArray()), e => e.PropertyType, cmbPropertyType);

            prodpropValueList = mcPropertyValue.QueryByNav(c => true);


            prodpropList = mcProperty.QueryByNav(c => true);
            //DataBindingHelper.BindData4CmbByEnumData<tb_Prod>(entity, k => k.PropertyType, cmbPropertyType);
            DataBindingHelper.InitDataToCmb<tb_ProdProperty>(p => p.Property_ID, t => t.PropertyName, cmb属性);
            dataGridViewProd.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(tb_Prod));
            dataGridViewProd.XmlFileName = "UCMultiPropertyEditor_" + typeof(tb_Prod).Name;
            dataGridViewProd.FieldNameList = FieldNameList1;
            dataGridViewProd.DataSource = null;
            bindingSourc产品.DataSource = new List<tb_Prod>();
            dataGridViewProd.DataSource = bindingSourc产品;

            listView1.ContextMenuStrip = contextMenuStrip1;
            treeGridView1.ContextMenuStrip = contextMenuStripTreeGrid;
            //LoadGrid1();
        }




        //当前编辑的品
        tb_Prod EditEntity;


        private void cmb属性_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb属性.SelectedItem is tb_ProdProperty)
            {
                tb_ProdProperty ppv = cmb属性.SelectedItem as tb_ProdProperty;
                if (!listView1.Groups.Cast<TileGroup>().Any(i => i.GroupID == ppv.Property_ID.ToString().Trim()))
                {
                    btnAddProperty.Enabled = true;
                }

            }
            return;
        }
        List<tb_ProdProperty> prodpropList = new List<tb_ProdProperty>();
        List<tb_ProdPropertyValue> prodpropValueList = new List<tb_ProdPropertyValue>();
        tb_ProdPropertyController<tb_ProdProperty> mcProperty = Startup.GetFromFac<tb_ProdPropertyController<tb_ProdProperty>>();
        tb_ProdPropertyValueController<tb_ProdPropertyValue> mcPropertyValue = Startup.GetFromFac<tb_ProdPropertyValueController<tb_ProdPropertyValue>>();


        /// <summary>
        /// 属性原始组合队列，以文字显示。方便查看而已。后面已经改为了ID
        /// </summary>
        // public List<KeyValuePair<long, string[]>> attrGoupsByName { get => _attrGoups; set => _attrGoups = value; }

        // private List<KeyValuePair<long, string[]>> _attrGoups = new List<KeyValuePair<long, string[]>>();


        private void btnAddProperty_Click(object sender, EventArgs e)
        {
            if (cmb属性.SelectedItem == null)
            {
                return;
            }
            if (cmb属性.SelectedItem is tb_ProdProperty)
            {
                tb_ProdProperty ppv = cmb属性.SelectedItem as tb_ProdProperty;
                if (ppv.Property_ID == -1)
                {
                    return;
                }
                AddProperty(ppv);
                listView1.UpdateUI();
            }
        }


        private void AddProperty(tb_ProdProperty ppv)
        {
            if (ppv == null)
            {
                return;
            }
            if (ppv.Property_ID == -1)
            {
                return;
            }
            AddProdProperty(ppv, prodpropValueList);
            ControlBtn(ProductAttributeType.可配置多属性);
            // List<KeyValuePair<long, string[]>> attrGoupsByName = GetAttrGoups(listView1, g => g.GroupID, lvitem => lvitem.Text);
            //this.attrGoupsByName = attrGoupsByName;
            //CreateSKUList();
        }


        /*
                /// <summary>
                /// 将listview的UI值转为属性组
                /// </summary>
                /// <param name="lv"></param>
                /// <param name="getId">取ID</param>
                /// <param name="getValue">取名称值</param>
                /// <returns></returns>
                private List<KeyValuePair<long, string[]>> GetAttrGoups(TileListView lv, Func<TileGroup, string> getId, Func<CheckBox, string> getValue)
                {
                    List<KeyValuePair<long, string[]>> attrGoups = new List<KeyValuePair<long, string[]>>();
                    foreach (TileGroup g in lv.Groups)
                    {
                        tb_ProdProperty pp = g.BusinessData as tb_ProdProperty;
                        long key = getId(g).ToLong();
                        string values = string.Empty;
                        foreach (CheckBox lvitem in g.Items)
                        {
                            if (lvitem.Checked)
                            {
                                values += getValue(lvitem) + ",";
                            }
                        }
                        values = values.TrimEnd(',');
                        if (values.Length > 0)
                        {
                            KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(key, values.Split(','));
                            attrGoups.Add(kvp);
                        }
                    }
                    return attrGoups;
                }
        */

        /// <summary>
        /// 将listview的UI值转为属性组 属性ID+值的ID|名称
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="getId"></param>
        /// <param name="getValue"></param>
        /// <returns></returns>
        private List<KeyValuePair<long, string[]>> GetAttrGoupsByIDName(TileListView lv, Func<TileGroup, string> getId, Func<CheckBox, string> getValue)
        {
            List<KeyValuePair<long, string[]>> attrGoups = new List<KeyValuePair<long, string[]>>();
            foreach (TileGroup g in lv.Groups)
            {
                tb_ProdProperty pp = g.BusinessData as tb_ProdProperty;
                long GroupKey = getId(g).ToLong();
                string values = string.Empty;
                foreach (CheckBox lvitem in g.Items)
                {
                    if (lvitem.Checked)
                    {
                        // 将 ID 和名称组合起来
                        values += $"{lvitem.Name}|{getValue(lvitem)},";
                    }
                }
                values = values.TrimEnd(',');
                if (values.Length > 0)
                {
                    KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(GroupKey, values.Split(','));
                    attrGoups.Add(kvp);
                }
            }
            return attrGoups;
        }

        /*
        private void CreateSKUList()
        {
            List<Eav_ProdDetails> propGroups = new List<Eav_ProdDetails>();
            if (bindingSourceSKU明细.DataSource is List<Eav_ProdDetails>)
            {
                propGroups = bindingSourceSKU明细.DataSource as List<Eav_ProdDetails>;
            }

            //如果存在则更新，
            // List<string> rs = ArrayCombination.Combination4Table(para);
            //目前在数据库端控制属性值表中的名称不能重复。再通过对应的值名找对应的属性值ID和属性ID
            List<string> newMix = new List<string>(); //  ArrayCombination.Combination4Table(attrGoupsByName);
            ////Intersect 交集，Except 差集，Union 并集 Distinct去重 如果是值类型可以直接用，如果是引用类型则要重写 
            //参考凯旋游戏中的 差集合等处理再加上排序
            //按组合先后排序？
            List<string> oldMix = new List<string>();
            foreach (Eav_ProdDetails epd in propGroups)
            {
                oldMix.Add(epd.GroupName);
            }

            var Item交集 = newMix.Intersect(oldMix);// 交集
            //如果交集没有，则认为新的，与旧的完全不一样。把旧的全删除
            if (Item交集.Count() == 0)
            {
                foreach (var old in oldMix)
                {
                    //更新删除
                    Eav_ProdDetails ep = propGroups.Cast<Eav_ProdDetails>().FirstOrDefault(w => w.GroupName == old.Trim());
                    bindingSourceSKU明细.Remove(ep);
                    //将删除的sku行 暂时加入一个临时列表中
                    // removeSkuList.Add(ep);
                }
                //添加新的
                foreach (var newItem in newMix)
                {
                    Eav_ProdDetails ppg = new Eav_ProdDetails();
                    ppg.GroupName = newItem;
                    ppg.SKU = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.SKU_No);
                    if (MainForm.Instance.AppContext.SysConfig.UseBarCode)
                    {
                        //补码
                        ppg.BarCode = BizCodeGenerator.Instance.GetBarCode(ppg.SKU, EditEntity.CNName.Substring(0).ToCharArray()[0]);
                    }
                    bindingSourceSKU明细.Add(ppg);
                }
            }
            else
            {
                //如果组合少了，则删除？
                if (newMix.Count < oldMix.Count)
                {
                    var Item差集 = oldMix.Except(newMix);
                    foreach (var item in Item差集)
                    {
                        //更新删除
                        Eav_ProdDetails ep = propGroups.Cast<Eav_ProdDetails>().FirstOrDefault(w => w.GroupName == item.Trim());
                        bindingSourceSKU明细.Remove(ep);
                        //将删除的sku行 暂时加入一个临时列表中
                        // removeSkuList.Add(ep);
                    }
                }
                else
                {
                    var Item差集 = newMix.Except(oldMix);
                    foreach (var item in Item差集)
                    {
                        Eav_ProdDetails ppg = new Eav_ProdDetails();
                        ppg.GroupName = item;
                        ppg.SKU = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.SKU_No);
                        bindingSourceSKU明细.Add(ppg);

                    }
                }

            }
        }
        */

        /// <summary>
        /// 单属性可以变为多属性，反过来不可以。
        /// </summary>
        /// <param name="pat"></param>
        private void ControlBtn(ProductAttributeType pat)
        {
            switch (pat)
            {
                case ProductAttributeType.单属性:
                    cmb属性.Enabled = false;
                    btnAddProperty.Enabled = false;
                    btnClear.Enabled = false;
                    cmbPropertyType.Enabled = true;

                    break;
                case ProductAttributeType.可配置多属性:
                    cmb属性.Enabled = true;
                    btnAddProperty.Enabled = true;
                    btnClear.Enabled = true;
                    cmbPropertyType.Enabled = false;

                    break;
                case ProductAttributeType.捆绑:
                    break;
                case ProductAttributeType.虚拟:
                    break;
                default:
                    break;
            }
        }





        /// <summary>    
        /// 添加右键菜单项   
        /// </summary>    
        /// <param name="text">要显示的文字，如果为 - 则显示为分割线</param>    
        /// <param name="cms">要添加到的子菜单集合</param>    
        /// <param name="callback">点击时触发的事件</param>    
        /// <returns>生成的子菜单，如果为分隔条则返回null</returns>    
        ToolStripMenuItem AddContextMenu(string text, ToolStripItemCollection cms, EventHandler callback)
        {
            if (text == "-")
            {
                ToolStripSeparator tsp = new ToolStripSeparator();
                cms.Add(tsp);

                return null;
            }
            else if (!string.IsNullOrEmpty(text))
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(text);// Image.FromFile("图标路径"));
                tsmi.Name = text;
                if (callback != null)
                {
                    tsmi.Click += callback;
                }
                cms.Add(tsmi);

                tsmi.Font = new Font("Arial", 9, FontStyle.Regular); //字体设置   
                                                                     //tsmi.Image = Image.FromFile("图标路径"); //菜单图标设置   

                return tsmi;
            }
            return null;
        }
        /// <summary>   
        /// 菜单事件   
        /// </summary>   
        void menuClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem stripItem = (ToolStripMenuItem)sender;
            tb_ProdProperty tpp = stripItem.Tag as tb_ProdProperty;
            bool isAll = false;
            if (stripItem.Text.Contains("全选"))
            {
                isAll = true;
            }
            foreach (TileGroup g in listView1.Groups)
            {
                tb_ProdProperty pp = g.BusinessData as tb_ProdProperty;
                if (pp.Property_ID == tpp.Property_ID)
                {
                    foreach (CheckBox lvitem in g.Items)
                    {

                        if (lvitem.Checked != isAll)
                        {
                            lvitem.Checked = isAll;
                        }

                    }
                    break;
                }
            }

        }
        /// <summary>
        /// 添加产品特性
        /// </summary>
        private void AddProdProperty(tb_ProdProperty ppv, List<tb_ProdPropertyValue> listOptionValue)
        {
            #region 新增修改式
            if (!contextMenuStrip1.Items.ContainsKey("【" + ppv.PropertyName + "】全选"))
            {
                //加入分割线 美观一下
                if (contextMenuStrip1.Items.Count > 0)
                {
                    AddContextMenu("-", contextMenuStrip1.Items, menuClicked);
                    //属性都多了，之前的值全是不需要的
                    bindingSourceSKU明细.Clear();
                }
                //添加菜单   
                var yes = AddContextMenu("【" + ppv.PropertyName + "】全选", contextMenuStrip1.Items, menuClicked);
                yes.Tag = ppv;
                var no = AddContextMenu("【" + ppv.PropertyName + "】全不选", contextMenuStrip1.Items, menuClicked);
                no.Tag = ppv;
            }

            if (!listView1.Groups.Cast<TileGroup>().Any(i => i.GroupID == ppv.Property_ID.ToString().Trim()))
            {
                TileGroup tileGroup = listView1.AddGroup(ppv.Property_ID.ToString(), ppv.PropertyName.Trim());
                tileGroup.BusinessData = ppv;

                string keys = string.Empty;
                string names = string.Empty;
                foreach (var item in listOptionValue.Where(w => w.Property_ID == ppv.Property_ID).ToList())
                {
                    keys += item.PropertyValueID + ",";
                    names += item.PropertyValueName + ",";

                    //CheckBox checkBox = new CheckBox
                    bool selected = false;
                    //如果是加载数据时则根据数据判断选中状态
                    if (EditEntity.tb_Prod_Attr_Relations.Any(c => c.PropertyValueID == item.PropertyValueID
                    && c.Property_ID == ppv.Property_ID))
                    {
                        selected = true;
                    }
                    CheckBox checkBox = listView1.AddItemToGroup(ppv.Property_ID.ToString(), item.PropertyValueName, selected, item.PropertyValueID.ToString(), item);
                    checkBox.CheckStateChanged -= CheckBox_CheckStateChanged;
                    checkBox.CheckStateChanged += CheckBox_CheckStateChanged;
                }
                keys = keys.Trim(',');
                names = names.Trim(',');
                //if (!string.IsNullOrEmpty(names))
                //{
                //    propertyEavList.TryAdd(ppv.Property_ID.ToString(), names);
                //}
            }



            #endregion
        }

        private void CheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            #region 思路 与GetAttrGoups(listView1) 不一样，因为选择状态问题
            CheckBox cb = sender as CheckBox;
            //List<KeyValuePair<long, string[]>> attrGoupsByID = GetAttrGoups(listView1, g => g.GroupID, lvitem => lvitem.Name);
            List<KeyValuePair<long, string[]>> attrGoupsByID = GetAttrGoupsByIDName(listView1, g => g.GroupID, lvitem => lvitem.Text);

            List<string> MixByListView = ArrayCombination.Combination4Table(attrGoupsByID);
#if DEBUG
            //attrGoupsByName = GetAttrGoups(listView1, g => g.GroupID, lvitem => lvitem.Text);
            //List<string> newMixName = ArrayCombination.Combination4Table(attrGoupsByName);
#endif

            //要比较的集合数据
            List<string> MixByTreeGrid = new List<string>();

            #region 获取最新的组合关系。并且保存为一个新的数组与现有的组合关系进行比较 取差集
            List<tb_Prod_Attr_Relation> attr_Relations = GetProdDetailsFromTreeGrid();
            var existDetails = attr_Relations.GroupBy(c => c.ProdDetailID).ToList();

            ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));
            if (EditEntity.tb_Prod_Attr_Relations.Count == 1)
            {
                //如果为单属性时，则直接添加为空
                if (EditEntity.tb_Prod_Attr_Relations[0].Property_ID == null)
                {
                    MixByTreeGrid.Add("");
                }
                else
                {
                    foreach (var item in existDetails)
                    {
                        var array = attr_Relations.Where(c => c.ProdDetailID == item.Key).ToList().Select(c => c.PropertyValueID + "|" + c.tb_prodpropertyvalue.PropertyValueName).ToArray();
                        MixByTreeGrid.Add(string.Join(",", array));
                    }
                }

            }
            else
            {
                foreach (var item in existDetails)
                {
                    var array = attr_Relations.Where(c => c.ProdDetailID == item.Key).ToList().Select(c => c.PropertyValueID + "|" + c.tb_prodpropertyvalue.PropertyValueName).ToArray();
                    MixByTreeGrid.Add(string.Join(",", array));
                }
            }


            #endregion

            if (cb.Tag is tb_ProdPropertyValue ppv)
            {
                tb_ProdProperty tpp = ppv.tb_prodproperty;
                bool existProperty = EditEntity.tb_Prod_Attr_Relations.Any(c => c.Property_ID == ppv.Property_ID);
                //首先判断是选中，还是取消
                if (cb.Checked)
                {
                    //增加时以listview为新数据源，gridview为旧数据源
                    //Intersect 交集，Except 差集，Union 并集 Distinct去重
                    var addItem差集 = MixByListView.Except(MixByTreeGrid).ToList();
                    //先判断差值的唯度。就是属性的个数是否一样
                    if (MixByTreeGrid.Count > 0 && MixByListView.Count > 0)
                    {
                        if (MixByTreeGrid[0].Split(',').Count() != MixByListView[0].Split(',').Count())
                        {
                            #region 全新的属性
                            //实际是按明细来分组。每个组中都要包含所有属性的其中一个值
                            var Details = EditEntity.tb_Prod_Attr_Relations.GroupBy(c => c.ProdDetailID.Value).ToList();

                            foreach (var Detail in Details)
                            {
                                //如果明细中的属性值数量不等于属性数量（有勾选的属性组），则新增属性
                                if (Detail.Count() != listView1.Groups.Where(c => c.Items.Any(c => c.Checked)).ToList().Count)
                                {
                                    #region add
                                    TreeGridNode nodeDetail = treeGridView1.Nodes.FirstOrDefault(c => c.NodeName == Detail.Key.ToString());
                                    nodeDetail.Cells[6].Value = "编辑";
                                    nodeDetail.ImageIndex = 2;//0 加载，1 新增 2 编辑 3 删除 4 下拉
                                    tb_ProdDetail detailData = nodeDetail.Tag is tb_ProdDetail ? nodeDetail.Tag as tb_ProdDetail : new tb_ProdDetail();
                                    detailData.Modified_at = DateTime.Now;
                                    detailData.Modified_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                                    treeGridView1.Refresh();
                                    if (nodeDetail != null)
                                    {
                                        //添加

                                        tb_Prod_Attr_Relation par = new tb_Prod_Attr_Relation();
                                        par.PropertyValueID = ppv.PropertyValueID;
                                        par.Property_ID = ppv.Property_ID;
                                        par.ProdBaseID = EditEntity.ProdBaseID;
                                        par.tb_prodproperty = ppv.tb_prodproperty;
                                        par.tb_prodpropertyvalue = ppv;
                                        par.ProdDetailID = Detail.Key;//等待生成

                                        par.ActionStatus = ActionStatus.新增;
                                        EditEntity.tb_Prod_Attr_Relations.Add(par);
                                        detailData.tb_Prod_Attr_Relations.Add(par);
                                        long rowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                        TreeGridNode subnode = nodeDetail.Nodes.Add(rowid, par.RAR_ID, ppv.tb_prodproperty.PropertyName, ppv.PropertyValueName, "", "", "新增");
                                        subnode.NodeName = rowid.ToString();//标记节点ID， 
                                        subnode.Tag = par;
                                        subnode.ImageIndex = 1;//0 加载，1 新增 2 编辑 3 删除 4 下拉
                                        //加载属性值时，勾选对应的属性值
                                        // listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);
                                    }
                                    nodeDetail.Cells[3].Value = GetPropertiesText(detailData.tb_Prod_Attr_Relations);
                                    #endregion
                                }
                            }


                            #endregion
                        }
                        else
                        {
                            #region  存在属性，则判断属性值是否存在

                            //如果选中的是现有的属性，先判断是否已经存在。
                            foreach (var MItem in addItem差集)
                            {
                                string[] values = MItem.Split(',');

                                if (existProperty)
                                {
                                    #region 属性存在，添加属性值
                                    #region 补充性添加ok

                                    long SkuRowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                    Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                                    string prop = string.Empty;
                                    TreeGridNode node = treeGridView1.Nodes.Add(SkuRowid, 0, "", prop, "等待生成", EditEntity.CNName, "新增");
                                    tb_ProdDetail detail = new tb_ProdDetail();
                                    detail.ProdBaseID = EditEntity.ProdBaseID;
                                    detail.ProdDetailID = SkuRowid; //为了后面可以查询暂时保存行号。实际保存DB前要生新设置为0.
                                    //detail.SKU = prop;为了不浪费  保存时再成生一次

                                    detail.ActionStatus = ActionStatus.新增;
                                    detail.Is_enabled = true;
                                    detail.Is_available = true;
                                    detail.Created_at = DateTime.Now;
                                    detail.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                                    node.NodeName = SkuRowid.ToString();//标记节点ID， 
                                    node.ImageIndex = 1;//0 加载，1 新增 2 编辑 3 删除 4 下拉
                                    node.Tag = detail;
                                    detail.tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>();
                                    node.DefaultCellStyle.Font = boldFont;
                                    //添加具体的属性值
                                    foreach (string value in values)
                                    {
                                        tb_ProdPropertyValue newppv = prodpropValueList.FirstOrDefault(c => c.PropertyValueID.ToString() == value.Split('|')[0]);
                                        if (newppv != null)
                                        {
                                            //1) 添加属性值
                                            #region 添加现在属性的新的属性值组合
                                            tb_Prod_Attr_Relation par = new tb_Prod_Attr_Relation();
                                            par.PropertyValueID = newppv.PropertyValueID;
                                            par.Property_ID = newppv.Property_ID;
                                            par.ProdBaseID = EditEntity.ProdBaseID;
                                            par.tb_prodproperty = newppv.tb_prodproperty;
                                            par.tb_prodpropertyvalue = newppv;
                                            par.ProdDetailID = detail.ProdDetailID;
                                            par.ActionStatus = ActionStatus.新增;
                                            EditEntity.tb_Prod_Attr_Relations.Add(par);

                                            long rowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                            TreeGridNode subnode = node.Nodes.Add(rowid, par.RAR_ID, newppv.tb_prodproperty.PropertyName, newppv.PropertyValueName, "", "", "新增");
                                            subnode.NodeName = rowid.ToString();//标记节点ID， 
                                            subnode.ImageIndex = 1;//0 加载，1 新增 2 编辑 3 删除 4 下拉
                                            subnode.Tag = par;
                                            detail.tb_Prod_Attr_Relations.Add(par);
                                            node.Cells[3].Value = GetPropertiesText(detail.tb_Prod_Attr_Relations);

                                            //加载属性值时，勾选对应的属性值
                                            //listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);

                                            #endregion
                                        }
                                    }
                                    #endregion

                                    //添加产品明细的详情
                                    EditEntity.tb_ProdDetails.Add(detail);
                                    #endregion
                                }
                                else
                                {
                                    //单属性变多属性的情况：将第一个属性值添加到第一个产品详情中。产品详情应该是编辑状态
                                    if (EditEntity.tb_ProdDetails.Count == 1)
                                    {
                                        TreeGridNode nodeDetail = treeGridView1.Nodes.FirstOrDefault(c => c.NodeName == EditEntity.tb_ProdDetails[0].ProdDetailID.ToString());
                                        nodeDetail.Cells[6].Value = "编辑";
                                        nodeDetail.ImageIndex = 2;//0 加载，1 新增 2 编辑 3 删除 4 下拉
                                        tb_ProdDetail detailData = nodeDetail.Tag is tb_ProdDetail ? nodeDetail.Tag as tb_ProdDetail : new tb_ProdDetail();
                                        if (nodeDetail != null)
                                        {
                                            if (detailData.tb_Prod_Attr_Relations.Count == 1 && EditEntity.tb_Prod_Attr_Relations.Count == 1)
                                            {
                                                detailData.tb_Prod_Attr_Relations[0].Property_ID = ppv.Property_ID;
                                                detailData.tb_Prod_Attr_Relations[0].PropertyValueID = ppv.PropertyValueID;
                                                detailData.tb_Prod_Attr_Relations[0].ActionStatus = ActionStatus.修改;
                                                if (detailData.tb_Prod_Attr_Relations[0].tb_prodpropertyvalue == null)
                                                {
                                                    detailData.tb_Prod_Attr_Relations[0].tb_prodpropertyvalue = ppv;
                                                }
                                                #region 更新空值的属性值节点
                                                long rarid = detailData.tb_Prod_Attr_Relations[0].RAR_ID;
                                                TreeGridNode subnode = nodeDetail.Nodes.FirstOrDefault(c => c.NodeName == rarid.ToString());
                                                subnode.ImageIndex = 2;//0 加载，1 新增 2 编辑 3 删除 4 下拉
                                                subnode.Cells[2].Value = tpp.PropertyName;
                                                subnode.Cells[3].Value = ppv.PropertyValueName;
                                                subnode.Cells[6].Value = "编辑";
                                                subnode.Tag = detailData.tb_Prod_Attr_Relations[0];
                                                EditEntity.tb_Prod_Attr_Relations[0].Property_ID = ppv.Property_ID;
                                                EditEntity.tb_Prod_Attr_Relations[0].PropertyValueID = ppv.PropertyValueID;
                                                EditEntity.tb_Prod_Attr_Relations[0].ActionStatus = ActionStatus.修改;
                                                #endregion
                                            }
                                            detailData.ActionStatus = ActionStatus.修改;
                                            detailData.Modified_at = DateTime.Now;
                                            detailData.Modified_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;

                                            nodeDetail.Cells[3].Value = GetPropertiesText(detailData.tb_Prod_Attr_Relations);
                                        }
                                        treeGridView1.Refresh();
                                    }
                                    else
                                    {

                                    }
                                    /*
                                    List<long> PVlist = new List<long>();
                                    var list = EditEntity.tb_Prod_Attr_Relations.Where(c => MItem.Contains(c.PropertyValueID.ToString())).ToList();
                                    //判断是否已经存在，定义一个集合，用于存储属性值，如果正好少一个属性值，则添加就是的勾选的那个值。
                                    foreach (string value in values)
                                    {
                                        if (EditEntity.tb_Prod_Attr_Relations.Any(c => c.PropertyValueID.ToString() == value))
                                        {
                                            PVlist.Add(value.ToLong());
                                        }
                                    }
                                    if (PVlist.Count == values.Length - 1)
                                    {
                                        //添加
                                        //找到哪一行的产品明细的节点
                                        List<long> prodDeitalIds = new List<long>();
                                        foreach (long pvid in PVlist)
                                        {
                                            //将属性值ID转换为产品明细ID。再分组找到对应的明细
                                            EditEntity.tb_Prod_Attr_Relations.Where(c => c.PropertyValueID.ToString() == pvid.ToString()).ToList().ForEach(c => prodDeitalIds.Add(c.ProdDetailID.Value));
                                        }

                                        // 找到 prodDeitalID 中唯一值的数量与 PVlist 长度相同的值
                                        var uniqueProdDeitalIDs = prodDeitalIds
                                            .Distinct() // 移除重复项
                                            .Where(id => prodDeitalIds.Count(x => x == id) == PVlist.Count) // 筛选出每个出现次数与 PVlist 长度相同的 ID
                                            .ToList();

                                        //应该只有一个值
                                        foreach (var prodDeitalId in uniqueProdDeitalIDs)
                                        {
                                            TreeGridNode node = treeGridView1.Nodes.FirstOrDefault(c => c.NodeName == prodDeitalId.ToString());
                                            if (node != null)
                                            {
                                                tb_ProdDetail detailData = node.Tag is tb_ProdDetail ? node.Tag as tb_ProdDetail : new tb_ProdDetail();
                                                //添加
                                                node.Cells[6].Value = "编辑";
                                                node.Cells[3].Value = prop;
                                                node.ImageIndex = 2;
                                                node.NodeName = prodDeitalId.ToString();//标记节点ID，实际就是产品明细ID 等待生成

                                                tb_Prod_Attr_Relation par = new tb_Prod_Attr_Relation();
                                                par.PropertyValueID = ppv.PropertyValueID;
                                                par.Property_ID = ppv.Property_ID;
                                                par.ProdBaseID = EditEntity.ProdBaseID;
                                                par.tb_prodproperty = ppv.tb_prodproperty;
                                                par.ProdDetailID = prodDeitalId;//等待生成
                                                EditEntity.tb_Prod_Attr_Relations.Add(par);
                                                detailData.tb_Prod_Attr_Relations.Add(par);
                                                long rowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                                TreeGridNode subnode = node.Nodes.Add(rowid, par.RAR_ID, ppv.tb_prodproperty.PropertyName, ppv.PropertyValueName, "", "", "新增");
                                                subnode.NodeName = rowid.ToString();//标记节点ID， 
                                                subnode.Tag = par;
                                                subnode.ImageIndex = 1;
                                                //加载属性值时，勾选对应的属性值
                                                // listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);

                                            }
                                        }
                                    }
                                    */
                                }
                            }

                            #endregion
                        }
                    }

                }
                else
                {
                    //减少时以listview为旧数据源，Treegridview为新数据源
                    //新比旧，新在前
                    var reduceItem差集 = MixByTreeGrid.Except(MixByListView).ToList();
                    //
                    #region 选中的取消属性值

                    //先判断差值的唯度。就是属性的个数是否一样
                    if (MixByTreeGrid.Count > 0 && MixByListView.Count > 0)
                    {
                        if (MixByTreeGrid[0].Split(',').Count() != MixByListView[0].Split(',').Count())
                        {
                            var reduceItem差集2 = MixByListView.Except(MixByTreeGrid).ToList();
                            #region 新属性的值取消时  取消的是差集的这个属性值？

                            foreach (var item in reduceItem差集)
                            {
                                List<TreeGridNode> NodetoRemove = new List<TreeGridNode>(); // 存储需要删除的子节点
                                string[] values = item.Split(',');
                                for (int n = 0; n < treeGridView1.Nodes.Count; n++)
                                {
                                    var node = treeGridView1.Nodes[n];
                                    if (node.Tag is tb_ProdDetail pd)
                                    {
                                        List<tb_Prod_Attr_Relation> parDeleteList = new List<tb_Prod_Attr_Relation>();
                                        // 检查并输出结果
                                        foreach (var value in values)
                                        {
                                            var dpar = pd.tb_Prod_Attr_Relations.FirstOrDefault(c => c.PropertyValueID.ToString() == value.Split('|')[0]);
                                            if (dpar != null)
                                            {
                                                parDeleteList.Add(dpar);
                                            }
                                        }

                                        if (parDeleteList.Count == values.Length)
                                        {
                                            List<TreeGridNode> SubNodetoRemove = new List<TreeGridNode>(); // 存储需要删除的子节点

                                            for (int i = 0; i < node.Nodes.Count; i++)
                                            {
                                                var subNode = node.Nodes[i];
                                                if (subNode.Tag is tb_Prod_Attr_Relation par)
                                                {
                                                    //如果是新增的，并且是当前取消的属性值，则直接删除
                                                    if (par.ActionStatus == ActionStatus.新增 && par.PropertyValueID == ppv.PropertyValueID)
                                                    {
                                                        //直接删除
                                                        // 标记为删除
                                                        SubNodetoRemove.Add(subNode);
                                                        pd.tb_Prod_Attr_Relations.Remove(par);
                                                        EditEntity.tb_Prod_Attr_Relations.Remove(par);
                                                    }
                                                }
                                            }
                                            // 删除所有标记为删除的子节点
                                            foreach (var subNode in SubNodetoRemove)
                                            {
                                                node.Nodes.Remove(subNode);
                                                node.Cells[3].Value = node.Cells[3].Value.ToString().Replace("," + ppv.PropertyValueName, "");
                                                node.Cells[3].Value = GetPropertiesText(pd.tb_Prod_Attr_Relations);
                                            }
                                            // 如果父节点也需要删除
                                            if (pd.ActionStatus == ActionStatus.新增 && node.Nodes.Count == 0)
                                            {
                                                EditEntity.tb_ProdDetails.Remove(pd);
                                                NodetoRemove.Add(node);
                                            }
                                        }
                                    }
                                }

                                // 删除所有标记为删除的子节点
                                foreach (var node in NodetoRemove)
                                {
                                    treeGridView1.Nodes.Remove(node);
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            #region 旧属性的值取消时  取消的是差集的这一整行？

                            foreach (var item in reduceItem差集)
                            {
                                List<TreeGridNode> NodetoRemove = new List<TreeGridNode>(); // 存储需要删除的子节点
                                string[] values = item.Split(',');
                                for (int n = 0; n < treeGridView1.Nodes.Count; n++)
                                {
                                    var node = treeGridView1.Nodes[n];
                                    if (node.Tag is tb_ProdDetail pd)
                                    {
                                        List<tb_Prod_Attr_Relation> parDeleteList = new List<tb_Prod_Attr_Relation>();
                                        // 检查并输出结果
                                        foreach (var value in values)
                                        {
                                            var dpar = pd.tb_Prod_Attr_Relations.FirstOrDefault(c => c.PropertyValueID.ToString() == value.Split('|')[0]);
                                            if (dpar != null)
                                            {
                                                parDeleteList.Add(dpar);
                                            }
                                        }

                                        if (parDeleteList.Count == values.Length)
                                        {
                                            List<TreeGridNode> SubNodetoRemove = new List<TreeGridNode>(); // 存储需要删除的子节点

                                            for (int i = 0; i < node.Nodes.Count; i++)
                                            {
                                                var subNode = node.Nodes[i];
                                                if (subNode.Tag is tb_Prod_Attr_Relation par)
                                                {
                                                    if (par.ActionStatus == ActionStatus.新增)
                                                    {
                                                        //直接删除
                                                        // 标记为删除
                                                        SubNodetoRemove.Add(subNode);
                                                        pd.tb_Prod_Attr_Relations.Remove(par);
                                                        EditEntity.tb_Prod_Attr_Relations.Remove(par);
                                                    }
                                                }
                                            }
                                            // 删除所有标记为删除的子节点
                                            foreach (var subNode in SubNodetoRemove)
                                            {
                                                node.Nodes.Remove(subNode);
                                                node.Cells[3].Value = GetPropertiesText(pd.tb_Prod_Attr_Relations);
                                            }

                                            // 如果父节点也需要删除
                                            if (pd.ActionStatus == ActionStatus.新增 && node.Nodes.Count == 0)
                                            {
                                                EditEntity.tb_ProdDetails.Remove(pd);
                                                NodetoRemove.Add(node);
                                            }
                                        }
                                    }
                                }

                                // 删除所有标记为删除的子节点 //0 加载，1 新增 2 编辑 3 删除 4 下拉
                                foreach (var node in NodetoRemove)
                                {
                                    treeGridView1.Nodes.Remove(node);
                                }
                            }
                            #endregion
                        }
                    }
                    /*

                    //当前属性值所在属性已经存在于实际数据时,标记删除， 如果是新增加临时性的直接删除
                    foreach (var item in reduceItem差集2)
                    {
                        List<TreeGridNode> NodetoRemove = new List<TreeGridNode>(); // 存储需要删除的子节点
                        string[] values = item.Split(',');
                        for (int n = 0; n < treeGridView1.Nodes.Count; n++)
                        {
                            var node = treeGridView1.Nodes[n];
                            if (node.Tag is tb_ProdDetail pd)
                            {
                                List<tb_Prod_Attr_Relation> parDeleteList = new List<tb_Prod_Attr_Relation>();
                                // 检查并输出结果
                                foreach (var value in values)
                                {
                                    var dpar = pd.tb_Prod_Attr_Relations.FirstOrDefault(c => c.PropertyValueID.ToString() == value);
                                    if (dpar != null)
                                    {
                                        parDeleteList.Add(dpar);
                                    }
                                }

                                if (parDeleteList.Count == values.Length)
                                {
                                    List<TreeGridNode> SubNodetoRemove = new List<TreeGridNode>(); // 存储需要删除的子节点

                                    for (int i = 0; i < node.Nodes.Count; i++)
                                    {
                                        var subNode = node.Nodes[i];
                                        if (subNode.Tag is tb_Prod_Attr_Relation par)
                                        {
                                            if (par.ActionStatus == ActionStatus.新增)
                                            {
                                                //直接删除
                                                // 标记为删除
                                                SubNodetoRemove.Add(subNode);
                                                pd.tb_Prod_Attr_Relations.Remove(par);
                                                EditEntity.tb_Prod_Attr_Relations.Remove(par);
                                            }
                                        }
                                    }
                                    // 删除所有标记为删除的子节点
                                    foreach (var subNode in SubNodetoRemove)
                                    {
                                        node.Nodes.Remove(subNode);
                                    }

                                    // 如果父节点也需要删除
                                    if (pd.ActionStatus == ActionStatus.新增 && node.Nodes.Count == 0)
                                    {
                                        EditEntity.tb_ProdDetails.Remove(pd);
                                        NodetoRemove.Add(node);
                                    }
                                }
                            }
                        }

                        // 删除所有标记为删除的子节点
                        foreach (var node in NodetoRemove)
                        {
                            treeGridView1.Nodes.Remove(node);
                        }
                    }

                    return;

                    //也分为两种情况，当前属性值所在属性已经存在于实际数据时,标记删除， 如果是新增加临时性的直接删除
                    if (existProperty)
                    {

                    }
                    else
                    {
                        // 以old为标准，恢复原来的
                        foreach (var item in reduceItem差集2)
                        {
                            string[] values = item.Split(',');
                            for (int n = 0; n < treeGridView1.Nodes.Count; n++)
                            {
                                var node = treeGridView1.Nodes[n];
                                if (node.Tag is tb_ProdDetail pd)
                                {
                                    List<tb_Prod_Attr_Relation> parDeleteList = new List<tb_Prod_Attr_Relation>();
                                    // 检查并输出结果
                                    foreach (var value in values)
                                    {
                                        var dpar = pd.tb_Prod_Attr_Relations.FirstOrDefault(c => c.PropertyValueID.ToString() == value);
                                        if (dpar != null)
                                        {
                                            parDeleteList.Add(dpar);
                                        }
                                    }

                                    if (parDeleteList.Count == values.Length)
                                    {
                                        List<TreeGridNode> toRemove = new List<TreeGridNode>(); // 存储需要删除的子节点

                                        for (int i = 0; i < node.Nodes.Count; i++)
                                        {
                                            var subNode = node.Nodes[i];
                                            if (subNode.Tag is tb_Prod_Attr_Relation par)
                                            {
                                                if (par.ActionStatus == ActionStatus.新增)
                                                {
                                                    //直接删除
                                                    // 标记为删除
                                                    toRemove.Add(subNode);
                                                    pd.tb_Prod_Attr_Relations.Remove(par);
                                                    EditEntity.tb_Prod_Attr_Relations.Remove(par);
                                                }
                                            }
                                        }
                                        // 删除所有标记为删除的子节点
                                        foreach (var subNode in toRemove)
                                        {
                                            node.Nodes.Remove(subNode);
                                        }

                                        // 如果父节点也需要删除
                                        if (pd.ActionStatus == ActionStatus.新增 && node.Nodes.Count == 0)
                                        {
                                            EditEntity.tb_ProdDetails.Remove(pd);
                                            treeGridView1.Nodes.Remove(node);
                                        }

                                    }
                                    else
                                    {
                                        continue;
                                    }




                                }



                            }
                        }
                    }
                    */
                    #endregion
                }
            }
            #endregion
        }


        private string GetPropertiesText(List<tb_Prod_Attr_Relation> list)
        {
            string prop = string.Empty;
            foreach (var item in list)
            {
                prop += item.tb_prodpropertyvalue.PropertyValueName + ",";
            }
            prop = prop.TrimEnd(',');
            return prop;
        }


        /// <summary>
        /// 从TreeGrid中获取产品属性值等各种关系信息
        /// </summary>
        /// <returns></returns>
        private List<tb_Prod_Attr_Relation> GetProdDetailsFromTreeGrid()
        {
            List<tb_Prod_Attr_Relation> prodDetails_relation = new List<tb_Prod_Attr_Relation>();
            foreach (TreeGridNode item in treeGridView1.Nodes)
            {
                if (item.HasChildren)
                {
                    foreach (var subNode in item.Nodes)
                    {
                        prodDetails_relation.Add(subNode.Tag as tb_Prod_Attr_Relation);
                    }
                }
            }
            // 按照 PropertyValueID 进行排序 方便后面比较
            prodDetails_relation = prodDetails_relation.OrderBy(p => p.PropertyValueID).ToList();

            return prodDetails_relation;
        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            //attrGoupsByName.Clear();
            contextMenuStrip1.Items.Clear();
            bindingSourceSKU明细.Clear();


            #region 单属性

            ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));// EnumHelper.GetEnumByString<ProductAttributeType>(cmbPropertyType.SelectedItem.ToString());
            switch (pt)
            {
                case ProductAttributeType.单属性:
                    if (cmb属性.SelectedItem == null)
                    {
                        //添加单属性时的SKU
                        #region
                        btnAddProperty.Enabled = false;

                        //UCSKUlist ucskulist = new UCSKUlist();

                        //tableLayoutPanel1.Controls.Remove(ucskulist);
                        #endregion
                    }
                    break;
                case ProductAttributeType.可配置多属性:
                    btnAddProperty.Enabled = true;
                    break;
                case ProductAttributeType.捆绑:
                    break;
                case ProductAttributeType.虚拟:
                    break;
                default:
                    break;
            }

            // tableLayoutPanel1.Controls.Remove(ucskulist);

            #endregion
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!dataGridViewProd.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = dataGridViewProd.Columns[e.ColumnIndex].Name;

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (dataGridViewProd.Columns[e.ColumnIndex].Name == "Images")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    if (image != null)
                    {
                        //缩略图 这里用缓存 ?
                        //  Image thumbnailthumbnail = this.thumbnail(image, 100, 100);
                        // e.Value = thumbnailthumbnail;
                    }

                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }


        private void dataGridViewProd_DoubleClick(object sender, EventArgs e)
        {
            LoadProdDetail();
        }

        //加载产品属性时也要排序，按后面添加多属性一样的规则，这样才正确的比较重复等
        private void LoadProdDetail()
        {
            if (dataGridViewProd.CurrentRow != null)
            {
                EditEntity = null;
                oldOjb = null;
                EditEntity = dataGridViewProd.CurrentRow.DataBoundItem as tb_Prod;
                //oldOjb = CloneHelper.DeepCloneObject<tb_Prod>(EditEntity);
                oldOjb = EditEntity.DeepCloneByjson();
                if (EditEntity != null)
                {
                    if (EditEntity.tb_ProdDetails != null && EditEntity.tb_ProdDetails.Count > 0)
                    {
                        listView1.Clear();
                        //attrGoupsByName.Clear();

                        //加载属性
                        //去掉临时添加的关系数据，因为要重新加载
                        EditEntity.tb_Prod_Attr_Relations.RemoveWhere(c => c.RAR_ID == 0);
                        LoadTreeGridItems(EditEntity);
                        //attrGoupsByName = GetAttrGoups(listView1, g => g.GroupID, lvitem => lvitem.Text);

                        //加载属性类型，再加载属性及对应的值
                        cmbPropertyType.SelectedValue = EditEntity.PropertyType;

                        //加载动态属性区域

                        //如果是单属性则属性值不能为空
                        var propGroup = EditEntity.tb_Prod_Attr_Relations.Where(c => c.Property_ID.HasValue).GroupBy(c => c.Property_ID.Value);
                        foreach (var item in propGroup)
                        {
                            var prop = prodpropList.FirstOrDefault(c => c.Property_ID == item.Key);
                            if (prop != null)
                            {
                                AddProperty(prop);
                            }
                        }

                        //加载属性值后  不可以再修改选中状态。
                        //循环所有属性，如果有值则选中并且不可以修改
                        if (EditEntity.tb_Prod_Attr_Relations != null && EditEntity.tb_Prod_Attr_Relations.Count > 1)//要多属性的时候才需要
                        {
                            var propertys = EditEntity.tb_Prod_Attr_Relations.Select(c => c.Property_ID.Value).Distinct().ToList();
                            foreach (var item in propertys)
                            {
                                var pvs = EditEntity.tb_Prod_Attr_Relations.Where(c => c.Property_ID.Value == item).Select(c => c.PropertyValueID.Value).Distinct().ToList();
                                foreach (var pv in pvs)
                                {
                                    var group = listView1.Groups.FirstOrDefault(c => c.GroupID == item.ToString());
                                    if (group != null)
                                    {
                                        //group.Items.FirstOrDefault(c => c.Name == pv.ToString()).Checked = true;
                                        group.Items.FirstOrDefault(c => c.Name == pv.ToString()).Enabled = false;
                                    }

                                }
                            }
                        }

                        listView1.UpdateUI();
                    }

                }
            }
        }


        /// <summary>
        /// 加载树形控件中的节点
        /// </summary>
        /// <param name="prod"></param>
        private void LoadTreeGridItems(tb_Prod prod)
        {
            //清空树形控件中的节点和行
            treeGridView1.Nodes.Clear();
            treeGridView1.Rows.Clear();
            //创建字体对象，设置字体加粗
            Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);
            //遍历产品明细集合
            foreach (var item in prod.tb_ProdDetails)
            {


                // 按照 PropertyValueID 进行排序 方便后面比较
                List<tb_Prod_Attr_Relation> ProdAttrRelations = item.tb_Prod_Attr_Relations.OrderBy(p => p.PropertyValueID).ToList();


                //根据产品明细ID查询View_ProdDetail视图
                var viewProdDetail = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>()
                                .Where(c => c.ProdDetailID == item.ProdDetailID).Single();
                //如果查询到视图，则添加节点
                if (viewProdDetail != null)
                {
                    string DisplayPropText = string.Empty;
                    if (viewProdDetail.prop != null)
                    {
                        DisplayPropText = viewProdDetail.prop;
                        var array = ProdAttrRelations.Where(c => c.ProdDetailID == item.ProdDetailID).ToList().Select(c => c.tb_prodpropertyvalue.PropertyValueName).ToArray();
                        DisplayPropText = string.Join(",", array);
                    }
                    TreeGridNode node = treeGridView1.Nodes.Add(item.ProdDetailID, item.ProdDetailID, "", DisplayPropText, item.SKU, viewProdDetail.CNName, "加载");
                    //标记节点ID，实际就是产品明细ID
                    node.NodeName = item.ProdDetailID.ToString();
                    node.Tag = item;
                    node.ImageIndex = 0;
                    //遍历产品属性关系集合
                    foreach (var par in ProdAttrRelations)
                    {
                        node.DefaultCellStyle.Font = boldFont;
                        //如果属性值不为空，则添加子节点,
                        if (par.tb_prodpropertyvalue != null && node.Index != -1)
                        {
                            TreeGridNode subnode = node.Nodes.Add(par.RAR_ID, par.RAR_ID, par.tb_prodpropertyvalue.tb_prodproperty.PropertyName, par.tb_prodpropertyvalue.PropertyValueName, "", "", "加载");
                            //标记节点ID，实际就是产品属性关系ID
                            subnode.NodeName = par.RAR_ID.ToString();
                            subnode.Tag = par;

                            //加载属性值时，勾选对应的属性值
                            //listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);
                        }
                        else
                        {
                            //单属性时 属性值给的是空。
                            TreeGridNode subnode = node.Nodes.Add(par.RAR_ID, par.RAR_ID, "", "", "", "", "加载");
                            //标记节点ID，实际就是产品属性关系ID
                            subnode.NodeName = par.RAR_ID.ToString();
                            subnode.Tag = par;
                        }
                        node.ImageIndex = 0;
                    }
                }

            }





        }




        private void cmbPropertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbPropertyType.Tag == null)
            //{
            //    return;
            //}

            if (cmbPropertyType.SelectedItem != null)
            {
                object selectObj = cmbPropertyType.SelectedValue;
                if (selectObj.GetPropertyValue("PropertyType").ToInt() != -1)
                {
                    ProductAttributeType pt = (ProductAttributeType)cmbPropertyType.SelectedValue;
                    switch (pt)
                    {
                        case ProductAttributeType.单属性:

                            ControlBtn(pt);
                            #region 新增修改式
                            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ProdBaseID == 0)
                            {
                                bindingSourceSKU明细.Clear();

                                if (dataGridViewProd.Rows.Count == 0)
                                {
                                    //BindToSkulistGrid(new List<Eav_ProdDetails>());
                                }
                                if (EditEntity.ActionStatus != ActionStatus.加载)
                                {
                                    Eav_ProdDetails ppg = new Eav_ProdDetails();
                                    ppg.GroupName = "";
                                    ppg.SKU = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.SKU_No);
                                    bindingSourceSKU明细.Add(ppg);
                                }

                            }
                            #endregion

                            if (dataGridViewProd.Columns.Contains("GroupName"))
                            {
                                dataGridViewProd.Columns["GroupName"].Visible = false;
                            }

                            break;
                        case ProductAttributeType.可配置多属性:
                            ControlBtn(pt);
                            cmb属性.Enabled = true;
                            btnAddProperty.Enabled = true;
                            bindingSourceSKU明细.Clear();

                            //绑定对应的选项及其值
                            DataBindingHelper.InitDataToCmb<tb_ProdProperty>(p => p.Property_ID, t => t.PropertyName, cmb属性);
                            cmb属性.SelectedIndex = -1;



                            break;
                        case ProductAttributeType.捆绑:
                            break;
                        case ProductAttributeType.虚拟:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        tb_Prod oldOjb = null;
        private async void btnOk_Click(object sender, EventArgs e)
        {


            List<string> MixByTreeGrid = new List<string>();



            #region 获取最新的组合关系。并且保存为一个新的数组与现有的组合关系进行比较 取差集
            List<tb_Prod_Attr_Relation> attr_Relations = GetProdDetailsFromTreeGrid();
            var existDetails = attr_Relations.GroupBy(c => c.ProdDetailID).ToList();
            foreach (var item in existDetails)
            {
                var array = attr_Relations.Where(c => c.ProdDetailID == item.Key).ToList().Select(c => c.PropertyValueID + "|" + c.tb_prodpropertyvalue.PropertyValueName).ToArray();
                MixByTreeGrid.Add(string.Join(",", array));

            }
            #endregion
            if (MixByTreeGrid.Count > 0)
            {
                //保存的关系中的唯度。每个产品详情的属性个数要一样。如果不一样。则提示不能保存。
                // 获取第一个元素的数据维度作为参考
                int referenceDimension = MixByTreeGrid[0].Split(',').Length;
                // 检查列表中每个元素的数据维度是否与参考维度相等
                if (!MixByTreeGrid.All(item => item.Split(',').Length == referenceDimension))
                {
                    //数据维度不一致,提示不能保存
                    MessageBox.Show("产品明细中，属性维度不一致,保存失败，请重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }


            #region 判断是否有重复的属性值。将属性值添加到列表，按一定规则排序，然后判断是否有重复

            List<string> DuplicateAttributes = new List<string>();
            string sortedDaString = string.Empty;
            foreach (var item in existDetails)
            {
                // da 是一个 string 数组
                string[] da = attr_Relations
                .Where(c => c.ProdDetailID == item.Key)
                .ToList()
                .Select(c => c.tb_prodpropertyvalue.PropertyValueName)
                .ToArray();

                // 将 da 转换为排序后的列表
                List<string> sortedDa = da.OrderBy(x => x).ToList();

                // 将排序后的列表转换为字符串
                sortedDaString = string.Join(", ", sortedDa);

                // 添加到 DuplicateAttributes 集合中
                DuplicateAttributes.Add(sortedDaString);
            }

            // 找出 DuplicateAttributes 中的重复值
            var duplicates = DuplicateAttributes
                .GroupBy(s => s)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Count > 0)
            {
                // 输出重复的值
                foreach (var dup in duplicates)
                {
                    MainForm.Instance.PrintInfoLog("属性值重复:" + dup);
                }
                MessageBox.Show("产品明细中，属性值重复,保存失败，请重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            #endregion



            //https://github.com/ValeraT1982/ObjectsComparer
            var _comparer = new ObjectsComparer.Comparer<tb_Prod>(
              new ComparisonSettings
              {
                  //Null and empty error lists are equal
                  EmptyAndNullEnumerablesEqual = true
              });

            _comparer.IgnoreMember("Created_by");//
            _comparer.IgnoreMember("Created_at");//
            _comparer.IgnoreMember("Modified_at");//
            _comparer.IgnoreMember("Modified_by");//
            if (EditEntity.tb_ProdDetails == null)
            {
                EditEntity.tb_ProdDetails = new List<tb_ProdDetail>();
            }

            //var detail = from p in EditEntity.tb_Prod_Attr_Relations select p;
            _comparer.AddComparerOverride(() => new tb_Prod().tb_Prod_Attr_Relations, DoNotCompareValueComparer.Instance);
            IEnumerable<Difference> differences;
            var isEqual = _comparer.Compare(oldOjb, EditEntity, out differences);
            if (isEqual)
            {
                MessageBox.Show("数据没有任何变化，不需要保存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                EditEntity.ActionStatus = ActionStatus.修改;
            }

            //如果是单属性，则关系中只会有一条数据。并且属性值为空。
            ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));
            if (pt == ProductAttributeType.单属性 && EditEntity.tb_Prod_Attr_Relations.Count > 1)
            {
                MessageBox.Show("单属性的产品时，属性关系不能超过一条。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                EditEntity.PropertyType = (int)ProductAttributeType.可配置多属性;
            }
            //生成属性后 所有的属性值都按一个规则排序，再比较是否有相同的。如果有则提示不能保存。



            //如果SKU为空。则是新的数据 detailid=0;
            foreach (var item in EditEntity.tb_ProdDetails)
            {
                //判断明细是不是修改       
                //如果关系数据中关系ID大于0则是原来的。与包含=0的总数比较。如果不一致。则说明有修改
                if (item.tb_Prod_Attr_Relations.Count != item.tb_Prod_Attr_Relations.Where(c => c.RAR_ID > 0).Count())
                {
                    if (item.ActionStatus != ActionStatus.新增)
                    {
                        item.ActionStatus = ActionStatus.修改;
                    }

                }
                if (item.SKU.IsNullOrEmpty())
                {
                    item.SKU = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.SKU_No);
                    if (item.ActionStatus == ActionStatus.新增)
                    {
                        item.ProdDetailID = 0;
                    }
                }
            }

            tb_ProdController<tb_Prod> pctr = Startup.GetFromFac<tb_ProdController<tb_Prod>>();
            ReturnResults<tb_Prod> rr = new ReturnResults<tb_Prod>();
            rr = await pctr.SaveOrUpdateAsync(EditEntity);
            if (rr.Succeeded)
            {
                MainForm.Instance.uclog.AddLog("保存成功");
                this.Exit(this);
            }
            else
            {
                MainForm.Instance.uclog.AddLog($"保存失败:{rr.ErrorMsg}");
            }

        }

        protected virtual void Exit(object thisform)
        {
            CloseTheForm(thisform);
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
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
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


        private void treeGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //这个特殊这里是第一行的行号
            if (e.RowIndex == 0 && e.ColumnIndex == -1)
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

            if (e.RowIndex > 0)
            {
                #region 画行号
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    DataGridViewPaintParts paintParts =
                        e.PaintParts & ~DataGridViewPaintParts.Focus;

                    e.Paint(e.ClipBounds, paintParts);
                    e.Handled = true;
                }

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

                #endregion
            }

            ////画总行数行号
            //if (e.ColumnIndex < 0 && e.RowIndex < 0)
            //{
            //    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
            //    Rectangle indexRect = e.CellBounds;
            //    indexRect.Inflate(-2, -2);

            //    TextRenderer.DrawText(e.Graphics,
            //        (treeGridView1.Nodes.Count + "#").ToString(),
            //        e.CellStyle.Font,
            //        indexRect,
            //        e.CellStyle.ForeColor,
            //        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
            //    e.Handled = true;
            //}
        }


        private void 删除属性值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该功能暂时没有实现。");
        }

        private async void 删除SKU明细toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeGridView1.CurrentCell != null)
            {
                if (dataGridViewProd.CurrentRow != null)
                {
                    EditEntity = null;
                    oldOjb = null;
                    if (dataGridViewProd.CurrentRow.DataBoundItem is tb_Prod Prod)
                    {
                        var detailID = treeGridView1.CurrentRow.NodeName;
                        var detail = Prod.tb_ProdDetails.FirstOrDefault(c => c.ProdDetailID.ToString() == detailID);
                        if (detail != null)
                        {
                            if (detail.ProdDetailID > 0)
                            {
                                //删除
                                if (MessageBox.Show("确定删除该SKU明细吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    bool counter = await MainForm.Instance.AppContext.Db.DeleteNav<tb_ProdDetail>(detail)
                                        .Include(c => c.tb_Prod_Attr_Relations)
                                        .ExecuteCommandAsync(); ;
                                    if (counter)
                                    {
                                        //如果有外键引用了。会出错。这里删除没有问题。
                                        //刷新
                                        LoadProdDetail();
                                        MainForm.Instance.ShowStatusText("删除SKU明细成功。");
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void toolStripMI全部展开_Click(object sender, EventArgs e)
        {
            foreach (var node in treeGridView1.Nodes)
            {
                node.Expand();
            }
        }

        private void toolStripMI全部折叠_Click(object sender, EventArgs e)
        {
            foreach (var node in treeGridView1.Nodes)
            {
                node.Collapse();
            }
        }
    }

    internal class AttachmentColumnHeader : DataGridViewColumnHeaderCell
    {
        public Image _image;
        public AttachmentColumnHeader(Image img)
            : base()
        {
            this._image = img;
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            graphics.DrawImage(_image, cellBounds.X + 4, cellBounds.Y + 2);
        }
        protected override object GetValue(int rowIndex)
        {
            return null;
        }
    }

}
