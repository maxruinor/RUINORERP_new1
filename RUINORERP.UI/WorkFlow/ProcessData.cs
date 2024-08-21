using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlow
{
    /// <summary>
    /// 单据处理中间数据
    /// </summary>
    public class ProcessData
    {
        private int _counter = 0;
        public int Counter { get => _counter; set => _counter = value; }

        public string RequestData { get; set; }
        public string MyName { get; set; }
    }

}
