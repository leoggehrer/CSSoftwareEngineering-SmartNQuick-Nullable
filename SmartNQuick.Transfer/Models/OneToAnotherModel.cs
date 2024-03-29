﻿//@BaseCode
//MdStart
using System.Text.Json.Serialization;

namespace SmartNQuick.Transfer.Models
{
    public abstract partial class OneToAnotherModel<TOne, TOneModel, TAnother, TAnotherModel> : IdentityModel
        where TOne : Contracts.IIdentifiable
        where TAnother : Contracts.IIdentifiable
        where TOneModel : IdentityModel, Contracts.ICopyable<TOne>, TOne, new()
        where TAnotherModel : IdentityModel, Contracts.ICopyable<TAnother>, TAnother, new()
    {
        public virtual TOneModel OneModel { get; set; } = new TOneModel();
        [JsonIgnore]
        public virtual TOne OneItem => OneModel;

        public virtual TAnotherModel AnotherModel { get; set; } = new TAnotherModel();
        [JsonIgnore]
        public virtual TAnother AnotherItem => AnotherModel;

        public override int Id { get => OneModel.Id; set => OneModel.Id = value; }
        public byte[] RowVersion
        {
            get
            {
                var result = default(byte[]);

                if (OneModel is VersionModel ve)
                    result = ve.RowVersion;

                return result;
            }
            set
            {
                if (OneModel is VersionModel ve)
                    ve.RowVersion = value;
            }
        }
    }
}
//MdEnd