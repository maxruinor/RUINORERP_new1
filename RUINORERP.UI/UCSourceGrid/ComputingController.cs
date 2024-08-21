using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 自定义控制器，以处理产品数量输入并更新总价
    /// </summary>
    public class ComputingController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private SourceGridDefine _currentGridDefine;
        /// <summary>
        /// 当前格子的定义
        /// </summary>
        public SourceGridDefine CurrentGridDefine { get => _currentGridDefine; set => _currentGridDefine = value; }

  
        public ComputingController(SourceGridDefine _CurrentGridDefine)
        {
            CurrentGridDefine = _CurrentGridDefine;
        }

        public override void OnValueChanged(SourceGrid.CellContext context, EventArgs e)
        {
            // 当数量单元格的值更改时，更新总价单元格的值
            int quantity = 0;
            if (int.TryParse(context.Value.ToString(), out quantity))
            {
                //int totalPrice = quantity * product.Price;
               // grid1[context.Position.Row, 4].Value = totalPrice;
            }
        }
    }

}
