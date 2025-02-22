using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Toolkit.Suite.Extended.TreeGridView.Components
{
    public class ProgressBarColumn : DataGridViewColumn
    {
        public ProgressBarColumn()
        {
            this.CellTemplate = new ProgressBarCell();
        }
    }
}
