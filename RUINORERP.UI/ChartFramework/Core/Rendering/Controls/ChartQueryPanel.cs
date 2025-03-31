using Netron.GraphLib;
using RUINORERP.Model;
using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Controls
{
    public partial class ChartQueryPanel : UserControl
    {
        public event EventHandler<DataRequest> QueryRequested;
        public ChartQueryPanel()
        {
            InitializeComponent();
            InitializeControls();
        }

        public DataRequest request { get; set; }
        private void InitializeControls()
        {
            // 1. 时间范围选择
            var timeRangeGroup = new GroupBox { Text = "时间范围", Dock = DockStyle.Top };
            var cmbRangeType = new ComboBox
            {
                DataSource = Enum.GetValues(typeof(DataRequest.DateTimeRange)),
                Dock = DockStyle.Left
            };
            //var dtpStart = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd" };
            //var dtpEnd = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd" };
            //// ...布局代码...

            // 2. 维度选择
            var dimGroup = new GroupBox { Text = "分组维度" };
            var clbDimensions = new CheckedListBox
            {
                DataSource = GetAvailableDimensions(),
                DisplayMember = "DisplayName"
            };
            // ...布局代码...

            // 3. 指标选择
            var metricGroup = new GroupBox { Text = "统计指标" };
            var dgvMetrics = new DataGridView
            {
                DataSource = new BindingList<MetricConfig>(GetAvailableMetrics()),
                AutoGenerateColumns = false
            };
            // ...配置列...

            // 4. 图表类型
            var chartTypeGroup = new GroupBox { Text = "图表设置" };
            var cmbChartType = new ComboBox
            {
                DataSource = Enum.GetValues(typeof(ChartType))
            };
            var cmbValueType = new ComboBox
            {
                DataSource = Enum.GetValues(typeof(ValueType))
            };
            // ...布局代码...

            // 查询按钮
            var btnQuery = new Button { Text = "生成图表", Dock = DockStyle.Bottom };
            btnQuery.Click += (s, e) =>
            {
                var request = BuildRequest();
                if (request.Validate(out var error))
                    QueryRequested?.Invoke(this, request);
                else
                    MessageBox.Show(error);
            };


            //DataBindingHelper.BindData4Cmb<tb_Department>(request, k => k.c, v => v.DepartmentName, cmbDepartment);
            DataBindingHelper.BindData4DataTime<DataRequest>(request, t => t.StartTime, dtpStart, false);
            DataBindingHelper.BindData4DataTime<DataRequest>(request, t => t.EndTime, dtpEnd, false);
            //   DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.JobTitle, txtJobTitle, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<DataRequest>(request, t => t.TimeRange, cmbRangeType, BindDataType4Enum.EnumName, typeof(TimeRangeType));
            Controls.Add(timeRangeGroup);
            Controls.Add(dimGroup);
            Controls.Add(metricGroup);
            Controls.Add(chartTypeGroup);
            Controls.Add(btnQuery);
        }

        private DataRequest BuildRequest()
        {
            return new DataRequest
            {
                StartTime = dtpStart.Value,
                EndTime = dtpEnd.Value,
                //RangeType = (DataRequest.DateTimeRange)cmbRangeType.SelectedValue,
                //Dimensions = clbDimensions.CheckedItems.Cast<DataRequest.Dimension>().ToList(),
                //Metrics = ((BindingList<DataRequest.Metric>)dgvMetrics.DataSource).ToList(),
                //ChartType = (ChartType)cmbChartType.SelectedValue,
                //ValueType = (ValueType)cmbValueType.SelectedValue
            };
        }

        // 从配置或数据库加载可选维度
        private List<DimensionConfig> GetAvailableDimensions()
        {
            return new List<DimensionConfig>
        {
            new() { FieldName = "Employee_ID", DisplayName = "员工" },
            new() { FieldName = "Department_ID", DisplayName = "部门" },
            new() { FieldName = "Product_Code", DisplayName = "产品" },
            new() { FieldName = "Region", DisplayName = "区域", IsTimeBased = false },
            new() { FieldName = "Created_at", DisplayName = "周", IsTimeBased = true }
        };
        }

        // 从配置或数据库加载可选指标
        private List<MetricConfig> GetAvailableMetrics()
        {
            return new List<MetricConfig>
        {
            new() { FieldName = "Amount", DisplayName = "金额", Unit = MetricUnit.元 },
            new() { FieldName = "Count", DisplayName = "订单数", Unit = MetricUnit.笔 },
            new() { FieldName = "CustomerCount", DisplayName = "客户数", Unit = MetricUnit.人 }
        };
        }
    }
}
