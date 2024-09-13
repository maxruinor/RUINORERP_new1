
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:59
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
    /// 菜单权限表
    /// </summary>
    public partial class tb_P4MenuServices : BaseServices<tb_P4Menu>, Itb_P4MenuServices
    {
        IMapper _mapper;
        public tb_P4MenuServices(IMapper mapper, IBaseRepository<tb_P4Menu> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}