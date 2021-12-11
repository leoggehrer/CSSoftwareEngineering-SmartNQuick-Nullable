//@BaseCode
//MdStart

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public partial class SubmitApplyCmd : SubmitCmd
    {
        public SubmitApplyCmd()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public string ApplyText { get; set; } = "Apply";
        public string ApplyCss { get; set; } = "btn btn-outline-primary";
        public string ApplyStyle { get; set; } = "min-width: 8em;";
        public string ApplyAction { get; set; } = string.Empty;
    }
}
//MdEnd
