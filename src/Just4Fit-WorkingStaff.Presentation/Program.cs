using System.Reflection;
using Just4Fit_WorkingStaff.Core.Exercises.Repositories;
using Just4Fit_WorkingStaff.Core.Food.Repositories;
using Just4Fit_WorkingStaff.Core.News.Repositories;
using Just4Fit_WorkingStaff.Core.SportSupplements.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Data;
using Just4Fit_WorkingStaff.Infrastructure.Exercises.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Food.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.News.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Repositories;
using Just4Fit_WorkingStaff.Presentation.Options;
using Just4Fit_WorkingStaff.Presentation.Roles.Models;
using Just4Fit_WorkingStaff.Users.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");

var jwtOptions = jwtOptionsSection.Get<JwtOptions>() ?? throw new Exception("Couldn't create jwt options object");

builder.Services.Configure<JwtOptions>(jwtOptionsSection);

var infrastructureAssembly = typeof(JustForFitWorkingStaffDbContext).Assembly;

builder.Services.AddMediatR(configurations =>
{
    configurations.RegisterServicesFromAssembly(infrastructureAssembly);
});

builder.Services.AddScoped<IExerciseRepository, ExerciseSqlRepository>();

builder.Services.AddScoped<IFoodRepository, FoodSqlRepository>();

builder.Services.AddScoped<INewsRepository, NewsSqlRepository>();

builder.Services.AddScoped<ISportSupplementRepository, SportSupplementRepository>();

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("FitnessDb");

builder.Services.AddDbContext<JustForFitWorkingStaffDbContext>(dbContextOptionsBuilder =>
{
    dbContextOptionsBuilder.UseNpgsql(connectionString, o =>
    {
        o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
    });
});

builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = true;
})
    .AddEntityFrameworkStores<JustForFitWorkingStaffDbContext>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    const string scheme = "Bearer";

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "4FitBody (api for working staff)",
        Version = "v1"
    });

    options.AddSecurityDefinition(
        name: scheme,

        new OpenApiSecurityScheme() {
            Description = "Enter here jwt token with Bearer",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = scheme
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement() {
            {
                new OpenApiSecurityScheme() {
                    Reference = new OpenApiReference() {
                        Id = scheme,
                        Type = ReferenceType.SecurityScheme
                    }
                } ,
                new string[] {}
            }
        }
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtOptions.KeyInBytes),

            ValidateLifetime = true,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuer = true,
            ValidIssuers = jwtOptions.Issuers,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options => {
    options.AddPolicy("BlazorWasmPolicy", corsBuilder => {
        corsBuilder
            .WithOrigins("http://localhost:5192")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var dbContext = scope.ServiceProvider.GetRequiredService<JustForFitWorkingStaffDbContext>();
    
    await dbContext.Database.MigrateAsync();
    
    await dbContext.Database.EnsureCreatedAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await roleManager.CreateAsync(new IdentityRole(DefaultRoles.Admin.ToString()));
    
    await roleManager.CreateAsync(new IdentityRole(DefaultRoles.Moderator.ToString()));
    
    await roleManager.CreateAsync(new IdentityRole(DefaultRoles.Trainer.ToString()));
    
    await roleManager.CreateAsync(new IdentityRole(DefaultRoles.Nutritionist.ToString()));
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();