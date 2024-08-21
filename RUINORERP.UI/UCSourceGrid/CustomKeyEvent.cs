using SourceGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.Extensions;
using SourceGrid.Cells.Controllers;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 自定义键的控制
    /// </summary>
    public class CustomKeyEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        public CustomKeyEvent()
        {

        }
        public override void OnKeyDown(SourceGrid.CellContext sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                sender.Grid.Selection.FocusRow(sender.Position.Row + 1);
                e.Handled = true;
            }
            else
            {
                //需要手工输入的才到这里判断
                if (e.KeyCode == Keys.Enter)
                {
                    if (sender.Cell.Editor != null)
                    {
                        SourceGridDefine sgDefine = sender.Grid.Tag as SourceGridDefine;

                        
                        string txtValue = sender.Cell.Editor.GetEditedText();
                        //也许还有其他验证方法，目前是通过属性得到       如果数据验证通过时
                        if (sgDefine[sender.Position.Column] != null)
                        {
                            var ctr = sender.Cell.FindController<ToolTipText>();
                            if (!txtValue.Is(sgDefine[sender.Position.Column].ColPropertyInfo.PropertyType))
                            {
                                //ctr.ToolTipTitle = "请输入正确的值。";
                                ctr.ApplyToolTipText(sender, e);

                                sender.Grid.ToolTip.Show("请输入正确的值", sender.Grid);


                                MainForm.Instance.uclog.AddLog("请输入正确格式的值");
                                e.Handled = true;//true的意思是 已经处理了。不需要处理后面的逻辑 比方 数量cell输入小数时
                            }
                            else
                            {
                                sender.Grid.ToolTipText = null;
                            }
                        }
                    }
                    else
                    {

                    }



                }
                //base.OnKeyDown(sender, e);
            }

            //CustomGrid_KeyDown(sender, e);
        }

        private void CustomGrid_KeyDown(SourceGrid.CellContext sender, KeyEventArgs e)
        {
            //if (sender.Grid.PositionAtPoint == new SourceGrid.Position(sender. - 1, sender.ColumnsCount - 1))
            //{
            if (e.KeyCode == Keys.Tab)
            {
                sender.Grid.Selection.FocusRow(sender.Position.Row + 1);
                e.Handled = true;
            }

            //}
            //else
            //{
            //    if (e.KeyCode == Keys.Tab)
            //    {
            //        this.Selection.ResetSelection(false);
            //        this.Selection.SelectCell(new SourceGrid.Position(this.Selection.ActivePosition.Row, this.Selection.ActivePosition.Column + 1), true);
            //        e.Handled = true;
            //    }
            //}
        }

    }
}
