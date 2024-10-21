using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using finance_control_server.Context;
using finance_control_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using AutoMapper;
using finance_control_server.DTOs.Mappings;
using finance_control_server.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("CONEXAO_BANCO");

builder
    .Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));

builder
    .Services.AddIdentity<Usuario, Cargo>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration[Environment.GetEnvironmentVariable("JWT_KEY")!]!)
            )
        });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<SignInManager<Usuario>>();
builder.Services.AddScoped<UserManager<Usuario>>();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddScoped<CargoUtils>();
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<MailServices>();
builder.Services.AddScoped<ContatoServices>();

builder
    .Services.AddControllers()
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
          options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
          options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

builder
    .Services.AddCors(options =>
        {
          options.AddPolicy("AngularCors",
              builder => builder
                  .WithOrigins("http://localhost:4200",
                    "https://portfolio.jasmim.dev",
                    "https://www.jasmim.dev")
                  .AllowAnyHeader()
                  .AllowAnyMethod());
        });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Exemplo: \'Bearer 12345abcdef\'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.CreateCargos();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("AngularCors");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();