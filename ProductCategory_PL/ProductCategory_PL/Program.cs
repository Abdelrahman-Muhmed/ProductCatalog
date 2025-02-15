using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog_BLL.Helpers.Mapping;
using ProductCatalog_BLL.Helpers.PictureResolver;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.IdentityModel;
using ProductCatalog_DAL.Models.Product;
using ProductCatalog_DAL.Prsistence.Data;
using ProductCatalog_DAL.Prsistence.Data.DataSeeding;
using ProductCatalog_DAL.Prsistence.Data.UserDataSeeding;
using ProductCatalog_DAL.Prsistence.Repository;
using ProductCatalog_DAL.Repository;
using ProductCatalog_Service.ServiceRepo;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
builder.Services.AddScoped(typeof(IProductService), typeof(ProductServic));
builder.Services.AddScoped(typeof(IGenericRepository<Products>), typeof(GenericRepository<Products>));
builder.Services.AddScoped(typeof(IGenericRepository<ProductBrand>), typeof(GenericRepository<ProductBrand>));
builder.Services.AddScoped(typeof(IGenericRepository<ProductCategory>), typeof(GenericRepository<ProductCategory>));

builder.Services.AddScoped(typeof(IProductRepo), typeof(ProductRepo));



builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
	.AddEntityFrameworkStores<ProductContext>()
	.AddDefaultTokenProviders();

// Configure Identity to use custom ApplicationUser and ApplicationRole with int keys
//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ProductContext>()
//    .AddDefaultTokenProviders();
//For IAuthService 
builder.Services.AddScoped(typeof(IAuthServic), typeof(AuthService));


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

//Authrization 
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
	options.AddPolicy("Student", policy => policy.RequireRole("Student"));
});
#endregion

#region Auto Mapping 
//builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));
//builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register AutoMapper and PictureUrlResolver
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton<PictureUrlResolver>();

#endregion

#region logging 
builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Services.AddSingleton<ILoggingService, LoggingService>();
builder.Services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
	.WriteTo.Console()
	.WriteTo.MSSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection"),
	sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true, AutoCreateSqlDatabase = true })
	.WriteTo.Seq("http://localhost:7063/")
	.CreateLogger();

builder.Host.UseSerilog();

#endregion


var app = builder.Build();

#region Seeding Data 
//Ask CLR For Creating Object From DBContext 
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<ProductContext>();
var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();


var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{

	// Migrate Data 
	await dbContext.Database.MigrateAsync();
	// Seeding Data To Make Admin User 
	var userId = await UserSeeding.UserDataSeeding(userManager, roleManager);
	if (userId.HasValue)
	{
		var creationDate = DateTime.UtcNow;
		await ProductsSeeding.SeedAsync(dbContext, userId.Value, creationDate);
	}
}
catch (Exception ex)
{
	var logger = loggerFactory.CreateLogger<Program>();
	logger.LogError(ex, "An error happened during migration");
}
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

app.UseAuthentication();

app.UseAuthorization();
app.UseCors("Policy");
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
