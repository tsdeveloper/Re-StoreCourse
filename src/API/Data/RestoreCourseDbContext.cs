using System.Reflection;
using API.Entities.Roles;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RestoreCourseDbContext : IdentityDbContext<UserCustom, RoleCustom, int>
{
  public RestoreCourseDbContext(DbContextOptions<RestoreCourseDbContext> options)
  : base(options) { }

  protected override void OnModelCreating(ModelBuilder b)
  {
    base.OnModelCreating(b);
    
    b.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    // b.Entity<IdentityRole>()
    //   .HasData(
    //     new List<IdentityRole> {
    //       new IdentityRole("Member"),
    //     new IdentityRole("Admin"),
    //     }
    //   );
  }
}

public static class RestoreCourseDbContextExtensions
{
  public static DbSet<TEntityType> DbSet<TEntityType>(this RestoreCourseDbContext context)
  where TEntityType : class 
  {
    return context.Set<TEntityType>();
  }
}
