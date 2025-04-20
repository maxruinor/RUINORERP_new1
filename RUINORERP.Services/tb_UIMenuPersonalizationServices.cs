
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:12
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据
    /// </summary>
    public partial class tb_UIMenuPersonalizationServices : BaseServices<tb_UIMenuPersonalization>, Itb_UIMenuPersonalizationServices
    {
        IMapper _mapper;
        public tb_UIMenuPersonalizationServices(IMapper mapper, IBaseRepository<tb_UIMenuPersonalization> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}