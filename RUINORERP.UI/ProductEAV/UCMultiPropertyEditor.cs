using AutoUpdateTools;
using ExCSS;
using FastReport.Data;
using FastReport.DevComponents.DotNetBar.Controls;
using Force.DeepCloner;
using Krypton.Navigator;
using Krypton.Workspace;
using MathNet.Numerics.LinearAlgebra.Complex.Solvers;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Netron.GraphLib;
using NPOI.SS.Formula.Functions;
using ObjectsComparer;
using RUINOR.Core;
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
using System.Windows.Media.Imaging;
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
                    //如果一个属性都没有选 。可能是取消选中，全没有选。则要恢复默认第一个产品中的属性为空，或直接加载
                    #region
                    //去掉临时添加的关系数据，因为要重新加载
                    EditEntity.tb_ProdDetails.RemoveWhere(c => c.ProdDetailID == 0);
                    EditEntity.tb_ProdDetails.RemoveWhere(c => c.ActionStatus == ActionStatus.新增);
                    if (EditEntity.tb_ProdDetails != null && EditEntity.tb_ProdDetails.Count > 0)
                    {
                        //加载属性
                        //去掉临时添加的关系数据，因为要重新加载
                        EditEntity.tb_Prod_Attr_Relations.RemoveWhere(c => c.RAR_ID == 0);
                        EditEntity.tb_Prod_Attr_Relations.RemoveWhere(c => c.ActionStatus == ActionStatus.新增);
                        foreach (var detail in EditEntity.tb_ProdDetails)
                        {
                            detail.tb_Prod_Attr_Relations.RemoveWhere(c => c.RAR_ID == 0);
                            detail.tb_Prod_Attr_Relations.RemoveWhere(c => c.ActionStatus == ActionStatus.新增);
                            foreach (var relation in detail.tb_Prod_Attr_Relations)
                            {
                                relation.tb_prodproperty = null;
                                relation.tb_prodpropertyvalue = null;
                                relation.Property_ID = null;
                                relation.PropertyValueID = null;
                            }
                        }
                        LoadTreeGridItems(EditEntity);
                    }
                    #endregion
                    return;
                }

                // 获取当前已有的属性组合
                var existingCombinations = GetExistingAttributeCombinations();

                // 获取选中的属性组ID列表(所有有勾选的）
                var selectedGroupIds = selectedAttributeGroups.Select(g => g.Property.Property_ID).ToList();

                // 获取现有属性组合中使用的属性组ID列表
                var existingGroupIds = new HashSet<long>();
                foreach (var combination in existingCombinations)
                {
                    foreach (var prop in combination.Properties)
                    {
                        if (prop.Property != null)
                        {
                            existingGroupIds.Add(prop.Property.Property_ID);
                        }
                    }
                }

                // 判断是否是添加全新属性
                bool isAddingNewProperty = selectedGroupIds.Except(existingGroupIds).Any();

                // 检查是否是单属性转多属性的特殊场景
                bool isSingleToMultiAttribute = EditEntity != null &&
                                               EditEntity.PropertyType == (int)ProductAttributeType.单属性 &&
                                               (ProductAttributeType)cmbPropertyType.SelectedValue == ProductAttributeType.可配置多属性;

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

                // 处理需要删除的组合 只删除了临时的，数据库的已经有的没有删除
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
                var combination = new AttributeCombination
                {
                    Properties = new List<AttributeValuePair>
                    {
                        new AttributeValuePair
                        {
                            Property = attributeGroups[0].Property,
                            PropertyValue = value
                        }
                    }
                };
                // 排序属性列表
                combination.Properties = combination.Properties
                    .OrderBy(p => p.Property != null ? p.Property.Property_ID : long.MinValue)
                    .ToList();
                combinations.Add(combination);
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

                        // 排序属性列表
                        newCombination.Properties = newCombination.Properties
                            .OrderBy(p => p.Property != null ? p.Property.Property_ID : long.MinValue)
                            .ToList();

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
                        // 排序属性列表
                        combination.Properties = combination.Properties
                            .OrderBy(p => p.Property != null ? p.Property.Property_ID : long.MinValue)
                            .ToList();

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
                                    combination.ProductDetail.tb_Prod_Attr_Relations.Remove(relation);
                                }
                            }
                            // 移除产品详情
                            EditEntity.tb_ProdDetails.Remove(combination.ProductDetail);
                            treeGridView1.Nodes.Remove(nodeToRemove);
                        }
                        // 如果是已存在的产品详情，则处理关系子节点 即属性值的节点
                        else
                        {

                            foreach (var Prop in combination.Properties)
                            {
                                var SubNode = nodeToRemove.Nodes.Where(c => c.Tag is tb_Prod_Attr_Relation attr_Relation
                                && attr_Relation.Property_ID == Prop.Property.Property_ID
                                && attr_Relation.PropertyValueID == Prop.PropertyValue.PropertyValueID).FirstOrDefault();

                                if (SubNode != null)
                                {
                                    if (SubNode.Tag is tb_Prod_Attr_Relation attr_Relation)
                                    {
                                        if (attr_Relation.ActionStatus == ActionStatus.新增)
                                        {
                                            nodeToRemove.Nodes.Remove(SubNode);
                                            combination.ProductDetail.tb_Prod_Attr_Relations.Remove(attr_Relation);
                                        }
                                        else
                                        {
                                            //重置
                                            attr_Relation.tb_prodproperty = null;
                                            attr_Relation.tb_prodpropertyvalue = null;
                                        }
                                    }
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
                }
            }
        }

        /// <summary>
        /// 处理需要添加的属性组合
        /// </summary>
        /// <param name="combinationsToAdd">需要添加的组合列表</param>
        private async Task HandleCombinationsToAdd(List<AttributeCombination> combinationsToAdd)
        {
            if (combinationsToAdd.Count == 0)
            {
                return;
            }

            #region 先根据treeGrid中的数据情况给组合赋值->产品详情实体    这里是已经有一个种属性的情况下再增加其它属性的设置。
            //如果从单属性的空白到新增第一个属性时则需要另外的实现
            // 遍历TreeGrid中的产品详情节点，匹配属性组合并复制产品详情实体
            List<TreeGridNode> DetailNodes = treeGridView1.Nodes.Where(c => c.Tag is tb_ProdDetail).ToList();
            foreach (var combination in combinationsToAdd)
            {
                // 遍历TreeGrid中的产品详情节点
                foreach (var node in DetailNodes)
                {
                    if (node.Tag is tb_ProdDetail existingDetail && node.HasChildren && node.Nodes.Count > 0)
                    {
                        // 获取当前节点的属性关系
                        var existingRelations = node.Nodes
                            .Where(n => n.Tag is tb_Prod_Attr_Relation)
                            .Select(n => n.Tag as tb_Prod_Attr_Relation)
                            .ToList();

                        // 检查当前组合的属性是否与现有产品详情的属性匹配
                        // 在指定属性下 只要有一个属性值属于现有关系中，则认为这个产品就是旧产品了
                        bool isMatch = true;
                        foreach (var attrValuePair in combination.Properties)
                        {
                            // 检查现有关系中是否包含当前组合的每个属性值对
                            bool foundMatch = existingRelations.Any(r =>
                                r.Property_ID == attrValuePair.Property.Property_ID &&
                                r.PropertyValueID == attrValuePair.PropertyValue.PropertyValueID);
                            if (!foundMatch)
                            {
                                isMatch = false;
                                break;
                            }
                        }

                        // 如果属性匹配，并且现有产品详情的属性数量与组合属性数量相同，则复制现有产品详情
                        if (isMatch)
                        {
                            // 复制现有产品详情实体到组合中
                            combination.ProductDetail = existingDetail;
                            break;
                        }
                    }
                }
            }
            #endregion
            bool IsOriginalNode = false;
            //所有产品的对应关系中的这个关系的属性为空时才是第一个默认的
            List<TreeGridNode> ProdDetailNodes = treeGridView1.Nodes.Where(c => c.Tag is tb_ProdDetail).ToList();
            foreach (TreeGridNode item in ProdDetailNodes)
            {
                var AttrsSubNodes = item.Nodes.Where(c => c.Tag is tb_Prod_Attr_Relation).ToList();
                foreach (TreeGridNode SubNode in AttrsSubNodes)
                {
                    if (SubNode.Tag is tb_Prod_Attr_Relation relation)
                    {
                        if (relation.tb_prodproperty == null)
                        {
                            IsOriginalNode = true;
                            break;
                        }
                    }
                }
                if (IsOriginalNode)
                {
                    break;
                }
            }

            //说明还没有设置成功。则认为是第一个默认的由单属性过来的
            if (combinationsToAdd.Where(c => c.ProductDetail != null).ToList().Count == 0
                && EditEntity.tb_ProdDetails.Count == 1
                && IsOriginalNode
                )
            {
                AttributeCombination combination = combinationsToAdd[0];
                combination.ProductDetail = EditEntity.tb_ProdDetails[0];
                for (int i = 0; i < combination.Properties.Count; i++)
                {
                    for (int j = 0; j < combination.ProductDetail.tb_Prod_Attr_Relations.Count; j++)
                    {
                        combination.ProductDetail.tb_Prod_Attr_Relations[j].Property_ID = combination.Properties[i].Property.Property_ID;
                        combination.ProductDetail.tb_Prod_Attr_Relations[j].PropertyValueID = combination.Properties[i].PropertyValue.PropertyValueID;
                        combination.ProductDetail.tb_Prod_Attr_Relations[j].tb_prodproperty = combination.Properties[i].Property;
                        combination.ProductDetail.tb_Prod_Attr_Relations[j].tb_prodpropertyvalue = combination.Properties[i].PropertyValue;
                    }
                }

            }





            // 创建TreeGrid节点
            Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);

            foreach (var combination in combinationsToAdd)
            {
                TreeGridNode node = null;
                //先处理产品
                if (combination.ProductDetail == null)
                {
                    // 创建新的产品详情
                    long skuRowId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                    var newDetail = new tb_ProdDetail
                    {
                        ProdBaseID = EditEntity.ProdBaseID,
                        ActionStatus = ActionStatus.新增,
                        Is_enabled = true,
                        Is_available = true,
                        Created_at = DateTime.Now,
                        Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID,
                        tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>()
                    };
                    combination.ProductDetail = newDetail;
                    #region 添加关系，因为生成SKU需要这些关系 生成更友好的编号
                    // 为每个属性值创建属性关系
                    foreach (var attrValuePair in combination.Properties)
                    {
                        //根据属性及值找到对应的关系。如果存在则不管。不存在则新增
                        tb_Prod_Attr_Relation attr_Relation = combination.ProductDetail.tb_Prod_Attr_Relations.FirstOrDefault(c => c.Property_ID == attrValuePair.Property.Property_ID && c.PropertyValueID == attrValuePair.PropertyValue.PropertyValueID);

                        if (attr_Relation != null)
                        {
                            attr_Relation.ActionStatus = ActionStatus.修改;
                        }
                        else
                        {
                            long RARId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                            var relation = new tb_Prod_Attr_Relation
                            {
                                Property_ID = attrValuePair.Property.Property_ID,
                                PropertyValueID = attrValuePair.PropertyValue.PropertyValueID,
                                ProdBaseID = EditEntity.ProdBaseID,
                                RAR_ID = RARId,
                                ProdDetailID = combination.ProductDetail?.ProdDetailID,// skuRowId,
                                ActionStatus = ActionStatus.新增,
                                tb_prodproperty = attrValuePair.Property,
                                tb_prodpropertyvalue = attrValuePair.PropertyValue
                            };
                            combination.ProductDetail.tb_Prod_Attr_Relations.Add(relation);
                        }
                    }
                    #endregion

                    BusinessHelper.Instance.EditEntity(combination.ProductDetail); // 初始化编辑信息
                    if (string.IsNullOrEmpty(combination.ProductDetail.SKU))
                    {
                        await CreateSKUAsync(combination.ProductDetail);
                    }
                    combination.ProductDetail.ProdDetailID = skuRowId; // 临时ID，保存到DB前会重新设置,保存时是根据ActionStatus

                    node = treeGridView1.Nodes.Add(skuRowId, 0, "", GetPropertiesText(combination),
                          combination.ProductDetail.SKU != null ? combination.ProductDetail.SKU : "等待生成", EditEntity.CNName, "新增", combination.ProductDetail.Is_enabled);
                    node.NodeName = skuRowId.ToString();
                    node.ImageIndex = 1; // 新增图标
                    node.Tag = combination.ProductDetail;
                    node.DefaultCellStyle.Font = boldFont;

                    // 添加到产品详情列表
                    if (!EditEntity.tb_ProdDetails.Any(c => c.ProdDetailID == combination.ProductDetail.ProdDetailID))
                    {
                        EditEntity.tb_ProdDetails.Add(combination.ProductDetail);
                    }
                }
                else
                {
                    #region 更新关系 ，下面会根据关系来添加节点
                    #region 添加关系，因为生成SKU需要这些关系 生成更友好的编号
                    // 为每个属性值创建属性关系
                    foreach (var attrValuePair in combination.Properties)
                    {
                        //根据属性及值找到对应的关系。如果存在则不管。不存在则新增
                        tb_Prod_Attr_Relation attr_Relation = combination.ProductDetail.tb_Prod_Attr_Relations.FirstOrDefault(c => c.Property_ID == attrValuePair.Property.Property_ID && c.PropertyValueID == attrValuePair.PropertyValue.PropertyValueID);

                        if (attr_Relation != null)
                        {
                            if (attr_Relation.ActionStatus != ActionStatus.新增)
                            {
                                attr_Relation.ActionStatus = ActionStatus.修改;
                            }

                        }
                        else
                        {
                            long RARId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                            var relation = new tb_Prod_Attr_Relation
                            {
                                Property_ID = attrValuePair.Property.Property_ID,
                                PropertyValueID = attrValuePair.PropertyValue.PropertyValueID,
                                ProdBaseID = EditEntity.ProdBaseID,
                                RAR_ID = RARId,
                                ProdDetailID = combination.ProductDetail?.ProdDetailID,// skuRowId,
                                ActionStatus = ActionStatus.新增,
                                tb_prodproperty = attrValuePair.Property,
                                tb_prodpropertyvalue = attrValuePair.PropertyValue
                            };
                            combination.ProductDetail.tb_Prod_Attr_Relations.Add(relation);
                        }
                    }
                    #endregion
                    if (combination.ProductDetail.ActionStatus == ActionStatus.新增)
                    {
                        BusinessHelper.Instance.InitEntity(combination.ProductDetail); // 初始化编辑信息
                    }
                    else
                    {
                        BusinessHelper.Instance.EditEntity(combination.ProductDetail); // 初始化编辑信息
                    }

                    if (string.IsNullOrEmpty(combination.ProductDetail.SKU) && combination.ProductDetail.ActionStatus == ActionStatus.新增)
                    {
                        //更新SKU
                        await CreateSKUAsync(combination.ProductDetail);
                    }

                    // 添加到产品详情列表
                    if (!EditEntity.tb_ProdDetails.Any(c => c.ProdDetailID == combination.ProductDetail.ProdDetailID))
                    {
                        EditEntity.tb_ProdDetails.Add(combination.ProductDetail);
                    }

                    #endregion


                    node = treeGridView1.Nodes.FirstOrDefault(c => c.NodeName == combination.ProductDetail.ProdDetailID.ToString());
                    if (node != null)
                    {
                        node.Cells[3].Value = GetPropertiesText(combination);
                        node.Tag = combination.ProductDetail;
                        if (combination.ProductDetail.ActionStatus == ActionStatus.新增)
                        {
                            //
                            node.ImageIndex = 1; // 新增图标
                        }
                    }
                }












                // 为每个属性值创建属性关系后  添加节点
                foreach (var attrValuePair in combination.Properties)
                {
                    foreach (var DetailRelation in combination.ProductDetail.tb_Prod_Attr_Relations)
                    {

                        // 创建子节点
                        TreeGridNode subNode = node.Nodes.FirstOrDefault(c => c.NodeName == DetailRelation.RAR_ID.ToString());
                        if (subNode == null)
                        {
                            subNode = node.Nodes.Add(DetailRelation.RAR_ID, DetailRelation.RAR_ID, DetailRelation.tb_prodproperty.PropertyName, DetailRelation.tb_prodpropertyvalue.PropertyValueName, "", "", "新增");
                            subNode.NodeName = DetailRelation.RAR_ID.ToString();
                            subNode.Tag = DetailRelation;
                            subNode.ImageIndex = 1; // 新增图标
                        }
                        else
                        {
                            //更新
                            //如果由单属性转换过来的，第一个则为默认的

                            subNode.Cells[2].Value = DetailRelation.tb_prodproperty.PropertyName;
                            subNode.Cells[3].Value = DetailRelation.tb_prodpropertyvalue.PropertyValueName;
                            if (DetailRelation.ActionStatus == ActionStatus.加载)
                            {
                                subNode.Cells[6].Value = "编辑";
                                subNode.ImageIndex = 2; // 编辑图标  
                            }
                            if (DetailRelation.ActionStatus == ActionStatus.修改)
                            {
                                subNode.Cells[6].Value = "编辑";
                                subNode.ImageIndex = 2; // 编辑图标  
                            }

                            subNode.Tag = DetailRelation;
                            //subNode.
                        }

                    }



                }
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
            cmbPropertyType.Enabled = true;
            cmb属性.Enabled = false;
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
                    //去掉临时添加的关系数据，因为要重新加载
                    EditEntity.tb_ProdDetails.RemoveWhere(c => c.ProdDetailID == 0);
                    EditEntity.tb_ProdDetails.RemoveWhere(c => c.ActionStatus == ActionStatus.新增);
                    if (EditEntity.tb_ProdDetails != null && EditEntity.tb_ProdDetails.Count > 0)
                    {
                        listView1.Clear();

                        //加载属性
                        //去掉临时添加的关系数据，因为要重新加载
                        EditEntity.tb_Prod_Attr_Relations.RemoveWhere(c => c.RAR_ID == 0);
                        EditEntity.tb_Prod_Attr_Relations.RemoveWhere(c => c.ActionStatus == ActionStatus.新增);
                        foreach (var detail in EditEntity.tb_ProdDetails)
                        {
                            detail.tb_Prod_Attr_Relations.RemoveWhere(c => c.RAR_ID == 0);
                            detail.tb_Prod_Attr_Relations.RemoveWhere(c => c.ActionStatus == ActionStatus.新增);
                        }

                        LoadTreeGridItems(EditEntity);

                        //加载属性类型，再加载属性及对应的值
                        cmbPropertyType.SelectedValue = EditEntity.PropertyType;

                        //加载动态属性区域 根据数据库的值加载控件及选中属性
                        foreach (var detail in EditEntity.tb_ProdDetails)
                        {
                            //如果是单属性则属性值不能为空
                            var propGroup = detail.tb_Prod_Attr_Relations.Where(c => c.Property_ID.HasValue).GroupBy(c => c.Property_ID.Value);
                            foreach (var item in propGroup)
                            {
                                var prop = prodpropList.FirstOrDefault(c => c.Property_ID == item.Key);
                                if (prop != null)
                                {
                                    AddProperty(prop);
                                }
                            }
                        }

                        //加载属性值后  不可以再修改选中状态。
                        //循环所有属性，如果有值则选中并且不可以修改
                        if (EditEntity.PropertyType == (int)ProductAttributeType.可配置多属性)//要多属性的时候才需要
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
                    TreeGridNode node = treeGridView1.Nodes.Add(item.ProdDetailID, item.ProdDetailID, "", DisplayPropText, item.SKU, viewProdDetail.CNName, "加载", item.Is_enabled);
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
                            subnode.ImageIndex = 0;
                            subnode.Tag = par;
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
                            // 检查是否从单属性转换为多属性
                            if (EditEntity != null && EditEntity.PropertyType == (int)ProductAttributeType.单属性)
                            {
                                // 显示转换提示对话框
                                DialogResult result = MessageBox.Show("将产品从单属性转换为多属性将创建多个产品详情记录。系统会将第一个属性值默认分配给原始单属性产品详情记录。\n\n是否继续？",
                                    "属性类型转换提示",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information);

                                if (result == DialogResult.No)
                                {
                                    // 如果用户取消，恢复原属性类型选择
                                    cmbPropertyType.SelectedValue = (int)ProductAttributeType.单属性;
                                    return;
                                }
                            }

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
                                        int counter = await MainForm.Instance.AppContext.Db.Deleteable(Prod_Attr_Relation).ExecuteCommandAsync();
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

                        //编辑的是关系，
                        TreeGridNode node = treeGridView1.Nodes.FirstOrDefault(c => c.NodeName == relation.ProdDetailID.ToString());
                        if (node != null)
                        {
                            // 创建子节点
                            TreeGridNode subNode = node.Nodes.FirstOrDefault(c => c.NodeName == relation.RAR_ID.ToString());
                            if (subNode != null)
                            {
                                subNode.NodeName = relation.RAR_ID.ToString();

                                subNode.ImageIndex = 2; // 新增图标

                                var ProdProperty = CacheManager.GetEntity<tb_ProdProperty>(relation.Property_ID);
                                if (ProdProperty != null)
                                {
                                    subNode.Cells[2].Value = ProdProperty.PropertyName;
                                }
                                var ProdPropertyValue = CacheManager.GetEntity<tb_ProdPropertyValue>(relation.PropertyValueID);
                                if (ProdPropertyValue != null)
                                {
                                    subNode.Cells[3].Value = ProdPropertyValue.PropertyValueName;
                                }
                                relation.tb_prodproperty = ProdProperty;
                                relation.tb_prodpropertyvalue = ProdPropertyValue;
                                subNode.Tag = relation;

                            }

                            var attr_Relations = new List<tb_Prod_Attr_Relation>();

                            foreach (var item in node.Nodes)
                            {
                                if (item.Tag is tb_Prod_Attr_Relation _Attr_Relation)
                                {
                                    attr_Relations.Add(_Attr_Relation);
                                }
                            }

                            string DisplayPropText = string.Empty;
                            var array = attr_Relations
                                .OrderBy(c => c.Property_ID) // 按属性 ID 排序
                                .ToList().Select(c => c.tb_prodpropertyvalue.PropertyValueName).ToArray();
                            DisplayPropText = string.Join(",", array);

                            node.Cells[3].Value = DisplayPropText;
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
