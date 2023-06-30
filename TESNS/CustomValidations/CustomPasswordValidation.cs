using TESNS.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace TESNS.CustomValidations
{
    public class CustomPasswordValidation : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> identityErrors = new List<IdentityError>();
            if (password.Length < 5)
                identityErrors.Add(new IdentityError { Code = "PasswordLength", Description = "Please enter a password with at least 5 digits" });
            if (password.ToLower().Contains(user.UserName.ToLower())) 
                identityErrors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Password should not include username." });

            if (!identityErrors.Any())
                return Task.FromResult(IdentityResult.Success);
            else
                return Task.FromResult(IdentityResult.Failed(identityErrors.ToArray()));
        }
    }
}
