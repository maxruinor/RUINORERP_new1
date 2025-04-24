
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/23/2025 23:27:33
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
    /// 收付款方式与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面
    /// </summary>
    public partial class tb_PayMethodAccountMapperServices : BaseServices<tb_PayMethodAccountMapper>, Itb_PayMethodAccountMapperServices
    {
        IMapper _mapper;
        public tb_PayMethodAccountMapperServices(IMapper mapper, IBaseRepository<tb_PayMethodAccountMapper> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}