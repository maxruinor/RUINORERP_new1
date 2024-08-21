using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevAge.ComponentModel.Validator
{
    /// <summary>
    ///  ����ValueMapping��������һ���޸Ķ���
    ///  The ValueMapping class can be used to easily map a value to a string value or a display string for conversion
    /// </summary>
    public class ComboxValueMapping
    {

        #region Null

        /// <summary>
        /// ����Ƿ������ֵ������
        /// </summary>
        [DefaultValue(true), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowNull { get; set; } = false;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ComboxValueMapping(bool allowNull)
        {
            AllowNull = allowNull;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="valueList">A list of valid value. If null an error occurred. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
        /// <param name="displayStringList">A list of displayString. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
        /// <param name="specialList">A list of object that can be converted to value. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
        /// <param name="specialType">The type of object stored in the specialList collection.</param>
        //public ComboxValueMapping(IValidator validator, System.Collections.IList valueList, System.Collections.IList displayStringList, System.Collections.IList specialList, Type specialType)
        //{
        //    ValueList = valueList;
        //    DisplayStringList = displayStringList;
        //    SpecialList = specialList;
        //    if (validator != null)
        //        BindValidator(validator);
        //}

        /// <summary>
        /// Bind the specified validator
        /// </summary>
        /// <param name="p_Validator"></param>
        public void BindValidator(IValidator p_Validator)
        {
            p_Validator.ConvertingValueToDisplayString += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
            p_Validator.ConvertingObjectToValue += new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
            p_Validator.ConvertingValueToObject += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
        }

        /// <summary>
        /// Unbind the specified validator
        /// </summary>
        /// <param name="p_Validator"></param>
        public void UnBindValidator(IValidator p_Validator)
        {
            p_Validator.ConvertingValueToDisplayString -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
            p_Validator.ConvertingObjectToValue -= new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
            p_Validator.ConvertingValueToObject -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
        }
        //private System.Collections.IList m_ValueList;

        private ConcurrentDictionary<string, string> m_ValueList;

        private Type mSpecialType = typeof(string);

        // private System.Collections.IList mSpecialList;

        private System.Collections.IList m_DisplayStringList;

        /// <summary>
        /// A list of valid value. If null an error occurred. The index must match the index of ValueList and DisplayStringList
        /// ��Чֵ���б����Ϊnull��������������������ValueList��DisplayStringList������ƥ��
        /// </summary>
        //public System.Collections.IList ValueList
        //{
        //    get { return m_ValueList; }
        //    set { m_ValueList = value; }
        //}

        public ConcurrentDictionary<string, string> ValueList
        {
            get { return m_ValueList; }
            set { m_ValueList = value; }
        }

        /// <summary>
        /// A list of object that can be converted to value. Can be null. The index must match the index of ValueList and DisplayStringList. Must be a list of object of the type specified in the SpecialType property.
        /// ����ת��Ϊֵ�Ķ����б�����Ϊnull������������ValueList��DisplayStringList������ƥ�䡣������SpecialType������ָ�����͵Ķ����б�
        /// ͨ������ִ���ض����͵�����ת��ʱ����ʹ�ô����ԡ����磬������뽫ö��ֵ��idֵӳ�䵽�ַ����Ի�ø��õ��û����顣
        /// </summary>
        //public System.Collections.IList SpecialList
        //{
        //    get { return mSpecialList; }
        //    set { mSpecialList = value; }
        //}

        /// <summary>
        /// Gets or sets the type used for converting an object to a value and a value to an object when populating the SpecialList property. Default is System.String.
        /// ��ȡ�����������SpecialList����ʱ���ڽ�����ת��Ϊֵ�ͽ�ֵת��Ϊ���������,System.String.
        /// </summary>
        public Type SpecialType
        {
            get { return mSpecialType; }
            set { mSpecialType = value; }
        }

        /// <summary>
        /// A list of displayString. Can be null. The index must match the index of ValueList and DisplayStringList
        /// displayString���б�����Ϊnull������������ValueList��DisplayStringList������ƥ��
        /// </summary>
        public System.Collections.IList DisplayStringList
        {
            get { return m_DisplayStringList; }
            set { m_DisplayStringList = value; }
        }

        private bool m_bThrowErrorIfNotFound = true;

        /// <summary>
        /// Gets or sets, if throw an error when the value if not found in one of the collections.
        /// Default true.
        /// </summary>
        public bool ThrowErrorIfNotFound
        {
            get { return m_bThrowErrorIfNotFound; }
            set { m_bThrowErrorIfNotFound = value; }
        }

        private void p_Validator_ConvertingValueToDisplayString(object sender, ConvertingObjectEventArgs e)
        {
            if (m_ValueList == null)
                throw new ApplicationException("ValueList cannot be null");

            //���ֵΪ�գ�
            if ((AllowNull && e.Value == null) || (AllowNull && string.IsNullOrEmpty(e.Value.ToString())))
            {
                e.Value = null;
                e.ConvertingStatus = ConvertingStatus.Completed;
                return;
            }
            else
            {

            }
            //valuelist �������  key=id,value=name
            //if (m_ValueList.Values.Contains(e.Value.ToString()))
            //{

            //}
            //else if (m_bThrowErrorIfNotFound)
            //    e.ConvertingStatus = ConvertingStatus.Error;

            if(e.ConvertingStatus == ConvertingStatus.Completed)
            {
                return;
            }

            if (m_ValueList.ContainsKey(e.Value.ToString()))
            {
                e.Value = m_ValueList[e.Value.ToString()].ToString();
                e.ConvertingStatus = ConvertingStatus.Completed;
                return;
            }
            else if (m_bThrowErrorIfNotFound)
            {
                e.ConvertingStatus = ConvertingStatus.Error;
                return;
            }
                
            //if (m_bThrowErrorIfNotFound)
            //    e.ConvertingStatus = ConvertingStatus.Error;
        }

        private void p_Validator_ConvertingObjectToValue(object sender, ConvertingObjectEventArgs e)
        {
            //by watson ��Ӵ��� ���������Ϊ����ֱ�ӷ��ؿ�
            if ((AllowNull && e.Value == null) || (AllowNull && string.IsNullOrEmpty(e.Value.ToString())))
            {
                e.Value = null;
                e.ConvertingStatus = ConvertingStatus.Completed;
                return;
            }
            //end

            if (m_ValueList == null)
                throw new ApplicationException("ValueList cannot be null");

            //Verifico se fa parte della lista di valori
            if (m_ValueList.ContainsKey(e.Value.ToString()))
            {
                //e.Value = m_ValueList[(K)e.Value].ToString();
                e.Value = e.Value;
                e.ConvertingStatus = ConvertingStatus.Completed;
            }
            else
            {
                //Verifico se fa parte della lista di oggetti, in questo caso restituisco il valore corrispondente
                //index = mSpecialList.IndexOf(e.Value);
                //if (index >= 0)
                //{
                //    e.Value = m_ValueList[index];
                //    e.ConvertingStatus = ConvertingStatus.Completed;
                //}
                //else 
                if (m_bThrowErrorIfNotFound)
                    e.ConvertingStatus = ConvertingStatus.Error;
            }

        }

        private void p_Validator_ConvertingValueToObject(object sender, ConvertingObjectEventArgs e)
        {

            //by watson ��Ӵ��� ���������Ϊ����ֱ�ӷ��ؿ�
            if ((AllowNull && e.Value == null) || (AllowNull && string.IsNullOrEmpty(e.Value.ToString())))
            {
                e.Value = null;
                e.ConvertingStatus = ConvertingStatus.Completed;
                return;
            }
            //end

            if (m_ValueList == null)
                throw new ApplicationException("ValueList cannot be null");

            //Verifico se il valore ?presente nella list, in questo caso restituisco l'oggetto associato
            if (m_ValueList.ContainsKey(e.Value.ToString()))
            {
                e.Value = m_ValueList[e.Value.ToString()].ToString();
                e.ConvertingStatus = ConvertingStatus.Completed;
            }
            else
            {
                if (m_bThrowErrorIfNotFound)
                    e.ConvertingStatus = ConvertingStatus.Error;
            }

        }
    }
}
