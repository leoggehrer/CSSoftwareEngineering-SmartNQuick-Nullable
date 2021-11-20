//@BaseCode
//MdStart
using System;
using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Models
{
    public interface IMasterDetails
    {
        IdentityModel Master { get; }
        Type MasterType { get; }
        IEnumerable<IdentityModel> Details { get; }
        Type DetailType { get; }
    }
}
//MdEnd