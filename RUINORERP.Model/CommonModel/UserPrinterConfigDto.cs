using System;
using System.Collections.Generic;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 用户打印机配置数据传输对象
    /// </summary>
    public class UserPrinterConfigDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 是否使用用户自己的打印机
        /// </summary>
        public bool UseUserOwnPrinter { get; set; }

        /// <summary>
        /// 全局打印机名称
        /// </summary>
        public string GlobalPrinterName { get; set; }

        /// <summary>
        /// 按业务类型区分的打印机配置
        /// </summary>
        public Dictionary<string, string> BizTypePrinters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 最后同步时间
        /// </summary>
        public DateTime LastSyncTime { get; set; }

        /// <summary>
        /// 获取指定业务类型的打印机
        /// </summary>
        /// <param name="bizName">业务名称</param>
        /// <returns>打印机名称，如果未配置返回null</returns>
        public string GetPrinterForBizType(string bizName)
        {
            if (BizTypePrinters != null && BizTypePrinters.TryGetValue(bizName, out string printerName))
            {
                return printerName;
            }
            return null;
        }

        /// <summary>
        /// 设置指定业务类型的打印机
        /// </summary>
        /// <param name="bizName">业务名称</param>
        /// <param name="printerName">打印机名称</param>
        public void SetPrinterForBizType(string bizName, string printerName)
        {
            BizTypePrinters ??= new Dictionary<string, string>();
            if (string.IsNullOrEmpty(printerName))
            {
                BizTypePrinters.Remove(bizName);
            }
            else
            {
                BizTypePrinters[bizName] = printerName;
            }
        }
    }
}
