using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 思路  比方 在窗体类 出入库类型中 有 出入类型 False为出库 True为入库
    /// </summary>
    public class GlobalDataDictionary
    {
        


        ConcurrentDictionary<string, KeyValuePair<bool, string>> _DataDictionary = new ConcurrentDictionary<string, KeyValuePair<bool, string>>();

        public ConcurrentDictionary<string, KeyValuePair<bool, string>> DataDictionary { get => _DataDictionary; set => _DataDictionary = value; }

        //void init()
        //{
        //    _DataDictionary.AddOrUpdate("")
        //}
    }

}
