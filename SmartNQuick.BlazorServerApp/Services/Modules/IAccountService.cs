//@BaseCode
//MdStart
using SmartNQuick.BlazorServerApp.Models.Modules.Account;
using SmartNQuick.BlazorServerApp.Models.Modules.Session;

namespace SmartNQuick.BlazorServerApp.Services.Modules.Authentication
{
    public interface IAccountService
    {
        event EventHandler<AuthorizationSession> AuthorizationChanged;
        AuthorizationSession CurrentAuthorizationSession { get; }

        Task InitAuthorizationSessionAsync();

        Task<AuthorizationSession> LogonAsync(LoginModel loginModel);
        Task<bool> IsSessionAliveAsync(string sessionToken);
        Task ChangePasswordAsync(string oldPassword, string newPassword);
        Task ChangePasswordForAsync(string email, string newPassword);
        Task LogoutAsync();
    }
}
//MdEnd