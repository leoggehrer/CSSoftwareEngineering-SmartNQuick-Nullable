//@BaseCode
//MdStart

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public partial class SubmitCmd
    {
        public SubmitCmd()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public bool RightAlign { get; set; } = false;
        public string SubmitText { get; set; } = "Save";
        public string SubmitCss { get; set; } = "btn btn-primary";
        public string SubmitStyle { get; set; } = "min-width: 8em;";
    }
}
//MdEnd
