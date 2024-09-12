using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.WebServer
{
    public partial class frmMain : Form
    {
        private static  frmMain _instance ;
        public static frmMain Instance
        {
            get
            {
                return _instance;
            }
        }
        public frmMain()
        {
            InitializeComponent();
            _instance = this;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            HttpServerService.Start();
            FileServer.Instance.Start();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            HttpServerService.Stop();
            FileServer.Instance.Stop();
        }
    }
}
