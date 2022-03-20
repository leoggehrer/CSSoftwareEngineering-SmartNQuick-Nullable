//@BaseCode
//MdStart

namespace SmartNQuick.AspMvc.Models
{
    public interface ITwoPartView
    {
        IdentityModel FirstModel { get; }
        IdentityModel SecondModel { get; }
    }
}
//MdEnd