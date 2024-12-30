using HLH.Lib.Helper;
using HLH.Lib.Security;
using HLH.Lib.Security.HLH.Lib.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.SecurityTool
{
    public partial class frmRUINORERPSecurity : Form
    {
        public frmRUINORERPSecurity()
        {
            InitializeComponent();
        }



        private void btnEncrypt_Click(object sender, EventArgs e)
        {

            HardwareInfo hardware = new HardwareInfo();
            //获取CPU ID
            string CPU_ID = hardware.GetComputerHardWareInfo("Win32_Processor", "ProcessorId");
            //获取主板序列号
            string Board_SN = hardware.GetComputerHardWareInfo("Win32_BaseBoard", "SerialNumber");
            //获取硬盘序列号
            string Disk_SN = hardware.GetComputerHardWareInfo("Win32_DiskDrive", "Model");
            //获取UUID
            string UUID = hardware.GetComputerHardWareInfo("Win32_ComputerSystemProduct", "UUID");


            Console.WriteLine("CPU ID: " + hardware.GetCpuId());
            Console.WriteLine("Hard Disk ID: " + hardware.GetHardDiskId());

          
            txtNewData.Text = CreateRegisterCodeWin.EnText(txtOldData.Text.Trim(), txtKey.Text);

        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtNewData.Text = CreateRegisterCodeWin.DeText(txtOldData.Text.Trim(), txtKey.Text);
        }
    }
}
