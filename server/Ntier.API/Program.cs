using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ntier.API;
using Ntier.BLL;
using Ntier.DAL;
using Ntier.DAL.Context;
using Ntier.DAL.Entities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder =>
//        {
//            builder.WithOrigins("https://miki-jewelry-ochre.vercel.app")
//                   .AllowAnyHeader()
//                   .AllowAnyMethod()
//                   .AllowCredentials();
//        });
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL"), b => b.MigrationsAssembly("Ntier.API"));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ShopContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        //khong su dung dich vu ngoai
        ValidateIssuer = false,
        ValidateAudience = false,
        //validate key
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});
builder.Services.RegisterDALDependencies(builder.Configuration);
builder.Services.RegisterBLLDependencies(builder.Configuration);
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
await app.MigrateData();
app.MapControllers();

app.Run();
