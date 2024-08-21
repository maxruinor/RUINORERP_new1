using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowTester
{
    public class MyNameClass
    {
        private int _counter = 0;
        public int Counter { get => _counter; set => _counter = value; }

        public string MyName { get; set; }
    }
}
