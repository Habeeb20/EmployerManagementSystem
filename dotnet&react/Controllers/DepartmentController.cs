using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_react.Data;
using dotnet_react.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace dotnet_react.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {

        private readonly DataContext _context;

        public DepartmentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult>GetDepartment()
        {
            var department = await _context.Departments.ToListAsync();
            return new JsonResult(department);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if(department == null){
                return NotFound();
            }

            return new JsonResult(department);
            
        }

          [HttpPost]

        public async Task<IActionResult> PostDepartment([FromBody] Department department)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartment), new{id = department.Id}, department);

        }

         [HttpPut]

        public async Task<IActionResult> PutDepartment(int id, [FromBody] Department department)
        {
            if(id != department.Id){
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                if(!_context.Departments.Any(e => e.Id == id))
                {
                    return NotFound();

                }

            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if(department is null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        
    }
}