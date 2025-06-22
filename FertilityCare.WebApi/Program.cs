using Fertilitycare.Share.Models;
using FertilityCare.Infrastructure.Configurations;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Infrastructure.Repositories;
using FertilityCare.Infrastructure.Services;
using FertilityCare.UseCase.Implements;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace FertilityCare.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.Configure<GoogleAuthConfiguration>(builder.Configuration.GetSection("GoogleAuth"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClient", policy =>
                {
                    policy.WithOrigins("http://localhost:5174", "http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddDbContext<FertilityCareDBContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                           .UseLazyLoadingProxies());

            var jwtConfig = builder.Configuration.GetSection(JwtConfiguration.SectionName).Get<JwtConfiguration>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<FertilityCareDBContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer,
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();
            builder.Services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<ISlotRepository, SlotRepository>();
            builder.Services.AddScoped<ITreatmentServiceRepository, TreatmentServiceRepository>();
            builder.Services.AddScoped<ITreatmentStepRepository, TreatmentStepRepository>();
            builder.Services.AddScoped<IPublicTreatmentService, PublicTreatmentService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IOrderStepRepository, OrderStepRepository>();
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderStepService, OrderStepService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPatientSecretService, PatientSecretService>();
            builder.Services.AddScoped<IEggGainedRepository, EggGainedRepository>();
            builder.Services.AddScoped<IEggGainedService, EggGainedService>();
            builder.Services.AddScoped<IEmbryoGainedRepository, EmbryoGainedRepository>();
            builder.Services.AddScoped<IEmbryoGainedService, EmbryoGainedService>();
            builder.Services.AddScoped<IEmbryoTransferRepository, EmbryoTransferRepository>();
            builder.Services.AddScoped<IEmbryoTransferService, EmbryoTransferService>();



            builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                await SeedRolesAsync(roleManager);
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowClient");
            //app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            await app.RunAsync();
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = new[] { "User", "Admin", "Doctor", "Patient" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                }
            }
        }
    }
}
