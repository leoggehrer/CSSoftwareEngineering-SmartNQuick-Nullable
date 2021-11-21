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
        void ClearDetails();
        IdentityModel CreateDetail();
        void AddDetail(IdentityModel model);
        void RemoveDetail(IdentityModel model);
    }
}
//MdEndhttps://localhost:44387/img/informatik_ai.jpg