using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientCorntracts = SmartNQuick.Contracts;

namespace SmartNQuick.Logic.DataContext
{
	partial class ProjectDbContext
	{
		public DbSet<Entities.MusicStore.Genre> Genres { get; set; }
		public DbSet<Entities.MusicStore.Artist> Artists { get; set; }


		partial void GetDbSet<C, E>(ref DbSet<E> dbset) where E : class
		{
			if (typeof(C) == typeof(ClientCorntracts.Persistence.MusicStore.IGenre))
			{
				dbset = Genres as DbSet<E>;
			}
			else if (typeof(C) == typeof(ClientCorntracts.Persistence.MusicStore.IArtist))
			{
				dbset = Artists as DbSet<E>;
			}
		}

		partial void BeforeOnModelCreating(ModelBuilder modelBuilder, ref bool handled)
		{
			var genreBuilder = modelBuilder.Entity<Entities.MusicStore.Genre>();

			genreBuilder.HasKey(p => p.Id);
			genreBuilder.Property(p => p.RowVersion).IsRowVersion();
			genreBuilder.Property(p => p.Name)
						 .IsRequired()
						 .HasMaxLength(256);
			genreBuilder.HasIndex(p => p.Name)
						 .IsUnique();

			var artistBuilder = modelBuilder.Entity<Entities.MusicStore.Artist>();

			artistBuilder.HasKey(p => p.Id);
			artistBuilder.Property(p => p.RowVersion).IsRowVersion();
			artistBuilder.Property(p => p.Name)
						 .IsRequired()
						 .HasMaxLength(256);
			artistBuilder.HasIndex(p => p.Name)
						 .IsUnique();
		}


	}
}
