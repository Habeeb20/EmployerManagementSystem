using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_react.Data;
using dotnet_react.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;

namespace dotnet_react.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(DataContext context, IWebHostEnvironment env) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly IWebHostEnvironment _env = env;
         [HttpGet]

         public async Task<IActionResult> GetEmployee ()
         {
            var employee = await _context.Employees.ToListAsync();
            return new JsonResult(employee);
          
         }

        [HttpGet("{id}")]

         public async Task<IActionResult> GetEmployeeId (int id)
         {
            var employee = await _context.Employees.FindAsync(id);
            if(employee is null)
            {
                return NotFound();
            }
            return new JsonResult(employee);
          
         }

         [HttpPost]

         public async Task<IActionResult> PostEmployee([FromBody]Employee employees )
         {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            };

            _context.Employees.Add(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new{id = employees.EmployeeId}, employees);
          
         }

         [HttpPut]

         public async Task<IActionResult> UpdateEmployee (int id, [FromBody] Employee employee)
         {
            if(id !=employee.EmployeeId )
            {
                return NotFound();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                if(!_context.Employees.Any(e => e.EmployeeId == id)){
                    return NotFound();
                }

            }
            return NoContent();
         }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteEmployee (int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee is null)
            {
                return NotFound(employee);
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Route("Savefile")]
        [HttpPost]

    //     public async Task<IActionResult> UploadPhoto([FromForm] IFormFile photo)
    // {
    //     if (photo == null || photo.Length == 0)
    //         return BadRequest("No photo uploaded.");

    //     string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
    //     if (!Directory.Exists(uploadsFolder))
    //     {
    //         Directory.CreateDirectory(uploadsFolder);
    //     }

    //     string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
    //     string filePath = Path.Combine(uploadsFolder, uniqueFileName);

    //     using (var stream = new FileStream(filePath, FileMode.Create))
    //     {
    //         await photo.CopyToAsync(stream);
    //     }

    //     var newPhoto = new Photo
    //     {
    //         FileName = uniqueFileName,
    //         FilePath = Path.Combine("uploads", uniqueFileName)
    //     };

    //     _context.Employees.Add(newPhoto);
    //     await _context.SaveChangesAsync();

    //     return Ok(new { newPhoto.FilePath });
    // }

    //     private class Photo : Employee
    //     {
    //         public string FileName { get; set; }
    //         public string FilePath { get; set; }
    //     }




        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var PhysicalPath = env.ContentRootPath + "/photos/" + fileName;

                using (var stream = new FileStream(PhysicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (System.Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }






    }

}