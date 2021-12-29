//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc.Models.Modules.Common
{
    public enum ActionMode : int
    {
        Search = 1,
        Filter = 2 * Search,
        Sorter = 2 * Filter,
        Index = 2 * Sorter,
        IndexByPageIndex = 2 * Index,
        IndexByPageSize = 2 * IndexByPageIndex,

        Create = 2 * IndexByPageSize,
        CreateById = 2 * Create,
        Insert = 2 * CreateById,
        Edit = 2 * Insert,
        Update = 2 * Edit,
        Delete = 2 * Update,
        Apply = 2 * Delete,

        CreateDetail = 2 * Apply,
        CreateDetailById = 2 * CreateDetail,
        InsertDetail = 2 * CreateDetailById,
        EditDetail = 2 * InsertDetail,
        UpdateDetail = 2 * EditDetail,
        DeleteDetail = 2 * UpdateDetail,
        ApplyDetail = 2 * DeleteDetail,

        Details = 2 * ApplyDetail,
    }
}
//MdEnd