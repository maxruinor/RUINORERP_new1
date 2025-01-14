using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{
    /// <summary>
    /// 所有自动生成的查询条件都在这里能找到对应的类型，后面会根据类型进行处理
    /// 注意这个条件，只是中间使用 但是其中的时间区间值用来排序所以最到最后
    /// 将好定义的放前面。太长的放后面？
    /// </summary>
    [Serializable]
    public enum AdvQueryProcessType
    {
        /// <summary>
        /// 给一个默认值
        /// </summary>
        None,

        /// <summary>
        /// 文本模糊查询
        /// </summary>
        stringLike,

        /// <summary>
        /// 下拉默认值请选择
        /// </summary>
        defaultSelect,

        //枚举值下拉选择
        EnumSelect,


        TextSelect,

        /// <summary>
        /// 时间
        /// </summary>
        datetime,


        YesOrNo,

        /// <summary>
        /// 两者选一 可选择应用
        /// </summary>
        useYesOrNoToAll,

        /// <summary>
        /// 下拉多选可忽略，勾选生效
        /// </summary>
        CmbMultiChoiceCanIgnore,

        /// <summary>
        /// 下拉多选
        /// </summary>
        CmbMultiChoice,

        /// <summary>
        /// 时间区间段
        /// </summary>
        datetimeRange,
    }

    //数据类型
    //https://blog.csdn.net/weixin_61361738/article/details/128961196
    public enum EnumDataType
    {
        Boolean,
        Char,
        Single,
        Double,
        Decimal,
        SByte,
        Byte,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        IntPtr,
        UIntPtr,
        Object,
        String,
        DateTime
    }





}
