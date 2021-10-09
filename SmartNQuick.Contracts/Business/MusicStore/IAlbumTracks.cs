//@Ignore

namespace SmartNQuick.Contracts.Business.MusicStore
{
    public interface IAlbumTracks : IOneToMany<Persistence.MusicStore.IAlbum, Persistence.MusicStore.ITrack>, ICopyable<IAlbumTracks>
    {
    }
}
