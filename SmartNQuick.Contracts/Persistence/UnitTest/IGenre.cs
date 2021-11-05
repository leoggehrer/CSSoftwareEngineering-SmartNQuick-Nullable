//@Ignore
using CommonBase.Attributes;

namespace SmartNQuick.Contracts.Persistence.UnitTest
{
    [ContractInfo(ContextType = ContextType.Table)]
	public interface IGenre : Modules.Base.INameable, IVersionable, ICopyable<IGenre>
	{

	}
}
