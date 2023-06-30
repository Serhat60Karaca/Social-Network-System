using TESNS.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace TESNS.CustomValidations
{
    public class CustomUserValidation : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (int.TryParse(user.UserName[0].ToString(), out int _)) 
                errors.Add(new IdentityError { Code = "UserNameNumberStartWith", Description = "Username should not begin with numeric" });
            if (user.UserName.Length < 3 && user.UserName.Length > 25) 
                errors.Add(new IdentityError { Code = "UserNameLength", Description = "Username length must be between 3-15 characters" });
            if (user.Email.Length > 70) 
                errors.Add(new IdentityError { Code = "EmailLength", Description = "E-mail can not be longer than 70 characters" });

            if (!errors.Any())
                return Task.FromResult(IdentityResult.Success);
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
