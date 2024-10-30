using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RUINORERP.ServerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();

            //tb_PurOrder order = new tb_PurOrder();
            //order.PurOrderNo = "PUR2132";
            //tb_PurEntry purEntry = mapper.Map<tb_PurEntry>(order);

        }
    }
}
