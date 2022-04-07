//@BaseCode
//MdStart
namespace SmartNQuick.BlazorServerApp.Models.Modules.Common
{
    [System.Flags]
    public enum VisibilityMode : byte
    {
        Hidden = 0,

        ListView = 0b00000001,
        DetailView = 0b00000010,
        ListDetailView = ListView + DetailView,
        CreateView = 0b00000100,
        ListCreateView = ListView + CreateView,
        DetailCreateView = DetailView + CreateView,
        ListDetailCreateView = ListView + DetailView + CreateView,
        UpdateView = 0b00001000,
        ListUpdateView = ListView + UpdateView,
        DetailUpdateView = DetailView + UpdateView,
        ListDetailUpdateView = ListView + DetailView + UpdateView,
        CreateUpdateView = CreateView + UpdateView,
        ListCreateUpdateView = ListView + CreateView + UpdateView,
        DetailCreateUpdateView = DetailView + CreateView + UpdateView,
        ListDetailCreateUpdateView = ListView + DetailView + CreateView + UpdateView,
        DeleteView = 0b00010000,
        ListDeleteView = ListView + DeleteView,
        DetailDeleteView = DetailView + DeleteView,
        ListDetailDeleteView = ListView + DetailView + DeleteView,
        CreateDeleteView = CreateView + DeleteView,
        ListCreateDeleteView = ListView + CreateView + DeleteView,
        DetailCreateDeleteView = DetailView + CreateView + DeleteView,
        ListDetailCreateDeleteView = ListView + DetailView + CreateView + DeleteView,
        UpdateDeleteView = UpdateView + DeleteView,
        ListUpdateDeleteView = ListView + UpdateView + DeleteView,
        DetailUpdateDeleteView = DetailView + UpdateView + DeleteView,
        ListDetailUpdateDeleteView = ListView + DetailView + UpdateView + DeleteView,
        CreateUpdateDeleteView = CreateView + UpdateView + DeleteView,
        ListCreateUpdateDeleteView = ListView + CreateView + UpdateView + DeleteView,
        DetailCreateUpdateDeleteView = DetailView + CreateView + UpdateView + DeleteView,
        ListDetailCreateUpdateDeleteView = ListView + DetailView + CreateView + UpdateView + DeleteView,

        Visible = ListDetailCreateUpdateDeleteView,
    }
}
//MdEnd