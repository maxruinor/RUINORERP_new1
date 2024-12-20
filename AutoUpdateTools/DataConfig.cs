using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools
{
    [Serializable]
    public class DataConfig
    {

        /// <summary>
        /// 存在的目录
        /// </summary>
        public string BaseDir { get; set; }


        /// <summary>
        /// 哈希比较时新的来源目录
        /// </summary>
        public string CompareSource { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string MineCode { get; set; }

        /// <summary>
        /// 主程序入口
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// http更新地址
        /// </summary>
        public string UpdateHttpAddress { get; set; }

        /// <summary>
        /// 排除文件集合
        /// </summary>
        public string ExcludeFiles { get; set; }


        /// <summary>
        /// 要更新的文件集合
        /// </summary>
        public string UpdatedFiles { get; set; }


        public string SavePath { get; set; }


        public bool CustomVer { get; set; }

        public string CustomVersion { get; set; }
    }

}
