using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class EmployeeRequest
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public int Salary { get; set; }
        public double HourlyRate { get; set; }
        public int type {get;set;}
    }

}
