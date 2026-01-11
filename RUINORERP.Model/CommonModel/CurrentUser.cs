using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{

    /// <summary>
    /// 内存中的业务级的用户信息
    /// </summary>
    public class CurrentUserInfo : INotifyPropertyChanged
    {

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public long EmpID { get; set; }



        #region 数据库级别

        /// <summary>
        /// 设置了默认值
        /// </summary>
        public List<tb_ModuleDefinition> UserModList { get; set; } = new List<tb_ModuleDefinition>();

        //public List<tb_ButtonInfo> UserButtonList { get; set; } = new List<tb_ButtonInfo>();
        //public List<tb_FieldInfo> UserFieldList { get; set; } = new List<tb_FieldInfo>();
        //public List<tb_MenuInfo> UserMenuList { get; set; } = new List<tb_MenuInfo>();

        public tb_UserInfo UserInfo { get; set; } = new tb_UserInfo();

        public tb_Employee tb_Employee { get; set; } = new tb_Employee();

        #endregion

        private string _sessionid;
        private string _用户名;
        private string _姓名;
        private string _当前模块;
        private string _当前窗体;
        private DateTime _登录时间;
        private int _心跳数;
        private string _最后心跳时间;
        private string _客户端版本;
        private string _客户端IP;
        private long _静止时间;
        private long _Employee_ID;
        private string _操作系统;
        private string _机器名;
        private string _CPU信息;
        private string _内存大小;

        #region  属性
        public long Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                if (_Employee_ID != value)
                {
                    _Employee_ID = value;
                    OnPropertyChanged(nameof(Employee_ID));
                }
            }
        }


        public string SessionId
        {
            get { return _sessionid; }
            set
            {
                if (_sessionid != value)
                {
                    _sessionid = value;
                    OnPropertyChanged(nameof(SessionId));
                }
            }
        }

        public string 用户名
        {
            get { return _用户名; }
            set
            {
                if (_用户名 != value)
                {
                    _用户名 = value;
                    OnPropertyChanged(nameof(用户名));
                }
            }
        }

        public string 姓名
        {
            get { return _姓名; }
            set
            {
                if (_姓名 != value)
                {
                    _姓名 = value;
                    OnPropertyChanged(nameof(姓名));
                }
            }
        }

        public string 当前模块
        {
            get { return _当前模块; }
            set
            {
                if (_当前模块 != value)
                {
                    _当前模块 = value;
                    OnPropertyChanged(nameof(当前模块));
                }
            }
        }

        public string 当前窗体
        {
            get { return _当前窗体; }
            set
            {
                if (_当前窗体 != value)
                {
                    _当前窗体 = value;
                    OnPropertyChanged(nameof(当前窗体));
                }
            }
        }

        public DateTime 登录时间
        {
            get { return _登录时间; }
            set
            {
                if (_登录时间 != value)
                {
                    _登录时间 = value;
                    OnPropertyChanged(nameof(登录时间));
                }
            }
        }

        public int 心跳数
        {
            get { return _心跳数; }
            set
            {
                if (_心跳数 != value)
                {
                    _心跳数 = value;
                    OnPropertyChanged(nameof(心跳数));
                }
            }
        }

        public string 最后心跳时间
        {
            get { return _最后心跳时间; }
            set
            {
                if (_最后心跳时间 != value)
                {
                    _最后心跳时间 = value;
                    OnPropertyChanged(nameof(最后心跳时间));
                }
            }
        }

        public string 客户端版本
        {
            get { return _客户端版本; }
            set
            {
                if (_客户端版本 != value)
                {
                    _客户端版本 = value;
                    OnPropertyChanged(nameof(客户端版本));
                }
            }
        }

        public string 客户端IP
        {
            get { return _客户端IP; }
            set
            {
                if (_客户端IP != value)
                {
                    _客户端IP = value;
                    OnPropertyChanged(nameof(客户端IP));
                }
            }
        }

        public long 静止时间
        {
            get { return _静止时间; }
            set
            {
                if (_静止时间 != value)
                {
                    _静止时间 = value;
                    OnPropertyChanged(nameof(静止时间));
                }
            }
        }
        #endregion

        private bool _online;
        private bool _IsSuperUser;
        private long _userID;
        private bool _serverAuthentication = true;

        /// <summary>
        /// 用户名表中的主键
        /// 比方系统提醒的人。这个时间员工可能都不用系统。所以是用户名
        /// </summary>
        public long UserID
        {
            get => _userID;
            set
            {
                if (_userID != value)
                {
                    _userID = value;
                    OnPropertyChanged(nameof(UserID));
                }
            }
        }
        public bool 超级用户
        {
            get => _IsSuperUser;
            set
            {
                if (_IsSuperUser != value)
                {
                    _IsSuperUser = value;
                    OnPropertyChanged(nameof(超级用户));
                }
            }
        }

        /// <summary>
        /// 登录过，还保持连接时，客户端会定时发送心跳包，服务器会记录最后一次收到心跳包的时间
        /// </summary>
        public bool 在线状态
        {
            get => _online;
            set
            {
                if (_online != value)
                {
                    _online = value;
                    OnPropertyChanged(nameof(在线状态));
                }
            }
        }


        private readonly object _lock = new object();

        /// <summary>
        /// 登录成功 的正常状态
        /// </summary>
        public bool 授权状态
        {
            get
            {
                lock (_lock)
                {
                    //System.Diagnostics.Debug.WriteLine($"读取  {_serverAuthentication} by Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                    return _serverAuthentication;
                }
            }
            set
            {
                lock (_lock)
                {
                    _serverAuthentication = value;
                    //System.Diagnostics.Debug.WriteLine($"授权状态 changed to {value}. StackTrace: {new System.Diagnostics.StackTrace()}");
                    // System.Diagnostics.Debug.WriteLine($"设置 set to {value} by Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                    //System.Diagnostics.Debug.WriteLine($"Call Stack: {Environment.StackTrace}");

                    if (_serverAuthentication != value)
                    {
                        //System.Diagnostics.Debug.WriteLine($"授权状态{_serverAuthentication}: {Thread.CurrentThread.ManagedThreadId}");
                        OnPropertyChanged(nameof(授权状态));
                    }
                }
            }


        }

        public string UserGroup { get; set; }

        /// <summary>
        /// 操作系统信息
        /// </summary>
        public string 操作系统
        {
            get { return _操作系统; }
            set
            {
                if (_操作系统 != value)
                {
                    _操作系统 = value;
                    OnPropertyChanged(nameof(操作系统));
                }
            }
        }

        /// <summary>
        /// 机器名
        /// </summary>
        public string 机器名
        {
            get { return _机器名; }
            set
            {
                if (_机器名 != value)
                {
                    _机器名 = value;
                    OnPropertyChanged(nameof(机器名));
                }
            }
        }

        /// <summary>
        /// CPU信息
        /// </summary>
        public string CPU信息
        {
            get { return _CPU信息; }
            set
            {
                if (_CPU信息 != value)
                {
                    _CPU信息 = value;
                    OnPropertyChanged(nameof(CPU信息));
                }
            }
        }

        /// <summary>
        /// 内存大小
        /// </summary>
        public string 内存大小
        {
            get { return _内存大小; }
            set
            {
                if (_内存大小 != value)
                {
                    _内存大小 = value;
                    OnPropertyChanged(nameof(内存大小));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
