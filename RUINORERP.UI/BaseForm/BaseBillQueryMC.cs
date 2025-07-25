﻿using AutoMapper;
using FastReport.Barcode;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using FastReport.Utils;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.X509.Qualified;
using RUINOR.Core;
using RUINORERP.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Models;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.FormProperty;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.UI.PSI.PUR;
using RUINORERP.UI.Report;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UCSourceGrid;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserPersonalized;
using SourceGrid2.Win32;
using SqlSugar;
using System;
using System.Collections;
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
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using static StackExchange.Redis.Role;
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

        private readonly EnhancedBizTypeMapper _mapper;

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
                    if (_mapper == null)
                    {
                        _mapper = Startup.GetFromFac<EnhancedBizTypeMapper>();
                    }


                    //提前统一插入批量处理的菜单按钮

                    cbbatch.Text = "批量处理";
                    cbbatch.CheckStateChanged += (s, ex) =>
                    {
                        //this.Text = cb.CheckState.ToString();
                        //kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;
                        _UCBillMasterQuery.newSumDataGridViewMaster.MultiSelect = cbbatch.Checked;
                        _UCBillMasterQuery.newSumDataGridViewMaster.UseSelectedColumn = cbbatch.Checked;
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
            await UIBizSrvice.SetGridViewAsync(typeof(C), _UCBillChildQuery.newSumDataGridViewChild, CurMenuInfo, true);
        }

        private async void button表格显示设置_Click(object sender, EventArgs e)
        {
            await UIBizSrvice.SetGridViewAsync(typeof(M), _UCBillMasterQuery.newSumDataGridViewMaster, CurMenuInfo, true, _UCBillMasterQuery.InvisibleCols);
        }

        private async void button设置查询条件_Click(object sender, EventArgs e)
        {
            bool rs = await UIBizSrvice.SetQueryConditionsAsync(CurMenuInfo, QueryConditionFilter, QueryDto as BaseEntity);
            if (rs)
            {
                QueryDto = LoadQueryConditionToUI();
            }
        }



        //protected virtual void MenuPersonalizedSettings()
        //{
        //    UserCenter.frmMenuPersonalization frmMenu = new UserCenter.frmMenuPersonalization();
        //    frmMenu.MenuPathKey = CurMenuInfo.ClassPath;
        //    if (frmMenu.ShowDialog() == DialogResult.OK)
        //    {
        //        if (!this.DesignMode)
        //        {
        //            MenuPersonalization personalization = new MenuPersonalization();
        //            UserGlobalConfig.Instance.MenuPersonalizationlist.TryGetValue(CurMenuInfo.ClassPath, out personalization);
        //            if (personalization != null)
        //            {
        //                decimal QueryShowColQty = personalization.QueryConditionShowColsQty;
        //                QueryDtoProxy = LoadQueryConditionToUI(QueryShowColQty);
        //            }
        //            else
        //            {
        //                QueryDtoProxy = LoadQueryConditionToUI(frmMenu.QueryShowColQty.Value);
        //            }
        //        }
        //    }
        //}


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
            foreach (var item in MainForm.Instance.list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            ChildColNameDataDictionary.TryAdd("ProdDetailID", proDetailList);
        }

        public delegate void QueryHandler();

        [Browsable(true), Description("查询主表")]
        public event QueryHandler OnQuery;


        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            DoButtonClick(RUINORERP.Common.Helper.EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
        }
        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual async void DoButtonClick(MenuItemEnums menuItem)
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
                    Query(QueryDto);
                    toolStripSplitButtonPrint.Enabled = true;
                    break;
                case MenuItemEnums.复制性新增:
                    if (selectlist.Count == 0)
                    {
                        return;
                    }
                    AddByCopy(selectlist);
                    break;
                //case MenuItemEnums.高级查询:
                //    AdvQuery();
                //    break;
                case MenuItemEnums.关闭:
                    Exit(this);
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
                        ApprovalEntity ae = await Review(selectlist[0]);
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
                            MainForm.Instance.AuditLogHelper.CreateAuditLog<M>("结案", selectlist[0], $"结案意见:{rs}");
                        }
                    }
                    break;

                case MenuItemEnums.打印:

                    if (PrintConfig != null && PrintConfig.tb_PrintTemplates != null)
                    {
                        //如果当前单据只有一个模块，就直接打印
                        if (PrintConfig.tb_PrintTemplates.Count == 1)
                        {
                            Print(RptMode.PRINT);
                            return;
                        }
                    }

                    //个性化设置了打印要选择模板打印时，就进入设计介面
                    if (MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.HasValue
                           && MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.Value)
                    {
                        Print(RptMode.DESIGN);
                    }
                    else
                    {
                        Print(RptMode.PRINT);
                    }
                    break;
                case MenuItemEnums.预览:
                    Print(RptMode.PREVIEW);
                    break;
                case MenuItemEnums.设计:
                    Print(RptMode.DESIGN);
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





        BizTypeMapper Bizmapper = new BizTypeMapper();
        public virtual async void Print(RptMode rptMode)
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
                        BizType bizType = Bizmapper.GetBizType(typeof(M).Name);
                        if (MessageBox.Show($"当前【{bizType.ToString()}】没有审核,你确定要打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            continue;
                        }
                    }
                }

                //打印次数提醒
                if (item.ContainsProperty("PrintStatus"))
                {
                    BizType bizType = Bizmapper.GetBizType(typeof(T).Name);
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
        public async virtual Task<bool> DeleteRemoteImages(M EditEntity)
        {
            await Task.Delay(0);
            return false;

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
                            //await DeleteRemoteImages();
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


            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();

            CommonUI.frmApproval frm = new CommonUI.frmApproval();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<M>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;

            //// 获取销售订单实体类型
            //var entityType = _mapper.GetEntityType(BizType.销售订单);

            //var bi = _mapper.GetBizType(typeof(M), EditEntity);
            //// 获取字段映射
            //var (idField, noField) = _mapper.GetEntityFields(typeof(M));

            // 使用字段名查询数据库...
            //var order = await MainForm.Instance.AppContext.Db.Queryable<M>(entityType.Name)
            //    .Includes()
            //    .Where($"{idField} = @id", new { pkid })
            //   .SingleAsync();



            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            await Task.Delay(1);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
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
                    Query(QueryDto);
                    //这里推送到审核，启动工作流
                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg},请联系管理员！", Color.Red);
                    MessageBox.Show($"{ae.bizName}:{ae.BillNo}审核失败。\r\n {rmr.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MainForm.Instance.AuditLogHelper.CreateAuditLog<M>("审核", EditEntity, $"意见{ae.ApprovalOpinions}" + $"结果:{(ae.ApprovalResults ? "通过" : "拒绝")},{rmr.ErrorMsg}");
            }

            return ae;
        }

        //protected async virtual Task<ApprovalEntity> ReReview()
        //{
        //    await Task.Delay(0);
        //    return null;
        //}


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

            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmReApproval frm = new CommonUI.frmReApproval();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<M>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<M>(EditEntity);
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
                QueryDto = QueryParameters;
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
                //因为时间不会去掉选择，这里特殊处理,(与下面的时间清空是不是重复设置了）
                foreach (var item in nodeParameter.queryFilter.QueryFields)
                {
                    if (item.FieldPropertyInfo.PropertyType.Name == "DateTime")
                    {
                        //因为查询UI生成时。自动 转换成代理类如：tb_SaleOutProxy，并且时间是区间型式,将起为null即可
                        QueryDto.SetPropertyValue(item.FieldName + "_Start", null);
                        var fieldNameControl = kryptonPanelQuery.Controls.Find(item.FieldName, true);
                        if (fieldNameControl != null && fieldNameControl.Count() > 0)
                        {
                            if (kryptonPanelQuery.Controls.Find(item.FieldName, true)[0] is UCAdvDateTimerPickerGroup timerPickerGroup)
                            {
                                timerPickerGroup.dtp1.Checked = false;
                                timerPickerGroup.dtp2.Checked = false;
                            }
                        }

                        //KryptonDateTimePicker dtp = _UCBillQueryCondition.kryptonPanelQuery.Controls.Find(item.FieldName + "_Start", true) as KryptonDateTimePicker;
                        //if (dtp != null)
                        //{
                        //    dtp.check
                        //}
                    }
                }

                //查询条件给值前先将条件清空
                foreach (var item in nodeParameter.queryFilter.QueryFields)
                {
                    if (item.FKTableName.IsNotEmptyOrNull() && item.IsRelated)
                    {
                        QueryDto.SetPropertyValue(item.FieldName, -1L);
                        continue;
                    }
                    if (item.FieldPropertyInfo.PropertyType.IsGenericType && item.FieldPropertyInfo.PropertyType.GetBaseType().Name == "DateTime")
                    {
                        QueryDto.SetPropertyValue(item.FieldName, null);
                        if (QueryDto.ContainsProperty(item.FieldName + "_Start"))
                        {
                            QueryDto.SetPropertyValue(item.FieldName + "_Start", null);
                        }
                        if (QueryDto.ContainsProperty(item.FieldName + "_End"))
                        {
                            QueryDto.SetPropertyValue(item.FieldName + "_End", null);
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
                            ConditionalCollections ccs = nodeParameter.conditionals[i] as ConditionalCollections;
                            for (int c = 0; c < ccs.ConditionalList.Count; c++)
                            {
                                ConditionalModel item = ccs.ConditionalList[c].Value;
                                if (item.ConditionalType == ConditionalType.Equal)
                                {
                                    switch (item.CSharpTypeName)
                                    {
                                        case "int":
                                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToInt());
                                            break;
                                        case "long":
                                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToLong());
                                            break;
                                        case "bool":
                                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToBool());
                                            break;
                                        default:
                                            QueryDto.SetPropertyValue(item.FieldName, item.FieldValue);
                                            break;
                                    }
                                }

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
                                        QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToInt());
                                        break;
                                    case "long":
                                        QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToLong());
                                        break;
                                    case "bool":
                                        QueryDto.SetPropertyValue(item.FieldName, item.FieldValue.ToBool());
                                        break;
                                    default:
                                        QueryDto.SetPropertyValue(item.FieldName, item.FieldValue);
                                        break;
                                }
                            }
                        }
                        #endregion
                    }

                }



            }

            Query(QueryDto, false);

        }

        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="QueryDto">查询参数实体 ：比方我要查销售订单，结果是一个订单列表。查询条件是 订单日期区间，订单号等直接赋值给订单实体本身。再到方法中提取日期等条件</param>
        /// <param name="UIQuery">如果条件来自UI时要验证UI，否则不验证</param>
        [MustOverride]
        protected async virtual void Query(object QueryDto, bool UIQuery = true)
        {
            if (UIQuery)
            {
                this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

                if (ValidationHelper.hasValidationErrors(this.Controls))
                    return;
            }

            BaseController<M> ctr = Startup.GetFromFacByName<BaseController<M>>(typeof(M).Name + "Controller");


            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRow.Text);
            List<M> list = new List<M>();
            //提取指定的列名，即条件集合
            List<string> queryConditions = new List<string>();


            queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());

            if (QueryConditionFilter.FilterLimitExpressions == null)
            {
                QueryConditionFilter.FilterLimitExpressions = new List<LambdaExpression>();
            }
            if (!QueryConditionFilter.FilterLimitExpressions.Contains(LimitQueryConditions))
            {
                QueryConditionFilter.FilterLimitExpressions.Add(LimitQueryConditions);
            }
            //list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, queryConditions, LimitQueryConditions, QueryDto, pageNum, pageSize) as List<M>;
            list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDto, pageNum, pageSize) as List<M>;


            _UCBillMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();
            _UCBillMasterQuery.ShowSummaryCols();
            if (list.Count > 0)
            {
                toolStripSplitButtonPrint.Visible = true;
            }
            else
            {
                toolStripSplitButtonPrint.Visible = false;
            }
            if (ResultAnalysis && _UCOutlookGridAnalysis1 != null)
            {
                _UCOutlookGridAnalysis1.ColDisplayTypes = new List<Type>();
                
                _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(M));

                _UCOutlookGridAnalysis1.FieldNameList = _UCBillMasterQuery.newSumDataGridViewMaster.FieldNameList;
                _UCOutlookGridAnalysis1.bindingSourceOutlook.DataSource = list;
                //控制列的显示
                _UCOutlookGridAnalysis1.ColumnDisplays = _UCBillMasterQuery.newSumDataGridViewMaster.ColumnDisplays;

                // _UCOutlookGridAnalysis2.GridRelated.SetRelatedInfo<View_Inventory, tb_BOM_S>(c => c.BOM_ID, r => r.BOM_ID);
                _UCOutlookGridAnalysis1.LoadDataToGrid<M>(list);
            }
            //测试代码
            //foreach (DataGridViewColumn dc in _UCBillMasterQuery.newSumDataGridViewMaster.Columns)
            //{
            //    if (dc.Visible==true)
            //    {
            //        MainForm.Instance.uclog.AddLog(dc.HeaderText);
            //    }
            //}
        }

        public List<Expression<Func<M, object>>> QueryConditions = new List<Expression<Func<M, object>>>();




        public void Builder()
        {

            BuildInvisibleCols();

            //先添加主要的条件 在processor中
            BuildQueryCondition();

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
        public virtual void BuildQueryCondition()
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

        protected virtual async void Exit(object thisform)
        {
            try
            {
                if (_UCBillMasterQuery != null && _UCBillMasterQuery.newSumDataGridViewMaster != null)
                {
                    UIBizSrvice.SaveGridSettingData(CurMenuInfo, _UCBillMasterQuery.newSumDataGridViewMaster, typeof(M));
                }
                if (_UCBillChildQuery != null && _UCBillChildQuery.newSumDataGridViewChild != null)
                {
                    UIBizSrvice.SaveGridSettingData(CurMenuInfo, _UCBillChildQuery.newSumDataGridViewChild, typeof(C));
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
                        toolStripSplitButtonPrint.Enabled = true;
                        break;
                }

            }
            return false;
        }


        public KryptonDockingWorkspace ws;
        KryptonCheckBox cbbatch = new KryptonCheckBox();
        private async void BaseBillQueryMC_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            else
            {
                MainForm.Instance.AppContext.log.ActionName = sender.ToString();
                //先加载一遍缓存
                var tableNames = MainForm.Instance.CacheInfoList.Keys.ToList();
                foreach (var nextTableName in tableNames)
                {
                    //MainForm.Instance.TryRequestCache(nextTableName);
                }

                Builder();
                this.CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(M).Name && m.ClassPath == this.ToString()).FirstOrDefault();
                if (CurMenuInfo == null && !MainForm.Instance.AppContext.IsSuperUser)
                {
                    MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                    return;
                }
                QueryDto = LoadQueryConditionToUI();



            }

            // Setup docking functionality
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

            // Add docking pages
            /*
            // Add docking pages
            kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Top, new KryptonPage[] { QueryCondition() });
            kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Left, new KryptonPage[] { ChildQuery() });
            // kryptonDockingManagerQuery.AddDockspace("Control", DockingEdge.Bottom, new KryptonPage[] { NewInput(), NewInput() });
            //  kryptonDockingManagerQuery.AddAutoHiddenGroup("Control", DockingEdge.Right, new KryptonPage[] { NewPropertyGrid() });
            //kryptonDockingManagerQuery.AddToWorkspace("Workspace", new KryptonPage[] { MasterQuery(), MasterQuery() });            kryptonDockingManagerQuery.AddToWorkspace("Workspace", new KryptonPage[] { MasterQuery(), MasterQuery() });
            kryptonDockingManagerQuery.AddToWorkspace("Workspace", new KryptonPage[] { MasterQuery() });
            */




            List<M> list = new List<M>();
            _UCBillMasterQuery.bindingSourceMaster.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            _UCBillMasterQuery.OnSelectDataRow += _UCBillMasterQuery_OnSelectDataRow;

            _UCBillMasterQuery.ShowSummaryCols();

            #region 请求缓存
            //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
            UIBizSrvice.RequestCache<M>();
            UIBizSrvice.RequestCache<C>();
            #endregion

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
                await UIBizSrvice.SetGridViewAsync(typeof(M), BaseMainDataGridView, CurMenuInfo, false, _UCBillMasterQuery.InvisibleCols, _UCBillMasterQuery.DefaultHideCols);

            }
            if (_UCBillChildQuery != null)
            {
                _UCBillChildQuery.newSumDataGridViewChild.NeedSaveColumnsXml = false;
                BaseSubDataGridView = _UCBillChildQuery.newSumDataGridViewChild;
                await UIBizSrvice.SetGridViewAsync(typeof(C), BaseSubDataGridView, CurMenuInfo, false, _UCBillChildQuery.InvisibleCols, _UCBillChildQuery.DefaultHideCols);
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


        private object _queryDto = new BaseEntity();



        [Obsolete("//生成 加载查询条件时 LoadQueryConditionToUI 时 为什么 一会用这个。一会用 QueryDtoProxy TODO有问题哦。")]
        public object QueryDto { get => _queryDto; set => _queryDto = value; }


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
            //UIQueryHelper<M> uIQueryHelper = new UIQueryHelper<M>();
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();
            if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            {
                QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
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
                        QueryDto = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryConditionFilter, menuSetting);
                    }
                    else
                    {
                        QueryDto = UIGenerateHelper.CreateQueryUI(typeof(M), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
                    }
                }

            }


            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
            return QueryDto;
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
            _UCBillChildQuery = new UCBillChildQuery();

            if (_UCBillChildQuery.GridRelated.RelatedInfoList.Count == 0 && RelatedBillEditCol != null)
            {
                _UCBillChildQuery.GridRelated.SetRelatedInfo(typeof(C).Name, RelatedBillEditCol.GetMemberInfo().Name);
            }

            _UCBillChildQuery.Name = "_UCBillChildQuery";
            _UCBillChildQuery.entityType = typeof(C);
            List<string> childlist = RuinorExpressionHelper.ExpressionListToStringList(ChildSummaryCols);
            _UCBillChildQuery.InvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(ChildInvisibleCols);
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
            _UCBillChildQuery.ColNameDataDictionary = ChildColNameDataDictionary;
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
                    // MainForm.Instance.logger.LogInformation("当前查询没有设置指向列，自动设置为主表类型及列");
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