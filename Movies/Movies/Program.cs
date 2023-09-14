using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using MovieStore.Core.Models;
using MovieStore.HostedService;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Configurations;
using MovieStore.Infrastructure.Data;
using MovieStore.Repository;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
var initialScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');


builder.Services.AddControllers();

builder.Services.AddDbContext<MovieStoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MovieStoreDB")));

builder.Services.Configure<EmailServiceOptions>(builder.Configuration.GetSection("EmailServiceOptions"));
builder.Services.Configure<BackgroundServiceOptions>(builder.Configuration.GetSection("BackgroundServiceOptions"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepository<PurchasedMovie>, PurchasedMovieRepository>();

builder.Services.AddFluentEmail("vojinb12345@gmail.com")
    .AddRazorRenderer()
    .AddSmtpSender("localhost", 25);

builder.Services.AddScoped<EmailService>();

builder.Services.AddOpenApiDocument(options => options.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

builder.Services.AddHostedService<MovieExpirationService>();


builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi3();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieStoreContext>();

    dbContext.Database.Migrate();
}
Log.Information("App started running");
app.Run();
Log.Information("App stopped running");
