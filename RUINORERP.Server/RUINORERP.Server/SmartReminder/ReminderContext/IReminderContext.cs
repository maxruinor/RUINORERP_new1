using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.ReminderContext
{
    public interface IReminderContext
    {
        Type DataType { get; }
        object GetData();
    }
}
