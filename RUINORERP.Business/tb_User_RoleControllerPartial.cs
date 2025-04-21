
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:42
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;

namespace RUINORERP.Business
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    public partial class tb_User_RoleController<T> : BaseController<T> where T : class
    {
        public  async Task<List<tb_User_Role>> QueryRoleByNavWithMoreInfo()
        {
            List<tb_User_Role> list = new List<tb_User_Role>();
            try
            {
                list = await _unitOfWorkManage.GetDbClient().Queryable<tb_User_Role>()
               .Includes(t => t.tb_roleinfo)
               .Includes(t => t.tb_UserPersonalizeds,w=>w.tb_UIMenuPersonalizations,v=>v.tb_UIGridSettings)
               .Includes(t => t.tb_UserPersonalizeds, w => w.tb_UIMenuPersonalizations, v => v.tb_UIQueryConditions)
               .Includes(t => t.tb_UserPersonalizeds, w => w.tb_UIMenuPersonalizations, v => v.tb_UIInputDataFields)
               .Includes(t => t.tb_userinfo)
               .ToListAsync();
                foreach (var item in list)
                {
                    item.HasChanged = false;
                }
            }
            catch (Exception ex)
            {


            }
            return list;
        }



    }
}



