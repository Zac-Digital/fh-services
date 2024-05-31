using FamilyHubs.ReferralService.Shared.Models;

namespace FamilyHubs.ReferralService.Shared.Tests;

public class MessageDtoComparerTests : PlainComparerTestBase<MessageDto, string>
{
    public MessageDtoComparerTests() : base(new MessageDto
    {
        RecipientEmail = "New",
        TemplateId = "3f23e8ee-7692-4716-aaab-770b69965977",
        TemplateTokens = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        }

    }, new MessageDto
    {
        RecipientEmail = "New",
        TemplateId = "3f23e8ee-7692-4716-aaab-770b69965977",
        TemplateTokens = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        }


    }, dto => dto.RecipientEmail)
    {

    }
}

