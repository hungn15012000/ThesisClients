using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiddleWare.Configuration;
using MiddleWare.Data;
using MiddleWare.Extensions;
using MiddleWare.Models.OptionsModels;
using MiddleWare.OPCUALayer;
using Serilog;
using System.Net.Sockets;
using System.Text;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MiddlewareDbConnection");
builder.Services.AddDbContext<MiddleWare_dbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MiddleWare_dbContext>();

builder.Services.AddAutoMapper(typeof(MapperConfiguration));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx,lc)=>
{
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
});
builder.Services.AddCors( options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});

//JSON Serializer
builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles)
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreReadOnlyProperties = true);
builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new CustomJsonConverterForType());
            });

builder.Services.AddOptions();

builder.Services.Configure<OPCUAServersOptions>(builder.Configuration.GetSection("OPCUAServersOptions"));


// Register a singleton service managing OPC UA interactions
builder.Services.AddSingleton<IUaClientSingleton, UaClient>();
//

builder.Services.AddSignalR();

builder.Services.AddAuthentication( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    
}

app.UseHttpsRedirection();

<<<<<<< Updated upstream
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:1446"));
=======
app.UseStaticFiles();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:5002,https://localhost:5001,https://localhost:5000,https://3.0.104.115:5000"));
>>>>>>> Stashed changes

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.UseResponseCompression();

app.Run();
