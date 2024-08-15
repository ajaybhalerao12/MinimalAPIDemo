namespace MinimalAPIDemo.Models
{
    public interface IEmployeeServiceAsync
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);

        Task<Employee> AddEmployeeAsync(Employee newEmployee);

        Task<Employee> UpdateEmployee(int id, Employee updatedEmployee);

        Task<bool> DeleteEmployeeAsync(int id);
    }
}
