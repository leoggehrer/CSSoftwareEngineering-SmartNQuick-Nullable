using CommonBase.Attributes;

namespace SmartNQuick.Contracts.Persistence.MusicStore
{
	[ContractInfo(ContextType = ContextType.Table)]
	public interface IArtist : IVersionable, ICopyable<IArtist>
	{
		[ContractPropertyInfo(Required = true, MaxLength = 128, IsUnique = true)]
		string Name { get; set; }
	}
}
