//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc.Models.Modules.Common
{
    public enum CommandMode : int
    {
        None = 0,
        Create = 1,
        Edit = 2 * Create,
        Remove = 2 * Edit,
        ShowDetails = 2 * Remove,
    }
}
//MdEnd