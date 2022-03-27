using DFRL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DFRL.Controllers
{
   [Authorization]
    public class PermittedUserMenuController : ApiController
    {
        private SingularityEdmx db = new SingularityEdmx();
        public IQueryable GetPermittedMenu(int userId)
        {
            int uId = Convert.ToInt32(userId);
            var roleInfo = db.tb_User.Where(x => x.UserId == uId).FirstOrDefault();
            var UserType = roleInfo.UserType;
            if (UserType == "Admin")
            {
                var itemAll = db.tb_main_menu.Where(x =>x.is_active==true).Select(x => new
                {
                    x.parent_icon_class,
                    x.parent_menu_text,
                    x.display_order,
                    tb_user_sub_menu = (from d in db.tb_sub_menu.Where(y => y.main_menu_id == x.Id)
                                        select new
                                        {
                                            d.sub_menu_text,
                                            d.sub_icon_class,
                                            d.page_url
                                        })
                });
                return itemAll.OrderBy(x => x.display_order).AsQueryable();
            }
            else
            {
                var itemAll = db.tb_main_menu.Where(x => x.parent_menu_text== "Employee").Select(x => new
                {
                    x.parent_icon_class,
                    x.parent_menu_text,
                    x.display_order,
                    tb_user_sub_menu = (from d in db.tb_sub_menu.Where(y => y.main_menu_id == x.Id)
                                        select new
                                        {
                                            d.sub_menu_text,
                                            d.sub_icon_class,
                                            d.page_url
                                        })
                });
                return itemAll.OrderBy(x => x.display_order).AsQueryable();
            }
        }
        [HttpGet]
        public IHttpActionResult GetPathPermission(int userId, string pathArray)
        {
            try
            {
                int uId = Convert.ToInt32(userId);
                var roleInfo = db.tb_User.Where(x => x.UserId == uId).FirstOrDefault();
                var UserType = roleInfo.UserType;
                if (UserType != "Admin")
                {
                    if (pathArray.ToUpper() != "/HOME/INDEX" || pathArray.ToUpper() != "/home/employeeList")
                    {
                        return BadRequest("You are not permitted");
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
            return Ok(HttpStatusCode.Accepted);
        }
        
    }
}
