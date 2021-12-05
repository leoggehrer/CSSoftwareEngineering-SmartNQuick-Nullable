//@BaseCode
//MdStart
using SmartNQuick.Contracts.Modules.Common;

namespace SmartNQuick.Contracts.ThirdParty
{
    public partial interface IStaticPage : IVersionable, ICopyable<IStaticPage>
    {
        string AppName { get; set; }
        string Key { get; set; }
        string Description { get; set; }
        string Content { get; set; }
        State State { get; set; }
    }
}
//MdEnd