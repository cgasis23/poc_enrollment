using Microsoft.EntityFrameworkCore;
using EnrollmentApi.Data;
using EnrollmentApi.DTOs;
using EnrollmentApi.Models;

namespace EnrollmentApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly EnrollmentDbContext _context;

        public CustomerService(EnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CustomerSearchDto? searchDto = null)
        {
            var query = _context.Customers.AsQueryable();

            if (searchDto != null)
            {
                if (!string.IsNullOrEmpty(searchDto.FirstName))
                    query = query.Where(c => c.FirstName.Contains(searchDto.FirstName));

                if (!string.IsNullOrEmpty(searchDto.LastName))
                    query = query.Where(c => c.LastName.Contains(searchDto.LastName));

                if (!string.IsNullOrEmpty(searchDto.Email))
                    query = query.Where(c => c.Email.Contains(searchDto.Email));

                if (!string.IsNullOrEmpty(searchDto.PhoneNumber))
                    query = query.Where(c => c.PhoneNumber.Contains(searchDto.PhoneNumber));

                if (searchDto.Status.HasValue)
                    query = query.Where(c => c.Status == searchDto.Status.Value);

                if (searchDto.CreatedFrom.HasValue)
                    query = query.Where(c => c.CreatedAt >= searchDto.CreatedFrom.Value);

                if (searchDto.CreatedTo.HasValue)
                    query = query.Where(c => c.CreatedAt <= searchDto.CreatedTo.Value);
            }

            // Apply pagination
            if (searchDto != null)
            {
                query = query.Skip((searchDto.Page - 1) * searchDto.PageSize)
                            .Take(searchDto.PageSize);
            }

            var customers = await query.ToListAsync();
            return customers.Select(MapToDto);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer != null ? MapToDto(customer) : null;
        }

        public async Task<CustomerDto?> GetCustomerByEmailAsync(string email)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);
            return customer != null ? MapToDto(customer) : null;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createDto)
        {
            // Check if customer with same email already exists
            if (await CustomerExistsByEmailAsync(createDto.Email))
                throw new InvalidOperationException($"Customer with email {createDto.Email} already exists.");

            var customer = new Customer
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                PhoneNumber = createDto.PhoneNumber,
                Address = createDto.Address,
                City = createDto.City,
                State = createDto.State,
                ZipCode = createDto.ZipCode,
                Country = createDto.Country,
                DateOfBirth = createDto.DateOfBirth,
                Ssn = createDto.Ssn,
                Status = EnrollmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return MapToDto(customer);
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto updateDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return null;

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateDto.FirstName))
                customer.FirstName = updateDto.FirstName;

            if (!string.IsNullOrEmpty(updateDto.LastName))
                customer.LastName = updateDto.LastName;

            if (!string.IsNullOrEmpty(updateDto.PhoneNumber))
                customer.PhoneNumber = updateDto.PhoneNumber;

            if (updateDto.Address != null)
                customer.Address = updateDto.Address;

            if (updateDto.City != null)
                customer.City = updateDto.City;

            if (updateDto.State != null)
                customer.State = updateDto.State;

            if (updateDto.ZipCode != null)
                customer.ZipCode = updateDto.ZipCode;

            if (updateDto.Country != null)
                customer.Country = updateDto.Country;

            if (updateDto.DateOfBirth.HasValue)
                customer.DateOfBirth = updateDto.DateOfBirth.Value;

            if (updateDto.Ssn != null)
                customer.Ssn = updateDto.Ssn;

            if (updateDto.Status.HasValue)
                customer.Status = updateDto.Status.Value;

            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CustomerExistsByEmailAsync(string email)
        {
            return await _context.Customers.AnyAsync(c => c.Email == email);
        }

        public async Task<int> GetTotalCustomersCountAsync(CustomerSearchDto? searchDto = null)
        {
            var query = _context.Customers.AsQueryable();

            if (searchDto != null)
            {
                if (!string.IsNullOrEmpty(searchDto.FirstName))
                    query = query.Where(c => c.FirstName.Contains(searchDto.FirstName));

                if (!string.IsNullOrEmpty(searchDto.LastName))
                    query = query.Where(c => c.LastName.Contains(searchDto.LastName));

                if (!string.IsNullOrEmpty(searchDto.Email))
                    query = query.Where(c => c.Email.Contains(searchDto.Email));

                if (!string.IsNullOrEmpty(searchDto.PhoneNumber))
                    query = query.Where(c => c.PhoneNumber.Contains(searchDto.PhoneNumber));

                if (searchDto.Status.HasValue)
                    query = query.Where(c => c.Status == searchDto.Status.Value);

                if (searchDto.CreatedFrom.HasValue)
                    query = query.Where(c => c.CreatedAt >= searchDto.CreatedFrom.Value);

                if (searchDto.CreatedTo.HasValue)
                    query = query.Where(c => c.CreatedAt <= searchDto.CreatedTo.Value);
            }

            return await query.CountAsync();
        }

        private static CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                ZipCode = customer.ZipCode,
                Country = customer.Country,
                DateOfBirth = customer.DateOfBirth,
                Status = customer.Status,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt,
                IsMfaEnabled = customer.IsMfaEnabled,
                MfaEnabledAt = customer.MfaEnabledAt
            };
        }
    }
}
