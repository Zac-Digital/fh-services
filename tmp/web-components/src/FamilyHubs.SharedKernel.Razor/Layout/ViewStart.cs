using FamilyHubs.SharedKernel.Razor.AlternativeServices;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FamilyHubs.SharedKernel.Razor.Layout;

public static class ViewStart
{
    public static void InitialiseFamilyHubs(FamilyHubsLayoutModel familyHubsLayoutModel, PageModel pageModel, ViewDataDictionary<PageModel> viewData)
    {
        familyHubsLayoutModel.PageModel = pageModel;
        var alt = familyHubsLayoutModel.PageModel as IAlternativeService;
        if (alt?.ServiceName != null)
        {
            var altFamilyHubsUiOptions = familyHubsLayoutModel.FamilyHubsUiOptions.Value.AlternativeFamilyHubsUi[alt.ServiceName];
            if (altFamilyHubsUiOptions.Enabled)
            {
                familyHubsLayoutModel.FamilyHubsUiOptions = Options.Create(altFamilyHubsUiOptions);
            }
        }
        viewData.SetFamilyHubsLayoutModel(familyHubsLayoutModel);
    }
}