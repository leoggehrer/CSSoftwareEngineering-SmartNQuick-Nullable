//@BaseCode
//MdStart
using System;

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
        void RemoveDetailById(int id);
    }
}
//MdEnd