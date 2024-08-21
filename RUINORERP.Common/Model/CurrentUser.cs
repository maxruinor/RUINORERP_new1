using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Model
{
    public class CurrentUser : ICurrentUser
    {
        /// <summary>
        /// 当前登录用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public long Id { get; set; }

        public DateTime CreateTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
