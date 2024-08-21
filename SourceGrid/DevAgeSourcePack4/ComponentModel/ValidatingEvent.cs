using System;
using System.ComponentModel;
using System.Globalization;

namespace DevAge.ComponentModel
{
    public class ConvertingObjectEventArgs : System.EventArgs
    {
        /*
        //by watson BY DOTO 添加了域区值，因为设置时editor的属性传不过来
        #region Culture

        private System.Globalization.CultureInfo m_CultureInfo = null;

        /// <summary>
        /// Culture for conversion. If null the default user culture is used. Default is null.
        /// </summary>
        [DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Globalization.CultureInfo CultureInfo
        {
            get { return m_CultureInfo; }
            set
            {
                if (m_CultureInfo != value)
                {
                    m_CultureInfo = value;
                }
            }
        }
        #endregion
        */

        //by watson BY DOTO 添加了tag值，因为显示值不是ID

        private object mTag_Value;
        public object TagValue
        {
            get { return mTag_Value; }
            set { mTag_Value = value; }
        }

        private object m_Value;
        private Type m_DestinationType;
        private ConvertingStatus m_ConvertingStatus = ConvertingStatus.Converting;
        public ConvertingObjectEventArgs(object p_Value, Type p_DestinationType)
        {
            m_Value = p_Value;
            m_DestinationType = p_DestinationType;
        }

        public ConvertingObjectEventArgs(object p_Value, object tag_value, Type p_DestinationType)
        {
            mTag_Value = tag_value;
            m_Value = p_Value;
            m_DestinationType = p_DestinationType;
        }
 

        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        /// <summary>
        /// Destination type to convert the Value. Can be null if no destination type is required.
        /// </summary>
        public Type DestinationType
        {
            get { return m_DestinationType; }
        }
        public ConvertingStatus ConvertingStatus
        {
            get { return m_ConvertingStatus; }
            set { m_ConvertingStatus = value; }
        }
    }
    public delegate void ConvertingObjectEventHandler(object sender, ConvertingObjectEventArgs e);

    public enum ConvertingStatus
    {
        Converting = 0,
        Error = 1,
        Completed = 2
    }

    public class ValueEventArgs : System.EventArgs
    {
        private object m_Value;
        public ValueEventArgs(object p_Value)
        {
            m_Value = p_Value;
        }
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

    }
    public delegate void ValueEventHandler(object sender, ValueEventArgs e);

}
