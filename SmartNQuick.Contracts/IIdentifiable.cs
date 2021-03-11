//@BaseCode

namespace SmartNQuick.Contracts
{
	public partial interface IIdentifiable : ICopyable<IIdentifiable>
	{
		int Id { get; }
	}
}
