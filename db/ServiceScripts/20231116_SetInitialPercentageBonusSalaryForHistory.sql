UPDATE w
SET w.SalaryInformation_PercentageBonusSalary = e.PercentageBonusSalary
FROM WorkingTimeRecordAggregatedHistories w
INNER JOIN Employees e ON w.UserId = e.Id