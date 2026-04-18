using System;
using System.Collections.Generic;

namespace RUINORERP.Business.ImportEngine.Models
{
    /// <summary>
    /// 导入结果报告
    /// </summary>
    public class ImportReport
    {
        public bool IsSuccess { get; set; }
        public int TotalRows { get; set; }
        public int SuccessRows { get; set; }
        public List<ImportError> Errors { get; set; } = new List<ImportError>();
        public string Message { get; set; }
    }

    /// <summary>
    /// 导入错误详情
    /// </summary>
    public class ImportError
    {
        public int RowIndex { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, object> RawData { get; set; } = new Dictionary<string, object>();
    }
}
