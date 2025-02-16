using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence;

class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<User>(entity => {
            entity.HasIndex(e => e.Id).IsUnique();
            entity.Property(t => t.Name).IsRequired();
        });

        base.OnModelCreating(builder);
    }
}
