using System;
using System.IO;
using System.Reflection;
using System.Resources;
namespace HLH.Lib.Helper
{
    /// <summary>
    /// �Զ�����־��¼��
    /// <para>����־���ͻ��ֻ���һ����Ч��С��ʹ��</para>
    /// </summary>
    public class log4netCustomer
    {

        //˼·����̬д�������ļ�

        //private static string loggerName = ConfigHelper.getWebConfigAttribute("log4netLogger");



        private static string _loggerName = string.Empty;


        private static readonly object mylock = new object();


        public static string loggerName
        {
            get
            {
                if (!string.IsNullOrEmpty(_loggerName))
                {
                    return _loggerName;
                }
                else
                {
                    throw new Exception("���ȶ�����־��ǩ");
                };
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //�������ֵ�б仯����һ��������
                    if (!_loggerName.Equals(value))
                    {
                        _loggerName = value;
                        CreatConfig(value);
                    }
                }
                else
                {
                    throw new Exception("��־��ǩ����Ϊ��");
                }
                ;
            }
        }



        /// <summary>
        /// ��̬���������ļ��������ؼ���
        /// </summary>
        /// <param name="ploggerName"></param>
        private static void CreatConfig(string ploggerName)
        {

            #region ��̬���������ļ��������ؼ���
            lock (mylock)
            {
                Assembly ab = Assembly.GetExecutingAssembly();
                string logconfigPath = System.IO.Path.Combine(ab.Location.Replace(ab.ManifestModule.Name, ""), ploggerName + ".xml");
                if (!System.IO.File.Exists(logconfigPath))
                {
                    //����
                    ResourceManager rm = new ResourceManager("HLH.Lib.Properties.resources", ab);

                    System.IO.FileStream f = System.IO.File.Create(logconfigPath);
                    f.Close();

                    System.IO.StreamWriter f2 = new System.IO.StreamWriter(logconfigPath, false, System.Text.Encoding.UTF8);
                    string temp = rm.GetString("log4net.config");
                    temp = temp.Replace("#Watson#", ploggerName);
                    f2.Write(temp);
                    f2.Close();
                    f2.Dispose();
                }

                //��������ʱ����Ҫ�����ؼ���
                FileInfo finfo = new FileInfo(logconfigPath);
                log4net.Config.XmlConfigurator.Configure(finfo);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(finfo);
            }
            #endregion

        }










        /// <summary>
        /// ��¼������Ϣ
        /// </summary>
        /// <param name="message"></param>
        public static void debug(string message)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
            log = null;
        }

        /// <summary>
        /// ��¼������Ϣ
        /// </summary>
        /// <param name="message"></param>
        public static void error(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
            log = null;
        }

        /// <summary>
        /// ��¼������Ϣ,���쳣��Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void error(string message, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsErrorEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        log.Error(message, ex.InnerException);
                    }
                    else
                    {
                        log.Error(message, ex);
                    }
                }
            }
            log = null;
        }

        /// <summary>
        /// ��¼��������,���쳣��Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void fatal(string message, Exception ex)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsFatalEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        log.Fatal(message, ex.InnerException);
                    }
                    else
                    {
                        log.Fatal(message, ex);
                    }
                }
            }
            log = null;
        }

        /// <summary>
        /// ��¼һ����Ϣ
        /// </summary>
        /// <param name="message"></param>
        public static void info(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }

        /// <summary>
        /// ��¼������Ϣ
        /// </summary>
        /// <param name="message"></param>
        public static void warn(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
            log = null;
        }
    }
}

