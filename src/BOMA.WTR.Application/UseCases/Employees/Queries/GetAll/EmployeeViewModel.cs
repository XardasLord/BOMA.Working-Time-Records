namespace BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;

public class EmployeeViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal SalaryBonusPercentage { get; set; }
    public int RcpId { get; set; }
    public string DepartmentName { get; set; }
    public int DepartmentId { get; set; }
    public int? ShiftTypeId { get; set; }
    public string ShiftTypeName { get; set; }
    public string Position { get; set; }
}