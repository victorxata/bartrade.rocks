using System.Collections.Generic;

namespace Domain.Data.Identity
{
    public class ManageInfoModel
    {
        public string LocalLoginProvider { get; set; }

        public string UserName { get; set; }

        public IEnumerable<UserLoginInfoModel> Logins { get; set; }

        public IEnumerable<ExternalLoginModel> ExternalLoginProviders { get; set; }
    }
}