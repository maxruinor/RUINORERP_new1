
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:24
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
    /// 品质检验记录表
    /// </summary>
    public partial class tb_Quality_inspection_recordServices : BaseServices<tb_Quality_inspection_record>, Itb_Quality_inspection_recordServices
    {
        IMapper _mapper;
        public tb_Quality_inspection_recordServices(IMapper mapper, IBaseRepository<tb_Quality_inspection_record> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}