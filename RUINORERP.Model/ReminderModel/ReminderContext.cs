using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel
{
    /// <summary>
    /// 要提醒的内容
    /// </summary>
    public interface IReminderContext
    {
        Type DataType { get; }
        object GetData();
    }


}
