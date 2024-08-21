using System.ComponentModel;
using System.Windows.Forms;

namespace SHControls.Mycontrol
{
    public partial class MytoolStrip : System.Windows.Forms.ToolStrip
    {
        public MytoolStrip()
        {
            InitializeComponent();
            // addDateTimePicker();
        }

        public MytoolStrip(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void addDateTimePicker()
        {
            DateTimePicker dt1 = new DateTimePicker();
            //dt1.ShowCheckBox = true;
            dt1.Width = 110;
            ToolStripControlHost host1 = new ToolStripControlHost(dt1);
            host1.Alignment = ToolStripItemAlignment.Right;


            DateTimePicker dt2 = new DateTimePicker();
            dt1.ShowCheckBox = true;
            dt2.Width = 110;
            ToolStripControlHost host2 = new ToolStripControlHost(dt2);
            host2.Alignment = ToolStripItemAlignment.Right;

            //toolStrip1.Items.Insert(10, host1);
            this.Items.Add(host1);
            this.Items.Add(host2);
        }
    }
}
