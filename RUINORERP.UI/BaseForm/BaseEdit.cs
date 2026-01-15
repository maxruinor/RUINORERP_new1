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
using FluentValidation;
using RUINORERP.Business;
using System.Text.RegularExpressions;
using RUINORERP.Global.CustomAttribute;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.UI.HelpSystem.Core;
using RUINORERP.UI.HelpSystem.Extensions;
using RUINORERP.UI.HelpSystem.Components;

namespace RUINORERP.UI.BaseForm
{
    [PreCheckMustOverrideBaseClass]
    public partial class BaseEdit : KryptonForm
    {


        public BaseEdit()
        {
            InitializeComponent();
            //this.KeyPreview = true;
            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BaseEdit_KeyPress);
            
            // 初始化帮助系统
            InitializeHelpSystem();
        }


        [MustOverride]
        public virtual void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        //public virtual void BindData<T>(BaseEntity entity)
        //{

        //}


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
                        CloseTheForm(this);
                        break;
                    case Keys.F1:
                        // 使用新的帮助管理器
                        ShowContextHelp();
                        return true;
                }

            }
            return false;
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
            MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
            page.Dispose();
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

                                //可以边框为红色不？
                                //或必填项目有特别提示？
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 编辑对应的外键实体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BsaEdit_Click(object sender, EventArgs e)
        {
            ButtonSpecAny bsa = sender as ButtonSpecAny;
            KryptonComboBox ktb = bsa.Owner as KryptonComboBox;
            #region 找到绑定的字段
            BindingSource bs = new BindingSource();
            bs = ktb.DataSource as BindingSource;
            Binding binding = ktb.DataBindings[0];

            //2023-10-24 可以直接用 T的类型了

            //string fktableName = string.Empty;
            //fktableName = bs.Current.GetType().Name;//这个会出错，current 可能会为空。
            //下面也是取外键表名的代码。复杂
            Regex r = new Regex(@"\[(\w+.+)\]");
            var ms = r.Matches(bs.DataSource.ToString());
            string classObjName = string.Empty;
            foreach (Match NextMatch in ms)
            {
                Console.Write("匹配的位置：{0} ", NextMatch.Index);
                Console.Write("匹配的内容：{0} ", NextMatch.Value);
                char[] charsToTrim = { '[', ']' };
                classObjName = NextMatch.Value.Trim(charsToTrim);
                int pointindex = classObjName.LastIndexOf('.') + 1;
                classObjName = classObjName.Substring(pointindex);
                break;
            }
            #endregion
            string filedName = binding.BindingMemberInfo.BindingField;

            //这里调用权限判断
            //调用通用的查询编辑基础资料。
            //需要对应的类名，如果修改新增了数据要重新加载下拉数据
            tb_MenuInfoController<tb_MenuInfo> mc = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
            List<tb_MenuInfo> menuList = mc.Query();
            tb_MenuInfo menuinfo = menuList.FirstOrDefault(t => t.EntityName == classObjName.ToString());
            if (menuinfo == null)
            {
                MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，或配置菜时参数不正确。请联系管理员。");
                return;
            }
            UserControl menu = Startup.GetFromFacByName<UserControl>(menuinfo.FormName);
            //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
            frmBaseEditList frmedit = new frmBaseEditList();
            frmedit.StartPosition = FormStartPosition.CenterScreen;
            menu.Dock = DockStyle.Fill;
            frmedit.kryptonPanel1.Controls.Add(menu);
            if (frmedit.ShowDialog() == DialogResult.OK)
            {
                BaseForm.BaseList bl = menu as BaseForm.BaseList;
                string ucTypeName = bsa.Owner.GetType().Name;
                if (ucTypeName == "KryptonComboBox")
                {
                    //选中的值，一定要在重新加载前保存，下面会清空重新加载会变为第一个项
                    object obj = bl.bindingSourceList.Current;
                    //重新加载
                    object entity = binding.DataSource;
                    Common.DataBindingHelper.BindData4Cmb(bl.ListDataSoure, entity, ktb.ValueMember, ktb.DisplayMember, ktb);
                    ktb.SelectedItem = obj;

                    //因为选择中 实体数据并没有更新，下面两行是将对象对应的属性给一个选中的值。
                    object selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, ktb.ValueMember);
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(entity, ktb.ValueMember, selectValue);

                }
            }
        }


        private void Bsa_Click(object sender, EventArgs e)
        {
            // 如果启用了智能帮助系统,使用新系统
            if (EnableSmartHelp)
            {
                ButtonSpecAny bsa = sender as ButtonSpecAny;
                KryptonTextBox ktb = bsa.Owner as KryptonTextBox;
                ShowControlHelp(ktb);
            }
            else
            {
                // 否则使用原有的帮助系统
                ProcessHelpInfo(true, sender);
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
        public void InitRequiredToControl<T>(AbstractValidator<T> rules, System.Windows.Forms.Control.ControlCollection Controls)
        {
            List<string> notEmptyList = new List<string>();
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
        public void InitEditItemToControl(BaseEntity entity, System.Windows.Forms.Control.ControlCollection Controls)
        {
            //思路通过控件的数据源 调试时是产品，对应绑定的关联表的主键及 可的到对应的关联对象实体名。再通过菜单中对应关系找到相差窗体类。
            //https://www.cnblogs.com/GaoUpUp/p/17187770.html
            foreach (var item in Controls)
            {
                if (item is Control)
                {
                    if (item is VisualControlBase)
                    {
                        if (item.GetType().Name == "KryptonComboBox")
                        {
                            KryptonComboBox ktb = item as KryptonComboBox;
                            if ((item as Control).DataBindings.Count > 0)
                            {
                                if (ktb.DataBindings.Count > 0 && ktb.DataSource is BindingSource)
                                {
                                    ButtonSpecAny bsa = new ButtonSpecAny();
                                    bsa.Image = Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                    bsa.Tag = ktb;
                                    bsa.Click += BsaEdit_Click;
                                    ktb.ButtonSpecs.Add(bsa);
                                    //可以边框为红色不？
                                    //或必填项目有特别提示？
                                    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;

                                }



                            }
                        }
                    }
                }
            }
        }

        #endregion



        private void BaseEdit_Load(object sender, EventArgs e)
        {

        }

        #region 帮助系统集成 - 新增功能

        /// <summary>
        /// 帮助管理器实例
        /// </summary>
        protected HelpManager HelpManager => HelpManager.Instance;

        /// <summary>
        /// 是否启用智能帮助
        /// </summary>
        [Category("帮助系统")]
        [DefaultValue(true)]
        [Description("是否启用智能帮助功能")]
        public bool EnableSmartHelp { get; set; } = true;

        /// <summary>
        /// 窗体帮助键(可选,覆盖默认值)
        /// </summary>
        [Category("帮助系统")]
        [Description("窗体帮助键,留空则使用窗体类型名称")]
        public string FormHelpKey { get; set; }

        /// <summary>
        /// 初始化帮助系统
        /// 在窗体构造函数中调用
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

                    // 初始化字段帮助按钮
                    InitFieldHelpButtons();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化帮助系统失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示当前窗体的帮助
        /// </summary>
        public virtual void ShowFormHelp()
        {
            HelpManager.Instance.ShowFormHelp(this);
        }

        /// <summary>
        /// 显示指定控件的帮助
        /// </summary>
        /// <param name="control">控件</param>
        public virtual void ShowControlHelp(Control control)
        {
            HelpManager.Instance.ShowControlHelp(control);
        }

        /// <summary>
        /// 显示字段帮助
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="fieldName">字段名称</param>
        public virtual void ShowFieldHelp(Type entityType, string fieldName)
        {
            HelpManager.Instance.ShowFieldHelp(entityType, fieldName);
        }

        /// <summary>
        /// 显示上下文帮助
        /// 根据当前焦点控件显示相应的帮助
        /// </summary>
        private void ShowContextHelp()
        {
            // 获取当前焦点控件
            Control focusedControl = GetFocusedControl(this);

            if (focusedControl != null)
            {
                // 显示控件帮助
                ShowControlHelp(focusedControl);
            }
            else
            {
                // 显示窗体帮助
                ShowFormHelp();
            }
        }

        /// <summary>
        /// 获取焦点控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <returns>焦点控件</returns>
        private Control GetFocusedControl(Control parent)
        {
            if (parent.Focused)
            {
                return parent;
            }

            foreach (Control child in parent.Controls)
            {
                Control focused = GetFocusedControl(child);
                if (focused != null)
                {
                    return focused;
                }
            }

            return null;
        }

        /// <summary>
        /// 初始化字段帮助按钮
        /// 为绑定的字段添加帮助按钮
        /// </summary>
        protected virtual void InitFieldHelpButtons()
        {
            try
            {
                // 调用原有的帮助信息初始化方法
                InitHelpInfoToControl(this.Controls);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化字段帮助按钮失败: {ex.Message}");
            }
        }



        #endregion


    }
}
