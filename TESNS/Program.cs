using TESNS.CustomValidations;
using TESNS.Models;
using TESNS.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using TESNS.Services;
using TESNS.Helpers;
using TESNS.Repositories;
using TESNS.Services.Concrete;
using TESNS.Repositories.Concrete;
using TESNS.Data;
using TESNS.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<IUserInteractionRepository, UserInteractionRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ISendEmailService,SendEmailService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetSection(key: "ConnectionStrings:DefaultConnection").Value));
builder.Services.AddIdentity<AppUser, AppRole>(e =>
{
    e.Password.RequiredLength = 5;
    e.Password.RequireNonAlphanumeric = false;
    e.Password.RequireLowercase = false;
    e.Password.RequireUppercase = false;
    e.Password.RequireDigit = false;
    e.User.RequireUniqueEmail = true; //Unique email addresses
    e.User.AllowedUserNameCharacters = "abc�defghi�jklmno�pqrs�tu�vwxyzABC�DEFGHI�JKLMNO�PQRS�TU�VWXYZ0123456789-._@+";
}).AddPasswordValidator<CustomPasswordValidation>().AddUserValidator<CustomUserValidation>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(e => {
    e.LoginPath = new PathString("/user/login");
    e.Cookie = new CookieBuilder()
    {
        Name = "LogisticSystemCookie", //cookie name
        HttpOnly = false, //prevent access to cookie from client-side
        //Expiration = TimeSpan.FromMinutes(2), //expritaion time for cookie
        SameSite = SameSiteMode.Lax, //prevent sending unnecessary cookies for some requests
        SecurePolicy = CookieSecurePolicy.Always //accessible on https
    };
    e.SlidingExpiration = true;
    e.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//DataSeed.SeedCategories(app);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapHub<ChatHub>("/chatHub"); //buraya bak�cam
app.MapControllers();
/*
app.MapControllerRoute(Hus
    "PersonnelsDelete",
    "personnels/delete/{id?}",
    new { Controller = "Personnel", Action = "Delete" },
    new { id = @"\d{1}" }
);
app.MapControllerRoute(name: "Personnel",
                pattern: "Personnel/{*Delete}",
                defaults: new { controller = "Personnel", action = "Delete" });
*/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
