
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/22/2024 16:24:09
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
    /// 账户管理，财务系统中使用
    /// </summary>
    public partial class tb_FM_AccountServices : BaseServices<tb_FM_Account>, Itb_FM_AccountServices
    {
        IMapper _mapper;
        public tb_FM_AccountServices(IMapper mapper, IBaseRepository<tb_FM_Account> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}