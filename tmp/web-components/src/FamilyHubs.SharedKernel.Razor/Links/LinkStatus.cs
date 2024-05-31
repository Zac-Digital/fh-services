namespace FamilyHubs.SharedKernel.Razor.Links;

public enum LinkStatus
{
    Visible,
    NotVisible, // NotRendered? (we might want a Hidden also, for rendered, but html hidden). could also have VisibleIfJavascriptEnabled
    Active
}