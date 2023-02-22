using DTOExemple.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DTOExemple.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeWriteDTO>> Create(EmployeeWriteDTO employeeDTO)
        {
            var employee = new Employee
            {
                UserName = employeeDTO.UserName,
                Email = employeeDTO.Email,
                Password = employeeDTO.Password
            };

            _context.Employee.Add(employee);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, WriteMapper(employee));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeReadDTO>> GetById(long id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
                return NotFound();

            return ReadMapper(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeReadDTO>>> GetAll()
        {
            return await _context.Employee
                .Select(x => ReadMapper(x))
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, EmployeeWriteDTO employeeDTO)
        {
            if (id != employeeDTO.Id)
            {
                return BadRequest();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.UserName = employeeDTO.UserName;
            employee.Email = employeeDTO.Email;
            employee.Password = employeeDTO.Password;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!EmployeeExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(long id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
        private static EmployeeReadDTO ReadMapper(Employee employee)
        {
            return new EmployeeReadDTO
            {
                Id = employee.Id,
                UserName = employee.UserName,
                Email = employee.Email,
            };
        }

        private static EmployeeWriteDTO WriteMapper(Employee employee)
        {
            return new EmployeeWriteDTO
            {
                Id = employee.Id,
                UserName = employee.UserName,
                Email = employee.Email,
                Password = employee.Password
            };
        }
    }
}