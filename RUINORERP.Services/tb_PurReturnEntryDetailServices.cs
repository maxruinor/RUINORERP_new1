
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/16/2024 20:05:38
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
    /// 采购退货入库单明细
    /// </summary>
    public partial class tb_PurReturnEntryDetailServices : BaseServices<tb_PurReturnEntryDetail>, Itb_PurReturnEntryDetailServices
    {
        IMapper _mapper;
        public tb_PurReturnEntryDetailServices(IMapper mapper, IBaseRepository<tb_PurReturnEntryDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}