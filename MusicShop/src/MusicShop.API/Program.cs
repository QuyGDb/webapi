using MusicShop.Infrastructure;
using MusicShop.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Setup Logging (Serilog) - Detailed configuration to follow
builder.Host.UseSerilog((context, logger) => 
    logger.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

// 2. Add services to the container
builder.Services.AddControllers(); // Necessary for using [ApiController]
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Register Clean Architecture Layers
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use Authentication & Authorization (Detailed configuration in the next step)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Map controllers automatically

app.Run();

