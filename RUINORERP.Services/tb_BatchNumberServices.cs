
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:41
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
    /// 批次表 在采购入库时和出库时保存批次ID
    /// </summary>
    public partial class tb_BatchNumberServices : BaseServices<tb_BatchNumber>, Itb_BatchNumberServices
    {
        IMapper _mapper;
        public tb_BatchNumberServices(IMapper mapper, IBaseRepository<tb_BatchNumber> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}