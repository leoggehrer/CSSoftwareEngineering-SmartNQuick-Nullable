//@Ignore
using CommonBase.Extensions;
using SmartNQuick.Transfer.Persistence.MusicStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factory = SmartNQuick.Adapters.Factory;

namespace SmartNQuick.ConApp
{
    partial class Program
    {
        static partial void AfterRun()
        {
            Task.Run(async () =>
                await ImportCsvDataAsync()
            ).Wait();
        }
        private static async Task ImportCsvDataAsync()
        {
            using var genreCtrl = Factory.Create<Contracts.Persistence.MusicStore.IGenre>();
            using var artistCtrl = Factory.Create<Contracts.Persistence.MusicStore.IArtist>();
            using var albumCtrl = Factory.Create<Contracts.Persistence.MusicStore.IAlbum>();
            using var trackCtrl = Factory.Create<Contracts.Persistence.MusicStore.ITrack>();
            var genreData = File.ReadAllLines("Data\\Genre.csv", Encoding.Default).Skip(1).Select(l =>
            {
                var data = l.Split(';');
                return new { Data = data, Id = Int32.Parse(data[0]), Entity = new Genre { Name = data[1] } };
            });
            var artistData = File.ReadAllLines("Data\\Artist.csv", Encoding.Default).Skip(1).Select(l =>
            {
                var data = l.Split(';');
                return new { Data = data, Id = Int32.Parse(data[0]), Entity = new Artist { Name = data[1] } };
            });
            var albumData = File.ReadAllLines("Data\\Album.csv", Encoding.Default).Skip(1).Select(l =>
            {
                var data = l.Split(';');
                return new { Data = data, Id = Int32.Parse(data[0]), Entity = new Album { Title = data[1], ArtistId = Int32.Parse(data[2]) } };
            });
            var trackData = File.ReadAllLines("Data\\Track.csv", Encoding.Default).Skip(1).Select(l =>
            {
                var data = l.Split(';');
                return new { Data = data, Id = Int32.Parse(data[0]), Entity = new Track { Title = data[1], AlbumId = Int32.Parse(data[2]), GenreId = Int32.Parse(data[3]), Composer = data[4].Equals("<NULL>") ? null : data[4], Milliseconds = Int32.Parse(data[5]), Bytes = Int32.Parse(data[6]), UnitPrice = Double.Parse(data[7]) } };
            });
            var genres = new List<Contracts.Persistence.MusicStore.IGenre>();
            var artists = new List<Contracts.Persistence.MusicStore.IArtist>();
            var alben = new List<Contracts.Persistence.MusicStore.IAlbum>();
            var tracks = new List<Contracts.Persistence.MusicStore.ITrack>();

            // import genre
            genres.AddRange(await genreCtrl.InsertAsync(genreData.Select(e => e.Entity)));
            // import artist
            artists.AddRange(await artistCtrl.InsertAsync(artistData.Select(e => e.Entity)));
            // import alben
            foreach (var item in albumData)
            {
                var idx = artistData.FindIndex(e => e.Id == item.Entity.ArtistId);

                if (idx > -1)
                {
                    item.Entity.ArtistId = artists[idx].Id;
                }
            }
            alben.AddRange(await albumCtrl.InsertAsync(albumData.Select(e => e.Entity)));
            // import tracks
            foreach (var item in trackData)
            {
                var idx = genreData.FindIndex(e => e.Id == item.Entity.GenreId);

                if (idx > -1)
                {
                    item.Entity.GenreId = genres[idx].Id;
                }

                idx = albumData.FindIndex(e => e.Id == item.Entity.AlbumId);

                if (idx > -1)
                {
                    item.Entity.AlbumId = alben[idx].Id;
                }
            }
            tracks.AddRange(await trackCtrl.InsertAsync(trackData.Select(e => e.Entity)));
        }
    }
}
