//@BaseCode
//MdStart
using System.Collections.Generic;
using System.Text;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public class SorterValues : Dictionary<string, SorterItem>
    {
        public string CreateOrderBy()
        {
            var result = new StringBuilder();

            foreach (var item in this)
            {
                if (item.Key.EndsWith($"{StaticLiterals.SortOperationPostfix}") == false)
                {
                    if (TryGetValue($"{item.Key}", out SorterItem sorterItem))
                    {
                        if (result.Length > 0)
                        {
                            result.Append(", ");
                        }
                        result.Append($"{sorterItem.Name} {sorterItem.Operation}");
                    }
                }
            }
            return result.ToString();
        }
    }
}
//MdEnd