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

        CreateDetail = 2 * Remove,
        EditDetail = 2 * CreateDetail,
        RemoveDetail = 2 * EditDetail,

        ShowDetails = 2 * RemoveDetail,
    }
}
//MdEnd