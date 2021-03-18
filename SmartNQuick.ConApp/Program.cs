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

                genre.Name = $"Genre - {Guid.NewGuid().ToString()}";
                await genreCtrl.InsertAsync(genre);
			}
            for (int i = 0; i < 20; i++)
            {
                var artist = await artistCtrl.CreateAsync();

                artist.Name = $"Künstler - {Guid.NewGuid().ToString()}";
                await artistCtrl.InsertAsync(artist);
            }
            await genreCtrl.SaveChangesAsync();
        }
    }
}
