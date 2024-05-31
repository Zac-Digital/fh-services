using FamilyHubs.Idams.Maintenance.Core.Services;
using FamilyHubs.Idams.Maintenance.UI.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FamilyHubs.Idams.Maintenance.UI.Pages;

public class AddDfEAdminAccountModel : HeaderPageModel
{
    [Required]
    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254, MinimumLength = 3)] // the EmailAddress attribute allows emails that are too long, so we have this too
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string? PhoneNumber { get; set; }
    public record FormPropertyError(string Property, string ErrorMessage);
    public FormPropertyError[]? FormPropertyErrors { get; set; }

    public bool ValidationValid { get; private set; } = true;
    private readonly IIdamService _idamService;


    public AddDfEAdminAccountModel(IIdamService idamService)
    {
        _idamService = idamService;
    }

    public async Task<IActionResult> OnPost()
    {

        if (!string.IsNullOrWhiteSpace(PhoneNumber) && !Regex.IsMatch(PhoneNumber, @"^[A-Za-z0-9]*$"))
        {
            ModelState.AddModelError("PhoneNumber", "Please Enter a valid phone number");
            ValidationValid = false;
        }

        if (string.IsNullOrWhiteSpace(Email) || !IsValidEmailAddress(Email))
        {
            ModelState.AddModelError("Email", "Please Enter a valid email address");
            ValidationValid = false;
        }

        if (!ModelState.IsValid || !ValidationValid)
        {
            FormPropertyErrors = GetErrors(nameof(Name), nameof(Email), nameof(PhoneNumber)).ToArray();
            return Page();
        }

        var result = await  _idamService.AddNewDfeAccount(Name, Email.ToLower(), PhoneNumber);
        if (result != Email.ToLower())
        {
            ValidationValid = false;
            ModelState.AddModelError("Name", "Failed to create DfE Admin");
            return Page();
        }

        long accountId = await _idamService.GetAccountIdByEmail(result);

        return RedirectToPage($"AddDfEAccountConfirmation", new { accountId });
    }

    private IEnumerable<FormPropertyError> GetErrors(params string[] propertyNames)
    {
        return propertyNames.Select(p => (propertyName: p, entry: ModelState[p]))
            .Where(t => t.entry!.ValidationState == ModelValidationState.Invalid)
            .Select(t => new FormPropertyError(t.propertyName, t.entry!.Errors[0].ErrorMessage));
    }

    private static bool IsValidEmailAddress(string email)
    {
        try
        {
            MailAddress mailAddress = new(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
