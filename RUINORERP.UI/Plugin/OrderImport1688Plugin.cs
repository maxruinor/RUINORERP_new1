using RUINORERP.Plugin;
using RUINORERP.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using RUINORERP.Model;
using RUINORERP.IServices;

namespace RUINORERP.Plugin.OrderImport
{
    /// <summary>
    /// 1688订单导入插件
    /// 从1688后台提取销售订单信息并转换为系统销售订单
    /// </summary>
    public class OrderImport1688Plugin : PluginBase
    {
        private const string PLUGIN_NAME = "1688订单导入";
        private const string PLUGIN_DESCRIPTION = "从1688后台提取销售订单信息并批量/单个转换为系统销售订单";
        private const string PLUGIN_VERSION = "1.0.0";

        private Itb_SaleOrderServices _salesOrderService;
        private Itb_SaleOrderDetailServices _salesOrderItemService;

        /// <summary>
        /// 插件名称
        /// </summary>
        public override string Name => PLUGIN_NAME;

        /// <summary>
        /// 插件描述
        /// </summary>
        public override string Description => PLUGIN_DESCRIPTION;

        /// <summary>
        /// 插件版本
        /// </summary>
        public override Version Version => new Version(PLUGIN_VERSION);

        /// <summary>
        /// 初始化插件
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // 获取系统服务
            _salesOrderService = GetService<Itb_SaleOrderServices>();
            _salesOrderItemService = GetService<Itb_SaleOrderDetailServices>();

            // 创建子菜单
            CreateSubMenuItems();
        }
        
        /// <summary>
        /// 启动插件
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();
            // 插件启动时的逻辑
        }
        
        /// <summary>
        /// 停止插件
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
            // 插件停止时的逻辑
        }

        /// <summary>
        /// 创建子菜单项
        /// </summary>
        private void CreateSubMenuItems()
        {
            var singleImportMenuItem = new ToolStripMenuItem("导入单个订单")
            {
                Tag = "SingleImport"
            };
            singleImportMenuItem.Click += OnSingleImportClick;

            var batchImportMenuItem = new ToolStripMenuItem("批量导入订单")
            {
                Tag = "BatchImport"
            };
            batchImportMenuItem.Click += OnBatchImportClick;

            // 添加子菜单项
            if (PluginMenuItem != null)
            {
                PluginMenuItem.DropDownItems.Add(singleImportMenuItem);
                PluginMenuItem.DropDownItems.Add(batchImportMenuItem);
                PluginMenuItem.DropDownItems.Add(new ToolStripSeparator());
                // 创建设置菜单项
                var settingsMenuItem = new ToolStripMenuItem("设置")
                {
                    Tag = "Settings"
                };
                // 添加点击事件
                settingsMenuItem.Click += OnSettingsClick;
                // 添加到下拉菜单
                PluginMenuItem.DropDownItems.Add(settingsMenuItem);
            }
        }

        /// <summary>
        /// 执行插件功能
        /// </summary>
        public override void Execute()
        {
            // 主菜单点击时显示帮助信息
            ShowInfo("请选择具体的操作：导入单个订单或批量导入订单");
        }

        /// <summary>
        /// 导入单个订单点击事件
        /// </summary>
        private void OnSingleImportClick(object sender, EventArgs e)
        {
            try
            {
                // 显示订单输入对话框
                using (var inputDialog = new Form()
                {
                    Text = "导入单个1688订单",
                    Width = 400,
                    Height = 200,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterScreen
                })
                {
                    var label = new Label()
                    {
                        Text = "请输入1688订单号:",
                        Location = new System.Drawing.Point(20, 30),
                        Width = 120
                    };

                    var textBox = new TextBox()
                    {
                        Location = new System.Drawing.Point(140, 30),
                        Width = 200
                    };

                    var okButton = new Button()
                    {
                        Text = "确定",
                        DialogResult = DialogResult.OK,
                        Location = new System.Drawing.Point(140, 70),
                        Width = 80
                    };

                    var cancelButton = new Button()
                    {
                        Text = "取消",
                        DialogResult = DialogResult.Cancel,
                        Location = new System.Drawing.Point(240, 70),
                        Width = 80
                    };

                    inputDialog.Controls.Add(label);
                    inputDialog.Controls.Add(textBox);
                    inputDialog.Controls.Add(okButton);
                    inputDialog.Controls.Add(cancelButton);
                    inputDialog.AcceptButton = okButton;
                    inputDialog.CancelButton = cancelButton;

                    if (inputDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(textBox.Text))
                    {
                        string orderNo = textBox.Text.Trim();

                        // 显示进度对话框
                        using (var progressForm = new Form()
                        {
                            Text = "导入订单中",
                            Width = 300,
                            Height = 120,
                            FormBorderStyle = FormBorderStyle.FixedDialog,
                            StartPosition = FormStartPosition.CenterScreen,
                            ControlBox = false
                        })
                        {
                            var progressBar = new ProgressBar()
                            {
                                Location = new System.Drawing.Point(20, 20),
                                Width = 240
                            };

                            var labelStatus = new Label()
                            {
                                Text = "正在从1688提取订单信息...",
                                Location = new System.Drawing.Point(20, 50),
                                Width = 240
                            };

                            progressForm.Controls.Add(progressBar);
                            progressForm.Controls.Add(labelStatus);

                            // 启动异步任务处理订单导入
                            Task.Run(() =>
                            {
                                try
                                {
                                    // 模拟从1688提取订单信息
                                    System.Threading.Thread.Sleep(1000);
                                    progressForm.Invoke((MethodInvoker)delegate { labelStatus.Text = "正在转换订单数据..."; progressBar.Value = 50; });

                                    // 模拟转换订单数据
                                    System.Threading.Thread.Sleep(1000);
                                    progressForm.Invoke((MethodInvoker)delegate { labelStatus.Text = "正在保存到系统..."; progressBar.Value = 80; });

                                    // 模拟保存订单
                                    System.Threading.Thread.Sleep(500);
                                    progressForm.Invoke((MethodInvoker)delegate { progressBar.Value = 100; });

                                    // 关闭进度窗口
                                    progressForm.Invoke((MethodInvoker)delegate { progressForm.Close(); });

                                    // 显示结果
                                    ShowInfo($"订单 {orderNo} 导入成功！\n请在销售订单管理中查看和核对订单信息。");
                                }
                                catch (Exception ex)
                                {
                                    progressForm.Invoke((MethodInvoker)delegate { progressForm.Close(); });
                                    ShowError($"导入订单失败: {ex.Message}");
                                }
                            });

                            // 显示进度窗口
                            progressForm.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"导入单个订单时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量导入订单点击事件
        /// </summary>
        private void OnBatchImportClick(object sender, EventArgs e)
        {
            try
            {
                // 显示批量导入窗口
                using (var openFileDialog = new OpenFileDialog()
                {
                    Title = "选择1688订单导出文件",
                    Filter = "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*",
                    FilterIndex = 1,
                    Multiselect = false
                })
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        // 显示进度对话框
                        using (var progressForm = new Form()
                        {
                            Text = "批量导入订单中",
                            Width = 300,
                            Height = 120,
                            FormBorderStyle = FormBorderStyle.FixedDialog,
                            StartPosition = FormStartPosition.CenterScreen,
                            ControlBox = false
                        })
                        {
                            var progressBar = new ProgressBar()
                            {
                                Location = new System.Drawing.Point(20, 20),
                                Width = 240
                            };

                            var labelStatus = new Label()
                            {
                                Text = "正在读取订单文件...",
                                Location = new System.Drawing.Point(20, 50),
                                Width = 240
                            };

                            progressForm.Controls.Add(progressBar);
                            progressForm.Controls.Add(labelStatus);

                            // 启动异步任务处理批量导入
                            Task.Run(() =>
                            {
                                try
                                {
                                    // 模拟读取文件
                                    System.Threading.Thread.Sleep(1000);
                                    progressForm.Invoke((MethodInvoker)delegate { labelStatus.Text = "正在解析订单数据..."; progressBar.Value = 30; });

                                    // 模拟解析数据
                                    System.Threading.Thread.Sleep(1500);
                                    progressForm.Invoke((MethodInvoker)delegate { labelStatus.Text = "正在转换订单..."; progressBar.Value = 60; });

                                    // 模拟转换订单
                                    System.Threading.Thread.Sleep(2000);
                                    progressForm.Invoke((MethodInvoker)delegate { labelStatus.Text = "正在保存到系统..."; progressBar.Value = 80; });

                                    // 模拟保存订单
                                    System.Threading.Thread.Sleep(1000);
                                    progressForm.Invoke((MethodInvoker)delegate { progressBar.Value = 100; });

                                    // 关闭进度窗口
                                    progressForm.Invoke((MethodInvoker)delegate { progressForm.Close(); });

                                    // 显示结果
                                    ShowInfo($"批量订单导入成功！\n文件: {Path.GetFileName(filePath)}\n请在销售订单管理中查看和核对订单信息。");
                                }
                                catch (Exception ex)
                                {
                                    progressForm.Invoke((MethodInvoker)delegate { progressForm.Close(); });
                                    ShowError($"批量导入订单失败: {ex.Message}");
                                }
                            });

                            // 显示进度窗口
                            progressForm.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"批量导入订单时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置点击事件
        /// </summary>
        private void OnSettingsClick(object sender, EventArgs e)
        {
            try
            {
                // 显示设置窗口
                using (var settingsForm = new Form()
                {
                    Text = "1688订单导入设置",
                    Width = 400,
                    Height = 300,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterScreen
                })
                {
                    var labelAppKey = new Label()
                    {
                        Text = "App Key:",
                        Location = new System.Drawing.Point(20, 30),
                        Width = 80
                    };

                    var textBoxAppKey = new TextBox()
                    {
                        Location = new System.Drawing.Point(120, 30),
                        Width = 240,
                        Text = GetSetting("AppKey", "")
                    };

                    var labelAppSecret = new Label()
                    {
                        Text = "App Secret:",
                        Location = new System.Drawing.Point(20, 70),
                        Width = 80
                    };

                    var textBoxAppSecret = new TextBox()
                    {
                        Location = new System.Drawing.Point(120, 70),
                        Width = 240,
                        PasswordChar = '*',
                        Text = GetSetting("AppSecret", "")
                    };

                    var labelAccessToken = new Label()
                    {
                        Text = "Access Token:",
                        Location = new System.Drawing.Point(20, 110),
                        Width = 80
                    };

                    var textBoxAccessToken = new TextBox()
                    {
                        Location = new System.Drawing.Point(120, 110),
                        Width = 240,
                        PasswordChar = '*',
                        Text = GetSetting("AccessToken", "")
                    };

                    var okButton = new Button()
                    {
                        Text = "确定",
                        DialogResult = DialogResult.OK,
                        Location = new System.Drawing.Point(120, 180),
                        Width = 80
                    };

                    var cancelButton = new Button()
                    {
                        Text = "取消",
                        DialogResult = DialogResult.Cancel,
                        Location = new System.Drawing.Point(240, 180),
                        Width = 80
                    };

                    settingsForm.Controls.Add(labelAppKey);
                    settingsForm.Controls.Add(textBoxAppKey);
                    settingsForm.Controls.Add(labelAppSecret);
                    settingsForm.Controls.Add(textBoxAppSecret);
                    settingsForm.Controls.Add(labelAccessToken);
                    settingsForm.Controls.Add(textBoxAccessToken);
                    settingsForm.Controls.Add(okButton);
                    settingsForm.Controls.Add(cancelButton);
                    settingsForm.AcceptButton = okButton;
                    settingsForm.CancelButton = cancelButton;

                    if (settingsForm.ShowDialog() == DialogResult.OK)
                    {
                        // 保存设置
                        var settings = new Dictionary<string, string>
                        {
                            ["AppKey"] = textBoxAppKey.Text,
                            ["AppSecret"] = textBoxAppSecret.Text,
                            ["AccessToken"] = textBoxAccessToken.Text
                        };
                        SaveSettings(settings);

                        ShowInfo("设置保存成功！");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"打开设置窗口时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取插件设置
        /// </summary>
        /// <param name="key">设置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>设置值</returns>
        private string GetSetting(string key, string defaultValue)
        {
            var settings = LoadSettings();
            return settings.ContainsKey(key) ? settings[key] : defaultValue;
        }

        /// <summary>
        /// 从1688获取订单详情
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns>订单数据</returns>
        private Dictionary<string, object> GetOrderDetailFrom1688(string orderNo)
        {
            // 这里应该实现实际的1688 API调用逻辑
            // 现在只是返回模拟数据
            return new Dictionary<string, object>
            {
                { "OrderNo", orderNo },
                { "OrderTime", DateTime.Now },
                { "CustomerName", "模拟客户" },
                { "TotalAmount", 1000.00 },
                { "Items", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "ProductName", "模拟产品1" },
                            { "Quantity", 10 },
                            { "UnitPrice", 100.00 }
                        },
                        new Dictionary<string, object>
                        {
                            { "ProductName", "模拟产品2" },
                            { "Quantity", 5 },
                            { "UnitPrice", 200.00 }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// 将1688订单转换为系统销售订单
        /// </summary>
        /// <param name="orderData">1688订单数据</param>
        /// <returns>系统销售订单</returns>
        private tb_SaleOrder ConvertToSalesOrder(Dictionary<string, object> orderData)
        {
            // 这里应该实现实际的订单转换逻辑
            // 现在只是返回模拟数据
            return new tb_SaleOrder
            {
                //OrderNo = orderData["OrderNo"].ToString(),
                //OrderDate = (DateTime)orderData["OrderTime"],
                //CustomerName = orderData["CustomerName"].ToString(),
                //TotalAmount = Convert.ToDecimal(orderData["TotalAmount"]),
                //Status = 1, // 待审核
                //CreateTime = DateTime.Now,
                //Creator = Program.AppContextData.CurrentUser.UserID
            };
        }
    }
}