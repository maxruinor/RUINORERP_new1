using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using RUINORERP.Model;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using HLH.Lib.Net;
using HLH.Lib.Security;


namespace RUINORERP.Business.Security
{
    public static class PTPrincipal
    {
        public static event Action NewUser;
        public static void LoginAsync(string username, string password, ApplicationContext appcontext)
        {
            try
            {
                var claims = new Claim[]
              {
                      new Claim(ClaimTypes.NameIdentifier, "1"),
                      new Claim(ClaimTypes.Name, "watson"),
                  //new Claim(ClaimTypes.Role,RUINORERP.Business.Security.Roles.Administrator),
              };
                var identity = new ClaimsIdentity(claims, "Test", ClaimTypes.Name, ClaimTypes.Role);
                appcontext.User = new ClaimsPrincipal(identity);
                appcontext.Status = "登陆成功";
                NewUser?.Invoke();
            }
            catch
            {
                Logout(appcontext);
            }
        }

        public static bool Login(string username, string password, ApplicationContext appcontext)
        {

            bool loginSucceed = false;
            appcontext.CurUserInfo = null;
            //超级密码，为了第一次进系统
            if (username == "admin" && password == "amwtjhwxf")
            {
                appcontext.IsSuperUser = true;

                #region 暂时性写死给账号权限等

                if (appcontext.User.Identity == null || !appcontext.User.Identity.IsAuthenticated)
                {
                    var claims = new Claim[]
                    {
                      new Claim(ClaimTypes.NameIdentifier, "1"),
                      new Claim(ClaimTypes.Name, "watson"),
                        new Claim(ClaimTypes.Role,RUINORERP.Business.Security.Roles.Administrator),
                    };
                    var identity = new ClaimsIdentity(claims, "Test", ClaimTypes.Name, ClaimTypes.Role);
                    appcontext.User = new ClaimsPrincipal(identity);
                }

                #endregion
            }
            if (!appcontext.IsSuperUser || username != "admin")
            {
                #region 正常用户验证  两套方式  正常是用P4表来控制的

                //password = EncryptionHelper.AesDecryptByHashKey(enPwd, username);
                string EnPassword = EncryptionHelper.AesEncryptByHashKey(password, username);

                tb_UserInfoController<tb_UserInfo> ctrUser = appcontext.GetRequiredService<tb_UserInfoController<tb_UserInfo>>();
                List<tb_UserInfo> users = new List<tb_UserInfo>();
                users = ctrUser.QueryByNavWithMoreInfo(u => u.UserName == username && u.Password == EnPassword && u.is_available && u.is_enabled);
                if (users == null || users.Count == 0)
                {
                    loginSucceed = false;
                    return loginSucceed;
                }
                tb_UserInfo user = users[0];
                if (user != null)
                {
                    if (GetAllAuthorizationInfo(appcontext, user))
                    {
                        var claims = new Claim[]
                   {
                              new Claim(ClaimTypes.NameIdentifier, user.User_ID.ToString()),
                              new Claim(ClaimTypes.Name,user.UserName),
                       // new Claim(ClaimTypes.Role,RUINORERP.Business.Security.Roles.Administrator),
                   };
                        var identity = new ClaimsIdentity(claims, "Test", ClaimTypes.Name, ClaimTypes.Role);
                        appcontext.User = new ClaimsPrincipal(identity);
                        appcontext.Status = "登陆成功";
                        if (user.tb_employee != null)
                        {
                            appcontext.log.Operator = user.tb_employee.Employee_Name;
                        }
                        appcontext.log.User_ID = user.User_ID;
                        //获取本地计算机名+本地计算机登录名
                        appcontext.log.MachineName = appcontext.OnlineUser.客户端版本 + "-" + System.Environment.MachineName + "-" + System.Environment.UserName;

                        //调用方法
                        MacAddressHelper macHelper = new MacAddressHelper();
                        //调用方法一
                        string mac1 = macHelper.GetMacByIpConfig();
                        appcontext.log.MAC = mac1;

                        appcontext.IsSuperUser = user.IsSuperUser;

                        // List<tb_RoleInfo> roles = CtrRoles.QueryAsync();

                        //Thread.CurrentPrincipal设置会导致在异步线程中设置的结果丢失
                        //因此统一采用 AppDomain.CurrentDomain.SetThreadPrincipal中设置，确保进程中所有线程都会复制到信息
                        //意思是线程上下文的数据也可以用这个传过去
                        //这里要完善  ！！！
                        //IPrincipal principal = new GenericPrincipal(identity, rolesName.Toaay);
                        //AppDomain.CurrentDomain.SetThreadPrincipal(principal);

                        //appcontext.CurrentUser.userMenuList = user.tb_User_Roles;
                        loginSucceed = true;
                    }
                }
                else
                {
                    loginSucceed = false;
                }
                #endregion 
            }
            else
            {
                #region superUser


                CurrentUserInfo cuser = new CurrentUserInfo();
                cuser.Name = "超级管理员";
                cuser.Id = 0;
                appcontext.CurUserInfo = cuser;

                cuser.UserInfo = new tb_UserInfo();
                cuser.UserInfo.UserName = "超级管理员";

                List<tb_ModuleDefinition> modlist = new List<tb_ModuleDefinition>();
                //两套方式 正常是用P4表来控制的  超级管理员用默认菜单结构
                modlist = appcontext.Db.CopyNew().Queryable<tb_ModuleDefinition>()
                            .Includes(a => a.tb_MenuInfos, b => b.tb_ButtonInfos)
                            .Includes(a => a.tb_MenuInfos, b => b.tb_FieldInfos)
                            .ToList();

                //List<tb_MenuInfo> menulist = new List<tb_MenuInfo>();
                //menulist = appcontext.Db.CopyNew().Queryable<tb_MenuInfo>()
                //            .Includes(a => a.tb_ButtonInfos)
                //            .Includes(a => a.tb_FieldInfos)
                //            .ToList();

                //加载模块
                //foreach (var mod in modlist)
                //{
                //    foreach (var menu in mod.tb_MenuInfos)
                //    {
                //        #region
                //        tb_MenuInfo MenuInfo = menulist.FirstOrDefault(m => m.MenuID == menu.MenuID);
                //        menu.tb_ButtonInfos = new List<tb_ButtonInfo>();
                //        menu.tb_ButtonInfos.AddRange(MenuInfo.tb_ButtonInfos);
                //        menu.tb_FieldInfos = new List<tb_FieldInfo>();
                //        menu.tb_FieldInfos.AddRange(MenuInfo.tb_FieldInfos);
                //        #endregion
                //    }
                //}
                appcontext.CurUserInfo.UserModList = modlist;
                loginSucceed = true;
                #endregion
            }



            return loginSucceed;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appcontext"></param>
        /// <param name="user"></param>
        /// <param name="roleInfo">正常登陆时传空，换角色时指定</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool GetAllAuthorizationInfo(ApplicationContext appcontext, tb_UserInfo user, tb_RoleInfo roleInfo = null)
        {
            bool loginSucceed = false;
            if (appcontext.CurUserInfo == null)
            {
                appcontext.CurUserInfo = new CurrentUserInfo();
            }
            appcontext.CurUserInfo.UserInfo = user;
            if (user.tb_employee == null)
            {
                throw new Exception("您使用的账号没有所属员工。");
            }
            appcontext.CurUserInfo.Id = user.tb_employee.Employee_ID;
            appcontext.CurUserInfo.Name = user.tb_employee.Employee_Name;
            appcontext.OnlineUser.登陆时间 = System.DateTime.Now;


            //一个人能在不同的角色组。可以多个。但是需要为已授权
            List<tb_RoleInfo> roles = new List<tb_RoleInfo>();

            //== 保存给下面使用     
            List<tb_User_Role> CheckRoles = user.tb_User_Roles.Where(c => c.Authorized).ToList();
            CheckRoles = CheckRoles.Distinct().ToList();//应该不会重复
            //默认的排前面
            CheckRoles = CheckRoles.OrderByDescending(c => c.DefaultRole).ThenBy(c => c.ID).ToList();
            if (CheckRoles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("您不属于任何角色组，请联系管理员。");
                loginSucceed = false;
                return loginSucceed;
            }
            //系统支持多个组，但是每次生效只一个。按第一个优化
            foreach (var item in CheckRoles)
            {
                roles.Add(item.tb_roleinfo);
            }
            appcontext.Roles = roles;
            if (roleInfo == null)
            {
                appcontext.CurrentRole = roles[0];
                appcontext.CurrentUser_Role = CheckRoles[0];
            }
            else
            {
                appcontext.CurrentRole = roleInfo;
                appcontext.CurrentUser_Role = CheckRoles.FirstOrDefault(c => c.RoleID == roleInfo.RoleID);
            }

            //每个用户和角色对应一个用户配置
            if (appcontext.CurrentUser_Role.tb_UserPersonalizeds == null)
            {
                appcontext.CurrentUser_Role.tb_UserPersonalizeds = new();

            }
            appcontext.CurrentUser_Role_Personalized = appcontext.CurrentUser_Role.tb_UserPersonalizeds.FirstOrDefault(c => c.ID == appcontext.CurrentUser_Role.ID);
            if (appcontext.CurrentUser_Role_Personalized == null)
            {
                appcontext.CurrentUser_Role_Personalized = new tb_UserPersonalized();
                appcontext.CurrentUser_Role_Personalized.ID = appcontext.CurrentUser_Role.ID;
                appcontext.CurrentUser_Role.tb_UserPersonalizeds.Add(appcontext.CurrentUser_Role_Personalized);
                appcontext.Db.Insertable(appcontext.CurrentUser_Role_Personalized).ExecuteReturnSnowflakeIdAsync();
            }

            //设置角色的属性配置，默认只取第一个角色组的配置,，并且设置提前了一级
            if (appcontext.CurrentRole.tb_rolepropertyconfig != null)
            {
                appcontext.rolePropertyConfig = appcontext.CurrentRole.tb_rolepropertyconfig;
            }


            //获取工作台配置
            appcontext.WorkCenterConfigList = new List<tb_WorkCenterConfig>();
            appcontext.WorkCenterConfigList = appcontext.Db.CopyNew().Queryable<tb_WorkCenterConfig>().Where(c => c.RoleID == appcontext.CurrentRole.RoleID).ToList();


            //下面所有能控制的东西都有路径和从属性一级传导到下一级 菜单名都可相同
            //模块

            //权限由上到下。上面没有，下面就不用处理了
            List<tb_RoleInfo> UseRoles = new List<tb_RoleInfo>();
            UseRoles.Add(appcontext.CurrentRole);
            tb_UserInfoController<tb_UserInfo> ctrUser = appcontext.GetRequiredService<tb_UserInfoController<tb_UserInfo>>();
            List<tb_RoleInfo> urs = ctrUser.QueryALLPowerByNavWithMoreInfo(UseRoles);
            foreach (var item in urs)
            {
                //下面有多个循环
                foreach (var sub in item.tb_P4Modules)
                {
                    #region  模块
                    //可见才加入,可用的话，使用时控制
                    if (sub.IsVisble)
                    {
                        tb_ModuleDefinition exmod = appcontext.CurUserInfo.UserModList.FirstOrDefault(w => w.ModuleName == sub.tb_moduledefinition.ModuleName);
                        if (exmod == null)
                        {
                            appcontext.CurUserInfo.UserModList.Add(sub.tb_moduledefinition);
                        }
                        exmod = sub.tb_moduledefinition;
                        if (exmod.tb_P4Menus == null)
                        {
                            //补充关系
                            exmod.tb_P4Menus = new List<tb_P4Menu>();
                        }
                        #region  模块下的菜单
                        List<tb_P4Menu> menuGX = item.tb_P4Menus.Where(p => p.ModuleID == exmod.ModuleID).ToList();
                        foreach (var subMenu in menuGX)
                        {
                            //可见才加入
                            if (subMenu.IsVisble && !exmod.tb_P4Menus.Contains(subMenu))
                            {
                                exmod.tb_P4Menus.Add(subMenu);

                                #region  模块下的菜单下的按钮

                                foreach (var subMenuBtn in item.tb_P4Buttons.Where(b => b.MenuID == subMenu.MenuID))
                                {
                                    if (subMenu.tb_menuinfo.tb_P4Buttons == null)
                                    {
                                        subMenu.tb_menuinfo.tb_P4Buttons = new List<tb_P4Button>();
                                    }
                                    //按钮可控属性多。只要不重复都加入
                                    if (!subMenu.tb_menuinfo.tb_P4Buttons.Contains(subMenuBtn))
                                    {
                                        subMenu.tb_menuinfo.tb_P4Buttons.Add(subMenuBtn);
                                    }
                                }

                                //subMenu.tb_menuinfo.tb_P4Buttons.AddRange(btnlist.Distinct().ToList());
                                #endregion

                                #region  模块下的菜单下的字段

                                foreach (var subMenuField in item.tb_P4Fields.Where(f => f.MenuID == subMenu.MenuID))
                                {
                                    if (subMenu.tb_menuinfo.tb_P4Fields == null)
                                    {
                                        subMenu.tb_menuinfo.tb_P4Fields = new List<tb_P4Field>();
                                    }

                                    if (!subMenu.tb_menuinfo.tb_P4Fields.Contains(subMenuField))
                                    {
                                        subMenu.tb_menuinfo.tb_P4Fields.Add(subMenuField);
                                    }
                                }
                                // appcontext.CurUserInfo.UserFieldList.AddRange(fieldlist.Distinct().ToList());
                                #endregion
                            }
                        }
                        #endregion

                    }


                    #endregion

                }

            }
            loginSucceed = true;
            return loginSucceed;
        }



        public static void Logout(ApplicationContext appcontext)
        {
            appcontext.User = new ClaimsPrincipal(new ClaimsIdentity());
            appcontext.IsSuperUser = false;
            appcontext.CurUserInfo = new CurrentUserInfo();
            NewUser?.Invoke();

        }
    }
}
