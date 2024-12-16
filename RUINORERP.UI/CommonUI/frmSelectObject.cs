using FastReport.Table;
using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Model;
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
    public partial class frmSelectObject : Krypton.Toolkit.KryptonForm
    {
        public frmSelectObject()
        {
            InitializeComponent();
        }

        public string DefaultTitle { get; set; } = string.Empty;

        public string SelectItemText { get; set; }
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


        /// <summary>
        /// 设置选择项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DataEntity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        public void SetSelectDataList<T>(BaseEntity DataEntity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue) where T : BaseEntity
        {
            DataBindingHelper.BindData4Cmb<T>(DataEntity, expkey, expValue, cmbSelectedList);
            //、、string FieldKey, string FieldName
            //  DataBindingHelper.InitDataToCmb<T>(expkey, expValue, cmbSelectedList);
            // DataBindingHelper.InitDataToCmb<T>(FieldKey, FieldName, nameof(T), cmbSelectedList);
            // base.InitRequiredToControl(new tb_FM_AccountValidator(), kryptonPanel1.Controls);
            // base.InitEditItemToControl(entity, kryptonPanel1.Controls);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
          
            if (cmbSelectedList.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要选择的对象！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbSelectedList.SelectedItem is not BaseEntity)
            {
                MessageBox.Show("请选择要选择的对象！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SelectItemText = cmbSelectedList.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
