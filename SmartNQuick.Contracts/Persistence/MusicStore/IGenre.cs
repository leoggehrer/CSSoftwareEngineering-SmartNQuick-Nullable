using CommonBase.Attributes;

namespace SmartNQuick.Contracts.Persistence.MusicStore
{
	[ContractInfo(ContextType = ContextType.Table)]
	public interface IGenre : IVersionable, ICopyable<IGenre>
	{
		[ContractPropertyInfo(Required = true, MaxLength = 256, IsUnique = true)]
		string Name { get; set; }
	}
}
