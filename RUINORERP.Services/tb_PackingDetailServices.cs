
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/14/2024 16:49:17
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
    /// 包装清单
    /// </summary>
    public partial class tb_PackingDetailServices : BaseServices<tb_PackingDetail>, Itb_PackingDetailServices
    {
        IMapper _mapper;
        public tb_PackingDetailServices(IMapper mapper, IBaseRepository<tb_PackingDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}