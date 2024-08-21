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
    /// 作为处理过程
    /// </summary>
    [Serializable]
    public class UCBasePara
    {
        private ProcessAction _action = new ProcessAction();

        private string _Input = string.Empty;
        private string _Output = string.Empty;


        private bool available = false;
        public bool Available
        {
            get { return available; }
            set { available = value; }
        }

        //公共属性GUID

        private string _GUID = string.Empty;
        public string GUID { get => _GUID; set => _GUID = value; }
        public string Input { get => _Input; set => _Input = value; }
        public string Output { get => _Output; set => _Output = value; }
        public ProcessAction Action { get => _action; set => _action = value; }


        public virtual string ProcessDo(string strIn)
        {
            return "请在子类中实现这个方法！";
        }


        protected virtual void OnDebugTacker(UCBasePara ucPara, object Para)
        {
            if (this.DebugTrackerEvent != null)
            {
                DebugTrackerEvent(ucPara, Para.ToString());
            }
        }
        protected virtual void OnDebugTacker(object Para)
        {
            if (this.DebugTrackerEvent != null)
            {
                DebugTrackerEvent(this, Para.ToString());
            }
        }

        //==========

        public void PrintDebugInfo(string info, Exception ex)
        {
            if (this.DebugTrackerEvent != null)
            {
                DebugTrackerEvent(this, ex);
            }
        }

        public void PrintDebugInfo(string info)
        {
            if (this.DebugTrackerEvent != null)
            {
                DebugTrackerEvent(this, info.ToString());
            }
        }






        /// <summary>
        /// 针对不同的动作的外部事件,处理这些都认为有输入输出
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate string BeforeOnProcessHandler(UCBasePara ucPara, object Parameters);

        /// <summary>
        /// 在内部主事件之前执行，针对不同的动作的外部事件，可以对参数不同做不同处理，
        /// 但是 如果 加载两个下载控件时。是否能自由处理不同事情 。还需要优化
        /// </summary>
        [Browsable(true), Description("在内部主事件之前执行")]
        public event BeforeOnProcessHandler BeforeOnProcessEvent;


        /// <summary>
        /// 针对不同的动作的外部事件,处理这些都认为有输入输出
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate string AfterOnProcessHandler(UCBasePara ucPara, object Parameters);

        /// <summary>
        /// 在内部主事件后执行，针对不同的动作的外部事件，可以对参数不同做不同处理，
        /// 但是 如果 加载两个下载控件时。是否能自由处理不同事情 。还需要优化
        /// </summary>
        [Browsable(true), Description("在内部主事件后执行")]
        public event AfterOnProcessHandler AfterOnProcessEvent;

        /// <summary>
        /// 针对不同的动作的外部事件,处理这些都认为有输入输出
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void DebugTrackerHandler(UCBasePara ucPara, object Parameters);

        /// <summary>
        /// 调试跟踪
        /// </summary>
        public event DebugTrackerHandler DebugTrackerEvent;

        /*
        public string BatchProcess(List<KeyValue> Actions, string strIn)
        {
            foreach (KeyValue kv in Actions)
            {
                UCBasePara para = kv.Value as UCBasePara;
                if (para.Available)
                {
                    if (BeforeOnProcessEvent != null)
                    {
                        ProcessAction pa = (ProcessAction)Enum.Parse(typeof(ProcessAction), kv.Key.ToString());
                        strIn = BeforeOnProcessEvent(pa, strIn);
                    }

                    strIn = para.ProcessDo(strIn);

                    if (AfterOnProcessEvent != null)
                    {
                        ProcessAction pa = (ProcessAction)Enum.Parse(typeof(ProcessAction), kv.Key.ToString());
                        strIn = AfterOnProcessEvent(pa, strIn);
                    }
                }
            }
            return strIn;
        }
        */
        public string BatchProcess(List<KeyValue> Actions, string strIn)
        {
            foreach (KeyValue kv in Actions)
            {
                UCBasePara para = kv.Value as UCBasePara;
                if (para.Available)
                {
                    #region 处理
                    if (BeforeOnProcessEvent != null)
                    {
                        //ProcessAction pa = (ProcessAction)Enum.Parse(typeof(ProcessAction), kv.Key.ToString());
                        strIn = BeforeOnProcessEvent(para, strIn);
                    }

                    para.DebugTrackerEvent += Para_DebugTrackerEvent;
                    strIn = para.ProcessDo(strIn);
                    para.DebugTrackerEvent -= Para_DebugTrackerEvent;
                    if (AfterOnProcessEvent != null)
                    {
                        //ProcessAction pa = (ProcessAction)Enum.Parse(typeof(ProcessAction), kv.Key.ToString());
                        strIn = AfterOnProcessEvent(para, strIn);
                    }
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
            return strIn;
        }
 
        public void Para_DebugTrackerEvent(UCBasePara ucPara, object Parameters)
        {
            if (DebugTrackerEvent != null)
            {
                DebugTrackerEvent(ucPara, Parameters);
            }
        }




        public void AddUCToUI(System.Windows.Forms.Panel panel4UC, ProcessAction spa)
        {
            AddUCToUI(panel4UC, spa, null);
        }

        public void AddUCToUI(System.Windows.Forms.Panel panel4UC, ProcessAction spa, UCBasePara Para)
        {
            if (Para != null)
            {
                Para.Action = spa;
            }
            panel4UC.Controls.Clear();
            switch (spa)
            {

                case ProcessAction.字符修补替换移除:
                    UCRepairString ucr = new UCRepairString();
                    if (Para != null)
                    {
                        ucr.LoadDataToUI(Para);
                    }
                    panel4UC.Controls.Add(ucr);
                    break;

                case ProcessAction.检测替换特殊字符:

                    UCFindSpecialChar ucfinder = new UCFindSpecialChar();
                    panel4UC.Controls.Add(ucfinder);
                    break;


                case ProcessAction.智能打包处理:
                    //实质就是对数值类型的数据计算
                    UCSmartPackaging ucsp = new UCSmartPackaging();
                    panel4UC.Controls.Add(ucsp);
                    break;
                case ProcessAction.自动补全标签:
                    //没有界面显示，下面的方法中已经处理
                    break;


                case ProcessAction.HTML结构分析式处理:
                    //case StringProcessAction.HTML结构分析式处理:
                    //rs = SmartProcessDescription(SourceString);
                    //  break;
                    UCHTMLStructuralAnalysis uchtml = new UCHTMLStructuralAnalysis();
                    // uchtml.OtherEvent += uchtml_OtherEvent;
                    panel4UC.Controls.Add(uchtml);
                    break;
                case ProcessAction.分割数组提取:
                    UC数组分割提取 ucfg = new UC数组分割提取();
                    //ucfg.OtherEvent += uchtml_OtherEvent;
                    if (Para != null)
                    {
                        ucfg.LoadDataToUI(Para);
                    }
                    panel4UC.Controls.Add(ucfg);
                    break;
                case ProcessAction.Json属性路径提取:
                    UCJson路径提取Find ucjson = new UCJson路径提取Find();
                    UCJson路径提取Para ucs = new UCJson路径提取Para();
                    if (Para != null)
                    {
                        ucs = Para as UCJson路径提取Para;
                        ucjson.LoadDataToUI(ucs);
                    }
                    //ucjson.OtherEvent += uchtml_OtherEvent;
                    panel4UC.Controls.Add(ucjson);
                    break;
                case ProcessAction.正则式提取:
                    UC正则式提取 uce = new UC正则式提取();
                    if (Para != null)
                    {
                        uce.LoadDataToUI(Para);
                    }
                    panel4UC.Controls.Add(uce);
                    break;
                case ProcessAction.下载:
                    UCDownloadFile ucd = new UCDownloadFile();
                    if (Para != null)
                    {
                        ucd.LoadDataToUI(Para);
                    }
                    panel4UC.Controls.Add(ucd);
                    break;
                case ProcessAction.Xpath提取内容:
                    UCXpathPick ucx = new UCXpathPick();
                    if (Para != null)
                    {
                        ucx.LoadDataToUI(Para);
                    }
                    panel4UC.Controls.Add(ucx);
                    break;

                case ProcessAction.移除HTML标签:
                    UCHtmltagProcess uch = new UCHtmltagProcess();
                    if (Para != null)
                    {
                        uch.LoadDataToUI(Para);
                    }
                    panel4UC.Controls.Add(uch);
                    break;
                case ProcessAction.动态程序代码处理:
                    UC动态程序代码处理 ucdt = new UC动态程序代码处理();
                    if (Para != null)
                    {
                        ucdt.LoadDataToUI(Para);
                    }
                    panel4UC.Controls.Add(ucdt);
                    break;
                default:
                    break;
            }
        }



        public void AddActions(System.Windows.Forms.Panel panel4UC, List<KeyValue> Actions, ProcessAction spa)
        {
            UpdateActions(panel4UC, Actions, spa, null);
        }

        public void UpdateActions(System.Windows.Forms.Panel panel4UC, List<KeyValue> Actions, ProcessAction spa, UCBasePara para)
        {
            IUCBase iuc = GetMyUC(panel4UC, spa);

            KeyValue kv2 = new KeyValue();
            if (para == null)
            {
                kv2 = new KeyValue();
                para = GetActionParaFactory(spa);
                para.GUID = Guid.NewGuid().ToString("N");
                //添加默认可用
                para.Available = true;
                iuc.SaveDataFromUI(para);
                kv2.Tag = para.GUID;
                kv2.Value = para;
                kv2.Key = spa.ToString();
                Actions.Add(kv2);
            }
            else
            {
                kv2 = Actions.Find(delegate (KeyValue d) { return d.Tag.ToString() == para.GUID; });
                if (kv2 != null)
                {
                    iuc.SaveDataFromUI(kv2.Value as UCBasePara);
                    //只需要更新值
                    kv2.Value = kv2.Value;
                }
            }
        }


        private IUCBase GetMyUC(System.Windows.Forms.Panel panel4UC, ProcessAction spa)
        {

            IUCBase iuc = panel4UC.Controls[0] as UCMyBase;
            switch (spa)
            {
                case ProcessAction.移除HTML标签:
                    iuc = panel4UC.Controls[0] as UCHtmltagProcess;
                    break;
                case ProcessAction.字符修补替换移除:
                    iuc = panel4UC.Controls[0] as UCRepairString;
                    break;
                case ProcessAction.正则式提取:
                    iuc = panel4UC.Controls[0] as UC正则式提取;
                    break;
                case ProcessAction.设置指定字段值:
                    break;
                case ProcessAction.检测替换特殊字符:
                    break;
                case ProcessAction.自动补全标签:
                    break;
                case ProcessAction.HTML结构分析式处理:
                    break;
                case ProcessAction.智能打包处理:
                    break;
                case ProcessAction.分割数组提取:
                    iuc = panel4UC.Controls[0] as UC数组分割提取;
                    break;
                case ProcessAction.Json属性路径提取:
                    iuc = panel4UC.Controls[0] as UCJson路径提取Find;
                    break;
                case ProcessAction.下载:
                    iuc = panel4UC.Controls[0] as UCDownloadFile;
                    break;
                case ProcessAction.Xpath提取内容:
                    iuc = panel4UC.Controls[0] as UCXpathPick;
                    break;
                case ProcessAction.动态程序代码处理:
                    iuc = panel4UC.Controls[0] as UC动态程序代码处理;
                    break;
                default:
                    iuc = panel4UC.Controls[0] as UCMyBase;
                    break;
            }

            return iuc;
        }

        private UCBasePara GetActionParaFactory(ProcessAction spa)
        {
            UCBasePara ipara = null;
            switch (spa)
            {
                case ProcessAction.移除HTML标签:
                    ipara = new UCHtmltagProcessPara();
                    break;
                case ProcessAction.字符修补替换移除:
                    ipara = new UCRepairStringPara();
                    break;
                case ProcessAction.正则式提取:
                    ipara = new UC正则式提取Para();
                    break;
                case ProcessAction.设置指定字段值:
                    break;
                case ProcessAction.检测替换特殊字符:
                    break;
                case ProcessAction.自动补全标签:
                    break;
                case ProcessAction.HTML结构分析式处理:
                    break;
                case ProcessAction.智能打包处理:
                    break;
                case ProcessAction.分割数组提取:
                    ipara = new UC数组分割提取Para();
                    break;
                case ProcessAction.Json属性路径提取:
                    ipara = new UCJson路径提取Para();
                    break;
                case ProcessAction.下载:
                    ipara = new UCDownloadFilePara();
                    break;
                case ProcessAction.Xpath提取内容:
                    ipara = new UCXpathPickPara();
                    break;
                case ProcessAction.动态程序代码处理:
                    ipara = new UC动态程序代码处理Para();
                    break;
                default:
                    throw new Exception("请实现对应的工厂参数");
                    break;
            }
            ipara.Action = spa;
            return ipara;
        }




        //===
    }
}
