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
                appcontext.Status = "��½�ɹ�";
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
            //�������룬Ϊ�˵�һ�ν�ϵͳ
            if (username == "admin" && password == "amwtjhwxf")
            {
                appcontext.IsSuperUser = true;
                #region ��ʱ��д�����˺�Ȩ�޵�

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
                #region �����û���֤  ���׷�ʽ  ��������P4�������Ƶ�

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

                //�������Ϊ��ʼ123456����ÿ�ε�½����ʾ�޸�
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
                        appcontext.Status = "��½�ɹ�";
                        if (user.tb_employee != null)
                        {
                            appcontext.log.Operator = user.tb_employee.Employee_Name;
                        }
                        appcontext.log.User_ID = user.User_ID;
                        //��ȡ���ؼ������+���ؼ������¼��
                        appcontext.log.MachineName = appcontext.CurrentUser.�ͻ��˰汾 + "-" + System.Environment.MachineName + "-" + System.Environment.UserName;

                        //���÷���
                        MacAddressHelper macHelper = new MacAddressHelper();
                        //���÷���һ
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
                cuser.Name = "��������Ա";
                cuser.Id = 0;
                appcontext.CurUserInfo = cuser;

                cuser.UserInfo = new tb_UserInfo();
                cuser.UserInfo.UserName = "��������Ա";

                List<tb_ModuleDefinition> modlist = new List<tb_ModuleDefinition>();
                //���׷�ʽ ��������P4�������Ƶ�  ��������Ա��Ĭ�ϲ˵��ṹ
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
        /// ���û������������������е����϶����浽ȫ��������appcontext
        /// </summary>
        /// <param name="appcontext"></param>
        /// <param name="user"></param>
        /// <param name="CurrentRole">������½ʱ���գ�����ɫʱָ��</param>
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
                throw new Exception("��ʹ�õ��˺�û������Ա����");
            }
            appcontext.CurUserInfo.Id = user.tb_employee.Employee_ID;
            appcontext.CurrentUser.Employee_ID = user.tb_employee.Employee_ID;
            appcontext.CurUserInfo.Name = user.tb_employee.Employee_Name;
            appcontext.CurrentUser.��½ʱ�� = System.DateTime.Now;
            appcontext.CompanyInfo = user.tb_employee.tb_department.tb_company;

            //һ�������ڲ�ͬ�Ľ�ɫ�顣���Զ����������ҪΪ����Ȩ
            List<tb_RoleInfo> roles = new List<tb_RoleInfo>();

            //== ���������ʹ��     
            List<tb_User_Role> CheckRoles = user.tb_User_Roles.Where(c => c.Authorized).ToList();
            CheckRoles = CheckRoles.Distinct().ToList();//Ӧ�ò����ظ�
            //Ĭ�ϵ���ǰ��
            CheckRoles = CheckRoles.OrderByDescending(c => c.DefaultRole).ThenBy(c => c.ID).ToList();
            if (CheckRoles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("���������κν�ɫ�飬����ϵ����Ա��");
                loginSucceed = false;
                return loginSucceed;
            }
            //ϵͳ֧�ֶ���飬����ÿ����Чֻһ��������һ���Ż�
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

            //�Ѿ����ˡ�
            //�����в˵���ť���ֶζ�������ǰȨ��
            //if (appcontext.CurrentRole != null)
            //{
            //    appcontext.CurrentRole = user.tb_User_Roles.Where(c => c.RoleID == CurrentRole.RoleID).FirstOrDefault().tb_roleinfo;
            //}

            //ÿ���û��ͽ�ɫ��Ӧһ���û�����
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

            //���ý�ɫ���������ã�Ĭ��ֻȡ��һ����ɫ�������,������������ǰ��һ��
            if (appcontext.CurrentRole.tb_rolepropertyconfig != null)
            {
                appcontext.rolePropertyConfig = appcontext.CurrentRole.tb_rolepropertyconfig;
            }

            //��ȡ����̨����
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
