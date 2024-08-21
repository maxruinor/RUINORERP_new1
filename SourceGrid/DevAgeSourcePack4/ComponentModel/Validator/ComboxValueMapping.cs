using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevAge.ComponentModel.Validator
{
    /// <summary>
    ///  基于ValueMapping，复制了一份修改而成
    ///  The ValueMapping class can be used to easily map a value to a string value or a display string for conversion
    /// </summary>
    public class ComboxValueMapping
    {

        #region Null

        /// <summary>
        /// 添加是否允许空值的属性
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
        /// 有效值的列表。如果为null，则发生错误。索引必须与ValueList和DisplayStringList的索引匹配
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
        /// 可以转换为值的对象列表。可以为null。索引必须与ValueList和DisplayStringList的索引匹配。必须是SpecialType属性中指定类型的对象列表。
        /// 通常，在执行特定类型的特殊转换时可以使用此属性。例如，如果您想将枚举值或id值映射到字符串以获得更好的用户体验。
        /// </summary>
        //public System.Collections.IList SpecialList
        //{
        //    get { return mSpecialList; }
        //    set { mSpecialList = value; }
        //}

        /// <summary>
        /// Gets or sets the type used for converting an object to a value and a value to an object when populating the SpecialList property. Default is System.String.
        /// 获取或设置在填充SpecialList属性时用于将对象转换为值和将值转换为对象的类型,System.String.
        /// </summary>
        public Type SpecialType
        {
            get { return mSpecialType; }
            set { mSpecialType = value; }
        }

        /// <summary>
        /// A list of displayString. Can be null. The index must match the index of ValueList and DisplayStringList
        /// displayString的列表。可以为null。索引必须与ValueList和DisplayStringList的索引匹配
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

            //如果值为空，
            if ((AllowNull && e.Value == null) || (AllowNull && string.IsNullOrEmpty(e.Value.ToString())))
            {
                e.Value = null;
                e.ConvertingStatus = ConvertingStatus.Completed;
                return;
            }
            else
            {

            }
            //valuelist 保存的是  key=id,value=name
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
            //by watson 添加代码 如果请允许为空则直接返回空
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

            //by watson 添加代码 如果请允许为空则直接返回空
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
