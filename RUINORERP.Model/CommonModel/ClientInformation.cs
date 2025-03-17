using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 用户信息在服务器端显示管理客户端的信息
    /// 与客户端有关的信息
    /// </summary>
    public class UserInfo : INotifyPropertyChanged
    {
        private string _sessionid;
        private string _用户名;
        private string _姓名;
        private string _当前模块;
        private string _当前窗体;
        private DateTime _登陆时间;
        private int _心跳数;
        private string _最后心跳时间;
        private string _客户端版本;
        private string _客户端IP;
        private long _静止时间;
        private long _Employee_ID;

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

        public DateTime 登陆时间
        {
            get { return _登陆时间; }
            set
            {
                if (_登陆时间 != value)
                {
                    _登陆时间 = value;
                    OnPropertyChanged(nameof(登陆时间));
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
        /// 登陆过，还保持连接时，客户端会定时发送心跳包，服务器会记录最后一次收到心跳包的时间
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
        /// 登陆成功 的正常状态
        /// </summary>
        public bool 授权状态
        {
            get
            {
                lock (_lock)
                {
                    //Console.WriteLine($"读取  {_serverAuthentication} by Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                    return _serverAuthentication;
                }
            }
            set
            {
                lock (_lock)
                {
                    _serverAuthentication = value;
                    //Console.WriteLine($"授权状态 changed to {value}. StackTrace: {new System.Diagnostics.StackTrace()}");
                   // Console.WriteLine($"设置 set to {value} by Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                    //Console.WriteLine($"Call Stack: {Environment.StackTrace}");

                    if (_serverAuthentication != value)
                    {
                        //Console.WriteLine($"授权状态{_serverAuthentication}: {Thread.CurrentThread.ManagedThreadId}");
                        OnPropertyChanged(nameof(授权状态));
                    }
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
