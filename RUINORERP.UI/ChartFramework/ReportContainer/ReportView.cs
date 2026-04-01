using RUINORERP.Model.ChartFramework.Contracts;
using RUINORERP.Model.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.QueryPanel;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.ReportContainer
{
    /// <summary>
    /// 报表视图控件
    /// </summary>
    public class ReportView : UserControl
    {
        private readonly IReportDefinition _reportDefinition;
        private FlowLayoutPanel _queryPanel;
        private Panel _chartHost;
        private Button _btnQuery;
        private DataRequest _currentRequest;

        public ReportView(IReportDefinition reportDefinition)
        {
            _reportDefinition = reportDefinition;
            InitializeComponent();
            InitializeQueryPanel();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 图表宿主面板
            _chartHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            this.Controls.Add(_chartHost);
            this.ResumeLayout(false);
        }

        private void InitializeQueryPanel()
        {
            // 创建查询参数实例
            var paramType = _reportDefinition.QueryParameterType;
            var parameters = Activator.CreateInstance(paramType);

            // 生成查询面板
            _queryPanel = QueryPanelGenerator.Generate(parameters);
            _queryPanel.Dock = DockStyle.Top;

            // 绑定查询按钮事件
            _btnQuery = _queryPanel.Controls.Find("QueryButton", false).FirstOrDefault() as Button;
            if (_btnQuery != null)
            {
                _btnQuery.Click += async (sender, e) => await ExecuteQueryAsync(parameters);
            }

            _chartHost.Controls.Add(_queryPanel);
        }

        private async Task ExecuteQueryAsync(object parameters)
        {
            try
            {
                // 构建数据请求
                _currentRequest = BuildDataRequest(parameters);

                // 显示加载状态
                ShowLoading();

                // 执行查询
                var chartData = await _reportDefinition.QueryHandler(_currentRequest);

                // 渲染图表
                RenderChart(chartData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoading();
            }
        }

        private DataRequest BuildDataRequest(object parameters)
        {
            // 从参数对象提取 DataRequest
            var request = new DataRequest();

            // 复制公共属性
            var props = parameters.GetType().GetProperties();
            foreach (var prop in props)
            {
                var requestProp = typeof(DataRequest).GetProperty(prop.Name);
                if (requestProp != null && requestProp.PropertyType == prop.PropertyType)
                {
                    requestProp.SetValue(request, prop.GetValue(parameters));
                }
            }

            return request;
        }

        private void RenderChart(ChartData chartData)
        {
            _chartHost.Controls.Clear();
            _chartHost.Controls.Add(_queryPanel);

            // TODO: 使用 ChartBuilderFactory 创建图表
            // 这里暂时显示占位文本
            var label = new Label
            {
                Text = $"图表：{chartData.Title}\n\n数据系列数：{chartData.Series?.Count ?? 0}\n\n(此处应显示实际图表)",
                AutoSize = true,
                Location = new Point(20, _queryPanel.Height + 20)
            };
            _chartHost.Controls.Add(label);
        }

        private void ShowLoading()
        {
            Cursor = Cursors.WaitCursor;
        }

        private void HideLoading()
        {
            Cursor = Cursors.Default;
        }
    }
}
