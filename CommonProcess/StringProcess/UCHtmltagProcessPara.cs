using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{


    [Serializable]
    public class UCHtmltagProcessPara : UCBasePara
    {
      
        public UCHtmltagProcessPara()
        {

        }

        private List<string> processForHtmlTags = new List<string>();


        public List<string> ProcessForHtmlTags
        {
            get { return processForHtmlTags; }
            set { processForHtmlTags = value; }
        }


        public override string ProcessDo(string StrIn)
        {
            string rs = StrIn;
            //去脚本
            foreach (string kv in ProcessForHtmlTags)
            {
                if (kv == null)
                {
                    continue;
                }
                rs = HLH.Lib.Helper.HtmlTagProcess.RemoveHtmltag(kv, StrIn).Trim();
            }
            return rs;
        }

    
    }
}
