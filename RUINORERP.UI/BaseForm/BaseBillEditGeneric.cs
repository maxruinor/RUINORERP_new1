using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.UCSourceGrid;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.CollectionExtension;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using RUINORERP.Model.Dto;
using DevAge.Windows.Forms;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.UI.Report;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.AdvancedUIModule;
using Krypton.Navigator;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using System.Collections;
using TransInstruction;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Model.CommonModel;
using FluentValidation;
using FluentValidation.Results;
using Krypton.Toolkit;
using System.IO;
using System.Diagnostics;
using SqlSugar;
using RUINORERP.Business.Processor;
using ExCSS;
using RUINORERP.Business.CommService;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using MySqlX.XDevAPI.Common;
using RUINORERP.Business.Security;
using RUINORERP.UI.CommonUI;
using ImageHelper = RUINORERP.UI.Common.ImageHelper;
using Netron.GraphLib;
using Newtonsoft.Json;
using RUINORERP.UI.SS;
using MathNet.Numerics.LinearAlgebra.Factorization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using SourceGrid;
using RUINORERP.UI.FormProperty;
using SourceGrid.Cells.Models;
using FastReport.Table;
using FastReport.DevComponents.AdvTree;
using Newtonsoft.Json.Linq;
using System.Web.Caching;
using Microsoft.Extensions.Caching.Memory;
using RUINORERP.UI.PSI.SAL;
using RUINORERP.UI.ClientCmdService;
using TransInstruction.CommandService;
using RUINORERP.Model.TransModel;
using System.Threading;
using static RUINORERP.Business.CommService.LockManager;
using System.Management.Instrumentation;
using FastReport.DevComponents.DotNetBar;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using System.Windows.Controls.Primitives;
using TransInstruction.DataModel;
using RUINORERP.Common.LogHelper;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.StatusManagerService;
using NPOI.POIFS.Crypt.Dsig;
using RUINORERP.Model.Base;
using System.Windows.Documents;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.UI.FM;
using RUINORERP.UI.FM.FMBase;
using LiveChartsCore.Geo;
using RUINORERP.UI.MRP.MP;
using Winista.Text.HtmlParser.Lex;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Management;
using RUINORERP.Business.BizMapperService;
using TransInstruction.Enums;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 单据类型的编辑 主表T子表C
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseBillEditGeneric<T, C> : BaseBillEdit, IContextMenuInfoAuth, IToolStripMenuInfoAuth where T : class, new() where C : class, new()
    {
        public virtual List<UControls.ContextMenuController> AddContextMenu()
        {
            List<UControls.ContextMenuController> list = new List<UControls.ContextMenuController>();
            return list;
        }

        public virtual ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }

        #region 单据明细中的 产品公共部分的字段提取。为了能统一控制这些公共字段

        public List<Type> PublicEntityObjects { get; set; } = new List<Type>();

        public virtual void AddPublicEntityObject(Type type)
        {
            if (!PublicEntityObjects.Contains(type))
            {
                PublicEntityObjects.Add(type);
            }

        }

        #endregion


        #region 单据状态 及按钮状态控制

        //private UIStateBinder<DataStatus> _UIStateBinder;
        //private IStatusEvaluator evaluator;
        private void InitializeStateManagement()
        {

            //if (_UIStateBinder == null && editEntity != null)
            //{
            //    BaseEntity baseEntity = editEntity as BaseEntity;
            //    evaluator = baseEntity.StatusEvaluator;
            //    _UIStateBinder = new UIStateBinder<DataStatus>(baseEntity, this.BaseToolStrip, evaluator);
            //}

            /*
            // 初始化
            var notificationService = new WorkflowNotificationService();
            StatusMachine = new StatusMachine(
              DataStatus.草稿,
              ApprovalStatus.未审核,
              false,//这里是如果有值则显示他的值，如果没有则false。要修复
              notificationService);
            //var statusMachine = new StatusMachine(
            //    (DataStatus)editEntity.DataStatus,
            //    (ApprovalStatus)editEntity.ApprovalStatus,
            //    editEntity.ApprovalResults ?? false,//这里是如果有值则显示他的值，如果没有则false。要修复
            //    notificationService);

            // 创建带自定义规则的UI绑定器
            StatusBinder = new UIStateBinder(StatusMachine, BaseToolStrip, MainForm.Instance.AppContext.workflowHost,
              (data, approval, result, op, actionStatus) =>
              {
                  // 示例：增加财务专属操作
                  if (op == MenuItemEnums.数据特殊修正)
                  {
                      return new ControlState
                      {
                          Visible = data == DataStatus.确认 || true,
                          Enabled = MainForm.Instance.AppContext.IsSuperUser
                      };
                  }
                  return StatusEvaluator.GetControlState(data, approval, result, op, actionStatus);
              });
            
            // 初始状态更新
            StatusBinder.UpdateAllControls();*/
        }

        #endregion

        public BaseBillEditGeneric()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                AddExcludeMenuList();
                AddExtendButton(CurMenuInfo);
                if (!this.DesignMode)
                {
                    frm = new frmFormProperty();

                    this.OnBindDataToUIEvent += BindData;

                    KryptonButton button保存当前单据 = new KryptonButton();
                    button保存当前单据.Text = "保存当前单据";
                    button保存当前单据.Click += button保存当前单据_Click;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button保存当前单据);

                    KryptonContextMenu kcm加载最新数据 = new KryptonContextMenu();
                    KryptonContextMenuItem menuItem选择要加载的数据 = new KryptonContextMenuItem("选择数据");
                    menuItem选择要加载的数据.Text = "选择数据";
                    menuItem选择要加载的数据.Click += MenuItem选择要加载的数据_Click;

                    KryptonContextMenuItems kryptonContextMenuItems1 = new KryptonContextMenuItems();

                    kcm加载最新数据.Items.AddRange(new KryptonContextMenuItemBase[] {
            kryptonContextMenuItems1});

                    kryptonContextMenuItems1.Items.AddRange(new KryptonContextMenuItemBase[] {
            menuItem选择要加载的数据});

                    KryptonDropButton button加载最新数据 = new KryptonDropButton();
                    button加载最新数据.Text = "加载数据";
                    button加载最新数据.Click += button加载最新数据_Click;
                    button加载最新数据.KryptonContextMenu = kcm加载最新数据;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button加载最新数据);

                    KryptonButton button快速录入数据 = new KryptonButton();

                    button快速录入数据.Text = "快速录入数据";
                    button快速录入数据.Click += button快速录入数据_Click;

                    frm.flowLayoutPanelButtonsArea.Controls.Add(button快速录入数据);

                    KryptonButton button请求协助处理 = new KryptonButton();
                    button请求协助处理.Text = "请求协助处理";
                    button请求协助处理.Click += button请求协助处理_Click;

                    frm.flowLayoutPanelButtonsArea.Controls.Add(button请求协助处理);


                    Krypton.Toolkit.KryptonButton button录入数据预设 = new Krypton.Toolkit.KryptonButton();
                    button录入数据预设.Text = "录入数据预设";
                    button录入数据预设.ToolTipValues.Description = "对单据，资料等数据进行预设，并且可以提供多个预设模板，提高录入速度。";
                    button录入数据预设.ToolTipValues.EnableToolTips = true;
                    button录入数据预设.ToolTipValues.Heading = "提示";
                    button录入数据预设.Click += button录入数据预设_Click;
                    button录入数据预设.Width = 120;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button录入数据预设);


                    BizTypeMapper mapper = new BizTypeMapper();
                    CurrentBizType = mapper.GetBizType(typeof(T).Name);
                    CurrentBizTypeName = CurrentBizType.ToString();
                }
                menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                _entityInfoService = Startup.GetFromFac<IEntityInfoService>();
            }
        }

        private async void button录入数据预设_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                EditEntity = Activator.CreateInstance(typeof(T)) as T;
            }
            bool rs = await UIBizSrvice.SetInputDataAsync<T>(CurMenuInfo, EditEntity);
            if (rs)
            {
                // EditEntity = LoadQueryConditionToUI();
            }
        }


        #region 单据 主表公共信息 如类型：名称

        public BizType CurrentBizType { get; set; }
        public string CurrentBizTypeName
        {
            get; set;

        }
        #endregion

        private void MenuItem选择要加载的数据_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "cache files (*.cache)|*.cache|All files (*.*)|*.*";
            //加载最新数据
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                EditEntity = manager.Deserialize<T>(openFileDialog1.FileName);
                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
                MainForm.Instance.uclog.AddLog("成功加载选择的数据。");
            }
        }

        private async void button请求协助处理_Click(object sender, EventArgs e)
        {
            //弹出一个弹出框，让用户输入协助处理的内容。
            //再把单据相关内容发送到服务器转发到管理员那里

            frmInputContent frm = new frmInputContent();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //发送协助处理请求
                //先获取当前单据的ID
                #region
                try
                {
                    if (EditEntity != null)
                    {
                        #region  单据数据  后面优化可以多个单?限制5个？
                        await Save(false);
                        string json = JsonConvert.SerializeObject(EditEntity,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                            });
                        OriginalData odforCache = ActionForClient.请求协助处理(MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID,
                          MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name, frm.Content, json, typeof(T).Name);
                        byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(odforCache);
                        MainForm.Instance.ecs.client.Send(buffer);
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "请求协助处理");
                }
                #endregion
            }

        }

        private void button快速录入数据_Click(object sender, EventArgs e)
        {
            frm.Close();
            frmQuicklyInputGeneric<C> frmQuicklyInput = new frmQuicklyInputGeneric<C>();
            frmQuicklyInput.OnApplyQuicklyInputData += OnApplyQuicklyInputData;
            frmQuicklyInput.CurMenuInfo = CurMenuInfo;
            if (EditEntity == null)
            {
                Add();
            }
            var details = EditEntity.GetPropertyValue(typeof(C).Name + "s");
            if (details == null)
            {
                details = new List<C>();
            }
            frmQuicklyInput.lines = details as List<C>;
            if (frmQuicklyInput.ShowDialog() == DialogResult.OK)
            {
                EditEntity.SetPropertyValue(typeof(C).Name + "s", frmQuicklyInput.lines);
                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            }
        }

        private void OnApplyQuicklyInputData(List<C> lines)
        {
            EditEntity.SetPropertyValue(typeof(C).Name + "s", lines);
            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
        }
        BizTypeMapper Bizmapper = new BizTypeMapper();

        RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
        private void button加载最新数据_Click(object sender, EventArgs e)
        {
            //RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
            EditEntity = manager.Deserialize<T>(CurMenuInfo.CaptionCN + ".cache");
            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            MainForm.Instance.uclog.AddLog("成功加载上次的数据。");
        }

        private async void button保存当前单据_Click(object sender, EventArgs e)
        {
            await AutoSaveDataAsync();
        }


        /// <summary>
        /// 绑定数据到UI
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void BindData(T entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.AcceptChanges();
            }
            #region 联查

            toolStripbtnRelatedQuery.DropDownItems.Clear();
            await LoadRelatedDataToDropDownItemsAsync();
            #endregion


            #region 转单

            toolStripbtnConvertDocuments.DropDownItems.Clear();
            //LoadRelatedDataToDropDownItemsAsync();
            if (toolStripbtnConvertDocuments.DropDownItems.Count > 0)
            {
                toolStripbtnConvertDocuments.Visible = true;
            }
            else
            {
                toolStripbtnConvertDocuments.Visible = false;
            }
            #endregion


            InitializeStateManagement();
            ToolBarEnabledControl(entity);

            //暂时统一不显示外币
            UIHelper.ControlForeignFieldInvisible<T>(this, false);
        }



        internal override void LoadDataToUI(object Entity)
        {
            if (Entity == null) return;
            if (Entity is BaseEntity baseEntity)
            {
                if (baseEntity.ActionStatus == ActionStatus.新增 || baseEntity.ActionStatus == ActionStatus.修改)
                {
                    toolStripButtonSave.Enabled = true;
                }
            }
            BindData(Entity as T);
            InitializeStateManagement();
            ToolBarEnabledControl(Entity);
        }




        /// <summary>
        /// 不同情况，显示不同的可用情况
        /// </summary>
        internal void ToolBarEnabledControl(MenuItemEnums menu)
        {
            switch (menu)
            {
                case MenuItemEnums.反审:
                    toolStripbtnReview.Enabled = true;
                    toolStripBtnReverseReview.Enabled = false;//先不支持反审
                    toolStripButtonRefresh.Enabled = true;
                    toolStripbtnModify.Enabled = true;
                    toolStripbtnDelete.Enabled = true;
                    break;

                case MenuItemEnums.审核:
                    toolStripbtnReview.Enabled = false;
                    toolStripBtnReverseReview.Enabled = true;//先不支持反审
                    toolStripbtnSubmit.Enabled = false;
                    toolStripButtonSave.Enabled = false;
                    toolStripbtnDelete.Enabled = false;
                    toolStripbtnModify.Enabled = false;
                    toolStripButtonRefresh.Enabled = true;

                    break;
                case MenuItemEnums.新增:
                    toolStripbtnAdd.Enabled = false;
                    toolStripButtonSave.Enabled = true;
                    toolStripbtnReview.Enabled = false;
                    toolStripBtnCancel.Visible = true;
                    toolStripbtnModify.Enabled = false;
                    toolStripBtnCancel.Enabled = true;
                    toolStripbtnDelete.Enabled = false;
                    break;

                case MenuItemEnums.取消:
                    toolStripbtnAdd.Enabled = true;
                    toolStripBtnCancel.Visible = false;
                    break;
                case MenuItemEnums.删除://可新增
                    toolStripbtnAdd.Enabled = true;
                    toolStripbtnDelete.Enabled = false;
                    break;

                case MenuItemEnums.修改:
                    toolStripbtnModify.Enabled = false;
                    toolStripButtonSave.Enabled = true;
                    break;
                case MenuItemEnums.查询:
                    toolStripbtnAdd.Enabled = true;
                    toolStripButtonSave.Enabled = false;
                    toolStripbtnPrint.Enabled = true;
                    toolStripbtnModify.Enabled = true;
                    break;
                case MenuItemEnums.保存:
                    toolStripbtnAdd.Enabled = true;
                    toolStripButtonSave.Enabled = false;
                    toolStripbtnSubmit.Enabled = true;
                    toolStripbtnPrint.Enabled = true;
                    toolStripbtnAdd.Enabled = true;
                    toolStripbtnDelete.Enabled = true;
                    toolStripButtonRefresh.Enabled = true;
                    break;
                //case MenuItemEnums.高级查询:
                //    break;
                case MenuItemEnums.关闭:
                    break;
                case MenuItemEnums.刷新:
                    toolStripbtnAdd.Enabled = true;
                    toolStripButtonSave.Enabled = false;
                    break;
                case MenuItemEnums.打印:
                    toolStripbtnPrint.Enabled = false;
                    break;
                case MenuItemEnums.提交:
                    toolStripbtnSubmit.Enabled = false;
                    toolStripButtonSave.Enabled = false;
                    toolStripbtnReview.Enabled = true;
                    toolStripButtonRefresh.Enabled = true;
                    break;
                case MenuItemEnums.结案:
                    toolStripbtnSubmit.Enabled = false;
                    toolStripButtonSave.Enabled = false;
                    toolStripbtnReview.Enabled = false;
                    toolStripbtnAdd.Enabled = true;
                    toolStripbtnDelete.Enabled = false;
                    toolStripBtnCancel.Visible = true;
                    toolStripbtnModify.Enabled = false;
                    toolStripBtnCancel.Enabled = true;
                    toolStripbtnPrint.Enabled = true;
                    toolStripButtonRefresh.Enabled = true;
                    break;
                case MenuItemEnums.导出:

                    break;

                default:
                    break;
            }
            Edited = toolStripButtonSave.Enabled;
        }

        /// <summary>
        /// 根据单据实体属性状态来对应显示各种按钮控制
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void ToolBarEnabledControl(object entity)
        {
            if (entity == null) return;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);

            //可以修改
            if (entity.ContainsProperty(typeof(DataStatus).Name))
            {
                DataStatus dataStatus = (DataStatus)int.Parse(entity.GetPropertyValue(typeof(DataStatus).Name).ToString());
                ActionStatus actionStatus = (ActionStatus)(Enum.Parse(typeof(ActionStatus), entity.GetPropertyValue(typeof(ActionStatus).Name).ToString()));
                switch (dataStatus)
                {
                    //点新增
                    case DataStatus.草稿:
                        toolStripbtnAdd.Enabled = false;
                        toolStripBtnCancel.Visible = true;
                        toolStripbtnModify.Enabled = true;
                        toolStripbtnSubmit.Enabled = true;
                        toolStripbtnReview.Enabled = false;
                        
                        if (actionStatus == ActionStatus.新增)
                        {
                            toolStripButtonSave.Enabled = true;
                            toolStripbtnModify.Enabled = false;
                        }
                        else
                        {
                            toolStripButtonSave.Enabled = false;
                        }

                        toolStripBtnReverseReview.Enabled = false;
                        toolStripbtnPrint.Enabled = false;
                        toolStripbtnDelete.Enabled = true;
                        toolStripButton结案.Enabled = false;
                        break;
                    case DataStatus.新建:
                        toolStripbtnAdd.Enabled = false;
                        toolStripBtnCancel.Visible = true;
                        toolStripbtnModify.Enabled = true;
                        toolStripbtnSubmit.Enabled = false;
                        toolStripBtnReverseReview.Enabled = false;
                        toolStripbtnReview.Enabled = true;
                        toolStripButtonSave.Enabled = true;
                        toolStripbtnDelete.Enabled = true;
                        toolStripbtnPrint.Enabled = true;
                        toolStripButton结案.Enabled = false;
                        break;
                    case DataStatus.确认:
                        toolStripbtnModify.Enabled = false;
                        toolStripbtnSubmit.Enabled = false;
                        toolStripBtnReverseReview.Enabled = true;
                        toolStripbtnReview.Enabled = false;
                        toolStripButtonSave.Enabled = false;
                        toolStripbtnPrint.Enabled = true;
                        toolStripButton结案.Enabled = true;
                        toolStripbtnDelete.Enabled = false;
                        break;
                    case DataStatus.完结:
                        //
                        toolStripbtnModify.Enabled = false;
                        toolStripbtnSubmit.Enabled = false;
                        toolStripbtnReview.Enabled = false;
                        toolStripButtonSave.Enabled = false;
                        toolStripBtnReverseReview.Enabled = false;
                        toolStripbtnPrint.Enabled = true;
                        toolStripButton结案.Enabled = false;
                        toolStripBtnCancel.Enabled = false;
                        toolStripbtnDelete.Enabled = false;
                        break;
                    default:
                        break;
                }

                //单据被锁定时。显示锁定图标。并且提示无法操作？
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
                if (pkid > 0)
                {
                    //如果要锁这个单 看这个单是不是已经被其它人锁，如果没有人锁则我可以锁
                    //TODO 注意 同一个人，同一个业务单据。只能锁定一张单。所以在锁新单时。清除所有旧单。
                    //关闭时会解锁，查询的方式不停加载也要解锁前面的
                    long userid = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

                    //解锁这个业务的自己名下的其它单
                    UNLockByBizName(userid);

                    if (MainForm.Instance.lockManager.GetLockedBy(pkid) > 0)
                    {
                        var lockinfo = MainForm.Instance.lockManager.GetLockStatus(pkid);
                        if (lockinfo.LockedByID == userid)
                        {
                            //得到了锁 就是自己。得不到就是
                            tsBtnLocked.AutoToolTip = false;
                            tsBtnLocked.ToolTipText = string.Empty;
                            //别人锁定了
                            string tipMsg = $"您锁定了当前单据。";
                            MainForm.Instance.uclog.AddLog(tipMsg);
                            tsBtnLocked.AutoToolTip = true;
                            tsBtnLocked.ToolTipText = tipMsg;
                            tsBtnLocked.Visible = true;
                            tsBtnLocked.Tag = lockinfo;
                            //自己就表达绿色
                            this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.unlockbill;



                        }
                        else
                        {
                            //别人锁定了
                            string tipMsg = $"单据已被用户【{lockinfo.LockedByName}】锁定，请刷新后再试,或点击【已锁定】联系锁定人员解锁。";
                            MainForm.Instance.uclog.AddLog(tipMsg);
                            tsBtnLocked.AutoToolTip = true;
                            tsBtnLocked.ToolTipText = tipMsg;
                            tsBtnLocked.Visible = true;
                            tsBtnLocked.Tag = lockinfo;
                            this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.Lockbill;
                            toolStripBtnCancel.Visible = true;
                            toolStripbtnModify.Enabled = false;
                            toolStripbtnSubmit.Enabled = false;
                            toolStripBtnReverseReview.Enabled = false;
                            toolStripbtnReview.Enabled = false;
                            toolStripButtonSave.Enabled = false;
                            toolStripbtnDelete.Enabled = false;
                            toolStripbtnPrint.Enabled = false;
                            toolStripButton结案.Enabled = false;
                        }
                    }
                    else
                    {
                        //没人锁定
                        tsBtnLocked.AutoToolTip = false;
                        tsBtnLocked.ToolTipText = string.Empty;
                        tsBtnLocked.Visible = false;
                        tsBtnLocked.Tag = null;
                    }
                }

                #region 数据状态修改时也会影响到按钮
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.AcceptChanges();
                    //为了不重复执行
                    // 定义一个局部变量来存储事件处理程序
                    EventHandler<ActionStatusChangedEventArgs> eventHandler = (sender, s2) =>
                    {
                        if (s2.OldValue != ActionStatus.修改 && s2.NewValue == ActionStatus.修改)
                        {
                            LockBill();
                        }
                    };

                    // 先移除之前可能添加的处理程序
                    baseEntity.ActionStatusChanged -= eventHandler;
                    // 再添加处理程序
                    baseEntity.ActionStatusChanged += eventHandler;


                    //如果属性变化 则状态为修改
                    baseEntity.PropertyChanged += (sender, s2) =>
                {
                    //权限允许
                    if ((true && dataStatus == DataStatus.草稿) || (true && dataStatus == DataStatus.新建))
                    {
                        baseEntity.ActionStatus = ActionStatus.修改;
                        ToolBarEnabledControl(MenuItemEnums.修改);
                    }

                    //数据状态变化会影响按钮变化
                    if (s2.PropertyName == "DataStatus")
                    {
                        if (dataStatus == DataStatus.草稿)
                        {
                            ToolBarEnabledControl(MenuItemEnums.新增);
                        }
                        if (dataStatus == DataStatus.新建)
                        {
                            ToolBarEnabledControl(MenuItemEnums.新增);
                        }
                        if (dataStatus == DataStatus.确认)
                        {
                            ToolBarEnabledControl(MenuItemEnums.审核);
                        }

                        if (dataStatus == DataStatus.完结)
                        {
                            ToolBarEnabledControl(MenuItemEnums.结案);
                        }

                    }

                };




                }
                #endregion
            }

            // 获取状态类型和值

            var statusType = FMPaymentStatusHelper.GetStatusType(entity as BaseEntity);

            if (statusType == null) return;

            // 动态获取状态值
            dynamic status = entity.GetPropertyValue(statusType.Name);
            int statusValue = (int)status;
            dynamic statusEnum = Enum.ToObject(statusType, statusValue);

            // 通用按钮状态
            bool isEditable = FMPaymentStatusHelper.CanModify(statusEnum);
            bool canCancel = FMPaymentStatusHelper.CanCancel(statusEnum, HasRelatedRecords(entity as BaseEntity));

            toolStripbtnModify.Enabled = isEditable;
            toolStripButtonSave.Enabled = isEditable;
            toolStripbtnDelete.Enabled = isEditable;
            toolStripBtnCancel.Visible = canCancel;

            // 特殊操作按钮
            //ConfigureSpecialButtons(statusEnum);

            // 通用按钮状态
            toolStripbtnSubmit.Enabled = FMPaymentStatusHelper.CanSubmit(statusEnum);
            toolStripbtnReview.Enabled = statusEnum is PrePaymentStatus pre && pre == PrePaymentStatus.待审核 ||
                                        statusEnum is ARAPStatus arap && arap == ARAPStatus.待审核 ||
                                        statusEnum is DataStatus dataStatus1  && dataStatus1 == DataStatus.新建||
                                        statusEnum is StatementStatus statementStatus && statementStatus == StatementStatus.已发送 ||
                                        statusEnum is PaymentStatus pay && pay == PaymentStatus.待审核;


            // 状态检测器
            var statusDetector = new StatusDetector(entity as BaseEntity);
            // 锁定状态处理
            HandleLockStatus(entity as BaseEntity, statusDetector);



        }

        #region 状态处理私有方法

        private void ConfigureSpecialButtons(dynamic status)
        {
            toolStripButton结案.Visible = false;
            //toolStripButton坏账.Visible = false;

            if (status is PrePaymentStatus preStatus)
            {
                toolStripButton结案.Visible = preStatus == PrePaymentStatus.待核销;
                toolStripButton结案.Enabled = preStatus == PrePaymentStatus.待核销;
            }
            else if (status is ARAPStatus arapStatus)
            {
                toolStripButton结案.Visible = arapStatus == ARAPStatus.待支付 ||
                                             arapStatus == ARAPStatus.部分支付;
                toolStripButton结案.Enabled = FMPaymentStatusHelper.CanReverse(arapStatus);

                //toolStripButton坏账.Visible = toolStripButton结案.Visible;
                //toolStripButton坏账.Enabled = toolStripButton结案.Enabled;
            }
        }

        private bool HasRelatedRecords(BaseEntity entity)
        {
            // 实现关联记录检查逻辑
            // 例如：检查是否有核销记录、支付记录等
            return false;
        }





        private long GetPrimaryKeyValue(BaseEntity entity)
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            return pkid;
        }

        private void DisableAllOperations()
        {
            toolStripbtnModify.Enabled = false;
            toolStripbtnSubmit.Enabled = false;
            toolStripBtnReverseReview.Enabled = false;
            toolStripbtnReview.Enabled = false;
            toolStripButtonSave.Enabled = false;
            toolStripbtnDelete.Enabled = false;
            toolStripbtnPrint.Enabled = false;
            toolStripButton结案.Enabled = false;
        }
        #endregion

        #region 辅助方法
        private Type GetActualStatusType(BaseEntity entity)
        {
            if (entity.ContainsProperty(typeof(PrePaymentStatus).Name)) return typeof(PrePaymentStatus);
            if (entity.ContainsProperty(typeof(ARAPStatus).Name)) return typeof(ARAPStatus);
            if (entity.ContainsProperty(typeof(PaymentStatus).Name)) return typeof(PaymentStatus);
            throw new InvalidOperationException("未知状态类型");
        }

        private Enum GetStatusValue(BaseEntity entity, Type statusType)
        {
            object value = entity.GetPropertyValue(statusType.Name);
            return (Enum)Enum.Parse(statusType, value.ToString());
        }



        #endregion


        #region 状态机处理新2025-6-17

        /// <summary>根据单据状态控制工具栏按钮</summary>
        protected virtual void ToolBarEnabledControl(BaseEntity entity)
        {
            if (entity == null) return;
            // 状态检测器
            var statusDetector = new StatusDetector(entity);

            // 通用按钮状态
            toolStripbtnAdd.Enabled = statusDetector.IsEditable;
            //toolStripBtnCancel.Visible = statusDetector.CanCancel;
            toolStripbtnModify.Enabled = statusDetector.IsEditable;
            toolStripbtnDelete.Enabled = statusDetector.IsEditable;
            toolStripButtonSave.Enabled = entity.HasChanged;

            // 特定状态按钮
            toolStripbtnSubmit.Enabled = statusDetector.CanSubmit;
            toolStripbtnReview.Enabled = statusDetector.CanReview;
            toolStripBtnReverseReview.Enabled = statusDetector.CanReverseReview;
            toolStripBtnReverseReview.Visible = statusDetector.CanReverseReview;
            toolStripButton结案.Enabled = statusDetector.CanClose;
            //  RefreshToolbar
            statusDetector.RefreshToolbar += (actionStatus, statusValue) =>
            {
                if (statusValue == null) return;


                //if (statusValue == typeof(PrePaymentStatus))
                //{
                //    HandlePrePaymentStatus((PrePaymentStatus)statusValue, actionStatus);
                //}
                //else if (statusValue == typeof(ARAPStatus))
                //{
                //    HandleARAPStatus((ARAPStatus)statusValue, actionStatus);
                //}
                //else if (statusValue == typeof(PaymentStatus))
                //{
                //    HandlePaymentStatus((PaymentStatus)statusValue, actionStatus);
                //}
            };
            // 锁定状态处理
            HandleLockStatus(entity, statusDetector);

            // 注册状态变更事件
            statusDetector.RegisterStatusChangeHandler();
        }

        //private void HandleARAPStatus(ARAPStatus statusValue, ActionStatus actionStatus)
        //{
        //    toolStripbtnSubmit.Enabled = statusValue == ARAPStatus.草稿;
        //    toolStripbtnReview.Enabled = statusValue == ARAPStatus.待审核;
        //    toolStripButton结案.Visible = false; // 付款单不需要结案按钮
        //}

        /// <summary>状态检测器 - 封装状态逻辑</summary>
        private class StatusDetector
        {
            private readonly BaseEntity _entity;

            //设置一个RefreshToolbar事件
            public StatusChangeHandler RefreshToolbar;
            public delegate void StatusChangeHandler(ActionStatus actionStatus, Enum StatusValue);

            public bool IsEditable { get; }
            public bool CanCancel { get; }
            public bool CanSubmit { get; }

            /// <summary>
            /// 可审核
            /// </summary>
            public bool CanReview { get; }

            /// <summary>
            /// 可反审
            /// </summary>
            public bool CanReverseReview { get; }

            public bool CanClose { get; }
            public bool IsFinalStatus { get; }

            public StatusDetector(BaseEntity entity)
            {
                _entity = entity;

                // 获取实际状态值
                var status = GetActualStatus();
                var hasRelatedRecords = CheckRelatedRecords();

                //可冲销
                bool CanReverse = false;
                // 计算属性值
                IsFinalStatus = FMPaymentStatusHelper.IsFinalStatus(status);
                IsEditable = FMPaymentStatusHelper.CanModify(status);
                CanCancel = FMPaymentStatusHelper.CanCancel(status, false);

                switch (status)
                {
                    case PrePaymentStatus pre:
                        CanSubmit = pre == PrePaymentStatus.草稿;
                        CanReview = pre == PrePaymentStatus.待审核;
                        CanReverseReview = pre == PrePaymentStatus.待核销 || pre == PrePaymentStatus.已生效;
                        CanClose = pre == PrePaymentStatus.待核销; // 可退款
                        CanReverse = false;
                        break;

                    case ARAPStatus arap:
                        CanSubmit = arap == ARAPStatus.草稿;
                        CanReview = arap == ARAPStatus.待审核;
                        CanReverseReview = arap == ARAPStatus.待支付;
                        //CanClose = arap.CanReverse(); // 可冲销
                        //CanReverse = arap.CanReverse();
                        break;

                    case PaymentStatus pay:
                        CanSubmit = pay == PaymentStatus.草稿;
                        CanReview = pay == PaymentStatus.待审核;
                        CanReverseReview = false;
                        CanClose = false;
                        CanReverse = false;
                        break;
                    case StatementStatus statementStatus:
                        CanSubmit = statementStatus == StatementStatus.草稿;
                        CanReview = statementStatus == StatementStatus.已发送;
                        CanReverseReview = statementStatus == StatementStatus.已确认;
                        CanClose = false;
                        CanReverse = false;
                        break;
                    case DataStatus dataStatus:
                        CanSubmit = dataStatus == DataStatus.草稿;
                        CanReview = dataStatus == DataStatus.新建;
                        CanReverseReview = dataStatus == DataStatus.确认;
                        CanClose = false;
                        CanReverse = false;
                        break;
                }
            }



            public Enum GetActualStatus()
            {
                if (_entity.ContainsProperty(typeof(DataStatus).Name))
                    return _entity.GetStatusValue<DataStatus>();

                if (_entity.ContainsProperty(typeof(PrePaymentStatus).Name))
                    return _entity.GetStatusValue<PrePaymentStatus>();

                if (_entity.ContainsProperty(typeof(ARAPStatus).Name))
                    return _entity.GetStatusValue<ARAPStatus>();

                if (_entity.ContainsProperty(typeof(PaymentStatus).Name))
                    return _entity.GetStatusValue<PaymentStatus>();

                return null;
            }

            private bool CheckRelatedRecords()
            {
                // 实现关联记录检查逻辑
                return false;
            }

            public void RegisterStatusChangeHandler()
            {
                _entity.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName.EndsWith("Status"))
                    {
                        // 刷新UI状态
                        // RefreshToolbar();
                        if (RefreshToolbar != null)
                        {
                            if (e.PropertyName == nameof(DataStatus))
                            {
                                //RefreshToolbar(actionStatus: ActionStatus.更新, statusValue: _entity.GetPropertyValue(nameof(DataStatus)));
                            }

                        }
                    }
                };
            }
        }

        /// <summary>处理单据锁定状态</summary>
        private void HandleLockStatus(BaseEntity entity, StatusDetector detector)
        {
            if (entity == null) return;

            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);

            if (pkid <= 0) return;

            var lockInfo = MainForm.Instance.lockManager.GetLockStatus(pkid);
            bool isLocked = lockInfo?.LockedByID > 0;
            bool isSelfLock = lockInfo?.LockedByID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

            // 更新锁定按钮状态
            tsBtnLocked.Visible = isLocked;
            tsBtnLocked.Image = isSelfLock ?
                Properties.Resources.unlockbill :
                Properties.Resources.Lockbill;

            tsBtnLocked.ToolTipText = isSelfLock ?
                "您已锁定当前单据" :
                $"单据已被【{lockInfo?.LockedByName}】锁定";

            // 被他人锁定时禁用所有操作
            if (isLocked && !isSelfLock)
            {
                toolStripbtnModify.Enabled = false;
                toolStripbtnSubmit.Enabled = false;
                toolStripbtnReview.Enabled = false;
                toolStripButtonSave.Enabled = false;
                toolStripbtnDelete.Enabled = false;

                // 根据不同类型禁用特定按钮
                if (detector.GetActualStatus() is PrePaymentStatus)
                {
                    toolStripButton结案.Enabled = false; // 退款按钮
                }
                else if (detector.GetActualStatus() is ARAPStatus)
                {
                    toolStripButton结案.Enabled = false; // 冲销按钮
                    //toolStripButton坏账.Enabled = false;
                }
            }
            else if (isSelfLock)
            {
                // 自己锁定时启用适当操作
                EnableOperationsBasedOnStatus(detector.GetActualStatus());
            }
        }
        /// <summary>根据状态启用操作按钮</summary>
        private void EnableOperationsBasedOnStatus(Enum status)
        {
            if (status is PrePaymentStatus preStatus)
            {
                toolStripbtnSubmit.Enabled = FMPaymentStatusHelper.CanSubmit(preStatus);
                toolStripbtnReview.Enabled = preStatus == PrePaymentStatus.待审核;
                toolStripButton结案.Enabled = preStatus == PrePaymentStatus.待核销;
            }
            else if (status is ARAPStatus arapStatus)
            {
                toolStripbtnSubmit.Enabled = FMPaymentStatusHelper.CanSubmit(arapStatus);
                toolStripbtnReview.Enabled = arapStatus == ARAPStatus.待审核;
                toolStripButton结案.Enabled = FMPaymentStatusHelper.CanReverse(arapStatus);
                //toolStripButton坏账.Enabled = arapStatus == ARAPStatus.待支付 ||arapStatus == ARAPStatus.部分支付;
            }
            else if (status is PaymentStatus payStatus)
            {
                toolStripbtnSubmit.Enabled = FMPaymentStatusHelper.CanSubmit(payStatus);
                toolStripbtnReview.Enabled = payStatus == PaymentStatus.待审核;
            }
        }

        /// <summary>设置实体状态属性值</summary>
        private void SetEntityStatus<TStatus>(BaseEntity entity, TStatus status) where TStatus : Enum
        {
            string propertyName = typeof(TStatus).Name;
            if (entity.ContainsProperty(propertyName))
            {
                ReflectionHelper.SetPropertyValue(entity, propertyName, (int)(object)status);
            }
        }


        /*
       private void HandleLockStatus(BaseEntity entity, StatusDetector detector)
       {    
           //单据被锁定时。显示锁定图标。并且提示无法操作？
           string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
           long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
           if (pkid <= 0) return;

           var lockInfo = MainForm.Instance.lockManager.GetLockStatus(pkid);
           bool isLocked = lockInfo?.LockedByID > 0;
           bool isSelfLock = lockInfo?.LockedByID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

           // 更新锁定按钮状态
           tsBtnLocked.Visible = isLocked;
           tsBtnLocked.Image = isSelfLock ?
               Properties.Resources.unlockbill :
               Properties.Resources.Lockbill;

           tsBtnLocked.ToolTipText = isSelfLock ?
      "您已锁定当前单据" :
      $"单据已被【{lockInfo?.LockedByName}】锁定";
           // 被他人锁定时禁用所有操作
           if (isLocked && !isSelfLock)
           {
               toolStripbtnModify.Enabled = false;
               toolStripbtnSubmit.Enabled = false;
               toolStripbtnReview.Enabled = false;
               toolStripButtonSave.Enabled = false;
               toolStripbtnDelete.Enabled = false;
               toolStripButton结案.Enabled = false;
           }

           if (pkid > 0)
           {
               //如果要锁这个单 看这个单是不是已经被其它人锁，如果没有人锁则我可以锁
               //TODO 注意 同一个人，同一个业务单据。只能锁定一张单。所以在锁新单时。清除所有旧单。
               //关闭时会解锁，查询的方式不停加载也要解锁前面的
               long userid = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

               //解锁这个业务的自己名下的其它单
               UNLockByBizName(userid);

               if (MainForm.Instance.lockManager.GetLockedBy(pkid) > 0)
               {
                   var lockinfo = MainForm.Instance.lockManager.GetLockStatus(pkid);
                   if (lockinfo.LockedByID == userid)
                   {
                       //得到了锁 就是自己。得不到就是
                       tsBtnLocked.AutoToolTip = false;
                       tsBtnLocked.ToolTipText = string.Empty;
                       //别人锁定了
                       string tipMsg = $"您锁定了当前单据。";
                       MainForm.Instance.uclog.AddLog(tipMsg);
                       tsBtnLocked.AutoToolTip = true;
                       tsBtnLocked.ToolTipText = tipMsg;
                       tsBtnLocked.Visible = true;
                       tsBtnLocked.Tag = lockinfo;
                       //自己就表达绿色
                       this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.unlockbill;



                   }
                   else
                   {
                       //别人锁定了
                       string tipMsg = $"单据已被用户【{lockinfo.LockedByName}】锁定，请刷新后再试,或点击【已锁定】联系锁定人员解锁。";
                       MainForm.Instance.uclog.AddLog(tipMsg);
                       tsBtnLocked.AutoToolTip = true;
                       tsBtnLocked.ToolTipText = tipMsg;
                       tsBtnLocked.Visible = true;
                       tsBtnLocked.Tag = lockinfo;
                       this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.Lockbill;
                       toolStripBtnCancel.Visible = true;
                       toolStripbtnModify.Enabled = false;
                       toolStripbtnSubmit.Enabled = false;
                       toolStripBtnReverseReview.Enabled = false;
                       toolStripbtnReview.Enabled = false;
                       toolStripButtonSave.Enabled = false;
                       toolStripbtnDelete.Enabled = false;
                       toolStripbtnPrint.Enabled = false;
                       toolStripButton结案.Enabled = false;
                   }
               }
               else
               {
                   //没人锁定
                   tsBtnLocked.AutoToolTip = false;
                   tsBtnLocked.ToolTipText = string.Empty;
                   tsBtnLocked.Visible = false;
                   tsBtnLocked.Tag = null;
               }
           }

           return;

       }

         */

        ///// <summary>刷新工具栏状态</summary>
        public void RefreshToolbar()
        {
            if (EditEntity != null)
            {
                ToolBarEnabledControl(EditEntity);
            }
        }

        #endregion


        #region 帮助信息提示


        public void InitHelpInfoToControl(System.Windows.Forms.Control.ControlCollection Controls)
        {
            foreach (var item in Controls)
            {
                if (item is Control)
                {
                    if (item.GetType().Name == "KryptonTextBox")
                    {
                        KryptonTextBox ktb = item as KryptonTextBox;
                        if ((item as Control).DataBindings.Count > 0)
                        {
                            if (GetHelpInfoByBinding(ktb.DataBindings).Length > 0)
                            {
                                ButtonSpecAny bsa = new ButtonSpecAny();
                                bsa.Image = System.Drawing.Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                bsa.Tag = ktb;
                                bsa.Click += Bsa_Click;
                                ktb.ButtonSpecs.Add(bsa);

                                //可以边框为红色不？
                                //或必填项目有特别提示？
                            }
                        }
                    }
                }

            }
        }





        private void Bsa_Click(object sender, EventArgs e)
        {
            ProcessHelpInfo(true, sender);
        }




        public new void ProcessHelpInfo(bool fromBtn, object sender)
        {
            string tipTxt = string.Empty;
            //时长timeout默认值设置的是3000ms(也就是3秒)
            int timeout = 3000;
            _timeoutTimer4tips = new System.Threading.Timer(OnTimerElapsed, null, timeout, System.Threading.Timeout.Infinite);
            toolTipBase.Hide(this);
            if (fromBtn)
            {
                ButtonSpecAny bsa = sender as ButtonSpecAny;
                tipTxt = GetHelpInfoByBinding((bsa.Owner as KryptonTextBox).DataBindings);
                Control ctl = bsa.Owner as Control;
                if (string.IsNullOrEmpty(tipTxt))
                {
                    return;
                }
                toolTipBase.SetToolTip(ctl, tipTxt);
                toolTipBase.Show(tipTxt, ctl);
            }
            else
            {
                #region F1
                if (ActiveControl.GetType().ToString() == "Krypton.Toolkit.KryptonTextBox+InternalTextBox")
                {
                    KryptonTextBox txt = ActiveControl.Parent as KryptonTextBox;
                    tipTxt = GetHelpInfoByBinding(txt.DataBindings);
                    //if (txt.DataBindings.Count > 0)
                    //{
                    //    string filedName = txt.DataBindings[0].BindingMemberInfo.BindingField;
                    //    string[] cns = txt.DataBindings[0].BindingManagerBase.Current.ToString().Split('.');
                    //    string className = cns[cns.Length - 1];
                    //    var obj = Startup.AutoFacContainer.ResolveNamed<BaseEntity>(className);
                    //    if (obj.HelpInfos.ContainsKey(filedName))
                    //    {
                    //        tipTxt = "【" + obj.FieldNameList.Find(f => f.Key == filedName).Value.Trim() + "】";
                    //        tipTxt += obj.HelpInfos[filedName].ToString();
                    //    }

                    //}
                }
                else
                {

                }
                if (string.IsNullOrEmpty(tipTxt))
                {
                    return;
                }
                toolTipBase.SetToolTip(ActiveControl, tipTxt);
                toolTipBase.Show(tipTxt, ActiveControl);
                #endregion
            }





        }


        /// <summary>
        /// 获取帮助信息集合对应的值
        /// </summary>
        /// <param name="cbc"></param>
        /// <returns></returns>
        private string GetHelpInfoByBinding(ControlBindingsCollection cbc)
        {
            string tipTxt = string.Empty;
            if (cbc.Count > 0)
            {
                string filedName = cbc[0].BindingMemberInfo.BindingField;
                if (cbc[0].BindingManagerBase == null)
                {
                    return tipTxt;
                }
                string[] cns = cbc[0].BindingManagerBase.Current.ToString().Split('.');
                string className = cns[cns.Length - 1];

                var obj = Startup.GetFromFacByName<BaseEntity>(className);
                if (obj.HelpInfos != null)
                {
                    if (obj.HelpInfos.ContainsKey(filedName))
                    {
                        tipTxt = "【" + obj.FieldNameList[filedName].Trim() + "】";
                        tipTxt += obj.HelpInfos[filedName].ToString();
                    }
                }


            }
            return tipTxt;
        }



        public void OnTimerElapsed(object state)
        {
            toolTipBase.Hide(this);
            _timeoutTimer4tips.Dispose();

        }
        private void toolTipBase_Popup(object sender, PopupEventArgs e)
        {
            //ToolTip tool = (ToolTip)sender;
            //if (e.AssociatedControl.Name == "textBox1")//e代表我们要触发的事件，我们是在textBox1触发
            //{
            //    tool.ToolTipTitle = "提示信息";//修改标题
            //    tool.ToolTipIcon = ToolTipIcon.Info;//修改图标
            //}
            //else
            //{
            //    tool.ToolTipTitle = "";
            //    tool.ToolTipIcon = ToolTipIcon.Info;
            //}
        }
        private void timerForToolTip_Tick(object sender, EventArgs e)
        {

        }
        #endregion



        #region 特殊显示必填项


        /// <summary>
        /// 特殊显示必填项 
        /// </summary>
        /// <typeparam name="T">要验证必填的类型</typeparam>
        /// <param name="rules"></param>
        /// <param name="Controls"></param>
        public void InitRequiredToControl(AbstractValidator<T> rules, System.Windows.Forms.Control.ControlCollection Controls)
        {
            List<string> notEmptyList = new List<string>();
            List<string> checkList = new List<string>();
            foreach (var item in rules)
            {
                string colName = item.PropertyName;
                var rr = item.Components;
                foreach (var com in item.Components)
                {
                    if (com.Validator.Name == "NotEmptyValidator")
                    {
                        //这里找到了不能为空的验证器。为了体验在UI
                        notEmptyList.Add(colName);
                    }
                    else
                    if (com.Validator.Name == "PredicateValidator")
                    {
                        checkList.Add(colName);
                    }
                }
            }


            foreach (var item in Controls)
            {
                if (item is Control)
                {
                    if (item is VisualControlBase)
                    {
                        if (item.GetType().Name == "KryptonTextBox")
                        {
                            KryptonTextBox ktb = item as KryptonTextBox;
                            if ((item as Control).DataBindings.Count > 0)
                            {
                                #region 找到绑定的字段
                                if (ktb.DataBindings.Count > 0)
                                {
                                    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;
                                    string col = notEmptyList.FirstOrDefault(c => c == filedName);
                                    if (col.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.Border.Color1 = Color.FromArgb(255, 128, 128);
                                    }

                                    string colchk = checkList.FirstOrDefault(c => c == filedName);
                                    if (colchk.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.Border.Color1 = Color.FromArgb(255, 0, 204);
                                    }

                                }
                                #endregion


                            }
                        }
                        if (item.GetType().Name == "KryptonComboBox")
                        {
                            KryptonComboBox ktb = item as KryptonComboBox;
                            if ((item as Control).DataBindings.Count > 0)
                            {
                                //ButtonSpecAny bsa = new ButtonSpecAny();
                                // bsa.Image = Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                // bsa.Tag = ktb;
                                //bsa.Click += Bsa_Click;
                                // ktb.ButtonSpecs.Add(bsa);
                                // ktb.StateCommon.Border.Color1 =  Color.FromArgb(255, 128, 128);
                                //可以边框为红色不？
                                //或必填项目有特别提示？
                                #region 找到绑定的字段
                                if (ktb.DataBindings.Count > 0)
                                {
                                    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;
                                    string col = notEmptyList.FirstOrDefault(c => c == filedName);
                                    if (col.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(255, 128, 128);
                                    }
                                    string colchk = checkList.FirstOrDefault(c => c == filedName);
                                    if (colchk.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(255, 0, 204);
                                    }
                                }
                                #endregion


                            }
                        }
                    }
                }
            }
        }

        #endregion



        #region 基础资料下拉添加编辑项





        Expression<Func<TParent, bool>> Wrap<TParent, TElement>(Expression<Func<TParent, IEnumerable<TElement>>> collection, Expression<Func<TElement, bool>> isOne, Expression<Func<IEnumerable<TElement>, Func<TElement, bool>, bool>> isAny)
        {
            var parent = Expression.Parameter(typeof(TParent), "parent");

            return
                (Expression<Func<TParent, bool>>)Expression.Lambda
                (
                    Expression.Invoke
                    (
                        isAny,
                        Expression.Invoke
                        (
                            collection,
                            parent
                        ),
                        isOne
                    ),
                    parent
                );
        }


        #endregion

        public delegate void BindDataToUIHander(T entity, ActionStatus actionStatus);

        [Browsable(true), Description("绑定数据对象到UI")]
        public event BindDataToUIHander OnBindDataToUIEvent;



        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected async override void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            // this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

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


            //操作前是不是锁定。自己排除
            long pkid = 0;
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {
                case MenuItemEnums.联查:
                    break;
                case MenuItemEnums.已锁定:
                    if (tsBtnLocked.Tag is LockInfo lockRequest)
                    {
                        //如果是自己时则不能申请
                        if (lockRequest.LockedByID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID)
                        {
                            return;
                        }
                        //弹出一个确认框来请求解锁 
                        if (tsBtnLocked.Visible && MessageBox.Show($"确认向锁定者【{lockRequest.LockedByName}】申请解锁吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            RequestUnLock();
                        }
                    }

                    break;
                case MenuItemEnums.新增:
                    Add();
                    break;
                case MenuItemEnums.取消:
                    Cancel();
                    break;
                case MenuItemEnums.复制性新增:
                    AddByCopy();
                    break;

                case MenuItemEnums.数据特殊修正:
                    SpecialDataFix();
                    break;

                case MenuItemEnums.删除:
                    if (IsLock())
                    {
                        return;
                    }
                    await Delete();

                    break;
                case MenuItemEnums.修改:
                    if (IsLock())
                    {
                        return;
                    }
                    LockBill();
                    Modify();
                    break;
                case MenuItemEnums.查询:
                    Query();
                    break;
                case MenuItemEnums.保存:
                    if (IsLock())
                    {
                        return;
                    }
                    //操作前将数据收集
                    this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
                    if (EditEntity != null)
                    {
                        pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
                        if (pkid > 0)
                        {
                            //如果有审核状态才去判断
                            if (editEntity.ContainsProperty(typeof(DataStatus).Name))
                            {
                                var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                                if (dataStatus == DataStatus.完结 || dataStatus == DataStatus.确认)
                                {
                                    toolStripbtnSubmit.Enabled = false;
                                    if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                                    {
                                        MainForm.Instance.uclog.AddLog("已经是【完结】或【确认】状态，保存失败。");
                                    }
                                    return;
                                }
                            }

                        }
                        toolStripButtonSave.Enabled = false;
                        bool rsSave = await Save(true);
                        if (!rsSave)
                        {
                            toolStripButtonSave.Enabled = true;
                        }

                        UNLock();
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("单据不能为空，保存失败。");
                    }


                    break;
                case MenuItemEnums.提交:
                    if (IsLock())
                    {
                        return;
                    }
                    //操作前将数据收集
                    this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

                    toolStripbtnSubmit.Enabled = false;
                    bool rs = await Submit();
                    if (!rs)
                    {
                        toolStripbtnSubmit.Enabled = true;
                    }
                    break;
                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.刷新:
                    Refreshs();
                    break;
                case MenuItemEnums.属性:
                    Property();
                    break;
                case MenuItemEnums.审核:
                    if (IsLock())
                    {
                        return;
                    }
                    LockBill();
                    toolStripbtnReview.Enabled = false;
                    ReviewResult reviewResult = await Review();
                    if (!reviewResult.Succeeded)
                    {
                        UNLock();
                        toolStripbtnReview.Enabled = true;
                    }

                    break;
                case MenuItemEnums.反审:
                    if (IsLock())
                    {
                        return;
                    }
                    LockBill();
                    toolStripBtnReverseReview.Enabled = false;
                    bool rs反审 = await ReReview();
                    if (!rs反审)
                    {
                        UNLock();
                        toolStripBtnReverseReview.Enabled = true;
                    }

                    break;
                case MenuItemEnums.结案:
                    if (IsLock())
                    {
                        return;
                    }
                    await CloseCaseAsync();
                    break;
                case MenuItemEnums.反结案:
                    if (IsLock())
                    {
                        return;
                    }
                    await AntiCloseCaseAsync();
                    break;
                case MenuItemEnums.打印:

                    if (PrintConfig != null && PrintConfig.tb_PrintTemplates != null)
                    {
                        //如果当前单据只有一个模块，就直接打印
                        if (PrintConfig.tb_PrintTemplates.Count == 1)
                        {
                            Print();
                            return;
                        }
                    }

                    //个性化设置了打印要选择模板打印时，就进入设计介面
                    if (MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.HasValue
                           && MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.Value)
                    {
                        PrintDesigned();
                    }
                    else
                    {
                        Print();
                    }

                    toolStripbtnPrint.Enabled = false;
                    break;
                case MenuItemEnums.预览:
                    Preview();
                    break;
                case MenuItemEnums.设计:
                    PrintDesigned();
                    break;
                case MenuItemEnums.导出:
                    break;

                default:
                    break;
            }


        }

        MenuPowerHelper menuPowerHelper = null;
        public async void MenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                if (menuItem.Tag is RelatedQueryParameter parameter)
                {
                    #region mrp生产模块
                    if (parameter.bizType == BizType.需求分析)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_ProductionDemand).Name
                        && m.FormName == nameof(UCProduceRequirement)
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_ProductionDemand>>(typeof(tb_ProductionDemand).Name + "Controller");
                            tb_ProductionDemand entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }

                    }

                    if (parameter.bizType == BizType.生产计划单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_ProductionPlan).Name
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_ProductionPlan>>(typeof(tb_ProductionPlan).Name + "Controller");
                            var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }


                    if (parameter.bizType == BizType.生产领料单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_MaterialRequisition).Name
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_MaterialRequisition>>(typeof(tb_MaterialRequisition).Name + "Controller");
                            var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.生产退料单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_MaterialReturn).Name
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_MaterialReturn>>(typeof(tb_MaterialReturn).Name + "Controller");
                            var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.制令单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_ManufacturingOrder).Name
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_ManufacturingOrder>>(typeof(tb_ManufacturingOrder).Name + "Controller");
                            var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.返工入库单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_MRP_ReworkEntry).Name
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_MRP_ReworkEntry>>(typeof(tb_MRP_ReworkEntry).Name + "Controller");
                            var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                    if (parameter.bizType == BizType.返工退库单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_MRP_ReworkReturn).Name
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_MRP_ReworkReturn>>(typeof(tb_MRP_ReworkReturn).Name + "Controller");
                            var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                    if (parameter.bizType == BizType.缴库单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FinishedGoodsInv).Name
                        && m.BIBaseForm.Contains("BaseBillEditGeneric")
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FinishedGoodsInv>>(typeof(tb_FinishedGoodsInv).Name + "Controller");
                            var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                    #endregion

                    if (parameter.bizType == BizType.对账单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FM_Statement).Name
                        && m.BizType == (int)parameter.bizType
                        && m.BIBaseForm == "BaseBillEditGeneric`2"
                        //&& m.BIBizBaseForm == nameof(UCFMStatement)
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FM_Statement>>(typeof(tb_FM_Statement).Name + "Controller");
                            tb_FM_Statement entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }


                    if (parameter.bizType == BizType.付款单 || parameter.bizType == BizType.收款单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FM_PaymentRecord).Name
                        && m.BIBizBaseForm == nameof(UCPaymentRecord)
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FM_PaymentRecord>>(typeof(tb_FM_PaymentRecord).Name + "Controller");
                            tb_FM_PaymentRecord entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }


                    if (parameter.bizType == BizType.应付款单 || parameter.bizType == BizType.应收款单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FM_ReceivablePayable).Name
                        && m.BIBizBaseForm == nameof(UCReceivablePayable)
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FM_ReceivablePayable>>(typeof(tb_FM_ReceivablePayable).Name + "Controller");
                            tb_FM_ReceivablePayable entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                    if (parameter.bizType == BizType.预付款单 || parameter.bizType == BizType.预收款单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FM_PreReceivedPayment).Name
                        && m.BIBizBaseForm == nameof(UCPreReceivedPayment)
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FM_PreReceivedPayment>>(typeof(tb_FM_PreReceivedPayment).Name + "Controller");
                            tb_FM_PreReceivedPayment entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }


                    if (parameter.bizType == BizType.其他费用收入 || parameter.bizType == BizType.其他费用支出)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FM_OtherExpense).Name
                        && m.BIBizBaseForm == nameof(UCOtherExpense)
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FM_OtherExpense>>(typeof(tb_FM_OtherExpense).Name + "Controller");
                            tb_FM_OtherExpense entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.采购价格调整单 || parameter.bizType == BizType.销售价格调整单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FM_PriceAdjustment).Name
                        && m.BIBizBaseForm == nameof(UCPriceAdjustment)
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FM_PriceAdjustment>>(typeof(tb_FM_PriceAdjustment).Name + "Controller");
                            tb_FM_PriceAdjustment entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                    if (parameter.bizType == BizType.采购订单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_PurOrder).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_PurOrder>>(typeof(tb_PurOrder).Name + "Controller");
                            tb_PurOrder entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                    if (parameter.bizType == BizType.采购入库单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_PurEntry).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_PurEntry>>(typeof(tb_PurEntry).Name + "Controller");
                            tb_PurEntry entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.采购退货单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_PurEntryRe).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_PurEntryRe>>(typeof(tb_PurEntryRe).Name + "Controller");
                            tb_PurEntryRe entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.归还单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_ProdReturning).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_ProdReturning>>(typeof(tb_ProdReturning).Name + "Controller");
                            tb_ProdReturning entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.销售订单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_SaleOrder).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_SaleOrder>>(typeof(tb_SaleOrder).Name + "Controller");
                            tb_SaleOrder entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.销售出库单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_SaleOut).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_SaleOut>>(typeof(tb_SaleOut).Name + "Controller");
                            tb_SaleOut entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.销售退回单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_SaleOutRe).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_SaleOutRe>>(typeof(tb_SaleOutRe).Name + "Controller");
                            tb_SaleOutRe entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                    if (parameter.bizType == BizType.费用报销单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_FM_ExpenseClaim).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_FM_ExpenseClaim>>(typeof(tb_FM_ExpenseClaim).Name + "Controller");
                            tb_FM_ExpenseClaim entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }


                    if (parameter.bizType == BizType.售后申请单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_AS_AfterSaleApply).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_AS_AfterSaleApply>>(typeof(tb_AS_AfterSaleApply).Name + "Controller");
                            tb_AS_AfterSaleApply entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.售后交付单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_AS_AfterSaleDelivery).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_AS_AfterSaleDelivery>>(typeof(tb_AS_AfterSaleDelivery).Name + "Controller");
                            tb_AS_AfterSaleDelivery entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.维修工单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_AS_RepairOrder).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_AS_RepairOrder>>(typeof(tb_AS_RepairOrder).Name + "Controller");
                            tb_AS_RepairOrder entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }
                    if (parameter.bizType == BizType.维修入库单)
                    {
                        var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == typeof(tb_AS_RepairInStock).Name
                        && m.MenuName.Contains(parameter.bizType.ToString())
                        ).FirstOrDefault();
                        if (RelatedBillMenuInfo != null)
                        {
                            var controller = Startup.GetFromFacByName<BaseController<tb_AS_RepairInStock>>(typeof(tb_AS_RepairInStock).Name + "Controller");
                            tb_AS_RepairInStock entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                            //要把单据信息传过去
                            menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                        }
                    }

                }
            }

        }

        /// <summary>
        ///UI上有显示锁定时不重复锁。后面要优化
        /// </summary>
        public override void LockBill()
        {
            if (EditEntity == null)
            {
                return;
            }
            //如果要锁这个单 看这个单是不是已经被其它人锁，如果没有人锁则我可以锁
            long userid = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

            // 锁这个单这前要把这个类型的前面的单全部释放出来。一个业务一个人只能锁一个单号
            UNLockByBizName(userid);

            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            if (pkid > 0)
            {

                var lockeduserid = MainForm.Instance.lockManager.GetLockedBy(pkid);
                if (lockeduserid != userid)
                {
                    if (lockeduserid > 0)
                    {
                        //被人锁了
                        this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.Lockbill;
                    }
                    else
                    {
                        LockBill(pkid, userid);
                    }
                }

                /*
                #region 锁定当前单据  后面流程上也要能锁定


                #endregion
                */
            }
        }




        private void LockBill(long BillID, long userid)
        {
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommBillData cbd = bcf.GetBillData(typeof(T), EditEntity);

            LockedInfo lockRequest = new LockedInfo
            {
                BillID = BillID,
                BillData = cbd,
                LockedUserID = userid,
                LockedUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name,
                MenuID = CurMenuInfo.MenuID
            };

            using (ClientLockManagerCmd cmd = new ClientLockManagerCmd(CmdOperation.Send))
            {
                lockRequest.PacketId = cmd.PacketId;
                cmd.lockCmd = LockCmd.LOCK;
                cmd.RequestInfo = lockRequest;
                // 注册锁定单据的事件处理程序
                ClientEventManager.Instance.AddCommandHandler(cmd.PacketId, cmd.HandleLockEvent);
                MainForm.Instance.dispatcher.DispatchAsync(cmd, CancellationToken.None);
                // 注册一次性事件（确保匿名委托不会重复）
                cmd.LockChanged += (sender, e) =>
                {
                    if (e.requestBaseInfo.BillID != BillID)
                    {
                        return;
                    }

                    if (e.isSuccess)
                    {
                        this.tsBtnLocked.Visible = true;
                        this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.unlockbill;
                    }

                };


            }
        }

        private bool IsLock()
        {
            //如果已经有锁定标记了。即使已经释放了锁。也要刷新数据后才可以保存。不然数据不会统一。
            if (tsBtnLocked.Tag != null && tsBtnLocked.Tag is LockInfo lockInfo)
            {

                if (lockInfo.IsLocked && lockInfo.LockedByID != MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID)
                {
                    var LockInfoCheck = MainForm.Instance.lockManager.GetLockStatus(lockInfo.BillID);
                    if (LockInfoCheck == null)
                    {
                        return false;
                    }
                    //别人锁定了
                    string tipMsg = $"单据已被用户{lockInfo.LockedByName}锁定，请刷新后再试,或【点击已锁定】联系锁定人员解锁。";
                    MainForm.Instance.uclog.AddLog(tipMsg);
                    tsBtnLocked.AutoToolTip = true;
                    tsBtnLocked.ToolTipText = tipMsg;
                    tsBtnLocked.Visible = true;
                    tsBtnLocked.Tag = lockInfo;
                    this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.Lockbill;
                    //显示锁定认为锁定。要刷新数据
                    MessageBox.Show($"单据已被{lockInfo.LockedByName}锁定，请刷新后重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

            }

            bool isLocked = false;
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = 0;
            #region 锁定当前单据  后面流程上也要能锁定
            if (EditEntity != null)
            {
                pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
                if (pkid > 0)
                {
                    var lockuserid = MainForm.Instance.lockManager.GetLockedBy(pkid);
                    if (lockuserid > 0)
                    {
                        if (lockuserid != MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID)
                        {
                            //锁定提示
                            MessageBox.Show($"单据已被用户{lockuserid}锁定，请稍后再试,或联系他解锁。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isLocked = true;
                        }
                    }

                }
            }
            #endregion
            return isLocked;
        }

        //泛型名称有一个尾巴，这里处理掉，但是总体要保持不能同时拥有同名的 泛型 和非泛型控制类
        //否则就是调用解析时用加小尾巴
        //注册时处理了所以用上面不加小尾巴
        //BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller`1");

        /// <summary>
        /// 结案处理
        /// 一般会自动结案，但是有些需要人工结案
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            frm.ShowCloseCaseImage = ReflectionHelper.ExistPropertyName<T>("CloseCaseImagePath");
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<T>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.CloseCaseOpinions = "完成结案";
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<T> needCloseCases = new List<T>();
                if (ReflectionHelper.ExistPropertyName<T>("CloseCaseOpinions"))
                {
                    EditEntity.SetPropertyValue("CloseCaseOpinions", frm.txtOpinion.Text);
                }
                //已经审核的并且通过的情况才能结案
                if (ReflectionHelper.ExistPropertyName<T>("DataStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                {
                    // 确认状态下 已经审核并且通过
                    if (EditEntity.GetPropertyValue("DataStatus").ToInt() == (int)DataStatus.确认
                        && EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                        && EditEntity.GetPropertyValue("ApprovalResults") != null
                        && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                        )
                    {
                        needCloseCases.Add(EditEntity);
                    }
                }

                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
                if (rs.Succeeded)
                {
                    if (frm.CloseCaseImage != null && ReflectionHelper.ExistPropertyName<T>("CloseCaseImagePath"))
                    {
                        string strCloseCaseImagePath = System.DateTime.Now.ToString("yy") + "/" + System.DateTime.Now.ToString("MM") + "/" + Ulid.NewUlid().ToString();
                        byte[] bytes = UI.Common.ImageHelper.imageToByteArray(frm.CloseCaseImage);
                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        ////上传新文件时要加后缀名
                        string uploadRsult = await httpWebService.UploadImageAsyncOK("", strCloseCaseImagePath + ".jpg", bytes, "upload");
                        if (uploadRsult.Contains("UploadSuccessful"))
                        {
                            EditEntity.SetPropertyValue("CloseCaseImagePath", strCloseCaseImagePath);
                            //这里更新数据库
                            await ctr.BaseSaveOrUpdate(EditEntity);
                        }
                        else
                        {
                            MainForm.Instance.LoginWebServer();
                        }
                    }

                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);
                    MainForm.Instance.auditLogHelper.CreateAuditLog<T>("结案", EditEntity, $"结案意见:{ae.CloseCaseOpinions}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{ae.BillNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private Business.BizMapperService.IEntityInfoService _entityInfoService;


        /// <summary>
        /// 审核 注意后面还需要加很多业务逻辑。
        /// 比方出库单，审核就会减少库存修改成本
        /// （如果有月结动作，则在月结时统计修改成本，更科学，因为如果退单等会影响成本）b
        /// </summary>
        protected async override Task<ReviewResult> Review()
        {
            ReviewResult reviewResult = new ReviewResult();
            if (EditEntity == null)
            {
                return reviewResult;
            }

            if (EditEntity is BaseEntity baseEntity)
            {
                if (baseEntity.HasChanged == true && baseEntity.GetEffectiveChanges().Count > 0)
                {
                    MessageBox.Show("数据已经被修改，不能再次审核。请【保存】或【刷新】后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return reviewResult;
                }
            }

            // 需要恢复的字段列表
            string[] fieldsToRestore = new string[]
            {
                nameof(PaymentStatus),
                nameof(ARAPStatus),
                nameof(PrePaymentStatus),
                nameof(ApprovalStatus),
                nameof(DataStatus),
                "ApprovalOpinions",
                "ApprovalResults"
            };


            ApprovalEntity ae = new ApprovalEntity();
            if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
            {
                //审核过，并且通过了，不能再次审核
                if ((EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核)
                    && EditEntity.GetPropertyValue("ApprovalResults") != null
                    && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                    )
                {
                    MainForm.Instance.uclog.AddLog("【未审核】或【驳回】的单据才能再次审核。");
                    return reviewResult;
                }
            }
            //如果已经审核并且审核通过，则不能再次审核



            //BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            //CommBillData cbd = bcf.GetBillData<T>(EditEntity);
            CommonUI.frmApproval frm = new CommonUI.frmApproval();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ae.BillID = pkid;

            var statusType = FMPaymentStatusHelper.GetStatusType(EditEntity as BaseEntity);
            if (statusType == typeof(DataStatus))
            {
                Business.BizMapperService.ERPEntityInfo entityInfo = _entityInfoService.GetEntityInfo<T>();
                if (entityInfo != null)
                {
                    ae.BillNo = EditEntity.GetPropertyValue(entityInfo.NoField).ToString();
                    ae.bizType = entityInfo.BizType;
                    ae.bizName = entityInfo.BizType.ToString();
                }
            }
            else
            {
                int flag = (int)ReflectionHelper.GetPropertyValue(EditEntity, nameof(ReceivePaymentType)); ;
                Business.BizMapperService.ERPEntityInfo entityInfo = _entityInfoService.GetEntityInfo<T>(flag);
                if (entityInfo != null)
                {
                    ae.BillNo = EditEntity.GetPropertyValue(entityInfo.NoField).ToString();
                    ae.bizType = entityInfo.BizType;
                    ae.bizName = entityInfo.BizType.ToString();
                }
            }


            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            await Task.Delay(1);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                RevertCommand command = new RevertCommand();
                //缓存当前编辑的对象。如果撤销就回原来的值
                //审核只是修改的审核状态。不用缓存全部
                T oldobj = CloneHelper.DeepCloneObject_maxnew<T>(EditEntity);

                // 克隆指定字段的值
                var originalFieldValues = CloneSpecificFields(EditEntity, fieldsToRestore);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                    //CloneHelper.SetValues<T>(EditEntity, oldobj);
                    // 只恢复指定的字段
                    RestoreSpecificFields(EditEntity, originalFieldValues);
                };

                if (ae.ApprovalResults == true)
                {
                    //审核了。数据状态要更新为
                    EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.确认);
                }
                else
                {
                    //审核了。驳回 时数据状态要更新为新建。要再次修改后提交
                    #region UI驳回直接保存返回。不用进入审核流程了。
                    EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.新建);
                    if (ReflectionHelper.ExistPropertyName<T>("ApprovalOpinions"))
                    {
                        EditEntity.SetPropertyValue("ApprovalOpinions", ae.ApprovalOpinions);
                    }
                    if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus"))
                    {
                        EditEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.驳回);
                    }
                    if (ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                    {
                        EditEntity.SetPropertyValue("ApprovalResults", false);
                    }
                    BusinessHelper.Instance.ApproverEntity(EditEntity);
                    BaseController<T> ctrBase = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                    //因为只需要更新主表
                    ReturnResults<T> rr = await ctrBase.BaseSaveOrUpdate(EditEntity);
                    reviewResult.approval = ae;
                    reviewResult.Succeeded = rr.Succeeded;
                    return reviewResult;
                    #endregion

                }


                //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
                PropertyInfo[] array_property = ae.GetType().GetProperties();
                {
                    foreach (var property in array_property)
                    {
                        //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                        if (ReflectionHelper.ExistPropertyName<T>(property.Name))
                        {
                            object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                            ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                        }
                    }
                }
                //审核通过赋值
                if (ReflectionHelper.ExistPropertyName<T>("ApprovalOpinions"))
                {
                    EditEntity.SetPropertyValue("ApprovalOpinions", ae.ApprovalOpinions);
                }
                if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus"))
                {
                    EditEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.已审核);
                }
                if (ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                {
                    EditEntity.SetPropertyValue("ApprovalResults", true);
                }

                ReturnResults<T> rmr = new ReturnResults<T>();
                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                // AdjustingInventoryAsync
                //因为只需要更新主表
                rmr = await ctr.ApprovalAsync(EditEntity);
                if (rmr.Succeeded)
                {
                    reviewResult.Succeeded = rmr.Succeeded;

                    //如果是出库单审核，则上传到服务器 锁定订单无法修改
                    if (ae.bizType == BizType.销售出库单)
                    {
                        //锁定对应的订单
                        if (EditEntity is tb_SaleOut saleOut)
                        {
                            if (saleOut.tb_saleorder != null)
                            {
                                LockBill(saleOut.tb_saleorder.SOrder_ID, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);

                                //OriginalData od = ActionForClient.单据锁定(saleOut.tb_saleorder.SOrder_ID,
                                //    MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID,
                                //    MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name,
                                //    (int)BizType.销售订单, 11);
                                //MainForm.Instance.ecs.AddSendData(od);
                            }
                        }

                        #region 销售出库单如果启用了财务模块，则会生成应收款单

                        AuthorizeController authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                        if (authorizeController.EnableFinancialModule())
                        {
                            if (MainForm.Instance.AppContext.FMConfig.AutoAuditReceiveable)
                            {
                                #region 自动审核应收款单
                                //销售订单审核时自动将预付款单设为"已生效"状态
                                var ctrpayable = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                                if (rmr.ReturnObjectAsOtherEntity is tb_FM_ReceivablePayable payable)
                                {
                                    payable.ApprovalOpinions = "系统自动审核";
                                    payable.ApprovalStatus = (int)ApprovalStatus.已审核;
                                    payable.ApprovalResults = true;
                                    ReturnResults<tb_FM_ReceivablePayable> autoApproval = await ctrpayable.ApprovalAsync(payable, true);
                                    if (!autoApproval.Succeeded)
                                    {
                                        autoApproval.Succeeded = false;
                                        autoApproval.ErrorMsg = $"应收款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                    }
                                    else
                                    {
                                        MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("应收款单自动审核成功", autoApproval.ReturnObject as tb_FM_ReceivablePayable);
                                    }
                                }
                                #endregion
                            }
                        }

                        #endregion
                    }

                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);

                    #region 销售退货

                    //如果是出库单审核，则上传到服务器 锁定订单无法修改
                    if (ae.bizType == BizType.销售退回单)
                    {
                        #region 销售退回单 如果启用了财务模块

                        AuthorizeController authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                        if (authorizeController.EnableFinancialModule())
                        {
                            if (MainForm.Instance.AppContext.FMConfig.AutoAuditReceiveable)
                            {
                                #region 自动审核应收款单
                                //销售订单审核时自动将预付款单设为"已生效"状态
                                var ctrpayable = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                                if (rmr.ReturnObjectAsOtherEntity is tb_FM_ReceivablePayable payable)
                                {
                                    //只有有应收记录。挂账都可以手工进行后面的步骤。
                                    //所以这里应收审核过了。说明平台退款了，认为销售退回单中的平台退款已经处理了。

                                    if ((payable.ApprovalStatus.GetValueOrDefault() == (int)ApprovalStatus.未审核) || !payable.ApprovalResults.GetValueOrDefault())
                                    {
                                        payable.ApprovalOpinions = "【销售退回单】审核时，系统自动审核";
                                        payable.ApprovalStatus = (int)ApprovalStatus.已审核;
                                        payable.ApprovalResults = true;
                                        ReturnResults<tb_FM_ReceivablePayable> autoApproval = await ctrpayable.ApprovalAsync(payable, true);
                                        if (!autoApproval.Succeeded)
                                        {
                                            autoApproval.Succeeded = false;
                                            autoApproval.ErrorMsg = $"【销售退回单】审核时,应收款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                        }
                                        else
                                        {
                                            MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("【销售退回单】审核时，应收款单自动审核成功", autoApproval.ReturnObject as tb_FM_ReceivablePayable);
                                        }

                                        //自动退款？
                                        //平台订单 经过运费在 平台退款操作后，退回单状态中已经是 退款状态了。
                                        if (MainForm.Instance.AppContext.FMConfig.AutoAuditReceivePaymentRecordByPlatform)
                                        {
                                            if (rmr.ReturnObject is tb_SaleOutRe saleOutRe)
                                            {
                                                if (saleOutRe.IsFromPlatform)
                                                {

                                                    #region 生成应收款红字单，退款
                                                    //自动生成销售退回单的对应的应该收款单（红字的）对应的收款记录
                                                    var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                                    List<tb_FM_ReceivablePayable> receivablePayables = new List<tb_FM_ReceivablePayable>();
                                                    receivablePayables.Add(payable);

                                                    tb_FM_PaymentRecord newPaymentRecord = paymentController.BuildPaymentRecord(receivablePayables);
                                                    newPaymentRecord.Remark = "平台单，已退款，货回仓审核时自动生成的收款单（负数）红字";
                                                    newPaymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
                                                    if (!newPaymentRecord.Paytype_ID.HasValue && saleOutRe.Paytype_ID.HasValue)
                                                    {
                                                        newPaymentRecord.Paytype_ID = saleOutRe.Paytype_ID;
                                                    }
                                                    var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(newPaymentRecord, false);
                                                    if (rrs.Succeeded)
                                                    {
                                                        if (saleOutRe.RefundStatus == (int)RefundStatus.已退款已退货)
                                                        {
                                                            //自动审核收款单
                                                            newPaymentRecord.ApprovalOpinions = "【平台单】已退款，销售退回单审核时，自动审核";
                                                            newPaymentRecord.ApprovalStatus = (int)ApprovalStatus.已审核;
                                                            newPaymentRecord.ApprovalResults = true;

                                                            ReturnResults<tb_FM_PaymentRecord> rrRecord = await paymentController.ApprovalAsync(newPaymentRecord);
                                                            if (!rrRecord.Succeeded)
                                                            {
                                                                MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_PaymentRecord>("【平台单】销售退回时，自动审核失败：" + rrRecord.ErrorMsg, rrRecord.ReturnObject as tb_FM_PaymentRecord);
                                                            }
                                                            else
                                                            {

                                                                MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_PaymentRecord>("【平台单】销售退回时，自动审核成功", rrRecord.ReturnObject as tb_FM_PaymentRecord);
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                }
                                            }
                                        }


                                    }



                                }
                                #endregion
                            }
                        }

                        #endregion
                    }

                    #endregion

                    //如果是出库单审核，则上传到服务器 锁定订单无法修改
                    if (ae.bizType == BizType.采购入库单 || ae.bizType == BizType.采购退货入库)
                    {
                        //锁定对应的订单
                        if (EditEntity is tb_PurEntry PurEntry)
                        {
                            if (PurEntry.tb_purorder != null)
                            {
                                LockBill(PurEntry.tb_purorder.PurOrder_ID, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);

                                //OriginalData od = ActionForClient.单据锁定(saleOut.tb_saleorder.SOrder_ID,
                                //    MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID,
                                //    MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name,
                                //    (int)BizType.销售订单, 11);
                                //MainForm.Instance.ecs.AddSendData(od);
                            }
                        }
                        if (EditEntity is tb_PurReturnEntry PurReturnEntry)
                        {
                            if (PurReturnEntry.tb_purentryre != null)
                            {
                                LockBill(PurReturnEntry.tb_purentryre.PurEntryRe_ID, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                            }
                        }

                        #region 采购入库单如果启用了财务模块，则会生成应付款单

                        AuthorizeController authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                        if (authorizeController.EnableFinancialModule())
                        {
                            if (MainForm.Instance.AppContext.FMConfig.AutoAuditPaymentable)
                            {
                                #region 自动审核应付款单
                                //销售订单审核时自动将预付款单设为"已生效"状态
                                var ctrpayable = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                                if (rmr.ReturnObjectAsOtherEntity is tb_FM_ReceivablePayable payable)
                                {
                                    if (payable.ARAPStatus == (int)ARAPStatus.待审核)
                                    {
                                        payable.ApprovalOpinions = "系统自动审核";
                                        payable.ApprovalStatus = (int)ApprovalStatus.已审核;
                                        payable.ApprovalResults = true;
                                        //if (PurEntry.tb_purorder != null && !payable.PayeeInfoID.HasValue)
                                        //{
                                        //    //通过订单添加付款信息
                                        //    payable.PayeeInfoID = PurEntry.tb_purorder.PayeeInfoID;
                                        //}

                                        ReturnResults<tb_FM_ReceivablePayable> autoApproval = await ctrpayable.ApprovalAsync(payable, true);
                                        if (!autoApproval.Succeeded)
                                        {
                                            autoApproval.Succeeded = false;
                                            autoApproval.ErrorMsg = $"应付款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                            if (MainForm.Instance.AppContext.SysConfig.ShowDebugInfo)
                                            {
                                                MainForm.Instance.logger.LogInformation(autoApproval.ErrorMsg);
                                            }
                                            await MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("应付款单自动审核失败", autoApproval.ReturnObject as tb_FM_ReceivablePayable, autoApproval.ErrorMsg);
                                        }
                                        else
                                        {
                                            MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("应付款单自动审核成功", autoApproval.ReturnObject as tb_FM_ReceivablePayable);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        #endregion


                    }


                    //审核成功
                    ToolBarEnabledControl(MenuItemEnums.审核);
                    //如果审核结果为不通过时，审核不是灰色。
                    if (!ae.ApprovalResults)
                    {
                        toolStripbtnReview.Enabled = true;
                    }
                    ToolBarEnabledControl(EditEntity);
                    ae.ApprovalResults = true;
                    reviewResult.approval = ae;
                    if (ae.bizType.ToString().Contains("款"))
                    {
                        MainForm.Instance.FMAuditLogHelper.CreateAuditLog<T>("审核", EditEntity, $"审核结果：{(ae.ApprovalResults ? "通过" : "拒绝")}-{ae.ApprovalOpinions}");
                    }
                    else
                    {
                        MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", EditEntity, $"审核结果：{(ae.ApprovalResults ? "通过" : "拒绝")}-{ae.ApprovalOpinions}");
                    }



                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核成功。", Color.Red);
                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    ae.ApprovalResults = false;
                    ae.ApprovalStatus = (int)ApprovalStatus.未审核;
                    ToolBarEnabledControl(EditEntity);
                    reviewResult.approval = ae;
                    // 记录审计日志
                    MainForm.Instance.AuditLogHelper.CreateAuditLog("审核失败", EditEntity, $"审核结果:{(ae.ApprovalResults ? "通过" : "拒绝")},{rmr.ErrorMsg}");
                    MainForm.Instance.logger.LogError($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg}");
                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg},请联系管理员！", Color.Red);
                    MessageBox.Show($"{ae.bizName}:{ae.BillNo}审核失败。\r\n {rmr.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                toolStripbtnReview.Enabled = false;
            }
            else
            {
                toolStripbtnReview.Enabled = true;
            }
            return reviewResult;
        }

        /// <summary>
        /// 反审核 与审核相反
        /// </summary>
        protected async override Task<bool> ReReview()
        {
            bool rs = false;
            ApprovalEntity ae = new ApprovalEntity();
            if (EditEntity == null)
            {
                return rs;
            }
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            if (pkid > 0)
            {
                //判断是否锁定
                if (IsLock())
                {
                    MainForm.Instance.uclog.AddLog($"单据已被锁定，请刷新后再试");
                    return rs;
                    //分读写锁  保存后就只有读。释放 写？
                }
            }

            if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
            {
                //反审，要审核过，并且通过了，才能反审。
                if (EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                    && EditEntity.GetPropertyValue("ApprovalResults") != null
                    && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                    )
                {
                    ae.ApprovalResults = true;
                }
                else
                {
                    MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审。");
                    return rs;
                }
            }

            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmReApproval frm = new CommonUI.frmReApproval();


            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<T>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                RevertCommand command = new RevertCommand();
                //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject_maxnew<T>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码 意思是如果取消反审，内存中反审核的数据要变为空白（之前的样子）
                    CloneHelper.SetValues<T>(EditEntity, oldobj);
                };
                //保存旧值要在下面更新值前

                //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
                PropertyInfo[] array_property = ae.GetType().GetProperties();
                {
                    foreach (var property in array_property)
                    {
                        //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                        //Expression<Func<ApprovalEntity, object>> PNameExp = t => t.ApprovalStatus;
                        //MemberInfo minfo = PNameExp.GetMemberInfo();
                        //string propertyName = minfo.Name;

                        //反审核。驳回 时数据状态要更新为新建。要再次修改后提交
                        EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.新建);
                        if (ReflectionHelper.ExistPropertyName<T>("ApprovalOpinions"))
                        {
                            EditEntity.SetPropertyValue("ApprovalOpinions", ae.ApprovalOpinions);
                        }
                        if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus"))
                        {
                            EditEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.未审核);
                        }
                        if (ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                        {
                            EditEntity.SetPropertyValue("ApprovalResults", false);
                        }
                        BusinessHelper.Instance.ApproverEntity(EditEntity);


                        if (ReflectionHelper.ExistPropertyName<T>(property.Name))
                        {
                            object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                            ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                        }
                    }
                }



                ReturnResults<T> rmr = new ReturnResults<T>();
                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");

                //反审前 刷新最新数据才能判断 比方销售订单 没有关掉当前UI时。已经出库。再反审。后面再优化为缓存处理锁单来不用查数据库刷新。
                BaseEntity pkentity = (editEntity as T) as BaseEntity;
                EditEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;

                rmr = await ctr.AntiApprovalAsync(EditEntity);
                if (rmr.Succeeded)
                {
                    rs = true;
                    //如果是出库单审核，则上传到服务器 锁定订单无法修改
                    if (ae.bizType == BizType.销售出库单)
                    {
                        //锁定对应的订单
                        if (EditEntity is tb_SaleOut saleOut)
                        {
                            if (saleOut.tb_saleorder != null)
                            {
                                UNLock(saleOut.tb_saleorder.SOrder_ID, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                                //OriginalData od = ActionForClient.单据锁定释放(saleOut.tb_saleorder.SOrder_ID,
                                //    MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID,
                                //    MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name,
                                //    (int)BizType.销售订单, CurMenuInfo.MenuID);
                                //MainForm.Instance.ecs.AddSendData(od);
                            }
                        }

                    }

                    ToolBarEnabledControl(MenuItemEnums.反审);
                    //这里推送到审核，启动工作流
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("反审", EditEntity, $"反审原因{ae.ApprovalOpinions}");
                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    rs = false;
                    ToolBarEnabledControl(EditEntity);
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("反审失败", EditEntity, $"反审原因{ae.ApprovalOpinions},{rmr.ErrorMsg}");
                    MainForm.Instance.logger.LogError($"{cbd.BillNo}反审失败{rmr.ErrorMsg}");
                    MainForm.Instance.PrintInfoLog($"{cbd.BillNo}反审失败{rmr.ErrorMsg},请联系管理员！", Color.Red);
                    MessageBox.Show($"{ae.bizName}:{ae.BillNo}反审失败。\r\n {rmr.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return rs;
        }


        #region
        // 添加辅助方法
        private Dictionary<string, object> CloneSpecificFields(T entity, params string[] fieldNames)
        {
            var fieldValues = new Dictionary<string, object>();
            foreach (var fieldName in fieldNames)
            {
                if (ReflectionHelper.ExistPropertyName<T>(fieldName))
                {
                    fieldValues[fieldName] = ReflectionHelper.GetPropertyValue(entity, fieldName);
                }
            }
            return fieldValues;
        }

        private void RestoreSpecificFields(T entity, Dictionary<string, object> fieldValues)
        {
            foreach (var kvp in fieldValues)
            {
                if (ReflectionHelper.ExistPropertyName<T>(kvp.Key))
                {
                    ReflectionHelper.SetPropertyValue(entity, kvp.Key, kvp.Value);
                }
            }
        }
        #endregion

        private T editEntity;
        public T EditEntity { get => editEntity; set => editEntity = value; }

        public List<T> PrintData { get; set; }

        /// <summary>
        /// 取消添加 取消修改
        /// </summary>
        protected virtual void Cancel()
        {
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();
                //OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            }

            ToolBarEnabledControl(MenuItemEnums.取消);
            bindingSourceSub.CancelEdit();

        }

        frmFormProperty frm = null;
        protected override void Property()
        {
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //保存属性
                ToolBarEnabledControl(MenuItemEnums.属性);
                //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("属性", EditEntity);
            }
            base.Property();
        }

        /*
        /// <summary>
        /// 删除远程的图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<bool> DeleteRemoteImages()
        {
            if (EditEntity == null || EditEntity.GetPropertyValue(typeof(T) + "Details") == null)
            {
                return false;
            }
            bool result = true;
            List<C> details = EditEntity.GetPropertyValue(typeof(T) + "Details") as List<C>;
            if (details == null)
            {
                return false;
            }
            foreach (C detail in details)
            {
                PropertyInfo[] props = typeof(C).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];//子类提上来用？
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            if (detail.GetPropertyValue(prop.Name) != null
                                && detail.GetPropertyValue(prop.Name).ToString().Contains("-"))
                            {
                                string imageNameValue = detail.GetPropertyValue(prop.Name).ToString();
                                //比较是否更新了图片数据
                                //old_new 无后缀文件名
                                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = new SourceGrid.Cells.Models.ValueImageWeb();
                                valueImageWeb.CellImageHashName = imageNameValue;
                                string oldfileName = valueImageWeb.GetOldRealfileName();
                                string newfileName = valueImageWeb.GetNewRealfileName();
                                string TempFileName = string.Empty;
                                //fileName = System.IO.Path.Combine(Application.StartupPath + @"\temp\", fileName);
                                //保存在本地临时目录 删除
                                if (System.IO.File.Exists(TempFileName))
                                {
                                    System.IO.File.Delete(TempFileName);
                                }
                                //上传到服务器，删除本地
                                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                string deleteRsult = await httpWebService.DeleteImageAsync(newfileName, "delete123");
                                MainForm.Instance.PrintInfoLog(deleteRsult);
                            }
                        }
                    }

                }
            }
            return result;
        }
        */


        /// <summary>
        /// 保存图片到服务器。所有图片都保存到服务器。即使草稿换电脑还可以看到
        /// </summary>
        /// <param name="RemoteSave"></param>
        /// <returns></returns>
        public async Task<bool> SaveFileToServer(SourceGridDefine sgd, List<C> Details)
        {
            bool result = true;
            List<SGDefineColumnItem> ImgCols = new List<SGDefineColumnItem>();
            foreach (C detail in Details)
            {
                PropertyInfo[] props = typeof(C).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        if (col.CustomFormat == CustomFormatType.WebPathImage && !ImgCols.Contains(col))
                        {
                            ImgCols.Add(col);
                        }
                    }
                }
            }
            try
            {
                result = await UploadImageAsync(ImgCols, sgd.grid, Details);
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                MainForm.Instance.LoginWebServer();
            }
            return result;
        }

        private async Task<bool> UploadImageAsync(List<SGDefineColumnItem> ImgCols, Grid grid, List<C> Details)
        {
            bool rs = true;
            //保存图片到本地临时目录，图片数据保存在grid1控件中，所以要循环控件的行，控件真实数据行以1为起始
            int totalRowsFlag = grid.RowsCount;
            if (grid.HasSummary)
            {
                totalRowsFlag--;//减去一行总计行
            }
            for (int i = 1; i < totalRowsFlag; i++)
            {
                foreach (var col in ImgCols)
                {
                    int realIndex = grid.Columns.GetColumnInfo(col.UniqueId).Index;
                    if (grid[i, realIndex].Value == null)
                    {
                        continue;
                    }
                    var model = grid[i, realIndex].Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
                    //比较是否更新了图片数据
                    string newhash = valueImageWeb.GetImageNewHash();
                    if (valueImageWeb.CellImageBytes != null)
                    {
                        #region 需要上传

                        if (!valueImageWeb.GetImageoldHash().Equals(newhash, StringComparison.OrdinalIgnoreCase)
                        && grid[i, realIndex].Value.ToString() == valueImageWeb.CellImageHashName)
                        {
                            string oldfileName = valueImageWeb.GetOldRealfileName();
                            string newfileName = valueImageWeb.GetNewRealfileName();
                            HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                            //如果服务器有旧文件 。可以先删除
                            if (!string.IsNullOrEmpty(valueImageWeb.GetImageoldHash()))
                            {
                                string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                                MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                            }
                            ////上传新文件时要加后缀名
                            string uploadRsult = await httpWebService.UploadImageAsync(CurMenuInfo.MenuID.ToString(), (EditEntity as BaseEntity).PrimaryKeyID.ToString(), newfileName + ".jpg", valueImageWeb.CellImageBytes, "upload");
                            if (uploadRsult.Contains("UploadSuccessful") || uploadRsult.Contains("ImageExists"))
                            {
                                // 提取文件名（无论是新上传还是已存在）
                                string resultFileName = uploadRsult.Contains("UploadSuccessful") ?
                                    uploadRsult.Replace("UploadSuccessful: ", "").Trim() :
                                    uploadRsult.Replace("ImageExists: ", "").Trim();

                                valueImageWeb.UpdateImageName(newhash);
                                grid[i, realIndex].Value = resultFileName;

                                string detailPKName = UIHelper.GetPrimaryKeyColName(typeof(C));
                                object PKValue = grid[i, realIndex].Row.RowData.GetPropertyValue(detailPKName);
                                var detail = Details.Where(x => x.GetPropertyValue(detailPKName).ToString().Equals(PKValue.ToString())).FirstOrDefault();
                                detail.SetPropertyValue(col.ColName, resultFileName);
                                rs = true;

                                if (uploadRsult.Contains("UploadSuccessful"))
                                {
                                    MainForm.Instance.PrintInfoLog("UploadSuccessful:" + resultFileName);
                                }
                                else
                                {
                                    MainForm.Instance.PrintInfoLog("ImageExists - 使用现有图片:" + resultFileName);
                                }
                            }
                            else
                            {
                                MainForm.Instance.LoginWebServer();
                                rs = false;
                            }
                        }
                        #endregion
                    }
                }
            }
            return rs;
        }

        protected override void Add()
        {

            List<T> list = new List<T>();
            EditEntity = Activator.CreateInstance(typeof(T)) as T;

            //StatusMachine.Create();
            try
            {
                //将预设值写入到新增的实体中
                if (MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations == null)
                {
                    MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations = new List<tb_UIMenuPersonalization>();
                }
                tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
                if (menuSetting != null && menuSetting.tb_UIInputDataFields != null)
                {
                    List<QueryField> fields = new List<QueryField>();
                    UIBizSrvice.GetInputDataField(typeof(T), fields);
                    foreach (var item in menuSetting.tb_UIInputDataFields)
                    {
                        if (item.EnableDefault1.HasValue && item.EnableDefault1.Value)
                        {
                            // 进行类型转换 后设置为默认值
                            var queryField = fields.FirstOrDefault(c => c.FieldName == item.FieldName);
                            if (queryField != null && item.Default1 != null)
                            {
                                object convertedValue = Convert.ChangeType(item.Default1, queryField.ColDataType);
                                EditEntity.SetPropertyValue(item.FieldName, convertedValue);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {


            }

            BusinessHelper.Instance.InitEntity(EditEntity);
            BusinessHelper.Instance.InitStatusEntity(EditEntity);
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();
                OnBindDataToUIEvent(EditEntity, ActionStatus.新增);
            }

            if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
            {
                ReflectionHelper.SetPropertyValue(EditEntity, typeof(ActionStatus).Name, (int)ActionStatus.新增);
            }

            ToolBarEnabledControl(MenuItemEnums.新增);
            //bindingSourceEdit.CancelEdit();
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

        protected override void Modify()
        {
            if (editEntity == null)
            {
                return;
            }
            // 获取状态类型和值
            var statusType = FMPaymentStatusHelper.GetStatusType(editEntity as BaseEntity);
            if (statusType != null)
            {
                // 动态获取状态值
                dynamic status = editEntity.GetPropertyValue(statusType.Name);
                int statusValue = (int)status;
                dynamic statusEnum = Enum.ToObject(statusType, statusValue);

                if (!FMPaymentStatusHelper.CanModify(statusEnum))
                {
                    toolStripbtnModify.Enabled = false;
                    toolStripButtonSave.Enabled = false;
                    MainForm.Instance.PrintInfoLog($"当前单据状态为{statusEnum}不允许修改!");
                    return;
                }

                var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {
                    base.Modify();
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("修改", EditEntity);
                }
                else
                {
                    toolStripbtnModify.Enabled = false;
                }
            }

            BusinessHelper.Instance.EditEntity(EditEntity);
            EditEntity.SetPropertyValue(typeof(ActionStatus).Name, ActionStatus.修改);
            ToolBarEnabledControl(MenuItemEnums.修改);
        }

        protected async override void SpecialDataFix()
        {
            if (EditEntity == null)
            {
                return;
            }
            //没有经验通过下面先不计算
            if (!Validator(EditEntity))
            {
                return;
            }

            List<C> details = new List<C>();
            bindingSourceSub.EndEdit();
            string detailPKName = UIHelper.GetPrimaryKeyColName(typeof(C));
            List<C> detailentity = bindingSourceSub.DataSource as List<C>;
            if (typeof(C).Name.Contains("Detail") && detailentity != null)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.GetPropertyValue(detailPKName).ToLong() > 0).ToList();
                EditEntity.SetPropertyValue(typeof(C).Name.ToLower() + "s", details);
                if (!Validator<C>(details))
                {
                    return;
                }
            }


            var result = await MainForm.Instance.AppContext.Db.UpdateableByObject(details).ExecuteCommandAsync();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            ReturnResults<T> SaveResult = new ReturnResults<T>();
            SaveResult = await ctr.BaseSaveOrUpdate(EditEntity);
            if (SaveResult.Succeeded)
            {
                MainForm.Instance.PrintInfoLog($"修正成功。");
            }
            else
            {
                MainForm.Instance.PrintInfoLog($"修正失败。", Color.Red);
            }

            MainForm.Instance.AuditLogHelper.CreateAuditLog("数据修正", EditEntity, $"结果:{(SaveResult.Succeeded ? "成功" : "失败")},{SaveResult.ErrorMsg}");
        }

        private string GetPrimaryKeyProperty(Type type)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.GetCustomAttributes(typeof(SugarColumn), true).Length > 0)
                {
                    if (((SugarColumn)property.GetCustomAttributes(typeof(SugarColumn), true)[0]).IsPrimaryKey)
                    {
                        return property.Name;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 加载相关数据的
        /// 联查数据
        /// </summary>
        protected virtual Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (toolStripbtnRelatedQuery.DropDownItems.Count > 0)
            {
                toolStripbtnRelatedQuery.Visible = true;
            }
            else
            {
                toolStripbtnRelatedQuery.Visible = false;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        ///  加载转单功能
        /// </summary>
        protected virtual void LoadConvertDocToDropDownItemsAsync()
        {

        }

        protected virtual void RelatedQuery()
        {
            MessageBox.Show("功能开发中。。。。");
            if (EditEntity == null)
            {
                return;
            }

            T NewEditEntity = default(T);
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();

                // NewEditEntity = Activator.CreateInstance(typeof(T)) as T;

                NewEditEntity = EditEntity.DeepCloneByjson();

                // 获取需要忽略的属性配置
                var ignoreProperties = ConfigureIgnoreProperties();
                //复制性新增 时  PK要清空，单据编号类的,还有他的关联性子集
                // 获取主键列名
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
                //这里只是取打印配置信息
                CommBillData cbd = new CommBillData();
                cbd = bcf.GetBillData(typeof(T), NewEditEntity);

                string billNoColName = cbd.BillNoColName;

                ReflectionHelper.SetPropertyValue(NewEditEntity, billNoColName, string.Empty);

                // 重置主实体的主键
                ResetPrimaryKey(NewEditEntity, PKCol);

                // 重置审批状态
                ResetApprovalStatus(NewEditEntity);

                // 递归处理所有导航属性（明细集合）
                ProcessNavigationProperties(NewEditEntity, PKCol, ignoreProperties);

                OnBindDataToUIEvent(NewEditEntity, ActionStatus.复制);

            }
            ToolBarEnabledControl(MenuItemEnums.新增);
            return;
        }
        protected virtual T AddByCopy()
        {
            if (EditEntity == null)
            {
                MessageBox.Show("请先选择一个单据作为复制的基准。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            T NewEditEntity = default(T);
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();

                NewEditEntity = EditEntity.DeepCloneByjson();
                //复制性新增 时  PK要清空，单据编号类的,还有他的关联性子集


                // 获取忽略属性配置
                var ignoreConfig = ConfigureIgnoreProperties();



                // 重置需要忽略的属性
                ResetIgnoredProperties(NewEditEntity, ignoreConfig);

                // 获取主键列名
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
                //这里只是取打印配置信息
                CommBillData cbd = new CommBillData();
                cbd = bcf.GetBillData(typeof(T), NewEditEntity);

                string billNoColName = cbd.BillNoColName;

                ReflectionHelper.SetPropertyValue(NewEditEntity, billNoColName, string.Empty);

                // 重置主实体的主键
                ResetPrimaryKey(NewEditEntity, PKCol);

                // 重置审批状态
                ResetApprovalStatus(NewEditEntity);

                // 递归处理所有导航属性（明细集合）
                ProcessNavigationProperties(NewEditEntity, PKCol, ignoreConfig);

                OnBindDataToUIEvent(NewEditEntity, ActionStatus.复制);

            }
            ToolBarEnabledControl(MenuItemEnums.新增);
            return NewEditEntity;
        }

        #region 复制性新增
        // 忽略属性配置
        // 基类中的配置方法，使用字符串方式配置通用属性
        protected virtual IgnorePropertyConfiguration ConfigureIgnoreProperties()
        {
            var config = new IgnorePropertyConfiguration();

            // 使用字符串方式配置通用属性（这些属性可能存在于各种实体中）
            config.IgnoreIfExists<T>("DataStatus")
                  .IgnoreIfExists<T>("PrimaryKeyID")
                  .IgnoreIfExists<T>("Created_at")
                  .IgnoreIfExists<T>("Created_by")
                  .IgnoreIfExists<T>("Modified_at")
                  .IgnoreIfExists<T>("Modified_by")
                  .IgnoreIfExists<T>("ApprovalStatus")
                  .IgnoreIfExists<T>("ApprovalResults")
                  .IgnoreIfExists<T>("Approver_by")
                  .IgnoreIfExists<T>("Approver_at")
                  .IgnoreIfExists<T>("PrintStatus");

            return config;
        }



        // 重置需要忽略的属性
        private void ResetIgnoredProperties(object entity, IgnorePropertyConfiguration ignoreConfig)
        {
            if (entity == null) return;

            var entityType = entity.GetType();
            // 检查是否有为该类型定义的忽略属性
            var ignoredProperties = ignoreConfig.GetIgnoredProperties(entityType);

            // 检查是否有为该类型定义的忽略属性
            foreach (var propName in ignoredProperties)
            {
                if (ReflectionHelper.ExistPropertyName(entityType, propName))
                {
                    var prop = entityType.GetProperty(propName);
                    if (prop != null && prop.CanWrite)
                    {
                        // 根据属性类型设置默认值
                        if (prop.PropertyType == typeof(string))
                            prop.SetValue(entity, null);
                        else if (prop.PropertyType == typeof(int))
                            prop.SetValue(entity, 0);
                        else if (prop.PropertyType == typeof(long))
                            prop.SetValue(entity, 0L);
                        else if (prop.PropertyType == typeof(decimal))
                            prop.SetValue(entity, 0m);
                        else if (prop.PropertyType == typeof(DateTime))
                            prop.SetValue(entity, DateTime.MinValue);
                        else if (prop.PropertyType == typeof(DateTime?))
                            prop.SetValue(entity, null);
                        else if (prop.PropertyType == typeof(bool))
                            prop.SetValue(entity, false);
                        // 可以根据需要添加更多类型的处理
                    }
                }
            }

            // 递归处理导航属性
            var navigationProperties = entityType.GetProperties()
                .Where(p => p.PropertyType.IsClass &&
                           p.PropertyType != typeof(string) &&
                           !p.PropertyType.IsValueType);

            foreach (var navProp in navigationProperties)
            {
                var navValue = navProp.GetValue(entity);
                if (navValue != null)
                {
                    if (navValue is System.Collections.IEnumerable &&
                        !(navValue is string))
                    {
                        // 处理集合类型的导航属性
                        foreach (var item in (System.Collections.IEnumerable)navValue)
                        {
                            ResetIgnoredProperties(item, ignoreConfig);
                        }
                    }
                    else
                    {
                        // 处理单个对象的导航属性
                        ResetIgnoredProperties(navValue, ignoreConfig);
                    }
                }
            }
        }
        // 重置实体的主键
        private void ResetPrimaryKey(object entity, string pkCol)
        {
            if (entity == null) return;

            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, pkCol);
            if (pkid > 0)
            {
                ReflectionHelper.SetPropertyValue(entity, pkCol, 0);
            }
        }

        // 重置审批状态相关属性
        private void ResetApprovalStatus(object entity)
        {
            if (entity == null) return;

            if (ReflectionHelper.ExistPropertyName(entity.GetType(), typeof(ApprovalStatus).Name))
            {
                ReflectionHelper.SetPropertyValue(entity, typeof(ApprovalStatus).Name, (int)ApprovalStatus.未审核);
            }
            if (ReflectionHelper.ExistPropertyName(entity.GetType(), typeof(DataStatus).Name))
            {
                ReflectionHelper.SetPropertyValue(entity, typeof(DataStatus).Name, (int)DataStatus.草稿);
            }

            if (ReflectionHelper.ExistPropertyName(entity.GetType(), "ApprovalResults"))
            {
                ReflectionHelper.SetPropertyValue(entity, "ApprovalResults", false);
            }

            BusinessHelper.Instance.InitEntity(entity);
            BusinessHelper.Instance.ClearEntityApproverInfo(entity);
            BusinessHelper.Instance.ClearEntityEditInfo(entity);
            BusinessHelper.Instance.InitStatusEntity(entity);
        }

        // 递归处理所有导航属性（明细集合）

        // 获取引用主实体的外键属性
        private PropertyInfo GetForeignKeyProperty(Type entityType, string parentPKCol)
        {
            // 尝试查找与主实体主键同名的属性
            var fkProperty = entityType.GetProperty(parentPKCol);
            if (fkProperty != null && fkProperty.PropertyType == typeof(long) || fkProperty.PropertyType == typeof(long?))
            {
                return fkProperty;
            }

            // 如果找不到同名属性，可以尝试其他约定，例如添加"_ID"后缀
            fkProperty = entityType.GetProperty($"{entityType.Name}_{parentPKCol}");
            if (fkProperty != null && fkProperty.PropertyType == typeof(long) || fkProperty.PropertyType == typeof(long?))
            {
                return fkProperty;
            }

            // 可以添加更多的外键检测逻辑...

            return null;
        }

        // 处理主实体及其一级明细集合
        private void ProcessNavigationProperties(object entity, string parentPKCol, IgnorePropertyConfiguration ignoreConfig)
        {
            if (entity == null) return;

            if (entity.ContainsProperty("PrimaryKeyID"))
                entity.SetPropertyValue("PrimaryKeyID", 0);

            var type = entity.GetType();
            var entityName = type.Name;



            // 获取主实体的主键值，用于更新明细的外键
            long parentPKValue = (long)ReflectionHelper.GetPropertyValue(entity, parentPKCol);

            // 查找所有以主实体名+Detail结尾的导航属性
            var detailProperties = type.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                           p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                           p.PropertyType.GetGenericArguments()[0].Name.EndsWith($"{entityName}Detail"))
                .ToList();

            foreach (var detailProperty in detailProperties)
            {
                // 获取明细集合类型
                var detailType = detailProperty.PropertyType.GetGenericArguments()[0];

                // 获取明细的主键和外键属性名
                string detailPKCol = BaseUIHelper.GetEntityPrimaryKey(detailType);
                string detailFKCol = $"{entityName}_ID"; // 假设外键名为"主表名_ID"

                // 检查外键属性是否存在
                var fkProperty = detailType.GetProperty(detailFKCol);
                if (fkProperty == null)
                {
                    // 尝试查找外键属性（多种可能的命名约定）
                    fkProperty = detailType.GetProperty($"{entityName}_ID") ??
                                    detailType.GetProperty(parentPKCol) ??
                                    detailType.GetProperties().FirstOrDefault(p =>
                                        p.Name.EndsWith("_ID") && p.PropertyType == typeof(long));
                }

                if (fkProperty != null)
                {
                    var collection = detailProperty.GetValue(entity) as System.Collections.IEnumerable;
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            if (item.ContainsProperty("PrimaryKeyID"))
                                item.SetPropertyValue("PrimaryKeyID", 0);

                            // 重置明细的主键
                            ReflectionHelper.SetPropertyValue(item, detailPKCol, 0);

                            // 重置明细的外键（指向主实体）
                            ReflectionHelper.SetPropertyValue(item, fkProperty.Name, 0);
                            // 重置需要忽略的属性
                            ResetIgnoredProperties(item, ignoreConfig);
                            // 处理明细的子明细（第二级）
                            ProcessSecondLevelDetails(item, detailPKCol, detailType.Name, ignoreConfig);
                        }
                    }
                }
            }
        }

        // 处理第二级明细集合（明细的明细）
        private void ProcessSecondLevelDetails(object entity, string parentPKCol, string parentEntityName,
            IgnorePropertyConfiguration ignoreConfig)
        {
            if (entity == null) return;

            var type = entity.GetType();

            // 查找所有以当前实体名+Detail结尾的导航属性
            var subDetailProperties = type.GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                           p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                           p.PropertyType.GetGenericArguments()[0].Name.EndsWith($"{parentEntityName}Detail"))
                .ToList();

            // 查找所有集合类型的导航属性（假设它们是子明细）
            //var subDetailProperties = type.GetProperties()
            //    .Where(p => p.PropertyType.IsGenericType &&
            //               p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            //    .ToList();




            foreach (var subDetailProperty in subDetailProperties)
            {
                // 获取子明细集合类型
                var subDetailType = subDetailProperty.PropertyType.GetGenericArguments()[0];

                // 获取子明细的主键和外键属性名
                string subDetailPKCol = BaseUIHelper.GetEntityPrimaryKey(subDetailType);
                string subDetailFKCol = $"{parentEntityName}_ID"; // 假设外键名为"父表名_ID"

                // 检查外键属性是否存在
                var fkProperty = subDetailType.GetProperty(subDetailFKCol);
                if (fkProperty == null)
                {
                    // 尝试其他可能的外键命名方式
                    fkProperty = subDetailType.GetProperty($"{parentEntityName}_ID") ??
                         subDetailType.GetProperty(parentPKCol) ??
                         subDetailType.GetProperties().FirstOrDefault(p =>
                             p.Name.EndsWith("_ID") && p.PropertyType == typeof(long));
                }

                if (fkProperty != null)
                {
                    var collection = subDetailProperty.GetValue(entity) as System.Collections.IEnumerable;
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            // 重置子明细的主键
                            ReflectionHelper.SetPropertyValue(item, subDetailPKCol, 0);

                            // 重置子明细的外键（指向父明细）
                            ReflectionHelper.SetPropertyValue(item, fkProperty.Name, 0);


                            // 重置需要忽略的属性
                            ResetIgnoredProperties(item, ignoreConfig);
                        }
                    }
                }
            }
        }

        #endregion



        protected override void Clear(SourceGridDefine sgd)
        {
            SourceGrid.Grid grid1 = sgd.grid;
            EditEntity = Activator.CreateInstance(typeof(T)) as T;
            BusinessHelper.Instance.InitEntity(EditEntity);
            BusinessHelper.Instance.InitStatusEntity(EditEntity);
            bindingSourceSub.Clear();
            //清空明细表格
            #region
            //先清空 不包含 列头和总计
            SourceGrid.RangeRegion rr = new SourceGrid.RangeRegion(new SourceGrid.Position(grid1.Rows.Count, grid1.Columns.Count));
            for (int ii = 0; ii < grid1.Rows.Count; ii++)
            {
                grid1.Rows[ii].RowData = null;
            }
            grid1.ClearValues(rr);


            #endregion
        }
        protected bool Validator(T EditEntity)
        {
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            bool vd = base.ShowInvalidMessage(ctr.BaseValidator(EditEntity));
            return vd;
        }

        /// <summary>
        /// 验证明细
        /// </summary>
        /// <typeparam name="Child"></typeparam>
        /// <param name="details"></param>
        /// <returns></returns>
        protected bool Validator<Child>(List<Child> details) where Child : class
        {
            List<bool> subrs = new List<bool>();
            var lastlist = ((IEnumerable<dynamic>)details).ToList();
            foreach (var item in lastlist)
            {
                BaseController<Child> ctr = Startup.GetFromFacByName<BaseController<Child>>(typeof(Child).Name + "Controller");
                bool sub_bool = base.ShowInvalidMessage(ctr.BaseValidator(item as Child));
                subrs.Add(sub_bool);
            }
            if (subrs.Where(c => c.Equals(false)).Any())
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 更新式保存，有一些单据，实在要修改，并且明细没有删除和添加时候执行
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<ReturnMainSubResults<T>> UpdateSave(T entity)
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            if (pkid == 0)
            {
                entity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.草稿);
            }
            ReturnMainSubResults<T> rmr = new ReturnMainSubResults<T>();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            rmr = await ctr.BaseUpdateWithChild(entity);
            if (rmr.Succeeded)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
                {
                    //注意這里保存的是枚举
                    ReflectionHelper.SetPropertyValue(entity, typeof(ActionStatus).Name, (int)ActionStatus.加载);
                }

                ToolBarEnabledControl(MenuItemEnums.保存);
                MainForm.Instance.uclog.AddLog("更新式保存成功");
                MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("更新式保存成功", rmr.ReturnObject);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("更新式保存成功失败，请重试;或联系管理员。" + rmr.ErrorMsg, UILogType.错误);
            }
            return rmr;
        }

        protected async Task<ReturnMainSubResults<T>> Save(T entity)
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            if (pkid == 0)
            {
                entity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.草稿);
                BusinessHelper.Instance.InitEntity(entity);
            }
            else
            {

                if (IsLock())
                {
                    string tipmsg = string.Empty;
                    //如果已经有锁定标记了。即使已经释放了锁。也要刷新数据后才可以保存。不然数据不会统一。
                    if (tsBtnLocked.Visible && tsBtnLocked.Tag != null && tsBtnLocked.Tag is LockInfo lockInfo)
                    {
                        //显示锁定认为锁定。要刷新数据
                        tipmsg = $"单据已被【{lockInfo.LockedByName}】锁定，请刷新后再试";
                    }

                    return new ReturnMainSubResults<T>()
                    {
                        Succeeded = false,
                        ErrorMsg = tipmsg
                    };
                }
            }

            if (editEntity.ContainsProperty(typeof(DataStatus).Name))
            {
                //如果修改前的状态是新建，则修改后的状态是草稿。要重新提交才进入下一步审核
                var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.新建)
                {
                    if (ReflectionHelper.ExistPropertyName<T>(typeof(DataStatus).Name))
                    {
                        ReflectionHelper.SetPropertyValue(EditEntity, typeof(DataStatus).Name, (int)DataStatus.草稿);
                    }
                }
            }

            ReturnMainSubResults<T> rmr = new ReturnMainSubResults<T>();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            rmr = await ctr.BaseSaveOrUpdateWithChild<T>(entity);
            if (rmr.Succeeded)
            {
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.AcceptChanges();
                }

                if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
                {
                    //注意這里保存的是枚举
                    ReflectionHelper.SetPropertyValue(entity, typeof(ActionStatus).Name, (int)ActionStatus.加载);
                }
                ToolBarEnabledControl(MenuItemEnums.保存);
                MainForm.Instance.uclog.AddLog("保存成功");
                //var sw = new Stopwatch();
                //sw.Start();
                //var resultContext = await next();
                //sw.Stop();

                //审计日志 保存
                //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("保存", rmr.ReturnObject);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("保存失败，请重试;或联系管理员。" + rmr.ErrorMsg, UILogType.错误);
                MainForm.Instance.logger.LogError("保存失败", rmr.ErrorMsg);
            }
            MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("保存", rmr.ReturnObject, $"结果:{(rmr.Succeeded ? "成功" : "失败")},{rmr.ErrorMsg}");
            return rmr;
        }

        /// <summary>
        /// 删除远程的图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<bool> DeleteRemoteImages()
        {
            await Task.Delay(0);
            return false;
        }


        protected async virtual Task<ReturnResults<T>> Delete()
        {
            ReturnResults<T> rss = new ReturnResults<T>();
            if (editEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (dataStatus == DataStatus.新建 && !AppContext.IsSuperUser)
                    {
                        if (ReflectionHelper.ExistPropertyName<T>("Created_by") && ReflectionHelper.GetPropertyValue(editEntity, "Created_by").ToString() != AppContext.CurUserInfo.Id.ToString())
                        {
                            MessageBox.Show("只有创建人才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人才能删除提交的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }
                    bool rs = false;
                    BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                    //这个表特殊当时没有命名好
                    if (typeof(C).Name.Contains("Detail") || typeof(C).Name.Contains("tb_ProductionDemand"))
                    {
                        rs = await ctr.BaseDeleteByNavAsync(editEntity as T);
                    }
                    else
                    {
                        rs = await ctr.BaseDeleteAsync(editEntity as T);
                    }
                    object PKValue = editEntity.GetPropertyValue(UIHelper.GetPrimaryKeyColName(typeof(T)));
                    rss.Succeeded = rs;
                    rss.ReturnObject = editEntity;
                    if (rs)
                    {
                        MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("删除", editEntity);
                        if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        {
                            //MainForm.Instance.logger.Debug($"单据显示中删除:{typeof(T).Name}，主键值：{PKValue.ToString()} "); //如果要生效 要将配置文件中 <add key="log4net.Internal.Debug" value="true " /> 也许是：logn4net.config <log4net debug="false"> 改为true
                        }
                        bindingSourceSub.Clear();

                        //删除远程图片及本地图片
                        await DeleteRemoteImages();

                        //提示一下删除成功
                        MainForm.Instance.uclog.AddLog("提示", "删除成功");

                        //加载一个空的显示的UI
                        bindingSourceSub.Clear();
                        OnBindDataToUIEvent(Activator.CreateInstance(typeof(T)) as T, ActionStatus.删除);
                    }
                    else
                    {
                        MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("删除失败", editEntity);
                    }
                }
                else
                {
                    //
                    MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                }
            }
            return rss;
        }




        /// <summary>
        /// 提交
        /// </summary>
        protected async override Task<bool> Submit()
        {
            if (EditEntity == null)
            {
                return false;
            }

            //if (StatusMachine.CanSubmit())
            //{
            //    StatusMachine.Submit();
            //    // 自动触发状态更新
            //}
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            bool submitrs = false;
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            if (pkid > 0)
            {
                var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.完结 || dataStatus == DataStatus.确认)
                {

                    if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                    {
                        MainForm.Instance.uclog.AddLog("单据已经是【完结】或【确认】状态，提交失败。");
                    }
                    return false;
                }
                else
                {

                    ReturnResults<T> rmr = await ctr.SubmitAsync(EditEntity, false);
                    //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                    if (rmr.Succeeded)
                    {
                        if (EditEntity is BaseEntity baseEntity)
                        {
                            baseEntity.AcceptChanges();
                        }
                        ToolBarEnabledControl(MenuItemEnums.提交);
                        //这里推送到审核，启动工作流 后面优化
                        // OriginalData od = ActionForClient.工作流提交(pkid, (int)BizType.盘点单);
                        // MainForm.Instance.ecs.AddSendData(od);]

                        //如果是销售订单或采购订单可以自动审核，有条件地执行？
                        CommBillData cbd = new CommBillData();
                        BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
                        cbd = bcf.GetBillData<T>(EditEntity as T);
                        ApprovalEntity ae = new ApprovalEntity();
                        ae.ApprovalOpinions = "自动审核";
                        ae.ApprovalResults = true;
                        ae.ApprovalStatus = (int)ApprovalStatus.已审核;
                        if (cbd.BizType == BizType.销售订单 && AppContext.SysConfig.AutoApprovedSaleOrderAmount > 0)
                        {
                            if (EditEntity is tb_SaleOrder saleOrder)
                            {
                                if (saleOrder.TotalAmount <= AppContext.SysConfig.AutoApprovedSaleOrderAmount)
                                {
                                    RevertCommand command = new RevertCommand();
                                    //缓存当前编辑的对象。如果撤销就回原来的值
                                    tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
                                    command.UndoOperation = delegate ()
                                    {
                                        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                                        CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
                                    };
                                    BusinessHelper.Instance.ApproverEntity(EditEntity);
                                    saleOrder.ApprovalResults = true;
                                    saleOrder.ApprovalStatus = (int)ApprovalStatus.已审核;
                                    saleOrder.ApprovalOpinions = "自动审核";
                                    saleOrder.DataStatus = (int)DataStatus.确认;
                                    tb_SaleOrderController<tb_SaleOrder> ctrSO = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                                    ReturnResults<tb_SaleOrder> rmrs = await ctrSO.ApprovalAsync(saleOrder);
                                    if (rmrs.Succeeded)
                                    {
                                        //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                                        //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                                        //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                                        //MainForm.Instance.ecs.AddSendData(od);
                                        //审核成功
                                        ToolBarEnabledControl(MenuItemEnums.审核);
                                        MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", rmr.ReturnObject, "满足金额设置条件，自动审核通过");
                                        //如果审核结果为不通过时，审核不是灰色。
                                        if (!ae.ApprovalResults)
                                        {
                                            toolStripbtnReview.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        command.Undo();
                                        //审核失败 要恢复之前的值
                                        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rmrs.ErrorMsg}请联系管理员！", Color.Red);
                                    }
                                }
                            }
                        }

                        if (cbd.BizType == BizType.采购订单 && AppContext.SysConfig.AutoApprovedPurOrderAmount > 0)
                        {
                            if (EditEntity is tb_PurOrder purOrder)
                            {
                                if (purOrder.TotalAmount <= AppContext.SysConfig.AutoApprovedPurOrderAmount)
                                {
                                    RevertCommand command = new RevertCommand();
                                    //缓存当前编辑的对象。如果撤销就回原来的值
                                    tb_PurOrder oldobj = CloneHelper.DeepCloneObject<tb_PurOrder>(EditEntity);
                                    command.UndoOperation = delegate ()
                                    {
                                        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                                        CloneHelper.SetValues<tb_PurOrder>(EditEntity, oldobj);
                                    };
                                    purOrder.ApprovalResults = true;
                                    purOrder.ApprovalStatus = (int)ApprovalStatus.已审核;
                                    purOrder.ApprovalOpinions = "自动审核";
                                    purOrder.DataStatus = (int)DataStatus.确认;
                                    BusinessHelper.Instance.ApproverEntity(EditEntity);
                                    tb_PurOrderController<tb_PurOrder> ctrSO = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                                    ReturnResults<tb_PurOrder> rmrs = await ctrSO.ApprovalAsync(purOrder);
                                    if (rmrs.Succeeded)
                                    {
                                        //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                                        //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                                        //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                                        //MainForm.Instance.ecs.AddSendData(od);

                                        //审核成功
                                        ToolBarEnabledControl(MenuItemEnums.审核);
                                        MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", rmr.ReturnObject, "满足金额设置条件，自动审核通过");
                                        //如果审核结果为不通过时，审核不是灰色。
                                        if (!ae.ApprovalResults)
                                        {
                                            toolStripbtnReview.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        command.Undo();
                                        //审核失败 要恢复之前的值
                                        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rmrs.ErrorMsg},请联系管理员！", Color.Red);

                                    }
                                }
                            }
                        }
                        submitrs = true;
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog($"提交失败，请重试;或联系管理员。\r\n 错误信息：{rmr.ErrorMsg}", UILogType.错误);
                        submitrs = false;
                    }
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("更新式提交", rmr.ReturnObject, $"结果:{(rmr.Succeeded ? "成功" : "失败")},{rmr.ErrorMsg}");
                }
            }
            else
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(DataStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(EditEntity, typeof(DataStatus).Name, (int)DataStatus.新建);
                }
                if (ReflectionHelper.ExistPropertyName<T>(typeof(ApprovalStatus).Name))
                {
                    ReflectionHelper.SetPropertyValue(EditEntity, typeof(ApprovalStatus).Name, (int)ApprovalStatus.未审核);
                }
                bool rs = await this.Save(true);
                if (rs)
                {
                    ToolBarEnabledControl(MenuItemEnums.提交);
                    submitrs = true;
                }
                MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("保存式提交", EditEntity, $"结果:{(rs ? "成功" : "失败")}");
            }
            return submitrs;
        }




        /// <summary>提交单据</summary>
        protected async Task<bool> Submit<TStatus>(TStatus targetStatus)
            where TStatus : Enum
        {
            if (EditEntity == null) return false;



            // 获取当前状态
            var statusProperty = typeof(TStatus).Name;
            var currentStatus = (TStatus)Enum.ToObject(
                typeof(TStatus),
                EditEntity.GetPropertyValue(statusProperty)
            );

            // 验证状态转换
            try
            {
                FMPaymentStatusHelper.ValidateTransition(currentStatus, targetStatus);
            }
            catch (InvalidOperationException ex)
            {
                MainForm.Instance.uclog.AddLog($"提交失败: {ex.Message}");
                return false;
            }


            if (!FMPaymentStatusHelper.CanSubmit(currentStatus))
            {


                MainForm.Instance.uclog.AddLog("单据非草稿状态，提交失败");
                toolStripbtnSubmit.Enabled = false;
                return false;
            }

            //   // 状态为草稿时直接更新状态  业务方法中判断了。不能重复
            //ReflectionHelper.SetPropertyValue(EditEntity, statusProperty, (int)(object)targetStatus);

            // 保存实体
            var ctr = Startup.GetFromFacByName<BaseController<T>>($"{typeof(T).Name}Controller");
            //var result = await controller.BaseSaveOrUpdate(EditEntity as T);
            ReturnResults<T> result = await ctr.SubmitAsync(EditEntity, true);
            if (result.Succeeded)
            {
                if (EditEntity is BaseEntity baseEntity)
                {
                    baseEntity.AcceptChanges();
                }
                MainForm.Instance.uclog.AddLog("提交成功");
                ToolBarEnabledControl(EditEntity);

                // 记录审计日志
                MainForm.Instance.AuditLogHelper.CreateAuditLog<T>(
                    "提交",
                    result.ReturnObject,
                    $"状态变更: {currentStatus} → {targetStatus}"
                );

                return true;
            }
            else
            {
                //单据保存后再提交
                MessageBox.Show("提交失败，请重试;或联系管理员。\r\n 错误信息：" + result.ErrorMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            MainForm.Instance.uclog.AddLog($"提交失败: {result.ErrorMsg}", UILogType.错误);
            return false;
        }


        /// <summary>
        /// 优化后的查询条件
        /// </summary>
        public QueryFilter QueryConditionFilter { get; set; } = new QueryFilter();

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public virtual void QueryConditionBuilder()
        {
            //添加默认全局的
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }



        protected override void Query()
        {
            if (base.Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }

            // 如果没有条件列表 直接查全部。
            // CommonUI.frmQuery<T> frm = new CommonUI.frmQuery<T>();
            //frm.QueryConditions = QueryConditions;
            //frm.LoadQueryConditionToUI(false);
            //frm.OnSelectDataRow += UcAdv_OnSelectDataRow;
            if (QueryConditionFilter.QueryFields.Count == 0)
            {
                QueryConditionBuilder();
            }

            //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
            UCAdvFilterGeneric<T> ucBaseList = new UCAdvFilterGeneric<T>(); // Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);

            ucBaseList.QueryConditionFilter = QueryConditionFilter;
            ucBaseList.CurMenuInfo = CurMenuInfo;
            ucBaseList.KeyValueTypeForDgv = typeof(T);
            ucBaseList.Runway = BaseListRunWay.选中模式;
            //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
            frmBaseEditList frm = new frmBaseEditList();
            frm.StartPosition = FormStartPosition.CenterScreen;
            ucBaseList.Dock = DockStyle.Fill;
            ucBaseList.Tag = frm;
            frm.kryptonPanel1.Controls.Add(ucBaseList);
            ucBaseList.OnSelectDataRow += UcBaseList_OnSelectDataRow;
            BizTypeMapper mapper = new BizTypeMapper();
            var BizTypeText = mapper.GetBizType(typeof(T).Name).ToString();
            frm.Text = "关联查询" + "-" + BizTypeText;
            frm.Show();

        }



        private void UcBaseList_OnSelectDataRow(object entity)
        {
            if (entity == null)
            {
                return;
            }
            if (OnBindDataToUIEvent != null)
            {
                bindingSourceSub.Clear();
                OnBindDataToUIEvent(entity as T, ActionStatus.加载);

                //如果 查出来的数据 能审核 能打印 等显示各种状态 TODO
                ToolBarEnabledControl(entity);

                ToolBarEnabledControl(MenuItemEnums.查询);
                //thisform
            }
            //使用了导航查询 entity包括了明细
            //details = (entity as tb_Stocktake).tb_StocktakeDetails;
            //LoadDataToGrid(details);
        }



        protected async override void Refreshs()
        {
            if (editEntity == null)
            {
                MainForm.Instance.PrintInfoLog("当前数据不存在，无法刷新。");
                return;
            }

            //MainForm.Instance.PrintInfoLog(evaluator.CurrentStatus.ToString());

            if (true)//CanRefresh()
            {
                using (StatusBusy busy = new StatusBusy("刷新中..."))
                {
                    //这里应该是重新加载单据内容 而不是查询
                    //但是，查询才是对的，因为数据会修改变化缓存。
                    if (!Edited)
                    {
                        if (OnBindDataToUIEvent != null)
                        {
                            BaseEntity pkentity = (editEntity as T) as BaseEntity;
                            if (pkentity.PrimaryKeyID > 0)
                            {
                                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                                editEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;
                            }
                            else
                            {
                                MessageBox.Show("数据不存在。系统自动转换为新增模式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                toolStripButtonRefresh.Enabled = false;
                                editEntity = Activator.CreateInstance<T>();
                            }
                            bindingSourceSub.Clear();
                            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);

                            //if (pkentity.PrimaryKeyID > 0)
                            //{
                            //    //可以修改
                            //    if (pkentity.ContainsProperty(typeof(DataStatus).Name))
                            //    {
                            //        if (pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.草稿).ToString() ||
                            //               pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.新建).ToString())
                            //        {
                            //            toolStripbtnModify.Enabled = true;
                            //            toolStripbtnSubmit.Enabled = true;
                            //            toolStripbtnReview.Enabled = true;
                            //        }
                            //    }
                            //}

                            ToolBarEnabledControl(pkentity);
                        }
                        else
                        {
                            //
                            MessageBox.Show("请实现数据绑定的事件。用于显示数据详情。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (MessageBox.Show(this, "有数据没有保存\r\n你确定要重新加载吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            if (OnBindDataToUIEvent != null)
                            {
                                BaseEntity pkentity = (editEntity as T) as BaseEntity;
                                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                                editEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;
                                bindingSourceSub.Clear();
                                if (editEntity == null)
                                {
                                    editEntity = Activator.CreateInstance<T>();
                                }
                                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
                                //if (pkentity.PrimaryKeyID > 0)
                                //{
                                //    //可以修改
                                //    if (pkentity.ContainsProperty(typeof(DataStatus).Name))
                                //    {
                                //        if (pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.草稿).ToString() ||
                                //               pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.新建).ToString())
                                //        {
                                //            toolStripbtnModify.Enabled = true;
                                //            toolStripbtnSubmit.Enabled = true;
                                //            toolStripbtnReview.Enabled = true;
                                //        }
                                //    }
                                //}
                                //刷新了。不再提示编辑状态了
                                Edited = false;
                                ToolBarEnabledControl(pkentity);
                            }
                        }
                    }
                }
            }
        }

        protected override void Exit(object thisform)
        {
            try
            {
                // 单据都会有 录入表格 SourceGridHelper 在 Grid_HandleDestroyed 中执行了。这样就不管关闭还是x
            }
            catch
            {

            }

            base.Exit(this);
        }

        public override void RequestUnLock()
        {
            if (EditEntity != null)
            {
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
                if (pkid > 0)
                {
                    //如果这个锁是自己锁的。就释放
                    long userid = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                    var LockInfo = MainForm.Instance.lockManager.GetLockStatus(pkid);
                    if (LockInfo != null && LockInfo.LockedByID != userid)
                    {
                        BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
                        //这里只是取打印配置信息
                        CommBillData cbd = new CommBillData();
                        cbd = bcf.GetBillData(typeof(T), EditEntity);

                        ClientLockManagerCmd cmd = new ClientLockManagerCmd(CmdOperation.Send);

                        cmd.lockCmd = LockCmd.RequestUnLock;

                        RequestUnLockInfo lockRequest = new RequestUnLockInfo();
                        lockRequest.RequestUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name;
                        lockRequest.RequestUserID = userid;
                        lockRequest.BillID = pkid;
                        lockRequest.BillData = cbd;
                        lockRequest.MenuID = CurMenuInfo.MenuID;
                        lockRequest.LockedUserID = LockInfo.LockedByID;
                        lockRequest.LockedUserName = LockInfo.LockedByName;
                        lockRequest.PacketId = cmd.PacketId;
                        cmd.RequestInfo = lockRequest;
                        ClientEventManager.Instance.AddCommandHandler(cmd.PacketId, cmd.HandleLockEvent);
                        MainForm.Instance.dispatcher.DispatchAsync(cmd, CancellationToken.None);
                        cmd.LockChanged += (sender, e) =>
                        {
                            MessageBox.Show($"已经向锁定者【{lockRequest.LockedUserName}】发送了解锁请求。等待结果中");
                        };
                    }

                }
            }
        }

        public override void UNLock()
        {
            if (EditEntity != null)
            {
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
                if (pkid > 0)
                {
                    //如果这个锁是自己锁的。就释放
                    long userid = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                    if (MainForm.Instance.lockManager.GetLockedBy(pkid) == userid)
                    {
                        UNLock(pkid, userid);
                        //先本地再服务器
                        if (MainForm.Instance.lockManager.Unlock(pkid, userid))
                        {
                            MainForm.Instance.PrintInfoLog($"本地删除{pkid}锁成功");
                        }
                        this.tsBtnLocked.Visible = true;
                        this.tsBtnLocked.Image = global::RUINORERP.UI.Properties.Resources.unlockbill;
                    }

                    //BizTypeMapper mapper = new BizTypeMapper();
                    //OriginalData od = ActionForClient.单据锁定释放(pkid, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID,
                    //         MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name,
                    //         (int)mapper.GetBizType(typeof(T)), CurMenuInfo.MenuID);
                    //MainForm.Instance.ecs.AddSendData(od);
                }
            }
        }


        public void UNLockByBizName(long userid)
        {
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommBillData cbd = new CommBillData();
            cbd = bcf.GetBillData(typeof(T), EditEntity);

            //得到了锁
            ClientLockManagerCmd cmd = new ClientLockManagerCmd(CmdOperation.Send);
            cmd.lockCmd = LockCmd.UnLockByBizName;
            UnLockInfo lockRequest = new UnLockInfo();
            lockRequest.BillData = cbd;
            lockRequest.LockedUserID = userid;
            lockRequest.LockedUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name;
            lockRequest.MenuID = CurMenuInfo.MenuID;
            lockRequest.PacketId = cmd.PacketId;
            cmd.RequestInfo = lockRequest;
            // 注册锁定单据的事件处理程序
            ClientEventManager.Instance.AddCommandHandler(cmd.PacketId, cmd.HandleLockEvent);
            MainForm.Instance.dispatcher.DispatchAsync(cmd, CancellationToken.None);
            if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
            {
                MainForm.Instance.PrintInfoLog($"向服务器发送【业务类型】解锁{lockRequest.BillData.BizName}成功");
            }

        }


        public void UNLock(long billid, long userid)
        {
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommBillData cbd = new CommBillData();
            cbd = bcf.GetBillData(typeof(T), EditEntity);

            //得到了锁
            ClientLockManagerCmd cmd = new ClientLockManagerCmd(CmdOperation.Send);
            cmd.lockCmd = LockCmd.UNLOCK;
            UnLockInfo lockRequest = new UnLockInfo();
            lockRequest.BillData = cbd;
            lockRequest.BillID = billid;
            lockRequest.LockedUserID = userid;
            lockRequest.LockedUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name;
            lockRequest.MenuID = CurMenuInfo.MenuID;
            lockRequest.PacketId = cmd.PacketId;
            cmd.RequestInfo = lockRequest;
            // 注册锁定单据的事件处理程序
            ClientEventManager.Instance.AddCommandHandler(cmd.PacketId, cmd.HandleLockEvent);

            MainForm.Instance.dispatcher.DispatchAsync(cmd, CancellationToken.None);

            if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
            {
                MainForm.Instance.PrintInfoLog($"向服务器发送锁{lockRequest.BillID}成功");
            }
            cmd.LockChanged += (sender, e) =>
            {
                if (e.requestBaseInfo.BillID != billid)
                {
                    return;
                }
                if (e.isSuccess)
                {
                    this.tsBtnLocked.Visible = false;
                }

            };

        }

        internal override void CloseTheForm(object thisform)
        {
            UNLock();
            base.CloseTheForm(thisform);
        }

        #region 打印相关
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
        public async Task PrintDesigned()
        {
            try
            {
                if (EditEntity == null)
                {
                    MessageBox.Show("请提供正确的打印数据！");
                    return;
                }
                List<T> list = new List<T>();
                list.Add(EditEntity);
                if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
                {
                    PrintConfig = PrintHelper<T>.GetPrintConfig(list);
                }
                bool rs = await PrintHelper<T>.Print(list, RptMode.DESIGN, PrintConfig);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex);

            }
        }

        /// <summary>
        /// 单个单据打印
        /// </summary>
        public async Task Print()
        {
            if (EditEntity == null)
            {
                MessageBox.Show("请提供正确的打印数据！");
                return;
            }
            List<T> list = new List<T>();
            list.Add(EditEntity);
            foreach (var item in list)
            {
                if (item == null)
                {
                    continue;
                }
                if (item.ContainsProperty("DataStatus"))
                {
                    if (item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.草稿).ToString().ToString())
                    {
                        MessageBox.Show("没有提交的数据不能打印！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.新建).ToString())
                    {
                        if (MessageBox.Show("没有审核的数据无法打印,你确定要打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
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
                            return;
                        }
                    }
                }
            }
            if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
            {
                PrintConfig = PrintHelper<T>.GetPrintConfig(list);
            }
            bool rs = await PrintHelper<T>.Print(list, RptMode.PRINT, PrintConfig);
            if (rs)
            {
                MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("打印", EditEntity);
            }

        }

        public async Task Preview()
        {
            bool rs = false;
            try
            {
                if (EditEntity == null)
                {
                    MessageBox.Show("请提供正确的打印预览数据！");
                    return;
                }
                List<T> list = new List<T>();
                list.Add(EditEntity);
                if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
                {
                    PrintConfig = PrintHelper<T>.GetPrintConfig(list);
                }
                rs = await PrintHelper<T>.Print(list, RptMode.PREVIEW, PrintConfig);
            }

            catch (Exception ex)
            {
                MainForm.Instance.logger.Error("打印配置加载异常", ex);
            }
        }

        #endregion

        private void BaseBillEditGeneric_Load(object sender, EventArgs e)
        {
            timerAutoSave.Start();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    QueryConditionBuilder();
                    #region 请求缓存
                    //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
                    UIBizSrvice.RequestCache<T>();
                    UIBizSrvice.RequestCache<C>();
                    // UIBizSrvice.RequestCache<tb_Prod>();
                    // UIBizSrvice.RequestCache<View_ProdDetail>();
                    //去检测产品视图的缓存并且转换为强类型
                    MainForm.Instance.TryRequestCache(nameof(View_ProdDetail), typeof(View_ProdDetail));

                    //先加载一遍缓存
                    var tableNames = MainForm.Instance.CacheInfoList.Keys.ToList();
                    foreach (var nextTableName in tableNames)
                    {
                        MainForm.Instance.TryRequestCache(nextTableName);
                    }
                    #endregion
                }
            }
        }

        private async void timerAutoSave_Tick(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    //自动保存的时间秒数  30秒
                    if (MainForm.Instance.AppContext.CurrentUser.静止时间 > 30 && MainForm.Instance.AppContext.IsOnline)
                    {
                        bool result = await AutoSaveDataAsync();
                        if (!result)
                        {
                            //如果保存失败，则停止自动保存？
                            timerAutoSave.Stop();
                        }
                    }
                }
            }

        }

        private async Task<bool> AutoSaveDataAsync()
        {
            bool result = false;
            try
            {
                if (EditEntity != null)
                {
                    #region 自动保存单据数据  后面优化可以多个单?限制5个？Cache
                    await Save(false);
                    string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data", CurMenuInfo.CaptionCN + ".cache");
                    System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
                    //判断目录是否存在
                    if (!System.IO.Directory.Exists(fi.Directory.FullName))
                    {
                        System.IO.Directory.CreateDirectory(fi.Directory.FullName);
                    }
                    string json = JsonConvert.SerializeObject(EditEntity,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                        });


                    /*
                     var settings = new JsonSerializerSettings
        {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
        };

        string jsonString = JsonConvert.SerializeObject(myObject, settings);
                     */


                    File.WriteAllText(PathwithFileName, json);
                    MainForm.Instance.uclog.AddLog("缓存数据保存成功。");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "缓存数据保存时出错");
            }
            return result;
        }


    }






}