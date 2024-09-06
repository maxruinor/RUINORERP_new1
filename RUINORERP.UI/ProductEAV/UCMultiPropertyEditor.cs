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
using NPOI.SS.Formula.Functions;
using System.Security.Cryptography;
using SiteRules.aliexpress;
using static OfficeOpenXml.ExcelErrorValue;
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
            this.imageStrip.ImageSize = new Size(16, 16);
            var image = Properties.Resources.WF_down.ToBitmap();
            this.imageStrip.Images.AddStrip(image);

            treeGridView1.ImageList = imageStrip;
            // attachment header cell
            this.ImgCol.HeaderCell = new AttachmentColumnHeader(imageStrip.Images[0]);
            this.ImgCol.Visible = false;//隐藏图片列 关系ID列

            EnumBindingHelper bindingHelper = new EnumBindingHelper();
            //https://www.cnblogs.com/cdaniu/p/15236857.html
            //加载枚举，并且可以过虑不需要的项
            List<int> exclude = new List<int>();
            exclude.Add((int)ProductAttributeType.虚拟);
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

        private ConcurrentDictionary<string, string> propertyEavList = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 保存属性及对应选中的值
        /// </summary>
        public ConcurrentDictionary<string, string> PropertyEavList { get => propertyEavList; set => propertyEavList = value; }
        /// <summary>
        /// 属性原始组合队列
        /// </summary>
        public List<KeyValuePair<long, string[]>> AttrGoups { get => _attrGoups; set => _attrGoups = value; }

        private List<KeyValuePair<long, string[]>> _attrGoups = new List<KeyValuePair<long, string[]>>();


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
            AttrGoups = GetAttrGoupsByName(listView1);
            CreateSKUList();
        }


        /// <summary>
        /// 将listview的UI值转为属性组
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        private List<KeyValuePair<long, string[]>> GetAttrGoupsByName(TileListView lv)
        {
            List<KeyValuePair<long, string[]>> attrGoups = new List<KeyValuePair<long, string[]>>();
            foreach (TileGroup g in lv.Groups)
            {
                tb_ProdProperty pp = g.BusinessData as tb_ProdProperty;
                long key = pp.Property_ID;
                string values = string.Empty;
                foreach (CheckBox lvitem in g.Items)
                {
                    if (lvitem.Checked)
                    {
                        values += lvitem.Text + ",";
                    }

                }
                values = values.TrimEnd(',');
                if (values.Trim().Length == 0)
                {
                    continue;
                }
                KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(key, values.Split(','));
                List<KeyValuePair<long, string[]>> exitkvps = attrGoups.Where(t => t.Key == key).ToList();
                if (exitkvps.Count == 0)
                {
                    attrGoups.Add(kvp);
                }
                else
                {

                    KeyValuePair<long, string[]> kvpf = exitkvps.FirstOrDefault();
                    attrGoups.Remove(kvpf);
                    attrGoups.Add(kvp);

                }
            }
            return attrGoups;
        }

        /// <summary>
        /// 将listview的UI值转为属性组
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        private List<KeyValuePair<long, string[]>> GetAttrGoupsByID(TileListView lv)
        {
            List<KeyValuePair<long, string[]>> attrGoups = new List<KeyValuePair<long, string[]>>();
            foreach (TileGroup g in lv.Groups)
            {
                tb_ProdProperty pp = g.BusinessData as tb_ProdProperty;
                long key = pp.Property_ID;
                string values = string.Empty;
                foreach (CheckBox lvitem in g.Items)
                {
                    if (lvitem.Checked)
                    {
                        values += lvitem.Name + ",";
                    }

                }
                values = values.TrimEnd(',');
                if (values.Trim().Length == 0)
                {
                    continue;
                }
                KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(key, values.Split(','));
                List<KeyValuePair<long, string[]>> exitkvps = attrGoups.Where(t => t.Key == key).ToList();
                if (exitkvps.Count == 0)
                {
                    attrGoups.Add(kvp);
                }
                else
                {

                    KeyValuePair<long, string[]> kvpf = exitkvps.FirstOrDefault();
                    attrGoups.Remove(kvpf);
                    attrGoups.Add(kvp);

                }
            }
            return attrGoups;
        }

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
            List<string> newMix = ArrayCombination.Combination4Table(AttrGoups);
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


        private void ControlBtn(ProductAttributeType pat)
        {
            switch (pat)
            {
                case ProductAttributeType.单属性:
                    cmb属性.Enabled = false;
                    btnAddProperty.Enabled = false;
                    btnClear.Enabled = false;


                    break;
                case ProductAttributeType.可配置多属性:
                    cmb属性.Enabled = true;
                    btnAddProperty.Enabled = true;
                    btnClear.Enabled = true;


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
                if (!string.IsNullOrEmpty(names))
                {
                    propertyEavList.TryAdd(ppv.Property_ID.ToString(), names);
                }
            }



            #endregion
        }

        private void CheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            #region 思路 与GetAttrGoups(listView1) 不一样，因为选择状态问题
            CheckBox cb = sender as CheckBox;

            if (cb.Tag is tb_ProdPropertyValue ppv)
            {
                tb_ProdProperty tpp = ppv.tb_prodproperty;
                bool existProperty = EditEntity.tb_Prod_Attr_Relations.Any(c => c.Property_ID == ppv.Property_ID);
                //bool existPropertyValue = EditEntity.tb_Prod_Attr_Relations.Any(c => c.PropertyValueID == ppv.PropertyValueID);
                //首先判断是选中，还是取消
                if (cb.Checked)
                {
                    if (!existProperty)
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
                                nodeDetail.Cells[3].Value = nodeDetail.Cells[3].Value+","+ ppv.PropertyValueName;
                                if (nodeDetail != null)
                                {
                                    //添加

                                    tb_Prod_Attr_Relation par = new tb_Prod_Attr_Relation();
                                    par.PropertyValueID = ppv.PropertyValueID;
                                    par.Property_ID = ppv.Property_ID;
                                    par.ProdBaseID = EditEntity.ProdBaseID;
                                    par.tb_prodproperty = ppv.tb_prodproperty;
                                    par.ProdDetailID = Detail.Key;//等待生成
                                    EditEntity.tb_Prod_Attr_Relations.Add(par);
                                    nodeDetail.NodeName = "0";//标记节点ID，实际就是产品明细ID 等待生成
                                    nodeDetail.ImageIndex = 0;
                                    long rowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                    TreeGridNode subnode = nodeDetail.Nodes.Add(rowid, par.RAR_ID, ppv.tb_prodproperty.PropertyName, ppv.PropertyValueName, "", "", "新增");
                                    subnode.NodeName = rowid.ToString();//标记节点ID， 
                                    subnode.Tag = par;

                                    //加载属性值时，勾选对应的属性值
                                    // listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);

                                }

                                #endregion
                            }
                        }


                        #endregion
                        return;
                    }
                    else
                    {

                    }


                    // 1) 如果选中的是现有的属性，先判断是否已经存在，如果属性存在。则只是要添加属性值
                    //如何处理呢？ 是不是不管原来是什么，先按全新生成 排列组合。存在的属性的添加属性值。没有有新增。
                    AttrGoups = GetAttrGoupsByName(listView1);
                    List<string> newMixName = ArrayCombination.Combination4Table(AttrGoups);
                    List<KeyValuePair<long, string[]>> pairs = GetAttrGoupsByID(listView1);
                    List<string> newMix = ArrayCombination.Combination4Table(pairs);
                    //如果选中的是现有的属性，先判断是否已经存在。
                    foreach (var MItem in newMix)
                    {
                        string str = MItem;
                        string[] values = str.Split(',');
                        //组合成显示用的属性值串
                        string prop = string.Empty;
                        foreach (string value in values)
                        {
                            prop += prodpropValueList.FirstOrDefault(c => c.PropertyValueID.ToString() == value).PropertyValueName + ",";
                        }
                        prop = prop.TrimEnd(',');
                        List<long> PVlist = new List<long>();
                        if (existProperty)
                        {
                            #region 属性存在，添加属性值

                            bool isExist = true;
                            //判断是否已经存在，定义一个集合，用于存储属性值，如果正好少一个属性值，则添加就是的勾选的那个值。

                            var list = EditEntity.tb_Prod_Attr_Relations.Where(c => str.Contains(c.PropertyValueID.ToString())).ToList();

                            foreach (string value in values)
                            {
                                //if (EditEntity.tb_Prod_Attr_Relations.Any(c => c.PropertyValueID.ToString() == value && c.RAR_ID > 0))

                                PVlist = EditEntity.tb_Prod_Attr_Relations.Where(c => c.PropertyValueID.ToString() == value).ToList().Select(c => c.ProdDetailID.Value).ToList();
                                //如果值存在的个数等于属性的个数，则说明已经存在，则不添加
                                if (PVlist.Count == values.Length)
                                {
                                    isExist = true;
                                }
                                else
                                {
                                    isExist = false;
                                }
                            }
                            if (!isExist)
                            {
                                #region 补充性添加

                                long SkuRowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                                TreeGridNode node = treeGridView1.Nodes.Add(SkuRowid, 0, "", prop, "等待生成", EditEntity.CNName, "新增");
                                node.NodeName = SkuRowid.ToString();//标记节点ID， 
                                node.DefaultCellStyle.Font = boldFont;
                                //添加具体的属性值
                                foreach (string value in values)
                                {
                                    tb_ProdPropertyValue newppv = prodpropValueList.FirstOrDefault(c => c.PropertyValueID.ToString() == value);
                                    if (newppv != null)
                                    {
                                        //1) 添加属性值
                                        #region 添加现在属性的新的属性值组合
                                        tb_Prod_Attr_Relation par = new tb_Prod_Attr_Relation();
                                        par.PropertyValueID = newppv.PropertyValueID;
                                        par.Property_ID = newppv.Property_ID;
                                        par.ProdBaseID = EditEntity.ProdBaseID;
                                        par.tb_prodproperty = newppv.tb_prodproperty;
                                        par.ProdDetailID = 0;//等待生成
                                        EditEntity.tb_Prod_Attr_Relations.Add(par);
                                        node.NodeName = "0";//标记节点ID，实际就是产品明细ID 等待生成
                                        node.ImageIndex = 0;
                                        long rowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                        TreeGridNode subnode = node.Nodes.Add(rowid, par.RAR_ID, newppv.tb_prodproperty.PropertyName, newppv.PropertyValueName, "", "", "新增");
                                        subnode.NodeName = rowid.ToString();//标记节点ID， 

                                        //加载属性值时，勾选对应的属性值
                                        //listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);

                                        node.ImageIndex = 1;

                                        #endregion
                                    }

                                }
                                #endregion
                            }






                            #endregion
                        }
                        else
                        {
                            var list = EditEntity.tb_Prod_Attr_Relations.Where(c => str.Contains(c.PropertyValueID.ToString())).ToList();



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
                                    node.Cells[6].Value = "编辑";
                                    node.Cells[3].Value = prop;
                                    if (node != null)
                                    {
                                        //添加

                                        tb_Prod_Attr_Relation par = new tb_Prod_Attr_Relation();
                                        par.PropertyValueID = ppv.PropertyValueID;
                                        par.Property_ID = ppv.Property_ID;
                                        par.ProdBaseID = EditEntity.ProdBaseID;
                                        par.tb_prodproperty = ppv.tb_prodproperty;
                                        par.ProdDetailID = prodDeitalId;//等待生成
                                        EditEntity.tb_Prod_Attr_Relations.Add(par);
                                        node.NodeName = "0";//标记节点ID，实际就是产品明细ID 等待生成
                                        node.ImageIndex = 0;
                                        long rowid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                                        TreeGridNode subnode = node.Nodes.Add(rowid, par.RAR_ID, ppv.tb_prodproperty.PropertyName, ppv.PropertyValueName, "", "", "新增");
                                        subnode.NodeName = rowid.ToString();//标记节点ID， 
                                        subnode.Tag = par;

                                        //加载属性值时，勾选对应的属性值
                                        // listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);

                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    #region 选中的取消属性值
                    #endregion
                }
            }
            #endregion
            //LoadTreeGridItems(EditEntity);
            ////编辑时的添加
            //if (listView1.Enabled)
            //{
            //    CreateSKUList();
            //}
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            AttrGoups.Clear();
            contextMenuStrip1.Items.Clear();
            bindingSourceSKU明细.Clear();
            propertyEavList.Clear();

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
            if (dataGridViewProd.CurrentRow != null)
            {
                EditEntity = dataGridViewProd.CurrentRow.DataBoundItem as tb_Prod;
                if (EditEntity != null)
                {
                    if (EditEntity.tb_ProdDetails != null && EditEntity.tb_ProdDetails.Count > 0)
                    {
                        listView1.Clear();
                        AttrGoups.Clear();

                        //加载属性
                        //去掉临时添加的关系数据，因为要重新加载
                        EditEntity.tb_Prod_Attr_Relations.RemoveWhere(c => c.RAR_ID == 0);
                        LoadTreeGridItems(EditEntity);
                        AttrGoups = GetAttrGoupsByName(listView1);


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
                        listView1.UpdateUI();
                    }

                }
            }

        }




        private void LoadTreeGridItems(tb_Prod prod)
        {
            treeGridView1.Nodes.Clear();
            treeGridView1.Rows.Clear();
            Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);
            foreach (var item in prod.tb_ProdDetails)
            {
                var viewProdDetail = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>()
                                .Where(c => c.ProdDetailID == item.ProdDetailID).Single();
                if (viewProdDetail != null)
                {
                    TreeGridNode node = treeGridView1.Nodes.Add(item.ProdDetailID, item.ProdDetailID, "", viewProdDetail.prop, item.SKU, viewProdDetail.CNName, "加载");
                    node.NodeName = item.ProdDetailID.ToString();//标记节点ID，实际就是产品明细ID
                    node.Tag = item;
                    node.ImageIndex = 0;
                    foreach (var par in item.tb_Prod_Attr_Relations)
                    {
                        node.DefaultCellStyle.Font = boldFont;
                        if (par.tb_prodpropertyvalue != null && node.Index != -1)
                        {
                            TreeGridNode subnode = node.Nodes.Add(par.RAR_ID, par.RAR_ID, par.tb_prodpropertyvalue.tb_prodproperty.PropertyName, par.tb_prodpropertyvalue.PropertyValueName, "", "", "加载");
                            subnode.NodeName = par.RAR_ID.ToString();//标记节点ID，实际就是产品属性关系ID
                            subnode.Tag = par;

                            //加载属性值时，勾选对应的属性值
                            //listView1.SetItemChecked(par.tb_prodpropertyvalue.tb_prodproperty.Property_ID.ToString(), par.tb_prodpropertyvalue.PropertyValueName, true);
                        }
                        node.ImageIndex = 1;
                    }
                }

            }





        }




        private void cmbPropertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPropertyType.Tag == null)
            {
                return;
            }
            object selectObj = cmbPropertyType.SelectedValue;
            if (cmbPropertyType.SelectedItem != null && selectObj.ObjToInt() != -1)
            {
                EditEntity.PropertyType = (int)cmbPropertyType.SelectedValue;
                ProductAttributeType pt = (ProductAttributeType)cmbPropertyType.SelectedValue;
                switch (pt)
                {
                    case ProductAttributeType.单属性:
                        //ControlBtn(pt, EditEntity.ActionStatus);
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
                        if (EditEntity.tb_Prod_Attr_Relations != null)
                        {
                            //编辑性加载
                        }


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

        private void btnOk_Click(object sender, EventArgs e)
        {

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
