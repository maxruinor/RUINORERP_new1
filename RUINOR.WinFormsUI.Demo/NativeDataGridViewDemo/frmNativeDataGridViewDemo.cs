using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.Demo.NativeDataGridViewDemo
{
    public partial class frmNativeDataGridViewDemo : Form
    {
        private BindingSource bindingSourceList;
        private List<RolePermissionDto> testDataList;

        public frmNativeDataGridViewDemo()
        {
            InitializeComponent();
            InitializeTestData();
            InitializeDataGridView();
        }

        /// <summary>
        /// 初始化测试数据（模拟 UCUserAuthorization 中的角色权限数据）
        /// </summary>
        private void InitializeTestData()
        {
            testDataList = new List<RolePermissionDto>
            {
                new RolePermissionDto { RoleID = 1, RoleName = "系统管理员", Authorized = true, DefaultRole = false, Description = "拥有所有权限" },
                new RolePermissionDto { RoleID = 2, RoleName = "部门经理", Authorized = true, DefaultRole = true, Description = "部门管理权限" },
                new RolePermissionDto { RoleID = 3, RoleName = "普通员工", Authorized = false, DefaultRole = false, Description = "基本操作权限" },
                new RolePermissionDto { RoleID = 4, RoleName = "财务人员", Authorized = true, DefaultRole = false, Description = "财务管理权限" },
                new RolePermissionDto { RoleID = 5, RoleName = "采购人员", Authorized = false, DefaultRole = false, Description = "采购管理权限" }
            };
        }

        /// <summary>
        /// 初始化 DataGridView（使用原生控件配置）
        /// </summary>
        private void InitializeDataGridView()
        {
            // 设置数据源
            bindingSourceList = new BindingSource();
            bindingSourceList.DataSource = testDataList;
            dataGridView1.DataSource = bindingSourceList;

            // 配置列属性
            ConfigureDataGridViewColumns();

            // 设置网格基本属性
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = false; // 允许编辑
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }

        /// <summary>
        /// 配置 DataGridView 列（关键：确保布尔属性正确映射为 CheckBoxColumn）
        /// </summary>
        private void ConfigureDataGridViewColumns()
        {
            // 方法 1: 让 DataGridView 自动根据布尔类型生成 CheckBoxColumn
            // 如果要手动控制，也可以显式创建列
            
            // 这里我们先让系统自动生成列，然后调整属性
            if (dataGridView1.Columns.Count > 0)
            {
                // 调整各列属性
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    switch (col.Name)
                    {
                        case "RoleID":
                            col.HeaderText = "角色 ID";
                            col.Width = 80;
                            break;
                        case "RoleName":
                            col.HeaderText = "角色名称";
                            col.Width = 120;
                            break;
                        case "Authorized":
                            col.HeaderText = "已授权";
                            col.Width = 80;
                            // 确保是 CheckBoxColumn
                            if (!(col is DataGridViewCheckBoxColumn))
                            {
                                // 如果自动生成的不是 CheckBoxColumn，需要手动替换
                                ReplaceWithCheckBoxColumn(col.Index, "Authorized");
                            }
                            break;
                        case "DefaultRole":
                            col.HeaderText = "默认角色";
                            col.Width = 80;
                            // 确保是 CheckBoxColumn
                            if (!(col is DataGridViewCheckBoxColumn))
                            {
                                ReplaceWithCheckBoxColumn(col.Index, "DefaultRole");
                            }
                            break;
                        case "Description":
                            col.HeaderText = "描述";
                            col.Width = 200;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定列替换为 CheckBoxColumn（如果自动生成的列类型不正确）
        /// </summary>
        private void ReplaceWithCheckBoxColumn(int columnIndex, string dataPropertyName)
        {
            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = dataPropertyName,
                HeaderText = columnIndex == FindColumnIndexByDataProperty("Authorized") ? "已授权" : "默认角色",
                Width = 80,
                TrueValue = true,
                FalseValue = false,
                ThreeState = false
            };

            // 插入新列并移除旧列
            dataGridView1.Columns.Insert(columnIndex, checkBoxColumn);
            if (columnIndex + 1 < dataGridView1.Columns.Count)
            {
                dataGridView1.Columns.RemoveAt(columnIndex + 1);
            }
        }

        /// <summary>
        /// 根据数据属性名查找列索引
        /// </summary>
        private int FindColumnIndexByDataProperty(string dataPropertyName)
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].DataPropertyName == dataPropertyName)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 按钮：全选授权
        /// </summary>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            bindingSourceList.EndEdit();
            foreach (RolePermissionDto item in testDataList)
            {
                item.Authorized = true;
            }
            dataGridView1.Refresh();
            lblStatus.Text = $"已全选授权 {testDataList.Count} 条记录";
        }

        /// <summary>
        /// 按钮：取消全选
        /// </summary>
        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            bindingSourceList.EndEdit();
            foreach (RolePermissionDto item in testDataList)
            {
                item.Authorized = false;
            }
            dataGridView1.Refresh();
            lblStatus.Text = $"已取消全选 {testDataList.Count} 条记录";
        }

        /// <summary>
        /// 按钮：获取选中状态
        /// </summary>
        private void btnGetSelectedState_Click(object sender, EventArgs e)
        {
            bindingSourceList.EndEdit();
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("当前授权状态:");
            sb.AppendLine("-------------------");
            
            foreach (RolePermissionDto item in testDataList)
            {
                sb.AppendLine($"角色：{item.RoleName}, 已授权：{item.Authorized}, 默认角色：{item.DefaultRole}");
            }

            MessageBox.Show(sb.ToString(), "授权状态", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblStatus.Text = "已查看当前授权状态";
        }

        /// <summary>
        /// 按钮：保存更改
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            bindingSourceList.EndEdit();
            
            int changedCount = testDataList.Count(item => item.Authorized);
            MessageBox.Show(
                $"保存成功！\r\n共 {testDataList.Count} 条记录\r\n已授权：{changedCount} 条\r\n未授权：{testDataList.Count - changedCount} 条",
                "保存提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            lblStatus.Text = $"已保存 {changedCount}/{testDataList.Count} 条授权记录";
        }

        /// <summary>
        /// 按钮：关闭窗体
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// DataGridView 单元格内容格式化事件（备用：用于自定义显示）
        /// </summary>
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 如果需要自定义 CheckBox 的显示，可以在此处理
            // 注意：不要在这里修改 Value 值，否则会影响交互
        }

        /// <summary>
        /// DataGridView 单元格点击事件（用于调试）
        /// </summary>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dataGridView1.Columns[e.ColumnIndex];
                if (column is DataGridViewCheckBoxColumn)
                {
                    // CheckBox 列被点击
                    var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                    if (cell != null && cell.Value != null && cell.Value != DBNull.Value)
                    {
                        bool isChecked = (bool)cell.Value;
                        lblStatus.Text = $"行 {e.RowIndex + 1} - {column.HeaderText}: {(isChecked ? "✓" : "✗")}";
                    }
                }
            }
        }
    }
}
