//@Ignore
using CommonBase.Attributes;

namespace SmartNQuick.Contracts.Persistence.MusicStore
{
    [ContractInfo]
    public interface ITrack : IVersionable, ICopyable<ITrack>
    {
        int AlbumId { get; set; }
        int GenreId { get; set; }
        [ContractPropertyInfo(Required = true, MinLength = 2, MaxLength = 1024, HasIndex = true)]
        string Title { get; set; }
        [ContractPropertyInfo(MaxLength = 512)]
        string Composer { get; set; }
        long Milliseconds { get; set; }
        long Bytes { get; set; }
        double UnitPrice { get; set; }
    }
}
