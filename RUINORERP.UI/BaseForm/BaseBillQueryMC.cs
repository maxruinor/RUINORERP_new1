using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.FormProperty;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Report;
using RUINORERP.UI.UserCenter;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using CommonHelper = RUINORERP.UI.Common.CommonHelper;
using ContextMenuController = RUINORERP.UI.UControls.ContextMenuController;
using XmlDocument = System.Xml.XmlDocument;

namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 主子表查询
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="C"></typeparam>
    public partial class BaseBillQueryMC<M, C> : BaseQuery, IContextMenuInfoAuth, IToolStripMenuInfoAuth where M : class, new() where C : class, new()
    {
        public virtual List<ContextMenuController> AddContextMenu()
        {
            List<ContextMenuController> list = new List<ContextMenuController>();
            return list;
        }

        public virtual ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }


        /// <summary>
        /// 判断是否需要加载子表明细。为了将这个基类适应于单表单据。如付款申请单
        /// </summary>
        public bool HasChildData { get; set; } = true;

        public bool ResultAnalysis { get; set; } = false;

        private QueryFilter _QueryConditionFilter = new QueryFilter();

        /// <summary>
        /// 查询条件  将来 把querypara优化掉
        /// </summary>
        public QueryFilter QueryConditionFilter { get => _QueryConditionFilter; set => _QueryConditionFilter = value; }


        /// <summary>
        /// 传入的是M,即主表类型的实体数据 
        /// </summary>
        /// <param name="obj"></param>
        public delegate void QueryRelatedRowHandler(object obj, System.Windows.Forms.BindingSource bindingSource);

        /// <summary>
        /// 查询引用单据明细
        /// </summary>
        [Browsable(true), Description("查询引用单据明细")]
        public event QueryRelatedRowHandler OnQueryRelatedChild;




        /// <summary>
        /// 比方采购入库，对应采购入库明细，
        /// 相关联的是 采购订单的明细，主单先不管
        /// 如果为空，则认为没有关联引用数据
        /// 第三方的子表类型
        /// </summary>
        public Type ChildRelatedEntityType;



        /// <summary>
        /// 双击哪一列会跳到单据编辑菜单
        /// </summary>
        public Expression<Func<M, string>> RelatedBillEditCol { get; set; }




        public BaseBillQueryMC()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    if (frm == null)
                    {
                        frm = new frmFormProperty();
                    }
                    
                    // 预初始化控件，确保在子类Load事件执行时_UCBillChildQuery不为null
                    PreInitializeControls();

                    //提前统一插入批量处理的菜单按钮

                    cbbatch.Text = "批量处理";
                    cbbatch.CheckStateChanged += (s, ex) =>
                    {
                        //this.Text = cb.CheckState.ToString();
                        //kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;
                        if(_UCBillMasterQuery != null && _UCBillMasterQuery.newSumDataGridViewMaster != null)
                        {
                            _UCBillMasterQuery.newSumDataGridViewMaster.MultiSelect = cbbatch.Checked;
                            _UCBillMasterQuery.newSumDataGridViewMaster.UseSelectedColumn = cbbatch.Checked;
                        }
                    };
                    ToolStripControlHost host = new ToolStripControlHost(cbbatch);
                    BaseToolStrip.Items.Insert(0, host);


                    //权限菜单
                    if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                    {
                        CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(M).Name && m.ClassPath == this.ToString()).FirstOrDefault();
                        if (CurMenuInfo == null)
                        {
                            CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.FormName == this.Name && m.ClassPath == this.ToString()).FirstOrDefault();
                            if (CurMenuInfo == null && !MainForm.Instance.AppContext.IsSuperUser)
                            {
                                MessageBox.Show(this.ToString() + "当前菜单不能为空，或无操作权限，请联系管理员。");
                                return;
                            }
                        }
                    }
                    MainForm.Instance.AppContext.log.ActionName = "BaseBillQueryMC()";

                    toolStripButton结案.Visible = false;
                    AddExcludeMenuList();

                    //其它的排除
                    foreach (var item in BaseToolStrip.Items)
                    {
                        if (item is ToolStripButton)
                        {
                            ToolStripButton subItem = item as ToolStripButton;
                            subItem.Click += Item_Click;
                            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, subItem, ExcludeMenuList);
                        }
                        else if (item is ToolStripDropDownButton subItemDr)
                        {
                            UIHelper.ControlButton<ToolStripDropDownButton>(CurMenuInfo, subItemDr, ExcludeMenuList);
                            subItemDr.Click += Item_Click;
                            //下一级
                            if (subItemDr.HasDropDownItems)
                            {
                                foreach (var sub in subItemDr.DropDownItems)
                                {
                                    ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                                    UIHelper.ControlButton<ToolStripMenuItem>(CurMenuInfo, subStripMenuItem);
                                    subStripMenuItem.Click += Item_Click;
                                }
                            }
                        }
                        else if (item is ToolStripSplitButton)
                        {
                            ToolStripSplitButton subItem = item as ToolStripSplitButton;
                            subItem.Click += Item_Click;
                            UIHelper.ControlButton<ToolStripSplitButton>(CurMenuInfo, subItem);
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
                        else if (item is ToolStripTextBox txt)
                        {
                            //UIHelper.ControlButton(CurMenuInfo, tsc); 最大行数 要显示
                        }
                        else if (item is ToolStripControlHost tsc)
                        {
                            UIHelper.ControlButton<ToolStripControlHost>(CurMenuInfo, tsc);
                        }
                        else
                        {

                        }
                    }

                    AddExtendButton(CurMenuInfo);

                    Krypton.Toolkit.KryptonButton button设置查询条件 = new Krypton.Toolkit.KryptonButton();
                    button设置查询条件.Text = "设置查询条件";
                    button设置查询条件.ToolTipValues.Description = "对查询条件进行个性化设置。";
                    button设置查询条件.ToolTipValues.EnableToolTips = true;
                    button设置查询条件.ToolTipValues.Heading = "提示";
                    button设置查询条件.Click += button设置查询条件_Click;
                    button设置查询条件.Width = 120;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button设置查询条件);

                    #region 设置子表格式
                    KryptonContextMenu kcm设置明细表格 = new KryptonContextMenu();
                    KryptonContextMenuItem menuItem设置明细表格 = new KryptonContextMenuItem("设置明细表格");
                    menuItem设置明细表格.Text = "设置明细表格";
                    menuItem设置明细表格.Click += menuItem设置明细表格_Click;
                    KryptonContextMenuItems kryptonContextMenuItems1 = new KryptonContextMenuItems();
                    kcm设置明细表格.Items.AddRange(new KryptonContextMenuItemBase[] { kryptonContextMenuItems1 });
                    kryptonContextMenuItems1.Items.AddRange(new KryptonContextMenuItemBase[] { menuItem设置明细表格 });
                    #endregion

                    Krypton.Toolkit.KryptonDropButton button表格显示设置 = new Krypton.Toolkit.KryptonDropButton();
                    button表格显示设置.Text = "表格显示设置";
                    button表格显示设置.KryptonContextMenu = kcm设置明细表格;
                    button表格显示设置.ToolTipValues.Description = "对表格显示设置进行个性化设置。";
                    button表格显示设置.ToolTipValues.EnableToolTips = true;
                    button表格显示设置.ToolTipValues.Heading = "提示";
                    button表格显示设置.Width = 120;
                    button表格显示设置.Click += button表格显示设置_Click;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button表格显示设置);

                }
            }

        }

        private async void menuItem设置明细表格_Click(object sender, EventArgs e)
        {
            await UIBizService.SetGridViewAsync(typeof(C), _UCBillChildQuery.newSumDataGridViewChild, CurMenuInfo, true);
        }

        private async void button表格显示设置_Click(object sender, EventArgs e)
        {
            await UIBizService.SetGridViewAsync(typeof(M), _UCBillMasterQuery.newSumDataGridViewMaster, CurMenuInfo, true, _UCBillMasterQuery.InvisibleCols);
        }

        private async void button设置查询条件_Click(object sender, EventArgs e)
        {
            bool rs = await UIBizService.SetQueryConditionsAsync(CurMenuInfo, QueryConditionFilter, QueryDtoProxy as BaseEntity);
            if (rs)
            {
                QueryDtoProxy = LoadQueryConditionToUI();
            }
        }






        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> MasterColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ChildColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ChildRelatedColNameDataDictionary { set; get; } = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        public virtual void BuildColNameDataDictionary()
        {
            MasterColNameDataDictionary.TryAdd(nameof(DataStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));
            MasterColNameDataDictionary.TryAdd(nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));
            MasterColNameDataDictionary.TryAdd(nameof(PayStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus)));
            MasterColNameDataDictionary.TryAdd(nameof(Priority), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority)));

            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in MainForm.Instance.View_ProdDetailList)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            ChildColNameDataDictionary.TryAdd("ProdDetailID", proDetailList);
        }

        public delegate void QueryHandler();

        [Browsable(true), Description("查询主表")]
        public event QueryHandler OnQuery;


        private async void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            await DoButtonClick(RUINORERP.Common.Helper.EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
        }
        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual async Task DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            MainForm.Instance.AppContext.log.ActionName = menuItem.ToString();
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                /*
                tb_MenuInfo menuInfo = MainForm.Instance.AppContext.CurUserInfo.UserMenuList.Where(c => c.MenuType == "行为菜单").Where(c => c.FormName == this.Name).FirstOrDefault();
                if (menuInfo == null)
                {
                    MessageBox.Show($"没有使用【{menuInfo.MenuName}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<tb_ButtonInfo> btnList = MainForm.Instance.AppContext.CurUserInfo.UserButtonList.Where(c => c.MenuID == menuInfo.MenuID).ToList();
                if (!btnList.Where(b => b.BtnText == menuItem.ToString()).Any())
                {
                    MessageBox.Show($"没有使用【{menuItem.ToString()}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }*/
            }
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            List<M> selectlist = GetSelectResult();
            switch (menuItem)
            {

                case MenuItemEnums.查询:
                    Query(QueryDtoProxy);
                    toolStripSplitButtonPrint.Enabled = true;
                    break;
                case MenuItemEnums.复制性新增:
                    if (selectlist.Count == 0)
                    {
                        return;
                    }
                    AddByCopy(selectlist);
                    break;
                case MenuItemEnums.关闭:
                    await Exit(this);
                    break;
                case MenuItemEnums.提交:
                    Submit();
                    break;
                case MenuItemEnums.属性:
                    Property();
                    break;
                case MenuItemEnums.审核:
                    // List<M> selectlist = GetSelectResult();
                    if (selectlist.Count > 0)
                    {
                        ApprovalEntity ae = await Review(selectlist);
                    }

                    break;
                case MenuItemEnums.反审:
                    if (selectlist.Count > 0)
                    {
                        //只操作批一行
                        ApprovalEntity ae = await ReReview(selectlist[0]);
                    }
                    break;
                case MenuItemEnums.结案:
                    // List<M> selectlist = GetSelectResult();
                    if (selectlist.Count > 0)
                    {
                        bool rs = await CloseCase(selectlist);
                        if (rs)
                        {
                          await  MainForm.Instance.AuditLogHelper.CreateAuditLog<M>("结案", selectlist[0], $"结案意见:{rs}");
                        }
                    }
                    break;

                case MenuItemEnums.打印:

                    if (PrintConfig != null && PrintConfig.tb_PrintTemplates != null)
                    {
                        //如果当前单据只有一个模块，就直接打印
                        if (PrintConfig.tb_PrintTemplates.Count == 1)
                        {
                          await  Print(RptMode.PRINT);
                            return;
                        }
                    }

                    //个性化设置了打印要选择模板打印时，就进入设计介面
                    if (MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.HasValue
                           && MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.Value)
                    {
                      await  Print(RptMode.DESIGN);
                    }
                    else
                    {
                      await  Print(RptMode.PRINT);
                    }
                    break;
                case MenuItemEnums.预览:
                  await  Print(RptMode.PREVIEW);
                    break;
                case MenuItemEnums.设计:
                 await   Print(RptMode.DESIGN);
                    break;
                case MenuItemEnums.删除:
                    Delete(selectlist);
                    break;
                case MenuItemEnums.导出:
                    UIExcelHelper.ExportExcel(_UCBillMasterQuery.newSumDataGridViewMaster);
                    break;
                default:
                    break;
            }
        }

        public virtual void AddByCopy(List<M> EditEntitys)
        {


        }





        
        public virtual async Task Print(RptMode rptMode)
        {
            List<M> printItems = new List<M>();
            List<M> selectlist = GetSelectResult();
            if (selectlist.Count == 0)
            {
                return;
            }
            foreach (var item in selectlist)
            {
                if (item == null)
                {
                    continue;
                }
                if (item.ContainsProperty("DataStatus"))
                {
                    if (item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.草稿).ToString() || item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.新建).ToString())
                    {
                        BizType bizType = Business.BizMapperService.EntityMappingHelper.GetBizType(typeof(M).Name);
                        if (MessageBox.Show($"当前【{bizType.ToString()}】没有审核,你确定要打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            continue;
                        }
                    }
                }

                //打印次数提醒
                if (item.ContainsProperty("PrintStatus"))
                {
                    BizType bizType = Business.BizMapperService.EntityMappingHelper.GetBizType(typeof(M).Name);
                    int printCounter = item.GetPropertyValue("PrintStatus").ToString().ToInt();
                    if (printCounter > 0)
                    {
                        if (MessageBox.Show($"当前【{bizType.ToString()}】已经打印过【{printCounter}】次,你确定要重新打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            continue;
                        }
                    }
                }

                printItems.Add(item);
            }

            if (printItems.Count == 0)
            {
                MessageBox.Show("没有需要打印的数据");
                return;
            }

            if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
            {
                PrintConfig = PrintHelper<M>.GetPrintConfig(printItems);
            }
            bool rs = await PrintHelper<M>.Print(printItems, rptMode, PrintConfig);
            if (rs && rptMode == RptMode.PRINT)
            {
                toolStripSplitButtonPrint.Enabled = false;
            }
        }

        #region 为了性能 打印认为打印时 检测过的打印机相关配置在一个窗体下成功后。即可不每次检测
        private tb_PrintConfig printConfig = null;
        public tb_PrintConfig PrintConfig
        {
            get
            {

                return printConfig;
            }
            set
            {
                printConfig = value;

            }
        }
        #endregion

        /// <summary>
        /// 删除远程的图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<bool> DeleteImages(M EditEntity)
        {
            var ctrpay = Startup.GetFromFac<FileManagementController>();
            try
            {
                var fileDeleteResponse = await ctrpay.DeleteImagesAsync(EditEntity as BaseEntity, true);
                if (fileDeleteResponse.IsSuccess && fileDeleteResponse.DeletedFileIds != null && fileDeleteResponse.DeletedFileIds.Count > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected async virtual void Delete(List<M> Datas)
        {
            if (Datas == null || Datas.Count == 0)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                int counter = 0;
                foreach (var item in Datas)
                {
                    //https://www.runoob.com/w3cnote/csharp-enum.html
                    var dataStatus = (DataStatus)(item.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                    if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                    {
                        BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
                        string PKColName = UIHelper.GetPrimaryKeyColName(typeof(M));
                        object PKValue = item.GetPropertyValue(PKColName);

                        bool rs = await ctr.BaseDeleteByNavAsync(item as M);
                        //bool rs = await ctr.BaseDeleteAsync(item);
                        if (rs)
                        {
                            //删除远程图片及本地图片
                            await DeleteImages(item as M);
                            MainForm.Instance.AuditLogHelper.CreateAuditLog<M>("删除", item);
                            counter++;

                            for (global::System.Int32 i = 0; i < this._UCBillMasterQuery.bindingSourceMaster.Count; i++)
                            {
                                if (this._UCBillMasterQuery.bindingSourceMaster[i] is M && this._UCBillMasterQuery.bindingSourceMaster[i].GetPropertyValue(PKColName).ToString() == PKValue.ToString())
                                {
                                    this._UCBillMasterQuery.bindingSourceMaster.Remove(this._UCBillMasterQuery.bindingSourceMaster[i]);
                                }
                            }

                        }
                    }
                    else
                    {
                        //
                        MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                    }
                }
                MainForm.Instance.uclog.AddLog("提示", $"成功删除数据：{counter}条.");
            }
        }




        public virtual List<M> GetSelectResult()
        {
            List<M> selectlist = new List<M>();
            if (_UCBillMasterQuery != null)
            {
                _UCBillMasterQuery.newSumDataGridViewMaster.EndEdit();
                if (cbbatch.Checked || _UCBillMasterQuery.newSumDataGridViewMaster.UseSelectedColumn)
                {
                    #region 批量处理
                    if (_UCBillMasterQuery.newSumDataGridViewMaster.SelectedRows != null)
                    {
                        foreach (DataGridViewRow dr in _UCBillMasterQuery.newSumDataGridViewMaster.Rows)
                        {
                            if (!(dr.DataBoundItem is M))
                            {
                                MessageBox.Show("TODO:请调试这里");
                            }

                            if (_UCBillMasterQuery.newSumDataGridViewMaster.UseSelectedColumn && (bool)dr.Cells["Selected"].Value)
                            {
                                selectlist.Add((M)dr.DataBoundItem);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 单行处理
                    if (_UCBillMasterQuery.newSumDataGridViewMaster.CurrentRow != null)
                    {
                        var dr = _UCBillMasterQuery.newSumDataGridViewMaster.CurrentRow;
                        if (!(dr.DataBoundItem is M))
                        {
                            MessageBox.Show("TODO:请调试这里");
                        }
                        selectlist.Add((M)dr.DataBoundItem);
                    }
                    #endregion
                }
            }
            return selectlist;
        }

        public virtual void AdvQuery()
        {

        }

        public virtual void Submit()
        {

        }

        /// <summary>
        /// 暂时只支持一级审核，将来可以设计配置 可选多级审核。并且能看到每级的审核情况
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        //public virtual Task<ApprovalEntity> Review(List<M> EditEntitys)
        //{
        //    ApprovalEntity ae = new ApprovalEntity();
        //    return Task.FromResult(ae);
        //}
        /// <summary>
        /// 审核 注意后面还需要加很多业务逻辑。
        /// 比方出库单，审核就会减少库存修改成本
        /// （如果有月结动作，则在月结时统计修改成本，更科学，因为如果退单等会影响成本）
        /// </summary>
        protected async virtual Task<ApprovalEntity> Review(M EditEntity)
        {
            return await Review(new List<M> { EditEntity });
        }

        /// <summary>
        /// 批量审核多个实体单据
        /// </summary>
        /// <param name="EditEntities">需要审核的实体列表</param>
        /// <returns>审核结果</returns>
        protected async virtual Task<ApprovalEntity> Review(List<M> EditEntities)
        {
            return await Review(EditEntities, 10); // 默认延时10ms
        }

        /// <summary>
        /// 批量审核多个实体单据（带处理延迟）
        /// </summary>
        /// <param name="EditEntities">需要审核的实体列表</param>
        /// <param name="delayMs">每个审核之间的延迟毫秒数</param>
        /// <returns>审核结果</returns>
        protected async virtual Task<ApprovalEntity> Review(List<M> EditEntities, int delayMs)
        {
            if (EditEntities == null || EditEntities.Count == 0)
            {
                return null;
            }

            // 如果只有一个实体，使用原有逻辑
            if (EditEntities.Count == 1)
            {
                var result = await ReviewSingle(EditEntities[0]);
                if (delayMs > 0)
                    await Task.Delay(delayMs);
                return result;
            }

            // 多个实体批量审核 - 先弹出审核对话框获取用户审核意见
            // 创建一个用于收集审核信息的ApprovalEntity
            ApprovalEntity batchApprovalInfo = new ApprovalEntity();
            
            // 使用第一个实体的信息来初始化审核对话框
            var firstEntity = EditEntities[0];
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(firstEntity, PKCol);
            batchApprovalInfo.BillID = pkid;
            CommBillData cbd = EntityMappingHelper.GetBillData<M>(firstEntity);
            batchApprovalInfo.BillNo = cbd.BillNo;
            batchApprovalInfo.bizType = cbd.BizType;
            batchApprovalInfo.bizName = cbd.BizName;
            batchApprovalInfo.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

            // 显示审核对话框
            CommonUI.frmApproval frm = new CommonUI.frmApproval();
            frm.BindData(batchApprovalInfo);
            await Task.Delay(1);
            
            if (frm.ShowDialog() != DialogResult.OK)
            {
                // 用户取消审核
                return null;
            }

            // 多个实体批量审核
            List<ApprovalEntity> approvalResults = new List<ApprovalEntity>();
            bool hasFailures = false;
            StringBuilder failureMessages = new StringBuilder();
            int successCount = 0;

            for (int i = 0; i < EditEntities.Count; i++)
            {
                var entity = EditEntities[i];
                try
                {
                    // 为每个实体创建独立的审核信息，但使用统一的审核结果
                    ApprovalEntity entityApprovalInfo = new ApprovalEntity();
                    string entityPKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
                    long entityPkid = (long)ReflectionHelper.GetPropertyValue(entity, entityPKCol);
                    entityApprovalInfo.BillID = entityPkid;
                    CommBillData entityCbd = EntityMappingHelper.GetBillData<M>(entity);
                    entityApprovalInfo.BillNo = entityCbd.BillNo;
                    entityApprovalInfo.bizType = entityCbd.BizType;
                    entityApprovalInfo.bizName = entityCbd.BizName;
                    entityApprovalInfo.Approver_by = batchApprovalInfo.Approver_by;
                    entityApprovalInfo.ApprovalResults = batchApprovalInfo.ApprovalResults;
                    entityApprovalInfo.ApprovalOpinions = batchApprovalInfo.ApprovalOpinions;
                    entityApprovalInfo.ApprovalStatus = batchApprovalInfo.ApprovalStatus;

                    var result = await ReviewSingleWithApprovalInfo(entity, entityApprovalInfo);
                    approvalResults.Add(result);
                    
                    // 检查审核是否成功
                    if (result != null && (ReflectionHelper.ExistPropertyName<M>("ApprovalResults") 
                        && entity.GetPropertyValue("ApprovalResults")?.ToBool() == true))
                    {
                        successCount++;
                    }
                    else
                    {
                        hasFailures = true;
                        string billNo = ReflectionHelper.ExistPropertyName<M>("BillNo") ? 
                            entity.GetPropertyValue("BillNo")?.ToString() : "未知单据";
                        failureMessages.AppendLine($"单据 {billNo} 审核失败");
                    }
                    
                    // 添加延迟（如果不是最后一个元素）
                    if (delayMs > 0 && i < EditEntities.Count - 1)
                    {
                        await Task.Delay(delayMs);
                    }
                }
                catch (Exception ex)
                {
                    hasFailures = true;
                    string billNo = ReflectionHelper.ExistPropertyName<M>("BillNo") ? 
                        entity.GetPropertyValue("BillNo")?.ToString() : "未知单据";
                    failureMessages.AppendLine($"单据 {billNo} 审核异常: {ex.Message}");
                }
            }

            // 显示批量审核结果
            if (hasFailures)
            {
                MessageBox.Show($"成功审核 {successCount} 个单据，失败 {EditEntities.Count - successCount} 个单据:\n{failureMessages}", "批量审核结果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show($"成功审核 {EditEntities.Count} 个单据", "批量审核结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 返回最后一个审核结果
            return approvalResults.LastOrDefault();
        }

        /// <summary>
        /// 审核单个实体（原Review方法逻辑）
        /// </summary>
        private async Task<ApprovalEntity> ReviewSingle(M EditEntity)
        {
            //如果已经审核并且审核通过，则不能再次审核
            ApprovalEntity ae = new ApprovalEntity();
            if (EditEntity == null)
            {
                return null;
            }
            //检查是不是可以正常审核  如果驳回了。就要修改为未审核的情况
            if (ReflectionHelper.ExistPropertyName<M>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<M>("ApprovalResults"))
            {
                //反审，要审核过，并且通过了，才能反审。
                if (EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                    && EditEntity.GetPropertyValue("ApprovalResults") != null
                    && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                    )
                {
                    MainForm.Instance.uclog.AddLog("提示", "【未审核】或【驳回】的单据才能审核。");
                    return ae;
                }
            }


            CommonUI.frmApproval frm = new CommonUI.frmApproval();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ae.BillID = pkid;
            CommBillData cbd = EntityMappingHelper.GetBillData<M>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;


            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            await Task.Delay(1);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                return await ReviewSingleWithApprovalInfo(EditEntity, ae);
            }

            return null;
        }

        /// <summary>
        /// 使用指定审核信息审核单个实体
        /// </summary>
        private async Task<ApprovalEntity> ReviewSingleWithApprovalInfo(M EditEntity, ApprovalEntity ae)
        {
            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            //M oldobj_old = CloneHelper.DeepCloneObject<M>(EditEntity);

            M oldobj = CloneHelper.DeepCloneObject_maxnew<M>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<M>(EditEntity, oldobj);
            };
            if (ae.ApprovalResults == true)
            {
                //审核了。数据状态要更新为
                EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.确认);
            }
            else
            {
                //审核了。驳回 时数据状态要更新为新建。要再次修改后提交
                EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.新建);
                if (ReflectionHelper.ExistPropertyName<M>("ApprovalOpinions"))
                {
                    EditEntity.SetPropertyValue("ApprovalOpinions", ae.ApprovalOpinions);
                }
                if (ReflectionHelper.ExistPropertyName<M>("ApprovalStatus"))
                {
                    EditEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.驳回);
                }
                if (ReflectionHelper.ExistPropertyName<M>("ApprovalResults"))
                {
                    EditEntity.SetPropertyValue("ApprovalResults", false);
                }
                BusinessHelper.Instance.ApproverEntity(EditEntity);
                BaseController<M> ctrBase = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
                //因为只需要更新主表
                await ctrBase.BaseSaveOrUpdate(EditEntity as M);
                return ae;
            }


            //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
            PropertyInfo[] array_property = ae.GetType().GetProperties();
            {
                /* 注意审核时要把这些值给到单据中
                entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                entity.ApprovalResults = approvalEntity.ApprovalResults;
                */

                foreach (var property in array_property)
                {
                    //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                    //Expression<Func<ApprovalEntity, object>> PNameExp = t => t.ApprovalStatus;
                    //MemberInfo minfo = PNameExp.GetMemberInfo();
                    //string propertyName = minfo.Name;
                    if (ReflectionHelper.ExistPropertyName<M>(property.Name))
                    {
                        object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                        //if (aeValue.Equals(true))
                        //{
                        //    aeValue = 1;
                        //}
                        //if (aeValue.Equals(false))
                        //{
                        //    aeValue = 0;
                        //}
                        ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                    }
                }
            }

            ReturnResults<M> rmr = new ReturnResults<M>();
            BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
            rmr = await ctr.ApprovalAsync(EditEntity);
            if (rmr.Succeeded)
            {
                //ToolBarEnabledControl(MenuItemEnums.反审);
                Query(QueryDtoProxy);
                //这里推送到审核，启动工作流
            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg},请联系管理员！", Color.Red);
                MessageBox.Show($"{ae.bizName}:{ae.BillNo}审核失败。\r\n {rmr.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            await MainForm.Instance.AuditLogHelper.CreateAuditLog<M>("审核", EditEntity, $"意见{ae.ApprovalOpinions}" + $"结果:{(ae.ApprovalResults ? "通过" : "拒绝")},{rmr.ErrorMsg}");

            return ae;
        }

           

        /// <summary>
        /// 反审
        /// 系统不支持批量反审，所以这里只支持单条记录。反审本身就是一个特殊情况。需要仔细核对
        /// 暂时只支持一级审核，将来可以设计配置 可选多级审核。并且能看到每级的审核情况
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        /// <param name="EditEntitys"></param>
        /// <returns></returns>
        public async virtual Task<ApprovalEntity> ReReview(M EditEntity)
        {
            if (EditEntity == null)
            {


                return null;
            }
            ApprovalEntity ae = new ApprovalEntity();
            #region 审核状态
            if (ReflectionHelper.ExistPropertyName<M>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<M>("ApprovalResults"))
            {
                //反审，要审核过，并且通过了，才能反审。
                if (EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                    && EditEntity.GetPropertyValue("ApprovalResults") != null
                    && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                    )
                {

                }
                else
                {
                    MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审。");
                    return ae;
                }
            }
 
            CommonUI.frmReApproval frm = new CommonUI.frmReApproval();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ae.BillID = pkid;
            CommBillData cbd = EntityMappingHelper.GetBillData<M>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                RevertCommand command = new RevertCommand();
                //缓存当前编辑的对象。如果撤销就回原来的值
                M oldobj = CloneHelper.DeepCloneObject_maxnew<M>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果取消反审，内存中反审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<M>(EditEntity, oldobj);
                };

                //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
                PropertyInfo[] array_property = ae.GetType().GetProperties();
                {
                    foreach (var property in array_property)
                    {
                        //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                        //Expression<Func<ApprovalEntity, object>> PNameExp = t => t.ApprovalStatus;
                        //MemberInfo minfo = PNameExp.GetMemberInfo();
                        //string propertyName = minfo.Name;
                        if (ReflectionHelper.ExistPropertyName<M>(property.Name))
                        {
                            object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                            ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                        }
                    }
                }


                ReturnResults<M> rmr = new ReturnResults<M>();
                BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
                rmr = await ctr.AntiApprovalAsync(EditEntity);
                if (rmr.Succeeded)
                {
                    //ToolBarEnabledControl(MenuItemEnums.反审);
                    //这里推送到审核，启动工作流
                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
                    MessageBox.Show($"{ae.bizName}:{ae.BillNo}反审核失败。\r\n {rmr.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MainForm.Instance.AuditLogHelper.CreateAuditLog<M>("反审", EditEntity, $"反审原因{ae.ApprovalOpinions}" + $"结果:{(ae.ApprovalResults ? "通过" : "拒绝")},{rmr.ErrorMsg}");
            }
            return ae;
            #endregion
        }




        public virtual Task<bool> CloseCase(List<M> EditEntitys)
        {
            return null;
        }




        protected frmFormProperty frm = null;
        protected virtual void Property()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new frmFormProperty();
            }

            if (frm.ShowDialog() == DialogResult.OK)
            {
                //保存属性
            }
        }

        /// <summary>
        /// 针对查询结果的限制
        /// 也可以对process中字段添加子限制来过滤加载的项目
        /// </summary>
        public virtual void BuildLimitQueryConditions()
        {
            try
            {
                // 应用行级权限过滤
                ApplyRowLevelAuthFilter();
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("应用行级权限过滤时发生错误", ex);
                // 发生错误时继续执行，不阻止查询
            }
        }

        /// <summary>
        /// 应用行级权限过滤条件
        /// </summary>
        private void ApplyRowLevelAuthFilter()
        {
            try
            {
                // 检查是否为超级管理员，如果是则跳过行级权限过滤
                if (MainForm.Instance != null && MainForm.Instance.AppContext != null && MainForm.Instance.AppContext.IsSuperUser)
                {
                    MainForm.Instance.logger.LogDebug("当前用户为超级管理员，跳过行级权限过滤");
                    return;
                }

                if (!MainForm.Instance.AppContext.FunctionConfig.EnableRowLevelAuth)
                {
                    return;
                }

                // 获取行级权限服务
                var rowAuthService = Startup.GetFromFac<IRowAuthService>();
                if (rowAuthService == null)
                {
                    MainForm.Instance.logger.LogWarning("无法获取行级权限服务，跳过行级权限过滤");
                    return;
                }

                // 获取实体类型
                Type entityType = typeof(M);
                MainForm.Instance.logger.LogDebug("为实体类型 {EntityType} 应用行级权限过滤", entityType.FullName);

                // 获取过滤条件SQL子句
                string filterClause = rowAuthService.GetUserRowAuthFilterClause(entityType, CurMenuInfo.MenuID);

                if (!string.IsNullOrEmpty(filterClause))
                {
                    MainForm.Instance.logger.LogDebug("获取到行级权限过滤条件: {FilterClause}", filterClause);
                    try
                    {
                        // 预处理filterClause，移除可能导致问题的外层括号和标准化SQL语法
                        string processedFilterClause = PreprocessFilterClause(filterClause);

                        #region 新逻辑

                        #endregion




                        // 检查是否包含复杂SQL结构(如EXISTS子查询)
                        if (ContainsComplexSqlStructure(processedFilterClause))
                        {
                            MainForm.Instance.logger.LogDebug("检测到复杂SQL表达式，使用专门的处理机制");
                            // 对于复杂SQL表达式，直接使用包装方法处理
                            Expression<Func<M, bool>> complexFilterExpression = t => EvaluateComplexFilterExpression(t, processedFilterClause);

                            if (LimitQueryConditions != null)
                            {
                                // 合并现有条件和复杂SQL条件
                                LimitQueryConditions = MergeWithComplexCondition(LimitQueryConditions, complexFilterExpression);
                            }
                            else
                            {
                                // 直接使用复杂SQL条件
                                LimitQueryConditions = complexFilterExpression;
                            }

                            // 不将复杂条件添加到QueryConditionFilter.FilterLimitExpressions中，避免SQL转换错误
                            MainForm.Instance.logger.LogDebug("复杂条件仅保存在LimitQueryConditions中，不添加到FilterLimitExpressions");
                        }
                        else
                        {
                            // 如果已有查询条件，将行级权限条件添加到现有条件中
                            if (LimitQueryConditions != null)
                            {
                                // 合并查询条件和行级权限条件
                                LimitQueryConditions = MergeQueryConditions(LimitQueryConditions, processedFilterClause);
                            }
                            else
                            {
                                // 如果没有现有条件，直接使用行级权限条件
                                LimitQueryConditions = CreateRowAuthExpression(processedFilterClause);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, "解析行级权限过滤条件时发生错误: {FilterClause}", filterClause);
                        // 详细记录异常信息，便于调试
                        MainForm.Instance.logger.LogError(ex.StackTrace);
                        // 发生错误时继续执行，不阻止查询
                    }
                }
                else
                {
                    MainForm.Instance.logger.LogDebug("未获取到行级权限过滤条件，可能是无权限限制或无适用规则");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "应用行级权限过滤时发生错误");
                MainForm.Instance.logger.LogError(ex.StackTrace);
                // 发生错误时继续执行，不阻止查询
            }
        }

        #region 旧的一些方法 只是保留一些写法。有AI 可能不需要了


        #endregion

        /// <summary>
        /// 检查过滤条件是否包含复杂SQL结构
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>是否包含复杂SQL结构</returns>
        private bool ContainsComplexSqlStructure(string filterClause)
        {
            if (string.IsNullOrEmpty(filterClause))
                return false;

            string lowerClause = filterClause.ToLower();
            // 检查是否包含常见的复杂SQL结构
            return lowerClause.Contains("exists") ||
                   lowerClause.Contains("in (") ||
                   lowerClause.Contains("join") ||
                   lowerClause.Contains("select ") ||
                   lowerClause.Contains("from ");
        }

        /// <summary>
        /// 检查表达式是否包含EvaluateComplexFilterExpression方法调用
        /// 用于识别无法直接转换为SQL的复杂条件表达式
        /// </summary>
        /// <param name="expression">要检查的表达式</param>
        /// <returns>如果表达式包含EvaluateComplexFilterExpression方法调用，则返回true；否则返回false</returns>
        private bool ContainsEvaluateComplexFilterExpression(Expression expression)
        {
            if (expression == null)
                return false;

            // 检查是否为方法调用表达式
            if (expression is MethodCallExpression methodCallExpr)
            {
                // 检查方法名是否为EvaluateComplexFilterExpression
                if (methodCallExpr.Method.Name == "EvaluateComplexFilterExpression")
                {
                    return true;
                }

                // 递归检查方法参数
                foreach (var arg in methodCallExpr.Arguments)
                {
                    if (ContainsEvaluateComplexFilterExpression(arg))
                    {
                        return true;
                    }
                }
            }
            // 检查是否为二元表达式
            else if (expression is BinaryExpression binaryExpr)
            {
                // 递归检查左操作数
                if (ContainsEvaluateComplexFilterExpression(binaryExpr.Left))
                {
                    return true;
                }

                // 递归检查右操作数
                if (ContainsEvaluateComplexFilterExpression(binaryExpr.Right))
                {
                    return true;
                }
            }
            // 检查是否为一元表达式
            else if (expression is UnaryExpression unaryExpr)
            {
                // 递归检查操作数
                if (ContainsEvaluateComplexFilterExpression(unaryExpr.Operand))
                {
                    return true;
                }
            }
            // 检查是否为lambda表达式
            else if (expression is LambdaExpression lambdaExpr)
            {
                // 递归检查lambda表达式体
                if (ContainsEvaluateComplexFilterExpression(lambdaExpr.Body))
                {
                    return true;
                }
            }
            // 检查是否为参数表达式（基本情况，无需递归）
            else if (expression is ParameterExpression)
            {
                // 参数表达式不包含方法调用
                return false;
            }
            // 检查是否为常量表达式（基本情况，无需递归）
            else if (expression is ConstantExpression)
            {
                // 常量表达式不包含方法调用
                return false;
            }
            // 其他类型的表达式
            else
            {
                // 对于其他类型的表达式，尝试访问其属性或字段
                try
                {
                    // 获取表达式的所有子表达式
                    var subExpressions = expression.GetType().GetProperties()
                        .Where(p => typeof(Expression).IsAssignableFrom(p.PropertyType))
                        .Select(p => p.GetValue(expression) as Expression)
                        .Where(e => e != null);

                    // 递归检查所有子表达式
                    foreach (var subExpr in subExpressions)
                    {
                        if (ContainsEvaluateComplexFilterExpression(subExpr))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 发生异常时，记录日志并返回false
                    MainForm.Instance.logger.LogWarning(ex, "检查表达式是否包含EvaluateComplexFilterExpression方法调用时发生异常");
                }
            }

            // 没有找到EvaluateComplexFilterExpression方法调用
            return false;
        }

        /// <summary>
        /// 合并现有查询条件和复杂条件表达式
        /// 优化的实现，提供更健壮的表达式合并逻辑和错误处理，避免重复条件合并
        /// </summary>
        /// <param name="existingCondition">现有查询条件</param>
        /// <param name="complexCondition">复杂条件表达式</param>
        /// <returns>合并后的查询条件</returns>
        private Expression<Func<M, bool>> MergeWithComplexCondition(Expression<Func<M, bool>> existingCondition, Expression<Func<M, bool>> complexCondition)
        {
            try
            {
                if (existingCondition == null)
                {
                    MainForm.Instance.logger.LogDebug("现有条件为空，直接返回复杂条件表达式");
                    return complexCondition;
                }

                if (complexCondition == null)
                {
                    MainForm.Instance.logger.LogDebug("复杂条件为空，直接返回现有查询条件");
                    return existingCondition;
                }

                // 检测是否已有相同的复杂条件，避免重复合并
                if (HasDuplicateComplexCondition(existingCondition, complexCondition))
                {
                    MainForm.Instance.logger.LogDebug("检测到重复的复杂条件，跳过合并操作");
                    return existingCondition;
                }

                MainForm.Instance.logger.LogDebug("准备合并现有查询条件和复杂条件表达式");

                // 创建统一的参数表达式，确保两个条件使用相同的参数实例
                var parameter = Expression.Parameter(typeof(M), "t");

                try
                {
                    // 从现有表达式中获取条件
                    var existingConditionBody = Expression.Invoke(existingCondition, parameter);

                    // 从复杂条件表达式中获取条件
                    var complexConditionBody = Expression.Invoke(complexCondition, parameter);

                    // 创建AND组合表达式
                    var combinedExpression = Expression.AndAlso(existingConditionBody, complexConditionBody);

                    // 创建最终的Lambda表达式
                    var combinedCondition = Expression.Lambda<Func<M, bool>>(combinedExpression, parameter);

                    MainForm.Instance.logger.LogDebug("成功合并现有查询条件和复杂条件表达式");
                    return combinedCondition;
                }
                catch (InvalidOperationException ioEx)
                {
                    // 捕获参数不匹配或表达式结构问题
                    MainForm.Instance.logger.LogWarning(ioEx, "使用标准方法合并表达式失败，尝试替代方法");

                    // 替代方法：创建一个新的Lambda表达式，内部调用两个条件方法
                    Expression<Func<M, bool>> alternativeCombined = t => existingCondition.Compile()(t) && complexCondition.Compile()(t);

                    MainForm.Instance.logger.LogDebug("成功使用替代方法合并表达式");
                    return alternativeCombined;
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "合并查询条件和复杂条件表达式时发生错误");
                // 详细记录异常栈
                MainForm.Instance.logger.LogError(ex.StackTrace);

                // 发生严重错误时，创建一个安全的后备条件，返回一个总是为true的表达式
                // 这样可以确保查询不会因为条件合并失败而完全无法执行
                Expression<Func<M, bool>> safeFallback = t => true;
                MainForm.Instance.logger.LogWarning("使用安全后备条件，所有记录都将被返回");

                return safeFallback;
            }
        }

        /// <summary>
        /// 检测现有条件中是否已包含相同的复杂条件
        /// 避免重复合并导致的表达式嵌套过深或重复计算
        /// </summary>
        /// <param name="existingCondition">现有查询条件</param>
        /// <param name="complexCondition">复杂条件表达式</param>
        /// <returns>是否包含重复的复杂条件</returns>
        private bool HasDuplicateComplexCondition(Expression<Func<M, bool>> existingCondition, Expression<Func<M, bool>> complexCondition)
        {
            try
            {
                // 获取表达式的调试视图字符串，用于比较
                string existingDebugView = GetExpressionDebugView(existingCondition);
                string complexDebugView = GetExpressionDebugView(complexCondition);

                // 检查现有条件是否已经包含了相同的复杂条件
                // 特别针对EvaluateComplexFilterExpression方法调用进行检查
                bool hasDuplicate = existingDebugView.Contains("EvaluateComplexFilterExpression") &&
                                   complexDebugView.Contains("EvaluateComplexFilterExpression");

                if (hasDuplicate)
                {
                    MainForm.Instance.logger.LogDebug("检测到重复的EvaluateComplexFilterExpression调用");
                }

                return hasDuplicate;
            }
            catch (Exception ex)
            {
                // 比较失败时默认为不重复，避免阻止正常的合并操作
                MainForm.Instance.logger.LogWarning(ex, "检测重复条件时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 获取表达式的调试视图字符串
        /// 用于表达式比较和调试
        /// </summary>
        /// <param name="expression">要获取调试视图的表达式</param>
        /// <returns>表达式的调试视图字符串</returns>
        private string GetExpressionDebugView(Expression expression)
        {
            try
            {
                // 使用反射获取Expression的DebugView属性
                var debugViewProperty = expression.GetType().GetProperty("DebugView",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (debugViewProperty != null)
                {
                    return debugViewProperty.GetValue(expression) as string ?? string.Empty;
                }

                // 如果无法获取DebugView，返回表达式的ToString()结果
                return expression.ToString();
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogWarning(ex, "获取表达式调试视图时发生错误");
                return expression.ToString();
            }
        }

        /// <summary>
        /// 合并现有查询条件和行级权限条件
        /// 优化的实现，提供更健壮的表达式合并逻辑和错误处理，避免重复条件合并
        /// </summary>
        /// <param name="existingCondition">现有查询条件</param>
        /// <param name="filterClause">行级权限过滤条件</param>
        /// <returns>合并后的查询条件</returns>
        private Expression<Func<M, bool>> MergeQueryConditions(Expression<Func<M, bool>> existingCondition, string filterClause)
        {
            try
            {
                // 检查filterClause是否为空或只包含恒真条件
                if (string.IsNullOrWhiteSpace(filterClause) ||
                    filterClause.Trim() == "(1=1)" || filterClause.Trim() == "1=1")
                {
                    // 如果是无条件或恒真条件，则不需要合并，直接使用现有条件
                    MainForm.Instance.logger.LogDebug("行级权限条件为空或恒真，无需合并");
                    return existingCondition;
                }

                if (existingCondition == null)
                {
                    MainForm.Instance.logger.LogDebug("现有条件为空，直接创建行级权限条件表达式");
                    return CreateRowAuthExpression(filterClause);
                }

                // 创建行级权限条件表达式
                Expression<Func<M, bool>> rowAuthExpression = CreateRowAuthExpression(filterClause);

                // 检测是否已有相同的复杂条件，避免重复合并
                if (HasDuplicateComplexCondition(existingCondition, rowAuthExpression))
                {
                    MainForm.Instance.logger.LogDebug("检测到重复的行级权限条件，跳过合并操作: {FilterClause}", filterClause);
                    return existingCondition;
                }

                MainForm.Instance.logger.LogDebug("准备合并现有查询条件和行级权限条件: {FilterClause}", filterClause);

                // 创建统一的参数表达式
                var parameter = Expression.Parameter(typeof(M), "t"); // 统一使用t作为参数名，与原始代码保持一致

                try
                {
                    // 从现有表达式中获取条件
                    var existingConditionBody = Expression.Invoke(existingCondition, parameter);

                    // 从行级权限表达式中获取条件
                    var rowAuthConditionBody = Expression.Invoke(rowAuthExpression, parameter);

                    // 创建AND组合表达式
                    var combinedExpression = Expression.AndAlso(existingConditionBody, rowAuthConditionBody);

                    // 创建最终的Lambda表达式
                    var combinedCondition = Expression.Lambda<Func<M, bool>>(combinedExpression, parameter);

                    MainForm.Instance.logger.LogDebug("成功合并现有查询条件和行级权限条件");
                    return combinedCondition;
                }
                catch (InvalidOperationException ioEx)
                {
                    // 捕获参数不匹配或表达式结构问题
                    MainForm.Instance.logger.LogWarning(ioEx, "使用标准方法合并表达式失败，尝试替代方法");

                    // 替代方法：使用编译后的委托进行条件组合
                    Expression<Func<M, bool>> alternativeCombined = t => existingCondition.Compile()(t) && rowAuthExpression.Compile()(t);

                    MainForm.Instance.logger.LogDebug("成功使用替代方法合并表达式");
                    return alternativeCombined;
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "合并查询条件和行级权限条件时发生错误");
                // 详细记录异常栈
                MainForm.Instance.logger.LogError(ex.StackTrace);

                // 发生严重错误时，创建一个安全的后备条件，返回一个总是为true的表达式
                // 这样可以确保查询不会因为条件合并失败而完全无法执行
                Expression<Func<M, bool>> safeFallback = t => true;
                MainForm.Instance.logger.LogWarning("使用安全后备条件，所有记录都将被返回");

                return safeFallback;
            }
        }

        /// <summary>
        /// 创建行级权限表达式
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>行级权限表达式</returns>
        private Expression<Func<M, bool>> CreateRowAuthExpression(string filterClause)
        {
            try
            {
                // 尝试直接解析行级权限条件
                var rowAuthExpression = DynamicExpressionParser.ParseLambda<M, bool>(
                    ParsingConfig.Default,
                    true,
                    filterClause);

                MainForm.Instance.logger.LogDebug("成功创建行级权限表达式");
                return rowAuthExpression;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogWarning(ex, "无法直接解析行级权限表达式，使用替代方案: {FilterClause}", filterClause);

                // 返回一个包含EvaluateComplexFilterExpression调用的表达式
                // 但在Query方法中我们会检测并在客户端处理这些表达式
                // 这样可以保留行级权限过滤的信息
                return t => EvaluateComplexFilterExpression(t, filterClause);
            }
        }

        /// <summary>
        /// 预处理过滤条件，移除可能导致问题的外层括号和标准化SQL语法
        /// 特别针对DynamicExpressionParser无法解析的复杂SQL表达式格式进行优化
        /// </summary>
        /// <param name="filterClause">原始过滤条件</param>
        /// <returns>预处理后的过滤条件</returns>
        private string PreprocessFilterClause(string filterClause)
        {
            if (string.IsNullOrWhiteSpace(filterClause))
                return filterClause;

            string processed = filterClause.Trim();

            // 第一步：移除外层括号（如果它们是配对的）
            processed = RemoveOuterBrackets(processed);

            // 第二步：标准化SQL语法格式
            processed = StandardizeSqlSyntax(processed);

            // 第三步：处理特殊情况
            processed = HandleSpecialCases(processed);

            MainForm.Instance.logger.LogDebug("预处理后的过滤条件: {ProcessedFilterClause}", processed);
            return processed;
        }

        /// <summary>
        /// 移除外层括号（如果它们是配对的）
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>处理后的过滤条件</returns>
        private string RemoveOuterBrackets(string filterClause)
        {
            string trimmed = filterClause.Trim();

            // 移除外层括号
            if (trimmed.StartsWith("(") && trimmed.EndsWith(")"))
            {
                int bracketCount = 1;
                for (int i = 1; i < trimmed.Length - 1; i++)
                {
                    if (trimmed[i] == '(')
                        bracketCount++;
                    else if (trimmed[i] == ')')
                        bracketCount--;

                    // 如果括号数量归零，说明外层括号不是一对
                    if (bracketCount == 0)
                        return filterClause;
                }

                // 如果括号数量最终为1，说明外层括号是一对，可以移除
                if (bracketCount == 1)
                    return trimmed.Substring(1, trimmed.Length - 2).Trim();
            }

            return filterClause;
        }

        /// <summary>
        /// 标准化SQL语法格式
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>标准化后的过滤条件</returns>
        private string StandardizeSqlSyntax(string filterClause)
        {
            string standardized = filterClause;

            // 处理常见的SQL语法标准化
            standardized = standardized.Replace("\"", "'"); // 将双引号替换为单引号
            standardized = standardized.Replace("''", "'"); // 处理重复的单引号

            // 移除多余的空格
            standardized = System.Text.RegularExpressions.Regex.Replace(standardized, @"\s+", " ");

            return standardized.Trim();
        }

        /// <summary>
        /// 处理特殊情况
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>处理后的过滤条件</returns>
        private string HandleSpecialCases(string filterClause)
        {
            string result = filterClause;

            // 处理特定的SQL函数格式问题
            // 例如处理DateTime函数格式
            if (result.Contains("DateTime."))
            {
                // 简单的示例，实际可能需要更复杂的处理
                result = result.Replace("DateTime.Now", "DateTime.Now");
            }

            return result;
        }

        /// <summary>
        /// 评估复杂的过滤表达式，用于处理DynamicExpressionParser无法直接解析的SQL语法
        /// 这个方法会使用EF Core来执行复杂的SQL表达式，而不是尝试将其解析为Lambda表达式
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>是否满足过滤条件</returns>
        private bool EvaluateComplexFilterExpression(M entity, string filterClause)
        {
            try
            {
                MainForm.Instance.logger.LogDebug("评估复杂过滤表达式: {FilterClause}", filterClause);

                // 获取实体主键值
                string PKCol = BaseUIHelper.GetEntityPrimaryKey(typeof(M));
                long entityId = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);

                // 执行专门的评估方法来处理包含EXISTS子查询的复杂表达式
                bool result = EvaluateExistsFilterExpression(entityId, filterClause);
                MainForm.Instance.logger.LogDebug("复杂过滤表达式评估结果: {Result}", result);
                return result;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "评估复杂过滤表达式时发生错误: {FilterClause}", filterClause);
                // 发生错误时默认返回true，确保查询不会被过度限制
                return true;
            }
        }


        /// <summary>
        /// 评估包含EXISTS子查询的过滤表达式
        /// 基于SqlSugar框架优化的实现，提供更健壮的复杂SQL表达式处理
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>是否满足过滤条件</returns>
        private bool EvaluateExistsFilterExpression(object entityId, string filterClause)
        {
            try
            {
                // 获取实体类型和表名
                var entityType = typeof(M);
                string tableName = GetTableName(entityType);

                if (string.IsNullOrEmpty(tableName))
                {
                    MainForm.Instance.logger.LogWarning("无法获取实体类型 {EntityType} 的表名，跳过复杂表达式评估", entityType.FullName);
                    return true;
                }

                // 获取主键列名
                string primaryKeyColumn = BaseUIHelper.GetEntityPrimaryKey(typeof(M));

                if (string.IsNullOrEmpty(primaryKeyColumn))
                {
                    MainForm.Instance.logger.LogWarning("无法获取实体类型 {EntityType} 的主键列名，跳过复杂表达式评估", entityType.FullName);
                    return true;
                }

                // 构建包含WHERE子句的完整SQL查询
                // 处理SQL表达式，确保其适合直接执行
                string processedFilterClause = ProcessSqlForDirectExecution(filterClause, tableName);

                // 构建检查记录是否存在的SQL
                string checkExistsSql = $"SELECT COUNT(1) FROM {tableName} WHERE {primaryKeyColumn} = @EntityId AND ({processedFilterClause})";

                MainForm.Instance.logger.LogDebug("执行SQL检查实体权限: {Sql}", checkExistsSql);

                // 安全检查SQL语句
                if (!IsSafeSql(checkExistsSql))
                {
                    MainForm.Instance.logger.LogWarning("检测到不安全的SQL语句，跳过执行: {Sql}", checkExistsSql);
                    return true;
                }

                // 使用参数化查询避免SQL注入
                var parameters = new { EntityId = entityId };

                // 执行SQL查询并获取结果
                int count = 0;
                try
                {
                    count = MainForm.Instance.AppContext.Db.Ado.SqlQuery<int>(checkExistsSql, parameters).FirstOrDefault();
                }
                catch (SqlSugar.SqlSugarException sqlEx)
                {
                    // 专门处理SqlSugar相关异常
                    MainForm.Instance.logger.LogError(sqlEx, "执行SqlSugar查询时发生SQL错误: {Sql}", checkExistsSql);
                    // 记录SQL错误详情
                    if (sqlEx.InnerException != null)
                    {
                        MainForm.Instance.logger.LogError(sqlEx.InnerException, "SQL错误详情");
                    }
                    return true;
                }

                MainForm.Instance.logger.LogDebug("EXISTS子查询执行结果: 找到 {Count} 条记录", count);
                return count > 0;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "执行EXISTS过滤表达式时发生错误: {FilterClause}", filterClause);
                // 详细记录异常栈
                MainForm.Instance.logger.LogError(ex.StackTrace);
                return true; // 失败时返回true，确保不阻止查询
            }
        }

        /// <summary>
        /// 获取实体类型对应的数据库表名
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>表名</returns>
        private string GetTableName(Type entityType)
        {
            try
            {
                // 尝试通过特性获取表名
                var tableAttribute = entityType.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), true)
                    .FirstOrDefault() as System.ComponentModel.DataAnnotations.Schema.TableAttribute;

                if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name))
                {
                    return tableAttribute.Name;
                }

                // 默认使用实体名称作为表名
                return entityType.Name;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "获取表名时发生错误");
                return entityType.Name;
            }
        }



        /// <summary>
        /// 检查SQL语句是否安全（防止SQL注入）
        /// </summary>
        /// <param name="sql">要检查的SQL语句</param>
        /// <returns>SQL语句是否安全</returns>
        private bool IsSafeSql(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return false;

            string lowerSql = sql.ToLower();

            // 基本的SQL注入检测
            string[] dangerousKeywords = new string[]
            {
                "drop ", "truncate ", "alter ", "exec ", "execute ", "insert ", "update ", "delete ",
                "create ", "grant ", "revoke ", "sp_"
            };

            foreach (string keyword in dangerousKeywords)
            {
                if (lowerSql.Contains(keyword))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 处理SQL语句，使其适合直接执行
        /// 针对复杂SQL表达式（如EXISTS子查询）进行特殊处理
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <param name="tableName">实体表名</param>
        /// <returns>处理后的SQL语句</returns>
        private string ProcessSqlForDirectExecution(string filterClause, string tableName = null)
        {
            if (string.IsNullOrEmpty(filterClause))
                return "1=1";

            string processed = filterClause.Trim();

            try
            {
                // 处理常见的SQL语法问题
                processed = processed.Replace("'", "''"); // 转义单引号

                // 处理EXISTS子查询中的表别名问题
                if (processed.ToLower().Contains("exists"))
                {
                    // 这里可以根据需要添加更复杂的EXISTS子查询处理逻辑
                    // 例如，确保子查询中的表名正确引用
                    MainForm.Instance.logger.LogDebug("处理EXISTS子查询表达式");
                }

                // 如果提供了表名，可以进行表名替换或其他处理
                if (!string.IsNullOrEmpty(tableName))
                {
                    // 这里可以添加表名相关的处理逻辑
                }

                // 确保表达式以有效的条件开始和结束
                if (!processed.StartsWith("(") && !processed.EndsWith(")"))
                {
                    // 检查是否已经是一个有效的条件表达式
                    if (!processed.Contains("="))
                    {
                        MainForm.Instance.logger.LogWarning("过滤条件可能不是有效的SQL表达式: {FilterClause}", processed);
                    }
                }

                MainForm.Instance.logger.LogDebug("处理后的SQL表达式: {ProcessedSql}", processed);
                return processed;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理SQL语句时发生错误");
                return "1=1";
            }
        }

        /// <summary>
        /// 处理SQL语句，使其适合直接执行
        /// 兼容旧版本方法签名，实际调用新的带表名参数的重载方法
        /// </summary>
        /// <param name="filterClause">过滤条件</param>
        /// <returns>处理后的SQL语句</returns>
        private string ProcessSqlForDirectExecution(string filterClause)
        {
            try
            {
                // 调用新的重载方法，使用默认表名参数
                // 这样可以确保所有SQL处理都使用相同的、更完善的逻辑
                return ProcessSqlForDirectExecution(filterClause, null);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理SQL语句时发生错误（兼容模式）");
                return filterClause; // 失败时返回原始表达式
            }
        }




        /// <summary>
        /// whereLambda
        /// Expression<Func<User, bool>> condition1 = t => t.Name.Contains("张");
        //// Expression<Func<User, bool>> condition2 = t => t.Age > 18;
        // Expression<Func<User, bool>> condition3 = t => t.Gender == "男";
        // Expression<Func<User, bool>> condition4 = t => t.Money > 1000;
        //var lambda = condition1.And(condition2).And(condition3).Or(condition4);
        // var users = UserDbContext.Query(lambda);
        ///对一个实体查询时，额外的条件 一般添加在ui中，没有在process中
        /// 只是针对查询结果的限制
        /// 也可以对process中字段添加子限制来过滤加载的项目
        /// </summary>
        public Expression<Func<M, bool>> LimitQueryConditions { get; set; }


        /// <summary>
        /// 这个方法是为了 打开这个菜单是来自不同的方式。他会传过来一些参数。
        /// 比方todolist 但是标题没有传过来。用现有的标题
        /// </summary>
        /// <param name="QueryParameters"></param>
        /// <param name="nodeParameter"></param>
        internal override void LoadQueryParametersToUI(object QueryParameters, QueryParameter nodeParameter)
        {
            if (QueryParameters != null)
            {
                QueryDtoProxy = QueryParameters;
            }
            if (nodeParameter != null)
            {
                if (nodeParameter.queryFilter != null)
                {
                    //比方todolist 但是标题没有传过来。用现有的标题
                    if (QueryConditionFilter.QueryFields != null)
                    {
                        foreach (var item in QueryConditionFilter.QueryFields)
                        {
                            if (nodeParameter.queryFilter.QueryFields != null)
                            {
                                var qf = nodeParameter.queryFilter.QueryFields.FirstOrDefault(c => c.FieldName == item.FieldName);
                                if (qf != null)
                                {
                                    qf.Caption = item.Caption;
                                }
                            }
                        }
                    }
                    QueryConditionFilter = nodeParameter.queryFilter;
                }
                // 确保QueryDtoProxy不为空
                if (QueryDtoProxy == null)
                {
                    QueryDtoProxy = LoadQueryConditionToUI();
                }

                // 查询条件给值前先将条件清空，合并时间条件处理逻辑
                foreach (var item in nodeParameter.queryFilter.QueryFields)
                {
                    if (item.FKTableName.IsNotEmptyOrNull() && item.IsRelated)
                    {
                        QueryDtoProxy.SetPropertyValue(item.FieldName, -1L);
                        continue;
                    }

                    // 合并时间类型字段的处理，包括代理类中的区间字段和UI控件
                    if (item.FieldPropertyInfo.PropertyType.Name == "DateTime" ||
                        (item.FieldPropertyInfo.PropertyType.IsGenericType &&
                         item.FieldPropertyInfo.PropertyType.GetBaseType().Name == "DateTime"))
                    {
                        QueryDtoProxy.SetPropertyValue(item.FieldName, null);
                        if (QueryDtoProxy.ContainsProperty(item.FieldName + "_Start"))
                        {
                            QueryDtoProxy.SetPropertyValue(item.FieldName + "_Start", null);
                        }
                        if (QueryDtoProxy.ContainsProperty(item.FieldName + "_End"))
                        {
                            QueryDtoProxy.SetPropertyValue(item.FieldName + "_End", null);
                        }

                        // 同步UI控件状态
                        var fieldNameControl = kryptonPanelQuery.Controls.Find(item.FieldName, true);
                        if (fieldNameControl != null && fieldNameControl.Length > 0 &&
                            fieldNameControl[0] is UCAdvDateTimerPickerGroup timerPickerGroup)
                        {
                            timerPickerGroup.dtp1.Checked = false;
                            timerPickerGroup.dtp2.Checked = false;
                        }
                        continue;
                    }
                }
                //传入查询对象的实例，有两种情况，一种是直接指定的条件。一种是 后面重新组合的  主要是 or， 如 应收中的部分支付和待支付都是要回款的。

                if (nodeParameter.conditionals is List<IConditionalModel>)
                {
                    if (nodeParameter.conditionals[0] is ConditionalCollections)
                    {
                        #region 组合查询
                        for (int i = 0; i < nodeParameter.conditionals.Count; i++)
                        {
                            if (nodeParameter.conditionals[i] is ConditionalCollections)
                            {
                                ConditionalCollections ccs = nodeParameter.conditionals[i] as ConditionalCollections;
                                for (int c = 0; c < ccs.ConditionalList.Count; c++)
                                {
                                    ConditionalModel item = ccs.ConditionalList[c].Value;
                                    if (item.ConditionalType == ConditionalType.Equal)
                                    {
                                        switch (item.CSharpTypeName)
                                        {
                                            case "int":
                                                QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue.ToInt());
                                                break;
                                            case "long":
                                                QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue.ToLong());
                                                break;
                                            case "bool":
                                                QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue.ToBool());
                                                break;
                                            default:
                                                QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue);
                                                break;
                                        }
                                    }

                                }
                            }

                            else
                            {
                                //KeyValuePair<WhereType, ConditionalModel> kv = new KeyValuePair<WhereType, ConditionalModel>(WhereType.And, item.Conditions[i] as ConditionalModel);
                                //ConditionalList.Add(kv);
                                //要进一步测试
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        #region 直接查询
                        foreach (ConditionalModel item in nodeParameter.conditionals)
                        {
                            if (item.ConditionalType == ConditionalType.Equal)
                            {
                                switch (item.CSharpTypeName)
                                {
                                    case "int":
                                        QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue.ToInt());
                                        break;
                                    case "long":
                                        QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue.ToLong());
                                        break;
                                    case "bool":
                                        QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue.ToBool());
                                        break;
                                    default:
                                        QueryDtoProxy.SetPropertyValue(item.FieldName, item.FieldValue);
                                        break;
                                }
                            }
                        }
                        #endregion
                    }

                }
            }

            // 使用Control.BeginInvoke确保在UI线程上执行查询，避免使用固定延迟
            // 利用Application.Idle事件确保在UI空闲时执行查询，避免异步时序问题
            this.BeginInvoke(new Action(() =>
            {
                // 注册空闲事件，确保在UI线程空闲时执行查询
                // 这样可以保证所有条件设置和UI更新都已完成
                System.EventHandler idleHandler = null;
                idleHandler = new System.EventHandler(async (sender, e) =>
                {
                    // 移除事件处理程序以避免重复执行
                    Application.Idle -= idleHandler;

                    // 短暂延迟确保所有UI更新完成
                    await Task.Delay(150);

                    // 确保在执行查询前，参数已经正确加载到UI控件并与QueryDtoProxy同步
                    // 这里UIQuery设置为true，让Query方法执行UI验证，确保控件与数据模型的一致性
                    Query(QueryDtoProxy, true);
                });

                // 添加空闲事件处理程序
                Application.Idle += idleHandler;
            }));

        }

        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询
        /// 优化版本：添加完善的异常处理、详细日志记录和取消支持
        /// </summary>
        /// <param name="QueryDto">查询参数实体 ：查询条件赋值给实体，方法中提取日期等条件</param>
        /// <param name="UIQuery">如果条件来自UI时要验证UI，否则不验证</param>
        [MustOverride]
        protected async virtual void Query(object QueryDto, bool UIQuery = true)
        {
            // 添加取消令牌支持
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                // 记录查询开始
                MainForm.Instance.logger.LogDebug("开始执行查询，实体类型: {EntityType}", typeof(M).Name);

                if (UIQuery)
                {
                    // 验证UI控件
                    this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

                    if (ValidationHelper.hasValidationErrors(this.Controls))
                    {
                        MainForm.Instance.logger.LogWarning("UI验证失败，取消查询");
                        return;
                    }
                }

                // 验证必要参数
                if (QueryDto == null)
                {
                    MainForm.Instance.logger.LogWarning("查询参数为空，取消查询");
                    return;
                }

                // 获取控制器
                BaseController<M> ctr = null;
                try
                {
                    ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");
                    if (ctr == null)
                    {
                        MainForm.Instance.logger.LogError("无法获取控制器: {ControllerName}", typeof(M).Name + "Controller");
                        throw new InvalidOperationException($"无法获取控制器: {typeof(M).Name}Controller");
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "获取控制器时发生错误");
                    throw;
                }

                // 获取分页参数
                int pageNum = 1;
                int pageSize = 0;
                try
                {
                    pageSize = int.Parse(txtMaxRow.Text);
                    if (pageSize <= 0 || pageSize > 5000)
                    {
                        MainForm.Instance.logger.LogWarning("无效的页面大小: {PageSize}，使用默认值200", pageSize);
                        pageSize = 200; // 默认值
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogWarning(ex, "解析页面大小时发生错误，使用默认值200");
                    pageSize = 200;
                }

                // 提取查询条件列名
                List<string> queryConditions = new List<string>();
                try
                {
                    if (QueryConditionFilter != null && QueryConditionFilter.QueryFields != null)
                    {
                        queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
                        MainForm.Instance.logger.LogDebug("提取查询条件字段: {FieldsCount}个", queryConditions.Count);
                    }
                    else
                    {
                        MainForm.Instance.logger.LogWarning("QueryConditionFilter或QueryFields为空");
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "提取查询条件时发生错误");
                }

                // 初始化过滤表达式列表
                if (QueryConditionFilter.FilterLimitExpressions == null)
                {
                    QueryConditionFilter.FilterLimitExpressions = new List<LambdaExpression>();
                }

                // 应用限制查询条件
                if (LimitQueryConditions != null && !QueryConditionFilter.FilterLimitExpressions.Contains(LimitQueryConditions))
                {
                    QueryConditionFilter.FilterLimitExpressions.Add(LimitQueryConditions);
                    MainForm.Instance.logger.LogDebug("应用了LimitQueryConditions限制条件");
                }

                // 获取并应用行级权限过滤
                string filterClause = string.Empty;
                try
                {
                    var rowAuthService = Startup.GetFromFac<IRowAuthService>();
                    if (rowAuthService != null)
                    {
                        Type entityType = typeof(M);
                        filterClause = rowAuthService.GetUserRowAuthFilterClause(entityType, CurMenuInfo.MenuID);
                        MainForm.Instance.logger.LogDebug("获取行级权限过滤条件: {FilterClause}", filterClause);
                    }
                    else
                    {
                        MainForm.Instance.logger.LogWarning("无法获取行级权限服务");
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "获取行级权限过滤条件时发生错误");
                    // 不中断查询，但使用空过滤条件
                }

                // 执行查询（带超时控制）
                List<M> rawList = null;
                try
                {
                    // 设置查询超时（30秒）
                    cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));

                    // 在异步方法中支持取消
                    var queryTask = ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDto, pageNum, pageSize, filterClause);
                    rawList = await Task.Run(async () =>
                    {
                        return await queryTask as List<M>;
                    }, cancellationTokenSource.Token);

                    MainForm.Instance.logger.LogDebug("查询执行完成，返回记录数: {Count}", rawList?.Count ?? 0);
                }
                catch (OperationCanceledException)
                {
                    MainForm.Instance.logger.LogWarning("查询操作已超时取消");
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("查询执行超时，请调整查询条件重试", "查询超时", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                    return;
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "执行查询时发生错误");
                    // 详细记录异常栈
                    MainForm.Instance.logger.LogError(ex.StackTrace);

                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show($"查询执行失败: {ex.Message}", "查询错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                    return;
                }

                // 处理查询结果
                try
                {
                    List<M> list = rawList ?? new List<M>();

                    // 确保在UI线程更新绑定源
                    this.BeginInvoke(new Action(() =>
                    {
                        _UCBillMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();
                        _UCBillMasterQuery.ShowSummaryCols();

                        // 控制打印按钮可见性
                        toolStripSplitButtonPrint.Visible = list.Count > 0;

                        // 处理结果分析（如果启用）
                        if (ResultAnalysis && _UCOutlookGridAnalysis1 != null)
                        {
                            _UCOutlookGridAnalysis1.ColDisplayTypes = new List<Type>();
                            _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(M));
                            _UCOutlookGridAnalysis1.FieldNameList = _UCBillMasterQuery.newSumDataGridViewMaster.FieldNameList;
                            _UCOutlookGridAnalysis1.bindingSourceOutlook.DataSource = list;
                            _UCOutlookGridAnalysis1.ColumnDisplays = _UCBillMasterQuery.newSumDataGridViewMaster.ColumnDisplays;
                            _UCOutlookGridAnalysis1.LoadDataToGrid<M>(list);
                        }

                        MainForm.Instance.logger.LogDebug("查询结果已绑定到UI控件");
                    }));
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "绑定查询结果到UI时发生错误");
                }
            }
            catch (Exception ex)
            {
                // 捕获所有未处理的异常
                MainForm.Instance.logger.LogError(ex, "查询过程中发生未预期的错误");

                // 在UI线程显示错误消息
                this.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show($"查询过程中发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
            finally
            {
                // 清理资源
                cancellationTokenSource.Dispose();
                MainForm.Instance.logger.LogDebug("查询过程完成，资源已清理");
            }
        }

        public List<Expression<Func<M, object>>> QueryConditions = new List<Expression<Func<M, object>>>();




        public void Builder()
        {

            BuildInvisibleCols();

            //先添加主要的条件 在processor中
            QueryConditionBuilder();

            //再添加UI上额外的情况
            BuildLimitQueryConditions();


            BuildColNameDataDictionary();

            BuildSummaryCols();
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// 条件如果描述为空 不会生效
        /// QueryConditions
        /// QueryParameters
        /// 目前支持上面两种，不能同时使用
        /// </summary>
        public virtual void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(M).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //如果添加了全局性重复的字段名为查询条件则加到日志中。便于查找
            //QueryConditionFilter.QueryFields中的FieldName有相同的数据。则找出来
            var queryFields = QueryConditionFilter.QueryFields.GroupBy(c => c.FieldName).Where(c => c.Count() > 1).ToList();
            if (queryFields != null && queryFields.Count > 0)
            {
                MainForm.Instance.logger.LogError($"{typeof(M).Name}Processor 设置了重复查询条件");
            }

            //添加默认全局的
            // base.QueryConditions.Add(c => c.Created_by);
            // List<string> slist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
        }

        /// <summary>
        /// 创建右键菜单
        /// </summary>
        public virtual void BuildContextMenuController()
        {

        }

        /// <summary>
        /// 设置不可见的列
        /// </summary>
        public virtual void BuildInvisibleCols()
        {
            //M 主表的主键不显示
            //下面已经统一处理了。
            //ChildRelatedInvisibleCols
            //找到子表中对应的主表的主键列，设置为不可见,下面统一处理了。
        }

        /// <summary>
        /// 设置加总的列
        /// </summary>
        public virtual void BuildSummaryCols()
        {
            ////添加默认全局的
            //// base.QueryConditions.Add(c => c.Created_by);

            //List<string> mlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            //_UCBillMasterQuery.SummaryCols = mlist;

            //List<string> clist = ExpressionHelper.ExpressionListToStringList(ChildSummaryCols);
            //_UCBillChildQuery.SummaryCols = clist;
        }

        public static string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.LayoutConfigDirectory);
        string xmlfilepath = System.IO.Path.Combine(basePath, "QueryMC" + typeof(M).Name + "Persistence.xml");

        protected virtual async Task Exit(object thisform)
        {
            try
            {
                if (_UCBillMasterQuery != null && _UCBillMasterQuery.newSumDataGridViewMaster != null)
                {
                  await  UIBizService.SaveGridSettingData(CurMenuInfo, _UCBillMasterQuery.newSumDataGridViewMaster, typeof(M));
                }
                if (_UCBillChildQuery != null && _UCBillChildQuery.newSumDataGridViewChild != null)
                {
                await    UIBizService.SaveGridSettingData(CurMenuInfo, _UCBillChildQuery.newSumDataGridViewChild, typeof(C));
                }


                //保存配置
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.NewLineChars = Environment.NewLine;
                settings.Indent = true;


                if (ws != null)
                {
                    using XmlWriter xmlWriter = XmlWriter.Create(xmlfilepath, settings);
                    {
                        ws.SaveElementToXml(xmlWriter);
                        xmlWriter.Close();
                    }
                }

                //保存超级用户的布局为默认布局
                if (MainForm.Instance.AppContext.IsSuperUser && System.IO.File.Exists(xmlfilepath))
                {
                    //加载XML文件
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmlfilepath);
                    //获取XML字符串
                    string xmlStr = xmldoc.InnerXml;
                    //字符串转XML
                    //xmldoc.LoadXml(xmlStr);
                    CurMenuInfo.DefaultLayout = xmlStr;
                    await MainForm.Instance.AppContext.Db.Storageable<tb_MenuInfo>(CurMenuInfo).ExecuteReturnEntityAsync();

                }

            }
            catch (Exception ex)
            {


            }
            //退出
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
                        // 使用同步方式调用异步的Exit方法
                        Task.Run(async () => await Exit(this)).Wait();
                        break;
                    case Keys.F1:
                        // 显示帮助 - 优先显示当前焦点控件的帮助
                        if (HelpManager.Config.IsHelpSystemEnabled)
                        {
                            // 对于UserControl，我们需要找到主窗体来显示帮助
                            Form mainForm = this.FindForm();
                            if (mainForm != null)
                            {
                                var focusedControl = mainForm.ActiveControl;
                                HelpManager.ShowHelpForControl(mainForm, focusedControl);
                            }
                            else
                            {
                                // 如果找不到主窗体，显示基于类型的帮助
                                HelpManager.ShowHelpByType(this.GetType());
                            }
                        }
                        return true;
                    case Keys.F2:
                        // 显示帮助系统主窗体
                        if (HelpManager.Config.IsHelpSystemEnabled)
                        {
                            Form mainForm = this.FindForm();
                            if (mainForm != null)
                            {
                                mainForm.ShowHelpSystemForm();
                            }
                        }
                        return true;
                    case Keys.Enter:
                        Query(QueryDtoProxy);
                        toolStripSplitButtonPrint.Enabled = true;
                        break;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        public KryptonDockingWorkspace ws;
        KryptonCheckBox cbbatch = new KryptonCheckBox();
        private async void BaseBillQueryMC_Load(object sender, EventArgs e)
        {

                MainForm.Instance.AppContext.log.ActionName = sender.ToString();
                await UIBizService.RequestCache<M>();
                await UIBizService.RequestCache<C>();
                //去检测产品视图的缓存并且转换为强类型
                await UIBizService.RequestCache(typeof(View_ProdDetail));
                Builder();
                this.CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(M).Name && m.ClassPath == this.ToString()).FirstOrDefault();
                if (CurMenuInfo == null && !MainForm.Instance.AppContext.IsSuperUser)
                {
                    MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                    return;
                }
                if (!this.DesignMode)
                {
                    #region 添加控件内容
                    ws = kryptonDockingManagerQuery.ManageWorkspace(kryptonDockableWorkspaceQuery);
                    kryptonDockingManagerQuery.ManageControl(kryptonPanelMainBig, ws);
                    kryptonDockingManagerQuery.ManageFloating(MainForm.Instance);

                    //创建面板并加入
                    KryptonPageCollection Kpages = new KryptonPageCollection();
                    if (Kpages.Count == 0)
                    {
                        Kpages.Add(MasterQuery());
                        if (HasChildData)
                        {
                            Kpages.Add(ChildQuery());
                        }
                        if (ChildRelatedEntityType == null)
                        {
                            ChildRelatedEntityType = typeof(C);
                        }
                        if (this.ChildRelatedEntityType != null)
                        {
                            Kpages.Add(Child_RelatedQuery());
                        }
                        //如果需要分析功能
                        if (ResultAnalysis)
                        {
                            Kpages.Add(UCOutlookGridAnalysis1Load());
                        }
                    }

                    //加载布局
                    try
                    {
                        if (!Directory.Exists(basePath))
                        {
                            Directory.CreateDirectory(basePath);
                        }
                        if (System.IO.File.Exists(xmlfilepath) && AuthorizeController.GetQueryPageLayoutCustomize(MainForm.Instance.AppContext))
                        {
                            #region load
                            // Create the XmlNodeReader object.
                            XmlDocument doc = new XmlDocument();
                            doc.Load(xmlfilepath);
                            XmlNodeReader nodeReader = new XmlNodeReader(doc);
                            // Set the validation settings.
                            XmlReaderSettings settings = new XmlReaderSettings();

                            using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                            {
                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
                                    {
                                        //加载停靠信息
                                        ws.LoadElementFromXml(reader, Kpages);
                                    }
                                }

                            }
                            #endregion
                        }
                        else
                        {
                            if (CurMenuInfo == null)
                            {
                                //找不到当前菜单直接返回
                                return;
                            }

                            //没有个性化文件时用默认的
                            if (!string.IsNullOrEmpty(CurMenuInfo.DefaultLayout))
                            {
                                #region load
                                //加载XML文件
                                XmlDocument xmldoc = new XmlDocument();
                                //获取XML字符串
                                string xmlStr = xmldoc.InnerXml;
                                //字符串转XML
                                xmldoc.LoadXml(CurMenuInfo.DefaultLayout);

                                XmlNodeReader nodeReader = new XmlNodeReader(xmldoc);
                                XmlReaderSettings settings = new XmlReaderSettings();
                                using (XmlReader reader = XmlReader.Create(nodeReader, settings))
                                {
                                    while (reader.Read())
                                    {
                                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DW")
                                        {
                                            //加载停靠信息
                                            ws.LoadElementFromXml(reader, Kpages);
                                        }
                                    }

                                }
                                #endregion
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog("加载查询页布局配置文件出错。" + ex.Message, Global.UILogType.错误);
                        MainForm.Instance.logger.LogError(ex, "加载查询页布局配置文件出错。");
                    }

                    //如果加载过的停靠信息中不正常。就手动初始化
                    foreach (KryptonPage page in Kpages)
                    {
                        if (!(page is KryptonStorePage) && !kryptonDockingManagerQuery.ContainsPage(page.UniqueName))
                        {
                            switch (page.UniqueName)
                            {
                                case "查询条件":
                                    //kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                                    kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Top, Kpages.Where(p => p.UniqueName == "查询条件").ToArray());
                                    break;
                                case "明细信息":
                                    // kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Left, Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                                    kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "明细信息").ToArray());
                                    break;
                                case "单据信息":
                                    kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "单据信息").ToArray());
                                    break;
                                case "结果分析":
                                    kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "结果分析").ToArray());
                                    break;
                                case "关联信息":
                                    if (ChildRelatedEntityType != null)
                                    {
                                        kryptonDockingManagerQuery.AddToWorkspace("Workspace", Kpages.Where(p => p.UniqueName == "关联信息").ToArray());
                                    }
                                    break;
                            }
                        }
                    }
                    #endregion
                }
            if (this.DesignMode)
            {
                return;
            }
            else
            {
                //设置的主键指向的编号在具体业务的查询窗体的构造函数中设置的。

                if (_UCBillMasterQuery.GridRelated.RelatedInfoList != null && RelatedBillEditCol != null)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                    {
                        // MainForm.Instance.logger.LogDebug("当前查询没有设置指向列，自动设置为主表类型及列");
                    }
                    _UCBillMasterQuery.GridRelated.SetRelatedInfo(typeof(M).Name, RelatedBillEditCol.GetMemberInfo().Name);
                }
                if (QueryDtoProxy==null)
                {
                    QueryDtoProxy = LoadQueryConditionToUI();
                }
            }


            List<M> list = new List<M>();
            _UCBillMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            _UCBillMasterQuery.OnSelectDataRow += _UCBillMasterQuery_OnSelectDataRow;



            _UCBillMasterQuery.ShowSummaryCols();

            //设置默认焦点
            tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
            if (menuSetting != null)
            {
                if (menuSetting.tb_UIQueryConditions != null && menuSetting.tb_UIQueryConditions.Count > 0)
                {
                    var queryConditionFocused = menuSetting.tb_UIQueryConditions.FirstOrDefault(c => c.Focused == true);
                    if (queryConditionFocused != null)
                    {
                        var controls = kryptonPanelQuery.Controls.Find(queryConditionFocused.FieldName, true);
                        if (controls.Length > 0)
                        {
                            var control = controls[0];
                            if (control is KryptonTextBox ktxt)
                            {
                                if (ktxt.CanFocus)
                                {
                                    ktxt.Focus();
                                }
                            }
                            if (control is KryptonComboBox kcb)
                            {
                                if (kcb.CanFocus)
                                {
                                    kcb.Focus();
                                }
                            }
                        }
                    }
                }
            }

            if (_UCBillMasterQuery != null)
            {
                _UCBillMasterQuery.newSumDataGridViewMaster.NeedSaveColumnsXml = false;
                BaseMainDataGridView = _UCBillMasterQuery.newSumDataGridViewMaster;
                await UIBizService.SetGridViewAsync(typeof(M), BaseMainDataGridView, CurMenuInfo, false, _UCBillMasterQuery.InvisibleCols, _UCBillMasterQuery.DefaultHideCols);

            }
            if (_UCBillChildQuery != null)
            {
                _UCBillChildQuery.newSumDataGridViewChild.NeedSaveColumnsXml = false;
                BaseSubDataGridView = _UCBillChildQuery.newSumDataGridViewChild;
                await UIBizService.SetGridViewAsync(typeof(C), BaseSubDataGridView, CurMenuInfo, false, _UCBillChildQuery.InvisibleCols, _UCBillChildQuery.DefaultHideCols);
            }

            if (_UCBillChildQuery_Related != null && _UCBillChildQuery_Related.newSumDataGridViewChild != null)
            {
                _UCBillChildQuery_Related.newSumDataGridViewChild.NeedSaveColumnsXml = false;
            }

            //调用了_UCBillMasterQuery中的表格所以要放到后面不然为空
            BuildContextMenuController();

            //查询不会有
            AddExcludeMenuList(MenuItemEnums.复制性新增);
        
        }

 

        private async void _UCBillMasterQuery_OnSelectDataRow(object entity, object bizKey)
        {
            if (_UCBillChildQuery == null)
            {
                return;
            }

            if (entity == null)
            {
                return;
            }
            var obj = (entity as M);
            List<C> list = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, typeof(C).Name + "s") as List<C>;
            if (list == null)
            {
                //M主为视图C子为表时
                if (typeof(M).Name.Contains("View"))
                {
                    string ChildFKColName = string.Empty;
                    string MasterPKColName = string.Empty;
                    foreach (var field in typeof(C).GetProperties())
                    {
                        //获取指定类型的自定义特性
                        object[] attrs = field.GetCustomAttributes(false);
                        foreach (var attr in attrs)
                        {
                            if (attr is FKRelationAttribute)
                            {
                                FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                                if (fkrattr.FKTableName == typeof(C).Name.Replace("Detail", ""))
                                {
                                    ChildFKColName = field.Name;
                                    MasterPKColName = fkrattr.FK_IDColName;
                                    break;
                                }
                            }
                        }
                    }
                    string pkid = obj.GetType().GetProperty(MasterPKColName).GetValue(obj).ToString();

                    //设置动态表达式
                    StaticConfig.DynamicExpressionParserType = typeof(DynamicExpressionParser);

                    //子表都是通过主表的主键关联的。与单号无关
                    BaseController<C> ctrDetail = Startup.GetFromFacByName<BaseController<C>>(typeof(C).Name + "Controller");

                    var conModels = new List<IConditionalModel>();
                    conModels.Add(new ConditionalModel
                    {
                        FieldName = ChildFKColName,
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = pkid
                    });

                    var listDeatails = await ctrDetail.BaseGetQueryableAsync()
                     .Where(conModels).ToListAsync();
                    if (listDeatails != null)
                    {
                        _UCBillChildQuery.bindingSourceChild.DataSource = listDeatails;
                        _UCBillChildQuery.newSumDataGridViewChild.DataSource = listDeatails;
                        ShowChildSum();
                    }
                }
            }
            else
            {
                _UCBillChildQuery.bindingSourceChild.DataSource = list.ToBindingSortCollection();
                _UCBillChildQuery.newSumDataGridViewChild.DataSource = list.ToBindingSortCollection();
                ShowChildSum();
            }

            //相关引用单据明细
            if (OnQueryRelatedChild != null)
            {
                OnQueryRelatedChild(entity, _UCBillChildQuery_Related.bindingSourceChild);
            }

        }

        public void ShowChildSum()
        {
            _UCBillChildQuery.newSumDataGridViewChild.IsShowSumRow = true;
            _UCBillChildQuery.newSumDataGridViewChild.SumColumns = _UCBillChildQuery.SummaryCols.ToArray();
        }

        private KryptonPage NewPage(string name, int image, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage();
            p.Text = name;
            p.TextTitle = name;
            p.TextDescription = name;
            p.UniqueName = p.Text;
            // p.ImageSmall = imageListSmall.Images[image];

            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);

            // _count++;
            return p;
        }



        /// <summary>
        /// 生成查询条件，并返回查询条件代理实体参数
        /// </summary>
        /// <param name="useLike">true：默认不是模糊查询</param>
        internal override object LoadQueryConditionToUI(decimal QueryConditionShowColQty = 5)
        {
            Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器 = kryptonPanelQuery;
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();
            if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            {
                base.QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
            }
            else
            {
                if (CurMenuInfo == null)
                {
                    MessageBox.Show(this.ToString() + "当前菜单不能为空，或无操作权限，请联系管理员。");
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
                        base.QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryConditionFilter, menuSetting);
                    }
                    else
                    {
                        QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
                    }
                }
            }

            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
            return QueryDtoProxy;
        }


        /// <summary>
        /// 主表要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> MasterSummaryCols { get; set; } = new List<Expression<Func<M, object>>>();

        /// <summary>
        /// 明细表要统计的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildSummaryCols { get; set; } = new List<Expression<Func<C, object>>>();


        /// <summary>
        /// 明细表要统计的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildRelatedSummaryCols { get; set; } = new List<Expression<Func<C, object>>>();



        /// <summary>
        /// 主表表不可见的列
        /// </summary>
        public List<Expression<Func<M, object>>> MasterInvisibleCols { get; set; } = new List<Expression<Func<M, object>>>();

        /// <summary>
        /// 明细表不可见的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildInvisibleCols { get; set; } = new List<Expression<Func<C, object>>>();

        /// <summary>
        /// 明细表不可见的列
        /// </summary>
        public List<Expression<Func<C, object>>> ChildRelatedInvisibleCols { get; set; } = new List<Expression<Func<C, object>>>();


        private KryptonPage ChildQuery()
        {
            // 如果_UCBillChildQuery已经在PreInitializeControls中初始化，则直接使用，否则创建新实例
            if (_UCBillChildQuery == null)
            {
                _UCBillChildQuery = new UCBillChildQuery();
                _UCBillChildQuery.Name = "_UCBillChildQuery";
                _UCBillChildQuery.entityType = typeof(C);
                _UCBillChildQuery.ColNameDataDictionary = ChildColNameDataDictionary;
                
                // 设置关联信息
                if (_UCBillChildQuery.GridRelated.RelatedInfoList.Count == 0 && RelatedBillEditCol != null)
                {
                    _UCBillChildQuery.GridRelated.SetRelatedInfo(typeof(C).Name, RelatedBillEditCol.GetMemberInfo().Name);
                }
            }

            // 设置其他属性
            List<string> childlist = RuinorExpressionHelper.ExpressionListToStringList(ChildSummaryCols);
            
            // 确保InvisibleCols不为null并添加必要的列
            if (_UCBillChildQuery.InvisibleCols == null)
            {
                _UCBillChildQuery.InvisibleCols = new HashSet<string>();
            }
            _UCBillChildQuery.InvisibleCols.UnionWith(RuinorExpressionHelper.ExpressionListToHashSet(ChildInvisibleCols));
            
            // 处理外键列隐藏
            List<BaseDtoField> tempChildFiledList = UIHelper.GetDtoFieldNameList<C>();//<M>
            foreach (var item in tempChildFiledList)
            {
                if (item.FKTableName == null)
                {
                    continue;
                }
                if (item.FKTableName.Replace("Detail", "") == typeof(M).Name)
                {
                    if (!_UCBillChildQuery.InvisibleCols.Contains(item.FieldName))
                    {
                        _UCBillChildQuery.InvisibleCols.Add(item.FieldName);
                    }
                }
            }
            
            _UCBillChildQuery.DefaultHideCols = new HashSet<string>();
            UIHelper.ControlColumnsInvisible(CurMenuInfo, _UCBillChildQuery.InvisibleCols, _UCBillChildQuery.DefaultHideCols, true);
            _UCBillChildQuery.SummaryCols = childlist;
            KryptonPage page = NewPage("明细信息", 1, _UCBillChildQuery);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            return page;
        }

        private UCBillChildQuery _UCBillChildQuery_Related;

        private KryptonPage Child_RelatedQuery()
        {
            _UCBillChildQuery_Related = new UCBillChildQuery();
            _UCBillChildQuery_Related.Name = "_UCBillChildQuery_Related";
            _UCBillChildQuery_Related.entityType = ChildRelatedEntityType;

            List<string> childlist = RuinorExpressionHelper.ExpressionListToStringList(ChildRelatedSummaryCols);
            _UCBillChildQuery_Related.SummaryCols = childlist;
            if (_UCBillChildQuery_Related.InvisibleCols == null)
            {
                _UCBillChildQuery_Related.InvisibleCols = new HashSet<string>();
            }
            _UCBillChildQuery_Related.InvisibleCols.AddRange(RuinorExpressionHelper.ExpressionListToHashSet(ChildRelatedInvisibleCols).ToArray());

            _UCBillChildQuery_Related.DefaultHideCols = new HashSet<string>();
            UIHelper.ControlColumnsInvisible(CurMenuInfo, _UCBillChildQuery_Related.InvisibleCols, _UCBillChildQuery_Related.DefaultHideCols, true);

            _UCBillChildQuery_Related.ColNameDataDictionary = ChildColNameDataDictionary;
            KryptonPage page = NewPage("关联信息", 1, _UCBillChildQuery_Related);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);
            return page;
        }

        public UCBillMasterQuery _UCBillMasterQuery;
        public UCBillChildQuery _UCBillChildQuery;
        
        /// <summary>
        /// 预初始化子控件
        /// </summary>
        private void PreInitializeControls()
        {            
            // 提前初始化_UCBillChildQuery，确保在子类Load事件执行时不为null
            if (HasChildData && _UCBillChildQuery == null)
            {
                _UCBillChildQuery = new UCBillChildQuery();
                _UCBillChildQuery.Name = "_UCBillChildQuery";
                _UCBillChildQuery.entityType = typeof(C);
                
                // 设置基本属性
                if (_UCBillChildQuery.InvisibleCols == null)
                {
                    _UCBillChildQuery.InvisibleCols = new HashSet<string>();
                }
                _UCBillChildQuery.DefaultHideCols = new HashSet<string>();
                _UCBillChildQuery.ColNameDataDictionary = ChildColNameDataDictionary;
                
                // 设置关联信息
                if (_UCBillChildQuery.GridRelated.RelatedInfoList.Count == 0 && RelatedBillEditCol != null)
                {
                    _UCBillChildQuery.GridRelated.SetRelatedInfo(typeof(C).Name, RelatedBillEditCol.GetMemberInfo().Name);
                }
            }
            
            // 提前初始化_UCBillMasterQuery，确保在子类Load事件执行时不为null
            if (_UCBillMasterQuery == null)
            {
                _UCBillMasterQuery = new UCBillMasterQuery();
                _UCBillMasterQuery.Name = "_UCBillMasterQuery";
                _UCBillMasterQuery.entityType = typeof(M);
                
                // 设置基本属性
                if (_UCBillMasterQuery.InvisibleCols == null)
                {
                    _UCBillMasterQuery.InvisibleCols = new HashSet<string>();
                }
                
                // 添加主键列到隐藏列表
                string PKColName = BaseUIHelper.GetEntityPrimaryKey<M>();
                if (!_UCBillMasterQuery.InvisibleCols.Contains(PKColName))
                {
                    _UCBillMasterQuery.InvisibleCols.Add(PKColName);
                }
                
                _UCBillMasterQuery.DefaultHideCols = new HashSet<string>();
                _UCBillMasterQuery.ColNameDataDictionary = MasterColNameDataDictionary;
                _UCBillMasterQuery.GridRelated.FromMenuInfo = CurMenuInfo;
                
                // 设置关联信息
                if (_UCBillMasterQuery.GridRelated.RelatedInfoList.Count == 0 && RelatedBillEditCol != null)
                {
                    _UCBillMasterQuery.GridRelated.SetRelatedInfo(typeof(M).Name, RelatedBillEditCol.GetMemberInfo().Name);
                }
            }
        }

        private KryptonPage MasterQuery()
        {
            _UCBillMasterQuery = new UCBillMasterQuery();
            _UCBillMasterQuery.entityType = typeof(M);
            _UCBillMasterQuery.Name = "_UCBillMasterQuery";
            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            _UCBillMasterQuery.SummaryCols = masterlist;



            _UCBillMasterQuery.InvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(MasterInvisibleCols);

            //一般主单的主键不用显示 这里统一处理？
            string PKColName = BaseUIHelper.GetEntityPrimaryKey<M>();
            if (!_UCBillMasterQuery.InvisibleCols.Contains(PKColName))
            {
                _UCBillMasterQuery.InvisibleCols.Add(PKColName);
            }


            _UCBillMasterQuery.DefaultHideCols = new HashSet<string>();

            UIHelper.ControlColumnsInvisible(CurMenuInfo, _UCBillMasterQuery.InvisibleCols, _UCBillMasterQuery.DefaultHideCols, false);

            _UCBillMasterQuery.ColNameDataDictionary = MasterColNameDataDictionary;
            _UCBillMasterQuery.GridRelated.FromMenuInfo = CurMenuInfo;

            if (_UCBillMasterQuery.GridRelated.RelatedInfoList.Count == 0 && RelatedBillEditCol != null)
            {
                if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                {
                    // MainForm.Instance.logger.LogDebug("当前查询没有设置指向列，自动设置为主表类型及列");
                }
                _UCBillMasterQuery.GridRelated.SetRelatedInfo(typeof(M).Name, RelatedBillEditCol.GetMemberInfo().Name);
            }



            SetGridViewDisplayConfig();
            KryptonPage page = NewPage("单据信息", 1, _UCBillMasterQuery);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }


        #region 分析1

        /// <summary>
        /// 分析结果中要统计的列
        /// </summary>
        public List<Expression<Func<M, object>>> OutlookGridAnalysis1SubtotalColumns { get; set; } = new List<Expression<Func<M, object>>>();



        public UCBillOutlookGridAnalysis _UCOutlookGridAnalysis1;


        /// <summary>
        /// 加载分析数据 部分 数据 不能太多。不然性能影响体验
        /// </summary>
        /// <returns></returns>

        private KryptonPage UCOutlookGridAnalysis1Load()
        {
            _UCOutlookGridAnalysis1 = new UCBillOutlookGridAnalysis();
            //_UCOutlookGridAnalysis1.entityType = typeof(M);
            //_UCOutlookGridAnalysis1.ColDisplayTypes = _UCMasterQuery.ColDisplayTypes;
            //List<string> masterlist = ExpressionHelper.ExpressionListToStringList(MasterSummaryCols);
            //_UCOutlookGridAnalysis1.SummaryCols = masterlist;
            // _UCOutlookGridAnalysis1.InvisibleCols = ExpressionHelper.ExpressionListToStringList(MasterInvisibleCols);
            // _UCOutlookGridAnalysis1.ColNameDataDictionary = MasterColNameDataDictionary;
            KryptonPage page = NewPage("结果分析", 1, _UCOutlookGridAnalysis1);
            //page.ClearFlags(KryptonPageFlags.All);
            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked | KryptonPageFlags.DockingAllowClose);
            return page;
        }


        #endregion

    }
}