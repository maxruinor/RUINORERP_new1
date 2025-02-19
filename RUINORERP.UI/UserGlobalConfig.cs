using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLH.Lib.Helper;
using RUINORERP.Model.Models;

namespace RUINORERP.UI
{


    /// <summary>
    /// 所有系统设置，所有用户相同 保存到数据库，只是一台电脑上的配置
    /// </summary>
    [Serializable()]
    public class UserGlobalConfig
    {
        //系统中经常会导入导出数据。这里是通用工具的一个配置文件目录 
        //定义 一个保存配置文件的目录
        private string matchColumnsConfigDir = System.IO.Path.Combine(Environment.CurrentDirectory, "ColumnsConfigTools");// System.Reflection.Assembly.GetEntryAssembly().Location;// "";
        public string MatchColumnsConfigDir
        {
            get
            {
                if (matchColumnsConfigDir == null)
                {
                    matchColumnsConfigDir = System.IO.Path.Combine(Environment.CurrentDirectory, "ColumnsConfigTools");
                }
                return matchColumnsConfigDir;
            }
            set { matchColumnsConfigDir = value; }
        }

        private string useName;
        private string passWord;

        private bool autoSavePwd;


        #region df

        private string uploadWebSiteCode = string.Empty;

        public string UploadWebSiteCode
        {
            get { return uploadWebSiteCode; }
            set { uploadWebSiteCode = value; }
        }
        private string uploadCategoryNo = string.Empty;

        public string UploadCategoryNo
        {
            get { return uploadCategoryNo; }
            set { uploadCategoryNo = value; }
        }



        private bool skipCurrentVersion = false;

        /// <summary>
        /// 是否跳过当前版本，如果更新操作成功后。设置为false,当强制，主动推送更新时失效
        /// </summary>
        public bool SkipCurrentVersion
        {
            get { return skipCurrentVersion; }
            set { skipCurrentVersion = value; }
        }


        #endregion



        private static UserGlobalConfig m_instance = null;



        /*
         临时性个性化设置
        目前暂时将每个窗体的属性会来设置这个。保存到这里是不个键值对。
        以菜单路径为key，以对应的属性为value

         */

        //public ConcurrentDictionary<string, MenuPersonalization> _MenuPersonalizationlist = new ConcurrentDictionary<string, MenuPersonalization>();

        /// <summary>
        /// 队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
        /// </summary>
        public ConcurrentDictionary<string, MenuPersonalization> MenuPersonalizationlist { get; set; } = new ConcurrentDictionary<string, MenuPersonalization>();

        
        #region 其他

        public UserGlobalConfig()
        {
        }


        public static UserGlobalConfig Instance
        {
            get
            {


                if (m_instance == null)
                {
                    m_instance = Deserialize();
                    if (m_instance == null)
                    { Initialize(); }
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }

        private bool autoRminderUpdate = true;

        /// <summary>
        /// 缓存用户名和密码
        /// </summary>
        public string UseName { get => useName; set => useName = value; }

        /// <summary>
        /// 服务器Port
        /// </summary>
        public string ServerPort { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// 数据库的IP
        /// </summary>
        public string DBServerIP { get; set; }

        public string PassWord { get => passWord; set => passWord = value; }

        private bool isSupperUser = false;

        public bool AutoSavePwd { get => autoSavePwd; set => autoSavePwd = value; }
        public bool AutoRminderUpdate { get => autoRminderUpdate; set => autoRminderUpdate = value; }
        public bool IsSupperUser { get => isSupperUser; set => isSupperUser = value; }



        // Shared (static) method to return the user settings from disk.  The SerializationHelper
        // class reads, decrypts and deserializes the user settings for the local user.
        public static UserGlobalConfig Deserialize()
        {
            UserGlobalConfig userGlobalConfig = new UserGlobalConfig();
            try
            {
                userGlobalConfig=(UserGlobalConfig)SerializationHelper.Deserialize(Application.StartupPath + "\\" + "UserGlobalConfig", false);
            }
            catch (Exception)
            {
                userGlobalConfig = new UserGlobalConfig();
            }
            return userGlobalConfig;
        }


        public void Serialize()
        {
            SerializationHelper.Serialize(this, System.Windows.Forms.Application.StartupPath + "\\" + "UserGlobalConfig", false);
        }


        public void Serialize(UserGlobalConfig userset)
        {
            //SerializationHelper.Serialize(userset, Application.StartupPath + "\\" + ConfigurationSettings.AppSettings["PersonalityFileName"]);
            SerializationHelper.Serialize(userset, Application.StartupPath + "\\" + "UserGlobalConfig", false);
        }


        public static void Delete()
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\" + "UserGlobalConfig"))
                {
                    File.Delete(Application.StartupPath + "\\" + "UserGlobalConfig");
                }
            }
            catch (Exception)
            {
                // Eat exception if this fails.
            }
        }


        /// <summary>
        /// 对象实例化
        /// </summary>
        public static void Initialize()
        {
            m_instance = new UserGlobalConfig();
        }
        #endregion
    }


    [StructLayout(LayoutKind.Sequential)]
    struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }
}
