using EcommerceAPI.SignalR;
using ECommerceAPI.API.Configurations.ColumnWriters;
using ECommerceAPI.API.Extension;
using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();

// Add Storage Services
//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();


//CORS Policy
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>

policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()

));

//Serilog configurations

Logger log = new LoggerConfiguration().WriteTo.Console()
                                      .WriteTo.File("logs/log.txt")
                                      .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
                                                         needAutoCreateTable: true,
                                                         columnOptions: new Dictionary<string, ColumnWriterBase>
                                                         {
                                                             {"message", new RenderedMessageColumnWriter() },
                                                             {"message_template", new MessageTemplateColumnWriter() },
                                                             {"level", new LevelColumnWriter() },
                                                             {"time_stamp", new TimestampColumnWriter() },
                                                             {"exception", new ExceptionColumnWriter() },
                                                             {"log_event",new LogEventSerializedColumnWriter() },
                                                             {"user_name", new UsernameColumnWriter() },

                                       
                                                         })
                                      .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"])
                                      .Enrich.FromLogContext()
                                      .MinimumLevel.Information()
                                      .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddControllers(o => o.Filters.Add<ValidationFilter>())
                                            .AddFluentValidation(conf => conf.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
                                            .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//JWT Settings
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Checks which origins, sites, users can use created token. => www.random.com
            ValidateIssuer = true, // Checks who shares created token => www.myapi.com
            ValidateLifetime = true, // Checks control time limit of created token.
            ValidateIssuerSigningKey = true, // Checks if created token verify that it belongs to our application via security key.


            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            NameClaimType = ClaimTypes.Name, // We can get value from User.Identity.Name property which matches with Name from JWT claim.
        };
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middleware with extension class
app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseStaticFiles();

//Serielog using
app.UseSerilogRequestLogging();

//Htttp logging
app.UseHttpLogging();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


//Serilog Middleware
app.Use(async (context, next) =>
{

    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name",username);

    await next();
});


app.MapControllers();
app.MapHubs();

app.Run();
