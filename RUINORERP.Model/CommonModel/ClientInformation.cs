using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{

    /// <summary>
    /// 与客户端有关的信息
    /// </summary>
    public class ClientInformation
    {
        /// <summary>
        /// 电脑空闲时间，毫秒（ms)
        /// </summary>
        public long ComputerFreeTime { get; set; } = 0;


        public DateTime loginTime { set; get; } = System.DateTime.Now;

        /// <summary>
        /// 心跳节拍数
        /// </summary>
        public int BeatData { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// MIS版本
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 最后心跳时间
        /// </summary>
        public DateTime LastBeatTime { get; set; }


        private string currentModule = string.Empty;
        private string currentFormUI = string.Empty;

        public string CurrentModule { get => currentModule; set => currentModule = value; }

        public string CurrentFormUI { get => currentFormUI; set => currentFormUI = value; }


    }
}
