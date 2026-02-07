using InTicket.Domain;
using InTicket.Persistence;
using InTicket.Application;
using InTicket.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(setupAction =>
{
    setupAction.InvalidModelStateResponseFactory = context =>
    {
        var problemDetailsFactory = context.HttpContext.RequestServices
            .GetRequiredService<ProblemDetailsFactory>();
        var validationProblemDetails = problemDetailsFactory
            .CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState);
        validationProblemDetails.Detail =
            "See the errors field for details.";
        validationProblemDetails.Instance =
            context.HttpContext.Request.Path;
        validationProblemDetails.Status =
            StatusCodes.Status422UnprocessableEntity;
        validationProblemDetails.Title =
            "One or more validation errors occurred.";
        return new UnprocessableEntityObjectResult(
            validationProblemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});
;
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