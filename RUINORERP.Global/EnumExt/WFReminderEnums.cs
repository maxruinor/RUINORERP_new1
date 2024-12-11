using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{

    /// <summary>
    /// 提醒记时器要处理的步骤
    /// </summary>
    public enum EnumProcessRemind
    {

        //确认 = 1, //：表示接收者已经知晓并确认提醒内容。
        //完成,//：表示与提醒相关的任务或事项已经完成。
        //忽略,//：接收者选择暂时不理会该提醒。
        稍后,//：希望在未来的某个时间再次收到相同的提醒。 5分钟，10分钟，30分钟
        转发,//：将提醒转发给其他相关人员。
        停止,
    }
}
