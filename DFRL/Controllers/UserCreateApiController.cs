using DFRL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace DFRL.Controllers
{
    public class UserCreateApiController : ApiController
    {
        private SingularityEdmx db = new SingularityEdmx();
        public IQueryable Gettb_User()
        {
            try
            {
                return db.tb_User;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IQueryable Gettb_User_id(int id)
        {
            var userList = db.tb_User.Where(w => w.UserId == id);
            return userList;
        }
        [HttpPost]
        public IHttpActionResult UserCreate(tb_User user)
        {
            try
            {
                var check = db.tb_User.Find(user.UserId);
                if (check == null)
                {
                    var password = "";
                    using (var sha256 = SHA256.Create())
                    {
                        var hashedBytesconfirmPwd = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.UserPassword));
                        var hashconfirmPwd = BitConverter.ToString(hashedBytesconfirmPwd).Replace("-", "").ToLower();
                        password = hashconfirmPwd;
                    }
                    var obj = new tb_User();
                    obj.UserName = user.UserName;
                    obj.UserPassword = password;
                    obj.UserMobile = user.UserMobile;
                    obj.UserEmail = user.UserEmail;
                    obj.IsActive = user.IsActive;
                    obj.AddedBy = user.AddedBy;
                    obj.AddedDate = user.AddedDate;
                    obj.UserType = user.UserType;
                    obj.IsCreatePermission = user.IsCreatePermission;
                    obj.IsEditPermission = user.IsEditPermission;
                    obj.IsViewPermission = user.IsViewPermission;
                    obj.IsDeletePermission = user.IsDeletePermission;
                    db.tb_User.Add(obj);
                    db.SaveChanges();
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    check.UserName = user.UserName;
                    check.UserMobile = user.UserMobile;
                    check.UserEmail = user.UserEmail;
                    check.IsActive = user.IsActive;
                    check.AddedBy = user.AddedBy;
                    check.AddedDate = user.AddedDate;
                    check.UserType = user.UserType;
                    check.IsCreatePermission = user.IsCreatePermission;
                    check.IsEditPermission = user.IsEditPermission;
                    check.IsViewPermission = user.IsViewPermission;
                    check.IsDeletePermission = user.IsDeletePermission;
                    db.SaveChanges();
                    return StatusCode(HttpStatusCode.NoContent);
                }

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
            }
            return StatusCode(HttpStatusCode.NoContent);
        }



    }
}
