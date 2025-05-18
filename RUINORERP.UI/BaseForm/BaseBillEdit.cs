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
using RUINORERP.UI.Common;
using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.ClientCmdService;
using System.Threading;
using TransInstruction.CommandService;

namespace RUINORERP.UI.BaseForm
{
    public partial class BaseBillEdit : UserControl
    {

        public ApplicationContext AppContext { set; get; }
        public ILogger<MainForm> logger { get; set; }
        public BaseBillEdit()
        {
            InitializeComponent();

            bwRemoting.DoWork += bwRemoting_DoWork;
            bwRemoting.RunWorkerCompleted += bwRemoting_RunWorkerCompleted;
            bwRemoting.ProgressChanged += bwRemoting_progressChanged;
            //如果打开单时。被其它人锁定。才显示锁定图标
            tsBtnLocked.Visible = false;

        }
        #region 如果窗体，有些按钮不用出现在这个业务窗体时。这里手动排除。集合有值才行

        List<MenuItemEnums> _excludeMenuList = new List<MenuItemEnums>();
        public List<MenuItemEnums> ExcludeMenuList { get => _excludeMenuList; set => _excludeMenuList = value; }

        List<string> _excludeMenuTextList = new List<string>();
        public List<string> ExcludeMenuTextList { get => _excludeMenuTextList; set => _excludeMenuTextList = value; }


        public virtual void AddExcludeMenuList(string menuItemText)
        {
            ExcludeMenuTextList.Add(menuItemText);
        }

        /// <summary>
        /// 如果查询窗体，有些按钮不用出现在这个业务窗体时。这里手动排除
        /// </summary>
        /// <returns></returns>
        public virtual void AddExcludeMenuList()
        {

        }

        public virtual void AddExcludeMenuList(MenuItemEnums menuItem)
        {
            ExcludeMenuList.Add(menuItem);
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
        public virtual void LockBill()
        {

        }

        public virtual void UNLock()
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

        public tb_MenuInfo CurMenuInfo { get; set; }


        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            if (sender.ToString().Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
            }
            else
            {

            }

        }

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

        protected virtual void DoButtonClick(MenuItemEnums menuItem)
        {

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


        protected virtual void AddByCopy()
        {

        }

        //protected virtual Task<ReturnResults<T>> Delete()
        //{
        //    return Task.FromResult(new ReturnResults<bool>(false));
        //}

        protected virtual void Modify()
        {

        }

        protected virtual void AdvQuery()
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
        /// <param name="LoadItem"></param>
        internal virtual void LoadDataToUI(object LoadItem)
        {

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
            set { _ListDataSoure = value; }
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
                                            UIHelper.ControlButton<ToolStripMenuItem>(CurMenuInfo, subStripMenuItem);
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
                                            UIHelper.ControlButton<ToolStripItem>(CurMenuInfo, subStripMenuItem);
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
    }
}
