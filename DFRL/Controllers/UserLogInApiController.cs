using DFRL.Models;
using DFRL.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace Singularity.Controllers
{
    public class UserLoginApiController : ApiController
    {
        private SingularityEdmx db = new SingularityEdmx();
        [ResponseType(typeof(tb_User))]
        public IHttpActionResult Login(tb_User UserObj)
        {
            try
            {
                tb_User responseLogin = new tb_User();
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(UserObj.UserPassword));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    var UserPassword = hash;
                    if (db.tb_User.Any(user => user.UserName.Equals(UserObj.UserEmail, StringComparison.OrdinalIgnoreCase) && user.UserPassword.Equals(UserPassword) && user.IsActive == true))
                    {
                        var userInfo = db.tb_User
                            .Where(user => user.UserName.Equals(UserObj.UserEmail, StringComparison.OrdinalIgnoreCase)
                            && user.UserPassword.Equals(UserPassword) && user.IsActive == true).FirstOrDefault();

                        responseLogin.UserId = userInfo.UserId;
                        responseLogin.UserName = userInfo.UserName;
                        responseLogin.IsCreatePermission = userInfo.IsCreatePermission;
                        responseLogin.IsEditPermission = userInfo.IsEditPermission;
                        responseLogin.IsDeletePermission = userInfo.IsDeletePermission;
                        responseLogin.IsViewPermission = userInfo.IsViewPermission;
                        responseLogin.Token = TokenManager.GenerateToken(userInfo.UserName);
                    }
                    else if (db.tb_User.Any(user => user.UserEmail.Equals(UserObj.UserEmail, StringComparison.OrdinalIgnoreCase) && user.UserPassword.Equals(UserPassword) && user.IsActive == true))
                    {
                        var userInfo = db.tb_User
                            .Where(user => user.UserEmail.Equals(UserObj.UserEmail, StringComparison.OrdinalIgnoreCase)
                            && user.UserPassword.Equals(UserPassword) && user.IsActive == true).FirstOrDefault();

                        responseLogin.UserId = userInfo.UserId;
                        responseLogin.UserName = userInfo.UserName;
                        responseLogin.IsCreatePermission = userInfo.IsCreatePermission;
                        responseLogin.IsEditPermission = userInfo.IsEditPermission;
                        responseLogin.IsDeletePermission = userInfo.IsDeletePermission;
                        responseLogin.IsViewPermission = userInfo.IsViewPermission;
                        responseLogin.Token = TokenManager.GenerateToken(userInfo.UserName);
                    }
                    else if (db.tb_User.Any(user => user.UserMobile.Equals(UserObj.UserEmail, StringComparison.OrdinalIgnoreCase) && user.UserPassword.Equals(UserPassword) && user.IsActive == true))
                    {
                        var userInfo = db.tb_User
                            .Where(user => user.UserMobile.Equals(UserObj.UserEmail, StringComparison.OrdinalIgnoreCase)
                            && user.UserPassword.Equals(UserPassword) && user.IsActive == true).FirstOrDefault();

                        responseLogin.UserId = userInfo.UserId;
                        responseLogin.UserName = userInfo.UserName;
                        responseLogin.IsCreatePermission = userInfo.IsCreatePermission;
                        responseLogin.IsEditPermission = userInfo.IsEditPermission;
                        responseLogin.IsDeletePermission = userInfo.IsDeletePermission;
                        responseLogin.IsViewPermission = userInfo.IsViewPermission;
                        responseLogin.Token = TokenManager.GenerateToken(userInfo.UserName);
                    }
                    else
                    {
                        return BadRequest("Invailed username or password");
                    }
                }
                return CreatedAtRoute("DefaultApi", new { Token = responseLogin.Token }, responseLogin);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        return BadRequest("exception:" + validationError.ErrorMessage + "" + "Property:" + validationError.PropertyName);
                    }
                }
                return StatusCode(HttpStatusCode.NoContent);
            }
          
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool tb_UserExists(int id)
        {
            return db.tb_User.Count(e => e.UserId == id) > 0;
        }
    }
}
