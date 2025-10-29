using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.Extensions;
using RUINORERP.Common;
using RUINORERP.Common.Helper;
using System.Linq.Expressions;
using Krypton.Workspace;
using Krypton.Navigator;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics;
using SqlSugar;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Business.Processor;
using RUINORERP.UI.BaseForm;
using RUINORERP.Model.Models;
using RUINORERP.UI.UserPersonalized;
using RUINORERP.Global.EnumExt;


namespace RUINORERP.UI.AdvancedUIModule
{
    /// <summary>
    /// 传入要查询的实体的类型和他的查询参数实体类型
    /// 主要用于高级查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class UCAdvFilterGeneric<T> : UCAdvFilter where T : class
    {

        /// <summary>
        /// 在初始化这个高级查询的菜单时要提供的他上级业务窗体的所属模块
        /// </summary>
        public long ModuleID { get; set; }
        public UCAdvFilterGeneric()
        {
            InitializeComponent();
            dataGridView1.NeedSaveColumnsXml = true;
            this.BaseToolStrip.ItemClicked += ToolStrip1_ItemClicked;
            string tableName = typeof(T).Name;
            DtoEntityTalbeName = tableName;
            DtoEntityType = typeof(T);
            DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList<T>().OrderBy(v => v.FieldName).ToList();
            DisplayTextResolver = new GridViewDisplayTextResolver(DtoEntityType);
            toolStripBtnExport.Visible = false;
            toolStripBtnImport.Visible = false;

            System.Linq.Expressions.Expression<Func<tb_Prod, int?>> expr;
            expr = (p) => p.SourceType;
            ColNameDataDictionary.TryAdd(expr.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource)));

            ColNameDataDictionary.TryAdd("ApprovalStatus", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));
            ColNameDataDictionary.TryAdd("PayStatus", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus)));
            ColNameDataDictionary.TryAdd("DataStatus", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));
            ColNameDataDictionary.TryAdd("Priority", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority)));
            ColNameDataDictionary.TryAdd("OrderPriority", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority)));
            ColNameDataDictionary.TryAdd("RepairStatus", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(RepairStatus)));
            ColNameDataDictionary.TryAdd("ASProcessStatus", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ASProcessStatus)));
            InitBaseValue();
            InitListData();
         
        }
        public GridViewDisplayTextResolver DisplayTextResolver;

        private tb_MenuInfo InitCurMenuInfo(tb_MenuInfo CurMenuInfo)
        {
            CurMenuInfo = new tb_MenuInfo();
            CurMenuInfo.FormName = this.Name;
            CurMenuInfo.ClassPath = menuKey;
            CurMenuInfo.IsVisble = true;
            CurMenuInfo.IsEnabled = true;
            CurMenuInfo.Created_at = DateTime.Now;
            CurMenuInfo.ModuleID = ModuleID;//这个模块
            CurMenuInfo.MenuType = "行为菜单";
            CurMenuInfo.Sort = -99999;
            CurMenuInfo.CaptionCN="高级查询";
            CurMenuInfo.MenuName = "高级查询";
            //CurMenuInfo = MainForm.Instance.AppContext.Db.Insertable<tb_MenuInfo>(CurMenuInfo).ExecuteReturnEntity();
            return CurMenuInfo;
        }


        /// <summary>
        /// 用来显示用关联外键的类型
        /// </summary>
        public Type KeyValueTypeForDgv { get; set; }

        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTBList = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 设置关联表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">可为空用另一个两名方法无参的</param>
        private void InitBaseValue()
        {
            string tableName = typeof(T).Name;
            foreach (var field in typeof(T).GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        //这个加入到这里没有看到使用，后面可以去掉。并且QueryFilter这也有类似处理的代码可以重构掉
                        FKValueColNameTBList.TryAdd(fkrattr.FK_IDColName, fkrattr.FKTableName);
                    }
                }
            }


            //这里不这样了，直接用登陆时查出来的。按菜单路径找到菜单 去再搜索 字段。
            //    显示按钮也一样的思路
            this.dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));
            /*
            bool hasMenu = false;
            tb_MenuInfo menuInfo
            MainForm.Instance.AppContext.CurUserInfo.UserModList.ForEach((mod) =>
            {
                hasMenu = mod.tb_MenuInfos.Where(m => m.IsVisble && m.EntityName == typeof(T).Name && m.ClassPath == this.ToString()).Any();
            });
            if (!hasMenu)
            {
                MessageBox.Show("菜单不能为空，请联系管理员。");
                return;
            }


            tb_MenuInfo menuInfo = MainForm.Instance.AppContext.CurUserInfo.UserMenuList.FirstOrDefault(m => m.ClassPath == this.GetType().FullName);
            if (menuInfo != null)
            {
                var fields = MainForm.Instance.AppContext.CurUserInfo.UserFieldList.Where(f => f.MenuID == menuInfo.MenuID).ToList();
                foreach (var field in fields)
                {
                    //这里权限控制了一下
                    if (!field.IsEnabled)
                    {
                        continue;
                    }
                    KeyValuePair<string, bool> kv;
                    if (this.dataGridView1.FieldNameList.TryGetValue(field.FieldName, out kv) && !field.IsEnabled)
                    {
                        //存在但不可用就是无权限   
                        this.dataGridView1.FieldNameList.TryRemove(field.FieldName, out kv);
                    }

                }
            }
            */


            //重构？
            dataGridView1.XmlFileName = tableName;

            // Refreshs();
        }

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            this.dataGridView1.DataSource = null;
            //绑定导航
            this.bindingNavigatorList.BindingSource = bindingSourceList;
            this.dataGridView1.DataSource = bindingSourceList.DataSource;
        }

        private List<BaseDtoField> dtoEntityfieldNameList;
        public List<BaseDtoField> DtoEntityFieldNameList { get => dtoEntityfieldNameList; set => dtoEntityfieldNameList = value; }


        // [AdvAttribute("asd")]
        public string DtoEntityTalbeName { get => dtoEntityTalbeName; set => dtoEntityTalbeName = value; }
        private object _queryDto = new BaseEntity();

        public object QueryDto { get => _queryDto; set => _queryDto = value; }

        private string dtoEntityTalbeName;
        private Type dtoEntityType;
        public Type DtoEntityType { get => dtoEntityType; set => dtoEntityType = value; }

        protected virtual void ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = e.ClickedItem.Text.ToString();
            if (e.ClickedItem.Text.Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(e.ClickedItem.Text));
            }
            else
            {

            }


        }

        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {
                case MenuItemEnums.查询:
                    toolStripBtnQuery.Select();
                    Query();
                    break;
                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.选中:
                    SelectedData();
                    break;
                case MenuItemEnums.属性:
                    MenuPersonalizedSettings();
                    break;
                default:
                    break;
            }

        }
        protected async virtual void MenuPersonalizedSettings()
        {
            bool rs = await UIBizService.SetQueryConditionsAsync(CurMenuInfo, QueryConditionFilter, QueryDto as BaseEntity);
            if (rs)
            {
                QueryDto = LoadQueryConditionToUI();
            }
        }
        /*
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
                        toolStripBtnQuery.Select();
                        Query(QueryDto);
                        break;
                }

            }
            return false;
        }
        */


        public async override void Query(bool UseAutoNavQuery = false)
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;

            dataGridView1.ReadOnly = true;
           
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            //QueryConditions 如果条件为0，则会查询全部结果
            //List<T> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditions, LimitQueryConditions, dto);

            List<T> list = new List<T>();
            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRows.Text);
            //提取指定的列名，即条件集合
            //List<string> queryConditions = new List<string>();
            //queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
            //ExpConverter expConverter = new ExpConverter();
            //if (Filter.FilterLimitExpression != null)
            //{
            //    var whereExp = expConverter.ConvertToFuncByClassName(typeof(T), Filter.FilterLimitExpression);
            //    LimitQueryConditions = whereExp as Expression<Func<T, bool>>;
            //}
            //else
            //{
            //    LimitQueryConditions = c => true;// queryFilter.FieldLimitCondition;
            //}
            // LimitQueryConditions = QueryConditionFilter.GetFilterExpression<T>();

            //list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, queryConditions, QueryConditionFilter.GetFilterExpression<T>(), QueryDto, pageNum, pageSize) as List<T>;
            if (QueryConditionFilter.FilterLimitExpressions == null)
            {
                QueryConditionFilter.FilterLimitExpressions = new List<LambdaExpression>();
            }

            list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDto, pageNum, pageSize) as List<T>;

            bindingSourceList.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = bindingSourceList;
        }

        #region 定义所有工具栏的方法


        protected void SelectedData()
        {
            if (bindingSourceList.Current != null)
            {
                if (OnSelectDataRow != null)
                {
                    OnSelectDataRow(bindingSourceList.Current);
                }
                //将选中的值保存到这里，用在 复杂编辑UI时 编辑外键的其他资料
                base.Tag = bindingSourceList;//传过去的是bindingSourceList。选中的值用bindingSourceList.Current
                //退出

                ListDataSoure = bindingSourceList;
                Form frm = (this as Control).Parent.Parent as Form;
                frm.DialogResult = DialogResult.OK;
                frm.Close();
                return;
            }

            if (control != null)
            {
                control.CausesValidation = true;
            }

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
                             //page.Dispose();TODO:
                MessageBox.Show("请联系管理员处理此问题。");
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
            /*
           if (page == null)
           {
               //浮动

           }
           else
           {
               //活动内
               if (cell.Pages.Contains(page))
               {
                   cell.Pages.Remove(page);
                   page.Dispose();
               }
           }
           */
        }

        protected virtual void Exit(object thisform)
        {
            //if (!Edited)
            //{
            //    //退出
            CloseTheForm(thisform);
            //}
            //else
            //{
            //    if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //    {
            //        //退出
            //        CloseTheForm(thisform);
            //    }
            //}
        }


        #endregion

        //public QueryFilter QueryConditionFilter { get; set; } = new QueryFilter();


        /// <summary>
        /// 默认不是模糊查询
        /// </summary>
        /// <param name="useLike"></param>
        public object LoadQueryConditionToUI(decimal QueryConditionShowColQty = 4)
        {
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            PanelForQuery.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
| System.Reflection.BindingFlags.NonPublic).SetValue(PanelForQuery, true, null);
            PanelForQuery.Visible = false;
            PanelForQuery.Controls.Clear();
            PanelForQuery.SuspendLayout();
            if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            {
                QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(T), true, PanelForQuery, QueryConditionFilter, QueryConditionShowColQty);
            }
            else
            {
                if (MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations == null)
                {
                    MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations = new List<tb_UIMenuPersonalization>();
                }
                tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
                if (menuSetting != null)
                {
                    QueryDto = UIGenerateHelper.CreateQueryUI(typeof(T), true, PanelForQuery, QueryConditionFilter, menuSetting);
                }
                else
                {
                    QueryDto = UIGenerateHelper.CreateQueryUI(typeof(T), true, PanelForQuery, QueryConditionFilter, QueryConditionShowColQty);
                }
            }

            //  QueryDto = UIGenerateHelper.CreateQueryUI(typeof(T), true, PanelForQuery, QueryConditionFilter, QueryConditionShowColQty);

            PanelForQuery.ResumeLayout();
            PanelForQuery.Visible = true;
            List<T> list = new List<T>();

            if (QueryConditionFilter != null && QueryConditionFilter.InvisibleCols != null)
            {
                //统一将主键ID不显示

                string pkColName = UIHelper.GetPrimaryKeyColName(typeof(T));
                //这里设置了指定列不可见
                if (!QueryConditionFilter.InvisibleCols.Contains(pkColName))
                {
                    QueryConditionFilter.InvisibleCols.Add(pkColName);
                }

                //目前没有想好，就是采购入库单中会有引用的采购订单ID，实际只要显示订单号即可。 其他类似业务也一样。就是引用另一个单的情况

                if (typeof(T).Name == typeof(tb_PurEntry).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("PurOrder_ID");
                }
                if (typeof(T).Name == typeof(tb_PurEntryRe).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("PurEntryID");
                }
                if (typeof(T).Name == typeof(tb_PurReturnEntry).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("PurEntryRe_ID");
                }
                if (typeof(T).Name == typeof(tb_SaleOut).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("SOrder_ID");
                }
                if (typeof(T).Name == typeof(tb_SaleOutRe).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("SaleOut_MainID");
                }
                if (typeof(T).Name == typeof(tb_ProductionPlan).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("SOrder_ID");
                }
                if (typeof(T).Name == typeof(tb_FinishedGoodsInv).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("MOID");
                }
                if (typeof(T).Name == typeof(tb_MRP_ReworkEntry).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("ReworkReturnID");
                }
                if (typeof(T).Name == typeof(tb_MRP_ReworkReturn).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("FG_ID");
                }
                if (typeof(T).Name == typeof(tb_ManufacturingOrder).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("PDID");
                }
                if (typeof(T).Name == typeof(tb_ProductionDemand).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("PPID");
                }
                if (typeof(T).Name == typeof(tb_ProductionPlan).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("SOrder_ID");
                }
                if (typeof(T).Name == typeof(tb_MaterialRequisition).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("MOID");
                }
                if (typeof(T).Name == typeof(tb_MaterialReturn).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("MR_ID");
                }
                if (typeof(T).Name == typeof(tb_CRM_Customer).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("Customer_id");
                }
                if (typeof(T).Name == typeof(tb_AS_RepairOrder).Name)
                {
                    QueryConditionFilter.InvisibleCols.Add("ASApplyID");
                }
                //这里设置了指定列不可见
                foreach (var item in QueryConditionFilter.InvisibleCols)
                {
                    KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                    dataGridView1.FieldNameList.TryRemove(item, out kv);
                }
            }


            bindingSourceList.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = bindingSourceList;

            //TODO:!!!!!!加总功能暂时不做。不方便显示。到时能控制再说。
            /*
            var SummaryCols = QueryConditionFilter.GetSummaryCols<T>();
            if (SummaryCols.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = SummaryCols.ToArray();
            }*/
            return QueryDto;
        }


        //以这个特殊的类路径来找这个高级过滤菜单 如果没有就新建保存
        string menuKey = typeof(T).Name + "UCAdvFilterGeneric";

        private async void UCAdvFilterGeneric_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                //这里是用来设置菜单的KEY ，并不是实际的菜单路径。只是继承了公共基类
                //这里为空时。引出的。InitFilterForControlNew

                //权限菜单  高级菜单
                if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                {
                    CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.FormName == this.Name && m.ClassPath == menuKey).FirstOrDefault();
                    if (CurMenuInfo == null)
                    {
                        CurMenuInfo = InitCurMenuInfo(CurMenuInfo);
                        if (!MainForm.Instance.MenuList.Contains(CurMenuInfo))
                        {
                            MainForm.Instance.MenuList.Add(CurMenuInfo);
                        }
                    }
                }

                tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
                if (menuSetting != null)
                {
                    if (menuSetting.tb_UIQueryConditions != null && menuSetting.tb_UIQueryConditions.Count > 0)
                    {
                        LoadQueryConditionToUI(menuSetting.QueryConditionCols);
                    }
                    else
                    {
                        LoadQueryConditionToUI(4);
                    }
                }
                else
                {
                    LoadQueryConditionToUI(4);
                }

                #region 请求缓存
                //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
                await UIBizService.RequestCache<T>();
                #endregion

            }
            dataGridView1.CellFormatting -= DataGridView1_CellFormatting;
            DisplayTextResolver.Initialize(dataGridView1);
        }

        #region

        public Control control { get; set; }

        public delegate void SelectDataRowHandler(object entity);

        [Browsable(true), Description("双击将数据载入到明细外部事件")]
        public event SelectDataRowHandler OnSelectDataRow;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedData();
        }
        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();
        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                    return;
                }
            }

            //固定字典值显示
            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;
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
                    }
                }
            }

            //动态字典值显示
            string colName = string.Empty;
            if (KeyValueTypeForDgv != null)
            {
                colName = UIHelper.ShowGridColumnsNameValue(KeyValueTypeForDgv, colDbName, e.Value);
            }
            else
            {
                colName = UIHelper.ShowGridColumnsNameValue<T>(colDbName, e.Value);
            }
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }



        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


        #endregion



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }
            if (OnSelectDataRow != null)
            {
                OnSelectDataRow(bindingSourceList.Current);
            }
        }
    }
}
