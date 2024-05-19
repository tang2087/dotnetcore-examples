using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace ef_migrations;

/// <summary>
/// Blog entity
/// </summary>
public class Blog
{
    [Key]
    public int BlogId { get; set; }

    [Required]
    [MaxLength(128)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(256)]
    public required string SubTitle { get; set; }

    [MaxLength(8000)]
    public string? Content { get; set; }

    [Required]
    public DateTime DateTimeAdd { get; set; }
}

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map table names
        modelBuilder.Entity<Blog>().ToTable("Blogs", "test");
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId);
            entity.HasIndex(e => e.Title).IsUnique();
            entity.Property(e => e.DateTimeAdd).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        base.OnModelCreating(modelBuilder);
    }
}