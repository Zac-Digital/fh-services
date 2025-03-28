using System.ComponentModel.DataAnnotations;
using FamilyHubs.Referral.Core.DistributedCache;
using FamilyHubs.Referral.Core.Models;
using FamilyHubs.Referral.Core.ValidationAttributes;
using FamilyHubs.Referral.Web.Pages.Shared;
using FamilyHubs.SharedKernel.Razor.FeatureFlags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace FamilyHubs.Referral.Web.Pages.ProfessionalReferral;

[FeatureGate(FeatureFlag.ConnectDashboard)]
public class ContactByPhoneModel : ProfessionalReferralCacheModel
{
    [BindProperty]
    public string? TelephoneNumber { get; set; }

    [BindProperty]
    public ReferrerContactType? Contact { get; set; }

    public ContactByPhoneModel(IConnectionRequestDistributedCache connectionRequestDistributedCache)
        : base(ConnectJourneyPage.ContactByPhone, connectionRequestDistributedCache)
    {
    }

    protected override void OnGetWithModel(ConnectionRequestModel model)
    {
        if (!Errors.HasErrors && model.ReferrerContact is not null)
        {
            Contact = model.ReferrerContact;
            TelephoneNumber = model.ReferrerTelephone;
        }
    }

    protected override IActionResult OnPostWithModel(ConnectionRequestModel model)
    {
        ErrorId? error = null;
        if (Contact == null)
        {
            error = ErrorId.ContactByPhone_NoContactSelected;
        }
        else if (Contact == ReferrerContactType.TelephoneAndEmail)
        {
            if (string.IsNullOrEmpty(TelephoneNumber))
            {
                error = ErrorId.ContactByPhone_NoTelephoneNumber;
            }
            else if (UkGdsTelephoneNumberAttribute.IsValid(TelephoneNumber) != ValidationResult.Success)
            {
                error = ErrorId.ContactByPhone_InvalidTelephoneNumber;
            }
        }

        if (error != null)
        {
            return RedirectToSelf(null, error.Value);
        }

        model.ReferrerContact = Contact;
        model.ReferrerTelephone = Contact == ReferrerContactType.TelephoneAndEmail ? TelephoneNumber : null;

        return NextPage();
    }
}