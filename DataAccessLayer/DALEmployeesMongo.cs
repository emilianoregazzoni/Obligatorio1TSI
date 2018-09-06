using Shared.Entities;
using System;
using System.Collections.Generic;
//using MongoDB.Driver;
//using MongoDB.Bson;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using MongoDB.Driver;
using MongoDB.Bson;
//using MongoWebApiDemo.Models;

namespace DataAccessLayer
{
    public class DALEmployeesMongo : IDALEmployees
    {

        private IMongoClient _client;
        private IMongoDatabase _database;

        public DALEmployeesMongo()
        {
            _client = new MongoClient("mongodb://localhost:27017");

            if (_client != null)
            {
                _database = _client.GetDatabase("tsi1"); /// record creo db y conexion
            }
        }

        public void AddEmployee(Employee emp)
        {
            var collection = _database.GetCollection<BsonDocument>("EmployeesTPH");
            BsonDocument result = collection.Find(new BsonDocument()).Sort(new BsonDocument("ID", -1)).FirstOrDefault();

            var document = new BsonDocument
            {
                { "EMP_ID", result == null ? 1 : result["EMP_ID"].ToInt32() + 1},
                { "NAME", emp.Name},
                { "TYPE_EMP", emp is FullTimeEmployee ? 2 : 1}, // si es full 2, si no es partime 1 siguiendo el model
                { emp is FullTimeEmployee ? "SALARY" : "RATE", emp is FullTimeEmployee ? ((FullTimeEmployee)emp).Salary : ((PartTimeEmployee)emp).HourlyRate},
                { "START_DATE", emp.StartDate}
            };
            collection.InsertOne(document);
        }

        public void DeleteEmployee(int id)
        {
            var collection = _database.GetCollection<BsonDocument>("EmployeesTPH");
            var filter = Builders<BsonDocument>.Filter.Eq("EMP_ID", id);
            collection.DeleteOne(filter);
        }

        public void UpdateEmployee(Employee emp)
        {
            var collection = _database.GetCollection<BsonDocument>("EmployeesTPH");
            var filter = Builders<BsonDocument>.Filter.Eq("EMP_ID", emp.Id);
            var update = Builders<BsonDocument>.Update
                .Set("NAME", emp.Name)
                .Set("START_DATE", emp.StartDate);
            BsonValue nul = null;
            if (emp is FullTimeEmployee)
            {
                update.Set("RATE", nul);
                update.Set("TYPE_EMP", 1);
                update.Set("SALARY", ((FullTimeEmployee)emp).Salary);
            }
            else
            {
                update.Set("SALARY", nul);
                update.Set("TYPE_EMP", 0);
                update.Set("RATE", ((PartTimeEmployee)emp).HourlyRate);
            }
            collection.UpdateOneAsync(filter, update);
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            var collection = _database.GetCollection<BsonDocument>("EmployeesTPH");
            var filter = new BsonDocument();

            using (var cursor = collection.FindSync(filter))
            {
                while (cursor.MoveNext())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        employees.Add(FromDocument(document));
                    }

                }
            }

            return employees;
        }

        public Employee GetEmployee(int id)
        {
            var collection = _database.GetCollection<BsonDocument>("EmployeesTPH");
            var filter = Builders<BsonDocument>.Filter.Eq("EMP_ID", id);

            Employee employee = null;

            using (var cursor = collection.FindSync(filter))
            {
                BsonDocument document = cursor.SingleOrDefault();

                employee = FromDocument(document);
            }
            return employee;
        }


        private Employee FromDocument(BsonDocument document)
        {
            Employee result = null;
            if (document != null)
            {
                if (document["TYPE_EMP"].ToInt32() == 1)
                {
                    FullTimeEmployee tmp = new FullTimeEmployee();
                    tmp.Salary = document["SALARY"].ToInt32();
                    result = tmp;
                }
                else
                {
                    PartTimeEmployee tmp = new PartTimeEmployee();
                    tmp.HourlyRate = document["RATE"].ToDouble();
                    result = tmp;
                }
                result.Id = document["EMP_ID"].ToInt32();
                result.Name = document["NAME"].ToString();
                result.StartDate = document["START_DATE"].ToLocalTime();
            }
            return result;
        } 
    }
}
