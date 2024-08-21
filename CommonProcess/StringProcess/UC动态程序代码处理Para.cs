using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{

    [Serializable]
    public class UC动态程序代码处理Para : UCBasePara
    {

        private List<string> referenceDlls = new List<string>();

        public List<string> ReferenceDlls
        {
            get { return referenceDlls; }
            set { referenceDlls = value; }
        }


        private string dynamicCsharpCode = string.Empty;

 

        /// <summary>
        /// 针对特殊的字段，需要用c#程序代码进行数据处理，
        /// 这个字段来保存要动态执行的代码
        /// </summary>
        public string DynamicCsharpCode
        {
            get { return dynamicCsharpCode; }
            set { dynamicCsharpCode = value; }
        }
        public UC动态程序代码处理Para()
        {

        }



        public override string ProcessDo(string StrIn)
        {
            string rs = StrIn;
            try
            {
                //深度处理应该是最后一招，其次再是下载文件

                if (@rs.Trim().Length == 0)
                {
                    // frmMain.InstancePicker.PrintInfoLog(Thread.CurrentThread.ManagedThreadId + "【深度处理时】输入参数为空.", System.Drawing.Color.Red);
                }
                else
                {
                    rs = DynamicProcessData(ReferenceDlls, DynamicCsharpCode, @rs);
                }

            }
            catch (Exception ex)
            {
                //PrintDebugInfo("【" + ReferenceDlls + "|" + DynamicCsharpCode + "】" + ex.Message + ex.StackTrace);
            }
            return rs;
        }

        public string DynamicProcessData(List<string> referenceDlls, string dynamicCode, object InputParameters)
        {
            string[] _SystemSpecRefs =
     {
            "System.Configuration",
            "System.Configuration.Install",
            "System.Data",
            //"System.Data.SqlClient",
            "System.Data.SqlXml",
            "System.Deployment",
            "System.Design",
            //"Sysemt.DirecoryServices",
            "System.DirectoryServices.Protocols",
            "System.Drawing",
            "System.Drawing.Design",
            "System.EnterpriseServices",
            "System.Management",
            "System.Messaging",
            "System.Runtime.Remoting",
            "System.Runtime.Serialization.Formatters.Soap",
            "System.Security",
            "System.ServiceProcess",
            "System.Transactions",
            "System.Web",
            "System.Web.Mobile",
            "System.Web.RegularExpressions",
            "System.Web.Services",
            "System.Windows.Forms",
            "System.Xml",
        };

            //先保存到List中，防止 重复
            List<string> ReferenceDll = new List<string>();
            //手动加入常用的dll
            ReferenceDll.Add("mscorlib.dll");
            ReferenceDll.Add("system.dll");
            ReferenceDll.Add("system.data.dll");
            ReferenceDll.Add("system.xml.dll");
            ReferenceDll.Add("System.Windows.Forms.dll");

            foreach (string s in _SystemSpecRefs)
            {
                if (!ReferenceDll.Contains(s + ".dll".ToLower().ToString().Trim()))
                {
                    ReferenceDll.Add(s + ".dll");
                }
            }

            foreach (string item in referenceDlls)
            {
                if (!ReferenceDll.Contains(item.ToString().Trim()))
                {
                    ReferenceDll.Add(item);
                }
            }
            CodeCompiler.Compile(ReferenceDll.ToArray(), dynamicCode, "", new object[] { InputParameters });
            // CodeCompiler.Compile(new string[] { }, richTextBox1.Text, "");
            foreach (string s in CodeCompiler.ErrorMessage)
            {
                //TODO:没完成
                //frmMain.InstancePicker.PrintInfoLog(s);
                //PrintDebugInfo(s);
            }
            return CodeCompiler.Message;


        }
    }





}
