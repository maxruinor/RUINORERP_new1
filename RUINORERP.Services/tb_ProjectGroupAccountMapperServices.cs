
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 10:43:38
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
    /// 项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面
    /// </summary>
    public partial class tb_ProjectGroupAccountMapperServices : BaseServices<tb_ProjectGroupAccountMapper>, Itb_ProjectGroupAccountMapperServices
    {
        IMapper _mapper;
        public tb_ProjectGroupAccountMapperServices(IMapper mapper, IBaseRepository<tb_ProjectGroupAccountMapper> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}