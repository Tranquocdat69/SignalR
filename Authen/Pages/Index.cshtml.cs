using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Authen.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            //Claim? userName = User.FindFirst(ClaimTypes.NameIdentifier);
            IdentityUser identityUser = await _userManager.GetUserAsync(User);
            if (identityUser is not null)
            {
                ViewData["currentUser"] = identityUser.UserName;
            }
        }
    }
}