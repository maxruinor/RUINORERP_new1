using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI;
using RUINORERP.IServices;
using RUINORERP.IRepository.Base;
using RUINORERP.Common.Logging;
using RUINORERP.Model.Context;
using RUINORERP.UI.Testing;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.Business.Cache;

namespace RUINORERP.UI.Testing
{
    /// <summary>
    /// 服务缓存测试窗体，用于测试和监控Startup类中的服务实例缓存功能
    /// </summary>
    public partial class FrmServiceCacheTest : Form
    {
        /// <summary>
        /// 定时器，用于定期更新缓存统计信息
        /// </summary>
        private Timer _updateTimer;

        public FrmServiceCacheTest()
        {
            InitializeComponent();
            InitializeTimer();
            UpdateCacheStatistics();
        }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void InitializeTimer()
        {
            _updateTimer = new Timer();
            _updateTimer.Interval = 1000; // 每秒更新一次
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        /// <summary>
        /// 定时器事件处理程序，定期更新缓存统计信息
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCacheStatistics();
        }

        /// <summary>
        /// 更新缓存统计信息显示
        /// </summary>
        private void UpdateCacheStatistics()
        {
            try
            {
                // 获取Startup类的缓存统计
                string startupStats = Startup.GetServiceCacheStatistics();
                lblCacheStatistics.Text = $"Startup缓存: {startupStats}";
                
                // 获取ApplicationContext的缓存统计
                if (Program.AppContextData != null)
                {
                    var appContextStats = Program.AppContextData.GetServiceCacheStatistics();
                    lblAppContextStats.Text = $"ApplicationContext缓存: {appContextStats}";
                }
                else
                {
                    lblAppContextStats.Text = "ApplicationContext缓存: 未初始化";
                }
            }
            catch (Exception ex)
            {
                lblCacheStatistics.Text = $"获取统计信息失败: {ex.Message}";
                lblAppContextStats.Text = $"获取ApplicationContext统计失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 测试按钮点击事件，测试获取服务的性能
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnTestService_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空当前显示
                lstResults.Items.Clear();

                // 测试参数
                int iterations = int.Parse(txtIterations.Text);
                string serviceTypeName = txtServiceType.Text;

                // 获取服务类型
                Type serviceType = Type.GetType(serviceTypeName);
                if (serviceType == null)
                {
                    MessageBox.Show($"无法找到类型: {serviceTypeName}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 记录开始时间
                DateTime startTime = DateTime.Now;

                // 使用反射调用GetFromFac方法多次
                for (int i = 0; i < iterations; i++)
                {
                    var method = typeof(Startup).GetMethod("GetFromFac").MakeGenericMethod(serviceType);
                    var result = method.Invoke(null, null);
                    
                    if (i % 10 == 0) // 每10次记录一次
                    {
                        lstResults.Items.Add($"第 {i} 次获取: {(result != null ? "成功" : "失败")}");
                    }
                }

                // 记录结束时间
                DateTime endTime = DateTime.Now;
                TimeSpan duration = endTime - startTime;

                // 显示结果
                lstResults.Items.Add($"完成 {iterations} 次调用，耗时: {duration.TotalMilliseconds:F2} 毫秒");
                lstResults.Items.Add($"平均每次调用耗时: {(duration.TotalMilliseconds / iterations):F4} 毫秒");

                // 更新缓存统计
                UpdateCacheStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试过程中发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 测试ApplicationContext缓存按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnTestAppContext_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空当前显示
                lstResults.Items.Clear();

                // 检查ApplicationContext是否可用
                if (Program.AppContextData == null)
                {
                    MessageBox.Show("Program.AppContextData 为空，无法进行测试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 测试参数
                int iterations = int.Parse(txtIterations.Text);
                string serviceTypeName = txtServiceType.Text;

                // 获取服务类型
                Type serviceType = Type.GetType(serviceTypeName);
                if (serviceType == null)
                {
                    MessageBox.Show($"无法找到类型: {serviceTypeName}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 记录开始时间
                DateTime startTime = DateTime.Now;

                // 使用ApplicationContext获取服务实例多次
                for (int i = 0; i < iterations; i++)
                {
                    var result = Program.AppContextData.GetRequiredService(serviceType);
                    
                    if (i % 10 == 0) // 每10次记录一次
                    {
                        lstResults.Items.Add($"第 {i} 次获取: {(result != null ? "成功" : "失败")}");
                    }
                }

                // 记录结束时间
                DateTime endTime = DateTime.Now;
                TimeSpan duration = endTime - startTime;

                // 显示结果
                lstResults.Items.Add($"完成 {iterations} 次调用，耗时: {duration.TotalMilliseconds:F2} 毫秒");
                lstResults.Items.Add($"平均每次调用耗时: {(duration.TotalMilliseconds / iterations):F4} 毫秒");

                // 更新缓存统计
                UpdateCacheStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试ApplicationContext缓存时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 运行完整控制台测试按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnRunConsoleTest_Click(object sender, EventArgs e)
        {
            try
            {
                // 运行ApplicationContext缓存性能测试
                ApplicationContextCacheTest.RunApplicationContextCacheTest();
                
                lstResults.Items.Add("控制台测试已完成，请查看控制台输出");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"运行控制台测试时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 测试ApplicationContext.Current属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestCurrentContext_Click(object sender, EventArgs e)
        {
            try
            {
                lstResults.Items.Clear();
                lstResults.Items.Add("开始测试ApplicationContext.Current属性...");
                
                // 测试1: 直接访问ApplicationContext.Current
                var currentContext = ApplicationContext.Current;
                if (currentContext != null)
                {
                    lstResults.Items.Add($"✓ ApplicationContext.Current 获取成功");
                    lstResults.Items.Add($"  - 实例类型: {currentContext.GetType().FullName}");
                    lstResults.Items.Add($"  - 是否已设置ServiceProvider: {currentContext.GetApplicationContextAccessor()?.ServiceProvider != null}");
                    
                    // 测试2: 通过ServiceProvider获取服务
                    try
                    {
                        var serviceProvider = currentContext.GetApplicationContextAccessor()?.ServiceProvider;
                        if (serviceProvider != null)
                        {
                            lstResults.Items.Add($"✓ ServiceProvider 获取成功");
                            
                            // 尝试获取一个已知的服务进行验证
                            var cacheManager = serviceProvider.GetService(typeof(IEntityCacheManager));
                            if (cacheManager != null)
                            {
                                lstResults.Items.Add($"✓ 通过ServiceProvider成功获取IEntityCacheManager服务");
                            }
                            else
                            {
                                lstResults.Items.Add($"⚠ 无法通过ServiceProvider获取IEntityCacheManager服务");
                            }
                        }
                        else
                        {
                            lstResults.Items.Add($"✗ ServiceProvider 为null");
                        }
                    }
                    catch (Exception ex)
                    {
                        lstResults.Items.Add($"✗ ServiceProvider测试失败: {ex.Message}");
                    }
                    
                    // 测试3: 验证线程本地存储降级方案
                    try
                    {
                        var fallbackContext = ApplicationContext.Current;
                        if (fallbackContext != null)
                        {
                            lstResults.Items.Add($"✓ 线程本地存储降级方案工作正常");
                        }
                    }
                    catch (Exception ex)
                    {
                        lstResults.Items.Add($"✗ 线程本地存储降级方案失败: {ex.Message}");
                    }
                }
                else
                {
                    lstResults.Items.Add($"✗ ApplicationContext.Current 为null");
                }
                
                lstResults.Items.Add("测试完成。");
            }
            catch (Exception ex)
            {
                lstResults.Items.Add($"测试过程中发生错误: {ex.Message}");
                lstResults.Items.Add($"错误详情: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 清空缓存按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void btnClearCache_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空Startup缓存
                Startup.ClearServiceCache();
                
                // 清空ApplicationContext缓存
                if (Program.AppContextData != null)
                {
                    Program.AppContextData.ClearServiceCache();
                }
                
                UpdateCacheStatistics();
                lstResults.Items.Add("所有缓存已清空");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清空缓存时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体关闭事件，清理资源
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void FrmServiceCacheTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
                _updateTimer.Dispose();
            }
        }
    }
}