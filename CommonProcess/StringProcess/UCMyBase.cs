using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CommonProcess.StringProcess
{

    [Serializable]
    public partial class UCMyBase : UserControl, IUCBase
    {
        public UCMyBase()
        {
            InitializeComponent();
        }


        //公共属性GUID

        // private string _GUID = string.Empty;

        /// <summary>
        /// ActionID
        /// </summary>
        // public string GUID { get => _GUID; set => _GUID = value; }


        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void PrintInfoHandler(string message, Color color);

        /// <summary>
        /// 引发外部显示提示信息事件
        /// </summary>
        [Browsable(true), Description("引发外部显示提示信息事件")]
        public event PrintInfoHandler PrintInfoEvent;



        public object OtherEventParameters { get; set; }

        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void OtherHandler(UCHTMLStructuralAnalysis uc, object Parameters);


        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;




        /// <summary>
        /// 显示提示信息的方法
        /// </summary>
        /// <param name="msg"></param>
        public void printInfoMessage(string msg, Color color)
        {
            if (PrintInfoEvent != null)
            {
                PrintInfoEvent(msg, color);
            }
        }

        /// <summary>
        /// 显示提示信息的方法
        /// </summary>
        /// <param name="msg"></param>
        public void printInfoMessage(string msg)
        {
            if (PrintInfoEvent != null)
            {
                PrintInfoEvent(msg, Color.Black);
            }
        }

        void IUCBase.SaveDataFromUI(UCBasePara bb)
        {
             
        }

        void IUCBase.LoadDataToUI(UCBasePara bb)
        {
            
        }
    }
}
