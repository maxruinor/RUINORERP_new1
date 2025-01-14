
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 20:57:17
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
    /// 返工入库单明细
    /// </summary>
    public partial class tb_MRP_ReworkEntryDetailServices : BaseServices<tb_MRP_ReworkEntryDetail>, Itb_MRP_ReworkEntryDetailServices
    {
        IMapper _mapper;
        public tb_MRP_ReworkEntryDetailServices(IMapper mapper, IBaseRepository<tb_MRP_ReworkEntryDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}