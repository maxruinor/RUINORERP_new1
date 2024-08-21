using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{
    public interface IUCBase
    {
        void SaveDataFromUI(UCBasePara bb);
        void LoadDataToUI(UCBasePara bb);
    }
}
