using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALEmployeesEF : IDALEmployees
    {

        protected void CheckEmployee(Employee e)
        {
            var err = "" ;

            if (e.StartDate == null) // valido fecha
            {
                err = "Tiene que ingresar fecha";
            }
            if (err != "") // 
            {
                throw new ArgumentNullException(err);
            }
        
            if (e.Name == null) // valido nombre 
            {
                err = "Nombre incompleto";
            }
        }


    public void AddEmployee(Employee emp)
        {
            try
            {
                string type = emp.GetType().BaseType.Name;
                using (DataAccessLayer.Model.ObligEntities context = new Model.ObligEntities() )
                {

                    DataAccessLayer.Model.Employee employee = null;
                    if(emp is PartTimeEmployee)
                    {
                        PartTimeEmployee partTimeEmployee = (PartTimeEmployee)emp; // casteito
                        employee = new DataAccessLayer.Model.PartTimeEmployee()
                        {
                            Name = partTimeEmployee.Name,
                            StartDate = partTimeEmployee.StartDate,
                            HourlyRate = partTimeEmployee.HourlyRate
                        };
                       
                    }
                    else
                    {
                        FullTimeEmployee fullTimeEmployee = (FullTimeEmployee)emp;
                        employee = new DataAccessLayer.Model.FullTimeEmployee()
                        {
                            Name = fullTimeEmployee.Name,
                            StartDate = fullTimeEmployee.StartDate,
                            Salary = fullTimeEmployee.Salary
                        };
                    }
                    context.Employee.Add(employee);
                    context.SaveChanges();
                }
// check
            }
            catch (Exception e)
            {
                throw e;
            }
            //throw new NotImplementedException();
        }

        public void DeleteEmployee(int id)
        {
            try
            {

                using (DataAccessLayer.Model.ObligEntities context = new Model.ObligEntities())
                {
                    var result = from emp in context.Employee
                                 where emp.EmployeeId == id
                                 select emp;
                    var error = "";
                    if (result.Count() == 1)
                    {
                        Model.Employee EmpleadoB = result.First();
                        context.Employee.Remove(EmpleadoB);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentNullException(error);
                    }
                }

            }catch(ArgumentNullException ex)
            {
                Console.WriteLine("Argumentos invalidos");
            }
        }
        public void UpdateEmployee(Employee emp)
        {
            try
            {
                using (Model.ObligEntities context = new Model.ObligEntities())
                {
                    var employee = context.Employee.Find(emp.Id);
                    var error = "";
                    if (emp is PartTimeEmployee)
                    {
                        var partTimeEmployeeModel = (Model.PartTimeEmployee)employee;
                        var partTimeEmployee = (PartTimeEmployee)emp;
                        if (partTimeEmployee is null)
                        {
                            throw new ArgumentNullException(error);
                        }
                        else
                        {
                            partTimeEmployeeModel.Name = partTimeEmployee.Name;
                            partTimeEmployeeModel.StartDate = partTimeEmployee.StartDate;
                            partTimeEmployeeModel.HourlyRate = partTimeEmployee.HourlyRate;
                        }
                    }
                    else
                    {
                        var fullTimeEmployeeModel = (Model.FullTimeEmployee)employee;
                        var fullTimeEmployee = (FullTimeEmployee)emp;
                            fullTimeEmployeeModel.Name = fullTimeEmployee.Name;
                            fullTimeEmployeeModel.StartDate = fullTimeEmployee.StartDate;
                            fullTimeEmployeeModel.Salary = fullTimeEmployee.Salary;
                     }
                    context.SaveChanges();
                }
            }
            catch (ArgumentNullException ex) // pasan emp null
            {
                Console.WriteLine("Argumentos invalidos");
            }
        }

        public List<Employee> GetAllEmployees()
        {
            try
            {
                List<Employee> employeesList = new List<Employee>();
                using (Model.ObligEntities context = new Model.ObligEntities())
                {
                    var employeeListModel = context.Employee.ToList();
                    foreach (var emp in employeeListModel)
                    {
                        if (emp is Model.FullTimeEmployee)
                        {
                            var fullTimeEmployeeModel = (Model.FullTimeEmployee)emp;
                            var fullTimeEmployee = new FullTimeEmployee()
                            {
                                Id = fullTimeEmployeeModel.EmployeeId,
                                Name = fullTimeEmployeeModel.Name,
                                Salary = Convert.ToInt32(fullTimeEmployeeModel.Salary),
                                StartDate = fullTimeEmployeeModel.StartDate
                            };
                            employeesList.Add(fullTimeEmployee);
                        }
                        else
                        {
                            var partTimeEmployeeModel = (Model.PartTimeEmployee)emp;
                            var partTimeEmployee = new PartTimeEmployee()
                            {
                                Id = partTimeEmployeeModel.EmployeeId,
                                HourlyRate = Convert.ToDouble(partTimeEmployeeModel.HourlyRate),
                                Name = partTimeEmployeeModel.Name,
                                StartDate = partTimeEmployeeModel.StartDate
                            };
                            employeesList.Add(partTimeEmployee);
                        }
                    }
                }

                return employeesList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Employee GetEmployee(int id)
        {
            try
            {
                using (Model.ObligEntities context = new Model.ObligEntities())
                {
                    var employee = context.Employee.Find(id);
                    if (employee is Model.FullTimeEmployee)
                    {
                        var fullTimeEmployeeModel = (Model.FullTimeEmployee)employee;
                        var fullTimeEmployee = new FullTimeEmployee()
                        {
                            Id = fullTimeEmployeeModel.EmployeeId,
                            Name = fullTimeEmployeeModel.Name,
                            Salary = Convert.ToInt32(fullTimeEmployeeModel.Salary),
                            StartDate = fullTimeEmployeeModel.StartDate
                        };
                        return fullTimeEmployee;
                    }
                    else
                    {
                        var partTimeEmployeeModel = (Model.PartTimeEmployee)employee;
                        var partTimeEmployee = new PartTimeEmployee()
                        {
                            Id = partTimeEmployeeModel.EmployeeId,
                            HourlyRate = Convert.ToDouble(partTimeEmployeeModel.HourlyRate),
                            Name = partTimeEmployeeModel.Name,
                            StartDate = partTimeEmployeeModel.StartDate
                        };

                        return partTimeEmployee;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //  throw new NotImplementedException();
}
}
