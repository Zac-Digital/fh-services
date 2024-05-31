using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Extensions;

public static class ViewDataDictionaryExtensions
{
    public static void SetFamilyHubsLayoutModel(
        this ViewDataDictionary viewData,
        FamilyHubsLayoutModel familyHubsLayoutModel)
    {
        viewData["FamilyHubsLayoutModel"] = familyHubsLayoutModel;
    }

    public static FamilyHubsLayoutModel GetFamilyHubsLayoutModel(this ViewDataDictionary viewData)
    {
        return viewData["FamilyHubsLayoutModel"] as FamilyHubsLayoutModel
            ?? throw new InvalidOperationException("FamilyHubsLayoutModel is not set in ViewDataDictionary");
    }

    public static FamilyHubsUiOptions GetFamilyHubsUiOptions(this ViewDataDictionary viewData)
    {
        return viewData.GetFamilyHubsLayoutModel().FamilyHubsUiOptions.Value;
    }
}