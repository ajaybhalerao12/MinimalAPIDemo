
namespace MinimalAPIDemo.Models
{
    // CRUD operation for the Employee model
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employeesList;
        public EmployeeService()
        {
            // Create an in memory list for the employe list
            _employeesList = new List<Employee>()
                {
                    new Employee(){ Id =  1 , Name = "Ajay1", Position= "Software Engineer", Salary = 12000},
                    new Employee{ Id = 2 , Name = "Prashant1", Position = "Project Manager", Salary = 98000 }
                };
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeesList;
        }

        public Employee? GetEmployeeById(int id)
        {
            return _employeesList.FirstOrDefault(emp => emp.Id == id);
        }
        public Employee AddEmployee(Employee employee)
        {
            employee.Id = _employeesList.Count > 0 ? _employeesList.Max(emp => emp.Id + 1) : 1;
            _employeesList.Add(employee);
            return employee;
        }

        public Employee? UpdateEmployee(int id, Employee updatedEmployee)
        {
            var employee = _employeesList.FirstOrDefault(emp => emp.Id == id);
            if (employee is null) return null;

            employee.Name = updatedEmployee.Name;
            employee.Position = updatedEmployee.Position;
            employee.Salary = updatedEmployee.Salary;

            return employee;
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeesList.FirstOrDefault(emp => emp.Id == id);
            if(employee is  null) return false;

            _employeesList.Remove(employee);
            return true;
        }
        
    }
}
