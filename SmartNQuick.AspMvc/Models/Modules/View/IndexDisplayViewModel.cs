//@BaseCode
//MdStart
using SmartNQuick.AspMvc.Modules.View;
using System;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public partial class IndexDisplayViewModel : ViewModel, IDisplayViewModel
    {
        static IndexDisplayViewModel()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private ModelObject model;
        private ModelObject displayModel;
        private IEnumerable<PropertyInfo> displayProperties;

        public ModelObject Model
        {
            get => model;
            set
            {
                model = value ?? model;
            }
        }
        public ModelObject DisplayModel
        {
            get => displayModel ?? model;
            set => displayModel = value;
        }
        public IEnumerable<PropertyInfo> DisplayProperties 
        {
            get => displayProperties; 
            set => displayProperties = value ?? displayProperties; 
        }

        public IndexDisplayViewModel(ViewBagWrapper viewBagInfo, Type modelType, Type displayType, ModelObject model, IEnumerable<PropertyInfo> displayProperties)
            : base(viewBagInfo, modelType, displayType)
        {
            Constructing();
            model.CheckArgument(nameof(model));
            displayProperties.CheckArgument(nameof(displayProperties));

            Model = model;
            DisplayProperties = displayProperties;
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public virtual IEnumerable<PropertyInfo> GetDisplayProperties()
        {
            return DisplayProperties;
        }

        public virtual object GetValue(PropertyInfo propertyInfo)
        {
            var handled = false;
            var value = default(object);

            BeforeGetValue(propertyInfo, ref value, ref handled);
            if (handled == false)
            {
                value = GetValue(DisplayModel, propertyInfo);
            }
            AfterGetValue(value);
            return value?.ToString() ?? string.Empty;
        }
        partial void BeforeGetValue(PropertyInfo propertyInfo, ref object? value, ref bool handled);
        partial void AfterGetValue(object? value);
        public virtual string GetDisplayValue(PropertyInfo propertyInfo)
        {
            var handled = false;
            var result = string.Empty;

            BeforeGetDisplayValue(propertyInfo, ref result, ref handled);
            if (handled == false)
            {
                result = GetDisplayValue(DisplayModel, propertyInfo);
            }
            AfterGetDisplayValue(result);
            return result;
        }
        partial void BeforeGetDisplayValue(PropertyInfo propertyInfo, ref string value, ref bool handled);
        partial void AfterGetDisplayValue(string value);
    }
}
//MdEnd