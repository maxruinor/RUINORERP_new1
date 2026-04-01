using RUINORERP.Model.ChartFramework.Contracts;
using RUINORERP.Model.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.ReportContainer
{
    /// <summary>
    /// 鎶ヨ〃瀹氫箟鎺ュ彛
    /// </summary>
    public interface IReportDefinition
    {
        /// <summary>
        /// 鎶ヨ〃鏍囬
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 鎶ヨ〃鍒嗙被 (濡?閿€鍞垎鏋?/"搴撳瓨鐩戞帶")
        /// </summary>
        string Category { get; }

        /// <summary>
        /// 鏌ヨ鍙傛暟绫诲瀷
        /// </summary>
        Type QueryParameterType { get; }

        /// <summary>
        /// 鏁版嵁鎻愪緵鑰?
        /// </summary>
        IDataProvider DataProvider { get; }

        /// <summary>
        /// 鏌ヨ澶勭悊濮旀墭
        /// </summary>
        Func<DataRequest, Task<ChartData>> QueryHandler { get; }
    }

    /// <summary>
    /// 閫氱敤鎶ヨ〃瀹氫箟瀹炵幇
    /// </summary>
    public class ReportDefinition<TParam> : IReportDefinition
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public Type QueryParameterType => typeof(TParam);
        public IDataProvider DataProvider { get; set; }
        public Func<DataRequest, Task<ChartData>> QueryHandler { get; set; }
    }
}

