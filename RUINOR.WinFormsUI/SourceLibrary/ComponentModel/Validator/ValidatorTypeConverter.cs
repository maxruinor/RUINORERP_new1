using System;

namespace SourceLibrary.ComponentModel.Validator
{
    /// <summary>
    /// A string editor that use a TypeConverter for conversion.
    /// </summary>
    public class ValidatorTypeConverter : ValidatorBase
    {
        #region Constructor
        /// <summary>
        /// Constructor. If the Type doesn't implements a TypeConverter no conversion is made.
        /// </summary>
        /// <param name="p_Type"></param>
        public ValidatorTypeConverter(Type p_Type) : this(p_Type, System.ComponentModel.TypeDescriptor.GetConverter(p_Type))
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Type">Cannot be null.</param>
        /// <param name="p_TypeConverter">Can be null to don't allow any conversion.</param>
        public ValidatorTypeConverter(Type p_Type, System.ComponentModel.TypeConverter p_TypeConverter) : base(p_Type)
        {
            m_TypeConverter = p_TypeConverter;

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
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConvertingObjectToValue(ConvertingObjectEventArgs e)
        {
            if (m_ConvertingObjectToValue != null)
                m_ConvertingObjectToValue(this, e);
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
                if (IsNullString(tmp))
                    e.Value = null;
                else if (e.DestinationType.IsAssignableFrom(e.Value.GetType())) //se la stringa non ?nulla e il tipo di destinazione ?sempre una string allora non faccio nessuna conversione
                {
                }
                else if (IsStringConversionSupported())
                    e.Value = m_TypeConverter.ConvertFromString(null, CultureInfo, tmp);
                else
                    throw new ApplicationException("String conversion not supported for this type of Validator.");
            }
            else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
            {
            }
            else if (m_TypeConverter != null)
            {
                try
                {
                    e.Value = m_TypeConverter.ConvertFrom(null, CultureInfo, e.Value);
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                try
                {
                    //e.Value = cenetcom.util.refutil.tools.ConvertTo(e.Value, e.DestinationType.FullName);
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);



                }
            }
        }
        /// <summary>
        /// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConvertingValueToObject(ConvertingObjectEventArgs e)
        {
            if (m_ConvertingValueToObject != null)
                m_ConvertingValueToObject(this, e);
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
                throw new ApplicationException("String conversion not supported for this type of Validator.");
            }
            else if (m_TypeConverter != null)
            {
                e.Value = m_TypeConverter.ConvertTo(null, CultureInfo, e.Value, e.DestinationType);
            }
        }
        #endregion

        #region Type
        private System.ComponentModel.TypeConverter m_TypeConverter;
        /// <summary>
        /// TypeConverter used for this type editor, cannot be null.
        /// </summary>
        public System.ComponentModel.TypeConverter TypeConverter
        {
            get { return m_TypeConverter; }
            set { m_TypeConverter = value; }
        }
        #endregion
    }
}
