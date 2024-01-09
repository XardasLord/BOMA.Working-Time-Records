using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Domain.Tests.Unit.AggregateModels;

public class DeactivateTests : TestBase
{
    private readonly Employee _employee;

    public DeactivateTests()
    {
        _employee = Create<Employee>();
        _employee._isActive = true;
    }
    
    private void Act() => _employee.Deactivate();

    [Fact]
    public void deactivate_deactivates_employee()
    {
        // Act
        Act();
        
        // Assert
        _employee.IsActive.Should().BeFalse();
        _employee.RcpId.Should().Be(999);
    }
    
    [Fact]
    public void deactivate_already_deactivated_employee_throws_exception()
    {
        // Arrange
        _employee._isActive = false;
        
        // Act
        var result = Record.Exception(Act);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<InvalidOperationException>();
    }
}