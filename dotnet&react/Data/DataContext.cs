using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_react.Models;
using Microsoft.EntityFrameworkCore;
namespace dotnet_react.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

          public DbSet<Department> Departments{get; set;}
          public DbSet<Employee> Employees{get; set;}
    }
}