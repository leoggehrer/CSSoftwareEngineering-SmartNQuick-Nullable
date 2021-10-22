//@BaseCode
//MdStart

using System;

namespace CSharpCodeGenerator.Logic.Common
{
    [Flags]
    public enum ItemType : long
    {
        DbContext = 1,
        Factory = 2,

        BusinessEntity = 32,
        ModuleEntity = 64,
        PersistenceEntity = 128,
        ShadowEntity = 256,
        Entiy = BusinessEntity + ModuleEntity + PersistenceEntity + ShadowEntity,

        BusinessModel = 1024,
        ModuleModel = 2048,
        PersistenceModel = 4096,
        ShadowModel = 8192,
        ThridPartyModel = (long)8192 * 2,
        Model = BusinessModel + ModuleModel + PersistenceModel + ShadowModel,

        LogicController = (long)8192 * 4,
        WebApiController = (long)8192 * 8,
        AspMvcController = (long)8192 * 16,
        Controller = LogicController  + WebApiController + AspMvcController,
    }
}
//MdEnd