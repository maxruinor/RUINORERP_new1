using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using SourceGrid.Utils;

namespace DevAge.ComponentModel.Validator
{
    /// <summary>
    /// A string editor that use a TypeConverter for conversion.
    /// </summary>
    public class ValidatorTypeConverter : ValidatorBase
    {
        #region Constructor
        /// <summary>
        /// Constructor. Initialize the Validator with a null TypeConverter.
        /// </summary>
        public ValidatorTypeConverter()
        {
            m_TypeConverter = null;
        }

        /// <summary>
        /// Constructor. If the Type doesn't implements a TypeConverter no conversion is made.
        /// </summary>
        /// <param name="p_Type"></param>
        public ValidatorTypeConverter(Type p_Type) : base(p_Type)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Type">Cannot be null.</param>
        /// <param name="p_TypeConverter">Can be null to don't allow any conversion.</param>
        public ValidatorTypeConverter(Type p_Type, System.ComponentModel.TypeConverter p_TypeConverter) : base(p_Type)
        {
            TypeConverter = p_TypeConverter;
        }
        #endregion

        #region Conversion
        /// <summary>
        /// Returns true if string conversion is suported. AllowStringConversion must be true and the current Validator must support string conversion.
        /// </summary>
        public override bool IsStringConversionSupported()
        {
            if (typeof(string).IsAssignableFrom(ValueType))
                return AllowStringConversion;

            if (m_TypeConverter != null)
                return AllowStringConversion && m_TypeConverter.CanConvertFrom(typeof(string)) && m_TypeConverter.CanConvertTo(typeof(string));
            else
                return base.AllowStringConversion;
        }

        /// <summary>
        /// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
        /// ������ת��Ϊָ��ֵʱ�������ӷ���ObjectToValue��IsValidObject����
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConvertingObjectToValue(ConvertingObjectEventArgs e)
        {
            base.OnConvertingObjectToValue(e);

            if (e.ConvertingStatus == ConvertingStatus.Error)
                throw new ApplicationException("Invalid conversion");
            else if (e.ConvertingStatus == ConvertingStatus.Completed)
                return;

            if (e.Value == null)
            {
            }
            else if (e.Value is string) //?importante fare prima il caso stringa per gestire correttamente il null
            {
                string tmp = (string)e.Value;
                if (IsNullString(tmp) && e.DestinationType.IsAssignableFrom(typeof(string)))
                {
                    e.Value = "";//TODO: by watson ע�� ��������Ӧ�÷���null��������Ϊstring���Ͳ�֧��null���������ﷵ�ؿ��ַ�����
                }
                else if (IsNullString(tmp) && !e.DestinationType.IsAssignableFrom(e.Value.GetType()))
                {
                    #region ��������ת��Ϊ���ַ�����ֵ����ת��ΪĬ��ֵ��ʵ����ȫ��ת��Ϊnull�ˡ�Ҫ���ж�ֵ���ͣ����ж��������ͲŶԡ�
                    // ���Խ��������͵���������Ϊ null
                    if (e.Value.GetType().IsClass)
                    {
                        e.Value = null;
                    }
                    // ֵ���͵����Բ�������Ϊ null��������Խ������⴦����������ΪĬ��ֵ
                    else if (e.Value.GetType().IsValueType)
                    {
                        // ����ΪĬ��ֵ
                        e.Value = Activator.CreateInstance(e.Value.GetType());
                    }
                    #endregion
                }
                else if (e.DestinationType.IsAssignableFrom(e.Value.GetType())) //���Ŀ��������ֵ������ͬ����ֱ�ӷ���ֵ
                {


                }
                else if (IsStringConversionSupported())
                {
                    //try
                    //{
                    e.Value = m_TypeConverter.ConvertFromString(EmptyTypeDescriptorContext.Empty, CultureInfo, tmp);
                    //}
                    //catch (Exception ex)
                    //{

                    //}

                }
                else
                    throw new ApplicationException("�����͵���֤����֧���ַ���ת����");
            }
            else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
            {
                //by watson TODO

            }
            else if (m_TypeConverter != null)
            {
                // For some reason string converter does not allow converting from
                // double to string. So here is just override with simple if statemenet
                //����ĳ��ԭ���ַ���ת�����������
                //doubleת��Ϊstring����������ֻ���ü򵥵�if statemenet��д
                if (m_TypeConverter is StringConverter)
                    e.Value = SourceGridConvert.To<string>(e.Value);
                else
                // ����ֻ���������ת��
                {
                    e.Value = m_TypeConverter.ConvertFrom(EmptyTypeDescriptorContext.Empty, CultureInfo, e.Value);
                }

            }
        }
        /// <summary>
        /// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConvertingValueToObject(ConvertingObjectEventArgs e)
        {
            base.OnConvertingValueToObject(e);

            if (e.ConvertingStatus == ConvertingStatus.Error)
                throw new ApplicationException("Invalid conversion");
            else if (e.ConvertingStatus == ConvertingStatus.Completed)
                return;

            if (e.Value == null)
            {
            }
            else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
            {
            }
            else if (e.DestinationType == typeof(string) && IsStringConversionSupported() == false)
            {
                throw new ApplicationException("�����͵���֤����֧���ַ���ת����");
            }
            else if (m_TypeConverter != null)
            {

                if (m_TypeConverter.GetType().Name == "CurrencyTypeConverter")
                {
                    //С��λ
                    int DataPrecision = 0;
                    string fat = "#0.##########";//Ĭ�ϸ�10λ
                    decimal tempMoney = decimal.Parse(e.Value.ToString());
                    string tmpValue = tempMoney.ToString(fat);
                    string numStr = tmpValue.ToString();
                    // �ж������Ƿ���С��
                    if (numStr.Contains("."))
                    {
                        int x = numStr.IndexOf(".") + 1;
                        int y = numStr.Length - x;
                        // ����С�����ĸ���
                        DataPrecision = y;
                    }

                    ComponentModel.Converter.CurrencyTypeConverter ctc = m_TypeConverter as ComponentModel.Converter.CurrencyTypeConverter;
                    if (ctc.AutoAddZero)
                    {
                        e.Value = m_TypeConverter.ConvertTo(EmptyTypeDescriptorContext.Empty, CultureInfo, e.Value, e.DestinationType);
                    }
                    else
                    {
                        if (DataPrecision <= CultureInfo.NumberFormat.CurrencyDecimalDigits)
                        {
                            fat = "{0:C" + DataPrecision + "}";
                            e.Value = string.Format(fat, tempMoney);
                        }
                        else
                        {
                            e.Value = m_TypeConverter.ConvertTo(EmptyTypeDescriptorContext.Empty, CultureInfo, e.Value, e.DestinationType);
                        }

                    }

                }
                else
                {
                    e.Value = m_TypeConverter.ConvertTo(EmptyTypeDescriptorContext.Empty, CultureInfo, e.Value, e.DestinationType);
                }

            }
        }
        #endregion

        #region TypeConverter

        private System.ComponentModel.TypeConverter m_TypeConverter;
        /// <summary>
        /// TypeConverter used for this type editor, cannot be null.
        /// </summary>
        [DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.ComponentModel.TypeConverter TypeConverter
        {
            get { return m_TypeConverter; }
            set
            {
                if (m_TypeConverter != value)
                {
                    m_TypeConverter = value;
                    OnLoadingTypeConverter();
                    OnChanged(EventArgs.Empty);
                }
            }
        }

        private void OnLoadingTypeConverter()
        {
            StandardValues = null;
            StandardValuesExclusive = false;

            //Populate properties using TypeConverter
            if (m_TypeConverter != null)
            {
                StandardValues = m_TypeConverter.GetStandardValues();
                if (StandardValues != null && StandardValues.Count > 0)
                    StandardValuesExclusive = m_TypeConverter.GetStandardValuesExclusive();
                else
                    StandardValuesExclusive = false;
            }
        }

        protected override void OnLoadingValueType()
        {
            base.OnLoadingValueType();

            if (ValueType != null)
                TypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(ValueType);
            else
                TypeConverter = null;
        }
        #endregion
    }
}
