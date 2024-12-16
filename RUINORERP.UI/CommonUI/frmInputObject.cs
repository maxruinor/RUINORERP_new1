using FastReport.Table;
using FluentValidation;
using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.CommonUI
{
    /// <summary>
    /// 选择一个对象实体
    /// </summary>
    public partial class frmInputObject : Krypton.Toolkit.KryptonForm
    {
        private ICustomValidationRule _rule;
        public frmInputObject(ICustomValidationRule rule)
        {
            InitializeComponent();
            _rule = rule;
        }

        public string DefaultTitle { get; set; } = string.Empty;

        public string InputContent { get; set; }
        /// <summary>
        /// 选择
        /// </summary>
      // public BaseEntity SourceObject { get; set; }
        private void frmInputContent_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DefaultTitle))
            {
                this.Text = DefaultTitle;
            }

        }
 

        private void btnOk_Click(object sender, EventArgs e)
        {
            InputContent = txtInputContent.Text;
            // 5. 执行验证
            if (_rule.Validate(InputContent))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("请输入正确的格式。");
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
