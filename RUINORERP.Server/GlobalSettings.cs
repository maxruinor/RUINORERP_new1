using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Server
{
    public enum DebugController
    {
        Info,
        Debug,
        Run
    }

    /// <summary>
    /// 天气
    /// </summary>
    public enum DayAndNight
    {
        Day,
        Night
    }

    public class GlobalSettings
    {
        public static DebugController _debug = DebugController.Run;
        public GlobalSettings()
        {
            //暂时初始化为调试
            // _debug = DebugController.Debug;
           // m_instance = this;
        }

        private static GlobalSettings m_instance;
        internal static GlobalSettings Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Initialize();  
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public static void Initialize()
        {
            m_instance = new GlobalSettings();
        }





        public int GameSystemTime { get => _GameSystemTime; set => _GameSystemTime = value; }

        private int _GameSystemTime = 1;


    }
}
