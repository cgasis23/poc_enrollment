using EnrollmentApi.DTOs;
using EnrollmentApi.Models;

namespace EnrollmentApi.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CustomerSearchDto? searchDto = null);
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDto?> GetCustomerByEmailAsync(string email);
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createDto);
        Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto updateDto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<bool> CustomerExistsAsync(int id);
        Task<bool> CustomerExistsByEmailAsync(string email);
        Task<int> GetTotalCustomersCountAsync(CustomerSearchDto? searchDto = null);
    }
}
