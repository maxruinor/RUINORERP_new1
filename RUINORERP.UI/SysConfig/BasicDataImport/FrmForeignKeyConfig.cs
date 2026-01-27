using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.UI.Common;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 外键配置对话框
    /// 用于配置字段的外键关系
    /// </summary>
    public partial class FrmForeignKeyConfig : KryptonForm
    {
        /// <summary>
        /// 当前映射配置
        /// </summary>
        public ColumnMapping CurrentMapping { get; set; }

        /// <summary>
        /// 是否为外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 关联表名
        /// </summary>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// 关联表字段
        /// </summary>
        public string RelatedTableField { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmForeignKeyConfig()
        {
            InitializeComponent();
            LoadRelatedTables();
        }

        /// <summary>
        /// 加载关联表列表
        /// </summary>
        private void LoadRelatedTables()
        {
            try
            {
                // 添加支持的关联表
                kcmbRelatedTable.Items.Clear();
                kcmbRelatedTable.Items.Add("请选择");
                kcmbRelatedTable.Items.Add("供应商表 (tb_Supplier)");
                kcmbRelatedTable.Items.Add("产品类目表 (tb_ProdCategories)");
                kcmbRelatedTable.Items.Add("产品信息表 (tb_Prod)");
                kcmbRelatedTable.Items.Add("产品属性表 (tb_ProdProperty)");
                kcmbRelatedTable.Items.Add("产品属性值表 (tb_ProdPropertyValue)");
                kcmbRelatedTable.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载关联表列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void FrmForeignKeyConfig_Load(object sender, EventArgs e)
        {
            if (CurrentMapping != null)
            {
                // 初始化当前配置
                kchkIsForeignKey.Checked = CurrentMapping.IsForeignKey;
                IsForeignKey = CurrentMapping.IsForeignKey;
                
                // 初始化关联表信息
                if (!string.IsNullOrEmpty(CurrentMapping.RelatedTableName))
                {
                    // 查找对应的显示文本
                    for (int i = 0; i < kcmbRelatedTable.Items.Count; i++)
                    {
                        string itemText = kcmbRelatedTable.Items[i].ToString();
                        if (itemText.Contains(CurrentMapping.RelatedTableName))
                        {
                            kcmbRelatedTable.SelectedIndex = i;
                            break;
                        }
                    }
                }
                
                RelatedTableName = CurrentMapping.RelatedTableName;
                RelatedTableField = CurrentMapping.RelatedTableField;
                ktxtRelatedField.Text = CurrentMapping.RelatedTableField;
            }
            
            // 更新控件状态
            UpdateControlStates();
        }

        /// <summary>
        /// 更新控件状态
        /// </summary>
        private void UpdateControlStates()
        {
            bool isForeignKey = kchkIsForeignKey.Checked;
            kcmbRelatedTable.Enabled = isForeignKey;
            ktxtRelatedField.Enabled = isForeignKey;
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (kchkIsForeignKey.Checked)
            {
                if (kcmbRelatedTable.SelectedIndex <= 0)
                {
                    MessageBox.Show("请选择关联表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(ktxtRelatedField.Text))
                {
                    MessageBox.Show("请输入关联表字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 获取关联表名（提取括号中的表名）
                string selectedTable = kcmbRelatedTable.SelectedItem.ToString();
                int startIndex = selectedTable.IndexOf('(') + 1;
                int endIndex = selectedTable.IndexOf(')');
                if (startIndex > 0 && endIndex > startIndex)
                {
                    RelatedTableName = selectedTable.Substring(startIndex, endIndex - startIndex);
                }
                else
                {
                    RelatedTableName = selectedTable;
                }
                
                RelatedTableField = ktxtRelatedField.Text;
            }
            else
            {
                RelatedTableName = string.Empty;
                RelatedTableField = string.Empty;
            }
            
            IsForeignKey = kchkIsForeignKey.Checked;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 是否为外键复选框点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kchkIsForeignKey_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }
    }
}
