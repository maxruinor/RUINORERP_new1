using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using RUINORERP.Model;
using RUINORERP.IServices;
using RUINORERP.Services;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model.Base;

namespace RUINORERP.UI
{
    public partial class MainForm_test : FrmBase
    {
        public MainForm_test()
        {
            InitializeComponent();
        }
        public static Autofac.IContainer AUTOFAC_Container { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            BaseTipsHelper.SHowInfo<tb_Prod>(p => p.ShortCode).WithMessage("");

            BaseTipsHelper.InfoFor<tb_ProdDetail>(p => p.BarCode, "条码规则是这样的");

            //List<System.Reflection.Assembly> alldlls = RUINORERP.Common.Helper.AssemblyHelper.GetReferanceAssemblies(AppDomain.CurrentDomain, true);

            //RUINORERP.Extensions.AutofacRegister.GetAllTypes(alldlls);
            //UnitController uc;
            //if (BaseServiceProvider != null)
            //{
            //    uc = BaseServiceProvider.GetRequiredService<UnitController>();// new UnitController(ius);
            //}
            //else
            //{
            //    uc = AutofacBuilder.GetFromFac<UnitController>(); //获取服务Service1
            //}

            //ISupplierServices us = Startup.GetFromFac<SupplierServices>();

            //TB_SupplierDto dto = new TB_SupplierDto();
            //dto.ID = 1;
            //dto.Name = "mnn";

            //tb_Supplier dto = new tb_Supplier();
            //dto.ID = 3;
            //dto.Name = "mnn";
           // tb_Supplier su = await us.SaveRole(dto);

            TestMethod();
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
            }
        }
        #endregion

        private async void button2_Click(object sender, EventArgs e)
        {
            UnitController uc = Startup.GetFromFac<UnitController>();
            await uc.TransTest();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }






        /*
         private void CreateDockWorkspace()
{
    KryptonDockingWorkspace w = kryptonDockingManager.ManageWorkspace("Workspace", kryptonDockableWorkspace);
    kryptonDockingManager.ManageControl("Control", kryptonPanel, w);
    kryptonDockingManager.ManageFloating("Floating", this);

    kryptonDockingManager.AddToWorkspace("Workspace", new KryptonPage[] {
            NewPage("Overview"),
            NewPage("Main"),
            NewPage("Report"),
    });

    // This is where you wire up the CloseAction event handler
    var workspace = kryptonDockingManager.CellsWorkspace.FirstOrDefault();
    workspace.CloseAction += HandleTabCloseAction;
}

private void HandleTabCloseAction(object sender, CloseActionEventArgs e)
{
    // This event handler ignores the action
    e.Action = CloseButtonAction.None;
}

private KryptonPage NewPage(string name)
{
    var p = new KryptonPage();
    p.Text = name;
    p.TextTitle = name;
    p.TextDescription = name;

    content.Dock = DockStyle.Fill;
    p.Controls.Add(content);

    return p;
}
         
         */


    }
}
