using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace fitnessChallenge.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Url]
            [Display(Name = "Profile Picture URL")]
            public string ProfilePictureUrl { get; set; }

            [Display(Name = "Bio")]
            public string Bio { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var profilePictureUrlClaim = await _userManager.GetClaimsAsync(user);
            var profilePictureUrl = profilePictureUrlClaim.FirstOrDefault(c => c.Type == "ProfilePictureUrl")?.Value;

            var bioClaim = await _userManager.GetClaimsAsync(user);
            var bio = bioClaim.FirstOrDefault(c => c.Type == "Bio")?.Value;

            Username = userName;

            Input = new InputModel
            {
                Username = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                ProfilePictureUrl = profilePictureUrl,
                Bio = bio
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userName = await _userManager.GetUserNameAsync(user);
            if (Input.Username != userName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUserNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set username.";
                    return RedirectToPage();
                }
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set email.";
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var profilePictureUrlClaim = await _userManager.GetClaimsAsync(user);
            var profilePictureUrl = profilePictureUrlClaim.FirstOrDefault(c => c.Type == "ProfilePictureUrl");
            if (profilePictureUrl == null)
            {
                await _userManager.AddClaimAsync(user, new Claim("ProfilePictureUrl", Input.ProfilePictureUrl));
            }
            else if (profilePictureUrl.Value != Input.ProfilePictureUrl)
            {
                await _userManager.RemoveClaimAsync(user, profilePictureUrl);
                await _userManager.AddClaimAsync(user, new Claim("ProfilePictureUrl", Input.ProfilePictureUrl));
            }

            var bioClaim = await _userManager.GetClaimsAsync(user);
            var bio = bioClaim.FirstOrDefault(c => c.Type == "Bio");
            if (bio == null)
            {
                await _userManager.AddClaimAsync(user, new Claim("Bio", Input.Bio));
            }
            else if (bio.Value != Input.Bio)
            {
                await _userManager.RemoveClaimAsync(user, bio);
                await _userManager.AddClaimAsync(user, new Claim("Bio", Input.Bio));
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
