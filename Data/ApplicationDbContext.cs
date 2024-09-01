using LeaveReportAppBetter.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveReportAppBetter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Leave> Leaves { get; set; }
    }
}
