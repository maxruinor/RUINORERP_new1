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

        public static bool Login(string username, string password, ApplicationContext appcontext, ref bool IsInitPwd)
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
                    var identity = new ClaimsIdentity(claims, "admin", ClaimTypes.Name, ClaimTypes.Role);
                    appcontext.User = new ClaimsPrincipal(identity);
                }

                #endregion
            }
            if (username != "admin")
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

                //密码如果为初始123456，则每次登陆会提示修改
                string enPwd = EncryptionHelper.AesEncryptByHashKey("123456", username);
                if (EnPassword == enPwd)
                {
                    IsInitPwd = true;
                }
                else
                {
                    IsInitPwd = false;
                }


                tb_UserInfo user = users[0];
                if (user != null)
                {
                    if (SetCurrentUserInfo(appcontext, user))
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
                        appcontext.log.MachineName = appcontext.CurrentUser.客户端版本 + "-" + System.Environment.MachineName + "-" + System.Environment.UserName;

                        //调用方法
                        MacAddressHelper macHelper = new MacAddressHelper();
                        //调用方法一
                        string mac1 = macHelper.GetMacByIpConfig();
                        appcontext.log.MAC = mac1;
                        appcontext.IsSuperUser = user.IsSuperUser;
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
                            .Includes(a => a.tb_MenuInfos, b => b.tb_UIMenuPersonalizations, c => c.tb_userpersonalized)
                            .Includes(a => a.tb_MenuInfos, b => b.tb_UIMenuPersonalizations, c => c.tb_UIQueryConditions)
                            .Includes(a => a.tb_MenuInfos, b => b.tb_UIMenuPersonalizations, c => c.tb_UIGridSettings)
                            .ToList();


                appcontext.CurUserInfo.UserModList = modlist;
                loginSucceed = true;
                #endregion
            }



            return loginSucceed;
        }


        /// <summary>
        /// 将用户名和密码查出来的所有的资料都缓存到全局上下文appcontext
        /// </summary>
        /// <param name="appcontext"></param>
        /// <param name="user"></param>
        /// <param name="CurrentRole">正常登陆时传空，换角色时指定</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool SetCurrentUserInfo(ApplicationContext appcontext, tb_UserInfo user, tb_RoleInfo CurrentRole = null)

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
            appcontext.CurrentUser.Employee_ID = user.tb_employee.Employee_ID;
            appcontext.CurUserInfo.Name = user.tb_employee.Employee_Name;
            appcontext.CurrentUser.登陆时间 = System.DateTime.Now;
            appcontext.CompanyInfo = user.tb_employee.tb_department.tb_company;

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
            if (CurrentRole == null)
            {
                appcontext.CurrentRole = roles[0];
                appcontext.CurrentUser_Role = CheckRoles[0];
            }
            else
            {
                appcontext.CurrentRole = CurrentRole;
                appcontext.CurrentUser_Role = CheckRoles.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
            }

            //已经有了。
            //将所有菜单按钮和字段都给到当前权限
            //if (appcontext.CurrentRole != null)
            //{
            //    appcontext.CurrentRole = user.tb_User_Roles.Where(c => c.RoleID == CurrentRole.RoleID).FirstOrDefault().tb_roleinfo;
            //}

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
                RUINORERP.Business.BusinessHelper.Instance.InitEntity(appcontext.CurrentUser_Role_Personalized);
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
