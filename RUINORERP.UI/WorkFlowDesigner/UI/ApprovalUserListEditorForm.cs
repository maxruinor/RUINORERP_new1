using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.WorkFlowDesigner.Entities;

namespace RUINORERP.UI.WorkFlowDesigner.UI
{
    /// <summary>
    /// 审批用户列表编辑器窗体
    /// </summary>
    public partial class ApprovalUserListEditorForm : Form
    {
        private List<ApprovalUser> _approvalUsers;
        
        public List<ApprovalUser> ApprovalUsers
        {
            get { return _approvalUsers; }
        }

        public ApprovalUserListEditorForm(List<ApprovalUser> approvalUsers)
        {
            InitializeComponent();
            _approvalUsers = approvalUsers ?? new List<ApprovalUser>();
            BindData();
        }

        private void BindData()
        {
            // 清空现有数据
            dataGridViewUsers.Rows.Clear();
            
            // 绑定审批用户列表到DataGridView
            foreach (var user in _approvalUsers)
            {
                int rowIndex = dataGridViewUsers.Rows.Add();
                DataGridViewRow row = dataGridViewUsers.Rows[rowIndex];
                row.Cells["UserId"].Value = user.Id;
                row.Cells["UserName"].Value = user.Name;
                row.Cells["DepartmentId"].Value = user.DepartmentId;
                row.Cells["DepartmentName"].Value = user.DepartmentName;
                row.Tag = user;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 添加新的审批用户
            ApprovalUser newUser = new ApprovalUser();
            newUser.Id = GetNewUserId();
            newUser.Name = "新用户";
            newUser.DepartmentId = 0;
            newUser.DepartmentName = "默认部门";
            
            _approvalUsers.Add(newUser);
            
            int rowIndex = dataGridViewUsers.Rows.Add();
            DataGridViewRow row = dataGridViewUsers.Rows[rowIndex];
            row.Cells["UserId"].Value = newUser.Id;
            row.Cells["UserName"].Value = newUser.Name;
            row.Cells["DepartmentId"].Value = newUser.DepartmentId;
            row.Cells["DepartmentName"].Value = newUser.DepartmentName;
            row.Tag = newUser;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // 删除选中的审批用户
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewUsers.SelectedRows[0];
                ApprovalUser user = selectedRow.Tag as ApprovalUser;
                if (user != null)
                {
                    _approvalUsers.Remove(user);
                }
                dataGridViewUsers.Rows.Remove(selectedRow);
            }
            else
            {
                MessageBox.Show("请选择要删除的用户。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // 保存修改
            foreach (DataGridViewRow row in dataGridViewUsers.Rows)
            {
                if (row.IsNewRow) continue;
                
                ApprovalUser user = row.Tag as ApprovalUser;
                if (user != null)
                {
                    user.Id = Convert.ToInt32(row.Cells["UserId"].Value ?? 0);
                    user.Name = Convert.ToString(row.Cells["UserName"].Value ?? "");
                    user.DepartmentId = Convert.ToInt32(row.Cells["DepartmentId"].Value ?? 0);
                    user.DepartmentName = Convert.ToString(row.Cells["DepartmentName"].Value ?? "");
                }
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private int GetNewUserId()
        {
            // 生成新的用户ID
            int maxId = 0;
            foreach (var user in _approvalUsers)
            {
                if (user.Id > maxId)
                {
                    maxId = user.Id;
                }
            }
            return maxId + 1;
        }

        private void dataGridViewUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 当单元格值改变时，更新对应的数据对象
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataGridViewUsers.Rows[e.RowIndex];
                ApprovalUser user = row.Tag as ApprovalUser;
                if (user != null)
                {
                    DataGridViewCell cell = row.Cells[e.ColumnIndex];
                    switch (dataGridViewUsers.Columns[e.ColumnIndex].Name)
                    {
                        case "UserId":
                            user.Id = Convert.ToInt32(cell.Value ?? 0);
                            break;
                        case "UserName":
                            user.Name = Convert.ToString(cell.Value ?? "");
                            break;
                        case "DepartmentId":
                            user.DepartmentId = Convert.ToInt32(cell.Value ?? 0);
                            break;
                        case "DepartmentName":
                            user.DepartmentName = Convert.ToString(cell.Value ?? "");
                            break;
                    }
                }
            }
        }
    }
}