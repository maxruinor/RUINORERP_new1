﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/14/2024 16:49:16
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
    /// 包装规格表
    /// </summary>
    public partial class tb_PackingServices : BaseServices<tb_Packing>, Itb_PackingServices
    {
        IMapper _mapper;
        public tb_PackingServices(IMapper mapper, IBaseRepository<tb_Packing> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}