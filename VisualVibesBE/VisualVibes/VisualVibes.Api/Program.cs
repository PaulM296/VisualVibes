using VisualVibes.Api.Extensions;
using VisualVibes.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR();
builder.Services.AddRepositories();
builder.Services.AddDbContext(builder);
builder.Services.AddFileSystemLogger();
builder.Services.AddAutoMapper();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTiming();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();