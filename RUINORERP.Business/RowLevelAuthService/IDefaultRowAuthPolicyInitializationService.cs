using RUINORERP.Global;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.RowLevelAuthService
{
    public interface IDefaultRowAuthPolicyInitializationService
    {
        Task InitializeDefaultPoliciesAsync();
        Task<bool> EnsureDefaultPoliciesForBizTypeAsync(BizType bizType);
        Task<List<tb_RowAuthPolicy>> GetOrCreateDefaultPoliciesAsync(BizType bizType);
    }
}
