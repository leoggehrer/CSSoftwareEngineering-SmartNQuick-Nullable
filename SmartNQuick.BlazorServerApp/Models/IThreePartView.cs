//@BaseCode
//MdStart

namespace SmartNQuick.BlazorServerApp.Models
{
    public interface IThreePartView
    {
        IdentityModel FirstModel { get; }
        IdentityModel SecondModel { get; }
        IdentityModel ThirdModel { get; }
    }
}
//MdEnd