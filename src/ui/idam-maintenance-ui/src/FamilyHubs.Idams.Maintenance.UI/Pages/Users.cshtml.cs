using FamilyHubs.Idams.Maintenance.Core.ApiClient;
using FamilyHubs.Idams.Maintenance.Core.Services;
using FamilyHubs.Idams.Maintenance.Data.Entities;
using FamilyHubs.ReferralService.Shared.Models;
using FamilyHubs.SharedKernel.Razor.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace FamilyHubs.Idams.Maintenance.UI.Pages;

public class UsersModel : PageModel
{
    [BindProperty]
    public string? Search { get; set; }

    public IPagination Pagination { get; set; }

    public int TotalResults { get; set; }

    public PaginatedList<Account> Accounts { get; private set; } = new();

    [BindProperty]
    public int PageNum { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public bool IsLaUser { get; set; } = false;

    [BindProperty]
    public bool IsVcsUser { get; set; } = false;

    [BindProperty]
    public string SortBy { get; set; } = string.Empty;

    [BindProperty]
    public required string LaOrganisationName { get; set; } = string.Empty;

    public required List<string> LocalAuthorities { get; set; } = new List<string>();

    private readonly IIdamService _idamService;
    private readonly IServiceDirectoryClient _serviceDirectoryClient;

    public UsersModel(IIdamService idamService, IServiceDirectoryClient serviceDirectoryClient)
    {
        _idamService = idamService;
        _serviceDirectoryClient = serviceDirectoryClient;
        Pagination = new DontShowPagination();
    }

    public async Task OnGet(int? pageNum, string? name, string? email, string? organisation, bool? isLa, bool? isVcs, string? sortBy)
    {
        LaOrganisationName = organisation ?? string.Empty;
        ResolveQueryParameters(pageNum, name, email, organisation, isLa, isVcs, sortBy);
        PageNum = pageNum ?? 1;
        var organisations = await _serviceDirectoryClient.GetOrganisations();
        LocalAuthorities = organisations.Select(x => x.Name).ToList();
        long? orgId = organisations.FirstOrDefault(org => org.Name.Equals(organisation))?.Id;
        Accounts = await _idamService.GetAccounts(Name, Email, orgId,  IsLaUser, IsVcsUser, SortBy, PageNum, PageSize);
        Pagination = new LargeSetPagination(Accounts.TotalPages, PageNum);
    }

    public IActionResult OnPost()
    {
        var query = CreateQueryParameters();
        return RedirectToPage(query);
    }

    public IActionResult OnClearFilters()
    {
        return RedirectToPage("Users");
    }

    private object CreateQueryParameters()
    {

        var routeValues = new Dictionary<string, object>();

        routeValues.Add("pageNum", PageNum);
        if (Name != null) routeValues.Add("name", Name);
        if (Email != null) routeValues.Add("email", Email);
        if (LaOrganisationName != null) routeValues.Add("organisation", LaOrganisationName);
        routeValues.Add("isLa", IsLaUser);
        routeValues.Add("isVcs", IsVcsUser);
        routeValues.Add("sortBy", SortBy);

        return routeValues;
    }

    private void ResolveQueryParameters(int? pageNumber, string? name, string? email, string? organisation, bool? isLaUser, bool? isVcsUser, string? sortBy)
    {
        if (pageNumber != null) PageNum = pageNumber.Value;
        if (name != null) Name = HttpUtility.UrlEncode(name);
        if (email != null) Email = HttpUtility.UrlEncode(email);
        if (organisation != null) LaOrganisationName = organisation;
        if (isLaUser != null) IsLaUser = isLaUser.Value;
        if (isVcsUser != null) IsVcsUser = isVcsUser.Value;
        if (sortBy != null) SortBy = sortBy;
    }

    public static string OrganisationName(Account account)
    {
        var organisationName = account?.Claims?.FirstOrDefault(x => x.Name == "OrganisationName")?.Value;
        return organisationName ?? string.Empty;
    }
}
