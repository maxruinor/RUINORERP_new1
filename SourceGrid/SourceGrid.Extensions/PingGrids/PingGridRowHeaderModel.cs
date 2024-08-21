
using System;
using System.ComponentModel;
using DevAge.ComponentModel;
using SourceGrid.Cells.Models;
using SourceGrid.Selection;

namespace SourceGrid.Extensions.PingGrids
{
	public class PingGridRowHeaderModel : IValueModel
	{
		public PingGridRowHeaderModel()
		{
		}

        public object GetTagValue(CellContext cellContext)
        {
            throw new NotImplementedException();
        }
        #region IValueModel Members
        public object GetValue(CellContext cellContext)
		{
			DataGrid dataGrid = (DataGrid)cellContext.Grid;
			if (dataGrid.DataSource != null &&
			    dataGrid.DataSource.AllowNew &&
			    cellContext.Position.Row == (dataGrid.Rows.Count - 1))
				return "*";
			else
				return null;
		}

        public void SetTagValue(CellContext cellContext, object p_Value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(CellContext cellContext, object p_Value)
		{
			throw new ApplicationException("Not supported");
		}
		#endregion
	}

}
