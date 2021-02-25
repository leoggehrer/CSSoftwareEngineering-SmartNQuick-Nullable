//@BaseCode

namespace SmartNQuick.Contracts
{
	public partial interface ICopyable<T>
	{
		void CopyProperties(T other);
	}
}
