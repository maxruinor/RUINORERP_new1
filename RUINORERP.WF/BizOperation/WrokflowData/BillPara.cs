using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation.WrokflowData
{
    public class BillPara : BaseBizData, IWorkflowDataMarker
    {
        public BillPara()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            var assemblyTitle = currentAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            string title = assemblyTitle.Length > 0 ? ((AssemblyTitleAttribute)assemblyTitle[0]).Title : null;

            DataType = $"{this.GetType().FullName},{title}";
            TypeName = "单据流程";
        }
        private string _datatype;

        public string DataType
        {
            get
            {
                return _datatype;
            }
            set
            {
                _datatype = value;
            }
        }

        private string _TypeName;

        public string TypeName { get => _TypeName; set => _TypeName = value; }



        /*
         这里写需要传递的参数,用于工作流启动时传入
         */
        //public int Value1 { get; set; }
        //public int Value2 { get; set; }
        //public int Answer { get; set; }

            

    }
}
