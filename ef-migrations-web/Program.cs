using System.Reflection;
using ef_migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlite("Data Source=app.db", b=>{
    b.MigrationsAssembly(Assembly.GetAssembly(typeof(ef_migrations.Migrations.InitMigrations)).FullName);
}));
var app = builder.Build();

app.MapGet("/blogs", async (MyDbContext dbContext) =>
{
    var blogs = await dbContext.Blogs.ToListAsync();
    return Results.Ok(blogs);
});

app.MapGet("/", () => "Hello World!");

app.Run();
