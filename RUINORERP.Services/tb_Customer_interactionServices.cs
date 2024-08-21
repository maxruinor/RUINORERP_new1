
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:55
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
    /// 客户交互表，CRM系统中使用      
    /// </summary>
    public partial class tb_Customer_interactionServices : BaseServices<tb_Customer_interaction>, Itb_Customer_interactionServices
    {
        IMapper _mapper;
        public tb_Customer_interactionServices(IMapper mapper, IBaseRepository<tb_Customer_interaction> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}