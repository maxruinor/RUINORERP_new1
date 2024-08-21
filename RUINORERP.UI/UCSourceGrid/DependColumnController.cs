using RUINORERP.Common.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 设置好相关列，查询结果后给目标列值的操作
    /// </summary>
    public class DependColumnController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private SourceGridDefine _gridDefine;
        private List<DependColumn> dependencyColumns = new List<UCSourceGrid.DependColumn>();
        public List<DependColumn> DependencyColumns { get => dependencyColumns; set => dependencyColumns = value; }
        public SourceGridDefine GridDefine { get => _gridDefine; set => _gridDefine = value; }



        public DependColumnController(SourceGridDefine sgd, List<DependColumn> _dependencyColumns)
        {
            _gridDefine = sgd;
            dependencyColumns = _dependencyColumns;
        }


        public delegate object ConvertDelegate(object source);
        public ConvertDelegate ConvertFunction;


        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            //I use the OnValueChanged to link the value of 2 cells
            // changing the value of the other cell
            //if (sender.Value == null)
            //{
            //    return;
            //}
            SetValueToTarget(sender);
        }

        public override void OnTagValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnTagValueChanged(sender, e);

            //I use the OnValueChanged to link the value of 2 cells
            // changing the value of the other cell
            //SourceGrid.Position TestCell = new SourceGrid.Position(sender.Position.Row, 5);
            //SourceGrid.CellContext TestContext = new SourceGrid.CellContext(sender.Grid, TestCell);
            //TestContext.Value = "测试";
            if (sender.Value == null)
            {
                return;
            }

            SetValueToTarget(sender);
        }


        private void SetValueToTarget(SourceGrid.CellContext sender)
        {
            foreach (var item in DependencyColumns)
            {
                try
                {
                    SourceGrid.Position otherCell = new SourceGrid.Position(sender.Position.Row, item.ColIndex);
                    SourceGrid.CellContext otherContext = new SourceGrid.CellContext(sender.Grid, otherCell);
                    object newVal = sender.Value;
                    object mytag = sender.Tag;
                    if (newVal == null)
                    {
                        //相当于当前的值清空。刚相关的也清空
                        otherContext.Value = null;
                        mytag = null;
                    }
                    if (mytag == null || newVal == null)
                    {

                        //清空目标列
                        if (GridDefine.grid.Rows[sender.Position.Row].RowData != null)
                        {
                            GridDefine.grid[sender.Position.Row, GridDefine.DependQuery.TargetCol.TargetColIndex].Value = null;
                            var currentObj = GridDefine.grid.Rows[sender.Position.Row].RowData;
                            ReflectionHelper.SetPropertyValue(currentObj, item.ColName, null);

                            //给值就给行号
                            //GridDefine.grid[sender.Position.Row, 0].Value = sender.Position.Row.ToString();
                        }



                        break;
                    }
                    if (newVal.GetType() == item.ValueObjType)
                    {
                        if (!object.Equals(otherContext.Value, newVal))
                        {
                            otherContext.Value = newVal;
                        }
                    }
                    else
                    {

                        var newotherVal = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(mytag, item.ColName);
                        if (newotherVal.GetType().FullName == "System.Byte[]")
                        {
                            Image temp = RUINORERP.Common.Helper.ImageHelper.ConvertByteToImg(newotherVal as Byte[]);
                            otherContext.Value = temp;
                            continue;
                        }
                        if (item.ColName == GridDefine.DependQuery.TargetCol.TargetColumnName)
                        {
                            if (GridDefine.grid.Rows[sender.Position.Row].RowData != null)
                            {
                                var currentObj = GridDefine.grid.Rows[sender.Position.Row].RowData;
                                ReflectionHelper.SetPropertyValue(currentObj, item.ColName, newotherVal);
                                //var datavalue = ReflectionHelper.GetPropertyValue(currentObj, CurrentGridDefine[sender.Position.Column].ColName);
                                //给值就给行号
                                GridDefine.grid[sender.Position.Row, 0].Value = sender.Position.Row.ToString();
                            }
                        }

                        if (!object.Equals(otherContext.Value, newotherVal))
                        {
                            otherContext.Value = newotherVal;
                        }
                    }

                }
                catch (Exception ex)
                {


                }
            }
        }


    }
}
