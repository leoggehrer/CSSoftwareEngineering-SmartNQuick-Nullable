//@Ignore
using CommonBase.Extensions;
using SmartNQuick.Contracts.Persistence.MusicStore;

namespace SmartNQuick.Logic.Entities.MusicStore
{
	internal partial class Artist : VersionEntity, IArtist
	{
		public string Name { get; set; }

		public void CopyProperties(IArtist other)
		{
			other.CheckArgument(nameof(other));

			Id = other.Id;
			RowVersion = other.RowVersion;
			Name = other.Name;
		}
	}
}
