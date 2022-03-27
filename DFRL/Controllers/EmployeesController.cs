using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DFRL.Models;
namespace DFRL.Controllers
{
    [Authorization]
    public class EmployeesController : ApiController
    {
        private SingularityEdmx db = new SingularityEdmx();
       
        [HttpGet]
        public IQueryable getAllEmployee()
        {
            try
            {
                return db.Employees;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IHttpActionResult getEmployeeInfo(int EmpId)
        {
            try
            {
                var empInfo = db.Employees.Where(w => w.EmpId == EmpId).FirstOrDefault();
                return CreatedAtRoute("DefaultApi", new { id = 1 }, empInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IHttpActionResult deleteEmployee(int EmpId)
        {
            try
            {
                var empInfo = db.Employees.Where(w => w.EmpId == EmpId).FirstOrDefault();
                if (empInfo != null)
                {
                    db.Employees.Remove(empInfo);
                    db.SaveChanges();
                }
                return CreatedAtRoute("DefaultApi", new { id = EmpId }, empInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IHttpActionResult SaveEmployee( Employee emp)
        {
            try
            {
                var check = db.Employees.Find(emp.EmpId);
                if (check == null)
                {
                    var nEmp = new Employee();
                    nEmp.EmpIdCardNo = emp.EmpIdCardNo;
                    nEmp.EmpName           =emp.EmpName;
                    nEmp.EmpMobile         =emp.EmpMobile;
                    nEmp.EmpEmail          =emp.EmpEmail;
                    nEmp.JoinDate          =emp.JoinDate;
                    nEmp.DepartmentName    =emp.DepartmentName;
                    nEmp.EmpJobDescription =emp.EmpJobDescription;
                    nEmp.IsActive          =emp.IsActive;
                    db.Employees.Add(nEmp);
                    db.SaveChanges();
                    return CreatedAtRoute("DefaultApi", new { id = nEmp.EmpId }, new { message="Employee has been  created."});
                }
                else
                {
                    check.EmpIdCardNo = emp.EmpIdCardNo;
                    check.EmpName = emp.EmpName;
                    check.EmpMobile = emp.EmpMobile;
                    check.EmpEmail = emp.EmpEmail;
                    check.JoinDate = emp.JoinDate;
                    check.DepartmentName = emp.DepartmentName;
                    check.EmpJobDescription = emp.EmpJobDescription;
                    check.IsActive = emp.IsActive;
                    db.SaveChanges();
                    return CreatedAtRoute("DefaultApi", new { id = check.EmpId }, new { message = "Employee has been  updated." });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}