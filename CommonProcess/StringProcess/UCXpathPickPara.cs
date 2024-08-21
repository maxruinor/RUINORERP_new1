using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{


    [Serializable]
    public class UCXpathPickPara : UCBasePara
    {


        public UCXpathPickPara()
        {

        }




        private string _XPath;

        public string XPath
        {
            get { return _XPath; }
            set { _XPath = value; }
        }


        private bool _useOuterHtml;

        public bool useOuterHtml
        {
            get { return _useOuterHtml; }
            set { _useOuterHtml = value; }
        }

        private bool _useInnerHtml;

        public bool useInnerHtml
        {
            get { return _useInnerHtml; }
            set { _useInnerHtml = value; }
        }

        private bool _useInnerText;

        public bool useInnerText
        {
            get { return _useInnerText; }
            set { _useInnerText = value; }
        }

        private bool _use指定属性值;

        public bool use指定属性值
        {
            get { return _use指定属性值; }
            set { _use指定属性值 = value; }
        }

        private string _Xpath指定属性值;

        public event EventHandler DebugTrackerEvent;

        public string Xpath指定属性值
        {
            get { return _Xpath指定属性值; }
            set { _Xpath指定属性值 = value; }
        }





        public override string ProcessDo(string StrIn)
        {
            string rs = StrIn;
            try
            {
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(StrIn);
                HtmlAgilityPack.HtmlNode mynode = htmlDoc.DocumentNode.SelectSingleNode(XPath);
                if (mynode != null)
                {
                    if (useOuterHtml)
                    {
                        rs = mynode.OuterHtml;
                    }
                    if (useInnerHtml)
                    {
                        rs = mynode.InnerHtml;
                    }
                    if (useInnerText)
                    {
                        rs = mynode.InnerText;
                    }
                    if (use指定属性值)
                    {
                        rs = mynode.Attributes[Xpath指定属性值].Value;
                    }
                }
                else
                {
                    // frmMain.InstancePicker.PrintInfoLog(System.Threading.Thread.CurrentThread.ManagedThreadId + df.FieldName + "【值不存在】" + df.XPath + "xpath选择的节点为空，请重新设置xpath.");

                    rs = "";
                }

            }
            catch (Exception ex)
            {
                // if (NotifyExternalEvent != null)
                //{
                //  NotifyExternalEvent("【" + df.FieldName + "】" + "-" + ex.Message);
                //}
            }


            return rs;
        }


    }
}
