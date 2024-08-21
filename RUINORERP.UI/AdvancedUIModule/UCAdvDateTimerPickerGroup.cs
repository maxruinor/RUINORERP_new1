﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace RUINORERP.UI.AdvancedUIModule
{
    public partial class UCAdvDateTimerPickerGroup : UserControl
    {
        public UCAdvDateTimerPickerGroup()
        {
            InitializeComponent();
        }

        private void UCAdvDateTimerPickerGroup_Load(object sender, EventArgs e)
        {
            dtp1.Format = DateTimePickerFormat.Custom;
            dtp1.CustomFormat = "yyyy-MM-dd";
            dtp2.Format = DateTimePickerFormat.Custom;
            dtp2.CustomFormat = "yyyy-MM-dd";
        }
    }
}
