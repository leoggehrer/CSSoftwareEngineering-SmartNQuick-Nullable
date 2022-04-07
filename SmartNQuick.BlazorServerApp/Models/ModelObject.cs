//@BaseCode
//MdStart
using SmartNQuick.BlazorServerApp.Models.Modules.Form;
using System.Collections;

namespace SmartNQuick.BlazorServerApp.Models
{
    public abstract partial class ModelObject
    {
        private List<ModelObject> subObjects = new();
        protected List<ModelObject> SubObjects
        {
            get
            {
                if (subObjects == null)
                {
                    subObjects = new List<ModelObject>();
                }
                return subObjects;
            }
        }
        public IEnumerable<ModelObject> GetSubObjects() => SubObjects;
        public string ErrorMessage { get; set; } = string.Empty;

        public virtual void BeforeDisplay()
        {
            foreach (var item in SubObjects)
            {
                item.BeforeDisplay();
            }
        }

        public virtual void BeforeEdit()
        {
            foreach (var item in SubObjects)
            {
                item.BeforeEdit();
            }
        }
        public virtual void BeforeSave()
        {
            foreach (var item in SubObjects)
            {
                item.BeforeSave();
            }
        }
        public virtual void AfterSave()
        {
            foreach (var item in SubObjects)
            {
                item.AfterSave();
            }
        }
        public virtual void CancelEdit()
        {
            foreach (var item in SubObjects)
            {
                item.CancelEdit();
            }
        }

        public virtual void BeforeCancel()
        {
            foreach (var item in SubObjects)
            {
                item.BeforeCancel();
            }
        }
        public virtual void AfterCancel()
        {
            foreach (var item in SubObjects)
            {
                item.AfterCancel();
            }
        }

        public virtual void BeforeDelete()
        {
            foreach (var item in SubObjects)
            {
                item.BeforeDelete();
            }
        }
        public virtual void ConfirmedDelete()
        {
            foreach (var item in SubObjects)
            {
                item.ConfirmedDelete();
            }
        }
        public virtual void AfterDelete()
        {
            foreach (var item in SubObjects)
            {
                item.AfterDelete();
            }
        }
        public virtual void CancelDelete()
        {
            foreach (var item in SubObjects)
            {
                item.CancelDelete();
            }
        }

        public virtual void EvaluateDisplayInfo(DisplayInfo displayInfo)
        {
            foreach (var item in SubObjects)
            {
                item.EvaluateDisplayInfo(displayInfo);
            }
        }
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