//@BaseCode

using CommonBase.Extensions;
using SmartNQuick.Contracts;

namespace SmartNQuick.Logic.Entities
{
	internal abstract partial class IdentityEntity : IIdentifiable
	{
		public int Id { get; set; }
	}
}
