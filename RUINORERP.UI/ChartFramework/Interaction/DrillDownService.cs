using RUINORERP.Model.ChartFramework.Contracts;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.Interaction
{
    /// <summary>
    /// 鏁版嵁鐐逛俊鎭?
    /// </summary>
    public class DataPoint
    {
        public string Label { get; set; }
        public double XValue { get; set; }
        public double YValue { get; set; }
        public string DimensionField { get; set; }
        public object Tag { get; set; }
    }

    /// <summary>
    /// 閽诲彇鏈嶅姟
    /// </summary>
    public class DrillDownService
    {
        private readonly IDataProvider _dataProvider;

        public DrillDownService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /// <summary>
        /// 鏄剧ず閽诲彇璇︽儏绐楀彛
        /// </summary>
        public async Task ShowDrillDownAsync(DataPoint point, DataRequest currentRequest)
        {
            try
            {
                // 鏋勫缓閽诲彇鏌ヨ
                var drillDownQuery = BuildDrillDownQuery(point, currentRequest);

                // 鑾峰彇璇︾粏鏁版嵁
                var detailData = await _dataProvider.GetDataAsync(drillDownQuery);

                // 鏄剧ず璇︽儏绐楀彛
                using var form = new DrillDownForm
                {
                    Text = $"閽诲彇璇︽儏 - {point.Label}",
                    Data = detailData
                };

                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"钻取失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 鏋勫缓閽诲彇鏌ヨ
        /// </summary>
        private DataRequest BuildDrillDownQuery(DataPoint point, DataRequest currentRequest)
        {
            return new DataRequest
            {
                ChartType = currentRequest.ChartType,
                TimeField = currentRequest.TimeField,
                StartTime = currentRequest.StartTime,
                EndTime = currentRequest.EndTime,
                Dimensions = new List<DimensionConfig>(), // 涓嶅啀鍒嗙粍锛屾樉绀烘槑缁?
                Metrics = currentRequest.Metrics,
                Filters = new List<FieldFilter>
                {
                    new FieldFilter
                    {
                        Field = point.DimensionField ?? GetFirstDimensionField(currentRequest),
                        Value = point.Label,
                        Operator = "="
                    }
                }
            };
        }

        private string GetFirstDimensionField(DataRequest request)
        {
            if (request.Dimensions == null || !request.Dimensions.Any())
                return "ID";

            return request.Dimensions.First().FieldName;
        }
    }

    /// <summary>
    /// 閽诲彇璇︽儏绐楀彛
    /// </summary>
    public class DrillDownForm : Form
    {
        private DataGridView _grid;
        public ChartData Data { get; set; }

        public DrillDownForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            this.Controls.Add(_grid);

            // 褰撹缃?Data 鏃跺～鍏呯綉鏍?
            this.Load += async (sender, e) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (Data == null)
                return;

            // TODO: 灏?ChartData 杞崲涓?DataTable 骞剁粦瀹氬埌 Grid
            // 杩欓噷鏆傛椂鏄剧ず鍗犱綅鏂囨湰
            _grid.Columns.Add("Series", "绯诲垪");
            _grid.Columns.Add("Value", "数值");

            if (Data.Series != null)
            {
                foreach (var series in Data.Series)
                {
                    for (int i = 0; i < series.Values.Count; i++)
                    {
                        _grid.Rows.Add(series.Name, series.Values[i]);
                    }
                }
            }
        }
    }
}

