﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 20:57:18
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
    /// 返工退库明细
    /// </summary>
    public partial class tb_MRP_ReworkReturnDetailServices : BaseServices<tb_MRP_ReworkReturnDetail>, Itb_MRP_ReworkReturnDetailServices
    {
        IMapper _mapper;
        public tb_MRP_ReworkReturnDetailServices(IMapper mapper, IBaseRepository<tb_MRP_ReworkReturnDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}