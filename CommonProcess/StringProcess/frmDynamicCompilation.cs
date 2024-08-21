using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Actions;

namespace CommonProcess.StringProcess
{
    public partial class frmDynamicCompilation : Form
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


        public frmDynamicCompilation()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void frmDynamicCompilation_Load(object sender, EventArgs e)
        {
            //如果新建 则给出提示

            if (DynamicCsharpCode.Trim().ToString().Length > 0 && ReferenceDlls.Count > 0)
            {
                // richTextBox临时不用的.Text = DynamicCsharpCode;
                txtEditorControl动态代码.Text = DynamicCsharpCode;
                watermaskRichTextBox1.Lines = ReferenceDlls.ToArray();
            }
            else
            {
                //给出默认的修改
                MessageBox.Show("动态代码处理默认提示代码。请自行修改。");
                watermaskRichTextBox1.AppendText("mscorlib.dll");
                watermaskRichTextBox1.AppendText("\r\n");
                watermaskRichTextBox1.AppendText("Newtonsoft.Json.dll");
                watermaskRichTextBox1.AppendText("\r\n");
                watermaskRichTextBox1.AppendText("MyLocoySpider.exe");
                watermaskRichTextBox1.AppendText("\r\n");
                watermaskRichTextBox1.AppendText("HLH.Lib.dll");
                watermaskRichTextBox1.AppendText("\r\n");

                txtEditorControl动态代码.Text = @"using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MyLocoySpider;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

  class  ClassDynamicProcessData

  {


  public string  GetResult(string s)
  {

//因为知道采集的内容中包含的unicode转义字符

               s=HLH.Lib.Helper.StringHelper.DecodeForUnicode(s);
  //MessageBox.Show(s);
            string rs = "";


           // JsonHelper helper = new JsonHelper();
           
            // object objJson = helper.GetObjectByJsonString(rs);

            object objJson=Newtonsoft.Json.JavaScriptConvert.DeserializeObject(s.ToString());

            Newtonsoft.Json.JavaScriptArray list = objJson as JavaScriptArray;

            foreach (object item in list)
            {
                Newtonsoft.Json.JavaScriptObject jso = (JavaScriptObject)item; 
                
                 //测试出 key
                 
                Dictionary<string, object> tempJso = new Dictionary<string, object>();
                tempJso = jso as Dictionary<string, object>;
                foreach (System.Collections.Generic.KeyValuePair<string, object> dkv in tempJso)
                {
                    //MessageBox.Show(dkv.Key.ToString());
                    // maxImageUrl displayImgUrl 这两个值 一个最大一个正常显示。有时候可能最大的不存在，则用显示值
                 
                  
                }
                
            }

            //foreach (string d in jso.Keys)
            //{
            //    str = d.ToString();
            //}
           rs = rs.TrimEnd('|');

            return rs;




}

}";

            }


            txtEditorControl动态代码.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ReferenceDlls.Clear();
            foreach (string item in watermaskRichTextBox1.Lines)
            {
                if (!ReferenceDlls.Contains(item.ToString().Trim()))
                {
                    ReferenceDlls.Add(item);
                }
            }

            // DynamicCsharpCode = richTextBox临时不用的.Text;
            DynamicCsharpCode = txtEditorControl动态代码.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }





        private void btnTest_Click(object sender, EventArgs e)
        {
            /*
            string ss = HLH.Lib.Helper.StringHelper.DecodeForUnicode(richTextBoxinput.Text);
            //MessageBox.Show(s);
            string rs = "";

            // JsonHelper helper = new JsonHelper();

            // object objJson = helper.GetObjectByJsonString(rs);

            object objJson = Newtonsoft.Json.JavaScriptConvert.DeserializeObject(ss.ToString());

            Newtonsoft.Json.JavaScriptArray list = objJson as Newtonsoft.Json.JavaScriptArray;

            foreach (object item in list)
            {
                Newtonsoft.Json.JavaScriptObject jso = (Newtonsoft.Json.JavaScriptObject)item;

                //测试出 key
                #region 测试
              
                Dictionary<string, object> tempJso = new Dictionary<string, object>();
                tempJso = jso as Dictionary<string, object>;
                foreach (System.Collections.Generic.KeyValuePair<string, object> dkv in tempJso)
                {
                    //MessageBox.Show(dkv.Key.ToString());
                    // maxImageUrl displayImgUrl 这两个值 一个最大一个正常显示。有时候可能最大的不存在，则用显示值


                    if (dkv.Key.ToString() == "maxImageUrl" && dkv.Value != null)
                    {
                        //MessageBox.Show(dkv.Value.ToString());
                        rs += dkv.Value.ToString() + "|";
                        break;
                    }
                    else
                    {
                        if (dkv.Key.ToString() == "displayImgUrl" && dkv.Value != null)
                        {
                            //MessageBox.Show(dkv.Value.ToString());
                            rs += dkv.Value.ToString() + "|";
                            break;
                        }
                    }
                }
               
                #endregion

                if (jso["maxImageUrl"] != null)
                {
                     rs += jso["maxImageUrl"].ToString() + "|";
                }
                else
                {
                    rs += jso["displayImgUrl"].ToString() + "|";
                }

                // rs += jso["thumbImgUrl"].ToString() + "|";
                // MessageBox.Show(jso["maxImageUrl"].ToString());
            }

            rs = rs.TrimEnd('|');

        */


            //测试前要保存
            try
            {
                #region 测试动态代码

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

                foreach (string item in watermaskRichTextBox1.Lines)
                {
                    if (!ReferenceDll.Contains(item.ToString().Trim()))
                    {
                        ReferenceDll.Add(item);
                    }
                }
                CodeCompiler.Compile(ReferenceDll.ToArray(), txtEditorControl动态代码.Text, "", new object[] { richTextBoxinput.Text });
                // CodeCompiler.Compile(new string[] { }, richTextBox1.Text, "");
                listBox1.Items.Clear();
                foreach (string s in CodeCompiler.ErrorMessage)
                {
                    listBox1.Items.Add(s);
                }
                listBox1.Items.Add(CodeCompiler.Message);

                #endregion
            }
            catch (Exception exx)
            {
                listBox1.Items.Add(exx.Message + "|" + exx.Source + "|" + exx.StackTrace);
                if (exx.InnerException != null)
                {
                    listBox1.Items.Add(exx.InnerException.Message + "|" + exx.InnerException.Source + "|" + exx.InnerException.StackTrace);
                }
            }
        }

        private void btmCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Clipboard.SetText(listBox1.SelectedItem.ToString());
            }
        }
    }










    static class CodeCompiler
    {
        static public string Message;
        static public List<string> ErrorMessage = new List<string>();

        public static bool Compile(string[] references, string source, string outputfile, object[] para)
        {
            // 编译参数  
            CompilerParameters param = new CompilerParameters(references, outputfile, true);

            //param.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            param.TreatWarningsAsErrors = false;
            param.GenerateExecutable = false;
            param.IncludeDebugInformation = true;

            // 编译  
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults result = provider.CompileAssemblyFromSource(param, new string[] { source });

            Message = "";
            ErrorMessage.Clear();
            if (!result.Errors.HasErrors)
            { // 反射调用  
                Type t = result.CompiledAssembly.GetType("ClassDynamicProcessData");
                if (t != null)
                {
                    object o = result.CompiledAssembly.CreateInstance("ClassDynamicProcessData");
                    Message = (string)t.InvokeMember("GetResult", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public, null, o, para);
                }
                return true;
            }

            foreach (CompilerError error in result.Errors)
            {  // 列出编译错误  
                if (error.IsWarning) continue;
                ErrorMessage.Add("Error(" + error.ErrorNumber + ") - " + error.ErrorText + "\t\tLine:" + error.Line.ToString() + "  Column:" + error.Column.ToString());
            }
            return false;
        }

    }

}
