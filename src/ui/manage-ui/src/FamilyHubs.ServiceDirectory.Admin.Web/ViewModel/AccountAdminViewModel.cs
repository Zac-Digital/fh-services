﻿using FamilyHubs.ServiceDirectory.Admin.Core.Models;
using FamilyHubs.ServiceDirectory.Admin.Core.Services;
using FamilyHubs.ServiceDirectory.Admin.Web.Pages.Shared;
using FamilyHubs.SharedKernel.Identity;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.Header;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.ServiceDirectory.Admin.Web.ViewModel;

public class AccountAdminViewModel : HeaderPageModel, IHasErrorStatePageModel
{
    public ICacheService CacheService { get; set; }
    public IErrorState Errors { get; protected set; }

    public string PageHeading { get; set; } = string.Empty;
    
    public string LaRoleTypeLabel { get; set; } = "Someone who works for a local authority";

    public string VcsRoleTypeLabel { get; set; } = "Someone who works for a voluntary and community sector organisation";
    
    public string PreviousPageLink { get; set; } = string.Empty;
    public string CurrentPageName { get; set; }
    public string NextPageLink { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string CacheId { get; set; } = string.Empty;

    [FromQuery(Name = "backToCheckDetails")]
    public bool BackToCheckDetails { get; set; }

    public PermissionModel PermissionModel { get; set; } = new();

    public AccountAdminViewModel(string currentPageName, ICacheService cacheService)
    {
        ArgumentException.ThrowIfNullOrEmpty(currentPageName);
        
        CacheService = cacheService;
        CurrentPageName = currentPageName;
        Errors = ErrorState.Empty;
    }

    public virtual async Task OnGet()
    {
        if (CurrentPageName == "TypeOfRole")
        {
            SetNavigationLinks(string.Empty, false);
            return;
        }
        
        var permissionModel = await CacheService.GetPermissionModel(CacheId);
        ArgumentNullException.ThrowIfNull(permissionModel);
        PermissionModel = permissionModel;

        SetNavigationLinks(permissionModel.OrganisationType, permissionModel.VcsJourney);
    }

    public virtual async Task<IActionResult> OnPost()
    {
        if (CurrentPageName == "TypeOfRole")
        {
            SetNavigationLinks(string.Empty, false);
            return Page();
        }
        
        var permissionModel = await CacheService.GetPermissionModel(CacheId);
        ArgumentNullException.ThrowIfNull(permissionModel);
        PermissionModel = permissionModel;
        
        SetNavigationLinks(permissionModel.OrganisationType, permissionModel.VcsJourney);

        return Page();
    }
    
    public void SetNavigationLinks(string? organisationType, bool isVcsJourney)
    {
        var isUserLaManager = HttpContext.IsUserLaManager();
        
        switch (CurrentPageName)
        {
            case "TypeOfRole" :
            {
                PreviousPageLink = "/Welcome";
                if (string.IsNullOrWhiteSpace(organisationType))
                {
                    NextPageLink = string.Empty;
                }
                else
                {
                    NextPageLink = organisationType == "LA" ? "/TypeOfUserLa" : "/TypeOfUserVcs";
                }
                break;
            }
            case "TypeOfUserLa" : {
                PreviousPageLink = "/TypeOfRole";
                NextPageLink = isUserLaManager ? "/UserEmail" : "/WhichLocalAuthority";
                break;
            }
            case "TypeOfUserVcs" : {
                PreviousPageLink = "/TypeOfRole";
                NextPageLink = isUserLaManager ? "/WhichVcsOrganisation" : "/WhichLocalAuthority";
                break;
            }
            case "WhichLocalAuthority" : {
                PreviousPageLink = isVcsJourney ? "/TypeOfUserVcs" : "/TypeOfUserLa";
                NextPageLink = isVcsJourney ? "/WhichVcsOrganisation" : "/UserEmail";
                break;
            }
            case "WhichVcsOrganisation" : {
                PreviousPageLink = isUserLaManager ? "/TypeOfUserVcs" : "/WhichLocalAuthority";
                NextPageLink = "/UserEmail";
                break;
            }
            case "UserEmail" : {
                if (isVcsJourney)
                {
                    PreviousPageLink = "/WhichVcsOrganisation";
                }
                else
                {
                    PreviousPageLink = isUserLaManager ? "/TypeOfUserLa" : "/WhichLocalAuthority";
                }
                NextPageLink = "/UserName";
                break;
            }
            case "UserName" : {
                PreviousPageLink = "/UserEmail";
                NextPageLink = "/AddPermissionCheckAnswer";
                break;
            }
            case "AddPermissionCheckAnswer" : {
                PreviousPageLink = "/UserName";
                NextPageLink = "/Confirmation";
                break;
            }
        }

        if (BackToCheckDetails)
        {
            PreviousPageLink = "/AddPermissionCheckAnswer";
        }
    }

    protected void SetRoleTypeLabelsForCurrentUser(string organisationName)
    {
        if (!HttpContext.IsUserLaManager())
        {
            return;
        }
        LaRoleTypeLabel = $"Someone who works for {organisationName}";
        VcsRoleTypeLabel = $"Someone who works for a voluntary and community sector organisation {organisationName}";
    }
}