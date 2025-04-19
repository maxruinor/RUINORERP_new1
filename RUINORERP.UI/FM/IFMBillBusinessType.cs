using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.UI.FM
{
    /// <summary>
    /// 财务模块合且时 实际窗体分开时的标识
    /// </summary>
    public interface IFMBillBusinessType
    {
        ReceivePaymentType PaymentType { get; }
    }


    
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class BillBusinessTypeRequiredAttribute : Attribute { }


    //////// 在基类添加特性（可选）  这样特性标记就可以在编译时检测到一定要实际这个接口
    //////[BillBusinessTypeRequired]
    //////public abstract class BaseBillQuery : UserControl { }
}
