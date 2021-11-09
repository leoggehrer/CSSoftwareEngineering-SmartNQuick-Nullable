//@BaseCode
//MdStart

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public partial class BackCmd
    {
        public BackCmd()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public string BackText { get; set; } = "Back to List";
        public string BackAction { get; set; } = "Index";
        public string BackController { get; set; }
        public string BackCss { get; set; } = "btn btn-outline-dark";
        public string BackStyle { get; set; } = "min-width: 8em;";
    }
}
//MdEnd
