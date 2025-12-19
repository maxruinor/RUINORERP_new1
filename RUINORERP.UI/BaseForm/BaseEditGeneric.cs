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
using Netron.GraphLib;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;


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

        /// <summary>
        /// 关联的菜单信息 实际是可以从点击时传入
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; }


        public delegate void CancelHandler();

        [Browsable(true), Description("引发外部查询事件")]
        public event CancelHandler CancelEvent;


        //为了区别是不是重写了这个方法来标记在基类中如何调用。只是为了不想改46个引用BindData(BaseEntity entity)
        public bool usedActionStatus = false;

        //[MustOverride]
        public virtual void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {

        }
        public virtual void BindData(BaseEntity entity)
        {
            //公共方法
            string PrimaryKeyColName = UIHelper.GetPrimaryKeyColName(typeof(T));
            if (!string.IsNullOrEmpty(PrimaryKeyColName))
            {
                entity.PrimaryKeyID = (long)ReflectionHelper.GetPropertyValue(entity, PrimaryKeyColName);
                //ToolBarEnabledControl(entity);
            }
            entity.AcceptChanges();
        }

        //public virtual void BindData(T entity, ActionStatus actionStatus = ActionStatus.无操作)
        //{

        //}
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


        private T _BaseEditEntity;
        public T BaseEditEntity { get => _BaseEditEntity; set => _BaseEditEntity = value; }


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
                MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，或配置菜时参数不正确。请联系管理员。");
                return;
            }
            //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
            BaseUControl ucBaseList = null;

            if (menuinfo.BIBaseForm == "BaseListWithTree`1")
            {
                ucBaseList = Startup.GetFromFacByName<BaseListWithTree>(menuinfo.FormName);
            }
            else
            {
                ucBaseList = Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);
            }

            ucBaseList.Runway = BaseListRunWay.选中模式;
            //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
            frmBaseEditList frmedit = new frmBaseEditList();
            frmedit.StartPosition = FormStartPosition.CenterScreen;
            ucBaseList.Dock = DockStyle.Fill;
            frmedit.kryptonPanel1.Controls.Add(ucBaseList);
            frmedit.Text = menuinfo.CaptionCN;

            //加载一次默认的查询
            ucBaseList.LoadQueryParametersToUI(null, null);

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
                        var cachelist = RUINORERP.Business.Cache.EntityCacheHelper.GetEntityListByTableName(fktableName);
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
                            Common.DataBindingHelper.InitDataToCmb(NewBsList, ktb.ValueMember, ktb.DisplayMember, ktb);
                            ////因为选择中 实体数据并没有更新，下面两行是将对象对应的属性给一个选中的值。
                            object selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, ktb.ValueMember);
                            Binding binding = null;
                            if (ktb.DataBindings.Count > 0)
                            {
                                binding = ktb.DataBindings[0]; //这个是下拉绑定的实体集合
                                RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, ktb.ValueMember, selectValue);
                            }
                            else
                            {
                               //实际到这里是错的
                                binding =  new Binding("SelectedValue", obj, ktb.ValueMember, true, DataSourceUpdateMode.OnValidation);
                                ktb.DataBindings.Add(binding);
                                RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(obj, ktb.ValueMember, selectValue);
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
            
            // 确保定时器被正确释放
            if (_timeoutTimer4tips != null)
            {
                _timeoutTimer4tips.Dispose();
            }
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
                if (ActiveControl==null)
                {
                    return;
                }
                if (ActiveControl.GetType().ToString() == "ComponentFactory.Krypton.Toolkit.KryptonTextBox+InternalTextBox")
                {
                    KryptonTextBox txt = ActiveControl.Parent as KryptonTextBox;
                    tipTxt = GetHelpInfoByBinding(txt.DataBindings);
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
        /// 特殊显示必填项 1
        /// </summary>
        /// <typeparam name="T">要验证必填的类型</typeparam>
        /// <param name="rules">验证器实例</param>
        /// <param name="Controls">控件集合</param>
        public void InitRequiredToControl(AbstractValidator<T> rules, System.Windows.Forms.Control.ControlCollection Controls)
        {
            List<string> requiredFields = new List<string>();
            Type entityType = typeof(T);
            
            // 遍历验证规则，识别所有必填验证器
            foreach (var item in rules)
            {
                string colName = item.PropertyName;
                var rr = item.Components;
                
                foreach (var com in item.Components)
                {
                    // 使用统一的必填验证器判断方法，传入属性名和实体类型
                    if (IsRequiredValidator(com, colName, entityType))
                    {
                        requiredFields.Add(colName);
                        break; // 找到一个必填验证器就足够了，无需继续检查同一属性的其他验证器
                    }
                }
            }

            // 应用样式到控件
            foreach (var item in Controls)
            {
                if (item is Control control && control is VisualControlBase)
                {
                    // 只处理有数据绑定的控件
                    if (control.DataBindings.Count > 0)
                    {
                        string fieldName = control.DataBindings[0].BindingMemberInfo.BindingField;
                        // 检查是否是必填字段
                        string requiredField = requiredFields.FirstOrDefault(c => c == fieldName);
                        
                        if (requiredField.IsNotEmptyOrNull())
                        {
                            ApplyRequiredFieldStyle(control);
                        }
                    }
                }
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 为控件应用必填字段样式
        /// </summary>
        /// <param name="control">要应用样式的控件</param>
        private void ApplyRequiredFieldStyle(Control control)
        {
            if (control is KryptonTextBox ktb)
            {
                ktb.StateCommon.Border.Color1 = Color.FromArgb(255, 128, 128);
            }
            else if (control is KryptonComboBox kcb)
            {
                kcb.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(255, 128, 128);
            }
            // 可以在这里添加更多控件类型的处理
        }

        /// <summary>
        /// 判断验证器组件是否为必填验证器
        /// </summary>
        /// <param name="component">验证器组件</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>如果是必填验证器返回true，否则返回false</returns>
        private bool IsRequiredValidator(FluentValidation.Internal.IRuleComponent component, string propertyName, Type entityType)
        {
            // 检查常见的必填验证器类型
            switch (component.Validator.Name)
            {
                case "NotEmptyValidator":
                case "NotNullValidator":
                    return true;
                    
                case "PredicateValidator":
                    // 对于外键验证器，需要检查属性是否为可空类型
                    return IsRequiredForeignKeyValidator(component, propertyName, entityType);
                    
                default:
                    return false;
            }
        }

        /// <summary>
        /// 判断验证器组件是否为必填的外键验证器
        /// </summary>
        /// <param name="component">验证器组件</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>如果是必填的外键验证器返回true，否则返回false</returns>
        private bool IsRequiredForeignKeyValidator(FluentValidation.Internal.IRuleComponent component, string propertyName, Type entityType)
        {
            // 检查是否是PredicateValidator（用于Must方法）
            if (component.Validator.Name != "PredicateValidator")
            {
                return false;
            }

            // 获取属性信息
            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return false;
            }

            // 检查属性是否有FKRelationAttribute特性，判断是否为外键
            var fkAttr = propertyInfo.GetCustomAttribute<FKRelationAttribute>(false);
            if (fkAttr == null)
            {
                return false; // 不是外键
            }

            // 检查属性类型是否为非空类型（非可空类型）
            // 如果是值类型且不是可空类型，则为必填
            if (propertyInfo.PropertyType.IsValueType)
            {
                // 检查是否是可空值类型
                var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                // 如果underlyingType为null，表示不是可空类型，即为必填
                return underlyingType == null;
            }

            // 对于引用类型，通常认为是可空的
            return false;
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
                            // 修复逻辑错误：不应该在找到一个控件有ButtonSpecs后就返回，而应该继续处理其他控件
                            if (ktb.ButtonSpecs.Count > 0)
                            {
                                continue; // 跳过已经有按钮的控件
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
            bool vd = UIHelper.ShowInvalidMessage(ctr.BaseValidator(EditEntity));
            return vd;
        }

        protected bool Validator<C>(C checkItem) where C : class
        {
            BaseController<C> ctr = Startup.GetFromFacByName<BaseController<C>>(typeof(C).Name + "Controller");
            bool vd = UIHelper.ShowInvalidMessage(ctr.BaseValidator(checkItem as C));
            return vd;
        }

        private void BaseEdit_Load(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 有些功能会在UI加载完后才生效
        /// </summary>
        public virtual void UIShown()
        {
            if (!this.DesignMode)
            {
                UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
            }

        }

        private void BaseEditGeneric_Shown(object sender, EventArgs e)
        {
            UIShown();
        }
    }
}
