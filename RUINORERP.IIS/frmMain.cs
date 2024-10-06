using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.IIS
{
    public partial class frmMain : Form
    {
        private static frmMain _instance;
        public static frmMain Instance
        {
            get
            {
                return _instance;
            }
        }
        public RUINORERP.Model.Context.ApplicationContext AppContext { set; get; }
        //public ILogger<frmMain> _logger { get; set; }
        public ILoggerService _logger { get; set; }

        public frmMain(ILoggerService logger)
        {
            InitializeComponent();
            _instance = this;
            _logger = logger;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            AppContext = Program.AppContextData;
        }

        async private void StartServerUI()
        {
            Application.DoEvents();
            // timerServerInfo.Start();
            btnStartServer.Enabled = false;
            try
            {
                PrintInfoLog("开始启动服务器");
                _logger.LogInformation("开始启动socket服务器");

                //启动socket
                frmMain.Instance.PrintInfoLog("StartServerUI Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                await Task.Run(() =>
                {
                    webserver.RunWebServer();
                    //StartServer(); 
                });


            }
            catch (Exception ex)
            {
                //log4netHelper.error("StartServer总异常", ex);
                _logger.LogError("总异常", ex);
                Console.WriteLine("StartServer总异常" + ex.Message);
                //throw;
            }
        }

        WebServer webserver = Startup.GetFromFac<WebServer>();
        async Task StartServer()
        {
            frmMain.Instance.PrintInfoLog("StartServer Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            try
            {
                webserver.RunWebServer();
            }
            catch (Exception e)
            {
                frmMain.Instance.PrintInfoLog("_host.RunAsync()" + e.Message);

            }

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public void PrintInfoLog(string msg)
        {
            try
            {
                if (IsDisposed || !frmMain.Instance.IsHandleCreated) return;
                frmMain.Instance.Invoke(new EventHandler(delegate
                {
                    frmMain.Instance.logViewer1.SelectionColor = Color.Black;
                    frmMain.Instance.logViewer1.AppendText(System.DateTime.Now.ToString());
                    frmMain.Instance.logViewer1.AppendText(" : ");
                    frmMain.Instance.logViewer1.AppendText(msg);
                    frmMain.Instance.logViewer1.AppendText("\r\n");
                    frmMain.Instance.logViewer1.SelectionColor = Color.Black;
                }
                ));
            }
            catch (Exception ex)
            {

            }
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            webserver.StopWebServer();
            btnStopServer.Enabled = false;
            btnStartServer.Enabled = true;
            PrintInfoLog("停止服务器");
            Application.DoEvents();
            Application.Exit();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            StartServerUI();
            btnStopServer.Enabled = true;
            btnStartServer.Enabled = false;
            PrintInfoLog("启动服务器");
        }


    }
}
