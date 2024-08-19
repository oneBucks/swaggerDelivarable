using Microsoft.EntityFrameworkCore;
using Company;

class CompanyDb : DbContext
{
    public CompanyDb(DbContextOptions<CompanyDb> options)
        : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Car> Cars => Set<Car>();
}