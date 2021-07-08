//@QnSBaseCode
//MdStart

namespace SmartNQuick.Adapters.Service
{
    public partial class InvokeServiceAdapter : ServiceAdapterObject
    {
        static InvokeServiceAdapter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public InvokeServiceAdapter(string baseUri)
            : base(baseUri)
        {
            Constructing();
            Constructed();
        }
        public InvokeServiceAdapter(string baseUri, string sessionToken)
            : base(baseUri, sessionToken)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
    }
}
//MdEnd