using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowDesigner.Entities
{
    /// <summary>
    /// 审批用户实体
    /// </summary>
    public class ApprovalUser
    {
        private int _id;
        private string _name;
        private int _departmentId;
        private string _departmentName;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int DepartmentId
        {
            get { return _departmentId; }
            set { _departmentId = value; }
        }

        public string DepartmentName
        {
            get { return _departmentName; }
            set { _departmentName = value; }
        }
    }
}