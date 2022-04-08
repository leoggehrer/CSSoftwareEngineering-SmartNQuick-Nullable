//@BaseCode
//MdStart

namespace SmartNQuick.BlazorServerApp.Models
{
    public interface ITwoPartView
    {
        IdentityModel FirstModel { get; }
        IdentityModel SecondModel { get; }
    }
}
//MdEnd