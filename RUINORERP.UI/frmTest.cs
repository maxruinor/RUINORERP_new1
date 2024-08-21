using Autofac;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.IServices;
using SourceGrid2;
using System.Collections.Concurrent;
using RUINORERP.UI.Common;
using System.Reflection;
using RUINORERP.UI.UCSourceGrid;


namespace RUINORERP.UI
{
    public partial class frmTest : Form
    {
        private int x = 10;
        private int y = 10;
        public frmTest()
        {
            InitializeComponent();
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            btnRedo.Enabled = false;
            Command command = new Command();
            Label label = new Label();
            Random r = new Random();
            label.Text = ((char)r.Next(65, 99)).ToString();
            label.Font = new Font("Arial", 16, FontStyle.Bold);
            label.AutoSize = true;
            label.Location = new Point(x, y);
            /*
             * 使用匿名委托，更加简单，而且匿名委托方法里还可以使用外部变量。
             */
            command.DoOperation = delegate ()
            {
                //Do或者Redo操作会执行到的代码
                x += label.Width + 10;
                y += label.Height + 10;
                this.Controls.Add(label);
            };
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码
                x -= label.Width + 10;
                y -= label.Height + 10;
                this.Controls.Remove(label);
            };
            command.Do();//执行DoOperation相应代码
            CommandManager.AddNewCommand(command);
            btnUndo.Enabled = true;
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            btnUndo.Enabled = true;
            CommandManager.Redo();
            if (CommandManager.RedoStepsCount == 0)
            {
                btnRedo.Enabled = false;
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            btnRedo.Enabled = true;
            CommandManager.Undo();
            if (CommandManager.UndoStepsCount == 0)
            {
                btnUndo.Enabled = false;
            }
        }
        tb_DepartmentController<tb_Department> mc = Startup.GetFromFac<tb_DepartmentController<tb_Department>>();
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            mc.GetDepartmentByID(99);
            System.Console.WriteLine("==========");
            return;
            using (var scope = Startup.AutoFacContainer.BeginLifetimeScope())
            {
                //解析的接口
                var Itb_DepartmentServices = scope.Resolve<Itb_DepartmentServices>();
                Itb_DepartmentServices.GetDepartments();
            }

            return;
            // mc.MethodAOPTest("aaaa");
            // mc.QueryAsync();
            using (var scope = Startup.AutoFacContainer.BeginLifetimeScope())
            {
                //var person = scope.Resolve<tb_DepartmentController>();
                // person.GetDepartmentByID(1);
                // person.MethodAOPTest();
                var personbu = scope.Resolve<PersonBus>();
                personbu.Method_NoVirtua();
                personbu.Method2PersonBus();
                personbu.Method3PersonBus("lastasop==", "aopwats");

            }
        }


        private async void kryptonButton2_Click(object sender, EventArgs e)
        {

            using (var scope = Startup.AutoFacContainer.BeginLifetimeScope())
            {
                //解析的接口
                var Itb_DepartmentServices = scope.Resolve<Itb_DepartmentServices>();
                await Itb_DepartmentServices.GetDepartments();
            }

            //mc.MethodAOPTest("我");
            //TestMethod();
        }

        #region 测试方法
        public static void TestMethod()
        {
            Console.WriteLine("使用自己new创建的对象无法使用容器提供的ioc");
            var person1 = new Person();
            person1.Method_NoVirtua();
            person1.Method2();
            person1.Method3("person1_p1", "person1_p2");
            Console.WriteLine();
            using (var scope = Startup.AutoFacContainer.BeginLifetimeScope())
            {
                var person = scope.Resolve<Person>();

                person.Method_NoVirtua();
                person.Method2();
                person.Method3("person_p1", "person_p2");

                var bus = scope.Resolve<PersonBus>();
                bus.Method_NoVirtua();
                bus.Method2PersonBus();
                bus.Method3PersonBus("ppp", "sss");


            }
        }
        #endregion

        private void btnUseSqlsugar_Click(object sender, EventArgs e)
        {
            using (var scope = Startup.AutoFacContainer.BeginLifetimeScope())
            {
                //解析的接口
                var EmployeeController = scope.Resolve<tb_EmployeeController<tb_Department>>();
                EmployeeController.GetEmployeeall();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Type combinedType = Common.EmitHelper.MergeTypes(true, typeof(Model.Dto.ProductSharePart), typeof(tb_OpeningInventory));
            object ReturnSumInst = Activator.CreateInstance(combinedType);
            //创建实例化
            //object o = ReturnSumInst.InvokeMember("ReturnSum", BindingFlags.InvokeMethod, null, ReturnSumInst, null);//调用方法
            //Console.WriteLine("Sum:{0}", o.ToString());//显示结果

            ///显示列表对应的中文
            System.Collections.Concurrent.ConcurrentQueue<KeyValuePair<string, PropertyInfo>> Ddc = EmitHelper.GetfieldNameList(combinedType);
            List<GridDefineColumnItem> listCols = new List<GridDefineColumnItem>();


            foreach (var item in Ddc)
            {
                GridDefineColumnItem gdc = new GridDefineColumnItem();
                gdc.name = item.Key;
                gdc.ColPropertyInfo = item.Value as PropertyInfo;
                listCols.Add(gdc);
            }

            // gridvisual gv = (gridvisual)cenetcom.util.refutil.tools.getattoftype(type, typeof(gridvisual));

            int[] sizes = new int[Ddc.Count];
            for (int ii = 0; ii < Ddc.Count; ii++)
            {
                sizes[ii] = 90;
            }
            GridDefine define = new GridDefine(listCols.ToArray());

            //define.DisableEdit();

            Common.GridHelper.InitGridForEdit(this.grid1, define, true);
            return;
            /*
            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.ColumnsCount = 3;
            //grid1.FixedRows = 1;
            grid1.Rows.Insert(0);
            grid1[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("String");
            grid1[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("DateTime");
            grid1[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader("CheckBox");
            for (int r = 1; r < 10; r++)
            {
                grid1.Rows.Insert(r);
                grid1[r, 0] = new SourceGrid2.Cells.Real.Cell("Hello " + r.ToString(), typeof(string));
                grid1[r, 1] = new SourceGrid2.Cells.Real.Cell(DateTime.Today, typeof(DateTime));
                grid1[r, 2] = new SourceGrid2.Cells.Real.CheckBox(true);
            }

            grid1.AutoSizeAll();
            */



            // Common.GridHelper.fillgrid<tb_Unit>(this.grid1, list, Common.UIHelper.GetFieldNameList<tb_Unit>());
            return;
            grid1.Header3D = true;


            // grid1.Rows.Insert(r);
            //grid1[r, 0] = new SourceGrid2.Cells.Real.Cell("Hello " + r.ToString(), typeof(string));
            //grid1[r, 1] = new SourceGrid2.Cells.Real.Cell(DateTime.Today, typeof(DateTime));
            //grid1[r, 2] = new SourceGrid2.Cells.Real.CheckBox(true);
            //grid1[r, 0] = new SourceGrid2.Cells.Real.Cell("  ", typeof(string));
            //grid1[r, 1] = new SourceGrid2.Cells.Real.Cell("  ", typeof(string));
            //grid1[r, 2] = new SourceGrid2.Cells.Real.Cell("  ", typeof(string));


            grid1.AutoSizeAll();

            return;

        
        }

        private void btnGridTest_Click(object sender, EventArgs e)
        {
            Type combinedType = Common.EmitHelper.MergeTypes(true, typeof(Model.Dto.ProductSharePart), typeof(tb_OpeningInventory));
            object ReturnSumInst = Activator.CreateInstance(combinedType);
            //创建实例化
            //object o = ReturnSumInst.InvokeMember("ReturnSum", BindingFlags.InvokeMethod, null, ReturnSumInst, null);//调用方法
            //Console.WriteLine("Sum:{0}", o.ToString());//显示结果

            ///显示列表对应的中文
            System.Collections.Concurrent.ConcurrentQueue<KeyValuePair<string, PropertyInfo>> Ddc = EmitHelper.GetfieldNameList(combinedType);
            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            foreach (var item in Ddc)
            {
                SourceGridDefineColumnItem gdc = new SourceGridDefineColumnItem();
                gdc.ColCaption = item.Value.Name;
                gdc.ColName = item.Key;
                gdc.ColPropertyInfo = item.Value as PropertyInfo;
                listCols.Add(gdc);
            }
            //SourceGridHelper<tb_ProdDetail, tb_OpeningInventory> sgh = new SourceGridHelper<tb_ProdDetail, tb_OpeningInventory>();
            SourceGridHelper sgh = new SourceGridHelper();
            SourceGridDefine sgd = new SourceGridDefine(grid2, listCols, true);
            sgh.InitGrid(grid2, sgd, "frmtest");

            grid2.SelectionMode = SourceGrid.GridSelectionMode.Row;
            for (int r = 1; r < 30; r++)
            {
                //SourceGridHelper<tb_Product, tb_OpeningIinventory_detail>.AddRow(grid2, sgd, true);
                //sgh.AddRow(grid2, sgd, true);
            }

            /*
            Random rnd = new Random();
            for (int r = 1; r < 10; r++)
            {
                int row = grid2.Rows.Count - 1;
                grid2.Rows.Insert(row + 1);


                //                grid2[r, 0] = new SourceGrid.Cells.RowHeader(r.ToString());
                SourceGridHelper.AddRowForEdit(grid2, sgd, false);
            }
            */


            for (int r = 0; r < grid2.RowsCount; r++)
            {
                //grid2.Rows[r].AutoSizeMode = SourceGrid.AutoSizeMode.None; //禁止调整
            }



            grid2.AutoSizeCells();






            //设置倒数第二行的高度，直接将最后一行挤到最底部；
            //grid2.Rows[grid2.Rows.Count - 2].Height = grid2.Height - grid2.Rows[0].Height * (grid2.RowsCount) + 10;
            //设置倒数第一行的内容
            //grid2.FixedRows = 20;


            //grid2.Rows[grid2.Rows.Count - 1]
            grid2.Rows[grid2.Rows.Count - 1].Height = 40;

            grid2[grid2.Rows.Count - 1, 0] = new SourceGrid.Cells.Cell("总计", typeof(string));
            grid2[grid2.Rows.Count - 1, 4] = new SourceGrid.Cells.Cell("562125" + "圆整", typeof(string));
            CellBackColorAlternate viewNormal = new CellBackColorAlternate(Color.Khaki, Color.DarkKhaki);
            //viewNormal.Border = cellBorder;
            for (int c = 0; c < grid2.ColumnsCount; c++)
            {
                grid2[grid2.Rows.Count - 1, c].View = viewNormal;
            }
            //grid2[grid2.Rows.Count - 1, 0].View.BackColor= Color.FromArgb(217, 217, 255);

        }

        // private IDataPortal<RUINORERP.Business.Csla.tb_LocationTypeList> _portal;
        private void btnCslaQuery_Click(object sender, EventArgs e)
        {
            //  var personList = await _portal.FetchAsync();
            // dataGridView1.DataSource = personList;
        }

        private void btnBIndTest_Click(object sender, EventArgs e)
        {

        }
    }
}

