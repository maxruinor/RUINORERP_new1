using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{

    [Serializable]
    public class UCJson路径提取Para : UCBasePara
    {
        private bool _isJson格式化 = false;
        public bool isJson格式化
        {
            get { return _isJson格式化; }
            set { _isJson格式化 = value; }
        }

        private string _jsonPath = string.Empty;



        /// <summary>
        /// path格式为  aa.bb.cc;  提取时是  obj[aa][bb][cc].tostring()
        /// </summary>
        public string jsonPath
        {
            get { return _jsonPath; }
            set { _jsonPath = value; }
        }



        public override string ProcessDo(string StrIn)
        {
            string html = StrIn;
            #region json提取操作
            try
            {
                //rs为参数输入 输出
                string[] sz = jsonPath.Split('.');
                if (isJson格式化)
                {
                    html = HLH.Lib.Helper.JsonHelper.ConvertJsonString(html);
                }
                var root = JToken.Parse(html);
                for (int i = 0; i < sz.Length; i++)
                {
                    if (i == sz.Length - 1)
                    {
                        if (root[sz[i]] == null)
                        {
                            html = "";
                        }
                        else
                        {
                            html = root[sz[i]].ToString();
                        }

                    }
                    else
                    {
                        if (root[sz[i]] != null)
                        {
                            root = Newtonsoft.Json.Linq.JObject.Parse(root[sz[i]].ToString());
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                PrintDebugInfo("【" + jsonPath + "】Json提取没有找到对应值。 " + "-" + ex.Message);
                //需要有调试机制
                //if (NotifyExternalEvent != null)
                //{
                //    NotifyExternalEvent("【" + df.FieldName + "|" + cd.jsonPath + "】Json提取没有找到对应值。 " + "-" + ex.Message);
                //}
            }


            #endregion

            return html;
        }

    }
}
