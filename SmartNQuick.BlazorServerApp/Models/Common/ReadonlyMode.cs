//@BaseCode
//MdStart
namespace SmartNQuick.BlazorServerApp.Models.Modules.Common
{
    [Flags]
    public enum ReadonlyMode : byte
    {
        None = 0,
        Create = 1,
        Update = 2 * Create,
        ListCreate = 2 * Update,
        ListUpdate = 2 * ListCreate,

        Readonly = Create + Update,
    }
}
//MdEnd