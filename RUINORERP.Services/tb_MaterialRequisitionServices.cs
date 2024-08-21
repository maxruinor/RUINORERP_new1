
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/05/2024 17:00:20
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
    /// 领料单(包括生产和托工)
    /// </summary>
    public partial class tb_MaterialRequisitionServices : BaseServices<tb_MaterialRequisition>, Itb_MaterialRequisitionServices
    {
        IMapper _mapper;
        public tb_MaterialRequisitionServices(IMapper mapper, IBaseRepository<tb_MaterialRequisition> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}