using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFRL.Models
{
    public class UserLoginInfo
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public int HomeCompanyCode { get; set; } 
        public string UserName { get; set;}
        public string BranchCode { get; set; } 
        public string EmployeeId { get; set; }
        public string AttendenceUserId { get; set; }
        public string Token { get; set; } 
    }
}