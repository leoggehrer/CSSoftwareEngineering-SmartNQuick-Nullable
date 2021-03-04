//@BaseCode

using SmartNQuick.Contracts;

namespace SmartNQuick.Logic.Entities
{
	internal abstract partial class VersionEntity : IdentityEntity, IVersionable
	{
		public byte[] RowVersion { get; set; }
	}
}
