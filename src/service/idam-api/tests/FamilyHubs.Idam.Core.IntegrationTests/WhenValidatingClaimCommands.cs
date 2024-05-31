using FamilyHubs.Idam.Core.Commands.Add;
using FamilyHubs.Idam.Core.Commands.Delete;
using FamilyHubs.Idam.Core.Commands.Update;
using FluentAssertions;

namespace FamilyHubs.Idam.Core.IntegrationTests;

public class WhenValidatingClaimCommands
{
    [Fact]
    public void ThenAddClaimCommandShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testClaim = TestDataProvider.GetSingleTestAccountClaim();
        var validator = new AddClaimCommandValidator();
        var testModel = new AddClaimCommand
        {
            AccountId  = testClaim.AccountId,
            Name = testClaim.Name,
            Value = testClaim.Value
        };

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenUpdateClaimCommandShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testClaim = TestDataProvider.GetSingleTestAccountClaim();
        var validator = new UpdateClaimCommandValidator();
        var testModel = new UpdateClaimCommand
        {
            AccountId  = testClaim.AccountId,
            Name = testClaim.Name,
            Value = testClaim.Value
        };

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenAddClaimCommandShouldNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testClaim = TestDataProvider.GetSingleTestAccountClaim();
        testClaim.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new AddClaimCommandValidator();
        var testModel = new AddClaimCommand
        {
            AccountId  = testClaim.AccountId,
            Name = testClaim.Name,
            Value = testClaim.Value
        };

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenUpdateClaimCommandShouldNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testClaim = TestDataProvider.GetSingleTestAccountClaim();
        testClaim.Id = Random.Shared.Next();
        testClaim.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new UpdateClaimCommandValidator();
        var testModel = new UpdateClaimCommand
        {
            AccountId  = testClaim.AccountId,
            Name = testClaim.Name,
            Value = testClaim.Value
        };

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenAddClaimCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testClaim = TestDataProvider.GetSingleTestAccountClaim();
        testClaim.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new AddClaimCommandValidator();
        var testModel = new AddClaimCommand
        {
            AccountId  = testClaim.AccountId,
            Name = testClaim.Name,
            Value = testClaim.Value
        };

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenUpdateClaimCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testClaim = TestDataProvider.GetSingleTestAccountClaim();
        testClaim.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new UpdateClaimCommandValidator();
        var testModel = new UpdateClaimCommand
        {
          AccountId  = testClaim.AccountId,
          Name = testClaim.Name,
          Value = testClaim.Value
        };

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenDeleteClaimByIdCommandShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new DeleteClaimCommandValidator();
        var testModel = new DeleteClaimCommand
        {
            AccountId = 1,
            Name = "test"
        };

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
