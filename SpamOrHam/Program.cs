using Microsoft.EntityFrameworkCore;
using SpamOrHam.Data;
using SpamOrHam.Services;
using SpamOrHam.Services.Interfaces;
using SpamOrHam.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(x =>
{
    x.AddDefaultPolicy(o =>
    {
        o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddDbContext<DatabaseContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});
builder.Services.AddSingleton<IClassificationService, ClassificationService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DatabaseContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedDataset(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
