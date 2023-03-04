using Devhunt.Helpers;
using Devhunt.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
        var connectionString = builder.Configuration.GetConnectionString("Default");
        options.UseMySql(connectionString,  ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<JwtServices>();
builder.Services.AddScoped<IPubRepository, PubRepository>();
builder.Services.AddScoped<IComsRepository, ComsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(options => options
    .WithOrigins("https://localhost:44416")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
