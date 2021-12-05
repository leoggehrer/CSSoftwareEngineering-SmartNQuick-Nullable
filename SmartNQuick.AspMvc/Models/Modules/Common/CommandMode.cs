//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc.Models.Modules.Common
{
    public enum CommandMode : int
    {
        None = 0,

        Create = 1,
        CreateById = 2 * Create,
        Edit = 2 * CreateById,
        Delete = 2 * Edit,

        CreateDetail = 2 * Delete,
        EditDetail = 2 * CreateDetail,
        DeleteDetail = 2 * EditDetail,

        Details = 2 * DeleteDetail,

        All = Create + CreateDetail + Edit + EditDetail + Delete + DeleteDetail + Details,
    }
}
//MdEnd