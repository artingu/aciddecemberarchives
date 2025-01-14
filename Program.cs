using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddRazorPages();
builder.Services.AddSingleton(FirestoreDb.Create("seismic-ground-286510"));
// Add after FirestoreDb registration
builder.Services.AddSingleton<TrackService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));


builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // This path is where unauthenticated users are redirected to sign in
        options.LoginPath = "/Login";
        // Optional: specify logout path, access denied path, etc.
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/Error";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(1440);
        options.SlidingExpiration = true;
    });


var app = builder.Build();


app.UseRateLimiter();

static string GetTicks() => (DateTime.Now.Ticks & 0x11111).ToString("00000");


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.MapGet("/", () => Results.Ok($"Hello {GetTicks()}"))
                           .RequireRateLimiting("fixed");

// Conditionally apply session AND authentication only for certain paths
app.UseWhen(context =>
    context.Request.Path.StartsWithSegments("/FileUpload", StringComparison.OrdinalIgnoreCase)
    || context.Request.Path.StartsWithSegments("/Submit", StringComparison.OrdinalIgnoreCase),
    subApp =>
    {
        // Enable session
        subApp.UseSession();

        // Enable cookie authentication only for these paths
        subApp.UseAuthentication();
        subApp.UseAuthorization();
    }
);

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapFallbackToPage("/splash"); 
app.Run();