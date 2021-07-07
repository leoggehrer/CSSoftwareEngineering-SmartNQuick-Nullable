//@Ignore
using CommonBase.Attributes;

namespace SmartNQuick.Contracts.Persistence.MusicStore
{
	[ContractInfo(ContextType = ContextType.Table)]
	public interface IArtist : Modules.Base.INameable, IVersionable, ICopyable<IArtist>
	{
	}
}
