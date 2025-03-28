﻿using FamilyHubs.ServiceDirectory.Admin.Core.ApiClient;
using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Services;
using FamilyHubs.ServiceDirectory.Admin.Web.Errors;
using FamilyHubs.ServiceDirectory.Admin.Web.ViewModel;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.ServiceDirectory.Admin.Web.Areas.AccountAdmin.Pages;

public class WhichVcsOrganisation : AccountAdminViewModel
{
    private readonly IServiceDirectoryClient _serviceDirectoryClient;

    public WhichVcsOrganisation(ICacheService cacheService, IServiceDirectoryClient serviceDirectoryClient) : base(nameof(WhichVcsOrganisation), cacheService)
    {
        PageHeading = "Which organisation do they work for?";
        _serviceDirectoryClient = serviceDirectoryClient;
        Errors = ErrorState.Empty;
    }

    [BindProperty]
    public required string VcsOrganisationName { get; set; } = string.Empty;

    public required List<string> VcsOrganisations { get; set; } = new List<string>();

    public override async Task OnGet()
    {
        await base.OnGet();
        
        var vcsOrganisations = await _serviceDirectoryClient.GetCachedVcsOrganisations(PermissionModel.LaOrganisationId);
        VcsOrganisations = vcsOrganisations.Select(l => l.Name).ToList();

        VcsOrganisationName = PermissionModel.VcsOrganisationName;
    }

    public override async Task<IActionResult> OnPost()
    {
        await base.OnPost();

        var vcsOrganisations = await _serviceDirectoryClient.GetCachedVcsOrganisations(PermissionModel.LaOrganisationId);
        var vcsOrganisationsNames = vcsOrganisations.Select(l => l.Name).ToList();

        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(VcsOrganisationName) && VcsOrganisationName.Length <= 255 
            && vcsOrganisationsNames.Contains(VcsOrganisationName))
        {
            PermissionModel.VcsOrganisationId = vcsOrganisations.Single(l => l.Name == VcsOrganisationName).Id;
            PermissionModel.VcsOrganisationName = VcsOrganisationName;

            await CacheService.StorePermissionModel(PermissionModel, CacheId);

            return RedirectToPage(NextPageLink, new {cacheId= CacheId});
        }

        Errors = ErrorState.Create(PossibleErrors.All, ErrorId.AccountAdmin_WhichVcsOrganisation_MissingSelection);
        
        VcsOrganisations = vcsOrganisations.Select(l => l.Name).ToList();
        
        return Page();
    }

    public async Task<IActionResult> OnGetAddOrganisation()
    {
        var permissionModel = await CacheService.GetPermissionModel(CacheId);
        var laOrganisations = await _serviceDirectoryClient.GetCachedLaOrganisations();   
        var laOrganisation  = laOrganisations.First(x => x.Id == permissionModel?.LaOrganisationId);

        await CacheService.StoreString(CacheKeyNames.AdminAreaCode, laOrganisation.AdminAreaCode);
        await CacheService.StoreString(CacheKeyNames.LaOrganisationId, laOrganisation.Id.ToString());

        return RedirectToPage("/AddOrganisation", new { area = "VcsAdmin", cacheId = CacheId });
    }
}