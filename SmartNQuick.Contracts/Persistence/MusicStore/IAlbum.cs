//@Ignore
using CommonBase.Attributes;

namespace SmartNQuick.Contracts.Persistence.MusicStore
{
	[ContractInfo(ContextType = ContextType.Table)]
	public partial interface IAlbum : IVersionable, ICopyable<IAlbum>
	{
		int ArtistId { get; set; }
		[ContractPropertyInfo(Required = true, MaxLength = 2048, IsUnique = true)]
		string Title { get; set; }
	}
}
