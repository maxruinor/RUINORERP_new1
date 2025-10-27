
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/27/2025 17:49:29
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
    /// 文件信息元数据表
    /// </summary>
    public partial class tb_FS_FileStorageInfoServices : BaseServices<tb_FS_FileStorageInfo>, Itb_FS_FileStorageInfoServices
    {
        IMapper _mapper;
        public tb_FS_FileStorageInfoServices(IMapper mapper, IBaseRepository<tb_FS_FileStorageInfo> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}