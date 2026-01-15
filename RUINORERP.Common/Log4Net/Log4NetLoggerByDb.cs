using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Context;

namespace RUINORERP.Common.Log4Net
{

    /// <summary>
    /// 2023-11-22 添加了数据库和自定义字段,重写了自定义字段写入方法
    /// 生成仓库时。应该也可以用配置文件。只是字符串明码了。也可以字串重写。其他自定义应该是可以的 CustomAdoNetAppender
    /// 如果发现有些日志没有保存进去。有些可以。要看下是不是日志内容太长了。数据库的字段不够长。
    /// </summary>
    public class Log4NetLoggerByDb : ILogger
    {
        private readonly string _name;
        private readonly XmlElement _xmlElement;
        private readonly ILog _log;
        private ApplicationContext _appcontext;
        
        // 静态成员变量用于存储单例日志仓库实例
        private static volatile ILoggerRepository _sharedLoggerRepository = null;
        // 用于线程同步的锁对象
        private static readonly object _lockObject = new object();
        
        // 静态ConcurrentDictionary用于存储所有logger实例，避免重复创建
        private static readonly ConcurrentDictionary<string, ILog> _sharedLogs = new ConcurrentDictionary<string, ILog>();
        
        public Log4NetLoggerByDb(string name, XmlElement xmlElement, string ConnectionStr, ApplicationContext appcontext)
        {
            _appcontext = appcontext;
            _name = name;
            _xmlElement = xmlElement;
            
            // 获取或创建共享的日志仓库
            var loggerRepository = CreateLoggerRepository(ConnectionStr);
            
            // 使用静态ConcurrentDictionary确保每个名称只创建一个logger实例
            _log = _sharedLogs.GetOrAdd(name, (loggerName) => 
            {
                System.Diagnostics.Debug.WriteLine($"创建新的日志记录器: {loggerName}");
                return LogManager.GetLogger(loggerRepository.Name, loggerName);
            });
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        /// <summary>
        /// 两种方式，一种是外面提供全部信息。解析。再一次种是外部不变。里面补充行为信息
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="TState">日志状态类型</typeparam>
        /// <param name="logLevel">日志级别</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="state">日志状态对象，支持字符串类型直接传入</param>
        /// <param name="exception">异常对象，可为null</param>
        /// <param name="formatter">日志格式化委托，可为null</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            System.Exception exception, Func<TState, System.Exception, string> formatter)
        {
            try
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }
                
                // 处理日志消息，支持多种情况：
                // 1. 有格式化器时使用格式化器生成消息
                // 2. 无格式化器但state为字符串时直接使用
                // 3. 无格式化器且state非字符串时调用ToString()
                string message = null;
                
                if (formatter != null)
                {
                    try
                    {
                        message = formatter(state, exception);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("格式化日志消息失败: " + ex.Message);
                        // 格式化失败时，尝试直接使用state
                        message = GetLogMessageFromState(state, exception, ex);
                    }
                }
                else
                {
                    // 无格式化器时，直接从state获取消息
                    message = GetLogMessageFromState(state, exception, null);
                }
                
                // 确保应用程序上下文不为空
                if (_appcontext == null)
                {
                    System.Diagnostics.Debug.WriteLine("警告: 应用程序上下文为空");
                    return;
                }
                
                // 确保日志对象不为空
                if (_appcontext.log == null)
                {
                    _appcontext.log = new Logs();
                    // 设置默认日志属性
                    _appcontext.log.IP = "server";
                    _appcontext.log.MachineName = System.Environment.MachineName + "-" + System.Environment.UserName;
                }

                // 使用ThreadContext.Properties代替MDC，确保与log4net新版本兼容
                // 同时设置MDC以保持向后兼容
                if (_appcontext.log.User_ID!=null)
                {
                    log4net.ThreadContext.Properties["User_ID"] = _appcontext.log.User_ID.ToString();
                    log4net.MDC.Set("User_ID", _appcontext.log.User_ID.ToString());
                }
                else
                {
                    log4net.ThreadContext.Properties["User_ID"] = "0"; // 使用0代替空字符串，避免格式转换异常
                    log4net.MDC.Set("User_ID", "0");
                }
             
                log4net.ThreadContext.Properties["ModName"] = _appcontext.log.ModName ?? "";
                log4net.MDC.Set("ModName", _appcontext.log.ModName ?? "");
                
                log4net.ThreadContext.Properties["ActionName"] = _appcontext.log.ActionName ?? "";
                log4net.MDC.Set("ActionName", _appcontext.log.ActionName ?? "");
                
                log4net.ThreadContext.Properties["IP"] = _appcontext.log.IP ?? "";
                log4net.MDC.Set("IP", _appcontext.log.IP ?? "");
                
                log4net.ThreadContext.Properties["Path"] = _appcontext.log.Path ?? "";
                log4net.MDC.Set("Path", _appcontext.log.Path ?? "");
                
                log4net.ThreadContext.Properties["MAC"] = _appcontext.log.MAC ?? "";
                log4net.MDC.Set("MAC", _appcontext.log.MAC ?? "");
                
                log4net.ThreadContext.Properties["MachineName"] = _appcontext.log.MachineName ?? "";
                log4net.MDC.Set("MachineName", _appcontext.log.MachineName ?? "");
                
                // 当CurUserInfo为空时，使用默认值
                if (_appcontext.CurUserInfo == null)
                {
                    log4net.ThreadContext.Properties["Operator"] = "系统服务";
                    log4net.MDC.Set("Operator", "系统服务");
                }
                else
                {
                    log4net.ThreadContext.Properties["Operator"] = _appcontext.CurUserInfo.客户端版本 ?? "未登录用户";
                    log4net.MDC.Set("Operator", _appcontext.CurUserInfo.客户端版本 ?? "未登录用户");
                }
                
                log4net.ThreadContext.Properties["Message"] = message ?? "";
                log4net.MDC.Set("Message", message ?? "");
                
                log4net.ThreadContext.Properties["Exception"] = exception?.StackTrace ?? "";
                log4net.MDC.Set("Exception", exception?.StackTrace ?? "");
                
                // 通过反射取出异常值 - 注意：这里只更新上下文属性，不重复记录日志
                try
                {
                    if (state != null)
                    {
                        FieldInfo[] fieldsInfo = state.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                        FieldInfo props = fieldsInfo.FirstOrDefault(o => o.Name == "_values");
                        if (props != null)
                        {
                            object[] values = (object[])props?.GetValue(state);
                            if (values != null)
                            {
                                foreach (var item in values)
                                {
                                    if (item is Exception ex)
                                    {
                                        string exMessage = ex.Message;
                                        string exStackTrace = ex.StackTrace;
                                        
                                        // 递归处理所有层级的InnerException
                                        Exception innerEx = ex.InnerException;
                                        while (innerEx != null)
                                        {
                                            exMessage += "\r\n" + innerEx.Message;
                                            exStackTrace += "\r\n" + innerEx.StackTrace;
                                            innerEx = innerEx.InnerException;
                                        }
                                        
                                        // 更新上下文属性
                                        if (!string.IsNullOrEmpty(message))
                                        {
                                            log4net.MDC.Set("Message", message + "\r\n" + exMessage);
                                        }
                                        else
                                        {
                                            log4net.MDC.Set("Message", exMessage);
                                        }
                                        
                                        if (!string.IsNullOrEmpty(exception?.StackTrace))
                                        {
                                            log4net.MDC.Set("Exception", exception.StackTrace + "\r\n" + exStackTrace);
                                        }
                                        else
                                        {
                                            log4net.MDC.Set("Exception", exStackTrace);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("处理日志状态对象失败: " + ex.Message);
                }
                
                // 确保至少有消息或异常时才记录日志
                if (!string.IsNullOrEmpty(message) || exception != null)
                {
                    switch (logLevel)
                    {
                        case LogLevel.Critical:
                            if (_log.IsFatalEnabled)
                                _log.Fatal(message, exception);
                            break;
                        case LogLevel.Debug:
                        case LogLevel.Trace:
                            if (_log.IsDebugEnabled)
                                _log.Debug(message, exception);
                            break;
                        case LogLevel.Error:
                            if (_log.IsErrorEnabled)
                                _log.Error(message, exception);
                            break;
                        case LogLevel.Information:
                            if (_log.IsInfoEnabled)
                                _log.Info(message, exception);
                            break;
                        case LogLevel.Warning:
                            if (_log.IsWarnEnabled)
                                _log.Warn(message, exception);
                            break;
                        default:
                            if (_log.IsWarnEnabled)
                                _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.", exception);
                            else if (_log.IsInfoEnabled)
                                _log.Info(message, exception);
                            break;
                    }
                }
                 
            }
            catch (Exception exx)
            {
                System.Diagnostics.Debug.WriteLine($"日志处理异常: {exx.Message}\n{exx.StackTrace}");
            }
        }
        
        /// <summary>
        /// 从日志状态对象中获取日志消息
        /// </summary>
        /// <typeparam name="TState">日志状态类型</typeparam>
        /// <param name="state">日志状态对象</param>
        /// <param name="exception">异常对象</param>
        /// <param name="formatException">格式化异常，可为null</param>
        /// <returns>日志消息字符串</returns>
        private string GetLogMessageFromState<TState>(TState state, System.Exception exception, Exception formatException)
        {
            string message = string.Empty;
            
            // 优先处理字符串类型的state
            if (state is string strState)
            {
                message = strState;
            }
            // 非字符串类型调用ToString()
            else if (state != null)
            {
                try
                {
                    message = state.ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("调用state.ToString()失败: " + ex.Message);
                    message = "无法获取日志消息: 调用ToString()失败";
                }
            }
            
            // 处理格式化异常信息
            if (formatException != null)
            {
                message += $" (格式化异常: {formatException.Message})";
            }
            
            // 如果没有获取到消息，但有异常，使用异常消息
            if (string.IsNullOrEmpty(message) && exception != null)
            {
                message = exception.Message;
            }
            
            return message;
        }


        /// <summary>
        /// 创建或获取日志仓库的单例实例
        /// 使用双重检查锁定模式确保线程安全
        /// </summary>
        /// <param name="_ConnectionString">数据库连接字符串</param>
        /// <returns>日志仓库实例</returns>
        public ILoggerRepository CreateLoggerRepository(string _ConnectionString)
        {
            // 使用双重检查锁定模式
            if (_sharedLoggerRepository == null)
            {
                lock (_lockObject)
                {
                    if (_sharedLoggerRepository == null)
                    {
                        // 创建一个固定名称的仓库，而不是每次生成新的GUID
                        _sharedLoggerRepository = log4net.LogManager.CreateRepository("RUINORERP_Shared_LoggerRepository");

                        AdoNetAppender adoNetAppender = new AdoNetAppender();
                        // 增加缓冲区大小以减少数据库连接开销
                        // 使用frmMainNew中设置的动态BufferSize值
                        adoNetAppender.BufferSize = 1;// RUINORERP.Server.frmMainNew.GetLogBufferSize(); // 批量写入日志
                        adoNetAppender.Lossy = false; // 确保不丢失日志

                        adoNetAppender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                        if (string.IsNullOrEmpty(_ConnectionString))
                        {
                            // 如果连接字符串为空，尝试从配置文件获取
                            try
                            {
                                _ConnectionString = CryptoHelper.GetDecryptedConnectionString();
                                System.Diagnostics.Debug.WriteLine("从配置文件获取连接字符串成功");
                                adoNetAppender.ConnectionString = _ConnectionString;
                                System.Diagnostics.Debug.WriteLine("已应用从配置文件获取的连接字符串");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("警告：获取连接字符串失败，使用备用连接字符串: " + ex.Message);
                                // 只有在无法从配置文件获取时才使用备用连接字符串
                                adoNetAppender.ConnectionString = "Server=192.168.0.254;Database=erpnew;UID=sa;Password=SA!@#123sa;Max Pool Size=1000;MultipleActiveResultSets=True;Min Pool Size=0;Connection Lifetime=0;";
                            }
                        }
                        else
                        {
                            // 如果传入了连接字符串，直接使用
                            adoNetAppender.ConnectionString = _ConnectionString;
                            System.Diagnostics.Debug.WriteLine("已应用传入的连接字符串进行日志数据库连接");
                        }

                        // 配置INSERT命令和参数 - 调整字段顺序和参数大小以匹配数据库表结构
                        adoNetAppender.CommandText = "INSERT INTO Logs ([Date],[Level],[Logger],[Message],[Exception],[Operator],[ModName],[Path],[ActionName],[IP],[MAC],[MachineName],[User_ID]) VALUES (@log_date, @log_level, @logger, @Message, @Exception, @Operator, @ModName, @Path, @ActionName, @IP, @MAC, @MachineName, @User_ID)";
                        adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_date", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
                        adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 10, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
                        adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 500, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });

                        // 提取公共方法减少重复代码 - 调整参数大小以匹配数据库表结构
                        AddParameter(adoNetAppender, "@Operator", System.Data.DbType.String, 200, "%property{Operator}");
                        AddParameter(adoNetAppender, "@User_ID", System.Data.DbType.Int64, 8, "%property{User_ID}");
                        AddParameter(adoNetAppender, "@ModName", System.Data.DbType.String, 50, "%property{ModName}");
                        AddParameter(adoNetAppender, "@MAC", System.Data.DbType.String, 30, "%property{MAC}");
                        AddParameter(adoNetAppender, "@IP", System.Data.DbType.String, 20, "%property{IP}");
                        AddParameter(adoNetAppender, "@Path", System.Data.DbType.String, 100, "%property{Path}");
                        AddParameter(adoNetAppender, "@ActionName", System.Data.DbType.String, 50, "%property{ActionName}");
                        AddParameter(adoNetAppender, "@MachineName", System.Data.DbType.String, 50, "%property{MachineName}");
                        // 对于text类型的字段，使用较大的大小
                        AddParameter(adoNetAppender, "@Message", System.Data.DbType.String, int.MaxValue, "%property{Message}");
                        AddParameter(adoNetAppender, "@Exception", System.Data.DbType.String, int.MaxValue, "%property{Exception}");
                        
                        // 只在所有参数添加完成后调用一次ActivateOptions
                        adoNetAppender.ActivateOptions();

                        // 配置仓库使用我们的appender
                        log4net.Config.BasicConfigurator.Configure(_sharedLoggerRepository, adoNetAppender);
                        System.Diagnostics.Debug.WriteLine("日志仓库已成功创建并配置");
                    }
                }
            }

            // 返回共享的日志仓库实例
            return _sharedLoggerRepository;
        }

        /// <summary>
        /// 添加参数到AdoNetAppender
        /// </summary>
        /// <param name="appender">AdoNetAppender实例</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="conversionPattern">转换模式</param>
        private void AddParameter(AdoNetAppender appender, string paramName, System.Data.DbType dbType, int size, string conversionPattern)
        {
            var layout = new EnhancedCustomLayout { ConversionPattern = conversionPattern };
            layout.ActivateOptions();
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = paramName,
                DbType = dbType,
                Size = size,
                Layout = new Layout2RawLayoutAdapter(layout)
            });
        }



    }
}

