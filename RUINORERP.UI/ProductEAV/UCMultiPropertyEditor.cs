using AutoUpdateTools;
using FastReport.Data;
using FastReport.DevComponents.DotNetBar.Controls;
using Force.DeepCloner;
using Krypton.Navigator;
using Krypton.Workspace;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Netron.GraphLib;
using ObjectsComparer;
using RUINOR.WinFormsUI;
using RUINOR.WinFormsUI.TileListView;
using RUINORERP.Business;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.Model.ProductAttribute;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.UCSourceGrid;
using SqlSugar;
using SqlSugar.SplitTableExtensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// 对于单个产品SKU来说，我们要处理的是产品明细下面的关系
    /// </summary>
    [MenuAttrAssemblyInfo("多属性编辑", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCMultiPropertyEditor : UCBaseClass
    {
        public UCMultiPropertyEditor()
        {
            InitializeComponent();
            clientBizCodeService = Startup.GetFromFac<ClientBizCodeService>();

        }
        private ClientBizCodeService clientBizCodeService;
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
                .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                .Includes(a => a.tb_ProdDetails, b => b.tb_Prod_Attr_Relations, c => c.tb_prodproperty)
                .Includes(c => c.tb_Prod_Attr_Relations, e => e.tb_prodproperty)
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
            if (!this.DesignMode)
            {
                this.cmbPropertyType.SelectedIndexChanged += async (sender, e) => await cmbPropertyType_SelectedIndexChanged(sender, e);
            }

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


            bindingHelper.InitDataToCmbByEnumOnWhere<tb_Prod>(typeof(ProductAttributeType).GetListByEnum<ProductAttributeType>(selectedItem: 2, exclude.ToArray()), e => e.PropertyType, cmbPropertyType);

            prodpropValueList = mcPropertyValue.QueryByNav(c => true);


            prodpropList = mcProperty.QueryByNav(c => true);
            //DataBindingHelper.BindData4CmbByEnumData<tb_Prod>(entity, k => k.PropertyType, cmbPropertyType);
            DataBindingHelper.InitDataToCmb<tb_ProdProperty>(p => p.Property_ID, t => t.PropertyName, cmb属性);
            dataGridViewProd.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(tb_Prod));
            dataGridViewProd.XmlFileName = "UCMultiPropertyEditor_" + typeof(tb_Prod).Name;
            dataGridViewProd.NeedSaveColumnsXml = true;
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
        }

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
               
            }

            #endregion
        }

        private async void CheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null || !(cb.Tag is tb_ProdPropertyValue ppv))
            {
                return;
            }

            try
            {
                // 禁用UI交互，防止重复操作
                this.Enabled = false;

                // 获取当前选中的所有属性组和属性值
                var selectedAttributeGroups = GetSelectedAttributeGroups();
                if (selectedAttributeGroups.Count == 0)
                {
                    return;
                }

                // 获取当前已有的属性组合
                var existingCombinations = GetExistingAttributeCombinations();

                // 获取选中的属性组ID列表
                var selectedGroupIds = selectedAttributeGroups.Select(g => g.Property.Property_ID).ToList();

                // 获取现有属性组合中使用的属性组ID列表
                var existingGroupIds = new HashSet<long>();
                foreach (var combination in existingCombinations)
                {
                    foreach (var prop in combination.Properties)
                    {
                        existingGroupIds.Add(prop.Property.Property_ID);
                    }
                }

                // 判断是否是添加全新属性
                bool isAddingNewProperty = selectedGroupIds.Except(existingGroupIds).Any();

                // 生成属性组合
                List<AttributeCombination> newCombinations = GenerateAttributeCombinations(selectedAttributeGroups);

                // 使用AttributeCombinationComparer来比较组合
                var comparer = new AttributeCombinationComparer();

                // 根据不同场景处理组合
                List<AttributeCombination> combinationsToAdd;
                List<AttributeCombination> combinationsToRemove;

                if (isAddingNewProperty)
                {
                    // 场景1：添加全新属性 - 保留原有组合，只添加新组合
                    // 当添加新属性时，我们需要删除所有现有组合（因为它们缺少新属性），
                    // 并添加包含新属性的完整组合集合
                    combinationsToRemove = existingCombinations.ToList();
                    combinationsToAdd = newCombinations;
                }
                else
                {
                    // 场景2：在已有属性中添加新属性值 - 排除已存在的组合
                    combinationsToAdd = newCombinations.Except(existingCombinations, comparer).ToList();
                    combinationsToRemove = existingCombinations.Except(newCombinations, comparer).ToList();
                }

                // 处理需要删除的组合
                HandleCombinationsToRemove(combinationsToRemove);

                // 处理需要添加的组合 - 异步等待完成
                await HandleCombinationsToAdd(combinationsToAdd);

                treeGridView1.Refresh();
                this.btnOk.Enabled = true;
            }
            catch (Exception ex)
            {
                // 记录异常并提示用户
                MainForm.Instance.uclog.AddLog($"属性选择发生错误: {ex.Message}");
                MessageBox.Show($"处理属性选择时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI交互
                this.Enabled = true;
            }
        }



        /// <summary>
        /// 异步创建SKU代码
        /// </summary>
        /// <param name="item">产品明细对象</param>
        /// <returns>异步任务</returns>
        private async Task CreateSKUAsync(tb_ProdDetail item)
        {
            try
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
                    // 生成SKU代码，包含错误处理
                    item.SKU = await clientBizCodeService.GenerateProductSKUCodeAsync(BaseInfoType.SKU_No, EditEntity, item);

                    // 验证生成的SKU是否有效
                    if (string.IsNullOrEmpty(item.SKU))
                    {
                        throw new InvalidOperationException("SKU代码生成失败，返回值为空");
                    }

                    if (item.ActionStatus == ActionStatus.新增)
                    {
                        item.ProdDetailID = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录SKU生成错误
                MainForm.Instance.uclog.AddLog($"SKU生成失败 - 产品: {EditEntity?.CNName ?? "未知"}, 明细: {item.ProdDetailID}, 错误: {ex.Message}");

                // 设置默认SKU，避免UI显示异常
                item.SKU = $"SKU_ERROR_{DateTime.Now:yyyyMMddHHmmss}";

                // 重新抛出异常，让调用方处理
                throw;
            }
        }


        /// <summary>
        /// 获取当前选中的所有属性组和属性值
        /// </summary>
        /// <returns>属性组列表，每个属性组包含属性信息和选中的属性值</returns>
        private List<AttributeGroup> GetSelectedAttributeGroups()
        {
            var attributeGroups = new List<AttributeGroup>();

            // 遍历所有属性组
            foreach (TileGroup group in listView1.Groups)
            {
                if (group.BusinessData is tb_ProdProperty property)
                {
                    var selectedValues = new List<tb_ProdPropertyValue>();

                    // 遍历属性组中的所有属性值检查框
                    foreach (CheckBox item in group.Items)
                    {
                        if (item.Checked && item.Tag is tb_ProdPropertyValue propertyValue)
                        {
                            selectedValues.Add(propertyValue);
                        }
                    }

                    // 如果当前属性组有选中的值，则添加到结果列表
                    if (selectedValues.Count > 0)
                    {
                        attributeGroups.Add(new AttributeGroup
                        {
                            Property = property,
                            SelectedValues = selectedValues
                        });
                    }
                }
            }

            return attributeGroups;
        }

        /// <summary>
        /// 生成所有可能的属性组合
        /// </summary>
        /// <param name="attributeGroups">属性组列表</param>
        /// <returns>所有可能的属性组合</returns>
        private List<AttributeCombination> GenerateAttributeCombinations(List<AttributeGroup> attributeGroups)
        {
            var combinations = new List<AttributeCombination>();

            // 如果没有属性组，返回空列表
            if (attributeGroups.Count == 0)
            {
                return combinations;
            }

            // 初始化第一个属性组的组合
            foreach (var value in attributeGroups[0].SelectedValues)
            {
                combinations.Add(new AttributeCombination
                {
                    Properties = new List<AttributeValuePair>
                    {
                        new AttributeValuePair
                        {
                            Property = attributeGroups[0].Property,
                            PropertyValue = value
                        }
                    }
                });
            }

            // 处理后续属性组，生成所有组合
            for (int i = 1; i < attributeGroups.Count; i++)
            {
                var tempCombinations = new List<AttributeCombination>();

                foreach (var existingCombination in combinations)
                {
                    foreach (var value in attributeGroups[i].SelectedValues)
                    {
                        var newCombination = new AttributeCombination
                        {
                            Properties = new List<AttributeValuePair>(existingCombination.Properties)
                        };

                        newCombination.Properties.Add(new AttributeValuePair
                        {
                            Property = attributeGroups[i].Property,
                            PropertyValue = value
                        });

                        tempCombinations.Add(newCombination);
                    }
                }

                combinations = tempCombinations;
            }

            return combinations;
        }

        /// <summary>
        /// 获取当前已有的属性组合
        /// </summary>
        /// <returns>已有的属性组合列表</returns>
        private List<AttributeCombination> GetExistingAttributeCombinations()
        {
            var existingCombinations = new List<AttributeCombination>();

            // 从TreeGrid中获取现有的产品详情
            foreach (TreeGridNode node in treeGridView1.Nodes)
            {
                if (node.Tag is tb_ProdDetail detail)
                {
                    var combination = new AttributeCombination
                    {
                        Properties = new List<AttributeValuePair>(),
                        ProductDetail = detail
                    };

                    // 遍历产品详情的所有属性关系
                    foreach (TreeGridNode subNode in node.Nodes)
                    {
                        if (subNode.Tag is tb_Prod_Attr_Relation relation)
                        {
                            combination.Properties.Add(new AttributeValuePair
                            {
                                Property = relation.tb_prodproperty,
                                PropertyValue = relation.tb_prodpropertyvalue
                            });
                        }
                    }

                    // 如果组合有属性，则添加到结果列表
                    if (combination.Properties.Count > 0)
                    {
                        //属性存在才添加，因为如果是从单属性转换为多属性时，第一个是没有特性的
                        if (combination.Properties.Where(c => c.Property != null).ToList().Count > 0)
                        {
                            existingCombinations.Add(combination);
                        }

                    }
                }
            }

            return existingCombinations;
        }

        /// <summary>
        /// 处理需要删除的属性组合
        /// </summary>
        /// <param name="combinationsToRemove">需要删除的组合列表</param>
        private void HandleCombinationsToRemove(List<AttributeCombination> combinationsToRemove)
        {
            foreach (var combination in combinationsToRemove)
            {
                if (combination.ProductDetail != null)
                {
                    // 从TreeGrid中移除对应的节点
                    TreeGridNode nodeToRemove = treeGridView1.Nodes.FirstOrDefault(n => n.Tag == combination.ProductDetail);
                    if (nodeToRemove != null)
                    {
                        // 如果是新增的产品详情，直接删除
                        if (combination.ProductDetail.ActionStatus == ActionStatus.新增)
                        {
                            // 移除所有子节点对应的属性关系
                            foreach (TreeGridNode subNode in nodeToRemove.Nodes)
                            {
                                if (subNode.Tag is tb_Prod_Attr_Relation relation)
                                {
                                    EditEntity.tb_Prod_Attr_Relations.Remove(relation);
                                }
                            }

                            // 移除产品详情
                            EditEntity.tb_ProdDetails.Remove(combination.ProductDetail);
                            treeGridView1.Nodes.Remove(nodeToRemove);
                        }
                        // 如果是已存在的产品详情，标记为删除
                        else
                        {
                            combination.ProductDetail.ActionStatus = ActionStatus.删除;
                            nodeToRemove.Cells[6].Value = "删除";
                            nodeToRemove.ImageIndex = 3; // 删除图标
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 处理需要添加的属性组合
        /// </summary>
        /// <param name="combinationsToAdd">需要添加的组合列表</param>
        private async Task HandleCombinationsToAdd(List<AttributeCombination> combinationsToAdd)
        {
            // 创建TreeGrid节点
            Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);

            // 检查是否是从单属性转换为多属性的情况，获取原始SKU信息
            tb_ProdDetail originalSkuDetail = null;
            if (EditEntity.tb_Prod_Attr_Relations.Count == 1)
            {
                ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));
                if (EditEntity.PropertyType == (int)ProductAttributeType.单属性 && pt == ProductAttributeType.可配置多属性 && EditEntity.tb_Prod_Attr_Relations[0].Property_ID == null)
                {
                    // 从单属性转换为多属性，获取原始产品详情
                    originalSkuDetail = EditEntity.tb_ProdDetails.FirstOrDefault();
                }
            }

            bool isFirstCombination = true;
            foreach (var combination in combinationsToAdd)
            {
                // 创建新的产品详情
                long skuRowId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                var newDetail = new tb_ProdDetail
                {
                    ProdBaseID = EditEntity.ProdBaseID,
                    ProdDetailID = 0,// skuRowId, // 临时ID，保存到DB前会重新设置
                    ActionStatus = ActionStatus.新增,
                    Is_enabled = true,
                    Is_available = true,
                    Created_at = DateTime.Now,
                    Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID,
                    tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>()
                };

                // 如果是第一个组合且存在原始SKU信息，继承原始SKU
                // /第一个组合设为默认
                if (isFirstCombination && originalSkuDetail != null)
                {
                    newDetail.SKU = originalSkuDetail.SKU;
                    // 复制其他相关字段
                    newDetail.ProdBaseID = originalSkuDetail.ProdBaseID;
                    newDetail.ProdDetailID = originalSkuDetail.ProdDetailID;
                    newDetail.PrimaryKeyID = originalSkuDetail.PrimaryKeyID;
                    newDetail.BarCode = originalSkuDetail.BarCode;
                    newDetail.Weight = originalSkuDetail.Weight;
                    newDetail.BOM_ID = originalSkuDetail.BOM_ID;
                    newDetail.DataStatus = originalSkuDetail.DataStatus;
                    newDetail.Created_at = originalSkuDetail.Created_at;
                    newDetail.Created_by = originalSkuDetail.Created_by;
                    BusinessHelper.Instance.EditEntity(newDetail); // 初始化编辑信息
                    newDetail.Discount_Price = originalSkuDetail.Discount_Price;
                    newDetail.Image = originalSkuDetail.Image;
                    newDetail.Images = originalSkuDetail.Images;
                    newDetail.ImagesPath = originalSkuDetail.ImagesPath;
                    newDetail.Is_available = originalSkuDetail.Is_available;
                    newDetail.Is_enabled = originalSkuDetail.Is_enabled;
                    newDetail.Market_Price = originalSkuDetail.Market_Price;
                    newDetail.Modified_at = originalSkuDetail.Modified_at;
                    newDetail.Modified_by = originalSkuDetail.Modified_by;

                    newDetail.Notes = originalSkuDetail.Notes;
                    newDetail.SalePublish = originalSkuDetail.SalePublish;
                    newDetail.Standard_Price = originalSkuDetail.Standard_Price;

                }

                // 为每个属性值创建属性关系
                foreach (var attrValuePair in combination.Properties)
                {
                    var relation = new tb_Prod_Attr_Relation
                    {
                        Property_ID = attrValuePair.Property.Property_ID,
                        PropertyValueID = attrValuePair.PropertyValue.PropertyValueID,
                        ProdBaseID = EditEntity.ProdBaseID,
                        ProdDetailID = newDetail.ProdDetailID,// skuRowId,
                        ActionStatus = ActionStatus.新增,
                        tb_prodproperty = attrValuePair.Property,
                        tb_prodpropertyvalue = attrValuePair.PropertyValue
                    };

                    //如果由单属性转换过来的，第一个则为默认的

                    if (isFirstCombination && originalSkuDetail != null)
                    {
                        relation.RAR_ID = originalSkuDetail.tb_Prod_Attr_Relations[0].RAR_ID;
                        relation.ActionStatus = ActionStatus.修改;
                    }

                    // 添加到产品详情和编辑实体
                    if (!newDetail.tb_Prod_Attr_Relations.Any(c => c.RAR_ID == relation.RAR_ID))
                    {
                        newDetail.tb_Prod_Attr_Relations.Add(relation);
                    }
                    else
                    {

                        var oldRelation = newDetail.tb_Prod_Attr_Relations.FirstOrDefault(c => c.RAR_ID == relation.RAR_ID);
                        if (oldRelation != null)
                        {
                            oldRelation.Property_ID = relation.Property_ID;
                            oldRelation.PropertyValueID = relation.PropertyValueID;
                            oldRelation.tb_prodproperty = relation.tb_prodproperty;
                            oldRelation.tb_prodpropertyvalue = relation.tb_prodpropertyvalue;
                        }

                    }

                    if (string.IsNullOrEmpty(newDetail.SKU))
                    {
                        await CreateSKUAsync(newDetail);
                    }
                    // 添加到产品关系
                    if (!EditEntity.tb_Prod_Attr_Relations.Any(c => c.RAR_ID == relation.RAR_ID))
                    {
                        EditEntity.tb_Prod_Attr_Relations.Add(relation);
                    }
                }

                TreeGridNode node = treeGridView1.Nodes.FirstOrDefault(c => c.NodeName == newDetail.ProdDetailID.ToString());
                if (node == null)
                {
                    node = treeGridView1.Nodes.Add(skuRowId, 0, "", GetPropertiesText(combination),
                      newDetail.SKU != null ? newDetail.SKU : "等待生成", EditEntity.CNName, "新增", newDetail.Is_enabled);
                    node.NodeName = skuRowId.ToString();
                    node.ImageIndex = 1; // 新增图标
                    node.Tag = newDetail;
                    node.DefaultCellStyle.Font = boldFont;
                }

                foreach (var relation in newDetail.tb_Prod_Attr_Relations)
                {
                    // 创建子节点
                    TreeGridNode subNode = treeGridView1.Nodes.FirstOrDefault(c => c.NodeName == relation.RAR_ID.ToString());
                    if (subNode == null)
                    {
                        subNode = node.Nodes.Add(relation.RAR_ID, relation.RAR_ID, relation.tb_prodproperty.PropertyName, relation.tb_prodpropertyvalue.PropertyValueName, "", "", "新增");
                        subNode.NodeName = relation.RAR_ID.ToString();
                        subNode.Tag = relation;
                        subNode.ImageIndex = 1; // 新增图标
                    }

                }

                // 添加到产品详情列表
                if (!EditEntity.tb_ProdDetails.Any(c => c.ProdDetailID == newDetail.ProdDetailID))
                {
                    EditEntity.tb_ProdDetails.Add(newDetail);
                }
                isFirstCombination = false;
            }
        }

        /// <summary>
        /// 根据属性组合生成显示文本
        /// </summary>
        /// <param name="combination">属性组合</param>
        /// <returns>显示文本</returns>
        private string GetPropertiesText(AttributeCombination combination)
        {
            return string.Join(",", combination.Properties.Select(p => p.PropertyValue.PropertyValueName));
        }

        /// <summary>
        /// 检查框状态改变事件处理
        /// 简化的多属性组合生成逻辑，确保包含完整的属性信息
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        [Obsolete]
        private void CheckBox_CheckStateChanged_old(object sender, EventArgs e)
        {
            // 获取触发事件的检查框
            CheckBox cb = sender as CheckBox;

            List<KeyValuePair<long, string[]>> attrGoupsByID = GetAttrGoupsByIDName(listView1, g => g.GroupID, lvitem => lvitem.Text);

            List<string> NewMixByListView = ArrayCombination.Combination4Table(attrGoupsByID);


            //要比较的集合数据
            List<string> OldMixByTreeGrid = new List<string>();

            #region 从下面TreeGrid中获取的组合关系。新旧进行比较 取差集 注意排序问题 不然比较结果有问题

            List<tb_Prod_Attr_Relation> attr_Relations = GetProdDetailsFromTreeGrid();

            var existDetails = attr_Relations.GroupBy(c => c.ProdDetailID).ToList();

            //看用户选择的是单属性还是多属性
            ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));
            if (EditEntity.tb_Prod_Attr_Relations.Count == 1 && pt == ProductAttributeType.可配置多属性)
            {
                //如果开始是由单属性转为多属性时，第一行默认给空 则直接添加为空
                if (EditEntity.tb_Prod_Attr_Relations[0].Property_ID == null)
                {
                    OldMixByTreeGrid.Add("");
                }
                else
                {
                    foreach (var item in existDetails)
                    {
                        var array = attr_Relations
                            .Where(c => c.ProdDetailID == item.Key)
                             .OrderBy(c => c.Property_ID) // 按属性 ID 排序
                            .ToList()
                            .Select(c => c.PropertyValueID + "|" + c.tb_prodpropertyvalue.PropertyValueName)
                            .ToArray();
                        OldMixByTreeGrid.Add(string.Join(",", array));
                    }
                }

            }
            else
            {
                foreach (var item in existDetails)
                {
                    var array = attr_Relations
                        .Where(c => c.ProdDetailID == item.Key)
                        .OrderBy(c => c.Property_ID) // 按属性 ID 排序
                        .ToList()
                        .Select(c => c.PropertyValueID + "|" + c.tb_prodpropertyvalue.PropertyValueName)
                        .ToArray();
                    OldMixByTreeGrid.Add(string.Join(",", array));
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
                    var addItem差集 = NewMixByListView.Except(OldMixByTreeGrid).ToList();
                    //先判断差值的唯度。就是属性的个数是否一样
                    if (OldMixByTreeGrid.Count > 0 && NewMixByListView.Count > 0)
                    {
                        if (OldMixByTreeGrid[0].Split(',').Count() != NewMixByListView[0].Split(',').Count())
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
                                    tb_ProdDetail detail = new tb_ProdDetail();
                                    detail.ProdBaseID = EditEntity.ProdBaseID;
                                    detail.ProdDetailID = SkuRowid; //为了后面可以查询暂时保存行号。实际保存DB前要生新设置为0.
                                    //detail.SKU = prop;为了不浪费  保存时再成生一次
                                    detail.ActionStatus = ActionStatus.新增;
                                    detail.Is_enabled = true;
                                    detail.Is_available = true;
                                    detail.Created_at = DateTime.Now;
                                    detail.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                                    TreeGridNode node = treeGridView1.Nodes.Add(SkuRowid, 0, "", prop, "等待生成", EditEntity.CNName, "新增", detail.Is_enabled);
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
                    var reduceItem差集 = OldMixByTreeGrid.Except(NewMixByListView).ToList();
                    //
                    #region 选中的取消属性值

                    //先判断差值的唯度。就是属性的个数是否一样
                    if (OldMixByTreeGrid.Count > 0 && NewMixByListView.Count > 0)
                    {
                        if (OldMixByTreeGrid[0].Split(',').Count() != NewMixByListView[0].Split(',').Count())
                        {
                            var reduceItem差集2 = NewMixByListView.Except(OldMixByTreeGrid).ToList();
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
                                                    else
                                                    {
                                                        MessageBox.Show($"已经存在的属性组合【{values[0].Substring(values[0].IndexOf('|') + 1)}】暂时不支持删除。\r\n可以通过勾选顺序来调整生成SKU属性值时的顺序。");
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

                    #endregion
                }
            }


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
            prodDetails_relation = prodDetails_relation.OrderBy(p => p.Property_ID).ToList();

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
                if (e.Value != null && e.Value.GetType() == typeof(byte[]))
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
                            var propertys = EditEntity.tb_Prod_Attr_Relations.Where(c => c.Property_ID.HasValue).Select(c => c.Property_ID.Value).Distinct().ToList();
                            foreach (var item in propertys)
                            {
                                var pvs = EditEntity.tb_Prod_Attr_Relations.Where(c => c.Property_ID.HasValue && c.Property_ID.Value == item).Select(c => c.PropertyValueID.Value).Distinct().ToList();
                                foreach (var pv in pvs)
                                {
                                    var group = listView1.Groups.FirstOrDefault(c => c.GroupID == item.ToString());
                                    if (group != null)
                                    {
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
        // 在类开始处添加：
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());

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
                List<tb_Prod_Attr_Relation> ProdAttrRelations = item.tb_Prod_Attr_Relations
                    .OrderBy(p => p.Property_ID).ToList();
                View_ProdDetail viewProdDetail = null;
                viewProdDetail = CacheManager.GetEntity<View_ProdDetail>(item.ProdDetailID);
                if (viewProdDetail == null)
                {
                    //根据产品明细ID查询View_ProdDetail视图
                    viewProdDetail = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>()
                                   .Where(c => c.ProdDetailID == item.ProdDetailID).Single();
                }

                //如果查询到视图，则添加节点
                if (viewProdDetail != null)
                {
                    string DisplayPropText = string.Empty;
                    if (viewProdDetail.prop != null)
                    {
                        DisplayPropText = viewProdDetail.prop;
                        var array = ProdAttrRelations
                            .Where(c => c.ProdDetailID == item.ProdDetailID)
                            .OrderBy(c => c.Property_ID) // 按属性 ID 排序
                            .ToList().Select(c => c.tb_prodpropertyvalue.PropertyValueName).ToArray();
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




        private async Task cmbPropertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                                    //BindToSkulistGrid(new List<tb_ProdDetail>());
                                }
                                if (EditEntity.ActionStatus != ActionStatus.加载)
                                {
                                    tb_ProdDetail ppg = new tb_ProdDetail();
                                    ppg.PropertyGroupName = "";
                                    ppg.SKU = await clientBizCodeService.GenerateProductSKUCodeAsync(BaseInfoType.SKU_No, EditEntity, ppg);
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
            btnOk.Enabled = false;
            List<string> MixByTreeGrid = new List<string>();
            #region 获取最新的组合关系。并且保存为一个新的数组与现有的组合关系进行比较 取差集
            List<tb_Prod_Attr_Relation> attr_Relations = GetProdDetailsFromTreeGrid();
            var existDetails = attr_Relations.GroupBy(c => c.ProdDetailID).ToList();
            foreach (var item in existDetails)
            {
                var array = attr_Relations
                    .Where(c => c.ProdDetailID == item.Key)
                     .OrderBy(c => c.Property_ID) // 按属性 ID 排序
                    .ToList()
                    .Select(c => c.PropertyValueID + "|" + c.tb_prodpropertyvalue.PropertyValueName).ToArray();
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



            tb_ProdController<tb_Prod> pctr = Startup.GetFromFac<tb_ProdController<tb_Prod>>();
            ReturnResults<tb_Prod> rr = new ReturnResults<tb_Prod>();
            rr = await pctr.SaveOrUpdateAsync(EditEntity);
            if (rr.Succeeded)
            {
                btnOk.Enabled = true;
                MainForm.Instance.uclog.AddLog("保存成功");
                this.Exit(this);
            }
            else
            {
                btnOk.Enabled = true;
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


        private async void 删除属性值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeGridView1.CurrentCell != null)
            {
                if (dataGridViewProd.CurrentRow != null)
                {
                    //EditEntity = null;
                    oldOjb = null;
                    if (dataGridViewProd.CurrentRow.DataBoundItem is tb_Prod Prod)
                    {
                        if (treeGridView1.CurrentRow.Tag is tb_Prod_Attr_Relation Prod_Attr_Relation)
                        {
                            //var RAR_ID = treeGridView1.CurrentRow.NodeName;
                            //var Prod_Attr_Relation = Prod.tb_Prod_Attr_Relations.FirstOrDefault(c => c.RAR_ID == RAR_ID);
                            if (Prod_Attr_Relation != null)
                            {
                                if (Prod_Attr_Relation.RAR_ID > 0)
                                {
                                    //删除
                                    if (MessageBox.Show("确定删除该SKU对应的属性值吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        int counter = await MainForm.Instance.AppContext.Db.Deleteable(Prod_Attr_Relation).ExecuteCommandAsync(); ;
                                        if (counter > 0)
                                        {
                                            //如果有外键引用了。会出错。这里删除没有问题。
                                            //刷新
                                            //LoadProdDetail();
                                            treeGridView1.Nodes.Remove(treeGridView1.CurrentNode);
                                            MainForm.Instance.ShowStatusText("成功删除该SKU对应的属性值。");
                                        }
                                    }
                                }
                                else
                                {
                                    //新增式 生成的组合有错误时。这里可以手动删除指定的行，其实就是树的节点
                                    if (treeGridView1.CurrentNode != null && treeGridView1.CurrentNode.Tag is tb_Prod_Attr_Relation)
                                    {
                                        treeGridView1.Nodes.Remove(treeGridView1.CurrentNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void 删除SKU明细toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeGridView1.CurrentCell != null)
            {
                if (dataGridViewProd.CurrentRow != null)
                {
                    //EditEntity = null;
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
                        else
                        {
                            //新增式 生成的组合有错误时。这里可以手动删除指定的行，其实就是树的节点

                            if (treeGridView1.CurrentNode != null && treeGridView1.CurrentNode.Tag is tb_ProdDetail)
                            {
                                treeGridView1.Nodes.Remove(treeGridView1.CurrentNode);
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

        /// <summary>
        /// TreeGridView单元格双击事件处理
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void treeGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            try
            {
                // 获取当前选中的节点
                var currentNode = treeGridView1.CurrentNode;
                if (currentNode == null)
                {
                    return;
                }

                // 判断节点类型并处理
                if (currentNode.Tag is tb_Prod_Attr_Relation relation)
                {
                    // 双击属性关系节点，打开编辑对话框
                    EditAttributeRelation(relation);
                }
                else if (currentNode.Tag is tb_ProdDetail prodDetail)
                {
                    // 双击产品明细节点，可以扩展功能，如查看明细详情
                    // 这里暂时不处理，后续可根据需求扩展
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"双击属性关系时发生错误: {ex.Message}");
                MessageBox.Show($"处理属性关系时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑属性关系菜单项点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void 编辑属性关系ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取当前选中的节点
                var currentNode = treeGridView1.CurrentNode;
                if (currentNode == null)
                {
                    MessageBox.Show("请先选择要编辑的属性关系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 判断节点类型
                if (currentNode.Tag is tb_Prod_Attr_Relation relation)
                {
                    // 打开编辑对话框
                    EditAttributeRelation(relation);
                }
                else
                {
                    MessageBox.Show("只能编辑属性关系节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"编辑属性关系时发生错误: {ex.Message}");
                MessageBox.Show($"编辑属性关系时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑属性关系
        /// </summary>
        /// <param name="relation">要编辑的属性关系</param>
        private void EditAttributeRelation(tb_Prod_Attr_Relation relation)
        {
            try
            {
                // 创建编辑窗体
                using (var editForm = new UCProductAttrRelationEdit())
                {
                    // 设置窗体标题
                    editForm.Text = $"编辑属性关系 - {EditEntity?.CNName ?? "未知产品"}";

                    // 绑定数据到编辑窗体
                    editForm.BindData(relation);

                    // 显示对话框
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // 更新实体状态
                        if (relation.ActionStatus == ActionStatus.加载)
                        {
                            relation.ActionStatus = ActionStatus.修改;
                        }

                        // 刷新TreeGridView显示
                        treeGridView1.Refresh();

                        // 启用保存按钮
                        this.btnOk.Enabled = true;

                        MainForm.Instance.ShowStatusText("属性关系编辑成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"编辑属性关系失败: {ex.Message}");
                MessageBox.Show($"编辑属性关系失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
