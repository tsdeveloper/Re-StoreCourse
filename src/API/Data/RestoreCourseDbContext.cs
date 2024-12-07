using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RestoreCourseDbContext : DbContext
{
  public RestoreCourseDbContext(DbContextOptions<RestoreCourseDbContext> options)
  : base(options) { }

  protected override void OnModelCreating(ModelBuilder b)
  {
    base.OnModelCreating(b);
    b.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
