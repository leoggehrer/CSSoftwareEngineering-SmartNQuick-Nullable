//@BaseCode
//MdStart

namespace SmartNQuick.AspMvc.Models
{
    public abstract partial class IdentityModel : ModelObject, Contracts.IIdentifiable
    {
        private System.Int32 _id;
        public virtual System.Int32 Id
        {
            get
            {
                OnIdReading();
                return _id;
            }
            set
            {
                bool handled = false;
                OnIdChanging(ref handled, ref _id);
                if (handled == false)
                {
                    _id = value;
                }
                OnIdChanged();
            }
        }
        partial void OnIdReading();
        partial void OnIdChanging(ref bool handled, ref System.Int32 _id);
        partial void OnIdChanged();
    }
}
//MdEnd