using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ExportApp.Context;
using ExportApp.Middlewares;
using ExportApp.Models;
using ExportApp.Repositories.EmployeeDetails;
using ExportApp.Services.Employee;
using ExportApp.Services.User;
using System.Text;
using ExportApp.Services.Login;

var builder = WebApplication.CreateBuilder(args);
var configManager = builder.Configuration;

// Add services for controllers to the specified IServiceCollection.
builder.Services.AddControllers();
// builder.Services.AddDbContext<CompanyContext>();
builder.Services.AddDbContext<CompanyDBContext>(options => options.UseSqlServer(configManager.GetConnectionString("CompanyDBConnectionString")));
// builder.Services.AddSingleton<IUserService, UserService>();
// builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<RequestLoggingMiddleware>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmployeeDetailsRepository, EmployeeDetailsRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = 307;
    options.HttpsPort = 443;
});

// builder.Services.AddCors();
// builder.Services.AddCors(options =>
// {
//    options.AddPolicy(name: "MyCorsPolicy",
//                      corsPolicyBuilder =>
//                      {
//                          corsPolicyBuilder.WithOrigins("http://localhost:4200").WithMethods().WithHeaders();
//                      });
// });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(configOptions =>
{
    // configOptions.RequireHttpsMetadata = false; // By default, this is true
    configOptions.SaveToken = true;
    configOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = configManager["Jwt:Audience"],
        ValidIssuer = configManager["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configManager["Jwt:Key"]))
    };
});
// builder.Services.AddAuthorization();

var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Hello from 1st delegate.");
//    await next();
//    // RunExtensions.Run(app, next);
//});

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Hello from Central delegate.");
//    await next();
//});

//app.Map("/auth", app =>
//{
//    app.Run(async (conext) =>
//    {
//        await conext.Response.WriteAsync("New branch for auth");
//    });
//});

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Hello from 2nd Middleware.");
//});

//// This will never execute
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Hello from 3rd Middleware");
//});

// app.UseCors();
// app.UseCors("MyCorsPolicy");
app.UseCors(corsPolicyBuilder =>
       corsPolicyBuilder
       .WithOrigins("http://localhost:4200", "http://localhost:3000")
       .AllowAnyMethod()
       .AllowAnyHeader());

//// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(config =>
{
    config.MapGet("/sendMessage", () => "Hello World");
});

app.UseEndpoints(config =>
{
    config.MapGet("/sendMessage1", () => "How are you ?....Ritesh ");
});

app.UseEndpoints(config =>
{
    config.MapPost("/SaveCustomerInfo", (UserDetails userDetails) =>
    {
        // do your post/submit process
        return userDetails;
    });
});

app.UseEndpoints(config =>
{
    config.MapPut("/UpdateCustomerInfo", (int id, UserDetails userDetails) =>
    {
        // do your put/update process
        return userDetails;
    });
});

app.UseEndpoints(config =>
{
    config.MapDelete("/DeleteCustomerInfo", (int id) =>
    {
        // do your delete process
        return id;
    });
});

app.MapMethods("/Customers", ["GET", "POST"], (HttpContext httpContext) => // works with both Get and Post Requests and route '/Customers' 
{
    return new { list = new string[] { "Ritesh", "Rohan" } };
});

app.MapControllers();
// app.MapControllerRoute(name: "DefaultRoute", pattern: "api/{controller=Test}/{id?}");
app.MapControllerRoute(name: "DefaultRoute1", pattern: "api/{controller=Test}/{action}/{id?}");

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello from 1st delegate.");
    await next();
    // RunExtensions.Run(app, next);
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello from Central delegate.");
    await next();
});

app.Map("/auth", app =>
{
    app.Run(async (conext) =>
    {
        await conext.Response.WriteAsync("New branch for auth");
    });
});

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello from 2nd Middleware");
});

// This will never execute
app.Run(async (context) =>
{
    await context.Response.WriteAsync("Hello from 3rd Middleware");
});

app.Run();
