//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc.Models.Modules.Common
{
    public enum EditMode : int
    {
        None = 0,
        Insert = 1,
        Edit = 2 * Insert,
        Delete = 2 * Edit,
    }
}
//MdEnd