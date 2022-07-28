using BOMA.WTR.Domain.Extensions;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Domain.Tests.Unit.Extensions.DateTimeExtensions;

public class WorkingDaysInMonthTests : TestBase
{
    private DateTime _date;

    private int Act() => _date.WorkingDaysInMonthExcludingBankHolidays();

    [Theory]
    [MemberData(nameof(Test2022Data))]
    [MemberData(nameof(Test2023Data))]
    [MemberData(nameof(Test2024Data))]
    public void GivenDateTime_Returns_NumberOfWorkingDaysExcludingBankHolidays(DateTime date, int expectedNumberOfWorkingDays)
    {
        // Arrange
        _date = date;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(expectedNumberOfWorkingDays);
    }

    public static IEnumerable<object[]> Test2022Data()
    {
        yield return new object[] { new DateTime(2022, 6, 1), 21 };
        yield return new object[] { new DateTime(2022, 7, 1), 21 };
        yield return new object[] { new DateTime(2022, 8, 1), 22 };
        yield return new object[] { new DateTime(2022, 9, 1), 22 };
        yield return new object[] { new DateTime(2022, 10, 1), 21 };
        yield return new object[] { new DateTime(2022, 11, 1), 20 };
        yield return new object[] { new DateTime(2022, 12, 1), 21 };
    }

    public static IEnumerable<object[]> Test2023Data()
    {
        yield return new object[] { new DateTime(2023, 1, 1), 21 };
        yield return new object[] { new DateTime(2023, 2, 1), 20 };
        yield return new object[] { new DateTime(2023, 3, 1), 23 };
        yield return new object[] { new DateTime(2023, 4, 1), 19 };
        yield return new object[] { new DateTime(2023, 5, 1), 21 };
        yield return new object[] { new DateTime(2023, 6, 1), 21 };
        yield return new object[] { new DateTime(2023, 7, 1), 21 };
        yield return new object[] { new DateTime(2023, 8, 1), 22 };
        yield return new object[] { new DateTime(2023, 9, 1), 21 };
        yield return new object[] { new DateTime(2023, 10, 1), 22 };
        yield return new object[] { new DateTime(2023, 11, 1), 20 };
        yield return new object[] { new DateTime(2023, 12, 1), 19 };
    }

    public static IEnumerable<object[]> Test2024Data()
    {
        yield return new object[] { new DateTime(2024, 1, 1), 21 };
        yield return new object[] { new DateTime(2024, 2, 1), 21 };
        yield return new object[] { new DateTime(2024, 3, 1), 21 };
        yield return new object[] { new DateTime(2024, 4, 1), 21 };
        yield return new object[] { new DateTime(2024, 5, 1), 20 };
        yield return new object[] { new DateTime(2024, 6, 1), 20 };
        yield return new object[] { new DateTime(2024, 7, 1), 23 };
        yield return new object[] { new DateTime(2024, 8, 1), 21 };
        yield return new object[] { new DateTime(2024, 9, 1), 21 };
        yield return new object[] { new DateTime(2024, 10, 1), 23 };
        yield return new object[] { new DateTime(2024, 11, 1), 19 };
        yield return new object[] { new DateTime(2024, 12, 1), 20 };
    }
}