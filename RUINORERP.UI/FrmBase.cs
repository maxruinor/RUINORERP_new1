using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Krypton.Toolkit;

namespace RUINORERP.UI
{
    public partial class FrmBase : KryptonForm
    {
        public static IServiceProvider BaseServiceProvider { get; set; }
        public FrmBase()
        {
            InitializeComponent();
        }
    }
}
