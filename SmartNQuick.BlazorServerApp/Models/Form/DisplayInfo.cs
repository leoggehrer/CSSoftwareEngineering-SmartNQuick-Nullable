//@BaseCode
//MdStart
using SmartNQuick.BlazorServerApp.Models.Modules.Common;

namespace SmartNQuick.BlazorServerApp.Models.Modules.Form
{
    public partial class DisplayInfo
    {
        public string Key => $"{ModelName}{OriginName}";
        public string ModelName { get; set; }
        public string OriginName { get; set; }
        public string MappingName { get; set; } = string.Empty;
        public string PropertyName => string.IsNullOrEmpty(MappingName) ? OriginName : MappingName;

        public bool ScaffoldItem { get; set; } = true;
        public bool IsModelItem { get; set; }
        public ReadonlyMode ReadonlyMode { get; set; } = ReadonlyMode.None;
        public VisibilityMode VisibilityMode { get; set; } = VisibilityMode.Visible;

        public bool ListSortable { get; set; } = true;
        public bool ListFilterable { get; set; } = true;
        public bool ListGroupable { get; set; }
        public bool ListHasFooter { get; set; }
        public string ListWidth { get; set; } = "100%";

        public string DisplayFormat { get; set; } = string.Empty;
        public int Order { get; set; } = 10_000;
        public Func<object, object, string?> ToDisplay { get; set; }
        public Func<string, string> GetFooterText { get; set; }

        public DisplayInfo()
            : this(string.Empty, string.Empty)
        {
        }
        public DisplayInfo(string originName)
            : this(string.Empty, originName)
        {
        }
        public DisplayInfo(string modelName, string originName)
        {
            ModelName = modelName;
            OriginName = originName;
            ToDisplay = (m, v) => v?.ToString();
            GetFooterText = n => string.Empty;
        }

        public override string ToString() => OriginName;
    }
}
//MdEnd
