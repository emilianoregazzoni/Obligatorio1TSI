using DataAccessLayer;
using Ninject;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BLEmployees : IBLEmployees
    {
        [Inject, Named("ef")]

        private readonly IDALEmployees _dal; // DI 


        public BLEmployees(IDALEmployees dALEmployees) // DI
        {
            _dal = dALEmployees;
        }

        public void AddEmployee(Employee emp)
        {
            //IDALEmployees dALEmployees = new DALEmployeesEF();
            IDALEmployees dALEmployees = new DALEmployeesMongo();
            dALEmployees.AddEmployee(emp);
        }

        public void DeleteEmployee(int id)
        {
            // DataAccessLayer.DALEmployeesEF dALEmployeesEF = new DALEmployeesEF();
            // dALEmployeesEF.DeleteEmployee(id);
            DataAccessLayer.DALEmployeesMongo dALEmployeesMongo = new DALEmployeesMongo();
            dALEmployeesMongo.DeleteEmployee(id);
        }

        public void UpdateEmployee(Employee emp)
        {
            //DataAccessLayer.DALEmployeesEF dALEmployeesEF = new DALEmployeesEF();
            //dALEmployeesEF.UpdateEmployee(emp); // reviso  bLEmployees.UpdateEmployee(emp);
            DataAccessLayer.DALEmployeesMongo dALEmployeesMongo = new DALEmployeesMongo();
            dALEmployeesMongo.UpdateEmployee(emp);
        }

        public List<Employee> GetAllEmployees()
        {

            DataAccessLayer.DALEmployeesEF dALEmployeesEF = new DALEmployeesEF();
            return dALEmployeesEF.GetAllEmployees();
            //return this._dal.GetAllEmployees();
        }

        public Employee GetEmployee(int id)
        {

            DataAccessLayer.DALEmployeesEF dALEmployeesEF = new DALEmployeesEF();
            return dALEmployeesEF.GetEmployee(id);
        }

        public double CalcPartTimeEmployeeSalary(int idEmployee, int hours)
        {
            {
                Employee emp = _dal.GetEmployee(idEmployee);
                if (emp == null || emp is FullTimeEmployee)
                {
                    throw new Exception("Emp incorrecto ");
                }
                PartTimeEmployee employee = (PartTimeEmployee)emp;
                return employee.HourlyRate * hours;
            }

        }
    }
}
