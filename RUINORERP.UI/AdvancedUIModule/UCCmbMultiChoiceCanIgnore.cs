using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using RUINORERP.Business;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using Krypton.Toolkit;
using RUINORERP.IServices;
using RUINORERP.Business.Processor;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.Cache;
using RUINOR.WinFormsUI.ChkComboBox;

namespace RUINORERP.UI.AdvancedUIModule
{
    /// <summary>
    /// 带使用状态的多选，可以忽略。勾选生效
    /// </summary>
    public partial class UCCmbMultiChoiceCanIgnore : UserControl
    {
        public UCCmbMultiChoiceCanIgnore()
        {
            InitializeComponent();
            chkCanIgnore.CheckedChanged += chkCanIgnore_CheckedChanged;

            // 设置按钮样式以更好地显示图片
            btnRef.BackgroundImageLayout = ImageLayout.Center;
            btnRef.FlatStyle = FlatStyle.Flat;
            btnRef.FlatAppearance.BorderSize = 0;
            btnRef.BackColor = Color.Transparent;
            btnRef.ImageAlign = ContentAlignment.MiddleCenter;
            btnRef.TextAlign = ContentAlignment.MiddleCenter;
            btnRef.Cursor = Cursors.Hand; // 鼠标悬停时显示手型光标

            // 添加鼠标悬停效果
            btnRef.MouseEnter += (sender, e) =>
            {
                btnRef.BackColor = Color.FromArgb(240, 240, 240); // 浅灰色背景
            };
            btnRef.MouseLeave += (sender, e) =>
            {
                btnRef.BackColor = Color.Transparent;
            };

            // 设置ComboBox样式
            chkMulti.FlatStyle = FlatStyle.Flat;

            // 为chkMulti添加边框样式
            chkMulti.BackColor = Color.White;
            chkMulti.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        ContextMenuStrip contentMenu1;
        private void chkCanIgnore_CheckedChanged(object sender, EventArgs e)
        {
            kryptonPanelQuery.Visible = chkCanIgnore.Checked;
        }

        private void UCCmbMultiChoiceCanIgnore_Load(object sender, EventArgs e)
        {
            chkCanIgnore.Checked = true;

            contentMenu1 = new ContextMenuStrip();
            contentMenu1.Items.Add("全选");
            contentMenu1.Items.Add("全不选");
            contentMenu1.Items.Add("反选");
            contentMenu1.Items[0].Click += new EventHandler(contentMenu1_CheckAll);
            contentMenu1.Items[1].Click += new EventHandler(contentMenu1_CheckNo);
            contentMenu1.Items[2].Click += new EventHandler(contentMenu1_Inverse);

            chkMulti.ContextMenuStrip = contentMenu1;

            // 为chkMulti的容器Panel设置样式
            panelForBorder.BackColor = Color.White;
        }

        private void contentMenu1_CheckAll(object sender, EventArgs e)
        {
            foreach (ListViewItem item in chkMulti.Items)
                item.Checked = true;
        }
        private void contentMenu1_CheckNo(object sender, EventArgs e)
        {
            foreach (ListViewItem item in chkMulti.Items)
            {
                item.Checked = false;
            }
        }
        private void contentMenu1_Inverse(object sender, EventArgs e)
        {
            foreach (ListViewItem item in chkMulti.Items)
            {
                if (item.Checked == true)
                    item.Checked = false;
                else
                    item.Checked = true;
            }

        }

        /// <summary>
        /// 实体类型，用于查询窗口
        /// </summary>
        private Type _targetEntityType;

        /// <summary>
        /// 获取或设置目标实体类型
        /// </summary>
        public Type TargetEntityType
        {
            get { return _targetEntityType; }
            set { _targetEntityType = value; }
        }

        /// <summary>
        /// 查询过滤器，用于限制查询结果
        /// </summary>
        private QueryFilter _queryFilter;

        /// <summary>
        /// 获取或设置查询过滤器
        /// </summary>
        public QueryFilter QueryFilter
        {
            get { return _queryFilter; }
            set { _queryFilter = value; }
        }

        /// <summary>
        /// 点击查询按钮时触发，打开实体查询窗口
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnRef_Click(object sender, EventArgs e)
        {
            if (_targetEntityType == null)
            {
                // 尝试从Tag属性获取实体类型
                if (this.Tag != null && this.Tag is Type)
                {
                    _targetEntityType = this.Tag as Type;
                }
                else
                {
                    MessageBox.Show("未设置目标实体类型，无法打开查询窗口。", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            try
            {
                // 获取实体表名
                string entityTypeName = _targetEntityType.Name;

                // 检查用户权限
                tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.EntityName == entityTypeName);
                if (menuinfo == null)
                {
                    MessageBox.Show("您没有执行此菜单的权限，或配置参数不正确。请联系管理员。", "权限提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 创建查询窗口和列表控件
                BaseUControl ucBaseList = null;
                bool canEdit = false; // 默认为只读查询模式

                if (canEdit)
                {
                    // 编辑模式（如果需要）
                    ucBaseList = Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);
                    ucBaseList.QueryConditionFilter = _queryFilter;
                }
                else
                {
                    // 查询模式
                    Type genericType = typeof(UCAdvFilterGeneric<>).MakeGenericType(_targetEntityType);
                    ucBaseList = Activator.CreateInstance(genericType) as BaseUControl;
                    if (ucBaseList != null)
                    {
                        ucBaseList.QueryConditionFilter = _queryFilter;
                        ucBaseList.Tag = this;
                    }
                }

                if (ucBaseList != null)
                {
                    // 设置为选择模式
                    ucBaseList.Runway = BaseListRunWay.选中模式;

                    // 创建并配置查询窗口
                    frmBaseEditList frmedit = new frmBaseEditList();
                    frmedit.StartPosition = FormStartPosition.CenterScreen;
                    ucBaseList.Dock = DockStyle.Fill;
                    frmedit.kryptonPanel1.Controls.Add(ucBaseList);

                    // 设置窗口标题
                    var bizType = Business.BizMapperService.EntityMappingHelper.GetBizType(entityTypeName);
                    string BizTypeText = string.Empty;
                    // 如果业务类型为"无对应数据"，则尝试获取实体的描述信息
                    if (bizType == RUINORERP.Global.BizType.无对应数据)
                    {
                        BizTypeText = Business.BizMapperService.EntityMappingHelper.GetEntityDescription(_targetEntityType);
                    }
                    else
                    {
                        BizTypeText = bizType.ToString();
                    }
                    frmedit.Text = "关联查询" + "-" + BizTypeText;

                    // 显示窗口并处理选择结果
                    if (frmedit.ShowDialog() == DialogResult.OK)
                    {
                        ProcessSelectedResults(ucBaseList);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开查询窗口时发生错误：" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 处理从查询窗口返回的选择结果
        /// </summary>
        /// <param name="ucBaseList">查询窗口的列表控件</param>
        private void ProcessSelectedResults(BaseUControl ucBaseList)
        {
            if (ucBaseList.Tag == null)
                return;

            try
            {
                // 获取选择的数据
                BindingSource bs = ucBaseList.ListDataSoure as BindingSource;
                if (bs != null && bs.List != null && bs.List.Count > 0)
                {
                    // 保存现有的选中项，确保不丢失
                    List<object> existingSelectedValues = new List<object>();
                    if (chkMulti.MultiChoiceResults != null)
                    {
                        existingSelectedValues.AddRange(chkMulti.MultiChoiceResults);
                    }

                    string valueField = string.Empty;
                    string displayField = string.Empty;

                    //// 尝试从CheckBoxComboBox的绑定信息中获取字段名
                    //if (chkMulti.DataBindings.Count > 0)
                    //{
                    //    Binding binding = chkMulti.DataBindings[0];
                    //    valueField = binding.BindingMemberInfo.BindingField;
                    //}
                    valueField = chkMulti.KeyFieldName;
                    // 如果没有绑定信息，尝试获取主键字段
                    if (string.IsNullOrEmpty(valueField))
                    {
                        var tableSchema = TableSchemaManager.Instance.GetSchemaInfo(_targetEntityType.Name);
                        if (tableSchema != null)
                        {
                            valueField = tableSchema.PrimaryKeyField;
                            displayField = tableSchema.DisplayField;
                        }
                    }

                    if (!string.IsNullOrEmpty(valueField))
                    {
                        // 处理多选情况，获取所有选中项
                        List<object> selectedValues = new List<object>();
                        foreach (var item in bs.List)
                        {
                            if (item != null)
                            {
                                object selectValue = ReflectionHelper.GetPropertyValue(item, valueField);
                                // 忽略空值和空字符串
                                if (selectValue != null && !string.IsNullOrEmpty(selectValue.ToString()))
                                {
                                    selectedValues.Add(selectValue);
                                }
                            }
                        }

                        // 创建新的MultiChoiceResults集合，包含原有的和新选择的项
                        List<object> newSelectedValues = new List<object>(existingSelectedValues);
                        foreach (var selectValue in selectedValues)
                        {
                            if (!newSelectedValues.Contains(selectValue))
                            {
                                newSelectedValues.Add(selectValue);
                            }
                        }

                        // 更新MultiChoiceResults
                        chkMulti.MultiChoiceResults.Clear();
                        foreach (var value in newSelectedValues)
                        {
                            // 再次检查，确保不添加空值和空字符串
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                chkMulti.MultiChoiceResults.Add(value);
                            }
                        }

                        // 使用CheckBoxComboBox提供的UpdateCheckedStates方法更新所有复选框的选中状态
                        // 这个方法会根据MultiChoiceResults自动同步UI
                        chkMulti.UpdateCheckedStates();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("处理选择结果时发生错误：" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
