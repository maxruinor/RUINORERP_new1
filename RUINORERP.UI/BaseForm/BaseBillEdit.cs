using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using FluentValidation.Results;
using Autofac;
using RUINORERP.Model;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using RUINORERP.UI.Properties;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Common.Helper;
using RUINORERP.UI.Report;
using RUINORERP.Business;
using Microsoft.Extensions.Logging;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.UI.UCSourceGrid;
using FluentValidation;
using RUINORERP.Model.CommonModel;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.HelpSystem.Extensions;
using RUINORERP.UI.Common;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Linq.Expressions;
using System.Web.UI;
using Control = System.Windows.Forms.Control;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.PacketSpec.Enums.Core;
using UserControl = System.Windows.Forms.UserControl;
using Org.BouncyCastle.Asn1.X509.Qualified;
using RUINORERP.UI.HelpSystem.Core;

namespace RUINORERP.UI.BaseForm
{
    public partial class BaseBillEdit : UserControl
    {

        public ApplicationContext AppContext { set; get; }
        public ILogger<MainForm> logger { get; set; }
        public BaseBillEdit()
        {
            InitializeComponent();
            InitializeStateManagement();
            bwRemoting.DoWork += bwRemoting_DoWork;
            bwRemoting.RunWorkerCompleted += bwRemoting_RunWorkerCompleted;
            bwRemoting.ProgressChanged += bwRemoting_progressChanged;
            //如果打开单时。被其它人锁定。才显示锁定图标
            tsBtnLocked.Visible = false;
            //tsBtnLocked.Click -= Item_Click;
            //tsBtnLocked.Click += Item_Click;

            // 初始化帮助系统
            InitializeHelpSystem();
        }

        #region 帮助系统集成

        /// <summary>
        /// 是否启用智能帮助系统
        /// </summary>
        [Browsable(true)]
        [Category("帮助系统")]
        [Description("是否启用智能帮助系统")]
        public bool EnableSmartHelp { get; set; } = true;

        /// <summary>
        /// 窗体帮助键(可选,覆盖默认值)
        /// </summary>
        [Category("帮助系统")]
        [Description("窗体帮助键,留空则使用窗体类型名称")]
        public string FormHelpKey { get; set; }

        /// <summary>
        /// 当前窗体的菜单信息
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; }

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        protected virtual void InitializeHelpSystem()
        {
            if (!EnableSmartHelp) return;

            try
            {
                // 启用F1帮助
                this.EnableF1Help();

                // 启用智能提示(避免设计模式时报错)
                if (!this.DesignMode && System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                {
                    // 为所有控件启用智能提示
                    HelpManager.Instance.EnableSmartTooltipForAll(this, FormHelpKey);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化帮助系统失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示控件帮助
        /// </summary>
        /// <param name="control">目标控件</param>
        protected void ShowControlHelp(Control control)
        {
            if (EnableSmartHelp)
            {
                HelpManager.Instance.ShowControlHelp(control);
            }
        }

        #endregion


        #region 状态控件的所有代码

        #region 字段

        /// <summary>
        /// v3统一状态管理器
        /// </summary>
        public IUnifiedStateManager _stateManager;


        /// <summary>
        /// 状态管理初始化标志，防止重复初始化
        /// </summary>
        protected bool _isStateManagementInitialized = false;

        #endregion


        #region 属性

        /// <summary>
        /// v3统一状态管理器
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IUnifiedStateManager StateManager
        {
            get => _stateManager;
            set
            {
                if (_stateManager != value)
                {
                    _stateManager = value;

                }
            }
        }


        // 防止重复UI更新的标志位
        public bool _isUpdatingUIStates = false;
        private BaseEntity _boundEntity;

        /// <summary>
        /// 绑定的实体对象
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseEntity BoundEntity
        {
            get { return _boundEntity; }
            set
            {
                if (_boundEntity != value)
                {
                    // 移除对旧实体的事件监听
                    if (_boundEntity != null)
                    {
                        UnsubscribeEntityEvents(_boundEntity);
                    }

                    _boundEntity = value;

                    // 添加对新实体的事件监听
                    if (_boundEntity != null)
                    {
                        SubscribeEntityEvents(_boundEntity);
                    }
                    else
                    {
                        UpdateAllUIStates(null);
                    }
                }
            }
        }



        #endregion

        #region 初始化

        /// <summary>
        /// 初始化状态管理
        /// 统一处理所有状态管理相关的初始化工作，子类可以重写以添加特定逻辑
        /// </summary>
        protected virtual void InitializeStateManagement()
        {
            // 防止重复初始化
            if (_isStateManagementInitialized) return;

            try
            {
                // 添加设计模式检测，避免在设计器中运行时出现空引用异常
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                {
                    return;
                }

                // 从服务容器获取状态管理器和UI控制器
                if (Startup.ServiceProvider != null)
                {
                    _stateManager = Startup.GetFromFac<IUnifiedStateManager>();
                }

                // 错误处理和日志记录
                if (_stateManager == null)
                {
                    logger?.LogWarning("无法从DI容器获取IUnifiedStateManager服务，请确保在Startup.cs中调用了builder.AddStateManager()");
                    System.Diagnostics.Debug.WriteLine("无法从DI容器获取IUnifiedStateManager服务");
                }

                // 标记已初始化
                _isStateManagementInitialized = true;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "初始化状态管理失败");
                System.Diagnostics.Debug.WriteLine($"初始化状态管理失败: {ex.Message}");
            }
        }

        #endregion

        #region 状态管理

        /// <summary>
        /// 绑定实体对象
        /// 简化版：直接设置BoundEntity属性，触发UI更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void BindEntity(BaseEntity entity)
        {
            BoundEntity = entity;
        }

        /// <summary>
        /// 解绑实体对象
        /// 简化版：清除BoundEntity引用
        /// </summary>
        public void UnbindEntity()
        {
            BoundEntity = null;
        }

 


        /// <summary>
        /// 更新UI（带参数版本）
        /// 统一的UI更新方法，确保UI状态与实体状态同步
        /// </summary>
        /// <param name="entity">实体对象</param>
        public virtual void UpdateAllUIStates(BaseEntity entity)
        {
 
        }



        /// <summary>
        /// 获取控件及其所有子控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <returns>控件列表</returns>
        protected virtual IEnumerable<Control> GetAllControls(Control parent) =>
            parent.Controls.Cast<Control>()
                .SelectMany(control => new[] { control }.Concat(GetAllControls(control)));

        /// <summary>
        /// 获取当前控件及其所有子控件
        /// </summary>
        /// <returns>控件列表</returns>
        protected virtual IEnumerable<Control> GetAllControls() =>
            GetAllControls(this);



        #endregion



        #endregion



        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }



        #region 如果窗体，有些按钮不用出现在这个业务窗体时。这里手动排除。集合有值才行

        List<MenuItemEnums> _excludeMenuList = new List<MenuItemEnums>();
        public List<MenuItemEnums> ExcludeMenuList { get => _excludeMenuList; set => _excludeMenuList = value; }

        List<string> _excludeMenuTextList = new List<string>();
        public List<string> ExcludeMenuTextList { get => _excludeMenuTextList; set => _excludeMenuTextList = value; }


        public virtual void AddExcludeMenuList(string menuItemText)
        {
            if (!ExcludeMenuTextList.Contains(menuItemText))
            {
                ExcludeMenuTextList.Add(menuItemText);
            }
        }

        /// <summary>
        /// 如果查询窗体，有些按钮不用出现在这个业务窗体时。这里手动排除
        /// </summary>
        /// <returns></returns>
        public virtual void AddExcludeMenuList()
        {

        }

        /// <summary>
        /// 使用表达式树配置列映射
        /// </summary>
        //public void AddExcludeMenu<MenuItemEnums>(Expression<Func<MenuItemEnums, MenuItemEnums>> excludeItemExpression)
        //{
        //    var excludeItem = excludeItemExpression.GetMemberInfo().Name;
        //    if (!ExcludeMenuList.Contains(excludeItem))
        //    {
        //        ExcludeMenuList.Add(propertyName);
        //    }
        //}

        public virtual void AddExcludeMenuList(MenuItemEnums menuItem)
        {
            if (!ExcludeMenuList.Contains(menuItem))
            {
                ExcludeMenuList.Add(menuItem);
            }
        }
        #endregion


        #region 相反，如果就一两个生效，我要手动指定设置菜单，那么不在这里指定的，则失效.这个优先处理,如果集合大于0，有值时

        List<MenuItemEnums> _includedMenuList = new List<MenuItemEnums>();
        public List<MenuItemEnums> IncludedMenuList { get => _includedMenuList; set => _includedMenuList = value; }
        /// <summary>
        /// 如果查询窗体，有些按钮不用出现在这个业务窗体时。这里手动排除
        /// </summary>
        /// <returns></returns>
        public virtual void AddIncludedMenuList()
        {

        }

        public virtual void AddIncludedMenuList(MenuItemEnums menuItem)
        {
            IncludedMenuList.Add(menuItem);
        }
        #endregion
        public virtual Task<bool> LockBill()
        {
            // 默认返回未锁定状态
            return Task.FromResult(false);
        }

        public virtual void UNLock(bool NeedUpdateUI = false)
        {

        }

        public virtual void RequestUnLock()
        {

        }

        public virtual void ShowPrintStatus(KryptonLabel lblPrintStatus, BaseEntity entity)
        {
            //可以修改
            if (entity.ContainsProperty(typeof(PrintStatus).Name))
            {
                PrintStatus printStatus = (PrintStatus)int.Parse(entity.GetPropertyValue(typeof(PrintStatus).Name).ToString());
                switch (printStatus)
                {
                    case PrintStatus.未打印:
                        lblPrintStatus.Text = "未打印";
                        break;
                    default:
                        // lblPrintStatus.Text = "已打印";
                        lblPrintStatus.Text = $"打印{entity.GetPropertyValue(typeof(PrintStatus).Name).ToString()}次";
                        break;
                }
            }
        }



        private void bwRemoting_progressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bwRemoting_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 检查上传是否成功
            if (e.Result is bool && (bool)e.Result)
            {
                // 上传成功，关闭窗体
                //this.Close();
            }
            else
            {
                // 上传失败，提示用户
                MessageBox.Show("上传失败，请重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void bwRemoting_DoWork(object sender, DoWorkEventArgs e)
        {
            // 在这里执行上传操作
            // 例如，使用 FTP 上传文件
            bool uploadSuccess = UploadFile((string)e.Argument);
            e.Result = uploadSuccess;
        }

        private bool UploadFile(string filePath)
        {
            try
            {
                // 这里添加你的上传逻辑，例如使用 FTP 客户端上传文件
                // 如果上传成功返回 true，否则返回 false
                return true; // 假设上传成功
            }
            catch (Exception ex)
            {
                // 处理异常
                MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
 


        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            string MenuKey = sender.ToString();
            if (MenuKey.Length > 0)
            {

                if (MenuKey.Contains("已锁定"))
                {
                    MenuKey = "已锁定";
                }
                bool isInEnum = Enum.IsDefined(typeof(MenuItemEnums), MenuKey);
                if (isInEnum)
                {
                    DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(MenuKey));
                }
            }
        }

        protected virtual void ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = e.ClickedItem.Text.ToString();
            if (e.ClickedItem.Text.Length > 0)
            {
                bool isInEnum = Enum.IsDefined(typeof(MenuItemEnums), e.ClickedItem.Text);
                if (isInEnum)
                {
                    DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(e.ClickedItem.Text));
                }
            }
        }

        protected virtual void DoButtonClick(MenuItemEnums menuItem)
        {

            // 根据菜单项执行相应操作
            switch (menuItem)
            {
                case MenuItemEnums.新增:
                    Add();
                    break;
                case MenuItemEnums.修改:
                    Modify();
                    break;
                case MenuItemEnums.删除:
                    // 删除操作需要特殊处理
                    break;
                case MenuItemEnums.保存:
                    // 防止重复点击：立即禁用保存按钮
                    var saveButton = toolStripButtonSave;
                    bool wasEnabled = saveButton?.Enabled ?? true;
                    if (saveButton != null)
                    {
                        saveButton.Enabled = false;
                    }
                    
                    try
                    {
                        // 保存操作是异步的
                        _ = Save(true);
                    }
                    finally
                    {
                        // 确保按钮恢复可用状态
                        if (saveButton != null)
                        {
                            saveButton.Enabled = wasEnabled;
                        }
                    }
                    break;
                case MenuItemEnums.提交:
                    // 提交操作是异步的
                    _ = Submit();
                    break;
                case MenuItemEnums.审核:
                    // 审核操作是异步的
                    _ = Review();
                    break;
                case MenuItemEnums.反审:
                    // 反审核操作是异步的
                    _ = ReReview();
                    break;
                case MenuItemEnums.结案:
                    // 结案操作是异步的
                    _ = CloseCaseAsync();
                    break;
                case MenuItemEnums.反结案:
                    // 反结案操作是异步的
                    _ = AntiCloseCaseAsync();
                    break;
                case MenuItemEnums.查询:
                    Query();
                    break;
                case MenuItemEnums.刷新:
                    Refreshs();
                    break;
                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.属性:
                    Property();
                    break;
                case MenuItemEnums.数据特殊修正:
                    SpecialDataFix();
                    break;
                default:
                    // 其他操作
                    break;
            }
        }

        public class Result
        {
            public bool IsEqual { get; set; }
            public string Msg { get; set; }

        }





        #region 定义所有工具栏的方法 参数在这一级能确认的。可以定义 在这，在下一级的。就不在这定义

        protected virtual void Clear(SourceGridDefine sgd)
        {

        }

        /// <summary>
        /// 反结案
        /// </summary>
        protected virtual Task<bool> AntiCloseCaseAsync()
        {
            return null;
        }


        /// <summary>
        /// 结案
        /// </summary>
        protected virtual Task<bool> CloseCaseAsync()
        {
            return null;
        }

        protected virtual void Add()
        {

        }
        protected virtual void SpecialDataFix()
        {

        }

        protected virtual void Modify()
        {

        }


        protected async virtual Task<bool> Submit()
        {
            await Task.Delay(0);
            return false;
        }

        protected virtual void Query()
        {

        }
        //protected virtual void Print()
        //{
        //    //https://www.cnblogs.com/westsoft/p/8594379.html  三联单
        //    RptPrintConfig config = new RptPrintConfig();
        //    config.ShowDialog();
        //}

        //protected virtual void PrintDesigned(List<object> main, List<object> sub, string fileName)
        //{
        //    List<Category> FBusinessObject;
        //    FastReport.Report FReport;
        //    FReport = new FastReport.Report();

        //    //  FReport.RegisterData(FBusinessObject, "Categories");
        //    FReport.Design();
        //}
        //protected virtual void PrintDesigned()
        //{
        //    //RptDesignForm frm = new RptDesignForm();
        //    //frm.ReportTemplateFile = "SOB.frx";
        //    //frm.ShowDialog();
        //    ////List<Category> FBusinessObject;
        //    ////FastReport.Report FReport;
        //    ////FReport = new FastReport.Report();

        //    //////  FReport.RegisterData(FBusinessObject, "Categories");
        //    ////FReport.Design();

        //}

        /// <summary>
        /// 暂时只支持一级审核，将来可以设计配置 可选多级审核。并且能看到每级的审核情况
        /// </summary>
        protected async virtual Task<ReviewResult> Review()
        {
            await Task.Delay(0);
            MessageBox.Show("应该有选项 同意和同意，原因？");
            return null;
        }

        protected async virtual Task<bool> ReReview()
        {
            await Task.Delay(0);
            return false;
        }
        protected virtual void Property()
        {
            //RptDesignForm frm = new RptDesignForm();
            //frm.ReportTemplateFile = "SOB.frx";
            //frm.ShowDialog();
        }

        //errorProviderForAllInput 引用这个控件。使用在保存时直接触发所有控件的验证来更新数据源
        private bool hasError = false;

        /// <summary>
        /// 验证控件UI层次
        ///<param name="NeedValidated">是否需要验证，缓存保存不要验证，正常时要</param>
        /// </summary>
        protected virtual async Task<bool> Save(bool NeedValidated)
        {
            await Task.Delay(1);
            return false;
            //   errorProviderForAllInput.Clear();
        }

        protected virtual void Exit(object thisform)
        {
            if (!Edited)
            {
                //退出
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //退出
                    CloseTheForm(thisform);
                }
            }
        }

        protected virtual void Refreshs()
        {

        }

        /// <summary>
        /// 传实体进去,具体在窗体那边判断    单据实体数据传入加载用
        /// </summary>
        /// <param name="LoadItem">要加载的实体对象</param>
        internal virtual void LoadDataToUI(object LoadItem)
        {
            // 如果加载的是BaseEntity类型的对象，则根据其状态更新UI
            if (LoadItem is BaseEntity entity)
            {
                // 设置加载状态和主键ID（统一在这里处理）
                entity.ActionStatus = ActionStatus.加载;
            }
        }


        #endregion

        internal virtual void CloseTheForm(object thisform)
        {
            try
            {
                KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
                if (cell == null)
                {
                    cell = new KryptonWorkspaceCell();
                    MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
                }
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
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
            catch (Exception)
            {


            }

        }

 
        private Type _EditForm;
        public Type EditForm { get => _EditForm; set => _EditForm = value; }


        public System.Windows.Forms.BindingSource _ListDataSoure = null;

        [Description("列表中的要显示的数据来源[BindingSource]"), Category("自定属性"), Browsable(true)]
        /// <summary>
        /// 列表的数据源(实际要显示的)
        /// </summary>
        public System.Windows.Forms.BindingSource ListDataSoure
        {
            get { return _ListDataSoure; }
            set
            {
                // 移除旧数据源的事件订阅
                if (_ListDataSoure != null)
                {
                    _ListDataSoure.CurrentChanged -= ListDataSoure_CurrentChanged;
                }

                _ListDataSoure = value;

                // 订阅新数据源的CurrentChanged事件
                if (_ListDataSoure != null)
                {
                    _ListDataSoure.CurrentChanged += ListDataSoure_CurrentChanged;
                }
            }
        }

        /// <summary>
        /// 数据源CurrentChanged事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void ListDataSoure_CurrentChanged(object sender, EventArgs e)
        {
            // 处理当前项变更，直接更新UI状态
            UpdateAllUIStates(ListDataSoure.Current as BaseEntity);
        }

        /// <summary>
        /// 处理实体状态订阅（增强版）
        /// 优化说明：
        /// 1. 统一管理实体状态变更事件订阅，避免重复订阅和内存泄漏
        /// 2. 作为状态管理统一入口，确保所有状态变更都通过此机制处理
        /// 3. 增强异常处理和日志记录，便于调试
        /// 4. 确保无论状态变更来自直接属性修改还是状态管理器，都能正确响应
        /// 5. 防止重复UI更新和潜在的性能问题
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="subscribe">true为订阅，false为取消订阅</param>
        public void HandleEntityStatusSubscription(BaseEntity entity, bool subscribe)
        {
            if (entity == null)
                return;

            try
            {
                // 先取消订阅，避免重复订阅问题
                entity.StatusChanged -= OnEntityStatusChanged;
                // 如果需要订阅，则添加事件处理程序
                if (subscribe)
                {
                    entity.StatusChanged += OnEntityStatusChanged;
                }
            }
            catch (Exception ex)
            {
                // 增强异常处理和日志记录
                Debug.WriteLine($"HandleEntityStatusSubscription错误: {ex.Message}");
                Debug.WriteLine($"实体类型: {entity.GetType().Name}");
                Debug.WriteLine($"操作类型: {(subscribe ? "订阅" : "取消订阅")}");
                // 不抛出异常，确保流程继续执行
            }
        }

        /// <summary>
        /// 订阅实体的状态变更事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected void SubscribeEntityEvents(BaseEntity entity)
        {
            HandleEntityStatusSubscription(entity, true);
        }

        /// <summary>
        /// 取消订阅实体的状态变更事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected void UnsubscribeEntityEvents(BaseEntity entity)
        {
            HandleEntityStatusSubscription(entity, false);
        }

        /// <summary>
        /// 实体状态变更事件处理程序
        /// 高效版：直接更新UI状态，避免冗余操作
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnEntityStatusChanged(object sender, StateTransitionEventArgs e)
        {
            // 安全检查
            if (e?.Entity == null) return;

            // 检查状态是否真正发生变化（性能优化）
            if (e.OldStatus?.Equals(e.NewStatus) ?? false)
            {
                return;
            }

            try
            {
                // 直接使用异步更新UI状态，避免阻塞主线程
                UpdateUIStatesAsync(e.Entity as BaseEntity);
            }
            catch (Exception ex)
            {
                // 增强的异常日志记录
                Debug.WriteLine($"状态变更事件处理失败: {ex.Message}");
                logger?.LogError(ex, "状态变更事件处理失败");
            }
        }

        /// <summary>
        /// 异步更新UI状态，线程安全且高效（增强版）
        /// 优化UI更新流程，确保在各种情况下安全更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void UpdateUIStatesAsync(BaseEntity entity)
        {
            // 使用BeginInvoke确保UI线程安全，同时不阻塞调用线程
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                this.BeginInvoke((Action)(() =>
                {
                    // 二次检查，防止在调用过程中窗体已关闭
                    if (!this.IsDisposed && !this.Disposing)
                    {
                        try
                        {
                            UpdateAllUIStates(entity);
                        }
                        catch (Exception ex)
                        {
                            // 捕获UI更新过程中的异常
                            Debug.WriteLine($"UI状态更新失败: {ex.Message}");
                            logger?.LogError(ex, "UI状态更新失败");
                        }
                    }
                }));
            }
        }


        private bool editflag;

        /// <summary>
        /// 是否为编辑 如果为是则true
        /// </summary>
        public bool Edited
        {
            get { return editflag; }
            set { editflag = value; }
        }

        protected Result ComPare<T>(T t, T s)
        {
            Result result = new Result();
            var comparer = new ObjectsComparer.Comparer<T>();
            IEnumerable<ObjectsComparer.Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                string differencesMsg = string.Join(Environment.NewLine, differences);
                result.Msg = differencesMsg;
            }
            return result;
        }



        public System.Threading.Timer _timeoutTimer4tips;

        //public delegate void ShowHelp();

        //[System.ComponentModel.Description("提示帮助事件")]
        //public event ShowHelp OnShowHelp;


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

        /// <summary>
        /// 处理命令键，包括F1帮助和Esc退出
        /// </summary>
        /// <param name="msg">消息对象</param>
        /// <param name="keyData">按键数据</param>
        /// <returns>是否处理了命令键</returns>
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
                        //this.Close();//csc关闭窗体
                        return true;
                    case Keys.F1:
                        // 使用新的帮助系统显示当前控件的帮助
                        // 获取当前焦点控件，支持Krypton控件
                        Control focusedControl = RUINORERP.UI.HelpSystem.Extensions.KryptonHelpExtensions.GetActualFocusedControl(this);
                        if (focusedControl != null)
                        {
                            HelpManager.Instance.ShowControlHelp(focusedControl);
                            return true;
                        }
                        break;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

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
                                bsa.Image = Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                bsa.Tag = ktb;
                                bsa.Click += Bsa_Click;
                                ktb.ButtonSpecs.Add(bsa);
                            }
                        }
                    }
                }

            }
        }



        private void Bsa_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonSpecAny bsa = sender as ButtonSpecAny;
                if (bsa == null) return;

                Control targetControl = bsa.Owner as Control;
                if (targetControl == null) return;

                // 使用新的帮助系统
                var context = HelpContext.FromControl(targetControl);
                HelpManager.Instance.ShowHelp(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示控件帮助失败: {ex.Message}");
            }
        }




        public void ProcessHelpInfo(bool fromBtn, object sender)
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
                if (ActiveControl.GetType().ToString() == "ComponentFactory.Krypton.Toolkit.KryptonTextBox+InternalTextBox")
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

                //                var obj = Startup.AutoFacContainer.ResolveNamed<BaseEntity>(className);
                var obj = Startup.GetFromFacByName<BaseEntity>(className);
                if (obj.HelpInfos.ContainsKey(filedName))
                {
                    tipTxt = "【" + obj.FieldNameList[filedName].Trim() + "】";
                    tipTxt += obj.HelpInfos[filedName].ToString();
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



        private System.Collections.Concurrent.ConcurrentDictionary<string, string> fieldNameList;

        private void BaseBillEdit_Load(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    logger = MainForm.Instance.logger;
                    AppContext = MainForm.Instance.AppContext;
                    #region 


                    //设置其闪烁样式
                    //BlinkIfDifferentError 当图标已经显示并且为控件设置了新的错误字符串时闪烁。
                    //AlwaysBlink 总是闪烁。
                    //NeverBlink 错误图标从不闪烁。
                    errorProviderForAllInput.BlinkStyle = ErrorBlinkStyle.NeverBlink;

                    //错误图标的闪烁速率（以毫秒为单位）。默认为 250 毫秒
                    errorProviderForAllInput.BlinkRate = 1000;
                    if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                    {
                        if (!this.DesignMode)
                        {
                            //实际已经在菜单点击时已经传入了正确的值 MenuHelper.cs
                            if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                            {
                                CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.FormName == this.GetType().Name && m.ClassPath == this.ToString()).FirstOrDefault();
                                if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                                {
                                    MessageBox.Show(this.ToString() + "菜单有显示，但无关联数据，请联系管理员。");
                                    return;
                                }
                            }

                            AddExcludeMenuList();
                   
                            foreach (var item in BaseToolStrip.Items)
                            {
                                if (item is ToolStripButton)
                                {
                                    ToolStripButton subItem = item as ToolStripButton;
                                    subItem.Click += Item_Click;
                                    UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, subItem, ExcludeMenuList);
                                }
                                if (item is ToolStripDropDownButton)
                                {
                                    ToolStripDropDownButton subItem = item as ToolStripDropDownButton;
                                    UIHelper.ControlButton<ToolStripDropDownButton>(CurMenuInfo, subItem);
                                    subItem.Click += Item_Click;
                                    //下一级
                                    if (subItem.HasDropDownItems)
                                    {
                                        foreach (var sub in subItem.DropDownItems)
                                        {
                                            ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                                            UIHelper.ControlButton<ToolStripMenuItem>(CurMenuInfo, subStripMenuItem, ExcludeMenuList);
                                            subStripMenuItem.Click += Item_Click;
                                        }
                                    }
                                }
                                if (item is ToolStripSplitButton)
                                {
                                    ToolStripSplitButton subItem = item as ToolStripSplitButton;
                                    subItem.Click += Item_Click;
                                    //下一级
                                    if (subItem.HasDropDownItems)
                                    {
                                        foreach (var sub in subItem.DropDownItems)
                                        {
                                            ToolStripItem subStripMenuItem = sub as ToolStripItem;
                                            subStripMenuItem.Click += Item_Click;
                                            UIHelper.ControlButton<ToolStripItem>(CurMenuInfo, subStripMenuItem, ExcludeMenuList);
                                        }
                                    }
                                }

                            }
                        }
                    }


                    #endregion

                }
            }


        }



        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="isEditable">是否可编辑</param>
        protected void SetControlsState(Control.ControlCollection controls, bool isEditable)
        {
            foreach (Control control in controls)
            {
                // 设置控件的只读状态
                SetControlReadOnly(control, isEditable);

                // 递归处理子控件
                if (control.HasChildren)
                {
                    SetControlsState(control.Controls, isEditable);
                }
            }
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="isEditable">是否可编辑</param>
        private void SetControlReadOnly(Control control, bool isEditable)
        {
            if (control is KryptonTextBox kryptonTextBox)
            {
                kryptonTextBox.ReadOnly = !isEditable;
            }
            else if (control is TextBox textBox)
            {
                textBox.ReadOnly = !isEditable;
            }
            else if (control is KryptonComboBox kryptonComboBox)
            {
                kryptonComboBox.Enabled = isEditable;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.Enabled = isEditable;
            }
            else if (control is KryptonCheckBox kryptonCheckBox)
            {
                kryptonCheckBox.Enabled = isEditable;
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.Enabled = isEditable;
            }
            else if (control is KryptonDateTimePicker kryptonDateTimePicker)
            {
                kryptonDateTimePicker.Enabled = isEditable;
            }
            else if (control is DateTimePicker dateTimePicker)
            {
                dateTimePicker.Enabled = isEditable;
            }
            else if (control is KryptonNumericUpDown kryptonNumericUpDown)
            {
                kryptonNumericUpDown.Enabled = isEditable;
            }
            else if (control is NumericUpDown numericUpDown)
            {
                numericUpDown.Enabled = isEditable;
            }
            else if (control is Button button)
            {
                // 根据按钮类型和业务逻辑决定是否启用
                // 这里可以根据实际需求进行调整
                button.Enabled = isEditable;
            }
        }

    }
}

