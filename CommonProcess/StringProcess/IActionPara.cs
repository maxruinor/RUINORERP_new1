using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CommonProcess.StringProcess
{

    [XmlInclude(typeof(UCJson路径提取Para))]
    [XmlInclude(typeof(UC数组分割提取Para))]
    [XmlInclude(typeof(UC正则式提取Para))]
    public interface IActionPara
    {

        /// <summary>
        /// 动作参数的唯一标记
        /// </summary>
        string GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        bool Available { get; set; }
       // string ProcessDo(BaseProcess bp, string StrIn);


    }
}
