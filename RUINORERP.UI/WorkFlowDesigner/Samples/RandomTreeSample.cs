using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace RUINORERP.UI.WorkFlowDesigner
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class RandomTreeSample : SampleBase
	{
	
		public RandomTreeSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
			mediator.AddRandomNodes(9);
			mediator.SetLayoutAlgorithm(GraphLayoutAlgorithms.Tree);
			mediator.GraphControl.StartLayout();	
		}

	
		
	
	}
}
