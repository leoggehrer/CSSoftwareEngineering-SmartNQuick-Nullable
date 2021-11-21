//@BaseCode
//MdStart
namespace SmartNQuick.AspMvc.Models.Modules.Common
{
    public enum ActionMode : int
    {
        Index = 1,

        Create = 2,
        Edit = 4,
        Delete = 8,
        Details = 16,

        CreateDetail = 32,
        EditDetail = 64,
        DeleteDetail = 128,
    }
}
//MdEnd