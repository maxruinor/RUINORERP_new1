using System;

namespace DevAge.ComponentModel.Converter
{
    /// <summary>
    /// A TypeConverter that support string conversion from and to string with the currency symbol.
    /// Support Conversion for Float, Double and Decimal, Int
    /// </summary>
    public class CurrencyTypeConverter : NumberTypeConverter
    {
        /// <summary>
        /// 是否自动补零 如果设置了四位小数精度， 0.2600。如果否则 0.26.使用"0.00##". 保留两位
        /// </summary>
        public bool AutoAddZero { get; set; } = false;

        /// <summary>
        /// 货币格式
        /// </summary>
        public string CurrencyFormat { get; set; }


        #region Constructors
        public CurrencyTypeConverter(Type p_BaseType) : base(p_BaseType)
        {
            Format = "C";
            NumberStyles = System.Globalization.NumberStyles.Currency;
        }

        public CurrencyTypeConverter(Type p_BaseType,
            string p_Format) : this(p_BaseType)
        {
            Format = p_Format;
        }
        #endregion
    }
}
