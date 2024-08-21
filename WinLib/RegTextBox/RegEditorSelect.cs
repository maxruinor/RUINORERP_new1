using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinLib.RegTextBox
{
    public partial class RegEditorSelect : Form
    {
        public RegEditorSelect()
        {
            InitializeComponent();

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                regSelect = dataGridView1.CurrentRow.DataBoundItem as RegularAuthenticationSettings;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (dataGridView1.CurrentRow != null)
            {
                //regSelect = dataGridView1.CurrentRow.DataBoundItem as RegularAuthenticationSettings;
            }
        }

        public List<RegularAuthenticationSettings> RegList = new List<RegularAuthenticationSettings>();


        /// <summary>
        /// 选择中的结果
        /// </summary>
        public RegularAuthenticationSettings regSelect { get; set; }

        private List<RegularAuthenticationSettings> regList { get; set; }
        private void RegEditorSelect_Load(object sender, EventArgs e)
        {
            RegularAuthenticationSettings rs = new RegularAuthenticationSettings();
            dataGridView1.DataSource = rs.CreateRegList(); ;
            if (regSelect != null)
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if ((dr.DataBoundItem as RegularAuthenticationSettings).RegDescription == regSelect.RegDescription)
                    {
                        dr.Selected = true;
                    }
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                regSelect = dataGridView1.CurrentRow.DataBoundItem as RegularAuthenticationSettings;
                this.Close();
            }
        }
    }
}
