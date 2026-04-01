using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.QueryPanel
{
    /// <summary>
    /// 鏌ヨ闈㈡澘鐢熸垚鍣?
    /// </summary>
    public class QueryPanelGenerator
    {
        /// <summary>
        /// 鐢熸垚鏌ヨ闈㈡澘
        /// </summary>
        public static FlowLayoutPanel Generate<TParam>(TParam parameters) where TParam : class
        {
            var panel = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10),
                Dock = DockStyle.Top
            };

            var properties = typeof(TParam).GetProperties()
                .Where(p => p.GetCustomAttribute<QueryParameterAttribute>() != null)
                .OrderBy(p => p.GetCustomAttribute<QueryParameterAttribute>()?.Order ?? 0);

            foreach (var prop in properties)
            {
                var control = CreateControlForProperty(prop, parameters);
                if (control != null)
                {
                    panel.Controls.Add(control);
                }
            }

            // 娣诲姞鏌ヨ鎸夐挳
            var btnQuery = new Button
            {
                Text = "鏌ヨ",
                AutoSize = true,
                Margin = new Padding(10, 10, 0, 0),
                Tag = "QueryButton"
            };
            panel.Controls.Add(btnQuery);

            return panel;
        }

        private static Control CreateControlForProperty(PropertyInfo prop, object instance)
        {
            var attr = prop.GetCustomAttribute<QueryParameterAttribute>();
            if (attr == null)
                return null;

            var value = prop.GetValue(instance);
            var labelText = string.IsNullOrWhiteSpace(attr.DisplayName) ? prop.Name : attr.DisplayName;

            return prop.PropertyType switch
            {
                Type t when t == typeof(DateTime) || t == typeof(DateTime?) =>
                    CreateDatePicker(prop, instance, value, labelText, attr),

                Type t when t.IsEnum =>
                    CreateComboBox(prop, instance, value, labelText, attr),

                Type t when t == typeof(long) || t == typeof(long?) ||
                           t == typeof(int) || t == typeof(int?) =>
                    CreateNumericUpDown(prop, instance, value, labelText, attr),

                Type t when t == typeof(string) && attr.ControlType == QueryControlType.ComboBox =>
                    CreateStringComboBox(prop, instance, value, labelText, attr),

                _ => CreateTextBox(prop, instance, value, labelText, attr)
            };
        }

        private static Control CreateDatePicker(PropertyInfo prop, object instance, object value,
            string labelText, QueryParameterAttribute attr)
        {
            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Margin = new Padding(5, 8, 5, 5)
            };

            var dtp = new DateTimePicker
            {
                Value = value as DateTime? ?? DateTime.Now,
                MinimumSize = new System.Drawing.Size(150, 20),
                Margin = new Padding(5, 5, 5, 5),
                Tag = prop
            };

            dtp.ValueChanged += (sender, e) =>
            {
                prop.SetValue(instance, dtp.Value);
            };

            var panel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0)
            };
            panel.Controls.Add(label);
            panel.Controls.Add(dtp);

            return panel;
        }

        private static Control CreateComboBox(PropertyInfo prop, object instance, object value,
            string labelText, QueryParameterAttribute attr)
        {
            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Margin = new Padding(5, 8, 5, 5)
            };

            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                MinimumSize = new System.Drawing.Size(150, 20),
                Margin = new Padding(5, 5, 5, 5),
                Tag = prop
            };
            
            // 绑定枚举值
            var enumType = prop.PropertyType;
            var values = Enum.GetValues(enumType);
            foreach (var enumValue in values)
            {
                var description = GetEnumDescription(enumValue);
                cmb.Items.Add(new KeyValuePair<object, string>(enumValue, description));
            }

            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";

            if (value != null)
            {
                cmb.SelectedValue = value;
            }

            cmb.SelectedIndexChanged += (sender, e) =>
            {
                if (cmb.SelectedItem is KeyValuePair<object, string> kvp)
                {
                    prop.SetValue(instance, kvp.Key);
                }
            };

            var panel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0)
            };
            panel.Controls.Add(label);
            panel.Controls.Add(cmb);

            return panel;
        }

        private static Control CreateNumericUpDown(PropertyInfo prop, object instance, object value,
            string labelText, QueryParameterAttribute attr)
        {
            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Margin = new Padding(5, 8, 5, 5)
            };

            var nud = new NumericUpDown
            {
                Minimum = attr.MinValue ?? decimal.MinValue,
                Maximum = attr.MaxValue ?? decimal.MaxValue,
                Value = value as decimal? ?? 0,
                MinimumSize = new System.Drawing.Size(100, 20),
                Margin = new Padding(5, 5, 5, 5),
                Tag = prop
            };

            nud.ValueChanged += (sender, e) =>
            {
                prop.SetValue(instance, nud.Value);
            };

            var panel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0)
            };
            panel.Controls.Add(label);
            panel.Controls.Add(nud);

            return panel;
        }

        private static Control CreateTextBox(PropertyInfo prop, object instance, object value,
            string labelText, QueryParameterAttribute attr)
        {
            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Margin = new Padding(5, 8, 5, 5)
            };

            var txt = new TextBox
            {
                Text = value?.ToString() ?? "",
                MinimumSize = new System.Drawing.Size(150, 20),
                Margin = new Padding(5, 5, 5, 5),
                Tag = prop
            };

            txt.TextChanged += (sender, e) =>
            {
                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(instance, txt.Text);
                }
                else if (prop.PropertyType == typeof(long?) || prop.PropertyType == typeof(long))
                {
                    if (long.TryParse(txt.Text, out var result))
                    {
                        prop.SetValue(instance, result);
                    }
                }
            };

            var panel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0)
            };
            panel.Controls.Add(label);
            panel.Controls.Add(txt);

            return panel;
        }

        private static Control CreateStringComboBox(PropertyInfo prop, object instance, object value,
            string labelText, QueryParameterAttribute attr)
        {
            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Margin = new Padding(5, 8, 5, 5)
            };

            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                MinimumSize = new System.Drawing.Size(150, 20),
                Margin = new Padding(5, 5, 5, 5),
                Tag = prop
            };

            // TODO: 浠庢暟鎹簮鍔犺浇閫夐」
            if (!string.IsNullOrWhiteSpace(attr.DataSource))
            {
                // 杩欓噷鍙互浠庢暟鎹簱鎴栭厤缃姞杞介€夐」
                cmb.Items.Add(value?.ToString() ?? "");
            }

            if (value != null)
            {
                cmb.Items.Add(value.ToString());
            }

            cmb.SelectedIndexChanged += (sender, e) =>
            {
                prop.SetValue(instance, cmb.SelectedItem);
            };

            var panel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0)
            };
            panel.Controls.Add(label);
            panel.Controls.Add(cmb);

            return panel;
        }

        private static string GetEnumDescription(object enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? enumValue.ToString();
        }
    }

    /// <summary>
    /// 鏌ヨ鍙傛暟鐗规€?
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryParameterAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public int Order { get; set; } = 0;
        public QueryControlType ControlType { get; set; } = QueryControlType.Auto;
        public string DataSource { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }

    /// <summary>
    /// 鏌ヨ鎺т欢绫诲瀷
    /// </summary>
    public enum QueryControlType
    {
        Auto,
        TextBox,
        ComboBox,
        DatePicker,
        Numeric
    }
}

