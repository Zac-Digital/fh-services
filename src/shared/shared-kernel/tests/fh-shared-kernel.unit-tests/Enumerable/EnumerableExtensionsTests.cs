using System.ComponentModel;
using FamilyHubs.SharedKernel.Enumerable;

namespace FamilyHubs.SharedKernel.UnitTests.Enumerable
{
    public class EnumerableExtensionsTests
    {
        private enum TestEnum
        {
            [Description("Value One")]
            Value1,
            [Description("value two")]
            Value2,
            [Description("Value three")]
            Value3
        }

        [Fact]
        public void ToDisplay_ReturnsExpectedDisplay()
        {
            // Arrange
            var enumValues = new List<TestEnum>
            {
                TestEnum.Value1,
                TestEnum.Value2,
                TestEnum.Value3
            };
            // Act
            var display = enumValues.ToDisplay();
            // Assert
            Assert.Equal("Value one, value three and value two", display);
        }
    }
}
