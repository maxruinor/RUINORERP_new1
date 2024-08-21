
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/28/2024 11:55:45
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
    /// 退料单明细
    /// </summary>
    public partial class tb_MaterialReturnDetailServices : BaseServices<tb_MaterialReturnDetail>, Itb_MaterialReturnDetailServices
    {
        IMapper _mapper;
        public tb_MaterialReturnDetailServices(IMapper mapper, IBaseRepository<tb_MaterialReturnDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}