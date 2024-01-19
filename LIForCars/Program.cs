using LIForCars.Data;
using LIForCars.Data.Components;
using LIForCars.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MyLIForCarsDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyLIForCarsDBConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped<ICoworkerRepository, CoworkerRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create the database if it does not exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyLIForCarsDBContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();