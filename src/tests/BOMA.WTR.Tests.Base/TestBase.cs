using AutoFixture;

namespace BOMA.WTR.Tests.Base;

public abstract class TestBase
{
    private readonly IFixture _fixture = new Fixture();
    
    public static IEnumerable<object[]> StringNullOrWhiteSpaceData =>
        new[] { "", " ", "  ", null }.ToMemberData();

    public static IEnumerable<object[]> StringEmptyOrWhiteSpaceData =>
        new[] { "", " ", "  " }.ToMemberData();

    public static IEnumerable<object[]> DecimalNegativeData =>
        new[] { -100m, -10m, -1m, -0.1m, -0.01m }.ToMemberData();

    public static IEnumerable<object[]> IntNegativeData =>
        new[] { -100, -10, -1 }.ToMemberData();

    public static IEnumerable<object[]> IntNegativeAndZeroData =>
        new[] { -100, -10, -1, 0 }.ToMemberData();

    public static IEnumerable<object[]> IntNullOrNegativeData =>
        new[] { (int?)null, -100, -10, -1 }.ToMemberData();

    public static IEnumerable<object[]> DecimalPositiveData =>
        new[] { 100m, 10m, 1m, 0.1m, 0.01m }.ToMemberData();

    public static IEnumerable<object[]> DecimalPositiveAndZeroData =>
        new[] { 100m, 10m, 1m, 0.1m, 0.01m, 0m }.ToMemberData();

    public static IEnumerable<object[]> BudgetCostInvalidData =>
        new decimal?[] { -1m, -0.1m, -10m, null, 0.001m, 0.123m }
            .ToMemberData();

    public static IEnumerable<object[]> BoolNullableInvalidData =>
        new[] { (bool?)null, false }
            .ToMemberData();

    public static IEnumerable<object[]> DecimalNullOrNegativeData =>
        new[] { (decimal?)null, -100m, -10m, -1m, -0.1m, -0.01m }
            .ToMemberData();
    
    protected T Create<T>() =>
        _fixture.Create<T>();

    protected string CreateString() =>
        Create<string>();

    protected DateTime CreateDateTime() =>
        Create<DateTime>();

    protected int CreateInt() =>
        Create<int>();

    protected int CreateSmallInt() =>
        CreateInt(1, 10);

    protected int CreateInt(int min, int max) =>
        _fixture.CreateInt(min, max);

    protected decimal CreateDecimal() =>
        Create<decimal>();

    protected Guid CreateGuid() =>
        Create<Guid>();

    protected T CreateEnumExceptGiven<T>(IEnumerable<T> exceptValues) where T : Enum =>
        CreateEnumExceptGiven(exceptValues.ToArray());

    protected T CreateEnumExceptGiven<T>(params T[] exceptValues) where T : Enum =>
        _fixture.GetEnumValue(exceptValues);

    protected static IEnumerable<object[]> GetEnumValuesExceptGiven<T>(params T[] exceptValues) where T : Enum =>
        TestExtensions.GetEnumValues(exceptValues);

    protected static IEnumerable<object[]> GetEnumValuesExceptGiven<T>(IEnumerable<T> exceptValues) where T : Enum =>
        TestExtensions.GetEnumValues(exceptValues.ToArray());

    protected static IEnumerable<object[]> GetEnumValuesExceptGiven<T>(IEnumerable<T> exceptValues,
        params T[] additionalExceptValues) where T : Enum =>
        TestExtensions.GetEnumValues(exceptValues.Concat(additionalExceptValues).ToArray());

    protected static IEnumerable<object[]> GetEnumValues<T>() where T : Enum =>
        TestExtensions.GetEnumValues<T>();
}