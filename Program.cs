using Api.Data;
using Api.Helpers;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<APIDbContext>(option =>
{
    //var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? builder.Configuration.GetConnectionString("DefaultConnection");

    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var JwtOptions = builder.Configuration.GetSection("Jwt").Get<jwtOptions>();
builder.Services.Configure<jwtOptions>(builder.Configuration.GetSection("Jwt"));


if (string.IsNullOrWhiteSpace(JwtOptions?.Key))
{
    new InvalidOperationException("Jwt Key is not configured. Please set key in appsttings.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options =>
{
    Options.IncludeErrorDetails = true;

    Options.MapInboundClaims = false;

    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = JwtOptions?.Issuer,
        
        ValidateAudience = true,
        ValidAudience = JwtOptions!.Audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions?.Key)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30),

        RoleClaimType = "role",
        NameClaimType = "name"
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();

app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<APIDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}