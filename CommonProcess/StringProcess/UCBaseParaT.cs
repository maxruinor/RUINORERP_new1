using HLH.Lib.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{

    /// <summary>
    /// 暂时没有弄好。还是用原始的 UCBasePara
    /// </summary>
    [Serializable]
    public class UCBaseParaT<T> : UCBasePara
    {
        public new T BatchProcess(List<KeyValue> Actions, string strIn)
        {
            foreach (KeyValue kv in Actions)
            {
                UCBasePara para = kv.Value as UCBasePara;
                if (para.Available)
                {
                    #region 处理
                    para.DebugTrackerEvent += Para_DebugTrackerEvent;
                    strIn = para.ProcessDo(strIn);
                    para.DebugTrackerEvent -= Para_DebugTrackerEvent;

                    #endregion
                    if (strIn.Trim().Length == 0)
                    {
                        OnDebugTacker(para.Action.ToString() + "  处理结果为空！");
                    }
                    else
                    {
                        if (strIn.Length > 30)
                        {
                            OnDebugTacker(para.Action.ToString() + "  处理结果:" + strIn.Substring(0, 20) + "...");
                        }
                        else
                        {
                            OnDebugTacker(para.Action.ToString() + "  处理结果:" + strIn + "...");
                        }

                    }
                }
            }
            T t = default(T);
            try
            {
                t = (T)(object)strIn;
            }
            catch (Exception)
            {

                t = default(T);
            }

            return t;
            //return (T)(strIn);
            //return default(T);
        }

        //===
    }
}
