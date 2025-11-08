using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Windows.Forms;


using System.ComponentModel;
using RUINORERP.Common.Helper;
using RUINORERP.Common.Extensions;
using System.Reflection;
using System.Text.RegularExpressions;
using RUINORERP.Model;
using System.Collections;
using System.Reflection.Emit;
//using System.Workflow.ComponentModel.Serialization;
using RUINORERP.Common;
using System.Globalization;
using RUINORERP.Model.Base;
using System.Drawing;
using System.IO;


using System.Drawing.Imaging;
using SqlSugar;

using Expression = System.Linq.Expressions.Expression;
using ConstantExpression = System.Linq.Expressions.ConstantExpression;

namespace RUINORERP.Common.Helper
{

    public enum BindType4Enum
    {

        /// <summary>
        /// 显示枚举名
        /// </summary>
        EnumName,

        /// <summary>
        /// 显示枚举名的显示特性 [Display(Name ="Permanent")]
        /// </summary>
        EnumDisplay
    }

    public enum BindDataType
    {
        Money,
        Text,
        Bool,
        Qty

    }

    public class DataBindHelper
    {
        public static void BindData4RadioGroupTrueFalse(object entity, string key, RadioButton RadioButton1, RadioButton RadioButton2)
        {
            string value = ReflectionHelper.GetModelValue(key, entity);
            if (value == null)
            {
                value = "false";
            }
            // Set initial values
            RadioButton1.Checked = bool.Parse(value);
            RadioButton2.Checked = !bool.Parse(value);
            // Change on event
            RadioButton1.CheckedChanged += delegate
            {
                ReflectionHelper.SetPropertyValue(entity, key, RadioButton1.Checked);
            };
            //RadioButton2.CheckedChanged += delegate
            //{
            //ReflectionHelper.SetPropertyValue(entity, key, RadioButton2.Checked);
            //};

            Binding binddata = null;
            binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            // binddata.Format += (s, args) => args.Value = ((string)args.Value) == RadioButton1.Text;
            //binddata.Parse += (s, args) => args.Value = (bool)args.Value ? RadioButton1.Text : RadioButton2.Text;
            //数据源的数据类型转换为控件要求的数据类型。
            binddata.Format += (s, args) => args.Value = args.Value == null ? false : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            binddata.Parse += (s, args) => args.Value = args.Value == null ? false : args.Value;
            RadioButton1.DataBindings.Add(binddata);
            //RadioButton2.DataBindings.Add(binddata);
        }



        //产品表中日子好的例子。。参考 之前 华聪的文章。金额等分类型？

        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnum<T>(object entity, Expression<Func<T, int?>> expkey, Type enumType, ComboBox cmbBox, bool addSelect)
        {
            cmbBox.DataBindings.Clear();
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            BindData4CmbByEnum<T>(entity, key, enumType, cmbBox, addSelect);
        }
        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnum<T>(object entity, string keyName, Type enumType, ComboBox cmbBox, bool addSelect)
        {
            cmbBox.Tag = keyName;
            InitDataToCmbByEnumDynamicGeneratedDataSource(enumType, keyName, cmbBox, addSelect);

            var depa = new Binding("SelectedValue", entity, keyName, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);

        }

        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnumData<T>(object entity, Expression<Func<T, int?>> expkey, ComboBox cmbBox)
        {
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            cmbBox.Tag = key;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }

        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="defaultExp">默认值的</param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnum<T>(Type enumType, object entity, Expression<Func<T, int?>> expkey, Func<object, object> defaultExp, ComboBox cmbBox, bool addSelect)
        {
            cmbBox.DataBindings.Clear();
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            InitDataToCmbByEnumDynamicGeneratedDataSource(enumType, key, cmbBox, addSelect);

            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
            cmbBox.SelectedValue = defaultExp;
        }


        /// <summary>
        /// 绑定数据到UI，只传入了id
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey">显示名</param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEntity<T>(object entity, Expression<Func<T, long>> expkey, ComboBox cmbBox) where T : class
        {
            cmbBox.DataBindings.Clear();

            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }


        #region  绑定数据到UI,下拉列表的主键在引用表中字段不一致的时候使用
        /// <summary>
        /// 这个绑定下拉时，比方在产品表中。T会为tb_unit，expkey为unit_id，expValue为unit_name，Unit_ID会作为外键保存在产品中。
        /// 如果外键列名和单位表本身主键ID不一致时会出错。
        /// 这个方法是为了解决上述问题
        /// <typeparamref name="T1"/>单位（引用表）<typeparamref name="T1"/>
        /// <typeparamref name="T2"/>产品表（主表）<typeparamref name="T2"/>
        /// </summary>
        public static void BindData4Cmb<T1, T2>(object entity, Expression<Func<T1, long>> expkey, Expression<Func<T1, string>> expValue,
            Expression<Func<T2, long>> RefExpkey, RadioButton cmbBox, bool SyncUI, Expression<Func<T1, bool>> expCondition = null) where T1 : class where T2 : class
        {
            cmbBox.DataBindings.Clear();

            // InitDataToCmb<T1>(expkey, expValue, cmbBox);
            MemberInfo minfo = RefExpkey.GetMemberInfo();
            string key = minfo.Name;
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            }



            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }



        #endregion





        /// <summary>
        /// 绑定数据到UI，绑定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expTxt">显示名</param>
        /// <param name="expValue">id</param>
        /// <param name="cmbBox"></param>
        public static void BindData4Cmb<T>(object entity, Expression<Func<T, string>> expTxt, ComboBox cmbBox, BindDataType dataType, bool SyncUI) where T : class
        {
            cmbBox.DataBindings.Clear();

            MemberInfo minfo = expTxt.GetMemberInfo();
            string key = minfo.Name;
            Binding depa = null;
            if (dataType == BindDataType.Text)
            {
                if (SyncUI)
                {
                    //双向绑定 应用于加载和编辑
                    depa = new Binding("SelectedText", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
                }
                else
                {
                    //单向绑定 应用于加载
                    depa = new Binding("SelectedText", entity, key, true, DataSourceUpdateMode.OnValidation);
                }
            }


            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;
            cmbBox.DataBindings.Add(depa);
        }


        /// <summary>
        /// 绑定数据到UI，要求引用的外键名要和表ID名一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey">显示名</param>
        /// <param name="expValue">id</param>
        /// <param name="cmbBox"></param>
        public static void BindData4Cmb<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, RadioButton cmbBox, bool SyncUI) where T : class
        {
            cmbBox.DataBindings.Clear();

            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            }



            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }


        public static void BindData4Cmb<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, RadioButton cmbBox, Expression<Func<T, bool>> expCondition)
        {
            cmbBox.DataBindings.Clear();



            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);


        }

        public static void BindData4Cmb<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, RadioButton cmbBox, Expression<Func<T, bool>> expCondition, bool WithClear)
        {
            BindData4Cmb<T>(entity, expkey, expValue, cmbBox, expCondition);
        }







        public static void BindData4Cmb<T>(object entity, string expkey, string expValue, string tableName, RadioButton cmbBox) where T : class
        {

            string key = expkey;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }

        public static void BindData4Cmb(BindingSource droplistdatasouce, object entity, string ValueMember, string DisplayMember, RadioButton cmbBox)
        {
            //InitDataToCmb(droplistdatasouce, ValueMember, DisplayMember, cmbBox);
            var depa = new Binding("SelectedValue", entity, ValueMember, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }

        #region CheckBox

        public static void BindData4CheckBox(object entity, string key, CheckBox chkBox, bool SyncUI)
        {
            Binding binddata = null;
            if (SyncUI)
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            else
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            string value = ReflectionHelper.GetModelValue(key, entity);
            if (value == null)
            {
                value = "false";
            }
            // Set initial values
            chkBox.Checked = bool.Parse(value);

            // Change on event
            chkBox.CheckedChanged += delegate
            {
                ReflectionHelper.SetPropertyValue(entity, key, chkBox.Checked);
            };
            //数据源的数据类型转换为控件要求的数据类型。
            binddata.Format += (s, args) => args.Value = args.Value == null ? false : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            binddata.Parse += (s, args) => args.Value = args.Value == null ? false : args.Value;
            chkBox.DataBindings.Add(binddata);
        }

        public static void BindData4CheckBox<T>(object entity, string key, CheckBox chkBox, bool SyncUI)
        {
            chkBox.DataBindings.Clear();
            Binding binddata = null;
            if (SyncUI)
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            else
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            string value = ReflectionHelper.GetModelValue(key, entity);
            if (value == null)
            {
                value = "false";
            }
            // Set initial values
            chkBox.Checked = bool.Parse(value);

            // Change on event
            chkBox.CheckedChanged += delegate
            {
                ReflectionHelper.SetPropertyValue(entity, key, chkBox.Checked);
            };
            //数据源的数据类型转换为控件要求的数据类型。
            binddata.Format += (s, args) => args.Value = args.Value == null ? false : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            binddata.Parse += (s, args) => args.Value = args.Value == null ? false : args.Value;
            chkBox.DataBindings.Add(binddata);
        }


        public static void BindData4CheckBox<T>(object entity, Expression<Func<T, bool?>> exp, CheckBox chkBox, bool SyncUI)
        {

            var mb = exp.GetMemberInfo();
            string key = mb.Name;
            BindData4CheckBox<T>(entity, key, chkBox, SyncUI);
        }


        #endregion

     

        public static void BindData4DataTime(object entity, object datetimeValue, string key, DateTimePicker dtp, bool SyncUI)
        {
            dtp.DataBindings.Clear();
            //chkbox
            Binding dtpdata = null;
            if (SyncUI)
            {
                dtpdata = new Binding("Value", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                dtpdata = new Binding("Value", entity, key, true, DataSourceUpdateMode.OnValidation);
            }


            //数据源的数据类型转换为控件要求的数据类型。
            dtpdata.Format += (s, args) =>
            {
                if (args.Value == null || args.Value.ToString() == "0001-01-01 00:00:00" || string.IsNullOrEmpty(args.Value.ToString()))
                {
                    // 不要设置空格式，而是设置默认值
                    args.Value = DateTime.Now;
                }

            };
            //将控件的数据类型转换为数据源要求的数据类型。
            dtpdata.Parse += (s, args) =>
            {
                args.Value = !dtp.Checked ? null : args.Value;
            };
            dtp.Validating += dtp_Validating;
            dtp.ValueChanged += Dtp_CheckedChanged;

            // 不要设置空格式，保持默认格式
            //if (datetimeValue == null)
            //{
            //    dtp.Format = DateTimePickerFormat.Custom;
            //    dtp.CustomFormat = "   ";
            //}

            dtp.DataBindings.Add(dtpdata);
        }


        public static void BindData4DataTime<T>(object entity, Expression<Func<T, DateTime?>> exp, DateTimePicker dtp, bool SyncUI)
        {
            var mb = exp.GetMemberInfo();
            string key = mb.Name;
            object datetimeValue = typeof(T).GetProperty(key).GetValue(entity, null);

            BindData4DataTime(entity, datetimeValue, key, dtp, SyncUI);
        }

        private static void Dtp_CheckedChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            if (!dtp.Checked)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "   ";
                //将绑定的实体的值清空 = null;
                if (dtp.DataBindings.Count > 0)
                {
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, null);
                }
                dtp.CausesValidation = false;
                //如果 CausesValidation 属性设置为 false，则将取消 Validating 和 Validated 事件。
                ////dtp.Value = System.DateTime.Now;

            }
            else
            {
                dtp.Format = DateTimePickerFormat.Short;
                dtp.CustomFormat = null;
                if (dtp.DataBindings.Count > 0)
                {
                    if (dtp.Value != null && dtp.Value.Year == 1)
                    {
                        dtp.Value = System.DateTime.Now;
                    }
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, dtp.Value);
                }
                else
                {
                    //if (dtp.DataBindings.Count == 0)
                    //{
                    //    dtp.DataBindings.Add(dtp.DataBindings[0]);
                    //}
                }
                dtp.CausesValidation = true;
                //如果 CausesValidation 属性设置为 false，则将取消 Validating 和 Validated 事件。

            }
        }



        private static void dtp_Validating(object sender, CancelEventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            if (!dtp.Checked)
            {
                //将绑定的实体的值清空 = null;
                if (dtp.DataBindings.Count > 0)
                {
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, null);
                }
                //dtp.DataBindings.Clear();
                e.Cancel = false;
            }
            else
            {
                //将绑定的实体的值清空 = null;
                if (dtp.DataBindings.Count > 0)
                {
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, dtp.Value);
                }
                //dtp.DataBindings.Clear();
                e.Cancel = false;
            }

        }

        public static void BindData4Money<T>(object entity, Expression<Func<T, int>> expkey, TextBox txtBox, bool SyncUI)
        {
            var mb = expkey.GetMemberInfo();
            string key = mb.Name;
            Binding depa = null;
            if (SyncUI)
            {
                depa = new Binding("Text", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            else
            {
                depa = new Binding("Text", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? 0 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? 0 : args.Value;
            txtBox.DataBindings.Add(depa);
        }




        public static void BindData4TextBox<T>(object entity,
        Expression<Func<T, object>> expTextField,
        TextBox txtBox,
        BindDataType type, bool SyncUI
        )
        {
            txtBox.DataBindings.Clear();
            // string textField = expTextField.Body.ToString().Split('.')[1];
            MemberInfo minfo = expTextField.GetMemberInfo();
            string textField = minfo.Name;
            BindData4TextBox<T>(entity, textField, txtBox, type, SyncUI);
        }


        /// <summary>
        /// 请注意。这个指定绑定的是tag属性不是Text->BindData4TextBoxWithQuery
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expValueField"></param>
        /// <param name="txtBox"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4TextBoxWithTagQuery<T>(object entity,
        Expression<Func<T, object>> expValueField,
        TextBox txtBox,
        bool SyncUI
        )
        {
            txtBox.DataBindings.Clear();
            //MemberInfo minfo = expTextField.GetMemberInfo();
            //string textField = minfo.Name;
            MemberInfo minfoValue = expValueField.GetMemberInfo();
            string valueField = minfoValue.Name;

            BindData4TextBoxWithQuery<T>(entity, valueField, txtBox, SyncUI);
        }



        public static void BindData4ControlByEnum<T>(object entity, Expression<Func<T, object>> expTextField, Control control,
        BindType4Enum type, Type enumType
        )
        {
            control.DataBindings.Clear();
            MemberInfo minfo = expTextField.GetMemberInfo();
            string textField = minfo.Name;
            BindData4ControlByEnum<T>(entity, textField, control, type, enumType);
        }

        public static void BindData4ControlByEnum<T>(object entity, string textField, Control txtBox, BindType4Enum type, Type enumType)
        {
            Binding depa = null;
            //if (SyncUI)
            //{
            //    //双向绑定 应用于加载和编辑
            //    depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            //}
            //else
            //{
            //    //单向绑定 应用于加载
            depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            //}

            switch (type)
            {
                case BindType4Enum.EnumName:
                    //数据源的数据类型转换为控件要求的数据类型。
                    //
                    depa.Format += (s, args) =>
                    {
                        if (args.Value == null)
                        {
                            args.Value = "";
                        }
                        else
                        {
                            args.Value = Enum.ToObject(enumType, args.Value).ToString();
                        }

                    };
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        if (args == null)
                        {
                            args.Value = 0;
                        }
                        else
                        {
                            args.Value = (int)args.Value;
                        }
                    };

                    break;
                case BindType4Enum.EnumDisplay:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;
                default:
                    break;
            }

            txtBox.DataBindings.Add(depa);
        }






        public static void BindData4Label<T>(object entity,
        Expression<Func<T, string>> expTextField,
        System.Windows.Forms.Label lbl,
        BindDataType type, bool SyncUI
        )
        {
            lbl.DataBindings.Clear();
            // string textField = expTextField.Body.ToString().Split('.')[1];
            MemberInfo minfo = expTextField.GetMemberInfo();
            string textField = minfo.Name;
            BindData4Label<T>(entity, textField, lbl, type, SyncUI);
        }


        public static void BindData4Label<T>(object entity, string textField, System.Windows.Forms.Label
             lbl, BindDataType type, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            switch (type)
            {
                case BindDataType.Qty:
                    //数据源的数据类型转换为控件要求的数据类型。

                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                        lbl.Text = args.Value.ToString();
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                    };


                    break;
                case BindDataType.Money:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                        lbl.Text = args.Value.ToString("##.##");//这里是不是可以控制小数显示位数？
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                    };


                    break;
                case BindDataType.Text:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;
                default:
                    break;
            }

            lbl.DataBindings.Add(depa);
        }


        /// <summary>
        /// 请注意。这个指定绑定的是tag属性不是Text
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="valueField"></param>
        /// <param name="txtBox"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4TextBoxWithQuery<T>(object entity, string valueField, TextBox txtBox, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Tag", entity, valueField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Tag", entity, valueField, true, DataSourceUpdateMode.OnValidation);
            }

            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;


            txtBox.DataBindings.Add(depa);

            //===============================

            /*
            Binding depaText = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depaText = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depaText = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            //数据源的数据类型转换为控件要求的数据类型。
            depaText.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depaText.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;


            txtBox.DataBindings.Add(depaText);
            */


        }



        public static void BindData4TextBox<T>(object entity, string textField, TextBox txtBox, BindDataType type, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            switch (type)
            {
                case BindDataType.Qty:
                    //数据源的数据类型转换为控件要求的数据类型。

                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                        txtBox.Text = args.Value.ToString();
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                    };


                    break;
                case BindDataType.Money:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                        txtBox.Text = args.Value.ToString("##.##");//这里是不是可以控制小数显示位数？
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                    };


                    break;
                case BindDataType.Text:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;

                default:
                    break;
            }

            txtBox.DataBindings.Add(depa);
        }

        public static void BindData4TextBox(object entity, string textField, TextBox txtBox, BindDataType type, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            switch (type)
            {
                case BindDataType.Qty:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? 0 : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? 0 : args.Value;

                    break;
                case BindDataType.Money:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? 0.00 : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? 0.00 : args.Value;

                    break;
                case BindDataType.Text:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;
                default:
                    break;
            }

            txtBox.DataBindings.Add(depa);
        }


        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmb<T>(Expression<Func<T, long>> expression, Expression<Func<T, string>> expValue, ComboBox cmbBox) where T : class
        {
            MemberInfo minfo = expression.GetMemberInfo();
            string key = minfo.Name;
            MemberInfo minfoValue = expValue.GetMemberInfo();
            string value = minfoValue.Name;
            string tableName = expression.Parameters[0].Type.Name;
            InitDataToCmb<T>(key, value, tableName, cmbBox);
        }





        public static void InitDataToCmbByEnumDynamicGeneratedDataSource<T>(Type enumType, Expression<Func<T, int?>> expKey, ComboBox cmbBox, bool addSelect)
        {
            MemberInfo minfo = expKey.GetMemberInfo();
            string key = minfo.Name;
            InitDataToCmbByEnumDynamicGeneratedDataSource(enumType, key, cmbBox, addSelect);
        }

        /// <summary>
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
        /// <param name="add请选择">  是数据源绑定形式不可以自由添加，只能在数据源上做文章 ("请选择", "-1")</param>
        public static void InitDropList(BindingSource bs, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto)
        {
            cmb.BeginUpdate();
            cmb.DataSource = bs;
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;
            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();
            foreach (var dr in bs.List)
            {
                // sc.Add(dr[DisplayMember].ToString());
                sc.Add(ReflectionHelper.GetPropertyValue(dr, DisplayMember).ToString());
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {

                    cmb.AutoCompleteCustomSource = sc;
                    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            cmb.EndUpdate();
            cmb.SelectedIndex = -1;


        }

        /// <summary>
        /// 枚举名称要与DB表中的字段名相同
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="enumTypeName"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmbByEnumDynamicGeneratedDataSource(Type enumType, string keyName, ComboBox cmbBox, bool addSelect)
        {
            //枚举值为int，动态生成一个类再绑定，
            //var type = typeof(T);
            var type = enumType;
            var aName = new System.Reflection.AssemblyName(Assembly.GetExecutingAssembly().GetName().Name);
            //var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);//定义
            //var mb = ab.DefineDynamicModule(aName.Name);
            //var tb = mb.DefineType(type.Name + "EnumProxy", System.Reflection.TypeAttributes.Public, type);

            //string newlikeProName1 = coldata.ColName + "_Like";
            //var attrlikeBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "like", newlikeProName1, AdvQueryProcessType.stringLike });
            ////动态属性要提前创建生成，后面要实体化传入控件
            //PropertyBuilder newlikeProp1 = AddProperty(tb, newlikeProName1);
            //newlikeProp1.SetCustomAttribute(attrlikeBuilder1);

            TypeConfig typeConfig = new TypeConfig();
            typeConfig.FullName = aName.Name;

            //要创建的属性
            PropertyConfig propertyConfigKey = new PropertyConfig();
            propertyConfigKey.PropertyName = keyName;// type.Name;默认枚举名改为可以指定名
            propertyConfigKey.PropertyType = typeof(int);//枚举值为int 默认

            PropertyConfig propertyConfigName = new PropertyConfig();
            propertyConfigName.PropertyName = "Name";
            propertyConfigName.PropertyType = typeof(string);

            typeConfig.Properties.Add(propertyConfigKey);
            typeConfig.Properties.Add(propertyConfigName);
            Type newType = TypeBuilderHelper.BuildType(typeConfig);

            List<object> list = new List<object>();
            //(enumType[])Enum.GetValues(typeof(enumType));
            Array enumValues = Enum.GetValues(type);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            while (e.MoveNext())
            {
                object eobj = Activator.CreateInstance(newType);
                currentValue = (int)e.Current;
                currentName = e.Current.ToString();
                eobj.SetPropertyValue(keyName, currentValue);
                eobj.SetPropertyValue("Name", currentName);
                list.Add(eobj);
            }
            if (addSelect)
            {
                object sobj = Activator.CreateInstance(newType);
                sobj.SetPropertyValue(keyName, -1);
                sobj.SetPropertyValue("Name", "请选择");
                list.Insert(0, sobj);
            }


            BindingSource bs = new BindingSource();
            bs.DataSource = list;

            cmbBox.DataSource = bs;
            cmbBox.DisplayMember = "Name";
            cmbBox.ValueMember = keyName;
            cmbBox.SelectedIndex = -1;

        }


        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmb<T>(string key, string value, string tableName, ComboBox cmbBox) where T : class
        {
            BindingSource bs = new BindingSource();
            InitDropList(bs, cmbBox, key, value, ComboBoxStyle.DropDown, true);
        }





        /// <summary>
        /// 搜寻匹配的列表
        /// </summary>
        /// <param name="currentPartX">X字符串的当前比较部分</param>
        /// <param name="currentPartY">Y字符串的当前比较部分</param>
        /// <returns></returns>
        private static List<string> SearchMatchedList(string currentPartX, string currentPartY, List<List<string>> _preferenceList)
        {
            List<string> matchedList = null;
            foreach (var list in _preferenceList)
            {
                if (list.Exists(currentPartX.Contains) && list.Exists(currentPartY.Contains))
                {
                    matchedList = list;
                    break;
                }
            }

            return matchedList;
        }



        /// <summary>
        /// 默认比较
        /// </summary>
        private static int DefaultCompare(string x, string y)
        {
            return string.Compare(x, y, false, CultureInfo.CurrentCulture);
        }







        /// <summary>
        /// 插入默认的下拉 请选择
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="list"></param>
        private static void InsertSelectItem<T>(string key, string value, List<T> list)
        {
            bool haveDefautValue = false;
            if (list == null)
            {
                return;
            }
            foreach (var item in list)
            {
                if (item.GetPropertyValue(value) == null || item.GetPropertyValue(value).ToString() == "请选择")
                {
                    haveDefautValue = true;
                    break;
                }
            }
            if (!haveDefautValue)
            {
                object defaultSelectObj = Activator.CreateInstance(typeof(T));
                if (defaultSelectObj.GetType().GetProperty(key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).PropertyType.FullName.Contains("Int64"))
                {
                    defaultSelectObj.SetPropertyValue(key, -1L);//long類型， F、f 或 M
                }
                else
                {
                    defaultSelectObj.SetPropertyValue(key, -1);
                }


                defaultSelectObj.SetPropertyValue(value, "请选择");
                list.Insert(0, (T)defaultSelectObj);
            }
        }


        private static void InsertSelectItem(string key, string value, IEnumerable list, Type type)
        {
            bool haveDefautValue = false;
            foreach (var item in list)
            {
                if (item.GetPropertyValue(value).ToString() == "请选择")
                {
                    haveDefautValue = true;
                    break;
                }
            }
            if (!haveDefautValue)
            {
                object defaultSelectObj = Activator.CreateInstance(type);
                defaultSelectObj.SetPropertyValue(key, -1);
                defaultSelectObj.SetPropertyValue(value, "请选择");
                // list.CastToList < type.GetType() >.Insert(0, defaultSelectObj);
            }
        }


        #region 原生cmb的绑定




        /// <summary>
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
        /// <param name="add请选择">  是数据源绑定形式不可以自由添加，只能在数据源上做文章 ("请选择", "-1")</param>
        public static void InitCmb(BindingSource bs, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add请选择)
        {
            cmb.BeginUpdate();
            cmb.DataSource = bs;
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;
            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();

            foreach (var dr in bs.List)
            {
                sc.Add(ReflectionHelper.GetPropertyValue(dr, DisplayMember).ToString());
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {

                    cmb.AutoCompleteCustomSource = sc;
                    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }

            cmb.EndUpdate();

            cmb.SelectedIndex = -1;
        }


        #endregion







        #region 









        #endregion




        #region 枚举绑定数据源的新实现 可以select枚举 选择对应的集合来使用
        public enum RULE
        {
            [Description("Любые, без ограничений")]
            any,
            [Description("Любые если будет три в ряд")]
            anyThree,
            [Description("Соседние, без ограничений")]
            nearAny,
            [Description("Соседние если будет три в ряд")]
            nearThree
        }

        public static object Values
        {
            get
            {
                List<object> list = new List<object>();
                foreach (RULE rule in Enum.GetValues(typeof(RULE)))
                {
                    string desc = rule.GetType().GetMember(rule.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description;
                    list.Add(new { value = rule, desc = desc });
                }
                return list;
            }
        }

        #endregion











    }


}
