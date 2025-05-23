﻿using FastReport.Editor.Dialogs;
using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Common;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.ClientCmdService;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserPersonalized
{
    [MenuAttrAssemblyInfo("查询条件编辑", true, UIType.单表数据)]
    public partial class UCQueryCondition : BaseEditGeneric<tb_UIQueryCondition>
    {
        public UCQueryCondition()
        {
            InitializeComponent();
        }


        public BaseEntity QueryDto { get; set; }
        /// <summary>
        /// 查询条件 根据这个来生成绑定。选择默认值
        /// </summary>
        public List<QueryField> QueryFields { get; set; }

        //添加一个事件。如果同步选中和标题


        public tb_UIQueryCondition EditEntity { get; set; }

        UCAdvDateTimerPickerGroup dtpgroup = new UCAdvDateTimerPickerGroup();
        KryptonComboBox DefaultCmb = new KryptonComboBox();

        DataBindingHelper dbh = new DataBindingHelper();
        public void BindData(tb_UIQueryCondition entity)
        {
            EditEntity = entity;
            DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.Caption, txtCaption, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.ValueType, txtValueType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.IsVisble, chkVisble, false);
            DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.Default1, txtDefault1, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.Default2, txtDefault2, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.EnableDefault1, chkEnableDefault1, false);
            DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.EnableDefault2, chkEnableDefault2, false);
            DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.Focused, chkFocused, false);
            DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.MultiChoice, chkMultiChoice, false);
            DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.UseLike, chkUseLike, false);

            var queryField = QueryFields.FirstOrDefault(c => c.FieldName == entity.FieldName);
            if (queryField != null)
            {
                switch (queryField.AdvQueryFieldType)
                {

                    case AdvQueryProcessType.defaultSelect:
                        txtDefault1.Visible = false;
                        txtDefault2.Visible = false;
                        lblDefault2.Visible = false;
                        chkEnableDefault1.Visible = true;
                        chkEnableDefault2.Visible = false;
                        #region     单选下拉


                        DefaultCmb.Name = queryField.FieldName;
                        DefaultCmb.Text = "";
                        DefaultCmb.Width = 150;
                        //只处理需要缓存的表
                        KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                        if (queryField.FKTableName.IsNotEmptyOrNull())
                        {
                            if (BizCacheHelper.Manager.NewTableList.TryGetValue(queryField.SubQueryTargetType.Name, out pair))
                            {
                                //关联要绑定的类型
                                Type mytype = queryField.SubQueryTargetType;
                                if (queryField.SubFilter.FilterLimitExpressions.Count == 0)
                                {
                                    #region 没有限制条件时

                                    if (mytype != null)
                                    {
                                        MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { mytype });
                                        object[] args = new object[5] { QueryDto, pair.Key, pair.Value, queryField.FKTableName, DefaultCmb };
                                        mf.Invoke(dbh, args);
                                    }
                                    else
                                    {
                                        MainForm.Instance.logger.LogError("动态加载外键数据时出错，" + queryField.FieldName + "在代理类中的属性名不对，自动生成规则可能有变化" + queryField.FKTableName.ToLower());
                                        MainForm.Instance.uclog.AddLog("动态加载外键数据时出错加载数据出错。请联系管理员。");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //非常值和学习借鉴有代码 TODO 重点学习代码
                                    //UI传入过滤条件 下拉可以显示不同的数据
                                    ExpConverter expConverter = new ExpConverter();
                                    var whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                                    #region 
                                    //绑定下拉

                                    if (pair.Key == queryField.FieldName)
                                    {
                                        MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                                        object[] args1 = new object[6] { QueryDto, pair.Key, pair.Value, queryField.FKTableName, DefaultCmb, whereExp };
                                        mf1.Invoke(dbh, args1);
                                    }
                                    else
                                    {
                                        MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbRefWithLimitedByAlias").MakeGenericMethod(new Type[] { mytype });
                                        //注意这样
                                        object[] args1 = new object[7] { QueryDto, pair.Key, queryField.FieldName, pair.Value, queryField.FKTableName, DefaultCmb, whereExp };
                                        mf1.Invoke(dbh, args1);
                                    }

                                    #endregion
                                }

                                ////注意这样调用不能用同名重载的方法名
                                //MethodInfo mf22 = dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { mytype });
                                //object[] args22 = new object[7] { QueryDto, DefaultCmb, pair.Value, queryField.SubFilter, null, null, false };
                                //mf22.Invoke(dbh, args22);
                            }
                        }

                        DefaultCmb.Location = new System.Drawing.Point(txtDefault1.Location.X, txtDefault1.Location.Y);
                        this.Controls.Add(DefaultCmb);
                        #endregion
                        break;

                    case AdvQueryProcessType.datetimeRange:
                        txtDefault1.Visible = false;
                        txtDefault2.Visible = false;
                        lblDefault2.Visible = false;
                        chkEnableDefault1.Visible = false;
                        chkEnableDefault2.Visible = false;
                        //重新生成日期类型的
                        #region
                        dtpgroup.Name = queryField.FieldName;
                        dtpgroup.dtp1.Name = queryField.ExtendedAttribute[0].ColName;
                        object datetimeValue1 = ReflectionHelper.GetPropertyValue(QueryDto, queryField.ExtendedAttribute[0].ColName);
                        DataBindingHelper.BindData4DataTime(QueryDto, datetimeValue1, queryField.ExtendedAttribute[0].ColName, dtpgroup.dtp1, true);
                        dtpgroup.dtp1.Checked = true;

                        dtpgroup.dtp2.Name = queryField.ExtendedAttribute[1].ColName;
                        object datetimeValue2 = ReflectionHelper.GetPropertyValue(QueryDto, queryField.ExtendedAttribute[1].ColName);
                        DataBindingHelper.BindData4DataTime(QueryDto, datetimeValue2, queryField.ExtendedAttribute[1].ColName, dtpgroup.dtp2, true);
                        dtpgroup.dtp2.Checked = true;
                        //时间控件更长为260px，这里要特殊处理
                        dtpgroup.Location = new System.Drawing.Point(txtDefault1.Location.X, txtDefault1.Location.Y);
                        dtpgroup.Size = new System.Drawing.Size(260, 100);
                        this.Controls.Add(dtpgroup);

                        #endregion
                        break;

                    default:
                        txtDefault1.Visible = false;
                        txtDefault2.Visible = false;
                        lblDefault1.Visible = false;
                        lblDefault2.Visible = false;
                        chkEnableDefault1.Visible = false;
                        chkEnableDefault2.Visible = false;
                        break;
                }

                if (queryField.SugarCol != null)
                {
                    if (queryField.SugarCol.SqlParameterDbType!=null)
                    {
                        //字符串才启用模糊查询
                        if (queryField.SugarCol.SqlParameterDbType.ToString() == "String")
                        {
                            lblUselike.Visible = true;
                            chkUseLike.Visible = true;
                        }
                        else
                        {
                            lblUselike.Visible = false;
                            chkUseLike.Visible = false;
                        }


                        //下拉才可能多选
                        if (queryField.SugarCol.SqlParameterDbType.ToString() == "Int64")
                        {
                            lblMultiChoice.Visible = true;
                            chkMultiChoice.Visible = true;
                        }
                        else
                        {
                            lblMultiChoice.Visible = false;
                            chkMultiChoice.Visible = false;
                        }

                    }
                    else
                    {
                        #region 用其它方式判断
                        //字符串才启用模糊查询
                        if (queryField.FieldPropertyInfo!=null && queryField.FieldPropertyInfo.PropertyType.ToString() == "System.String")
                        {
                            lblUselike.Visible = true;
                            chkUseLike.Visible = true;
                        }
                        else
                        {
                            lblUselike.Visible = false;
                            chkUseLike.Visible = false;
                        }
                        #endregion

                        #region 用其它方式判断是否能多选
                        //字符串才启用模糊查询
                        if (queryField.FieldPropertyInfo != null && queryField.FieldPropertyInfo.PropertyType.ToString() == "System.Int64")
                        {
                            lblMultiChoice.Visible = true;
                            chkMultiChoice.Visible = true;
                        }
                        else
                        {
                            lblMultiChoice.Visible = false;
                            chkMultiChoice.Visible = false;
                        }
                        #endregion
                    }

                }

            }

            entity.PropertyChanged += (sender, s2) =>
            {
                if (s2.PropertyName == entity.GetPropertyName<tb_UIQueryCondition>(c => c.Caption))
                {
                    if (OnSynchronizeUI != null) OnSynchronizeUI(sender, nameof(entity.Caption));
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_UIQueryCondition>(c => c.IsVisble))
                {
                    if (OnSynchronizeUI != null) OnSynchronizeUI(sender, nameof(entity.IsVisble));
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_UIQueryCondition>(c => c.Focused))
                {
                    if (OnSynchronizeUI != null) OnSynchronizeUI(sender, nameof(entity.Focused));
                }



            };
        }

        public event SynchronizeUIEventHandler OnSynchronizeUI;
        public delegate void SynchronizeUIEventHandler(object sender, object e);

        private void UCQueryCondition_Leave(object sender, EventArgs e)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            var queryField = QueryFields.FirstOrDefault(c => c.FieldName == EditEntity.FieldName);
            if (queryField != null)
            {
                if (queryField.AdvQueryFieldType == Global.AdvQueryProcessType.datetimeRange)
                {

                    EditEntity.EnableDefault1 = dtpgroup.dtp1.Checked;
                    if (EditEntity.EnableDefault1.Value)
                    {
                        //变化时算时天数差值
                        EditEntity.DiffDays1 = (ReflectionHelper.GetPropertyValue(QueryDto, queryField.ExtendedAttribute[0].ColName).ToDateTime() - System.DateTime.Now).Days;
                        if (EditEntity.DiffDays1 < -3650)
                        {
                            EditEntity.DiffDays1 = -3650;
                        }
                    }
                    else
                    {
                        EditEntity.DiffDays1 = 0;
                    }

                    EditEntity.EnableDefault2 = dtpgroup.dtp2.Checked;
                    if (EditEntity.EnableDefault2.Value)
                    {
                        EditEntity.DiffDays2 = (ReflectionHelper.GetPropertyValue(QueryDto, queryField.ExtendedAttribute[1].ColName).ToDateTime() - System.DateTime.Now).Days;
                        if (EditEntity.DiffDays2 < -3650)
                        {
                            EditEntity.DiffDays2 = -3650;
                        }
                    }
                    else
                    {
                        EditEntity.DiffDays2 = 0;
                    }

                }

                if (queryField.AdvQueryFieldType == Global.AdvQueryProcessType.defaultSelect)
                {
                    if (DefaultCmb.SelectedValue != null)
                    {
                        EditEntity.Default1 = DefaultCmb.SelectedValue.ToString();
                        EditEntity.EnableDefault1 = chkEnableDefault1.Checked;
                    }

                }

                if (chkFocused.Checked)
                {
                    queryField.Focused = chkFocused.Checked;
                    EditEntity.Focused = queryField.Focused;


                }
            }
        }
    }
}
