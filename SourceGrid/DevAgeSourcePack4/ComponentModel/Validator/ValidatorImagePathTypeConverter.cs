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
        /// 将对象转换为指定值时触发。从方法ObjectToValue和IsValidObject调用
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
                    e.Value = "";//TODO: by watson 注意 本来这里应该返回null，但是因为string类型不支持null，所以这里返回空字符串。
                }
                else if (IsNullString(tmp) && !e.DestinationType.IsAssignableFrom(e.Value.GetType()))
                {
                    #region 引用类型转换为空字符串，值类型转换为默认值，实际是全部转换为null了。要先判断值类型，再判断引用类型才对。
                    // 尝试将引用类型的属性设置为 null
                    if (e.Value.GetType().IsClass)
                    {
                        e.Value = null;
                    }
                    // 值类型的属性不能设置为 null，这里可以进行特殊处理，例如重置为默认值
                    else if (e.Value.GetType().IsValueType)
                    {
                        // 重置为默认值
                        e.Value = Activator.CreateInstance(e.Value.GetType());
                    }
                    #endregion
                }
                else if (e.DestinationType.IsAssignableFrom(e.Value.GetType())) //如果目标类型与值类型相同，则直接返回值
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
                    throw new ApplicationException("此类型的验证器不支持字符串转换。");
            }
            else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
            {
                //by watson TODO

            }
            else if (m_TypeConverter != null)
            {
                // For some reason string converter does not allow converting from
                // double to string. So here is just override with simple if statemenet
                //出于某种原因，字符串转换器不允许从
                //double转换为string。所以这里只是用简单的if statemenet重写
                if (m_TypeConverter is StringConverter)
                    e.Value = SourceGridConvert.To<string>(e.Value);
                else
                // 否则，只需进行正常转换
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
                throw new ApplicationException("此类型的验证器不支持字符串转换。");
            }
            else if (m_TypeConverter != null)
            {

                if (m_TypeConverter.GetType().Name == "CurrencyTypeConverter")
                {
                    //小数位
                    int DataPrecision = 0;
                    string fat = "#0.##########";//默认给10位
                    decimal tempMoney = decimal.Parse(e.Value.ToString());
                    string tmpValue = tempMoney.ToString(fat);
                    string numStr = tmpValue.ToString();
                    // 判断数字是否有小数
                    if (numStr.Contains("."))
                    {
                        int x = numStr.IndexOf(".") + 1;
                        int y = numStr.Length - x;
                        // 返回小数点后的个数
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
