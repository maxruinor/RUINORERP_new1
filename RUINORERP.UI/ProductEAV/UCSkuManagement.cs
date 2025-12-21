using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.UI.ProductEAV.Core;
using RUINORERP.Business;
using RUINORERP.Common.Helper;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// SKU管理组件
    /// 负责显示和编辑产品的SKU明细
    /// </summary>
    public partial class UCSkuManagement : UserControl
    {
        private long _prodBaseId;
        private SkuGenerationService _skuService;
        private ProductAttrService _attrService;
        private tb_ProdDetailController<tb_ProdDetail> _detailController;
        private List<tb_ProdDetail> _skuDetails;

        public UCSkuManagement()
        {
            InitializeComponent();
            _skuService = new SkuGenerationService();
            _attrService = new ProductAttrService();
            _detailController = Startup.GetFromFac<tb_ProdDetailController<tb_ProdDetail>>();
            InitializeDataGridView();
        }

        /// <summary>
        /// 初始化DataGridView
        /// </summary>
        private void InitializeDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AllowUserToAddRows = false;
            
            // 添加列
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = \"SKU\",
                HeaderText = \"SKU码\",
                Width = 120,
                ReadOnly = true
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = \"ProductSpec\",
                HeaderText = \"规格\",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = \"BarcodeNo\",
                HeaderText = \"条码\",
                Width = 120
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = \"Price\",
                HeaderText = \"售价\",
                Width = 80
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = \"CostPrice\",
                HeaderText = \"成本价\",
                Width = 80
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = \"StockQty\",
                HeaderText = \"库存\",
                Width = 80
            });

            // 删除列
            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = \"操作\";
            deleteColumn.Text = \"删除\";
            deleteColumn.UseColumnTextForButtonValue = true;
            deleteColumn.Width = 80;
            dataGridView.Columns.Add(deleteColumn);

            dataGridView.CellClick += DataGridView_CellClick;
        }

        /// <summary>
        /// 加载产品的SKU列表
        /// </summary>
        public async void LoadSkuDetails(long prodBaseId)
        {
            _prodBaseId = prodBaseId;
            
            if (prodBaseId <= 0) return;

            try
            {
                _skuDetails = await _attrService.GetProductDetailsAsync(prodBaseId);
                BindSkuData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(\$"加载SKU失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 绑定SKU数据到表格
        /// </summary>
        private void BindSkuData()
        {
            if (_skuDetails == null || _skuDetails.Count == 0)
            {
                dataGridView.DataSource = null;
                lblSkuCount.Text = \"暂无SKU\";
                return;
            }

            dataGridView.DataSource = _skuDetails;
            lblSkuCount.Text = \$\"共 {_skuDetails.Count} 个SKU\";
        }

        /// <summary>
        /// DataGridView单元格点击事件
        /// </summary>
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns.Count - 1 && e.RowIndex >= 0)
            {
                // 删除SKU
                if (MessageBox.Show(\"确定要删除这个SKU吗？\", \"确认\", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    var sku = _skuDetails[e.RowIndex];
                    _skuDetails.RemoveAt(e.RowIndex);
                    BindSkuData();
                }
            }
        }

        /// <summary>
        /// 生成缺失的SKU
        /// </summary>
        public async void GenerateMissingSkus()
        {
            if (_prodBaseId <= 0) return;

            try
            {
                // 调用Service生成SKU
                var newSkus = await _skuService.GenerateSkuDetailsFromAttributesAsync(_prodBaseId);
                
                if (newSkus != null && newSkus.Count > 0)
                {
                    // 重新加载
                    LoadSkuDetails(_prodBaseId);
                    MessageBox.Show(\$\"成功生成 {newSkus.Count} 个新SKU\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(\"未生成新SKU\", \"提示\", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(\$\"生成SKU失败: {ex.Message}\", \"错误\", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取所有SKU数据
        /// </summary>
        public List<tb_ProdDetail> GetSkuDetails()
        {
            return _skuDetails ?? new List<tb_ProdDetail>();
        }

        /// <summary>
        /// 刷新SKU列表
        /// </summary>
        public void RefreshSkuList()
        {
            LoadSkuDetails(_prodBaseId);
        }
    }
}
