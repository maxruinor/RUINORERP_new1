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

            kryptonPanelQuery.Controls.Clear();
            Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器 = kryptonPanelQuery;
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

            listBoxTableList.Items.Clear();

            foreach (var tableName in ShareDataList)
            {
                SuperValue kv = new SuperValue(tableName.Key.ToString(), tableName.Value);
                listBoxTableList.Items.Add(kv);
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
                    if (bindingSourceOut.DataSource != null)
                    {
                        BindingSortCollection<tb_Prod> bcProdDetailList = bindingSourceOut.DataSource as BindingSortCollection<tb_Prod>;
                        foreach (var item in bcProdDetailList)
                        {
                            MainForm.Instance.AppContext.Db.InsertNav(item)
                           .Include(it => it.tb_Prod_Attr_Relations, new InsertNavOptions() { OneToManyIfExistsNoInsert = true })
                           .ThenInclude(it => it.tb_prodproperty, new InsertNavOptions() { OneToManyIfExistsNoInsert = true })
                           .Include(it => it.tb_Prod_Attr_Relations, new InsertNavOptions() { OneToManyIfExistsNoInsert = true })
                           .ThenInclude(it => it.tb_prodpropertyvalue, new InsertNavOptions() { OneToManyIfExistsNoInsert = true })
                           .Include(it => it.tb_ProdDetails, new InsertNavOptions() { OneToManyIfExistsNoInsert = true })
                           .ExecuteCommand();
                            //    if (item.tb_Prod_Attr_Relations != null)
                            //    {
                            //        foreach (var Prod_Attr in item.tb_Prod_Attr_Relations)
                            //        {
                            //            if (Prod_Attr.tb_prodproperty != null)
                            //            {
                            //                //
                            //            }

                            //            if (Prod_Attr.tb_prodpropertyvalue != null)
                            //            {

                            //            }
                            //        }
                            //    }

                            //    if (item.tb_prod != null)
                            //    {
                            //        //添加主产品信息
                            //        if (item.tb_prod.tb_ProdDetails != null)
                            //        {
                            //            //添加SKU
                            //        }

                            //    }
                            //}

                            //统一保存。按 先被引用的

                        }
                    }
                    break;

                case MenuItemEnums.属性:
                    //Property();
                    break;
                case MenuItemEnums.导入:

                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        List<tb_Prod> ProdDetails = DataImportExportManager.ImportDataAsync(openFileDialog.FileName);

                        entityType = typeof(tb_Prod);
                        newSumDataGridViewImport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        newSumDataGridViewImport.XmlFileName = this.Name + "tb_Prod" + "ImportDataShare";
                        newSumDataGridViewImport.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_Prod));

                        KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                        newSumDataGridViewImport.FieldNameList.TryRemove("ProdDetailID", out kv);
                        newSumDataGridViewImport.FieldNameList.TryRemove("Images", out kv);

                        newSumDataGridViewImport.NeedSaveColumnsXml = true;
                        bindingSourceOut.DataSource = ProdDetails.ToBindingSortCollection();
                        newSumDataGridViewImport.DataSource = bindingSourceOut;
                    }

                    break;
                case MenuItemEnums.导出:

                    var list = bindingSourceOut.DataSource as BindingSortCollection<tb_Prod>;
                    if (list == null)
                    {
                        return;
                    }
                    List<tb_Prod> tb_ProdDetails = new List<tb_Prod>();
                    foreach (var item in list)
                    {
                        tb_ProdDetails.Add(item);
                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        DataImportExportManager.ExportDataAsync(tb_ProdDetails, saveFileDialog.FileName);
                    }
                    break;
                default:
                    break;
            }
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
            .Includes(a => a.tb_location)
            .Includes(a => a.tb_Packings, b => b.tb_PackingDetails)
            .Includes(a => a.tb_ProdDetails, b => b.tb_BOM_SDetailSecondaries)
            .Includes( b => b.tb_producttype)
            .Includes( b => b.tb_prodcategories)
            .Includes( b => b.tb_storagerack)
            .Includes( b => b.tb_Prod_Attr_Relations, c => c.tb_prodproperty)
            .Includes( b => b.tb_Prod_Attr_Relations, c => c.tb_prodpropertyvalue)
            .Includes( b => b.tb_Prod_Attr_Relations, c => c.tb_proddetail)
            .Includes( b => b.tb_ProdDetails, c => c.tb_BOM_Ss, d => d.tb_BOM_SDetails)
             .Includes(b => b.tb_ProdDetails, c => c.tb_BOM_Ss, d => d.tb_BOM_SDetailSecondaries)
            .AsNavQueryable()
            .Includes(a => a.tb_ProdDetails, b => b.tb_bom_s)
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
}
