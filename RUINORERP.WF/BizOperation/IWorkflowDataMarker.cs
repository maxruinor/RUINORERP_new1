using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation
{
    public interface IWorkflowDataMarker
    {

        string TypeName { get; set; }

        /// <summary>
        /// 数据类型：用于json中指定的类型，
        /// 值为本身路径
        /// </summary>
        string DataType { get; set; }
    }
}
