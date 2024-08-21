using System;
using System.Security.Claims;
using Csla;
using Csla.Core;
using Csla.Security;
using RUINORERP.Business.UseCsla;
using RUINORERP.Model;

namespace RUINORERP.Business.Security
{
    [Serializable]
    public class CredentialValidator : ReadOnlyBase<CredentialValidator>
    {
        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get => GetProperty(NameProperty);
            private set => LoadProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<string> AuthenticationTypeProperty = RegisterProperty<string>(nameof(AuthenticationType));
        public string AuthenticationType
        {
            get => GetProperty(AuthenticationTypeProperty);
            private set => LoadProperty(AuthenticationTypeProperty, value);
        }

        public static readonly PropertyInfo<MobileList<string>> RolesProperty = RegisterProperty<MobileList<string>>(nameof(Roles));
        public MobileList<string> Roles
        {
            get => GetProperty(RolesProperty);
            private set => LoadProperty(RolesProperty, value);
        }

        public ClaimsPrincipal GetPrincipal()
        {
            var identity = new ClaimsIdentity(AuthenticationType);

            //==
            if (ApplicationContext.User.Identity == null || !ApplicationContext.User.Identity.IsAuthenticated)
            {
                var claims = new Claim[]
                {
                new Claim(ClaimTypes.Name, "UserName"),
                new Claim(ClaimTypes.NameIdentifier, "UserID"),
                new Claim(ClaimTypes.Role, Security.Roles.Administrator),
                };
                //Custom
                //identity = new ClaimsIdentity(claims); //这样 identity.IsAuthenticated = false;

                //CustomApiKeyAuth 随便添加一个字符串，才能使identity.IsAuthenticated = true;
                //identity = new ClaimsIdentity(claims, "CustomApiKeyAuth");
                identity = new ClaimsIdentity(claims, "Custom");

                // var principal = new ClaimsPrincipal(new[] { identity });
                ApplicationContext.User = new ClaimsPrincipal(identity);
            }
            else
            {
                ApplicationContext.User = new ClaimsPrincipal();
            }
            //==
            /*
            if (!string.IsNullOrWhiteSpace(Name))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, Name));
                //添加权限
                //if (Roles != null)
                //    foreach (var item in Roles)
                //        identity.AddClaim(new Claim(ClaimTypes.Role, item));

                identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
            }
            */
            //ApplicationContext.HasPermission = true;
            return new ClaimsPrincipal(identity);
        }

        /*
        [Fetch]
        private void Fetch(Credentials credentials, [Inject] Itb_UserInfoDal dal)
        {
            tb_UserInfoFactory
            tb_UserInfo data = new tb_UserInfo();// dal.Fetch(credentials.Username, credentials.Password);
            data.UserName = "watson";
            data.Password = "123456";
            if (data != null)
            {
                AuthenticationType = "Custom";
                Name = data.UserName;
                Roles = new MobileList<string>(10);
            }
        }
        */

        /*
        [Fetch]
        private void Fetch(Credentials credentials, [Inject] tb_UserInfoFactory dal)
        {
            tb_UserInfoEditInfo data = dal.Create();
            //tb_UserInfo data = new tb_UserInfo();// dal.Fetch(credentials.Username, credentials.Password);
            data.UserName = "watson";
            data.Password = "123456";
            if (data != null)
            {
                AuthenticationType = "Custom";
                Name = data.UserName;
                Roles = new MobileList<string>(10);
            }
        }
        */
    }
}
