
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:04
// **************************************
using AutoMapper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Threading.Tasks;
using System;
﻿using SqlSugar;
using System.Collections.Generic;


namespace RUINORERP.Services
{
    /// <summary>
    /// 业务类型 报销，员工借支还款，运费
    /// </summary>
    public partial class tb_FM_ExpenseTypeServices : BaseServices<tb_FM_ExpenseType>, Itb_FM_ExpenseTypeServices
    {
        IMapper _mapper;
        public tb_FM_ExpenseTypeServices(IMapper mapper, IBaseRepository<tb_FM_ExpenseType> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}