using BusinessLogicLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.Filters;

namespace ApiServices.Controllers
{
    //[JwtAuthentication]
    public class EmployeeController : ApiController
    {
        private readonly IBLEmployees _bLEmployees;
        //private readonly 

        public EmployeeController(IBLEmployees bLEmployees)
        {
            this._bLEmployees = bLEmployees;
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public Employee Get(int id)
        {
            return this._bLEmployees.GetEmployee(id);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]EmployeeRequest EmployeeRequest)
        {
            Employee emp = null;
            if (EmployeeRequest.type == 1) {

                 emp = new PartTimeEmployee()
                {
                    Name = EmployeeRequest.Name,
                    StartDate = EmployeeRequest.StartDate,
                    HourlyRate = EmployeeRequest.HourlyRate
                };

            }
            else if(EmployeeRequest.type == 2)
            {
                 emp = new FullTimeEmployee()
                {
                    Name = EmployeeRequest.Name,
                    StartDate = EmployeeRequest.StartDate,
                    Salary = EmployeeRequest.Salary
                };

            }
            this._bLEmployees.AddEmployee(emp);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]EmployeeRequest EmployeeRequest)
        {

            /// BusinessLogicLayer.BLEmployees bLEmployees = new BLEmployees();
            Employee emp = null;

            if (EmployeeRequest.type == 1)
            {

                emp = new PartTimeEmployee()
                {
                    Id = id,
                    Name = EmployeeRequest.Name,
                    StartDate = EmployeeRequest.StartDate,
                    HourlyRate = EmployeeRequest.HourlyRate
                };

            }
            else if (EmployeeRequest.type == 2)
            {
                emp = new FullTimeEmployee()
                {
                    Id = id, // cubrir cuando se manda id vacio 
                    Name = EmployeeRequest.Name,
                    StartDate = EmployeeRequest.StartDate,
                    Salary = EmployeeRequest.Salary
                };

            }
            this._bLEmployees.UpdateEmployee(emp);
        }
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            this._bLEmployees.DeleteEmployee(id);
        }
    }
}

