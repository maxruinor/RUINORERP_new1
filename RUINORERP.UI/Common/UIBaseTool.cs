using Krypton.Toolkit;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    public class UIBaseTool
    {
        public tb_MenuInfo CurMenuInfo { get; set; }


        #region  单据用
        /// <summary>
        /// 初始化编辑项:查询过滤器
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Controls"></param>

        public void InitEditItemToControl<T>(BaseEntity entity, System.Windows.Forms.Control.ControlCollection Controls) where T : class
        {
            //思路通过控件的数据源 调试时是产品，对应绑定的关联表的主键及 可的到对应的关联对象实体名。再通过菜单中对应关系找到相差窗体类。
            //https://www.cnblogs.com/GaoUpUp/p/17187770.html
            foreach (var control in Controls)
            {
                if (control is Control)
                {
                    InitEditFilterForControl<T>(entity, control as Control);
                }

            }
        }

        /// <summary>
        /// 单个控件的过滤器设置
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        public void InitEditFilterForControl<T>(BaseEntity entity, System.Windows.Forms.Control item) where T : class
        {
            if (item is Control)
            {
                if (item is VisualControlBase)
                {
                    if (item.GetType().Name == "KryptonComboBox")
                    {
                        KryptonComboBox ktb = item as KryptonComboBox;
                        //不重复添加
                        if (ktb.ButtonSpecs.Count > 0)
                        {
                            return;
                        }

                        if ((item as Control).DataBindings.Count > 0)
                        {
                            if (ktb.DataBindings.Count > 0 && ktb.DataSource is BindingSource)
                            {
                                ButtonSpecAny bsa = new ButtonSpecAny();
                                bsa.Image = System.Drawing.Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                bsa.Tag = ktb;
                                // bsa.Click += BsaEdit_Click;
                                bsa.Click += (sender, e) =>
                                {
                                    #region 处理点击事件
                                    //ButtonSpecAny bsa = sender as ButtonSpecAny;
                                    //KryptonComboBox ktb = bsa.Owner as KryptonComboBox;
                                    #region 找到绑定的字段
                                    //取外键表名的代码
                                    string fktableName = string.Empty;
                                    if (ktb.Tag != null)
                                    {
                                        fktableName = ktb.Tag.ToString();
                                    }
                                    if (string.IsNullOrEmpty(fktableName))
                                    {
                                        var ss = ktb.DataBindings[0]; //这个是下拉绑定的实体集合
                                    }
                                    #endregion

                                    //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
                                    BaseUControl ucBaseList = new UCAdvFilterGeneric<T>(); // Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);
                                    ucBaseList.Runway = BaseListRunWay.选中模式;
                                    ucBaseList.CurMenuInfo = CurMenuInfo;
                                    //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
                                    frmBaseEditList frmedit = new frmBaseEditList();
                                    frmedit.StartPosition = FormStartPosition.CenterScreen;
                                    ucBaseList.Dock = DockStyle.Fill;
                                    frmedit.kryptonPanel1.Controls.Add(ucBaseList);
                                    BizTypeMapper mapper = new BizTypeMapper();
                                    var BizTypeText = mapper.GetBizType(typeof(T).Name).ToString();
                                    frmedit.Text = "关联查询" + "-" + BizTypeText;
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
                                    #endregion
                                };
                                ktb.ButtonSpecs.Add(bsa);
                                //可以边框为红色不？
                                //或必填项目有特别提示？
                                //    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 可编辑及查询

        /// <summary>
        /// 可编辑及查询。暂时没有过滤条件。InitFilterForControlByExpCanEdit才有。后面优化合并？
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="CanEdit"></param>
        public void AddEditableQueryControl<T>(Control item, bool CanEdit = false) where T : class
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
                                bsa.Click += (sender, e) =>
                                {
                                    #region  事件

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

                                    #endregion
                                };
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

        #endregion

    }
}
