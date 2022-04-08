//@BaseCode
//MdStart
namespace SmartNQuick.BlazorServerApp.Models
{
    public partial class IdentityModel : ModelObject, Contracts.IIdentifiable
    {
        private bool cloneData;

        public virtual int Id { get; set; }
        public virtual bool Cloneable => Id > 0;
        public virtual bool CloneData 
        {
            get => Cloneable && cloneData; 
            set => cloneData = value; 
        }
    }
}
//MdEnd