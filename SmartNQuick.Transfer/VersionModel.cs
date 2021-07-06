//@BaseCode

namespace SmartNQuick.Transfer
{
	public partial class VersionModel : IdentityModel, Contracts.IVersionable
	{
		public byte[] RowVersion { get; set; }
	}
}
