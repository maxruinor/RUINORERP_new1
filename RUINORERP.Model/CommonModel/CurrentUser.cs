using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RUINORERP.Model
{
    /// <summary>
    /// 用户操作界面信息
    /// 用于服务器端用户管理界面显示
    /// 通过心跳机制传输
    /// </summary>
    public class UserOperationInfo
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string CurrentModule { get; set; }
        public string CurrentForm { get; set; }
        public DateTime LoginTime { get; set; }
        public int HeartbeatCount { get; set; }
        public string ClientVersion { get; set; }
        public string ClientIp { get; set; }
        public long IdleTime { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsAuthorized { get; set; }
        public string OperatingSystem { get; set; }
        public string MachineName { get; set; }
        public string CpuInfo { get; set; }
        public string MemorySize { get; set; }
    }

    /// <summary>
    /// 当前用户信息 - 会话层
    /// 
    /// 【设计原则】
    /// 1. 最小化数据 - 只存储会话管理必需的少量信息
    /// 2. 单一数据源 - 每个数据只在一处定义
    /// 3. 命名统一 - 使用标准CamelCase命名
    /// </summary>
    public class CurrentUserInfo : INotifyPropertyChanged
    {
        #region 核心标识

        /// <summary>
        /// 用户ID - 用户表(tb_UserInfo)主键
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户名 - 登录时使用的名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 员工ID - 员工表(tb_Employee)外键
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// 员工姓名 - 用于显示
        /// </summary>
        public string DisplayName { get; set; }

        #endregion

        #region 认证状态

        public bool IsSuperUser { get; set; }
        public bool IsAuthorized { get; set; }

        #endregion

        #region 用户组

        /// <summary>
        /// 用户组名称
        /// </summary>
        public string UserGroup { get; set; }

        #endregion

        #region 会话信息

        public DateTime LoginTime { get; set; }
        public DateTime LastHeartbeatTime { get; set; }
        public int HeartbeatCount { get; set; }
        public long IdleTime { get; set; }
        public bool IsOnline { get; set; }

        #endregion

        #region 当前操作

        public string CurrentModule { get; set; }
        public string CurrentForm { get; set; }

        #endregion

        #region 客户端环境

        public string ClientVersion { get; set; }
        public string ClientIp { get; set; }
        public string OperatingSystem { get; set; }
        public string MachineName { get; set; }
        public string CpuInfo { get; set; }
        public string MemorySize { get; set; }

        #endregion

        #region 数据库完整对象

        public tb_UserInfo UserInfo { get; set; }
        public tb_Employee Employee { get; set; }
        public List<tb_ModuleDefinition> ModuleList { get; set; } = new List<tb_ModuleDefinition>();

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 从用户信息初始化
        /// </summary>
        public void InitializeFrom(tb_UserInfo user)
        {
            if (user == null) return;

            UserID = user.User_ID;
            UserName = user.UserName;
            IsSuperUser = user.IsSuperUser;
            IsAuthorized = true;
            LoginTime = DateTime.Now;
            IsOnline = true;

            if (user.tb_employee != null)
            {
                EmployeeId = user.tb_employee.Employee_ID;
                DisplayName = user.tb_employee.Employee_Name;
                Employee = user.tb_employee;
            }

            UserInfo = user;
        }

        /// <summary>
        /// 转换为操作信息
        /// </summary>
        public UserOperationInfo ToOperationInfo()
        {
            return new UserOperationInfo
            {
                UserName = UserName,
                DisplayName = DisplayName,
                CurrentModule = CurrentModule,
                CurrentForm = CurrentForm,
                LoginTime = LoginTime,
                HeartbeatCount = HeartbeatCount,
                ClientVersion = ClientVersion,
                ClientIp = ClientIp,
                IdleTime = IdleTime,
                IsSuperUser = IsSuperUser,
                IsAuthorized = IsAuthorized,
                OperatingSystem = OperatingSystem,
                MachineName = MachineName,
                CpuInfo = CpuInfo,
                MemorySize = MemorySize
            };
        }

        #endregion
    }
}
