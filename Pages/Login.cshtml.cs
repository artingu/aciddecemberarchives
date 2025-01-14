using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Google.Cloud.Firestore;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

[ValidateAntiForgeryToken]
[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(FirestoreDb db, ILogger<LoginModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    [BindProperty]
    [Required]
    public string UserName { get; set; }

    [BindProperty]
    [Required]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        // validate form input  
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userDoc = await _db.Collection("acidlogins").Document(UserName).GetSnapshotAsync();

        if (userDoc.Exists)
        {
            var storedPasswordHash = userDoc.GetValue<string>("passwordHash");

            if (!string.IsNullOrEmpty(storedPasswordHash) && BCrypt.Net.BCrypt.Verify(Password, storedPasswordHash))
            {
                _logger.LogInformation("User {UserName} successfully logged in.", UserName);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToPage("/submit");
            }
            else
            {
                _logger.LogWarning("Invalid password for user: {UserName}", UserName);
                ErrorMessage = "Invalid credentials";
            }
        }
        else
        {
            _logger.LogWarning("User not found: {UserName}", UserName);
            ErrorMessage = "Invalid credentials";
        }

        return Page();
    }
}