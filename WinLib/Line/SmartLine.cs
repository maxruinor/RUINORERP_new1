using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CSharpWin.Line
{
    [Designer("LineProject.SmartLineControlDesigner", typeof(System.ComponentModel.Design.IDesigner))]
    public class SmartLine : Control
    {
        public Orientation Orientation { get; internal set; }
    }
}
