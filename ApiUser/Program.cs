using ApiUser.Custom;
using Dominio.Entidades;
using Dominio.Puertos.Primarios;
using Dominio.Puertos.Secundarios;
using Dominio.Services;
using Infrastructura;
using Infrastructura.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                                                        builder.Configuration.GetConnectionString("Cadena");

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddSingleton<Utils>();

builder.Services.AddTransient<ICommonRepository<Usuario>,UserRepository>();
builder.Services.AddTransient<ICommonServices<Usuario>,ServiciosUsuario>();
builder.Services.AddTransient<ICuentaRepositorio, CuentaRepositorio>();
builder.Services.AddTransient<ICuentaServicios, ServicioCuenta>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };

    // Asegúrate de que el token JWT se lea desde las cookies
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Lee el token JWT de la cookie "jwt_token"
            if (context.Request.Cookies.ContainsKey("jwt"))
            {
                context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
    });
});



var app = builder.Build();

using(var scope= app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();
    context.Database.Migrate();
  
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

 

app.Run();
