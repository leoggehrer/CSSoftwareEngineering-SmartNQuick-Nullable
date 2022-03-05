//@BaseCode

using SmartNQuick.Contracts;

namespace SmartNQuick.Logic.Entities
{
	internal abstract partial class VersionEntity : IdentityEntity, IVersionable
	{
		public virtual byte[] RowVersion { get; set; }
	}
}
