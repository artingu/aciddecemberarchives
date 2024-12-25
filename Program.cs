using Google.Cloud.Firestore;
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



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseWhen(context =>
    context.Request.Path.StartsWithSegments("/FileUpload", StringComparison.OrdinalIgnoreCase), // or "/SpecialArea"
    subApp =>
    {
        subApp.UseSession();
    }
);
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();