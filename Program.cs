
using LeaveReportAppBetter.Data;
using LeaveReportAppBetter.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveReportAppBetter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Konfigurerar en HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            // Return all employees
            app.MapGet("/employees", async (ApplicationDbContext context) =>
            {
                var employees = await context.Employees.ToListAsync();
                if (employees == null || !employees.Any())
                {
                    return Results.NotFound("Hittade inga employees");
                }
                return Results.Ok(employees);
            });

            // Skapar en ny anstäld
            app.MapPost("/employees", async (Employee employee, ApplicationDbContext context) =>
            {
                context.Employees.Add(employee);
                await context.SaveChangesAsync();
                return Results.Created($"/employees/{employee.EmployeeId}", employee);
            });
            // Hämtar ett anställnings ID
            app.MapGet("/employees/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var employee = await context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return Results.NotFound("Employee not found");
                }
                return Results.Ok(employee);
            });
            // Modifierar en anställd
            app.MapPut("/employees/{id:int}", async (int id, Employee updatedEmployee, ApplicationDbContext context) =>
            {
                var employee = await context.Employees.FindAsync(id);

                if (employee == null)
                {
                    return Results.NotFound("Employee not found");
                }
                employee.EmployeeName = updatedEmployee.EmployeeName;
                employee.Email = updatedEmployee.Email;
                await context.SaveChangesAsync();
                return Results.Ok(employee);
            });
            // tarbort en anställd
            app.MapDelete("/employees/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var employee = await context.Employees.FindAsync(id);

                if (employee == null)
                {
                    return Results.NotFound("Employee not found");
                }
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
                return Results.Ok($"Employee with ID: {id} deleted");
            });
            //returnerar ledigheter
            app.MapGet("/leaves", async (ApplicationDbContext context) =>
            {
                var leaves = await context.Leaves.Include(e => e.Employee).ToListAsync();
                if (leaves == null || !leaves.Any())
                {
                    return Results.NotFound("Hittade inga leaves");
                }
                return Results.Ok(leaves);
            });

            // skapar en ny ledighet
            app.MapPost("/leaves", async (Leave leave, ApplicationDbContext context) =>
            {
                context.Leaves.Add(leave);
                await context.SaveChangesAsync();
                return Results.Created($"/leaves/{leave.LeaveId}", leave);
            });
            // hämtar en ledighet med ID
            app.MapGet("/leaves/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var leave = await context.Leaves.FindAsync(id);
                if (leave == null)
                {
                    return Results.NotFound("Leave not found");
                }
                return Results.Ok(leave);
            });
            // Uppdaterar en ledighet
            app.MapPut("/leaves/{id:int}", async (int id, Leave updatedLeave, ApplicationDbContext context) =>
            {
                var leave = await context.Leaves.FindAsync(id);

                if (leave == null)
                {
                    return Results.NotFound("Leave not found");
                }
                leave.StartDate = updatedLeave.StartDate;
                leave.EndDate = updatedLeave.EndDate;
                leave.Type = updatedLeave.Type;
                leave.Status = updatedLeave.Status;
                leave.FkEmployeeId = updatedLeave.FkEmployeeId;
                await context.SaveChangesAsync();
                return Results.Ok(leave);
            });
            // tarbort en ledighet
            app.MapDelete("/leaves/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var leave = await context.Leaves.FindAsync(id);

                if (leave == null)
                {
                    return Results.NotFound("Leave not found");
                }
                context.Leaves.Remove(leave);
                await context.SaveChangesAsync();
                return Results.Ok($"Leave with ID: {id} deleted");
            });
            app.Run();
        }
    }
}
