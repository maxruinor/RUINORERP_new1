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
using RUINORERP.UI.Common;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using FastReport.Table;
using Newtonsoft.Json.Linq;
using System.Web.UI;
using Control = System.Windows.Forms.Control;

namespace RUINORERP.UI.BaseForm
{
    [PreCheckMustOverrideBaseClass]
    public partial class BaseEditGeneric<T> : KryptonForm where T : class
    {

        public BaseEditGeneric()
        {
            InitializeComponent();
            //this.KeyPreview = true;
            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BaseEdit_KeyPress);
        }

        public delegate void CancelHandler();

        [Browsable(true), Description("引发外部查询事件")]
        public event CancelHandler CancelEvent;


        [MustOverride]
        public virtual void BindData(BaseEntity entity)
        {

        }

        //public virtual void BindData(T entity)
        //{

        //}
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
                        this.Close();//csc关闭窗体
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
        /// 就是在编辑复杂实体时，需要其他的一些基础资料表的配合。不需要一个一个去找。直接在这个窗体下操作添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BsaEdit_Click(object sender, EventArgs e)
        {
            ButtonSpecAny bsa = sender as ButtonSpecAny;
            KryptonComboBox ktb = bsa.Owner as KryptonComboBox;
            #region 找到绑定的字段

            //取外键表名的代码
            string fktableName = string.Empty;


            BindingSource bs = new BindingSource();
            bs = ktb.DataSource as BindingSource;//这个是对应的是主体实体


            if (bs.Current == null)
            {
                fktableName = bs.DataSource.GetType().GetGenericArguments()[0].Name;
            }
            else
            {
                fktableName = bs.Current.GetType().Name;//这个会出错，current 可能会为空。
            }

            #endregion

            //这里调用权限判断
            //调用通用的查询编辑基础资料。
            //需要对应的类名，如果修改新增了数据要重新加载下拉数据
            tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.EntityName == fktableName.ToString());
            if (menuinfo == null)
            {
                MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，请联系管理员。");
                return;
            }
            //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
            BaseUControl ucBaseList = Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);
            ucBaseList.Runway = BaseListRunWay.选中模式;
            //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
            frmBaseEditList frmedit = new frmBaseEditList();
            frmedit.StartPosition = FormStartPosition.CenterScreen;
            ucBaseList.Dock = DockStyle.Fill;
            frmedit.kryptonPanel1.Controls.Add(ucBaseList);
            frmedit.Text = menuinfo.CaptionCN;

            if (frmedit.ShowDialog() == DialogResult.OK)
            {
                string ucTypeName = bsa.Owner.GetType().Name;
                if (ucTypeName == "KryptonComboBox")
                {
                    //选中的值，一定要在重新加载前保存，下面会清空重新加载会变为第一个项
                    if (ucBaseList.Tag != null)
                    {
                        object obj = ucBaseList.Tag;
                        //从缓存中重新加载 
                        BindingSource NewBsList = new BindingSource();
                        //将List<T>类型的结果是object的转换为指定类型的List
                        //var lastlist = ((IEnumerable<dynamic>)rslist).Select(item => Activator.CreateInstance(mytype)).ToList();
                        var cachelist = BizCacheHelper.Manager.CacheEntityList.Get(fktableName);
                        if (cachelist != null)
                        {
                            // 获取原始 List<T> 的类型参数
                            Type listType = cachelist.GetType();
                            if (TypeHelper.IsGenericList(listType))
                            {
                                #region  强类型时
                                NewBsList.DataSource = ((IEnumerable<dynamic>)cachelist);
                                #endregion
                            }
                            else if (TypeHelper.IsJArrayList(listType))
                            {
                                Type elementType = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(Global.GlobalConstants.Model_NAME + "." + fktableName);
                                List<object> myList = TypeHelper.ConvertJArrayToList(elementType, cachelist as JArray);

                                #region  jsonlist
                                NewBsList.DataSource = myList;
                                #endregion

                            }
                            Common.DataBindingHelper.InitDataToCmb(NewBsList, ktb.ValueMember, ktb.DisplayMember, ktb);
                            ////因为选择中 实体数据并没有更新，下面两行是将对象对应的属性给一个选中的值。
                            object selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, ktb.ValueMember);
                            Binding binding = null;
                            if (ktb.DataBindings.Count > 0)
                            {
                                binding = ktb.DataBindings[0]; //这个是下拉绑定的实体集合
                            }
                            else
                            {
                                // binding = new Binding();
                            }

                            RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, ktb.ValueMember, selectValue);
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
            // 指定 CHM 文件路径和要定位的页面及段落（这里只是示例，你需要根据实际情况设置）
            string chmFilePath = System.IO.Path.Combine(Application.StartupPath, "ruinor.chm");
            string targetPage = "page_name";
            string targetParagraph = "paragraph_id";

            try
            {
                // 使用 HH.exe 来打开 CHM 文件并指定定位
                Process.Start("hh.exe", $"\"{chmFilePath}\"::{targetPage}#{targetParagraph}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开 CHM 文件出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


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


        /// <summary>
        /// 特殊显示必填项 
        /// </summary>
        /// <typeparam name="T">要验证必填的类型</typeparam>
        /// <param name="rules"></param>
        /// <param name="Controls"></param>
        public void InitRequiredToControl(AbstractValidator<T> rules, System.Windows.Forms.Control.ControlCollection Controls)
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

        /// <summary>
        /// 下拉边上加载快捷添加按钮,单据下暂时不显示
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Controls"></param>
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
                            if (ktb.ButtonSpecs.Count > 0)
                            {
                                return;
                            }
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

        protected bool Validator()
        {
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            var EditEntity = bindingSourceEdit.Current as T;
            bool vd = ShowInvalidMessage(ctr.BaseValidator(EditEntity));
            return vd;
        }

        protected bool Validator<C>(C checkItem) where C : class
        {
            BaseController<C> ctr = Startup.GetFromFacByName<BaseController<C>>(typeof(C).Name + "Controller");
            bool vd = ShowInvalidMessage(ctr.BaseValidator(checkItem as C));
            return vd;
        }

        private void BaseEdit_Load(object sender, EventArgs e)
        {

        }


    }
}
