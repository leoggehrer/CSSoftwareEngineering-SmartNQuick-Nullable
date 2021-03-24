using System;
using System.Threading.Tasks;

namespace SmartNQuick.ConApp
{
	class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Test SmartNQuick");

            using var genreCtrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
            using var artistCtrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IArtist>(genreCtrl);

            for (int i = 0; i < 10; i++)
			{
                var genre = await genreCtrl.CreateAsync();

                genre.Name = $"Genre - {Guid.NewGuid()}";
                await genreCtrl.InsertAsync(genre);
			}
            //await genreCtrl.SaveChangesAsync();

            for (int i = 0; i < 20; i++)
            {
                var artist = await artistCtrl.CreateAsync();

                artist.Name = $"Künstler - {Guid.NewGuid()}";
                await artistCtrl.InsertAsync(artist);
            }
            await artistCtrl.SaveChangesAsync();

            var artists = await artistCtrl.GetAllAsync();
            using var albumCtrl = Logic.Factory.Create<Contracts.Persistence.MusicStore.IAlbum>();

			foreach (var artist in artists)
			{
                var album = await albumCtrl.CreateAsync();

                album.ArtistId = artist.Id;
                album.Title = $"Titel - {Guid.NewGuid()}";
                await albumCtrl.InsertAsync(album);
			}
            await albumCtrl.SaveChangesAsync();
        }
    }
}
