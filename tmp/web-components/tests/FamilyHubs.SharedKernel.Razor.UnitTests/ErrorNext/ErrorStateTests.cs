using System.Collections.Immutable;
using FamilyHubs.SharedKernel.Razor.ErrorNext;

namespace FamilyHubs.SharedKernel.Razor.UnitTests.ErrorNext
{
    public class ErrorStateTests
    {
        public enum ExampleErrors
        {
            Error1,
            Error2,
            Error3
        }

        public static readonly ImmutableDictionary<int, PossibleError> PossibleErrors =
            ImmutableDictionary.Create<int, PossibleError>()
                .Add(ExampleErrors.Error1, "Error 1 message")
                .Add(ExampleErrors.Error2, "Error 2 message")
                .Add(ExampleErrors.Error3, "Error 3 message");

        [Fact]
        public void Empty_ErrorState_Should_Have_No_Errors()
        {
            var errorState = ErrorState.Empty;

            Assert.False(errorState.HasErrors);
            Assert.Empty(errorState.Errors);
        }

        [Fact]
        public void ErrorState_With_Errors_Should_Have_Errors()
        {
            var errorState = ErrorState.Create(PossibleErrors, ExampleErrors.Error2);

            Assert.True(errorState.HasErrors);
            Assert.Single(errorState.Errors);
        }

        [Fact]
        public void ErrorState_Should_Trigger_Error_If_Id_Present()
        {
            var errorState = ErrorState.Create(PossibleErrors, ExampleErrors.Error2, ExampleErrors.Error3);

            Assert.True(errorState.HasTriggeredError((int)ExampleErrors.Error2));
            Assert.NotNull(errorState.GetErrorIfTriggered((int)ExampleErrors.Error2));
        }

        [Fact]
        public void ErrorState_Should_Not_Trigger_Error_If_Id_Not_Present()
        {
            var errorState = ErrorState.Create(PossibleErrors, ExampleErrors.Error2, ExampleErrors.Error3);

            Assert.False(errorState.HasTriggeredError((int)ExampleErrors.Error1));
            Assert.Null(errorState.GetErrorIfTriggered((int)ExampleErrors.Error1));
        }
    }
}