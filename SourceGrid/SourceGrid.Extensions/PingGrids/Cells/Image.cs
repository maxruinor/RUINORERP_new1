
using System;

namespace SourceGrid.Extensions.PingGrids.Cells
{
	public class ImageCell : SourceGrid.Cells.Virtual.ImageCell
	{
	        public ImageCell()
		{
	            Model.AddModel(new PingGridValueModel());
		}
	}
}
