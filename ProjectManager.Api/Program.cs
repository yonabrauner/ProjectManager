using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProjectManager.Api.Data;
using ProjectManager.Api.Helpers;
using ProjectManager.Api.Services;


try
{
    var builder = WebApplication.CreateBuilder(args);

    // ----------------------------
    // Services
    // ----------------------------
    builder.Services.AddControllers();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                "https://your-frontend.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    // DbContext
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
                          ?? "Data Source=mini_project_manager.db"));

    // Scoped services
    // builder.Services.AddScoped<IAuthService, AuthService>();
    // builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();

    // JWT Auth
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = JwtHelper.GetTokenValidationParameters(builder.Configuration);
    });

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mini Project Manager API", Version = "v1" });
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."
        };
        c.AddSecurityDefinition("Bearer", securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, new string[] { } } });
    });

    var app = builder.Build();

    // Apply pending migrations automatically
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

    // ----------------------------
    // Middleware
    // ----------------------------
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini Project Manager API v1"));
    }

    app.UseExceptionHandler("/error"); // global error handling
    app.UseHttpsRedirection();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.Error.WriteLine("Fatal startup error: " + ex);
    throw;
}