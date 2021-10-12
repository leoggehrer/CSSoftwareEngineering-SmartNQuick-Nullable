//@BaseCode
//MdStart
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartNQuick.AspMvc.Models
{
    public class ModelObject
	{
        [ScaffoldColumn(false)]
        public bool HasError => string.IsNullOrEmpty(ActionError) == false;
        [ScaffoldColumn(false)]
        public string ActionError { get; set; }
        protected static bool IsEqualsWith(object obj1, object obj2)
        {
            bool result = false;

            if (obj1 == null && obj2 == null)
            {
                result = true;
            }
            else if (obj1 != null && obj2 != null)
            {
                if (obj1 is IEnumerable objEnum1 && obj2 is IEnumerable objEnum2)
                {
                    var enumerable1 = objEnum1.Cast<object>().ToList();
                    var enumerable2 = objEnum2.Cast<object>().ToList();

                    result = enumerable1.SequenceEqual(enumerable2);
                }
                else
                {
                    result = obj1.Equals(obj2);
                }
            }
            return result;
        }
    }
}
//MdEnd
