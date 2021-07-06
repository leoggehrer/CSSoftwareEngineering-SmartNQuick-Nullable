//@BaseCode
//MdStart

using System;

namespace CSharpCodeGenerator.Logic.Common
{
    [Flags]
    public enum ItemType : long
    {
        BusinessEntity = 1,
        ModuleEntity = 2,
        PersistenceEntity = 4,
        ShadowEntity = 8,
        Entiy = BusinessEntity + ModuleEntity + PersistenceEntity + ShadowEntity,

        DbContext = 16,
        Factory = 32,

        BusinessController = 64,
        PersistenceController = 128,
        ShadowController = 256,
        WebApiController = 512,
        Controller = BusinessController  + PersistenceController + ShadowController + WebApiController,

        BusinessModel = 1024,
        ModuleModel = 2048,
        PersistenceModel = 4096,
        ShadowModel = (long)8192,
        Model = BusinessModel + ModuleModel + PersistenceModel + ShadowModel,

        DataGridRazorPage = (long)8192 * 2,
        DataGridRazorPageCode = (long)8192 * 4,

        DataGridHandlerCode = (long)8192 * 8,
        DataGridComponentRazor = (long)8192 * 16,
        DataGridComponentCode = (long)8192 * 32,
        DataGridComponentCommonCode = (long)8192 * 64,
        DataGridColumnsComponentRazor = (long)8192 * 128,
        DataGridColumnsComponentCode = (long)8192 * 256,
        DataGridDetailComponentRazor = (long)8192 * 512,
        DataGridDetailComponentCode = (long)8192 * 1024,

        FieldSetHandlerCode = (long)8192 * 2048,
        FieldSetComponentRazor = (long)8192 * 4096,
        FieldSetComponentCode = (long)8192 * 8192,
        FieldSetDetailComponentRazor = (long)8192 * 8192 * 2,
        FieldSetDetailComponentCode = (long)8192 * 8192 * 4,

        MasterComponentRazor = (long)8192 * 8192 * 8,
        MasterComponentCode = (long)8192 * 8192 * 16,

        MasterDetailsRazorPage = (long)8192 * 8192 * 32,
        MasterDetailsRazorPageCode = (long)8192 * 8192 * 64,

        DetailsComponentRazor = (long)8192 * 8192 * 128,
        DetailsComponentCode = (long)8192 * 8192 * 256,

        TypeScriptEnum = (long)8192 * 8192 * 512,
        TypeScriptContract = (long)8192 * 8192 * 1024,

        Translations = (long)8192 * 8192 * 2048,
        Properties = (long)8192 * 8192 * 4096,

        IndexRazorPageAll = DataGridRazorPage 
                          + DataGridRazorPageCode 
                          + MasterDetailsRazorPage 
                          + MasterDetailsRazorPageCode,

        DataGridAll = DataGridHandlerCode 
                    + DataGridComponentRazor 
                    + DataGridComponentCode
                    + DataGridComponentCommonCode
                    + DataGridDetailComponentRazor 
                    + DataGridDetailComponentCode
                    + DataGridColumnsComponentRazor 
                    + DataGridColumnsComponentCode,

        FieldSetAll = FieldSetHandlerCode 
                    + FieldSetComponentRazor 
                    + FieldSetComponentCode
                    + FieldSetDetailComponentRazor 
                    + FieldSetDetailComponentCode,

        ComponentAll = MasterComponentRazor 
                     + MasterComponentCode
                     + DetailsComponentRazor
                     + DetailsComponentCode,
    }
}
//MdEnd