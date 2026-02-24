using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using System.Data.SqlClient;
using WardDapperMVC.DataAccess;
using WardDapperMVC.Repository;
using WardDapperMVC.Repository.Nusrse;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // Path to your login page
        options.LogoutPath = "/Login/Logout"; // Path to your logout page
        options.AccessDeniedPath = "/Home/Index"; // Path to your access denied page

        options.SlidingExpiration = true; // Enable sliding expiration
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set the expiration time if needed
        options.Cookie.HttpOnly = true; // Recommended for security
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Secure cookies in production
    });



// Register repositories and data access
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
//Main Administrator
builder.Services.AddTransient<IPatientRepository, PatientRepository>();
builder.Services.AddTransient<IMedicationRepository, MedicationRepository>();
builder.Services.AddTransient<IConditionRepository, ConditionRepository>();
builder.Services.AddTransient<IAllergyRepository, AllergyRepository>();
builder.Services.AddTransient<IWardRepository, WardRepository>();
builder.Services.AddTransient<IHospitalInformationRepository, HospitalInformationRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IBedRepository, BedRepository>();
builder.Services.AddTransient<IConsumableRepository, ConsumableRepository>();
builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();

//WardAdmin
builder.Services.AddTransient<IPatientFolderRepository, PatientFolderRepository>();
builder.Services.AddTransient<IDischargePatientRepository, DischargePatientRepository>();
builder.Services.AddTransient<IPatientMovementRepository, PatientMovementRepository>();

//Login
builder.Services.AddScoped<ILoginRepo, LoginRepo>();

// Nurse.
builder.Services.AddTransient<IPatientVitalRepo, PatientVitalRepo>();
builder.Services.AddTransient<IMedAdministrationRepo, MedAdministrationRepo>();
builder.Services.AddTransient<IDoctorAdviceRepo, DoctorAdviceRepo>();
builder.Services.AddTransient<ITreatPatientRepo, TreatPatientRepo>();
builder.Services.AddTransient<IPatientRepo, PatientRepo>();
builder.Services.AddTransient<IUserRepo, UserRepo>();
//Doctor Portal SERVICES
builder.Services.AddTransient<IPatientReferralsRepository, ReferralsRepository>();
builder.Services.AddTransient<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddTransient<IVisitScheduleRepository, VisitScheduleRepository>();
builder.Services.AddTransient<IPatientInstructionRepository, PatientInstructionRepository>();

// Register IDbConnection with a specific implementation (SqlConnection here)
builder.Services.AddTransient<IDbConnection>(sp => new SqlConnection(builder.Configuration.GetConnectionString("conn")));


var app = builder.Build();

//new here for reports


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

app.UseAuthentication(); // Ensure authentication is used before authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
