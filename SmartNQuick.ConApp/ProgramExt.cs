//@Ignore
using CommonBase.Extensions;
using SmartNQuick.Transfer.Models.Persistence.MusicStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factory = SmartNQuick.Adapters.Factory;
#if ACCOUNT_ON
using SmartNQuick.Adapters.Modules.Account;
using System.Reflection;
#endif
namespace SmartNQuick.ConApp
{
    internal partial class Program
    {
#if ACCOUNT_ON
        private static string SaUser => "LeoAdmin";
        private static string SaEmail => "LeoAdmin.SmartNQuick@gmx.at";
        private static string SaPwd => "1234LeoAdmin";

        private static string AaUser => "AppAdmin";
        private static string AaEmail => "AppAdmin.QuickNSmart@gmx.at";
        private static string AaPwd => "1234AppAdmin";
        private static string AaRole => "AppAdmin";

        private static bool AaEnableJwt => true;

        private static async Task InitAppAccessAsync()
        {
            await Logic.Factory.CreateAccountManager().InitAppAccessAsync(SaUser, SaEmail, SaPwd, true).ConfigureAwait(false);
        }
        private static async Task<Contracts.Business.Account.IAppAccess> AddAppAccessAsync(string user, string email, string pwd, bool enableJwtAuth, params string[] roles)
        {
            var accMngr = new AccountManager();
            var login = await accMngr.LogonAsync(SaEmail, SaPwd, string.Empty).ConfigureAwait(false);
            using var ctrl = Factory.Create<Contracts.Business.Account.IAppAccess>(login.SessionToken);
            var entity = await ctrl.CreateAsync();

            entity.OneItem.Name = user;
            entity.OneItem.Email = email;
            entity.OneItem.Password = pwd;
            entity.OneItem.EnableJwtAuth = enableJwtAuth;

            foreach (var item in roles)
            {
                var role = entity.CreateManyItem();

                role.Designation = item;
                entity.AddManyItem(role);
            }
            var identity = await ctrl.InsertAsync(entity).ConfigureAwait(false);
            await accMngr.LogoutAsync(login.SessionToken).ConfigureAwait(false);
            return identity;
        }
#endif
        static partial void AfterRun()
        {
            Adapters.Factory.BaseUri = "https://localhost:5001/api";
            Adapters.Factory.Adapter = Adapters.AdapterType.Controller;

#if ACCOUNT_ON
            Task.Run(async () =>
            {
                try
                {
                    await InitAppAccessAsync();
                    await AddAppAccessAsync(AaUser, AaEmail, AaPwd, AaEnableJwt, AaRole);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            ).Wait();
#endif
            Task.Run(async () =>
                await ImportCsvDataAsync()
            ).Wait();
        }
#if ACCOUNT_ON
        private static Contracts.Persistence.Account.ILoginSession Login { get; set; }
        private static Contracts.Client.IAdapterAccess<C> Create<C>()
        {
            if (Login == null)
            {
                Task.Run(async() =>
                {
                    var accMngr = new AccountManager();
                    
                    Login = await accMngr.LogonAsync(AaEmail, AaPwd);
                }).Wait();
            }
            return Factory.Create<C>(Login.SessionToken);
        }
#else
        private static Contracts.Client.IAdapterAccess<C> Create<C>()
        {
            return Factory.Create<C>();
        }
#endif
        private static async Task ImportCsvDataAsync()
        {
            using var genreCtrl = Create<Contracts.Persistence.MusicStore.IGenre>();
            using var artistCtrl = Create<Contracts.Persistence.MusicStore.IArtist>();
            using var albumCtrl = Create<Contracts.Persistence.MusicStore.IAlbum>();
            using var trackCtrl = Create<Contracts.Persistence.MusicStore.ITrack>();
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
