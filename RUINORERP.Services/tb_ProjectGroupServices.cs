
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:29
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
    /// 项目组信息 用于业务分组小团队
    /// </summary>
    public partial class tb_ProjectGroupServices : BaseServices<tb_ProjectGroup>, Itb_ProjectGroupServices
    {
        IMapper _mapper;
        public tb_ProjectGroupServices(IMapper mapper, IBaseRepository<tb_ProjectGroup> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}