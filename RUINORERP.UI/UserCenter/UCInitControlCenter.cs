using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using System.Drawing.Drawing2D;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Repository.UnitOfWorks;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace RUINORERP.UI.UserCenter
{
    [FormMark(typeof(UCInitControlCenter), "控制中心", true)]
    public partial class UCInitControlCenter : UserControl
    {
        public UCInitControlCenter()
        {
            InitializeComponent();
        }

        private IUnitOfWorkManage _unitOfWorkManage;
        private ILogger<MainForm> _logger;

        private void UCInitControlCenter_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            // 初始化
            _unitOfWorkManage = Startup.GetFromFac<IUnitOfWorkManage>();
            _logger = MainForm.Instance?.logger;

            // 创建右侧面板
            RightPanel rp = new RightPanel();
            rp.Dock = DockStyle.Fill;
            this.kryptonPanel1.Controls.Add(rp);

            // 加载流程图数据
            LoadFlowcharts(rp);

            rp.ItemClick += Rp_ItemClick;
        }

        private void LoadFlowcharts(RightPanel rp)
        {
            try
            {
                // 获取所有流程图定义
                var db = _unitOfWorkManage.GetDbClient();
                var flows = db.Queryable<tb_FlowchartDefinition>().ToList();
                
                if (flows == null || flows.Count == 0)
                {
                    // 如果没有数据，加载一个示例流程图
                    LoadSampleFlowchart(rp, "销售流程");
                    return;
                }

                // 加载第一个流程图
                var firstFlow = flows.FirstOrDefault();
                if (firstFlow != null)
                {
                    LoadFlowchartDetails(firstFlow, rp);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "加载流程图失败");
                // 加载示例
                LoadSampleFlowchart(rp, "销售流程");
            }
        }

        private void LoadFlowchartDetails(tb_FlowchartDefinition flow, RightPanel rp)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                
                // 加载流程图关联的Items
                var items = db.Queryable<tb_FlowchartItem>()
                    .Where(it => it.ID == flow.ID)
                    .ToList();
                flow.tb_FlowchartItems = items;

                // 加载流程图关联的Lines
                var lines = db.Queryable<tb_FlowchartLine>()
                    .Where(it => it.ID == flow.ID)
                    .ToList();
                flow.tb_FlowchartLines = lines;

                rp.LoadPage(flow);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "加载流程图详情失败");
                rp.LoadPage(flow); // 至少加载流程图基本信息
            }
        }

        private void LoadSampleFlowchart(RightPanel rp, string flowName)
        {
            // 创建示例流程图用于演示
            var flow = new tb_FlowchartDefinition
            {
                ID = 1,
                FlowchartName = flowName,
                FlowchartNo = "FLOW001"
            };

            // 创建示例节点 - 使用实际存在的图片
            var items = new List<tb_FlowchartItem>();
            
            // 报价单 - 使用销售界面图片
            items.Add(new tb_FlowchartItem
            {
                ID = 1,
                Title = "报价单",
                IconFile_Path = "销售界面/销售订货.png",
                PointToString = "50:80",
                SizeString = "64:64",
                MenuID = null
            });

            // 销售订单
            items.Add(new tb_FlowchartItem
            {
                ID = 2,
                Title = "销售订单",
                IconFile_Path = "销售界面/销售订货.png",
                PointToString = "200:80",
                SizeString = "64:64",
                MenuID = null
            });

            // 销售出库
            items.Add(new tb_FlowchartItem
            {
                ID = 3,
                Title = "销售出库",
                IconFile_Path = "销售界面/销售发货.png",
                PointToString = "350:80",
                SizeString = "64:64",
                MenuID = null
            });

            // 销售收款
            items.Add(new tb_FlowchartItem
            {
                ID = 4,
                Title = "销售收款",
                IconFile_Path = "销售界面/销售收款.png",
                PointToString = "500:80",
                SizeString = "64:64",
                MenuID = null
            });

            flow.tb_FlowchartItems = items;

            // 创建连接线
            var lines = new List<tb_FlowchartLine>();
            lines.Add(new tb_FlowchartLine { ID = 1, PointToString1 = "114:112", PointToString2 = "200:112" });
            lines.Add(new tb_FlowchartLine { ID = 2, PointToString1 = "264:112", PointToString2 = "350:112" });
            lines.Add(new tb_FlowchartLine { ID = 3, PointToString1 = "414:112", PointToString2 = "500:112" });
            flow.tb_FlowchartLines = lines;

            rp.LoadPage(flow);
        }

        private void Rp_ItemClick(object sender, EventArgs e)
        {
            // 处理流程节点点击事件，打开对应的单据窗体
            if (sender is tb_FlowchartItem item)
            {
                OpenFormByMenuID(item);
            }
        }

        private void OpenFormByMenuID(tb_FlowchartItem item)
        {
            try
            {
                if (item.MenuID.HasValue && item.MenuID.Value > 0)
                {
                    // 通过MenuID查找菜单信息
                    var db = _unitOfWorkManage.GetDbClient();
                    var menuInfo = db.Queryable<tb_MenuInfo>()
                        .Where(m => m.MenuID == item.MenuID.Value)
                        .First();
                        
                    if (menuInfo != null)
                    {
                        OpenFormByMenuInfo(menuInfo);
                    }
                    else
                    {
                        MessageBox.Show($"未找到菜单信息(MenuID: {item.MenuID})", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // 没有配置MenuID，提示用户
                    MessageBox.Show($"节点【{item.Title}】未配置对应的单据窗体，请联系管理员进行配置。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "打开窗体失败");
                MessageBox.Show($"打开窗体失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFormByMenuInfo(tb_MenuInfo menuInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(menuInfo.FormName))
                {
                    MessageBox.Show($"菜单【{menuInfo.MenuName}】未配置窗体名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 通过反射创建窗体实例
                var formType = Type.GetType(menuInfo.FormName);
                if (formType == null)
                {
                    MessageBox.Show($"找不到窗体类型：{menuInfo.FormName}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 创建窗体实例
                var form = (Form)Activator.CreateInstance(formType);
                
                // 设置菜单信息 - 如果窗体有CurMenuInfo属性
                var curMenuInfoProp = form.GetType().GetProperty("CurMenuInfo");
                if (curMenuInfoProp != null && curMenuInfoProp.CanWrite)
                {
                    curMenuInfoProp.SetValue(form, menuInfo);
                }

                // 显示窗体
                form.Show();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "打开窗体失败: {FormName}", menuInfo.FormName);
                MessageBox.Show($"打开窗体失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}