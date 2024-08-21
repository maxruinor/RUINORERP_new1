using System;

namespace RUINORERP.UI.WorkFlowDesigner
{
	/// <summary>
	/// Abstract base class for codons
	/// </summary>
	public abstract class CodonBase
	{
		string codonName;

		public string CodonName
		{
			get{return codonName;}
			set{codonName = value;}
		}

		public CodonBase(string name)
		{
			codonName = name;
		}
	}
}
