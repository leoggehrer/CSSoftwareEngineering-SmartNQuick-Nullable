//@BaseCode
//MdStart

namespace SmartNQuick.Contracts
{
    public partial interface IComposite<TConnector, TOne, TAnother> : IIdentifiable
        where TConnector : IIdentifiable
        where TOne : IIdentifiable
        where TAnother : IIdentifiable
    {
        TConnector ConnectorItem { get; }
        TOne OneItem { get; }
        /// <summary>
        /// Gets or sets the OneItemIncludeSave. If this flag is true, the data from the item is saved with the ConnectItem.
        /// </summary>
        bool OneItemIncludeSave { get; set; }

        TAnother AnotherItem { get; }
        /// <summary>
        /// Gets or sets the AnotherItemIncludeSave. If this flag is true, the data from the item is saved with the ConnectItem.
        /// </summary>
        bool AnotherItemIncludeSave { get; set; }
    }
}
//MdEnd