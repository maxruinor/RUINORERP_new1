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
                .Includes(c => c.tb_ProdDetails)
                .Includes(c => c.tb_Prod_Attr_Relations, d => d.tb_prodpropertyvalue, e => e.tb_prodproperty)
                .Where(exp)

                .ToList();


            //var list = dc.BaseQueryByWhereTop(exp, 100);


            bindingSourc产品.DataSource = list.ToBindingSortCollection();
        }
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;


        SourceGridDefine sgd1 = null;
        SourceGridHelper sgh1 = new SourceGridHelper();

        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        List<SourceGridDefineColumnItem> listCols1 = new List<SourceGridDefineColumnItem>();

        private void UCMultiPropertyEditor_Load(object sender, EventArgs e)
        {
            listView属性显示.CheckBoxes = true;
            EnumBindingHelper bindingHelper = new EnumBindingHelper();
            //https://www.cnblogs.com/cdaniu/p/15236857.html
            //加载枚举，并且可以过虑不需要的项
            List<int> exclude = new List<int>();
            exclude.Add((int)ProductAttributeType.虚拟);
            bindingHelper.InitDataToCmbByEnumOnWhere<tb_Prod>(typeof(ProductAttributeType).GetListByEnum(2, exclude.ToArray()), e => e.PropertyType, cmbPropertyType);

            prodpropValueList = mcPropertyValue.Query();
            prodpropList = mcProperty.Query();
            //DataBindingHelper.BindData4CmbByEnumData<tb_Prod>(entity, k => k.PropertyType, cmbPropertyType);
            DataBindingHelper.InitDataToCmb<tb_ProdProperty>(p => p.Property_ID, t => t.PropertyName, cmb属性);
            dataGridViewProd.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(tb_Prod));
            dataGridViewProd.XmlFileName = "UCMultiPropertyEditor_" + typeof(tb_Prod).Name;
            dataGridViewProd.FieldNameList = FieldNameList1;
            dataGridViewProd.DataSource = null;
            bindingSourc产品.DataSource = new List<tb_Prod>();
            dataGridViewProd.DataSource = bindingSourc产品;

            LoadGrid1();
        }

        //当前编辑的品
        tb_Prod EditEntity;

        private void LoadGrid1()
        { ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_ProdDetail>();


            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;



            //指定了关键字段ProdDetailID
            //listCols1 = sgh1.GetGridColumns<ProductSharePart, tb_ProdDetail>(c => c.ProdDetailID, false);
            listCols1 = sgh1.GetGridColumns<tb_ProdDetail>();
            //listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            //listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            //listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.ProdDetailID);
            //listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Discount_Price);
            //listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Discount_Price);
            //listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Discount_Price);
            //listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Discount_Price);
            listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Created_at);
            listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Created_by);
            listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Modified_at);
            listCols1.SetCol_NeverVisible<tb_ProdDetail>(c => c.Modified_by);
            //listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);

            if (!MainForm.Instance.AppContext.SysConfig.UseBarCode)
            {
                //listCols1.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            // ControlChildColumnsInvisible(listCols1);


            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);


            sgd1 = new SourceGridDefine(grid1, listCols1, true);
            //  sgd1.GridData = EditEntity;




            if (CurMenuInfo.tb_P4Fields != null)
            {
                foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
                {
                    //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                    listCols1.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_ProdDetail));
                }

            }
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/


            //公共到明细的映射 源 ，左边会隐藏
            //  sgh1.SetPointToColumnPairs<ProductSharePart, tb_ProdDetail>(sgd1, f => f.SKU, t => t.SKU);
            // sgh1.SetPointToColumnPairs<ProductSharePart, tb_ProdDetail>(sgd1, f => f.Standard_Price, t => t.Standard_Price);
            // sgh1.SetPointToColumnPairs<ProductSharePart, tb_ProdDetail>(sgd1, f => f.BarCode, t => t.BarCode);
            //  sgh1.SetPointToColumnPairs<ProductSharePart, tb_ProdDetail>(sgd1, f => f.Images, t => t.Images);


            //应该只提供一个结构
            List<tb_ProdDetail> lines = new List<tb_ProdDetail>();
            bindingSourceSKU明细.DataSource = lines;
            sgd1.BindingSourceLines = bindingSourceSKU明细;

            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
               .AndIF(true, w => w.CNName.Length > 0)
              // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
              .ToExpression();//注意 这一句 不能少
                              // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);
            //sgd1.SetDependencyObject<ProductSharePart, tb_ProdDetail>(list);

            sgd1.HasRowHeader = true;
            sgh1.InitGrid(grid1, sgd1, true, nameof(tb_ProdDetail));
        }
        //定义两个值，为了计算listview的高宽，高是属性的倍数 假设一个属性一行 是50px，有三组则x3
        //宽取每组属性中值的最多个数,的字长，一个字算20px?
        int PropertyCounter = 0;
        private void cmb属性_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb属性.SelectedItem is tb_ProdProperty)
            {
                tb_ProdProperty ppv = cmb属性.SelectedItem as tb_ProdProperty;
                if (!listView属性显示.Groups.Cast<ListViewGroup>().Any(i => i.Header == ppv.PropertyName.Trim()))
                {
                    btnAddProperty.Enabled = true;
                }
            }
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

            listView属性显示.ItemCheck -= listView属性显示_ItemCheck;
            AddProdProperty(ppv, prodpropValueList);
            listView属性显示.ItemCheck += listView属性显示_ItemCheck;
            PropertyCounter = listView属性显示.Groups.Count;
            //PropertyValueMaxCounter=prodpropValueList.Where(w=>w.)
            ControlBtn(ProductAttributeType.可配置多属性);
            btnAddProperty.Enabled = false;


            AttrGoups = GetAttrGoups(listView属性显示);
            CreateSKUList();
        }


        /// <summary>
        /// 将listview的UI值转为属性组
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        private List<KeyValuePair<long, string[]>> GetAttrGoups(ListView lv)
        {
            List<KeyValuePair<long, string[]>> attrGoups = new List<KeyValuePair<long, string[]>>();
            foreach (ListViewGroup g in lv.Groups)
            {
                tb_ProdProperty pp = g.Tag as tb_ProdProperty;
                long key = pp.Property_ID;
                string values = string.Empty;
                foreach (ListViewItem lvitem in g.Items)
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
                    kryptonGroupBoxListView.Visible = false;
                    listView属性显示.Width = flowLayoutPanel1.Width;
                    listView属性显示.Height = flowLayoutPanel1.Height;

                    break;
                case ProductAttributeType.可配置多属性:
                    cmb属性.Enabled = true;
                    btnAddProperty.Enabled = true;
                    btnClear.Enabled = true;
                    kryptonGroupBoxListView.Visible = true;
                    grid1.Visible = true;
                    listView属性显示.Height = 80 * PropertyCounter;
                    kryptonGroupBoxListView.Height = listView属性显示.Height;
                    kryptonGroupBoxListView.Width = listView属性显示.Width;


                    grid1.Width = flowLayoutPanel1.Width;
                    grid1.Height = flowLayoutPanel1.Height - kryptonGroupBoxListView.Height;


                    break;
                case ProductAttributeType.捆绑:
                    break;
                case ProductAttributeType.虚拟:
                    break;
                default:
                    break;
            }
        }


        private void listView属性显示_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            #region 思路 与GetAttrGoups(listView属性显示) 不一样，因为选择状态问题

            //attrGoups
            ListView lv = sender as ListView;
            if (lv.Items[e.Index].Tag is tb_ProdPropertyValue)
            {
                //tb_ProdPropertyValue ci = lv.Items[e.Index].Tag as tb_ProdPropertyValue;
                CheckState nv = e.NewValue;
                if (lv.Items[e.Index].Group.Tag is tb_ProdProperty)
                {
                    tb_ProdProperty tpp = lv.Items[e.Index].Group.Tag as tb_ProdProperty;
                    //先找到这个属性组
                    List<KeyValuePair<long, string[]>> exitkvps = AttrGoups.Where(t => t.Key == tpp.Property_ID).ToList();
                    if (exitkvps.Count > 0)
                    {
                        #region

                        List<string> text = exitkvps[0].Value.ToList();
                        if (nv == CheckState.Checked)//添加
                        {
                            if (!text.Contains(lv.Items[e.Index].Text))
                            {
                                text.Add(lv.Items[e.Index].Text);
                                //联动下拉
                            }
                        }
                        else//取消
                        {
                            text.Remove(lv.Items[e.Index].Text);
                            //联动下拉
                            #region

                            #endregion
                        }
                        KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(tpp.Property_ID, text.ToArray());
                        AttrGoups.Remove(exitkvps[0]);
                        AttrGoups.Add(kvp);
                        #endregion
                    }
                    else
                    {
                        //不存在这个情况
                    }


                }
            }

            #endregion


            //编辑时的添加
            if (listView属性显示.Enabled)
            {
                CreateSKUList();
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
            foreach (ListViewGroup g in listView属性显示.Groups)
            {
                tb_ProdProperty pp = g.Tag as tb_ProdProperty;
                if (pp.Property_ID == tpp.Property_ID)
                {
                    foreach (ListViewItem lvitem in g.Items)
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
            listView属性显示.Visible = true;
            listView属性显示.CheckBoxes = true;

            listView属性显示.ShowGroups = true;  //记得要设置ShowGroups属性为true（默认是false），否则显示不出分组
            listView属性显示.View = View.Details;

            ColumnHeader columnHeader1 = new ColumnHeader();
            listView属性显示.Columns.Add(columnHeader1);
            columnHeader1.Text = "属性值";
            columnHeader1.Width = 100;

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            listView属性显示.SmallImageList = imgList;

            //create goups
            ListViewGroup lvg = new ListViewGroup(ppv.PropertyName, HorizontalAlignment.Center);  //创建分组
            lvg.Header = ppv.PropertyName;  //设置组的标题。
            lvg.Name = ppv.Property_ID.ToString();
            lvg.Tag = ppv;
            //lvg.HeaderAlignment = HorizontalAlignment.Left;   //设置组标题文本的对齐方式。（默认为Left）

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

            if (!listView属性显示.Groups.Cast<ListViewGroup>().Any(i => i.Header == ppv.PropertyName.Trim()))
            {
                listView属性显示.Groups.Add(lvg);
                string keys = string.Empty;
                string names = string.Empty;
                foreach (var item in listOptionValue.Where(w => w.Property_ID == ppv.Property_ID).ToList())
                {
                    keys += item.PropertyValueID + ",";
                    names += item.PropertyValueName + ",";
                    ListViewItem lvi = new ListViewItem();
                    // lvi.ImageIndex = i;
                    lvi.Name = item.PropertyValueID.ToString();
                    lvi.Tag = item;
                    lvi.Text = item.PropertyValueName;
                    lvi.Checked = true;
                    lvi.ForeColor = Color.Blue;  //设置行颜色
                    lvg.Items.Add(lvi);   //分组添加子项
                    listView属性显示.Items.Add(lvi);
                }
                keys = keys.Trim(',');
                names = names.Trim(',');
                if (!string.IsNullOrEmpty(names))
                {
                    propertyEavList.TryAdd(ppv.Property_ID.ToString(), names);
                }
            }

            // BindToSkulistGrid(new List<Eav_ProdDetails>());

            if (EditEntity.tb_ProdDetails != null && EditEntity.tb_ProdDetails.Count > 0)
            {
                sgh1.LoadItemDataToGrid<tb_ProdDetail>(grid1, sgd1, EditEntity.tb_ProdDetails, c => c.ProdDetailID);
            }



            #endregion
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView属性显示.Items.Clear();
            listView属性显示.Groups.Clear();
            listView属性显示.Clear();

            bindingSourceSKU明细.Clear();
            AttrGoups.Clear();
            propertyEavList.Clear();
            //contextMenuStrip1.Items.Clear();

            #region 单属性

            //ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));// EnumHelper.GetEnumByString<ProductAttributeType>(cmbPropertyType.SelectedItem.ToString());
            //switch (pt)
            //{
            //    case ProductAttributeType.单属性:
            //        if (cmb属性.SelectedItem == null)
            //        {
            //            //添加单属性时的SKU
            //            #region
            //            btnAddProperty.Enabled = false;

            //            //UCSKUlist ucskulist = new UCSKUlist();

            //            //tableLayoutPanel1.Controls.Remove(ucskulist);
            //            #endregion
            //        }
            //        break;
            //    case ProductAttributeType.可配置多属性:
            //        btnAddProperty.Enabled = true;
            //        break;
            //    case ProductAttributeType.捆绑:
            //        break;
            //    case ProductAttributeType.虚拟:
            //        break;
            //    default:
            //        break;
            //}

            //// tableLayoutPanel1.Controls.Remove(ucskulist);

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
                        sgh1.LoadItemDataToGrid<tb_ProdDetail>(grid1, sgd1, EditEntity.tb_ProdDetails, c => c.ProdDetailID);
                        //加载属性类型，再加载属性及对应的值
                        cmbPropertyType.SelectedValue = EditEntity.PropertyType;
                        List<tb_ProdProperty> propList = new List<tb_ProdProperty>();
                        foreach (var item in EditEntity.tb_Prod_Attr_Relations)
                        {
                            if (item.tb_prodpropertyvalue != null && propList.Contains(item.tb_prodpropertyvalue.tb_prodproperty) == false)
                            {
                                propList.Add(item.tb_prodpropertyvalue.tb_prodproperty);
                            }
                        }
                        foreach (var item in propList)
                        {
                            AddProperty(item);
                        }
                        //加载属性
                        //EditEntity.tb_Prod_Attr_Relations = EditEntity.tb_Prod_Attr_Relations.OrderBy(c => c.tb_Prod_Attr.AttrOrder).ToList();

                        //加载属性值
                    }
                }
            }

        }

        private void BindToSkulistGrid(List<Eav_ProdDetails> propGroups)
        {
            dataGridViewProd.RowHeadersVisible = false;
            bindingSourceSKU明细.DataSource = propGroups;
            dataGridViewProd.DataSource = bindingSourceSKU明细;
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
                            listView属性显示.Visible = false;
                            listView属性显示.ShowGroups = true;  //记得要设置ShowGroups属性为true（默认是false），否则显示不出分组

                            if (dataGridViewProd.Rows.Count == 0)
                            {
                                BindToSkulistGrid(new List<Eav_ProdDetails>());
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
                        listView属性显示.ItemCheck -= listView属性显示_ItemCheck;
                        listView属性显示.ItemCheck += listView属性显示_ItemCheck;
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

    }
}
