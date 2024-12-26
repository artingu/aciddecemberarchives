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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


[AllowAnonymous]
public class LoginModel : PageModel
{

    private readonly FirestoreDb _db;

    public LoginModel(FirestoreDb db)
    {
        _db = db;
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
    // Hardcoded list of username/password pairs
    /*     private readonly Dictionary<string, string> _userStore = new()
        {
            { "alice", "password1" },
            { "bob", "secret2" }
        }; */

    public async Task<IActionResult> OnPostAsync(string userName, string password)
    {
        // validate form input  
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userDoc = await _db.Collection("acidlogins").Document(UserName).GetSnapshotAsync();

        // hash the password using BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
        // decrypt again with BCrypt.Verify(Password, hashedPassword)
        var unhashedPassword = BCrypt.Net.BCrypt.Verify(Password, hashedPassword);
        if (userDoc.Exists)
        {
            var storedPasswordHash = userDoc.GetValue<string>("passwordHash");

            if (BCrypt.Net.BCrypt.Verify(Password, storedPasswordHash))
            {

                // Validate user. If valid, issue cookie:
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName)
        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToPage("/FileUpload"); // or wherever
            }
            else { ViewData["ErrorMessage"] = "Wrong password"; return Page(); }


        }
        else { ViewData["ErrorMessage"] = "User not found"; return Page(); }



    }
}