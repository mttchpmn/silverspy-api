using Microsoft.AspNetCore.Authentication.JwtBearer;
using Payments.Services;
using Transactions.Services;
using Database;
using Google.Cloud.Diagnostics.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Google Logging
builder.Services.AddGoogleDiagnosticsForAspNetCore();

// Add domain service registries
TransactionsServiceRegistry.RegisterServices(builder.Services);
PaymentsServiceRegistry.RegisterServices(builder.Services);

// Configure Auth0 Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
        };
    });

// Configure authorization
// builder.Services.AddAuthorization(o =>
//     {
//         o.AddPolicy("todo:read-write", p => p.
//             RequireAuthenticatedUser().
//             RequireClaim("scope", "todo:read-write"));
//     });

// Add CORS policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("_localDevelopment", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000"); // TODO - Update CORS domains
    });
});

// DB MIGRATION CODE - May not work?
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new InvalidOperationException("Env variable 'DATABASE_CONNECTION_STRING' is unset");
var databaseHelper = new DatabaseHelper(connectionString);
var upgradeResult = databaseHelper.MigrateDatabase(connectionString);
Console.WriteLine(upgradeResult);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("_localDevelopment");
}

// Health check endpoint
app.MapGet("/", () => "Kia Ora! Silverspy API online");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();