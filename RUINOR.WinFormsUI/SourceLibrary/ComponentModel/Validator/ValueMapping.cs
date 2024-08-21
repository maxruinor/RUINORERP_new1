using System;

namespace SourceLibrary.ComponentModel.Validator
{
	/// <summary>
	/// The ValueMapping class can be used to easily map a value to a string value or a display string for conversion
	/// </summary>
	public class ValueMapping
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ValueMapping()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Validator"></param>
		/// <param name="p_ValueList">A list of valid value. If null an error occurred. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
		/// <param name="p_ObjectList">A list of object that can be converted to value. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
		/// <param name="p_DisplayStringList">A list of displayString. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
		public ValueMapping(ValidatorBase p_Validator, System.Collections.IList p_ValueList, System.Collections.IList p_ObjectList, System.Collections.IList p_DisplayStringList)
		{
			ValueList = p_ValueList;
			DisplayStringList = p_DisplayStringList;
			ObjectList = p_ObjectList;
			if (p_Validator != null)
				BindValidator(p_Validator);
		}

		/// <summary>
		/// Bind the specified validator
		/// </summary>
		/// <param name="p_Validator"></param>
		public void BindValidator(ValidatorBase p_Validator)
		{
			p_Validator.ConvertingValueToDisplayString += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
			p_Validator.ConvertingObjectToValue += new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
			p_Validator.ConvertingValueToObject += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
		}

		/// <summary>
		/// Unbind the specified validator
		/// </summary>
		/// <param name="p_Validator"></param>
		public void UnBindValidator(ValidatorBase p_Validator)
		{
			p_Validator.ConvertingValueToDisplayString -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
			p_Validator.ConvertingObjectToValue -= new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
			p_Validator.ConvertingValueToObject -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
		}

		private System.Collections.IList m_ValueList;
		private System.Collections.IList m_ObjectList;
		private System.Collections.IList m_DisplayStringList;

		/// <summary>
		/// A list of valid value. If null an error occurred. The index must match the index of ValueList, ObjectList and DisplayStringList
		/// </summary>
		public System.Collections.IList ValueList
		{
			get{return m_ValueList;}
			set{m_ValueList = value;}
		}
		/// <summary>
		/// A list of object that can be converted to value. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList
		/// </summary>
		public System.Collections.IList ObjectList
		{
			get{return m_ObjectList;}
			set{m_ObjectList = value;}
		}
		/// <summary>
		/// A list of displayString. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList
		/// </summary>
		public System.Collections.IList DisplayStringList
		{
			get{return m_DisplayStringList;}
			set{m_DisplayStringList = value;}
		}

		private bool m_bThrowErrorIfNotFound = true;

		/// <summary>
		/// If true throw an error when the value if not found in one of the dictionary
		/// </summary>
		public bool ThrowErrorIfNotFound
		{
			get{return m_bThrowErrorIfNotFound;}
			set{m_bThrowErrorIfNotFound = value;}
		}

		private void p_Validator_ConvertingValueToDisplayString(object sender, ConvertingObjectEventArgs e)
		{
			if (m_DisplayStringList != null)
			{
				if (m_ValueList == null)
					throw new ApplicationException("ValueList cannot be null");
				
				int l_Index = m_ValueList.IndexOf(e.Value);
				if (l_Index >= 0)
				{
					e.Value = m_DisplayStringList[l_Index];
					e.ConvertingStatus = ConvertingStatus.Completed;
				}
				else
				{
					if (m_bThrowErrorIfNotFound)
						e.ConvertingStatus = ConvertingStatus.Error;
				}
			}
		}

		private void p_Validator_ConvertingObjectToValue(object sender, ConvertingObjectEventArgs e)
		{
			if (m_ObjectList != null)
			{
				if (m_ValueList == null)
					throw new ApplicationException("ValueList cannot be null");
				
				int l_Index = m_ObjectList.IndexOf(e.Value);
				if (l_Index >= 0)
				{
					e.Value = m_ValueList[l_Index];
					e.ConvertingStatus = ConvertingStatus.Completed;
				}
				else
				{
					if (m_bThrowErrorIfNotFound)
						e.ConvertingStatus = ConvertingStatus.Error;
				}
			}
		}

		private void p_Validator_ConvertingValueToObject(object sender, ConvertingObjectEventArgs e)
		{
			if (m_ObjectList != null)
			{
				if (m_ValueList == null)
					throw new ApplicationException("ValueList cannot be null");
				
				int l_Index = m_ValueList.IndexOf(e.Value);
				if (l_Index >= 0)
				{
					e.Value = m_ObjectList[l_Index];
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
}
