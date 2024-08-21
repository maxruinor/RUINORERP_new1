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
        /// �Ƿ��Զ����� �����������λС�����ȣ� 0.2600��������� 0.26.ʹ��"0.00##". ������λ
        /// </summary>
        public bool AutoAddZero { get; set; } = false;

        /// <summary>
        /// ���Ҹ�ʽ
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
