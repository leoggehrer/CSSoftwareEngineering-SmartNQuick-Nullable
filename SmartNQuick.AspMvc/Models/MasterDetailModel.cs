//@BaseCode
//MdStart

namespace SmartNQuick.AspMvc.Models
{
    public partial class MasterDetailModel : ModelObject
	{
        public IdentityModel? Master { get; set; }
        public IdentityModel? Detail { get; set; }
    }
}
//MdEnd
