
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:30
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
    /// 用户角色个性化设置表
    /// </summary>
    public partial class tb_UserPersonalizedServices : BaseServices<tb_UserPersonalized>, Itb_UserPersonalizedServices
    {
        IMapper _mapper;
        public tb_UserPersonalizedServices(IMapper mapper, IBaseRepository<tb_UserPersonalized> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}