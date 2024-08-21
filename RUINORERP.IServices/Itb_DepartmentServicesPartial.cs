
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/18/2023 17:03:55
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.IServices.BASE;
using System.Threading.Tasks;
using RUINORERP.Model;

namespace RUINORERP.IServices
{
    /// <summary>
    /// 部门表
    /// </summary>
    public partial interface Itb_DepartmentServices : IBaseServices<tb_Department>
    {
        Task<List<tb_Department>> GetDepartments();
    }
}