using LiveChartsCore.Measure;
using Org.BouncyCastle.Asn1.Crmf;
using RUINORERP.Model.ReminderModel.ReminderResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Drawing;

namespace RUINORERP.UI.SmartReminderClient
{
    public class PopupNotification
    {
        public void ShowNotification(IReminderResult result)
        {
            // 创建简约通知
            var notification = new NotificationForm(result);
            notification.Show();
        }
    }

    public class NotificationForm : Form
    {
        private readonly IReminderResult _result;

        public NotificationForm(IReminderResult result)
        {
            _result = result;
            InitializeUI();
        }

        private void InitializeUI()
        {
            // 窗体设置
            Size = new Size(400, 150);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width - 10,
                                Screen.PrimaryScreen.WorkingArea.Height - Height - 10);

            // 标题
            var lblTitle = new Label
            {
                Text = _result.Title,
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 摘要
            var lblSummary = new Label
            {
                Text = _result.Summary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 10, 0)
            };

            // 操作按钮
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.RightToLeft
            };

            var btnClose = new Button { Text = "关闭", Size = new Size(80, 30) };
            btnClose.Click += (s, e) => Close();

            var btnDetail = new Button { Text = "查看详情", Size = new Size(80, 30) };
            btnDetail.Click += (s, e) => ShowDetail();

            btnPanel.Controls.AddRange(new Control[] { btnClose, btnDetail });

            Controls.Add(lblTitle);
            Controls.Add(lblSummary);
            Controls.Add(btnPanel);
        }

        private void ShowDetail()
        {
            // 打开详情视图
            var detailForm = new DetailForm(_result);
            detailForm.Show();
            Close();
        }
    }
}
