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
using RUINORERP.UI.Common;
using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Model.TransModel;
using System.Threading;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;
using System.Web.UI;
using Control = System.Windows.Forms.Control;
using RUINORERP.Model.Base.StatusManager;
using UserControl = System.Windows.Forms.UserControl;

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

        }



        #region 状态控件的所有代码移植自 StateAwareControl 中

        #region 字段

        /// <summary>
        /// v3统一状态管理器
        /// </summary>
        public IUnifiedStateManager _stateManager;

        /// <summary>
        /// v3状态上下文
        /// </summary>
        private IStatusTransitionContext _statusContext;
      
        
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



     


        /// <summary>
        /// 绑定的实体对象
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseEntity BoundEntity { get; set; }

        /// <summary>
        /// v3状态上下文
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IStatusTransitionContext StatusContext
        {
            get => _statusContext;
            set
            {
                if (_statusContext != value)
                {
                    _statusContext = value;
                    // 状态上下文变更时不再需要特殊处理，状态变更事件由UnifiedStateManager统一管理
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
        /// </summary>
        /// <param name="entity">实体对象</param>
        public virtual void BindEntity(BaseEntity entity)
        {
            if (entity == null)
            {
                UnbindEntity();
                return;
            }

            BoundEntity = entity;
            InitializeStatusContext(entity);
         
            UpdateAllUIStates(entity);
        }

        /// <summary>
        /// 初始化状态上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void InitializeStatusContext(BaseEntity entity)
        {
            try
            {
                // 优先使用状态管理器创建并初始化状态上下文
                if (_stateManager != null)
                {
                    // 使用状态管理器创建状态上下文
                    var dataStatus = _stateManager.GetDataStatus(entity);
                    StatusContext = _stateManager.CreateDataStatusContext(entity, dataStatus, Startup.ServiceProvider);
                }
                else if (StatusContext == null)
                {
                    // 备选方案：创建基本状态上下文
                    // 这种情况理论上不应该发生，因为_stateManager应该总是可用
                    logger?.LogWarning("状态管理器不可用，无法创建状态上下文");
                    StatusContext = null;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "初始化实体状态上下文失败");
                System.Diagnostics.Debug.WriteLine($"初始化实体状态上下文失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 解绑实体对象
        /// </summary>
        public virtual void UnbindEntity()
        {
            BoundEntity = null;
            StatusContext = null;
        }
       

        /// <summary>
        /// 更新状态栏显示
        /// </summary>
        protected virtual void UpdateStatusDisplay()
        {
            try
            {
                // 尝试查找并更新状态标签
                var statusLabel = this.Controls.Find("lblStatus", true).FirstOrDefault() as KryptonLabel;
                if (statusLabel != null && StatusContext?.CurrentStatus is DataStatus currentStatus)
                {
                    // 使用Description属性获取显示名称，因为DataStatus没有GetDisplayName方法
                    var fieldInfo = typeof(DataStatus).GetField(currentStatus.ToString());
                    var descriptionAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    string displayName = descriptionAttribute?.Description ?? currentStatus.ToString();
                    statusLabel.Text = $"状态: {displayName}";
                    statusLabel.ForeColor = GetStatusColor(currentStatus);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新状态栏显示失败");
            }
        }

        /// <summary>
        /// 统一更新所有UI状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void UpdateAllUIStates(BaseEntity entity)
        {
            if (entity == null) return;

            // 确保状态上下文已初始化
            if (StatusContext == null)
            {
                InitializeStatusContext(entity);
            }

            // 获取当前状态，优先使用状态管理器，回退到状态上下文
            DataStatus currentStatus;
            if (_stateManager != null)
            {
                currentStatus = _stateManager.GetDataStatus(entity);
            }
            else if (StatusContext?.CurrentStatus is DataStatus status)
            {
                currentStatus = status;
            }
            else
            {
                // 回退方案：直接从实体获取状态
                currentStatus = entity.GetDataStatus();
            }

      

            // 更新UI控件状态
            UpdateUIControlsByState(currentStatus);

            // 更新状态显示
            UpdateStatusDisplay();

            // 更新打印状态显示
            UpdatePrintStatusDisplay(entity);
        }

        /// <summary>
        /// 统一更新打印状态显示
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void UpdatePrintStatusDisplay(BaseEntity entity)
        {
            // 尝试查找打印状态标签控件
            var printStatusLabel = this.Controls.Find("lblPrintStatus", true).FirstOrDefault() as KryptonLabel;
            if (printStatusLabel != null)
            {
                ShowPrintStatus(printStatusLabel, entity);
            }
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

        #region 状态转换

        /// <summary>
        /// 执行状态转换
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public virtual async Task<StateTransitionResult> TransitionToAsync(DataStatus targetStatus, string reason = "")
        {
            if (StatusContext == null)
                return StateTransitionResult.Failure("状态上下文未初始化");

            try
            {
                var result = await StatusContext.TransitionTo(targetStatus, reason);

                if (result.IsSuccess)
                {
                    return StateTransitionResult.Success();
                }

                return StateTransitionResult.Failure($"转换到数据状态失败: {targetStatus}");
            }
            catch (Exception ex)
            {
                return StateTransitionResult.Failure($"状态转换失败: {ex.Message}");
            }
        }

    
        #endregion

        #endregion






        /// <summary>
        /// 处理特定的状态变更逻辑（子类可重写）
        /// 注意：推荐子类重写异步版本HandleSpecificStateChangeAsync
        /// </summary>
        /// <param name="originalState">原始状态</param>
        /// <param name="newState">新状态</param>
        /// <param name="reason">变更原因</param>
        protected virtual void HandleSpecificStateChange(object originalState, object newState, string reason)
        {
            // 子类可以重写此方法以处理特定的状态变更逻辑
            // 例如：销售订单在确认状态时检查库存，在完结状态时生成出库单等
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }



        /// <summary>
        /// 根据状态获取颜色
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>颜色</returns>
        protected virtual Color GetStatusColor(DataStatus status)
        {
            return status switch
            {
                DataStatus.草稿 => Color.Gray,
                DataStatus.新建 => Color.Blue,
                DataStatus.确认 => Color.Green,
                DataStatus.完结 => Color.DarkGreen,
                DataStatus.作废 => Color.Red,
                _ => Color.Black
            };
        }




        /// <summary>
        /// 根据状态更新UI控件（子类可重写）
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        protected virtual void UpdateUIControlsByState(DataStatus currentStatus)
        {
            try
            {
                // 使用UIControlRules获取按钮状态规则
                var buttonRules = RUINORERP.Model.Base.StatusManager.UIControlRules.GetButtonRules(currentStatus);
                
                // 根据规则更新按钮状态
                foreach (var rule in buttonRules)
                {
                    var control = this.Controls.Find(rule.Key, true).FirstOrDefault();
                    if (control != null)
                    {
                        //control.Enabled = rule.Value.Enabled;
                        //control.Visible = rule.Value.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新UI控件状态失败: {Message}", ex.Message);
            }
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

        protected virtual void UpdateAllButtonStates(DataStatus currentStatus)
        {

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

        public tb_MenuInfo CurMenuInfo { get; set; }


        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            if (sender.ToString().Length > 0)
            {
                bool isInEnum = Enum.IsDefined(typeof(MenuItemEnums), sender.ToString());
                if (isInEnum)
                {
                    DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
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


            //if (!CanTransitionToDataStatusAsync(menuItem))
            //{
            //    MessageBox.Show($"当前状态下不允许执行 {menuItem} 操作", "操作受限", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

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
                    // 保存操作是异步的
                    _ = Save(true);
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




        //protected virtual Task<ReturnResults<T>> Delete()
        //{
        //    return Task.FromResult(new ReturnResults<bool>(false));
        //}

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


                // 确保实体已初始化状态管理器
                if (!entity.IsStateManagerInitialized)
                {
                    entity.InitializeStateManager();
                }

                // 根据实体状态更新UI
                UpdateAllUIStates(entity);
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


        ///// <summary>
        ///// 表列名的中文描述集合
        ///// </summary>
        //[Description("表列名的中文描述集合"), Category("自定属性"), Browsable(true)]
        //public System.Collections.Concurrent.ConcurrentDictionary<string, string> FieldNameList
        //{
        //    get
        //    {
        //        return fieldNameList;
        //    }
        //    set
        //    {
        //        fieldNameList = value;
        //    }

        //}
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
            // 处理当前项变更，订阅实体的StatusChanged事件
            SubscribeToEntityStatusChanged(ListDataSoure.Current as BaseEntity);
        }

        /// <summary>
        /// 订阅实体的StatusChanged事件 - 使用V3状态管理系统优化版本
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected void SubscribeToEntityStatusChanged(BaseEntity entity)
        {
            if (entity == null) return;
            
            // 防止重复订阅，先取消再订阅
            entity.StatusChanged -= OnEntityStatusChanged;
            entity.StatusChanged += OnEntityStatusChanged;
            
            // 确保状态上下文已初始化
            EnsureStatusContext(entity);
        }
        
        /// <summary>
        /// 实体状态变更事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnEntityStatusChanged(object sender, StateTransitionEventArgs e)
        {
            try
            {
                // 当实体状态变更时，更新UI
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateAllUIStates(e.Entity as BaseEntity)));
                }
                else
                {
                    UpdateAllUIStates(e.Entity as BaseEntity);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理实体状态变更事件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 确保状态上下文被正确初始化
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected void EnsureStatusContext(BaseEntity entity)
        {
            if (entity == null) return;

            // 如果StatusContext为null，尝试从实体中获取
            if (StatusContext == null)
            {
                if (BoundEntity == null)
                {
                    BoundEntity = entity;
                }
                StatusContext = entity.StatusContext;
            }

            // 如果仍然为null，尝试重新初始化
            if (StatusContext == null && StateManager != null)
            {
                try
                {
                    // 获取实体当前的数据状态
                    var currentDataStatus = StateManager.GetDataStatus(entity);
                    var ServiceProvider = Startup.GetFromFac<IServiceProvider>();
                    // 使用状态管理器重新创建状态上下文
                    StatusContext = StateManager.CreateDataStatusContext(entity, currentDataStatus, ServiceProvider);
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "重新初始化状态上下文失败: {ex.Message}", ex);
                }
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
                        //this.Close();//csc关闭窗体
                        break;
                    case Keys.F1:
                        if (toolTipBase.Active)
                        {
                            //if (OnShowHelp != null)
                            //{

                            //OnShowHelp();
                            ProcessHelpInfo(false, null);
                            //}
                        }
                        break;
                }

            }
            return false;
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
            ProcessHelpInfo(true, sender);
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


                    // 获取按钮状态
                    //var printState = uiManager.GetButtonState(MenuItemEnums.打印);
                    //var exportState = uiManager.GetButtonState(MenuItemEnums.导出);

                    //// 在工具栏初始化时
                    //foreach (MenuItemEnums item in Enum.GetValues(typeof(MenuItemEnums)))
                    //{
                    //    var state = uiManager.GetButtonState(item);
                    //    toolStrip.Items.Add(new ToolStripButton
                    //    {
                    //        Text = item.ToString(),
                    //        Enabled = state.Enabled,
                    //        Visible = state.Visible
                    //    });
                    //}


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
                                //给按钮tag给个值。有什么意义？ 就为了后面注册状态用？用Text直接判断就可以了。
                                //foreach (MenuItemEnums menuItem in Enum.GetValues(typeof(MenuItemEnums)))
                                //{
                                //    var button = new ToolStripButton
                                //    {
                                //        Text = item.ToString(),
                                //        Tag = item // 关键：设置Tag属性为枚举值
                                //    };
                                //}


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
