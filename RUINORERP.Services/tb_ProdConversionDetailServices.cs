
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/10/2024 14:15:54
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
    /// 产品转换单明细
    /// </summary>
    public partial class tb_ProdConversionDetailServices : BaseServices<tb_ProdConversionDetail>, Itb_ProdConversionDetailServices
    {
        IMapper _mapper;
        public tb_ProdConversionDetailServices(IMapper mapper, IBaseRepository<tb_ProdConversionDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}