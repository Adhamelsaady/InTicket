using InTicket.Domain;
using InTicket.Persistence;
using InTicket.Application;
using InTicket.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InTicketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddInfrastructure();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();