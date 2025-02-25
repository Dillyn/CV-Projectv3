using Domain.Models;
using FirstAPI.Authentication.Contracts;
using FirstAPI.Authentication.Services;
using Infrastructure.Data;
using Integration.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<UserI, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<ApplicationDBcontext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
//    {
//        Description = "The API Key to access the API",
//        Type = SecuritySchemeType.ApiKey,
//        Name = "x-api-key",
//        In = ParameterLocation.Header,
//        Scheme = "ApiKeyScheme"
//    });
//    var scheme = new OpenApiSecurityScheme
//    {
//        Reference = new OpenApiReference
//        {
//            Type = ReferenceType.SecurityScheme,
//            Id = "ApiKey"
//        },
//        In = ParameterLocation.Header
//    };
//    var requirement = new OpenApiSecurityRequirement
//    {
//             {scheme, new List<string>() }
//    };
//    c.AddSecurityRequirement(requirement);
//});
builder.Services.AddSwaggerGen(c =>
{
    // Define the security scheme for JWT Bearer authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT Bearer token",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",  // "bearer" is used for JWT tokens
        BearerFormat = "JWT",  // Optional: Specifies the format of the token
        In = ParameterLocation.Header // Token will be passed in the header
    });

    // Create the security requirement that refers to the JWT Bearer token scheme
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer" // This refers to the scheme we defined above
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement
    {
        {scheme, new List<string>()}
    };

    // Add the security requirement to the Swagger configuration
    c.AddSecurityRequirement(requirement);
});

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


builder.Services.AddScoped<GIRepository, Repository>();
builder.Services.AddScoped<GUserService>();

//Add db context and configure it
builder.Services.AddDbContext<ApplicationDBcontext>(Options => { Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

//app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
