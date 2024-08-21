using RUINORERP.Common.Helper;
using SourceGrid;
using SourceGrid.Cells.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;

namespace RUINORERP.UI.UCSourceGrid
{


    /// <summary>
    /// 选择事件控制器,参考了单据控制器。冗余了很多代码
    /// </summary>
    public class SelectedForCheckBoxController : ControllerBase// where T : class
    {

        public string ControllerName { get; set; }


        private SourceGridDefine _currentGridDefine;
        /// <summary>
        /// 当前格子的定义
        /// </summary>
        public SourceGridDefine CurrGridDefine { get => _currentGridDefine; set => _currentGridDefine = value; }

        public SelectedForCheckBoxController(string _ControllerName, SourceGridDefine _CurrentGridDefine)
        {
            ControllerName= _ControllerName;
            CurrGridDefine = _CurrentGridDefine;
        }

        //sgd 要提供当前操作的行 列 值

        public delegate void ValidateDataRowsDelegate(object rowObj);

        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ValidateDataRowsDelegate OnValidateDataRows;

        public delegate void ValidateDataCellDelegate(object rowObj);

        /// <summary>
        /// 验证数据
        /// </summary>
        public event ValidateDataCellDelegate OnValidateDataCell;

 

        public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
            // CurrGridDefine.CurrentCellLocation = sender.Position;
        }
        public override void OnKeyPress(CellContext sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape && CurrGridDefine.grid.Focused)
            {

            }
            else
            {
                base.OnKeyPress(sender, e);
            }

            //如果有总计点到最后一行。直接返回
            if (CurrGridDefine.HasSummaryRow && sender.Position.Row == CurrGridDefine.grid.RowsCount - 1)
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                //如果是编辑状态则回车编辑控件

                if (sender.Cell.Editor != null && sender.Cell.Editor.IsEditing)
                {
                    CellContext editCellContext = sender.Cell.Editor.EditCellContext;
                    //点击按钮
                    //bool success = sender.Cell.Editor.InternalEndEdit(false);
                    //if (success)
                    //{
                    //    Grid.Controller.OnEditEnded(editCellContext, EventArgs.Empty);
                    //}
                    //return success;
                }

            }
        }

        //这里是任何变动都会执行
        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            if (sender.Value == null)
            {
                //清空关联值
                CurrGridDefine.SetDependTargetValue(null, sender.Position, null, CurrGridDefine[sender.Position.Column].ColName);
                return;
            }
            else
            {
                //如果是关联列就跳过,如果是指向  导向列，即可指向明细的列则更新值
                //if (CurrGridDefine.DependQuery.RelatedCols.Any(c => c.ColIndex == sender.Position.Column && !c.GuideToTargetColumn))
                //{
                //    return;
                //}
            }
           

            //要把当前合法的值给到 真正的对象
            var setcurrentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
            if (setcurrentObj != null)
            {
                ReflectionHelper.SetPropertyValue(setcurrentObj, CurrGridDefine[sender.Position.Column].ColName, sender.Value);
            }
            else
            {
                return;
            }

        }

      

    }



}
