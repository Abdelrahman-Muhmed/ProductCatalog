using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog_BLL.Helpers.Mapping;
using ProductCatalog_BLL.Helpers.PictureResolver;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.IdentityModel;
using ProductCatalog_DAL.Models.Product;
using ProductCatalog_DAL.Prsistence.Data;
using ProductCatalog_DAL.Repository;
using ProductCatalog_Service.ServiceRepo;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

#region Connection      
//Injection For DataBase 
builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#endregion

#region AlloW Depdancy Injection For Class 

//For Unit Of Work So i dont need Make IGenericRepository 
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));


builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<ProductContext>()
    .AddDefaultTokenProviders();

//For IAuthService 
builder.Services.AddScoped(typeof(IAuthServic), typeof(AuthService));
builder.Services.AddScoped(typeof(IGenericRepository<Products>), typeof(GenericRepository<Products>));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductServic));




builder.Services.AddCors(e => e.AddPolicy("Policy", e => e.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = builder.Configuration["JWT:issure"],
          ValidAudience = builder.Configuration["JWT:audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty)),
          ClockSkew = TimeSpan.Zero
      };

      options.Events = new JwtBearerEvents
      {
          OnMessageReceived = context =>
          {
              if (context.Request.Cookies.ContainsKey("AuthToken"))
              {
                  context.Token = context.Request.Cookies["AuthToken"];
              }
              return Task.CompletedTask;
          }
      };
  });
#endregion

#region Auto Mapping 

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton<PictureUrlResolver>();

#endregion

#region logging 
builder.Logging.ClearProviders();
builder.Services.AddSingleton<ILoggingService, LoggingService>();
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .WriteTo.MSSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection"),
    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true, AutoCreateSqlDatabase = true })
    .WriteTo.Seq("http://localhost:5341/")
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("Policy");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
