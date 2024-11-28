using FamilyHubs.ServiceDirectory.Admin.Core.ApiClient;
using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Services;
using FamilyHubs.ServiceDirectory.Admin.Web.Errors;
using FamilyHubs.ServiceDirectory.Admin.Web.ViewModel;
using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.Header;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.ServiceDirectory.Admin.Web.Areas.VcsAdmin.Pages
{
    public class AddOrganisationModel : InputPageViewModel, IHasErrorStatePageModel
    {
        private readonly ICacheService _cacheService;
        private readonly IServiceDirectoryClient _serviceDirectoryClient;

        [BindProperty]
        public string OrganisationName { get; set; } = string.Empty;

        public AddOrganisationModel(ICacheService cacheService, IServiceDirectoryClient serviceDirectoryClient)
        {
            _cacheService = cacheService;
            _serviceDirectoryClient = serviceDirectoryClient;
            
            PageHeading = "What is the organisation's name?";
            ErrorMessage = "Enter the organisation's name";
            ErrorElementId = "organisationName";
            Errors = ErrorState.Empty;
        }

        public IErrorState Errors { get; private set; }

        public async Task OnGet(bool changeName = false, string cacheId="")
        {            
            if (changeName)
            {
                OrganisationName = await _cacheService.RetrieveString(CacheKeyNames.AddOrganisationName);
            }
            
            var flow = await _cacheService.RetrieveUserFlow();
            var isLaManager = HttpContext.IsUserLaManager();            
            if (flow == "AddPermissions") {
                BackButtonPath = $"/AccountAdmin/WhichVcsOrganisation/{cacheId}";
            }else if (isLaManager)
            {
                BackButtonPath = "/Welcome";
            } 
            else
            {
                BackButtonPath = "/VcsAdmin/AddOrganisationWhichLocalAuthority";
            }
        }

        public async Task<IActionResult> OnPost(string cacheId = "")
        {
            SetBackButtonPath();

            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(OrganisationName) && OrganisationName.Length <= 255)
            {
                var laOrganisationId = await _cacheService.RetrieveString(CacheKeyNames.LaOrganisationId);
                var existingOrganisations = await _serviceDirectoryClient.GetCachedVcsOrganisations(long.Parse(laOrganisationId));
                if(existingOrganisations.Exists(x => x.Name == OrganisationName)) 
                {
                    await _cacheService.StoreCurrentPageName($"/VcsAdmin/AddOrganisation");
                    return RedirectToPage("AddOrganisationAlreadyExists");
                }

                await _cacheService.StoreString(CacheKeyNames.AddOrganisationName, OrganisationName);
                return RedirectToPage("/AddOrganisationCheckDetails", new { cacheId = cacheId });
            }

            HasValidationError = true;

            Errors = ErrorState.Create(PossibleErrors.All, ErrorId.Add_Organisation);

            return Page();
        }
    }
}
