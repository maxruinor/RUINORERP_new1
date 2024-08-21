using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{


    [Serializable]
    public class UC数组分割提取Para : UCBasePara
    {
        private int getIndex = 0;

        public string delimiter = string.Empty;


        public UC数组分割提取Para()
        {

        }




        /// <summary>
        /// 分隔符
        /// </summary>
        public string Delimiter
        {
            get { return delimiter; }
            set { delimiter = value; }
        }

        public int GetIndex { get => getIndex; set => getIndex = value; }

        public override string ProcessDo(string StrIn)
        {

            OnDebugTacker( "开始分割");
            string rs = string.Empty;
            string[] sz = StrIn.Split(new string[] { Delimiter }, StringSplitOptions.None);
            OnDebugTacker(this, "分割长度" + sz.Length);
            if (sz.Length > 0)
            {
                rs = sz[0];
                if (sz.Length > getIndex)
                {
                    rs = sz[getIndex];
                }
            }
            else
            {
                rs = StrIn;
            }
            return rs;
        }

    }
}
