using System;

namespace SourceGrid.Cells.Models
{
	/// <summary>
	/// 特定于包含单元格值的模型接口.
	/// </summary>
	public interface IValueModel : IModel
	{
		#region GetValue, SetValue
		/// <summary>
		/// Get the value of the cell at the specified position
		/// </summary>
		/// <param name="cellContext"></param>
		/// <returns></returns>
		object GetValue(CellContext cellContext);

		//by watson TODO
		object GetTagValue(CellContext cellContext);

		//by watson TODO
		void SetTagValue(CellContext cellContext, object p_Value);


		/// <summary>
		/// Set the value of the cell at the specified position. This method must call OnValueChanging and OnValueChanged() event.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <param name="p_Value"></param>
		void SetValue(CellContext cellContext, object p_Value);
		#endregion
	}
}

