//@BaseCode
//MdStart

using System.Collections.Generic;

namespace SmartNQuick.AspMvc.Models.Modules.Csv
{
    public class ImportProtocol : ModelObject
    {
        public string FilePath { get; set; }
        public string BackAction { get; set; } = "Index";
        public string BackController { get; set; } = "Home";
        public IEnumerable<ImportLog> LogInfos { get; set; } = System.Array.Empty<ImportLog>();
    }
}
//MdEnd
