//@BaseCode
//MdStart
using SmartNQuick.Contracts.Modules.Common;

namespace SmartNQuick.Contracts.ThirdParty
{
    public partial interface IHtmlAttribute : IVersionable, ICopyable<IHtmlAttribute>
    {
        string AppName { get; set; }
        string Key { get; set; }
        string Description { get; set; }
        string Content { get; set; }
        State State { get; set; }
    }
}
//MdEnd