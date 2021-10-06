//@Ignore
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Entities.Persistence.MusicStore
{
	partial class Album
	{
		public override string ToString()
		{
			return $"{Title} - [{Id}]";
		}
	}
}
