
namespace MinimalAPIDemo.Models
{
    public class EmployeeServiceAsync : IEmployeeServiceAsync
    {
        private readonly List<Employee> _employeesList;

        public EmployeeServiceAsync() {
            _employeesList = new List<Employee>()
            {
                new Employee() { Id=1, Name ="Ajay", Position = "Engineer", Salary = 65000 },
                new Employee() { Id=2, Name ="Pradnya", Position = "CA", Salary = 98000 },
            };
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return Task.FromResult<IEnumerable<Employee>>(_employeesList);
        }

        public Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = _employeesList.FirstOrDefault(emp=>emp.Id == id);
            return Task.FromResult(employee);            
        }

        public Task<Employee> AddEmployeeAsync(Employee newEmployee)
        {
            newEmployee.Id = _employeesList.Count > 0 ? _employeesList.Max(emp => emp.Id) + 1 : 1;
            _employeesList.Add(newEmployee);
            return Task.FromResult<Employee>(newEmployee);
        }
        public Task<Employee> UpdateEmployee(int id, Employee updatedEmployee)
        {
            var employee = _employeesList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null) {
                return Task.FromResult<Employee>(null);
            }
            employee.Name = updatedEmployee.Name;
            employee.Salary= updatedEmployee.Salary;
            employee.Position=updatedEmployee.Position;

            return Task.FromResult(employee);            
        }
        public Task<bool> DeleteEmployeeAsync(int id)
        {
            Employee? employee = _employeesList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null) {
                return Task.FromResult<bool>(false);
            }
            _employeesList.Remove(employee);
            return Task.FromResult(true);
        }

    }
}
