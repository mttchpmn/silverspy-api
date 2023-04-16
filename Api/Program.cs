using Database;
using Google.Cloud.Diagnostics.AspNetCore3;
using Google.Cloud.Diagnostics.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Payments.Services;
using Transactions.Services;

var builder = WebApplication.CreateBuilder(args);

// Register Google Logging
var env = Environment.GetEnvironmentVariable("ENVIRONMENT");
if (env == "PRODUCTION")
{
    builder.Services.AddGoogleDiagnosticsForAspNetCore();
    builder.Services.AddGoogleErrorReportingForAspNetCore();

    builder.Logging.AddGoogle();
}

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add domain service registries
TransactionsServiceRegistry.RegisterServices(builder.Services);
PaymentsServiceRegistry.RegisterServices(builder.Services);

// Configure Auth0 Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
        };
    });

// Add CORS policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("_localDevelopment", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000"); // TODO - Update CORS domains
    });
});

// DB MIGRATION CODE - May not work?
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ??
                       throw new InvalidOperationException("Env variable 'DATABASE_CONNECTION_STRING' is unset");
var databaseHelper = new DatabaseHelper(connectionString);
var upgradeResult = databaseHelper.MigrateDatabase(connectionString);
Console.WriteLine(upgradeResult);

var app = builder.Build();

app.Logger.LogInformation("LOG: App running");
Console.WriteLine("CONSOLE: App running");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("_localDevelopment");
}

// Health check endpoint
app.MapGet("/", () => "Kia Ora! Silverspy API online");
app.MapGet("/log", () =>
{
    Console.WriteLine("CONSOLE LOG");
    return "DONE";
});

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();