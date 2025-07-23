
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/23/2025 12:19:08
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
    /// 蓄水登记表
    /// </summary>
    public partial class tb_EOP_WaterStorageServices : BaseServices<tb_EOP_WaterStorage>, Itb_EOP_WaterStorageServices
    {
        IMapper _mapper;
        public tb_EOP_WaterStorageServices(IMapper mapper, IBaseRepository<tb_EOP_WaterStorage> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}