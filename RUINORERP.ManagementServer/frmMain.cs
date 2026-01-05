using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.TopServer.AuthorizationManagement;
using RUINORERP.TopServer.ConfigurationManagement;
using RUINORERP.TopServer.Network;
using RUINORERP.TopServer.ServerManagement;
using RUINORERP.TopServer.UserStatusMonitoring;

namespace RUINORERP.TopServer
{
    /// <summary>
    /// 管理服务器主窗体
    /// 提供服务器实例监控、授权管理、用户状态监控等功能的UI界面
    /// </summary>
    public partial class frmMain : Form
    {
        private AppManager _appManager;
        private BindingList<ServerInstanceInfo> _serverInstancesBindingList;
        private BindingList<AuthorizationInfo> _authorizationBindingList;
        private BindingList<UserInfo> _usersBindingList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            InitializeUI();
            InitializeBindingLists();
            InitializeDataGridViews();
            InitializeAppManager();
            RegisterEventHandlers();
        }

        /// <summary>
        /// 初始化UI组件
        /// </summary>
        private void InitializeUI()
        {
            this.Text = "RUINORERP 管理服务器";
            this.WindowState = FormWindowState.Maximized;
            
            // 初始化状态栏
            tsStatus.Text = "服务器未启动";
        }

        private BindingList<ConfigurationInfo> _configurationsBindingList;

        /// <summary>
        /// 初始化绑定列表
        /// </summary>
        private void InitializeBindingLists()
        {
            _serverInstancesBindingList = new BindingList<ServerInstanceInfo>();
            _authorizationBindingList = new BindingList<AuthorizationInfo>();
            _usersBindingList = new BindingList<UserInfo>();
            _configurationsBindingList = new BindingList<ConfigurationInfo>();
        }

        /// <summary>
        /// 初始化数据网格视图
        /// </summary>
        private void InitializeDataGridViews()
        {
            // 初始化服务器实例数据网格
            InitializeServerInstancesDataGridView();
            
            // 初始化授权信息数据网格
            InitializeAuthorizationDataGridView();
            
            // 初始化用户信息数据网格
            InitializeUsersDataGridView();
            
            // 初始化配置信息数据网格
            InitializeConfigurationsDataGridView();
        }

        /// <summary>
        /// 初始化服务器实例数据网格
        /// </summary>
        private void InitializeServerInstancesDataGridView()
        {
            dgvServerInstances.AutoGenerateColumns = false;
            dgvServerInstances.DataSource = _serverInstancesBindingList;
            
            // 添加列
            dgvServerInstances.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { HeaderText = "实例ID", DataPropertyName = "InstanceId", Width = 150, Visible = false },
                new DataGridViewTextBoxColumn { HeaderText = "实例名称", DataPropertyName = "InstanceName", Width = 150 },
                new DataGridViewTextBoxColumn { HeaderText = "IP地址", DataPropertyName = "IpAddress", Width = 120 },
                new DataGridViewTextBoxColumn { HeaderText = "端口", DataPropertyName = "Port", Width = 80 },
                new DataGridViewTextBoxColumn { HeaderText = "版本", DataPropertyName = "Version", Width = 100 },
                new DataGridViewTextBoxColumn { HeaderText = "注册时间", DataPropertyName = "RegisterTime", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" } },
                new DataGridViewTextBoxColumn { HeaderText = "最后心跳", DataPropertyName = "LastHeartbeatTime", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" } },
                new DataGridViewTextBoxColumn { HeaderText = "状态", DataPropertyName = "Status", Width = 100, CellTemplate = new DataGridViewComboBoxCell { DataSource = Enum.GetValues(typeof(ServerInstanceStatus)) } }
            });
        }

        /// <summary>
        /// 初始化授权信息数据网格
        /// </summary>
        private void InitializeAuthorizationDataGridView()
        {
            dgvAuthorization.AutoGenerateColumns = false;
            dgvAuthorization.DataSource = _authorizationBindingList;
            
            // 添加列
            dgvAuthorization.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { HeaderText = "实例ID", DataPropertyName = "InstanceId", Width = 150 },
                new DataGridViewTextBoxColumn { HeaderText = "授权类型", DataPropertyName = "AuthorizationType", Width = 120, CellTemplate = new DataGridViewComboBoxCell { DataSource = Enum.GetValues(typeof(AuthorizationType)) } },
                new DataGridViewTextBoxColumn { HeaderText = "开始时间", DataPropertyName = "StartTime", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } },
                new DataGridViewTextBoxColumn { HeaderText = "到期时间", DataPropertyName = "ExpireTime", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } },
                new DataGridViewTextBoxColumn { HeaderText = "最大用户数", DataPropertyName = "MaxUsers", Width = 100 },
                new DataGridViewTextBoxColumn { HeaderText = "最大事务数", DataPropertyName = "MaxTransactions", Width = 100 },
                new DataGridViewTextBoxColumn { HeaderText = "许可证密钥", DataPropertyName = "LicenseKey", Width = 200 },
                new DataGridViewTextBoxColumn { HeaderText = "状态", DataPropertyName = "Status", Width = 100, CellTemplate = new DataGridViewComboBoxCell { DataSource = Enum.GetValues(typeof(AuthorizationStatus)) } }
            });
        }

        /// <summary>
        /// 初始化用户信息数据网格
        /// </summary>
        private void InitializeUsersDataGridView()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = _usersBindingList;
            
            // 添加列
            dgvUsers.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { HeaderText = "用户ID", DataPropertyName = "UserId", Width = 120 },
                new DataGridViewTextBoxColumn { HeaderText = "用户名", DataPropertyName = "UserName", Width = 150 },
                new DataGridViewTextBoxColumn { HeaderText = "状态", DataPropertyName = "Status", Width = 100, CellTemplate = new DataGridViewComboBoxCell { DataSource = Enum.GetValues(typeof(UserStatus)) } },
                new DataGridViewTextBoxColumn { HeaderText = "登录时间", DataPropertyName = "LoginTime", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" } },
                new DataGridViewTextBoxColumn { HeaderText = "最后活动", DataPropertyName = "LastActivityTime", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" } },
                new DataGridViewTextBoxColumn { HeaderText = "IP地址", DataPropertyName = "IpAddress", Width = 120 }
            });
        }

        /// <summary>
        /// 初始化应用管理器
        /// </summary>
        private void InitializeAppManager()
        {
            _appManager = AppManager.Instance;
            _appManager.Initialize();
            
            // 更新状态栏
            tsStatus.Text = "服务器已启动";
        }

        /// <summary>
        /// 初始化配置信息数据网格
        /// </summary>
        private void InitializeConfigurationsDataGridView()
        {
            dgvConfigurations.AutoGenerateColumns = false;
            dgvConfigurations.DataSource = _configurationsBindingList;
            
            // 添加列
            dgvConfigurations.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { HeaderText = "配置名称", DataPropertyName = "Name", Width = 150 },
                new DataGridViewTextBoxColumn { HeaderText = "配置值", DataPropertyName = "Value", Width = 200 },
                new DataGridViewTextBoxColumn { HeaderText = "描述", DataPropertyName = "Description", Width = 250 },
                new DataGridViewTextBoxColumn { HeaderText = "最后更新时间", DataPropertyName = "LastUpdateTime", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" } }
            });
        }

        /// <summary>
        /// 注册事件处理器
        /// </summary>
        private void RegisterEventHandlers()
        {
            // 注册服务器管理器事件
            _appManager.ServerManager.ServerInstanceRegistered += OnServerInstanceRegistered;
            _appManager.ServerManager.ServerInstanceUnregistered += OnServerInstanceUnregistered;
            _appManager.ServerManager.ServerInstanceStatusChanged += OnServerInstanceStatusChanged;
            
            // 注册授权管理器事件
            _appManager.AuthorizationManager.AuthorizationStatusChanged += OnAuthorizationStatusChanged;
            
            // 注册用户管理器事件
            _appManager.UserManager.UserLoggedIn += OnUserLoggedIn;
            _appManager.UserManager.UserLoggedOut += OnUserLoggedOut;
            _appManager.UserManager.UserActivity += OnUserActivity;
            
            // 注册配置管理器事件
            _appManager.ConfigurationManager.ConfigurationUpdated += OnConfigurationUpdated;
        }
        
        /// <summary>
        /// 配置更新事件处理
        /// </summary>
        private void OnConfigurationUpdated(object sender, ConfigurationUpdatedEventArgs e)
        {
            UpdateConfigurationsList();
        }
        
        /// <summary>
        /// 更新配置列表
        /// </summary>
        private void UpdateConfigurationsList()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateConfigurationsList));
                return;
            }
            
            // 清空现有列表
            _configurationsBindingList.Clear();
            
            // 添加所有配置
            foreach (var config in _appManager.ConfigurationManager.GetAllConfigurations())
            {
                _configurationsBindingList.Add(config);
            }
        }

        /// <summary>
        /// 更新服务器实例列表
        /// </summary>
        private void UpdateServerInstancesList()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateServerInstancesList));
                return;
            }
            
            // 清空现有列表
            _serverInstancesBindingList.Clear();
            
            // 添加所有服务器实例
            foreach (var instance in _appManager.ServerManager.AllServerInstances)
            {
                _serverInstancesBindingList.Add(instance);
            }
            
            // 更新状态栏计数
            tsOnlineCount.Text = _appManager.ServerManager.OnlineServerInstances.Count().ToString();
            tsTotalCount.Text = _appManager.ServerManager.AllServerInstances.Count().ToString();
        }

        /// <summary>
        /// 更新授权信息列表
        /// </summary>
        private void UpdateAuthorizationList()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateAuthorizationList));
                return;
            }
            
            // 清空现有列表
            _authorizationBindingList.Clear();
            
            // 添加所有授权信息
            foreach (var instance in _appManager.ServerManager.AllServerInstances)
            {
                var authInfo = _appManager.AuthorizationManager.GetAuthorizationInfo(instance.InstanceId);
                if (authInfo != null)
                {
                    _authorizationBindingList.Add(authInfo);
                }
            }
        }

        /// <summary>
        /// 更新用户列表
        /// </summary>
        private void UpdateUsersList()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateUsersList));
                return;
            }
            
            // 清空现有列表
            _usersBindingList.Clear();
            
            // 添加所有用户
            foreach (var instance in _appManager.ServerManager.AllServerInstances)
            {
                var users = _appManager.UserManager.GetUsersByInstanceId(instance.InstanceId);
                foreach (var user in users)
                {
                    _usersBindingList.Add(user);
                }
            }
        }

        /// <summary>
        /// 服务器实例注册事件处理
        /// </summary>
        private void OnServerInstanceRegistered(object sender, ServerInstanceRegisteredEventArgs e)
        {
            UpdateServerInstancesList();
            UpdateAuthorizationList();
        }

        /// <summary>
        /// 服务器实例注销事件处理
        /// </summary>
        private void OnServerInstanceUnregistered(object sender, ServerInstanceUnregisteredEventArgs e)
        {
            UpdateServerInstancesList();
            UpdateAuthorizationList();
            UpdateUsersList();
        }

        /// <summary>
        /// 服务器实例状态变化事件处理
        /// </summary>
        private void OnServerInstanceStatusChanged(object sender, ServerInstanceStatusChangedEventArgs e)
        {
            UpdateServerInstancesList();
        }

        /// <summary>
        /// 授权状态变化事件处理
        /// </summary>
        private void OnAuthorizationStatusChanged(object sender, AuthorizationStatusChangedEventArgs e)
        {
            UpdateAuthorizationList();
        }

        /// <summary>
        /// 用户登录事件处理
        /// </summary>
        private void OnUserLoggedIn(object sender, UserLoggedInEventArgs e)
        {
            UpdateUsersList();
        }

        /// <summary>
        /// 用户登出事件处理
        /// </summary>
        private void OnUserLoggedOut(object sender, UserLoggedOutEventArgs e)
        {
            UpdateUsersList();
        }

        /// <summary>
        /// 用户活动事件处理
        /// </summary>
        private void OnUserActivity(object sender, UserActivityEventArgs e)
        {
            // 可以根据需要更新用户列表或添加活动日志
            UpdateUsersList();
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        private void ExitSystem()
        {
            if (_appManager != null)
            {
                _appManager.Shutdown();
            }
            Application.Exit();
        }

        /// <summary>
        /// 启动服务器菜单项点击事件
        /// </summary>
        private void 启动服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_appManager != null && !_appManager.NetworkServer.IsRunning)
                {
                    _appManager.NetworkServer.StartAsync().Wait();
                    tsStatus.Text = "服务器已启动";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动服务器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 停止服务器菜单项点击事件
        /// </summary>
        private void 停止服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_appManager != null && _appManager.NetworkServer.IsRunning)
                {
                    _appManager.NetworkServer.StopAsync().Wait();
                    tsStatus.Text = "服务器已停止";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止服务器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 退出菜单项点击事件
        /// </summary>
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitSystem();
        }

        /// <summary>
        /// 服务器实例菜单项点击事件
        /// </summary>
        private void 服务器实例ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpServerInstances;
        }

        /// <summary>
        /// 授权管理菜单项点击事件
        /// </summary>
        private void 授权管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpAuthorization;
        }

        /// <summary>
        /// 用户状态菜单项点击事件
        /// </summary>
        private void 用户状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpUsers;
        }

        /// <summary>
        /// 关于菜单项点击事件
        /// </summary>
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("RUINORERP 管理服务器 v1.0\n\n用于集中管控客户部署的各服务器实例\n\n© 2025 RUINORERP", "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// 配置管理菜单项点击事件
        /// </summary>
        private void 配置管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpConfigurations;
            // 更新配置列表
            UpdateConfigurationsList();
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitSystem();
        }
    }
}