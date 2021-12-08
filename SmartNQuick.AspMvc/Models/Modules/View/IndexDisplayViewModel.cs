//@BaseCode
//MdStart
using CommonBase.Extensions;
using SmartNQuick.AspMvc.Modules.View;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartNQuick.AspMvc.Models.Modules.View
{
    public class IndexDisplayViewModel : ViewModel, IDisplayViewModel
    {
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

        public IndexDisplayViewModel(ViewBagWrapper viewBagWrapper, Type modelType, Type displayType, ModelObject model, IEnumerable<PropertyInfo> displayProperties)
            : base(viewBagWrapper, modelType, displayType)
        {
            model.CheckArgument(nameof(model));
            displayProperties.CheckArgument(nameof(displayProperties));

            Model = model;
            DisplayProperties = displayProperties;
        }
        public IEnumerable<PropertyInfo> GetDisplayProperties()
        {
            return DisplayProperties;
        }

        public object GetValue(PropertyInfo propertyInfo)
        {
            return GetValue(DisplayModel, propertyInfo);
        }
        public string GetDisplayValue(PropertyInfo propertyInfo)
        {
            return GetDisplayValue(DisplayModel, propertyInfo);
        }
    }
}
//MdEnd