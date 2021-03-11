//@BaseCode

using CommonBase.Extensions;
using SmartNQuick.Contracts;

namespace SmartNQuick.Logic.Entities
{
	internal abstract partial class IdentityEntity : IIdentifiable
	{
		public int Id { get; set; }

		public void CopyProperties(IIdentifiable other)
		{
			other.CheckArgument(nameof(other));

			Id = other.Id;
		}
	}
}
