using DFRL;
using DFRL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Singularity.Controllers
{
    [Authorization]
    public class DropDownApiController : ApiController
    {
        private SingularityEdmx db = new SingularityEdmx();
        //[HttpGet]
        //public IQueryable ddlCompany()
        //{
        //    var bloodGroup = (from bg in db.tb_company
        //                      select new
        //                      {
        //                          Id = bg.CompanyId,
        //                          Name = bg.CompanyName,
        //                      }).Distinct();
        //    return bloodGroup;
        //}
    }
}
