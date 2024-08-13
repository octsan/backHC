using ERP_MaxysHC.Maxys.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_MaxysHC.Maxys.Model
{
    public class Jwt : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public class tokenVal
        {
            public bool success { get; set; }
            public string message { get; set; }
            public UsersModel result { get; set; }
        }

        public static tokenVal validateToken(ClaimsIdentity identity)
        {
            tokenVal res = new tokenVal();

            try
            {
                if (identity.Claims.Count() == 0)
                {
                    res.success = false;
                    res.message = "Token inválido";
                    res.result = null;
                    return res;
                }
                var id = identity.Claims.FirstOrDefault(x => x.Type.Equals("Id_user")).Value;
                var pos = identity.Claims.FirstOrDefault(x => x.Type.Equals("Position")).Value;
                var pass = identity.Claims.FirstOrDefault(x => x.Type.Equals("Password")).Value;
                UsersModel userModel = new UsersModel();
                userModel.Id_user = Convert.ToInt32(id);
                userModel.Position = pos;
                userModel.Password = pass;
                res.success = true;
                res.message = "Token válido";
                res.result = userModel;
                return res;
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = "Catch" + ex.Message;
                res.result = null;
                return res;
            }
        }
    }
}
