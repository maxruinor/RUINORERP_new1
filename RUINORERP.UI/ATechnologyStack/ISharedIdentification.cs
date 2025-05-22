using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ATechnologyStack
{
    /// <summary>
    /// 窗体模块合且时 实际窗体分开时的标识
    /// </summary>
    public interface ISharedIdentification
    {
        public SharedFlag sharedFlag { get; set; }
    }

    /// <summary>
    /// 共用窗体时一般也就2，3个。用一个枚举来标识。子窗体不重复使用其中的标识值即可
    /// 以业务表名，接口名，加这个枚举作为标识
    /// 比方 付款单  付款单查询。 在查询基类中传入查询时的菜单信息，SharedFlag这个值 在 付款单和付款单查询 中都固定为一样的值。双击时再按这个值去查询对应的窗体菜单
    /// 实际也可以通过 菜单名称来判断
    /// </summary>
    public enum SharedFlag
    {
        /// <summary>
        /// 财务模块中 用于收款类型
        /// </summary>
        [Description("收款")]
        Flag1 = 1,

        /// <summary>
        /// 财务模块中 用于付款类型
        /// </summary>
        [Description("付款")]
        Flag2 = 2,


        Flag3 = 3,

        Flag4 = 4,

        Flag5 = 5,

        Flag6 = 6,
    }


}
