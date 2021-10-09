//@Ignore

namespace SmartNQuick.Contracts.Business.MusicStore
{
    public interface IArtistAlbums : IOneToMany<Persistence.MusicStore.IArtist, Business.MusicStore.IAlbumTracks>, ICopyable<IArtistAlbums>
    {
    }
}
