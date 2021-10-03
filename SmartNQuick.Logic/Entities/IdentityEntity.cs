//@BaseCode
//MdStart
using SmartNQuick.Contracts;

namespace SmartNQuick.Logic.Entities
{
    internal abstract partial class IdentityEntity : EntityObject, IIdentifiable
	{
		public virtual int Id { get; set; }
    }
}
//MdEnd