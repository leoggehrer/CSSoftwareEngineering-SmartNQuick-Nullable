//@BaseCode
//MdStart
namespace SmartNQuick.BlazorServerApp.Models.Modules.Session
{
    public class AuthorizationSession
    {
        public int IdentityId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
        public bool HasRole(string role) => Roles != null && Roles.Any(r => r.Equals(role, StringComparison.CurrentCultureIgnoreCase));
        public bool HasAnyRole(params string[] roles) => Roles != null && roles != null && Roles.Any(r => roles.Any(r2 => r2.Equals(r, StringComparison.CurrentCultureIgnoreCase)));
        public DateTime LoginTime { get; set; }
    }
}
//MdEnd