//@BaseCode
//MdStart

using System.Reflection;

namespace SmartNQuick.AspMvc.Modules.View
{
    public class ViewBagWrapper
    {
        private dynamic ViewBag { get; set; }
        public ViewBagWrapper(dynamic viewBag)
        {
            ViewBag = viewBag;
        }

        public bool? Handled
        {
            get => ViewBag.Handled as bool?;
            set => ViewBag.Handle = value;
        }
        public PropertyInfo DisplayProperty
        {
            get => ViewBag.DisplayProperty as PropertyInfo;
            set => ViewBag.DisplayProperty = value;
        }
        public string[] HiddenNames
        {
            get => ViewBag.HiddenNames as string[];
            set => ViewBag.HiddenNames = value;
        }
        public string[] IgnoreNames
        {
            get => ViewBag.IgnoreNames as string[];
            set => ViewBag.IgnoreNames = value;
        }
        public string[] DisplayNames
        {
            get => ViewBag.DisplayNames as string[];
            set => ViewBag.DisplayNames = value;
        }
    }
}
//MdEnd