
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:02
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
    /// 价格记录表
    /// </summary>
    public partial class tb_PriceRecordServices : BaseServices<tb_PriceRecord>, Itb_PriceRecordServices
    {
        IMapper _mapper;
        public tb_PriceRecordServices(IMapper mapper, IBaseRepository<tb_PriceRecord> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}