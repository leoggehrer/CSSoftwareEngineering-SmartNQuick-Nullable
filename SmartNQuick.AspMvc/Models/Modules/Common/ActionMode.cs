//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc.Models.Modules.Common
{
    public enum ActionMode : int
    {
        Filter = 1,
        Sorter = 2 * Filter,
        Index = 2 * Sorter,
        IndexByPageIndex = 2 * Index,
        IndexByPageSize = 2 * IndexByPageIndex,

        Create = 2 * IndexByPageSize,
        CreateById = 2 * Create,
        Edit = 2 * CreateById,
        Delete = 2 * Edit,

        CreateDetail = 2 * Delete,
        CreateDetailById = 2 * CreateDetail,
        EditDetail = 2 * CreateDetailById,
        DeleteDetail = 2 * EditDetail,

        Details = 2 * DeleteDetail,
    }
}
//MdEnd