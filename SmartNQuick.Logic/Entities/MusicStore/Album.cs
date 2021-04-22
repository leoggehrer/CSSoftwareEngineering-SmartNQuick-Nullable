//@Ignore
using CommonBase.Extensions;
using SmartNQuick.Contracts.Persistence.MusicStore;

namespace SmartNQuick.Logic.Entities.MusicStore
{
	class Album : VersionEntity, SmartNQuick.Contracts.Persistence.MusicStore.IAlbum
	{
		public int ArtistId { get; set; }
		public string Title { get; set; }

		public void CopyProperties(IAlbum other)
		{
			other.CheckArgument(nameof(other));

			Id = other.Id;
			RowVersion = other.RowVersion;
			ArtistId = other.ArtistId;
			Title = other.Title;
		}
		#region Navigation Properties
		public Artist Artist { get; set; }
		#endregion
	}
}
