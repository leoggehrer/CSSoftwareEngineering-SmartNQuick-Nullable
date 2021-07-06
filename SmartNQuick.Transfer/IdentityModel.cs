//@BaseCode

namespace SmartNQuick.Transfer
{
	public partial class IdentityModel : TransferObject, Contracts.IIdentifiable
	{
		public int Id { get; set; }
	}
}
