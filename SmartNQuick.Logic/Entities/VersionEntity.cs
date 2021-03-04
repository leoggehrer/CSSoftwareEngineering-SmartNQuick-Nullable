//@BaseCode

namespace SmartNQuick.Logic.Entities
{
	abstract partial class VersionEntity : IdentityEntity, Contracts.IVersionable
	{
		public byte[] RowVersion { get; set; }
	}
}
