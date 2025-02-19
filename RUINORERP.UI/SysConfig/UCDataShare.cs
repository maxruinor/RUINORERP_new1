using AutoUpdateTools;
using CacheManager.Core;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using HLH.Lib.Helper;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows.Forms;
using System.Windows.Media;
using AutoMapper;
using FastReport.Barcode;

using FastReport.Utils;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;

using OfficeOpenXml;
using Org.BouncyCastle.Asn1.X509.Qualified;
using RUINOR.Core;
using RUINORERP.AutoMapper;

using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;

using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Models;

using RUINORERP.UI.FormProperty;

using RUINORERP.UI.UControls;
using RUINORERP.UI.UCSourceGrid;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserPersonalized;
using SqlSugar;

using System.Collections;
using System.Collections.Concurrent;

using System.IO;

using System.Linq.Dynamic.Core;

using System.Runtime.InteropServices;
using System.Security.Policy;

using System.Web.UI.WebControls;

using System.Xml;
using CommonHelper = RUINORERP.UI.Common.CommonHelper;
using XmlDocument = System.Xml.XmlDocument;
using NPOI.SS.Formula.Functions;
using Image = System.Drawing.Image;
using ICSharpCode.SharpZipLib.Zip;
using MathNet.Numerics.LinearAlgebra.Factorization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using FluentValidation;
using FluentValidation.Results;
using System.Windows.Documents;
using Org.BouncyCastle.Utilities;
using SixLabors.ImageSharp.Processing;



namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("数据分享", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataShare : UserControl
    {
        public UCDataShare()
        {
            InitializeComponent();
            ColNameDataDictionary.TryAdd(nameof(DataStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));
            foreach (var item in BaseToolStrip.Items)
            {
                if (item is ToolStripButton)
                {
                    ToolStripButton subItem = item as ToolStripButton;
                    subItem.Click += Item_Click;
                    UIHelper.ControlButton(CurMenuInfo, subItem);
                }
                if (item is ToolStripDropDownButton subItemDr)
                {
                    UIHelper.ControlButton(CurMenuInfo, subItemDr);
                    subItemDr.Click += Item_Click;
                    //下一级
                    if (subItemDr.HasDropDownItems)
                    {
                        foreach (var sub in subItemDr.DropDownItems)
                        {
                            ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                            UIHelper.ControlButton(CurMenuInfo, subStripMenuItem);
                            subStripMenuItem.Click += Item_Click;
                        }
                    }
                }
                if (item is ToolStripSplitButton)
                {
                    ToolStripSplitButton subItem = item as ToolStripSplitButton;
                    subItem.Click += Item_Click;
                    UIHelper.ControlButton(CurMenuInfo, subItem);
                    //下一级
                    if (subItem.HasDropDownItems)
                    {
                        foreach (var sub in subItem.DropDownItems)
                        {
                            ToolStripItem subStripMenuItem = sub as ToolStripItem;
                            subStripMenuItem.Click += Item_Click;
                        }
                    }
                }
            }


            foreach (var item in toolStripImport.Items)
            {
                if (item is ToolStripButton)
                {
                    ToolStripButton subItem = item as ToolStripButton;
                    subItem.Click += Item_Click;
                    UIHelper.ControlButton(CurMenuInfo, subItem);
                }
                if (item is ToolStripDropDownButton subItemDr)
                {
                    UIHelper.ControlButton(CurMenuInfo, subItemDr);
                    subItemDr.Click += Item_Click;
                    //下一级
                    if (subItemDr.HasDropDownItems)
                    {
                        foreach (var sub in subItemDr.DropDownItems)
                        {
                            ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                            UIHelper.ControlButton(CurMenuInfo, subStripMenuItem);
                            subStripMenuItem.Click += Item_Click;
                        }
                    }
                }
                if (item is ToolStripSplitButton)
                {
                    ToolStripSplitButton subItem = item as ToolStripSplitButton;
                    subItem.Click += Item_Click;
                    UIHelper.ControlButton(CurMenuInfo, subItem);
                    //下一级
                    if (subItem.HasDropDownItems)
                    {
                        foreach (var sub in subItem.DropDownItems)
                        {
                            ToolStripItem subStripMenuItem = sub as ToolStripItem;
                            subStripMenuItem.Click += Item_Click;
                        }
                    }
                }
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            DoButtonClick(RUINORERP.Common.Helper.EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
        }

        private void 请求缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                UIBizSrvice.RequestCache(tableName);
            }
        }

        private void UCDataShare_Load(object sender, EventArgs e)
        {


            LoadShareDataListTableToUI();

            BindData(TempProd);
        }

        public BaseEntity QueryDto { get; set; }
        private QueryFilter _QueryConditionFilter = new QueryFilter();

        /// <summary>
        /// 查询条件  将来 把querypara优化掉
        /// </summary>
        public QueryFilter QueryConditionFilter { get => _QueryConditionFilter; set => _QueryConditionFilter = value; }

        public tb_MenuInfo CurMenuInfo { get; set; } = new tb_MenuInfo();

        /// <summary>
        /// 生成查询条件，并返回查询条件代理实体参数
        /// </summary>
        /// <param name="useLike">true：默认不是模糊查询</param>
        private object LoadQueryConditionToUI(Type targetType)
        {
            //相关菜单  如果不是泛型的 就直接用类的路径
            if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
            {
                CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.ClassPath == this.ToString()).FirstOrDefault();
            }

            kryptonPanelOut.Controls.Clear();
            Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器 = kryptonPanelOut;
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(targetType.Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            {
                QueryDto = UIGenerateHelper.CreateQueryUI(targetType, true, kryptonPanel条件生成容器, QueryConditionFilter, 4);
            }
            else
            {
                tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
                if (menuSetting != null)
                {
                    QueryDto = UIGenerateHelper.CreateQueryUI(targetType, true, kryptonPanel条件生成容器, QueryConditionFilter, menuSetting);
                }
                else
                {
                    QueryDto = UIGenerateHelper.CreateQueryUI(targetType, true, kryptonPanel条件生成容器, QueryConditionFilter, 4);
                }
            }

            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
            return QueryDto;
        }



        List<KeyValue> ShareDataList = new List<KeyValue>();
        private void LoadShareDataListTableToUI()
        {
            KeyValue kvList = new KeyValue();
            kvList.Key = "产品数据";
            kvList.Value = typeof(tb_Prod);
            ShareDataList.Add(kvList);

            KeyValue kvCategories = new KeyValue();
            kvCategories.Key = "类目数据";
            kvCategories.Value = typeof(tb_ProdCategories);
            ShareDataList.Add(kvCategories);

            listBoxTableList.Items.Clear();

            foreach (var tableName in ShareDataList)
            {
                SuperValue kv = new SuperValue(tableName.Key.ToString(), tableName.Value);
                listBoxTableList.Items.Add(kv);
            }
            //默认加载第一个类型的查询条件
            if (listBoxTableList.Items.Count > 0)
            {
                var kv = listBoxTableList.Items[0] as SuperValue;
                string tableName = kv.superDataTypeName;
                LoadQueryConditionToUI(kv.Tag as Type);
            }
        }



        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                LoadQueryConditionToUI(kv.Tag as Type);
            }
            else
            {
                newSumDataGridViewOut.DataSource = null;
                return;
            }

        }

        private void 清空选中缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                BizCacheHelper.Manager.CacheEntityList.Remove(tableName);
                CacheInfo lastCacheInfo = new CacheInfo(tableName, 0);
                lastCacheInfo.HasExpire = false;
                //看是更新好。还是移除好。主要看后面的更新机制。
                MyCacheManager.Instance.CacheInfoList.AddOrUpdate(tableName, lastCacheInfo, c => lastCacheInfo);


            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {



        }


        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            MainForm.Instance.AppContext.log.ActionName = menuItem.ToString();
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {

            }
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {

                case MenuItemEnums.查询:
                    Query(QueryDto);
                    break;

                case MenuItemEnums.关闭:
                    Exit(this);
                    break;

                case MenuItemEnums.保存:
                    SaveShareData();
                    break;

                case MenuItemEnums.属性:
                    //Property();
                    break;
                case MenuItemEnums.导入:

                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ImportDataList = new List<tb_Prod>();
                        List<tb_Prod> Prods = DataImportExportManager.ImportDataAsync(openFileDialog.FileName);

                        entityType = typeof(tb_Prod);
                        newSumDataGridViewImport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        newSumDataGridViewImport.XmlFileName = this.Name + "tb_Prod" + "ImportDataShare";
                        newSumDataGridViewImport.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_Prod));

                        KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                        newSumDataGridViewImport.FieldNameList.TryRemove("ProdDetailID", out kv);
                        newSumDataGridViewImport.FieldNameList.TryRemove("Images", out kv);

                        newSumDataGridViewImport.NeedSaveColumnsXml = true;
                        bindingSourceOut.DataSource = Prods.ToBindingSortCollection();
                        newSumDataGridViewImport.DataSource = bindingSourceOut;
                        ImportDataList = Prods;
                    }

                    break;
                case MenuItemEnums.导出:

                    var list = bindingSourceOut.DataSource as BindingSortCollection<tb_Prod>;
                    if (list == null)
                    {
                        return;
                    }
                    ExportShareData(list);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 导出分享数据到文件。
        /// </summary>
        /// <param name="list"></param>
        private async void ExportShareData(BindingSortCollection<tb_Prod> list)
        {
            List<long> ids = new List<long>();
            //找到导出主产品的明细中的BOM中的bom明细对应的详情的主产品的ID。
            foreach (var item in list)
            {
                if (item.tb_ProdDetails != null)
                {
                    foreach (var detail in item.tb_ProdDetails)
                    {
                        if (detail.tb_bom_s != null)
                        {
                            if (detail.tb_bom_s.tb_BOM_SDetails == null)
                            {
                                foreach (var detail2 in detail.tb_bom_s.tb_BOM_SDetails)
                                {
                                    if (detail2.tb_proddetail != null)
                                    {
                                        if (detail2.tb_proddetail.tb_prod == null)
                                        {
                                            if (!ids.Any(c => c == detail2.tb_proddetail.tb_prod.ProdBaseID))
                                            {
                                                ids.Add(detail2.tb_proddetail.tb_prod.ProdBaseID);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            //这里提取原始导出数据中的BOM明细的主产品信息
            List<tb_Prod> bomPordList = await MainForm.Instance.AppContext.Db.Queryable<tb_Prod>()
            .Includes(a => a.tb_unit)
            .Includes(b => b.tb_producttype)
             .AsNavQueryable()
            .Includes(a => a.tb_prodcategories, b => b.tb_prodcategories_parent, c => c.tb_prodcategories_parent,
            d => d.tb_prodcategories_parent, e => e.tb_prodcategories_parent, f => f.tb_prodcategories_parent)
            .Includes(b => b.tb_storagerack)
            .Includes(a => a.tb_location)
            .Includes(a => a.tb_Packings, b => b.tb_PackingDetails)
            .Includes(b => b.tb_Prod_Attr_Relations, c => c.tb_prodproperty)
            .Includes(b => b.tb_Prod_Attr_Relations, c => c.tb_prodpropertyvalue)
            .Includes(b => b.tb_Prod_Attr_Relations, c => c.tb_proddetail)
            // .Includes(b => b.tb_ProdDetails, c => c.tb_BOM_Ss, d => d.tb_BOM_SDetailSecondaries)

            .AsNavQueryable()
            .Includes(b => b.tb_ProdDetails, c => c.tb_BOM_Ss, d => d.tb_BOM_SDetails, e => e.tb_unit)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_target)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_source)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_unit)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_producttype)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_prodcategories, g => g.tb_prodcategories_parent)//bom主

            .AsNavQueryable()
            .Includes(b => b.tb_ProdDetails, c => c.tb_bom_s, d => d.tb_BOM_SDetails, e => e.tb_unit)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_target)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_source)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_unit)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_producttype)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_prodcategories, g => g.tb_prodcategories_parent)//bom主

              .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodproperty)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodpropertyvalue)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_proddetail)


            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodproperty)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodpropertyvalue)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_proddetail)


            .Where(m => ids.ToArray().Contains(m.ProdBaseID)).ToListAsync() as List<tb_Prod>;


            List<tb_Prod> ProdList = new List<tb_Prod>();
            foreach (var item in bomPordList)
            {
                ProdList.Add(item);
            }
            foreach (var item in list)
            {
                ProdList.Add(item);
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataImportExportManager.ExportDataAsync(ProdList, saveFileDialog.FileName);
            }
        }

        public bool ShowInvalidMessage(ValidationResult results)
        {
            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }
            if (!results.IsValid)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return results.IsValid;
        }

        private tb_Prod TempProd { get; set; } = new tb_Prod();


        private List<tb_Prod> ImportDataList = new List<tb_Prod>();

        /// <summary>
        /// 保存数据 去掉了价格和供应商
        /// </summary>
        private async void SaveShareData()
        {
            var validator = new Temptb_ProdValidator();
            var result = validator.Validate(TempProd);
            bool vd = ShowInvalidMessage(result);
            if (!result.IsValid)
            {
                return;
            }

            if (bindingSourceOut.DataSource != null)
            {
                //BindingSortCollection<tb_Prod> bcProdDetailList = bindingSourceOut.DataSource as BindingSortCollection<tb_Prod>;
                //bcProdDetailList.ForEach(
                //    c =>
                //    {
                //        c.Category_ID = TempProd.Category_ID;
                //        c.Employee_ID = null;
                //        c.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                //        c.Rack_ID = TempProd.Rack_ID;
                //        c.Location_ID = TempProd.Location_ID;
                //        c.DepartmentID = TempProd.DepartmentID;
                //    }
                //);

                ImportDataList.ForEach(
                    c =>
                    {
                        //c.Category_ID = TempProd.Category_ID;
                        c.Employee_ID = null;
                        c.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                        c.Created_at = DateTime.Now;
                        c.Rack_ID = TempProd.Rack_ID;
                        c.Location_ID = TempProd.Location_ID;
                        c.DepartmentID = TempProd.DepartmentID;
                    }
                );
                //先要添加的有 单位，产品类型，产品类目(父类），库位，属性，属性值
                List<tb_Unit> unitList = new List<tb_Unit>();
                List<tb_ProductType> productTypeList = new List<tb_ProductType>();
                List<tb_ProdCategories> prodcategoriesList = new List<tb_ProdCategories>();
                List<tb_ProdProperty> ProdPropertyList = new List<tb_ProdProperty>();
                List<tb_ProdPropertyValue> ProdPropertyValueList = new List<tb_ProdPropertyValue>();
                List<tb_Prod_Attr_Relation> attr_RelationsList = new List<tb_Prod_Attr_Relation>();

                List<tb_BOM_S> MainbomList = new List<tb_BOM_S>();

                List<tb_BOM_SDetail> DetailbomList = new List<tb_BOM_SDetail>();
                //来自于bom明细的主产品信息
                List<tb_Prod> ProdList = new List<tb_Prod>();

                //来自于bom明细的明细产品信息
                List<tb_ProdDetail> ProdDetailList = new List<tb_ProdDetail>();


                foreach (var item in ImportDataList)
                {
                    if (item.tb_unit != null && !unitList.Exists(x => x.Unit_ID == item.tb_unit.Unit_ID))
                    {
                        unitList.Add(item.tb_unit);
                    }
                    if (item.tb_producttype != null && !productTypeList.Exists(x => x.Type_ID == item.tb_producttype.Type_ID))
                    {
                        productTypeList.Add(item.tb_producttype);
                    }
                    if (item.tb_prodcategories != null && !prodcategoriesList.Any(x => x.Category_ID == item.tb_prodcategories.Category_ID))
                    {
                        prodcategoriesList.Add(item.tb_prodcategories);
                        if (item.tb_prodcategories.tb_ProdCategorieses_parents != null && item.tb_prodcategories.tb_ProdCategorieses_parents.Count > 0)
                        {
                            item.tb_prodcategories.tb_ProdCategorieses_parents.ForEach(c =>
                            {
                                if (!prodcategoriesList.Any(x => x.Category_ID == c.tb_prodcategories_parent.Category_ID))
                                {
                                    prodcategoriesList.Add(c.tb_prodcategories_parent);
                                }

                                #region 再判断一级或用循环来判断？

                                if (c.tb_prodcategories_parent.tb_ProdCategorieses_parents != null && c.tb_prodcategories_parent.tb_ProdCategorieses_parents.Count > 0)
                                {
                                    c.tb_prodcategories_parent.tb_ProdCategorieses_parents.ForEach(c =>
                                    {
                                        if (!prodcategoriesList.Any(x => x.Category_ID == c.tb_prodcategories_parent.Category_ID))
                                        {
                                            prodcategoriesList.Add(c.tb_prodcategories_parent);
                                        }
                                    }
                                    );
                                }

                                #endregion
                            }
                            );
                        }

                    }
                    if (item.tb_Prod_Attr_Relations != null)
                    {
                        item.tb_Prod_Attr_Relations.ForEach(c =>
                        {
                            if (c.tb_prodproperty != null && !ProdPropertyList.Any(x => x.Property_ID == c.tb_prodproperty.Property_ID))
                            {
                                ProdPropertyList.Add(c.tb_prodproperty);
                            }
                            if (c.tb_prodpropertyvalue != null && !ProdPropertyValueList.Any(x => x.PropertyValueID == c.tb_prodpropertyvalue.PropertyValueID))
                            {
                                ProdPropertyValueList.Add(c.tb_prodpropertyvalue);
                            }
                            if (!attr_RelationsList.Any(r => r.RAR_ID == c.RAR_ID))
                            {
                                attr_RelationsList.Add(c);
                            }
                            if (c.tb_prod != null && !ProdList.Any(x => x.ProdBaseID == c.tb_prod.ProdBaseID))
                            {
                                ProdList.Add(c.tb_prod);
                            }
                            if (c.tb_proddetail != null && !ProdDetailList.Any(x => x.ProdDetailID == c.tb_proddetail.ProdDetailID))
                            {
                                ProdDetailList.Add(c.tb_proddetail);
                            }
                        });
                    }
                    if (item.tb_ProdDetails != null)
                    {
                        item.tb_ProdDetails.ForEach(c =>
                        {
                            c.BOM_ID = null;
                            c.Discount_Price = 0;
                            c.Market_Price = 0;
                            c.Standard_Price = 0;
                            c.tb_PriceRecords = null;
                            c.Created_at = System.DateTime.Now;
                            c.Created_by = TempProd.Created_by;
                            #region  根据tb_bom_s添加
                            if (c.tb_bom_s != null)
                            {
                                if (!MainbomList.Any(x => x.BOM_ID == c.tb_bom_s.BOM_ID))
                                {
                                    c.tb_bom_s.Created_at = System.DateTime.Now;
                                    c.tb_bom_s.Created_by = TempProd.Created_by;
                                    MainbomList.Add(c.tb_bom_s);
                                }

                                if (c.tb_bom_s.tb_BOM_SDetails != null)
                                {
                                    c.tb_bom_s.tb_BOM_SDetails.ForEach(x =>
                                    {
                                        if (x != null)
                                        {
                                            if (!DetailbomList.Any(Y => Y.SubID == x.SubID))
                                            {
                                                if (x.IsKeyMaterial == null)
                                                {
                                                    x.IsKeyMaterial = true;
                                                }
                                                DetailbomList.Add(x);

                                                #region  添加单位
                                                if (x.tb_unit != null
                                                && !unitList.Any(Y => Y.Unit_ID == x.tb_unit.Unit_ID))
                                                {
                                                    unitList.Add(x.tb_unit);
                                                }
                                                #endregion
                                            }
                                            if (x.tb_proddetail != null && !ProdDetailList.Any(Y => Y.ProdDetailID == x.tb_proddetail.ProdDetailID))
                                            {
                                                x.tb_proddetail.BOM_ID = null;
                                                x.tb_proddetail.Discount_Price = 0;
                                                x.tb_proddetail.Market_Price = 0;
                                                x.tb_proddetail.Standard_Price = 0;
                                                x.tb_proddetail.tb_PriceRecords = null;
                                                x.tb_proddetail.Created_at = System.DateTime.Now;
                                                x.tb_proddetail.Created_by = TempProd.Created_by;
                                                ProdDetailList.Add(x.tb_proddetail);
                                                if (x.tb_proddetail.tb_prod != null && !ProdList.Any(Y => Y.ProdBaseID == x.tb_proddetail.tb_prod.ProdBaseID))
                                                {
                                                    x.tb_proddetail.tb_prod.Created_at = System.DateTime.Now;
                                                    x.tb_proddetail.tb_prod.Created_by = TempProd.Created_by;
                                                    x.tb_proddetail.tb_prod.CustomerVendor_ID = null;
                                                    x.tb_proddetail.tb_prod.DepartmentID = TempProd.DepartmentID;
                                                    x.tb_proddetail.tb_prod.Rack_ID = TempProd.Rack_ID;
                                                    x.tb_proddetail.tb_prod.Location_ID = TempProd.Location_ID;
                                                    ProdList.Add(x.tb_proddetail.tb_prod);


                                                    if (x.tb_proddetail.tb_prod.tb_producttype != null && !productTypeList.Exists(p => p.Type_ID == x.tb_proddetail.tb_prod.tb_producttype.Type_ID))
                                                    {
                                                        productTypeList.Add(x.tb_proddetail.tb_prod.tb_producttype);
                                                    }


                                                    if (x.tb_proddetail.tb_prod.tb_prodcategories != null && !prodcategoriesList.Any(Y => Y.Category_ID == x.tb_proddetail.tb_prod.tb_prodcategories.Category_ID))
                                                    {
                                                        prodcategoriesList.Add(x.tb_proddetail.tb_prod.tb_prodcategories);
                                                        #region  添加父类目
                                                        if (x.tb_proddetail.tb_prod.tb_prodcategories.tb_prodcategories_parent != null
                                                        && !prodcategoriesList.Any(Y => Y.Category_ID == x.tb_proddetail.tb_prod.tb_prodcategories.tb_prodcategories_parent.Category_ID))
                                                        {
                                                            prodcategoriesList.Add(x.tb_proddetail.tb_prod.tb_prodcategories.tb_prodcategories_parent);
                                                        }
                                                        #endregion
                                                    }
                                                    #region  添加单位
                                                    if (x.tb_proddetail.tb_prod.tb_unit != null
                                                    && !unitList.Any(Y => Y.Unit_ID == x.tb_proddetail.tb_prod.tb_unit.Unit_ID))
                                                    {
                                                        unitList.Add(x.tb_proddetail.tb_prod.tb_unit);
                                                    }
                                                    #endregion

                                                    #region  添加产品类型
                                                    if (x.tb_proddetail.tb_prod.tb_producttype != null
                                                    && !productTypeList.Any(Y => Y.Type_ID == x.tb_proddetail.tb_prod.tb_producttype.Type_ID))
                                                    {
                                                        productTypeList.Add(x.tb_proddetail.tb_prod.tb_producttype);
                                                    }
                                                    #endregion

                                                    #region  添加单位
                                                    if (x.tb_proddetail.tb_prod.tb_unit != null
                                                    && !unitList.Any(Y => Y.Unit_ID == x.tb_proddetail.tb_prod.tb_unit.Unit_ID))
                                                    {
                                                        unitList.Add(x.tb_proddetail.tb_prod.tb_unit);
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }

                                    });
                                }
                            }
                            #endregion

                            if (c.tb_BOM_Ss != null)
                            {
                                c.tb_BOM_Ss.ForEach(t =>
                                {


                                    #region 根据tb_BOM_Ss添加
                                    if (!MainbomList.Any(x => x.BOM_ID == t.BOM_ID))
                                    {
                                        t.Created_at = System.DateTime.Now;
                                        t.Created_by = TempProd.Created_by;
                                        MainbomList.Add(t);
                                    }

                                    if (t.tb_BOM_SDetails != null)
                                    {
                                        t.tb_BOM_SDetails.ForEach(x =>
                                        {
                                            if (x != null)
                                            {
                                                if (!DetailbomList.Any(Y => Y.SubID == x.SubID))
                                                {
                                                    if (x.IsKeyMaterial == null)
                                                    {
                                                        x.IsKeyMaterial = true;
                                                    }
                                                    DetailbomList.Add(x);

                                                    #region  添加单位
                                                    if (x.tb_unit != null
                                                    && !unitList.Any(Y => Y.Unit_ID == x.tb_unit.Unit_ID))
                                                    {
                                                        unitList.Add(x.tb_unit);
                                                    }
                                                    #endregion
                                                }
                                                if (x.tb_proddetail != null && !ProdDetailList.Any(Y => Y.ProdDetailID == x.tb_proddetail.ProdDetailID))
                                                {
                                                    x.tb_proddetail.BOM_ID = null;
                                                    x.tb_proddetail.Discount_Price = 0;
                                                    x.tb_proddetail.Market_Price = 0;
                                                    x.tb_proddetail.Standard_Price = 0;
                                                    x.tb_proddetail.tb_PriceRecords = null;
                                                    x.tb_proddetail.Created_at = System.DateTime.Now;
                                                    x.tb_proddetail.Created_by = TempProd.Created_by;
                                                    ProdDetailList.Add(x.tb_proddetail);
                                                    if (x.tb_proddetail.tb_prod != null && !ProdList.Any(Y => Y.ProdBaseID == x.tb_proddetail.tb_prod.ProdBaseID))
                                                    {

                                                        #region 添加主产品信息
                                                        if (x.tb_proddetail.tb_prod != null)
                                                        {
                                                            if (!ProdList.Any(p => p.ProdBaseID == x.tb_proddetail.tb_prod.ProdBaseID))
                                                            {
                                                                x.tb_proddetail.tb_prod.Created_at = System.DateTime.Now;
                                                                x.tb_proddetail.tb_prod.Created_by = TempProd.Created_by;
                                                                x.tb_proddetail.tb_prod.CustomerVendor_ID = null;
                                                                x.tb_proddetail.tb_prod.DepartmentID = TempProd.DepartmentID;
                                                                x.tb_proddetail.tb_prod.Rack_ID = TempProd.Rack_ID;
                                                                x.tb_proddetail.tb_prod.Location_ID = TempProd.Location_ID;
                                                                ProdList.Add(x.tb_proddetail.tb_prod);

                                                                if (x.tb_proddetail.tb_prod.tb_prodcategories != null && !prodcategoriesList.Any(Y => Y.Category_ID == x.tb_proddetail.tb_prod.tb_prodcategories.Category_ID))
                                                                {
                                                                    prodcategoriesList.Add(x.tb_proddetail.tb_prod.tb_prodcategories);
                                                                }
                                                            }
                                                        }

                                                        #endregion

                                                        x.tb_proddetail.tb_prod.Created_at = System.DateTime.Now;
                                                        x.tb_proddetail.tb_prod.Created_by = TempProd.Created_by;
                                                        x.tb_proddetail.tb_prod.CustomerVendor_ID = null;
                                                        x.tb_proddetail.tb_prod.DepartmentID = TempProd.DepartmentID;
                                                        x.tb_proddetail.tb_prod.Rack_ID = TempProd.Rack_ID;
                                                        x.tb_proddetail.tb_prod.Location_ID = TempProd.Location_ID;
                                                        ProdList.Add(x.tb_proddetail.tb_prod);
                                                        if (x.tb_proddetail.tb_prod.tb_prodcategories != null && !prodcategoriesList.Any(Y => Y.Category_ID == x.tb_proddetail.tb_prod.tb_prodcategories.Category_ID))
                                                        {
                                                            prodcategoriesList.Add(x.tb_proddetail.tb_prod.tb_prodcategories);
                                                            #region  添加父类目
                                                            if (x.tb_proddetail.tb_prod.tb_prodcategories.tb_prodcategories_parent != null
                                                            && !prodcategoriesList.Any(Y => Y.Category_ID == x.tb_proddetail.tb_prod.tb_prodcategories.tb_prodcategories_parent.Category_ID))
                                                            {
                                                                prodcategoriesList.Add(x.tb_proddetail.tb_prod.tb_prodcategories.tb_prodcategories_parent);
                                                            }
                                                            #endregion
                                                        }
                                                        #region  添加单位
                                                        if (x.tb_proddetail.tb_prod.tb_unit != null
                                                        && !unitList.Any(Y => Y.Unit_ID == x.tb_proddetail.tb_prod.tb_unit.Unit_ID))
                                                        {
                                                            unitList.Add(x.tb_proddetail.tb_prod.tb_unit);
                                                        }
                                                        #endregion

                                                        #region  添加产品类型
                                                        if (x.tb_proddetail.tb_prod.tb_producttype != null
                                                        && !productTypeList.Any(Y => Y.Type_ID == x.tb_proddetail.tb_prod.tb_producttype.Type_ID))
                                                        {
                                                            productTypeList.Add(x.tb_proddetail.tb_prod.tb_producttype);
                                                        }
                                                        #endregion

                                                        #region  添加单位
                                                        if (x.tb_proddetail.tb_prod.tb_unit != null
                                                        && !unitList.Any(Y => Y.Unit_ID == x.tb_proddetail.tb_prod.tb_unit.Unit_ID))
                                                        {
                                                            unitList.Add(x.tb_proddetail.tb_prod.tb_unit);
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }

                                        });
                                    }
                                    #endregion

                                });
                            }

                            if (c.tb_prod != null)
                            {
                                if (!ProdList.Any(p => p.ProdBaseID == c.tb_prod.ProdBaseID))
                                {
                                    c.tb_prod.Created_at = System.DateTime.Now;
                                    c.tb_prod.Created_by = TempProd.Created_by;
                                    c.tb_prod.CustomerVendor_ID = null;
                                    c.tb_prod.DepartmentID = TempProd.DepartmentID;
                                    c.tb_prod.Rack_ID = TempProd.Rack_ID;
                                    c.tb_prod.Location_ID = TempProd.Location_ID;
                                    ProdList.Add(c.tb_prod);

                                    if (c.tb_prod.tb_prodcategories != null && !prodcategoriesList.Any(Y => Y.Category_ID == c.tb_prod.tb_prodcategories.Category_ID))
                                    {
                                        prodcategoriesList.Add(c.tb_prod.tb_prodcategories);
                                    }
                                }
                            }

                            if (!ProdDetailList.Any(Y => Y.ProdDetailID == c.ProdDetailID))
                            {
                                c.Created_at = System.DateTime.Now;
                                c.Created_by = TempProd.Created_by;
                                ProdDetailList.Add(c);
                            }
                        });
                    }
                }


                foreach (var item in ProdList)
                {
                    item.tb_Prod_Attr_Relations.ForEach(c =>
                    {
                        if (c.tb_prodproperty != null && !ProdPropertyList.Any(x => x.Property_ID == c.tb_prodproperty.Property_ID))
                        {
                            ProdPropertyList.Add(c.tb_prodproperty);
                        }
                        if (c.tb_prodpropertyvalue != null && !ProdPropertyValueList.Any(x => x.PropertyValueID == c.tb_prodpropertyvalue.PropertyValueID))
                        {
                            ProdPropertyValueList.Add(c.tb_prodpropertyvalue);
                        }
                        if (!attr_RelationsList.Any(r => r.RAR_ID == c.RAR_ID))
                        {
                            attr_RelationsList.Add(c);
                        }
                        if (c.tb_prod != null && !ProdList.Any(x => x.ProdBaseID == c.tb_prod.ProdBaseID))
                        {
                            ProdList.Add(c.tb_prod);
                        }
                        if (c.tb_proddetail != null && !ProdDetailList.Any(x => x.ProdDetailID == c.tb_proddetail.ProdDetailID))
                        {
                            ProdDetailList.Add(c.tb_proddetail);
                        }
                    });
                }


                var units = MainForm.Instance.AppContext.Db.Storageable(unitList).ToStorage();
                units.AsInsertable.ExecuteCommand();//不存在插入
                units.AsUpdateable.ExecuteCommand();//存在更新

                var producttypes = MainForm.Instance.AppContext.Db.Storageable(productTypeList).ToStorage();
                producttypes.AsInsertable.ExecuteCommand();//不存在插入
                producttypes.AsUpdateable.ExecuteCommand();//存在更新



                //类目要找一下他的上级ID直到Parent_id为0
                //List<tb_ProdCategories> ParentList = new List<tb_ProdCategories>();
                //for (int i = 0; i < prodcategoriesList.Count; i++)
                //{
                //    if (prodcategoriesList[i].Parent_id == 0)
                //    {
                //        break;
                //    }
                //    else
                //    {
                //        if (prodcategoriesList[i].tb_prodcategories_parent != null && prodcategoriesList.Any(x => x.Category_ID == prodcategoriesList[i].Parent_id))
                //        {
                //            ParentList.Add(prodcategoriesList[i].tb_prodcategories_parent);
                //        }
                //    }
                //}
                //var Parentcategory = MainForm.Instance.AppContext.Db.Storageable(ParentList).ToStorage();
                //Parentcategory.AsInsertable.ExecuteCommand();//不存在插入
                //Parentcategory.AsUpdateable.ExecuteCommand();//存在更新
                //这里是递归查找上级类目，直到Parent_id为0为止的反操作。不在这个树外的 统一再进行插入更新操作。
                List<tb_ProdCategories> TempCategoryList = new List<tb_ProdCategories>();
                TempCategoryList = prodcategoriesList.DeepCloneList<tb_ProdCategories>().ToList();

                SaveCategory(prodcategoriesList, 0, TempCategoryList);

                //var category = MainForm.Instance.AppContext.Db.Storageable(prodcategoriesList).ToStorage();
                //category.AsInsertable.ExecuteCommand();//不存在插入
                //category.AsUpdateable.ExecuteCommand();//存在更新

                var ProdPropertys = MainForm.Instance.AppContext.Db.Storageable(ProdPropertyList).ToStorage();
                ProdPropertys.AsInsertable.ExecuteCommand();//不存在插入
                ProdPropertys.AsUpdateable.ExecuteCommand();//存在更新

                var ProdPropertyValues = MainForm.Instance.AppContext.Db.Storageable(ProdPropertyValueList).ToStorage();
                ProdPropertyValues.AsInsertable.ExecuteCommand();//不存在插入
                ProdPropertyValues.AsUpdateable.ExecuteCommand();//存在更新

                ProdList.ForEach(m =>
                {
                    if (!prodcategoriesList.Any(x => x.Category_ID == m.Category_ID))
                    {

                    };

                }
 );

                var Prods = MainForm.Instance.AppContext.Db.Storageable(ProdList).ToStorage();
                Prods.AsInsertable.ExecuteCommand();//不存在插入
                Prods.AsUpdateable.ExecuteCommand();//存在更新

                var ProdDetails = MainForm.Instance.AppContext.Db.Storageable(ProdDetailList).ToStorage();
                ProdDetails.AsInsertable.ExecuteCommand();//不存在插入
                ProdDetails.AsUpdateable.ExecuteCommand();//存在更新

                MainbomList.ForEach(m =>
                {
                    m.OutApportionedCost = 0;
                    m.OutProductionAllCosts = 0;
                    m.SelfApportionedCost = 0;
                    m.SelfProductionAllCosts = 0;
                    m.TotalMaterialCost = 0;
                    m.TotalOutManuCost = 0;
                    m.TotalSelfManuCost = 0;
                });

                DetailbomList.ForEach(m =>
                {
                    m.SubtotalUnitCost = 0;
                    m.UnitCost = 0;
                });

                var boms = MainForm.Instance.AppContext.Db.Storageable(MainbomList).ToStorage();
                boms.AsInsertable.ExecuteCommand();//不存在插入
                boms.AsUpdateable.ExecuteCommand();//存在更新

                var Detailboms = MainForm.Instance.AppContext.Db.Storageable(DetailbomList).ToStorage();
                Detailboms.AsInsertable.ExecuteCommand();//不存在插入
                Detailboms.AsUpdateable.ExecuteCommand();//存在更新

                foreach (var item in ImportDataList)
                {
                    item.CustomerVendor_ID = null;
                    ////不存在插入主表，子表存在不插入，不存在插入
                    MainForm.Instance.AppContext.Db.InsertNav(item)
                          .Include(it => it.tb_ProdDetails, new InsertNavOptions()
                          { OneToManyIfExistsNoInsert = true })//配置存在不插入
                            .Include(it => it.tb_Prod_Attr_Relations, new InsertNavOptions()
                            { OneToManyIfExistsNoInsert = true })//配置存在不插入
                          .ExecuteCommand();
                }

                //更新产品详情中的BOM指向
                foreach (var ProdDetail in ProdDetailList)
                {
                    if (ProdDetail.tb_bom_s != null)
                    {
                        ProdDetail.BOM_ID = ProdDetail.tb_bom_s.BOM_ID;
                    }
                }

                await MainForm.Instance.AppContext.Db.Updateable(ProdDetailList).UpdateColumns(it => new { it.BOM_ID }).ExecuteCommandAsync();

                var attr_Relations = MainForm.Instance.AppContext.Db.Storageable(attr_RelationsList).ToStorage();
                attr_Relations.AsInsertable.ExecuteCommand();//不存在插入
                attr_Relations.AsUpdateable.ExecuteCommand();//存在更新


            }
        }


        private void SaveCategory(List<tb_ProdCategories> prodcategoriesList, long pid, List<tb_ProdCategories> TempCategoryList)
        {
            var list = prodcategoriesList.Where(x => x.Parent_id == pid).ToList();
            if (list.Count > 0)
            {

                var category = MainForm.Instance.AppContext.Db.Storageable(list).ToStorage();
                category.AsInsertable.ExecuteCommand();//不存在插入
                category.AsUpdateable.ExecuteCommand();//存在更新
                //保存过的就去掉，看还有没剩下的
                TempCategoryList.RemoveWhere(x => list.Any(y => y.Category_ID == x.Category_ID));

                for (int i = 0; i < list.Count; i++)
                {
                    SaveCategory(prodcategoriesList, list[i].Category_ID, TempCategoryList);
                }
            }

        }

        public void BindData(tb_Prod entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
            entity.Created_at = System.DateTime.Now;
            entity.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
            //设置一下默认的枚举
            //if (entity.tb_StocktakeDetails != null && entity.tb_StocktakeDetails.Count > 0)
            //{
            //    entity.tb_StocktakeDetails.ForEach(c => c.MainID = 0);
            //    entity.tb_StocktakeDetails.ForEach(c => c.SubID = 0);
            //}

            DataBindingHelper.BindData4CmbByEntity<tb_Location>(entity, k => k.Location_ID, cmbLocation_ID);
            DataBindingHelper.BindData4CmbByEntity<tb_StorageRack>(entity, k => k.Rack_ID, cmbStorageRack);
            DataBindingHelper.BindData4CmbByEntity<tb_Department>(entity, k => k.DepartmentID, cmbDepartment);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {



            };


        }


        public KryptonDockingWorkspace ws;
        protected void Exit(object thisform)
        {
            UIBizSrvice.SaveGridSettingData(CurMenuInfo, newSumDataGridViewOut, typeof(tb_Prod));

            UIBizSrvice.SaveGridSettingData(CurMenuInfo, newSumDataGridViewImport, typeof(tb_Prod));

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
            KryptonPage page = (thisform as Control).Parent as KryptonPage;
            if (page != null)
            {
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
        }

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
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
                        Query(QueryDto);
                        break;
                }

            }
            return false;
        }

        protected async virtual void Query(object QueryDto, bool UIQuery = true)
        {
            if (UIQuery)
            {
                this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

                if (ValidationHelper.hasValidationErrors(this.Controls))
                    return;
            }

            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRow.Text);

            List<string> queryConditions = QueryConditionFilter.GetQueryConditions();
            Expression<Func<tb_Prod, bool>> whereLambda = QueryConditionFilter.GetFilterExpression<tb_Prod>();

            //能分享的数据主要 是 产品数据和BOM配方
            //产品包含有属性类目主产品明细产品等等
            //通过主产品找到关系再到关系中的明细及属性属性值
            List<tb_Prod> tb_ProdDetails = await MainForm.Instance.AppContext.Db.Queryable<tb_Prod>()
            .Includes(a => a.tb_unit)
            .Includes(b => b.tb_producttype)
             .AsNavQueryable()
            .Includes(a => a.tb_prodcategories, b => b.tb_prodcategories_parent, c => c.tb_prodcategories_parent,
            d => d.tb_prodcategories_parent, e => e.tb_prodcategories_parent, f => f.tb_prodcategories_parent)
            //            .Includes(b => b.tb_prodcategories, d => d.tb_ProdCategorieses_parents, c => c.tb_prodcategories_parent)
            .Includes(b => b.tb_storagerack)
            .Includes(a => a.tb_location)
            .Includes(a => a.tb_Packings, b => b.tb_PackingDetails)
            .Includes(b => b.tb_Prod_Attr_Relations, c => c.tb_prodproperty)
            .Includes(b => b.tb_Prod_Attr_Relations, c => c.tb_prodpropertyvalue)
            .Includes(b => b.tb_Prod_Attr_Relations, c => c.tb_proddetail)
            // .Includes(b => b.tb_ProdDetails, c => c.tb_BOM_Ss, d => d.tb_BOM_SDetailSecondaries)


            .AsNavQueryable()
            .Includes(b => b.tb_ProdDetails, c => c.tb_BOM_Ss, d => d.tb_BOM_SDetails, e => e.tb_unit)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_target)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_source)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_unit)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_producttype)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_prodcategories, g => g.tb_prodcategories_parent)//bom主






            .AsNavQueryable()
            .Includes(b => b.tb_ProdDetails, c => c.tb_bom_s, d => d.tb_BOM_SDetails, e => e.tb_unit)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_target)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_unit_conversion, e => e.tb_unit_source)//bom主 
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_unit)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_producttype)//bom主
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_prodcategories, g => g.tb_prodcategories_parent)//bom主

             .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodproperty)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodpropertyvalue)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_Ss, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_proddetail)


              .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodproperty)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_prodpropertyvalue)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s, c => c.tb_BOM_SDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_Prod_Attr_Relations, g => g.tb_proddetail)


            .WhereIF(whereLambda != null, whereLambda)
            .WhereAdv(true, queryConditions, QueryDto)
            .ToPageListAsync(pageNum, pageSize) as List<tb_Prod>;

            entityType = typeof(tb_Prod);
            newSumDataGridViewOut.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            newSumDataGridViewOut.XmlFileName = this.Name + "tb_Prod" + "DataShare";
            newSumDataGridViewOut.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_Prod));

            KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
            newSumDataGridViewOut.FieldNameList.TryRemove("ProdDetailID", out kv);
            newSumDataGridViewOut.FieldNameList.TryRemove("Images", out kv);

            newSumDataGridViewOut.NeedSaveColumnsXml = true;
            bindingSourceOut.DataSource = tb_ProdDetails.ToBindingSortCollection();
            newSumDataGridViewOut.DataSource = bindingSourceOut;
        }
        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        private ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();


        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }
        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// </summary>
        public Type entityType { get; set; }

        private void newSumDataGridViewOut_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridViewOut.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }

            //图片特殊处理
            if (newSumDataGridViewOut.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    if (e.Value is byte[] && ((byte[])e.Value).Length == 0)
                    {
                        return;
                    }
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                    return;
                }
            }

            if (bindingSourceOut.Current != null)
            {
                Type dataType = bindingSourceOut.Current.GetType().GetProperty(newSumDataGridViewOut.Columns[e.ColumnIndex].DataPropertyName).PropertyType;
                // We need to check whether the property is NULLABLE
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    dataType = dataType.GetGenericArguments()[0];
                }

                //下次优化时。要注释一下 什么类型的字段 数据 要特殊处理。实际可能又把另一个情况弄错。
                switch (dataType.FullName)
                {
                    case "System.Boolean":
                        break;
                    case "System.DateTime":
                        if (newSumDataGridViewOut.Columns[e.ColumnIndex].HeaderText.Contains("日期"))
                        {
                            e.Value = DateTime.Parse(e.Value.ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case "System.Int32":
                    case "System.String":
                        if (dataType.FullName == "System.Int32"
                            && newSumDataGridViewOut.Columns[e.ColumnIndex].HeaderText.Contains("状态"))
                        {

                        }
                        else
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                }

            }



            //固定字典值显示
            string colDbName = newSumDataGridViewOut.Columns[e.ColumnIndex].Name;
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
            string colName = string.Empty;

            colName = UIHelper.ShowGridColumnsNameValue(entityType, colDbName, e.Value);

            if (!string.IsNullOrEmpty(colName) && colName != "System.Object")
            {
                e.Value = colName;
            }


        }

        private void kryptonSplitContainerLef_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    public class Temptb_ProdValidator : AbstractValidator<tb_Prod>
    {
        public Temptb_ProdValidator()
        {
            RuleFor(tb_Prod => tb_Prod.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("所属部门:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
            RuleFor(tb_Prod => tb_Prod.Location_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认仓库:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
            RuleFor(tb_Prod => tb_Prod.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认货架:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
        }

        private bool CheckForeignKeyValueCanNull(long? ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID.HasValue)
            {
                if (ForeignKeyID == 0 || ForeignKeyID == -1)
                {
                    return false;
                }
            }
            return rs;

        }
    }
}
