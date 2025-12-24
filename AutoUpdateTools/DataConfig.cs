using System;

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

        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// 是否使用基础exe版本
        /// </summary>
        public bool UseBaseExeVersion { get; set; }

        /// <summary>
        /// 基础版本号,格式为1.0.0.0,默认为1.0.0.0
        /// 用于生成软件的版本号会显示到版本UI上,但不用于生成更新包版本号
        /// </summary>
        public string BaseExeVersion { get; set; } = "1.0.0.0";
        
        /// <summary>
        /// 排除的文件后缀名集合，用换行分隔
        /// </summary>
        public string ExcludeExtensions { get; set; } = ".log\n.cache\n.tmp\n.bak";
    }
}