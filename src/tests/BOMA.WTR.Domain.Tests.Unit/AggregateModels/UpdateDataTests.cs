using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Domain.Tests.Unit.AggregateModels;

public class UpdateDataTests : TestBase
{
    private readonly Employee _employee;
    private Name _name;
    private Money _salary;
    private PercentageBonus _percentageBonus;
    private JobInformation _jobInformation;
    private PersonalIdentityNumber _personalIdentityNumber;
    private int _rcpId;
    private int _departmentId;

    public UpdateDataTests()
    {
        _employee = Create<Employee>();
    }
    
    private void Act() => _employee.UpdateData(_name, _salary, _percentageBonus, _jobInformation, _personalIdentityNumber, _rcpId, _departmentId);

    [Fact]
    public void update_data_sets_new_values()
    {
        // Arrange
        _name = Create<Name>();
        _salary = Create<Money>();
        _percentageBonus = Create<PercentageBonus>();
        _jobInformation = Create<JobInformation>();
        _personalIdentityNumber = Create<PersonalIdentityNumber>();
        _rcpId = CreateInt();
        _departmentId = CreateInt();
        
        // Act
        Act();
        
        // Assert
        _employee.Name.Should().Be(_name);
        _employee.Salary.Should().Be(_salary);
        _employee.SalaryBonusPercentage.Should().Be(_percentageBonus);
        _employee.JobInformation.Should().Be(_jobInformation);
        _employee.PersonalIdentityNumber.Should().Be(_personalIdentityNumber);
        _employee.RcpId.Should().Be(_rcpId);
        _employee.DepartmentId.Should().Be(_departmentId);
    }
}