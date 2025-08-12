using Microsoft.AspNetCore.Mvc;
using EnrollmentApi.Services;
using EnrollmentApi.DTOs;
using EnrollmentApi.Models;

namespace EnrollmentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get all customers with optional search and pagination
        /// </summary>
        /// <param name="searchDto">Optional search criteria including name, email, phone, status, and date range</param>
        /// <returns>List of customers matching the search criteria with pagination headers</returns>
        /// <response code="200">Returns the list of customers</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(
            [FromQuery] CustomerSearchDto? searchDto)
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync(searchDto);
                var totalCount = await _customerService.GetTotalCustomersCountAsync(searchDto);
                
                Response.Headers["X-Total-Count"] = totalCount.ToString();
                Response.Headers["X-Page"] = (searchDto?.Page ?? 1).ToString();
                Response.Headers["X-PageSize"] = (searchDto?.PageSize ?? 10).ToString();
                
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving customers.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get a customer by ID
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <returns>The customer details</returns>
        /// <response code="200">Returns the requested customer</response>
        /// <response code="404">If the customer was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                    return NotFound(new { error = $"Customer with ID {id} not found." });

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the customer.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get a customer by email
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(string email)
        {
            try
            {
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                    return NotFound(new { error = $"Customer with email {email} not found." });

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the customer.", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="createDto">The customer information to create</param>
        /// <returns>The newly created customer</returns>
        /// <response code="201">Returns the newly created customer</response>
        /// <response code="400">If the customer data is invalid or email already exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var customer = await _customerService.CreateCustomerAsync(createDto);
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while creating the customer.", details = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing customer
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, CustomerUpdateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var customer = await _customerService.UpdateCustomerAsync(id, updateDto);
                if (customer == null)
                    return NotFound(new { error = $"Customer with ID {id} not found." });

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the customer.", details = ex.Message });
            }
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                var deleted = await _customerService.DeleteCustomerAsync(id);
                if (!deleted)
                    return NotFound(new { error = $"Customer with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while deleting the customer.", details = ex.Message });
            }
        }

        /// <summary>
        /// Check if a customer exists
        /// </summary>
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> CustomerExists(int id)
        {
            try
            {
                var exists = await _customerService.CustomerExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while checking customer existence.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get customer statistics
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetCustomerStats()
        {
            try
            {
                var totalCustomers = await _customerService.GetTotalCustomersCountAsync();
                var pendingCustomers = await _customerService.GetTotalCustomersCountAsync(new CustomerSearchDto { Status = EnrollmentStatus.Pending });
                var completedCustomers = await _customerService.GetTotalCustomersCountAsync(new CustomerSearchDto { Status = EnrollmentStatus.Completed });
                var rejectedCustomers = await _customerService.GetTotalCustomersCountAsync(new CustomerSearchDto { Status = EnrollmentStatus.Rejected });

                return Ok(new
                {
                    total = totalCustomers,
                    pending = pendingCustomers,
                    completed = completedCustomers,
                    rejected = rejectedCustomers,
                    inProgress = await _customerService.GetTotalCustomersCountAsync(new CustomerSearchDto { Status = EnrollmentStatus.InProgress })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving customer statistics.", details = ex.Message });
            }
        }
    }
}
