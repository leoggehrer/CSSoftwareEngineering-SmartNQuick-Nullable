//@Ignore
using CommonBase.Extensions;
using SmartNQuick.Contracts.Persistence.MusicStore;

namespace SmartNQuick.Transfer.Models.Persistence.MusicStore
{
	public partial class Artist : VersionModel, Contracts.Persistence.MusicStore.IArtist
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
