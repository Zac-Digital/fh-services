namespace FamilyHubs.SharedKernel.Razor.Links;

//todo: add autodoc

public interface IFhRenderLink
{
    string Text { get; set; }
    string? Url { get; set; }
    bool OpenInNewTab { get; set; }
    LinkStatus? Status { get; set; }
}