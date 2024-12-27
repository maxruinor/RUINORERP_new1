
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:04:28
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
    /// 返工入库
    /// </summary>
    public partial class tb_MRP_ReworkEntryServices : BaseServices<tb_MRP_ReworkEntry>, Itb_MRP_ReworkEntryServices
    {
        IMapper _mapper;
        public tb_MRP_ReworkEntryServices(IMapper mapper, IBaseRepository<tb_MRP_ReworkEntry> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}