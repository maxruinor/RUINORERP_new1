using HLH.Lib.Helper;
using System;
using System.Collections;
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
    [XmlInclude(typeof(UCDownloadFilePara))]
    [XmlInclude(typeof(UCRepairStringPara))]
    [XmlInclude(typeof(UCXpathPickPara))]
    [XmlInclude(typeof(UCHtmltagProcessPara))]
    /// <summary>
    /// 模拟一个配置文件。在其他项目中的调用情况
    /// </summary>
    [Serializable]
    public class StringProcessConfig
    {

        private List<HLH.Lib.Helper.KeyValue> _actions = new List<HLH.Lib.Helper.KeyValue>();


        /// <summary>
        /// key 保存 动作枚举，value保存条件属性对象 tag GUID
        /// </summary>
        public List<KeyValue> Actions
        {
            get => _actions;
            set => _actions = value;
        }






    }
}
