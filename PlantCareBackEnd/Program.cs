using Microsoft.EntityFrameworkCore;
using PlantCareBackEnd.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PlantDbContext>(options =>
{
    options.UseInMemoryDatabase("PlantDatabase");
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowMyOrigin",
           policy => policy.WithOrigins("http://localhost:3000") // Url frontend app plant care
                           .AllowAnyHeader()
                           .AllowAnyMethod());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aquí aplicamos la política CORS
app.UseCors("AllowMyOrigin"); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
