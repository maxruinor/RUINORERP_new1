using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WinLib.RegTextBox
{
    public partial class RegEditorSelectControl : UserControl
    {
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource regularAuthenticationSettingsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn regDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn regularlyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorMessageDataGridViewTextBoxColumn;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RegEditorSelectControl()
        {

        }

        public List<RegularAuthenticationSettings> RegList = new List<RegularAuthenticationSettings>();

        private void RegEditorSelectControl_Load(object sender, EventArgs e)
        {
            RegList.Add(new RegularAuthenticationSettings(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "Email地址", "请输入正确的Email格式数据", "不能为空"));
            RegList.Add(new RegularAuthenticationSettings(@"^[\u4e00-\u9fa5]{0,}$", "汉字", "请输入汉字格式数据", "不能为空"));
            dataGridView1.DataSource = RegList;
        }

        /// <summary>
        /// 选择中的结果
        /// </summary>
        public RegularAuthenticationSettings regSelect { get; set; }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                regSelect = dataGridView1.CurrentRow.DataBoundItem as RegularAuthenticationSettings;
            }
        }
    }
}

