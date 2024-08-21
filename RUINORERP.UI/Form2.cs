using RUINORERP.Common.CustomAttribute;
using RUINORERP.UI.UserCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;

namespace RUINORERP.UI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public static Form2 Instance { get; private set; }
        private ApplicationContext ApplicationContext { get; }
        //private IServiceProvider ServiceProvider { get; }



       // private IDataPortal<RUINORERP.Business.UseCsla.tb_LocationTypeList> _portal;

        /*
        public Form2(Csla.ApplicationContext applicationContext, IServiceProvider serviceProvider
            , IDataPortal<RUINORERP.Business.UseCsla.tb_LocationTypeList> portal)
        {
            Instance = this;
            ServiceProvider = serviceProvider;
            ApplicationContext = applicationContext;
            _portal = portal;
            InitializeComponent();

        }
        */
        //[CustPropertyAutowiredAttribute]
        //public IDataPortal<tb_LocationTypeEditBindingList> _bportal { get; set; }


        private  void btnQuery_Click(object sender, EventArgs e)
        {
            /*
            var listPortal = Program.cslaAppContext.GetRequiredService<IChildDataPortal<tb_LocationTypeEditBindingList>>();
            var itemPortal = Program.cslaAppContext.GetRequiredService<IChildDataPortal<tb_LocationTypeEditInfo>>();

            itemPortal.CreateChild();
            var tt=itemPortal.CreateChild();
            
            var list = listPortal.CreateChild();

            //var lista = listPortal.Update(new tb_LocationTypeEditBindingList());

            list.Add(itemPortal.CreateChild(213, "abc"));
            list.Add(itemPortal.CreateChild(113, "qwe"));
            this.bindingSource1.DataSource = list;

            this.bindingSource1.DataSource = new Csla.SortedBindingList<tb_LocationTypeEditInfo>(list);

            var filtered = new Csla.FilteredBindingList<tb_LocationTypeEditInfo>(list);
            this.bindingSource1.DataSource = filtered;
            filtered.ApplyFilter("Name", "abc");
            */

            return;

           // var mylist = _bportal.Fetch();

            //IDataPortal<tb_LocationTypeList> _portal = Program.cslaAppContext.GetRequiredService<IDataPortal<tb_LocationTypeList>>();
            //var personList = await _portal.FetchAsync();
            //dataGridView1.DataSource = mylist;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (ApplicationContext.User.Identity == null || !ApplicationContext.User.Identity.IsAuthenticated)
            {
                var claims = new Claim[]
                {
          new Claim(ClaimTypes.Name, "Test User"),
          new Claim(ClaimTypes.Role, "Admin"),
                };
                var identity = new ClaimsIdentity(claims, "Test", ClaimTypes.Name, ClaimTypes.Role);
                ApplicationContext.User = new ClaimsPrincipal(identity);
            }
            else
            {
                ApplicationContext.User = new ClaimsPrincipal();
            }

            if (ApplicationContext.User.Identity == null || !ApplicationContext.User.Identity.IsAuthenticated)
                loginButton.Text = "Login";
            else
                loginButton.Text = ApplicationContext.User.Identity.Name;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            UCInitControlCenter _UCInitControlCenter = new UCInitControlCenter();
            //RightPanel rp = new RightPanel();
            this.Controls.Add(_UCInitControlCenter);
            //rp.LoadPage();
        }
    }
}
