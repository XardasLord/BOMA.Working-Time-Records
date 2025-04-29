using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Domain.Tests.Unit.AggregateModels.Specifications;

public class EmployeeWithCurrentAndFullHistoricalEntriesByRcpIdSpecTests : TestBase
{
    private EmployeeWithCurrentAndFullHistoricalEntriesByRcpIdSpec _spec;
    private const int TestRcpId = 300;
    private const int TestId = 3;

    private Employee? Act() => _spec.Evaluate(GetTestListOfEmployees()).FirstOrDefault();

    [Fact]
    public void Returns_aggregate_with_given_rcp_id()
    {
        // Arrange
        _spec = new EmployeeWithCurrentAndFullHistoricalEntriesByRcpIdSpec(TestRcpId);
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().NotBeNull();
        result?.RcpId.Should().Be(TestRcpId);
        result?.Id.Should().Be(TestId);
        _spec.IncludeExpressions.Should().NotBeEmpty();
    }

    private static IEnumerable<Employee> GetTestListOfEmployees() 
        => new List<Employee>() 
        {
            new() { Id = 1, _rcpId = 100 },
            new() { Id = 2, _rcpId = 200 },
            new() { Id = TestId, _rcpId = TestRcpId},
            new() { Id = 4, _rcpId = 400 }
        };
}